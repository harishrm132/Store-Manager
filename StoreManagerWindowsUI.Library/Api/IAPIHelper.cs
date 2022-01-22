using StoreManagerWindowsUI.Models;
using System.Threading.Tasks;

namespace StoreManagerWindowsUI.Library.Helpers
{
    public interface IAPIHelper
    {
        Task<AuthenticatedUser> AuthenticateAsync(string userName, string password);

        Task GetLoggedInUserInfo(string token);
    }
}