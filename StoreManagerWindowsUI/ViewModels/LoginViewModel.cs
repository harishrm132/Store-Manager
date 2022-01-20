using Caliburn.Micro;
using StoreManagerWindowsUI.Helpers;
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

        public LoginViewModel(IAPIHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public string UserName
        {
            get { return _userName; }
            set 
            { 
                _userName = value;
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
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }        
        }

    }
}
