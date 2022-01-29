 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;
using StoreManagerWindowsUI.EventModels;
using StoreManagerWindowsUI.Library.Helpers;
using StoreManagerWindowsUI.Library.Models;

namespace StoreManagerWindowsUI.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<LogOnEvent>
    {
        private IEventAggregator _events;
        private ILoggedInUserModel _user;
        private IAPIHelper _apiHelper;

        public ShellViewModel(IEventAggregator events, ILoggedInUserModel user, IAPIHelper apiHelper)
        {
            _events = events;
            _user = user;
            _apiHelper = apiHelper;

            //Subscribe to Events
            _events.SubscribeOnPublishedThread(this);
            ActivateItemAsync(IoC.Get<LoginViewModel>(), new CancellationToken());
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
            TryCloseAsync();
        }

        public async Task UserManagement()
        {
            await ActivateItemAsync(IoC.Get<UserDisplayViewModel>(), new CancellationToken());
        }

        public async Task LogOut()
        {
            _user.ResetUser();
            _apiHelper.LogOffUser();
            await ActivateItemAsync(IoC.Get<LoginViewModel>(), new CancellationToken());
            NotifyOfPropertyChange(() => IsLoggedIn);
        }

        public async Task HandleAsync(LogOnEvent message, CancellationToken cancellationToken)
        {
            await ActivateItemAsync(IoC.Get<SalesViewModel>(), cancellationToken);
            NotifyOfPropertyChange(() => IsLoggedIn);
        }
    }
}
