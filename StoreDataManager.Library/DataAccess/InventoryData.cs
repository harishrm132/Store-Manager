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
    public class InventoryData : IInventoryData
    {
        private readonly ISqlDataAccess sql;

        public InventoryData(ISqlDataAccess sql)
        {
            this.sql = sql;
        }

        public List<InventoryModel> GetInvetory()
        {
            return sql.LoadData<InventoryModel, dynamic>("[dbo].[spInventory_GetAll]", new { }, "StoreData");
        }

        public void SaveInvetory(InventoryModel item)
        {
            sql.SaveData("[dbo].[spInventory_Insert]", item, "StoreData");
        }
    }
}
