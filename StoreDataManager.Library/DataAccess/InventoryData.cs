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
    public class InventoryData
    {
        private readonly IConfiguration configuration;

        public InventoryData(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public List<InventoryModel> GetInvetory()
        {
            SqlDataAccess sql = new SqlDataAccess(configuration);
            return sql.LoadData<InventoryModel, dynamic>("[dbo].[spInventory_GetAll]", new { }, "StoreData");
        }

        public void SaveInvetory(InventoryModel item)
        {
            SqlDataAccess sql = new SqlDataAccess(configuration);
            sql.SaveData("[dbo].[spInventory_Insert]", item, "StoreData");
        }
    }
}
