using StoreDataManager.Library.Models;
using System.Collections.Generic;

namespace StoreDataManager.Library.DataAccess
{
    public interface IUserData
    {
        List<UserModel> GetUserById(string id);
    }
}