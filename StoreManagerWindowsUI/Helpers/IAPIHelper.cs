using StoreManagerWindowsUI.Models;
using System.Threading.Tasks;

namespace StoreManagerWindowsUI.Helpers
{
    public interface IAPIHelper
    {
        Task<AuthenticatedUser> AuthenticateAsync(string userName, string password);
    }
}