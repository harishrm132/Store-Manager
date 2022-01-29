using StoreDataManager.Library.Models;
using System.Collections.Generic;

namespace StoreDataManager.Library.DataAccess
{
    public interface IInventoryData
    {
        List<InventoryModel> GetInvetory();
        void SaveInvetory(InventoryModel item);
    }
}