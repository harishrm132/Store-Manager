using Caliburn.Micro;
using StoreManagerWindowsUI.EventModels;
using StoreManagerWindowsUI.Helpers;
using StoreManagerWindowsUI.Library.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace StoreManagerWindowsUI.ViewModels
{
    public class LoginViewModel : Screen
    {
        private string _userName;
        private string _password;
        private IAPIHelper _apiHelper;
        private IEventAggregator _eventAggregator;

        public LoginViewModel(IAPIHelper apiHelper, IEventAggregator eventAggregator)
        {
            _apiHelper = apiHelper;
            _eventAggregator = eventAggregator;

            #if DEBUG
                _userName = "test@gmil.com";
                _password = "Password@123";
            #endif
        }

        public string UserName
        {
            get { return _userName; }
            set 
            { 
                _userName = value; //TODO - Handles Email Address Change Over Time
                NotifyOfPropertyChange(() => UserName);
                NotifyOfPropertyChange(() => CanLogin);
            }
        }

        public string Password
        {
            get { return _password; }
            set 
            { 
                _password = value;
                NotifyOfPropertyChange(() => Password);
                NotifyOfPropertyChange(() => CanLogin);
            }
        }

        public bool IsErrorVisble
        {
            get 
            {
                return !string.IsNullOrWhiteSpace(ErrorMessage);
            }
        }

        private string _errorMessage;

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set 
            { 
                _errorMessage = value; 
                NotifyOfPropertyChange(() => ErrorMessage);
                NotifyOfPropertyChange(() => IsErrorVisble);

            }
        }


        public bool CanLogin
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Password))
                {
                    return true;
                }
                return false; 
            }
        }
        
        public async Task Login()
        {
            try
            {
                ErrorMessage = string.Empty;
                var result = await _apiHelper.AuthenticateAsync(UserName, Password);

                //Capture More Information about user
                await _apiHelper.GetLoggedInUserInfo(result.Access_Token);

                //Raise an event - Differ Event from other calls
                _eventAggregator.PublishOnUIThread(new LogOnEvent());
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }        
        }

    }
}
