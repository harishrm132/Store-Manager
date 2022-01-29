using Microsoft.Extensions.Configuration;
using StoreDataManager.Library.Internal.DataAccess;
using StoreDataManager.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreDataManager.Library.DataAccess
{
    public class UserData : IUserData
    {
        private readonly ISqlDataAccess sql;

        public UserData(ISqlDataAccess sql)
        {
            this.sql = sql;
        }

        public List<UserModel> GetUserById(string id)
        {
            var p = new { Id = id };
            var output = sql.LoadData<UserModel, dynamic>("dbo.[spUserLookup]", p, "StoreData");
            return output;
        }
    }
}
