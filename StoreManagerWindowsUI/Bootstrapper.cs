using AutoMapper;
using Caliburn.Micro;
using StoreManagerWindowsUI.Helpers;
using StoreManagerWindowsUI.Library.Api;
using StoreManagerWindowsUI.Library.Helpers;
using StoreManagerWindowsUI.Library.Models;
using StoreManagerWindowsUI.Models;
using StoreManagerWindowsUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace StoreManagerWindowsUI
{
    public class Bootstrapper : BootstrapperBase
    {

        private SimpleContainer _container = new SimpleContainer();

        public Bootstrapper()
        {
            Initialize();

            ConventionManager.AddElementConvention<PasswordBox>(
                PasswordBoxHelper.BoundPasswordProperty,
                "Password",
                "PasswordChanged");
        }

        private IMapper ConfigureAutoMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProductModel, ProductDisplayModel>();
                cfg.CreateMap<CartModel, CartDisplayModel>();
            });
            var mapper = config.CreateMapper();
            return mapper;
        }

        protected override void Configure()
        {
           
            _container.Instance(ConfigureAutoMapper());

            _container.Instance(_container)
                .PerRequest<IProductEndPoint, ProductEndPoint>()
                .PerRequest<ISaleEndPoint, SaleEndPoint>()
                .PerRequest<IUserEndPoint, UserEndPoint>();

            //Use Dependency Injection using simple container of Caliburn Micro
            _container
                .Singleton<IWindowManager, WindowManager>()
                .Singleton<IEventAggregator, EventAggregator>()
                .Singleton<ILoggedInUserModel, LoggedInUserModel>()
                .Singleton<IAPIHelper, APIHelper>()
                .Singleton<IConfigHelper, ConfigHelper>(); 

            //wire viewmodel that connect to views - reflection
            GetType().Assembly.GetTypes()
                .Where(type => type.IsClass)
                .Where(type => type.Name.EndsWith("ViewModel"))
                .ToList()
                .ForEach(vmType => _container.RegisterPerRequest(vmType, vmType.ToString(), vmType));
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();
        }

        protected override object GetInstance(Type service, string key)
        {
            return _container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }
    }
}
