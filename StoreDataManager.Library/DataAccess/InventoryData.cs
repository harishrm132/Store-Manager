using StoreDataManager.Library.Internal.DataAccess;
using StoreDataManager.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreDataManager.Library.DataAccess
{
    public class InventoryData
    {
        public List<InventoryModel> GetInvetory()
        {
            SqlDataAccess sql = new SqlDataAccess();
            return sql.LoadData<InventoryModel, dynamic>("[dbo].[spInventory_GetAll]", new { }, "StoreData");
        }

        public void SaveInvetory(InventoryModel item)
        {
            SqlDataAccess sql = new SqlDataAccess();
            sql.SaveData("[dbo].[spInventory_Insert]", item, "StoreData");
        }
    }
}
