using StoreManagerWindowsUI.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace StoreManagerWindowsUI.Library.Helpers
{
    public interface IAPIHelper
    {
        HttpClient ApiClient { get; }

        Task<AuthenticatedUser> AuthenticateAsync(string userName, string password);

        Task GetLoggedInUserInfo(string token);
    }
}