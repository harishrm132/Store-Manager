using StoreDataManager.Library.Internal.DataAccess;
using StoreDataManager.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreDataManager.Library.DataAccess
{
    public class UserData
    {
        public List<UserModel> GetUserById(string id)
        {
            SqlDataAccess sql = new SqlDataAccess();

            var p = new { Id = id };
            var output = sql.LoadData<UserModel, dynamic>("dbo.spUserLookup", p, "StoreData");
            return output;
        }
    }
}
