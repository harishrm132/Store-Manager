using StoreManagerWindowsUI.Library.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StoreManagerWindowsUI.Library.Api
{
    public interface IUserEndPoint
    {
        Task<List<UserModel>> GetAll();
    }
}