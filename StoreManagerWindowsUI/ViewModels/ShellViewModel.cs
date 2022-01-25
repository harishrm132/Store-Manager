 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using StoreManagerWindowsUI.EventModels;
using StoreManagerWindowsUI.Library.Models;

namespace StoreManagerWindowsUI.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<LogOnEvent>
    {
        private SalesViewModel _salesVM;
        private IEventAggregator _events;
        private ILoggedInUserModel _user;

        public ShellViewModel(IEventAggregator events, SalesViewModel salesVM, ILoggedInUserModel user)
        {
            _events = events;
            _salesVM = salesVM;
            _user = user;

            //Subscribe to Events
            _events.Subscribe(this);
            ActivateItem(IoC.Get<LoginViewModel>());
        }

        public bool IsLoggedIn
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_user.Token))
                {
                    return true;
                }
                return false;
            }
        }

        public void ExitApplication()
        {
            TryClose();
        }

        public void LogOut()
        {
            _user.LogOff();
            ActivateItem(IoC.Get<LoginViewModel>());
            NotifyOfPropertyChange(() => IsLoggedIn);
        }

        public void Handle(LogOnEvent message)
        {
            ActivateItem(_salesVM);
            NotifyOfPropertyChange(() => IsLoggedIn);
        }
    }
}
