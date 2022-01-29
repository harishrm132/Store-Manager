using Caliburn.Micro;
using StoreManagerWindowsUI.Library.Api;
using StoreManagerWindowsUI.Library.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace StoreManagerWindowsUI.ViewModels
{
    public class UserDisplayViewModel : Screen
    {
        private readonly StatusInfoViewModel _status;
        private readonly IWindowManager _window;
        private readonly IUserEndPoint _userEndPoint;

        private BindingList<UserModel> _users;

        public BindingList<UserModel> Users
        {
            get { return _users; }
            set 
            { 
                _users = value;
                NotifyOfPropertyChange(() => Users);
            }
        }

        private UserModel _selectedUser;

        public UserModel SelectedUser
        {
            get { return _selectedUser; }
            set 
            { 
                _selectedUser = value;
                SelectedUserName = value.Email;
                UserRoles.Clear();
                UserRoles = new BindingList<string>(value.Roles.Select(x => x.Value).ToList());
                LoadRoles();
                NotifyOfPropertyChange(() => SelectedUser);
            }
        }

        private string _selectedUserName;

        public string SelectedUserName
        {
            get { return _selectedUserName; }
            set 
            { 
                _selectedUserName = value;
                NotifyOfPropertyChange(() => SelectedUserName);
            }
        }

        private BindingList<string> _userRoles = new BindingList<string>();

        public BindingList<string> UserRoles
        {
            get { return _userRoles; }
            set 
            { 
                _userRoles = value;
                NotifyOfPropertyChange(() => UserRoles);
            }
        }

        private BindingList<string> _avaliableRoles = new BindingList<string>();

        public BindingList<string> AvaliableRoles
        {
            get { return _avaliableRoles; }
            set 
            { 
                _avaliableRoles = value;
                NotifyOfPropertyChange(() => AvaliableRoles);
            }
        }

        private string _selectedRoleToRemove;

        public string SelectedRoleToRemove
        {
            get { return _selectedRoleToRemove; }
            set 
            { 
                _selectedRoleToRemove = value;
                NotifyOfPropertyChange(() => SelectedRoleToRemove);
            }
        }

        private string _selectedAvaliableRole;

        public string SelectedAvaliableRole
        {
            get { return _selectedAvaliableRole; }
            set 
            { 
                _selectedAvaliableRole = value;
                NotifyOfPropertyChange(() => SelectedAvaliableRole);
            }
        }

        public UserDisplayViewModel(StatusInfoViewModel status, IWindowManager window, IUserEndPoint userEndPoint)
        {
            _status = status;
            _window = window;
            _userEndPoint = userEndPoint;
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            try
            {
                await LoadUsers();
            }
            catch (Exception ex)
            {
                //TODO - Make message box better
                dynamic settings = new ExpandoObject();
                settings.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                settings.ResizeMode = ResizeMode.NoResize;
                settings.Title = "System Error";

                _status.UpdateMessage("Exception", ex.Message);
                await _window.ShowDialogAsync(_status, null, settings);

                TryCloseAsync();
            }
        }

        private async Task LoadUsers()
        {
            var userlist = await _userEndPoint.GetAll();
            Users = new BindingList<UserModel>(userlist);
        }

        private async Task LoadRoles()
        {
            var roles = await _userEndPoint.GetAllRoles();
            foreach (var role in roles)
            {
                if(UserRoles.IndexOf(role.Value) < 0)
                {
                    AvaliableRoles.Add(role.Value);
                }
            }
        }

        public async Task AddSelectedRole()
        {
            await _userEndPoint.AddUserRole(SelectedUser.Id, SelectedAvaliableRole);
            UserRoles.Add(SelectedAvaliableRole);
            AvaliableRoles.Remove(SelectedAvaliableRole);
        }

        public async Task RemoveSelectedRole()
        {
            await _userEndPoint.RemoveUserFromRole(SelectedUser.Id, SelectedRoleToRemove);
            AvaliableRoles.Add(SelectedRoleToRemove);
            UserRoles.Remove(SelectedRoleToRemove);
        }
    }
}
