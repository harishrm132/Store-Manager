using StoreDataManager.Library.DataAccess;
using StoreDataManager.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace StoreDataManager.Controllers
{
    [Authorize]
    public class InventoryController : ApiController
    {
        [HttpGet]
        public List<InventoryModel> Get()
        {
            InventoryData data = new InventoryData();
            return data.GetInvetory();
        }

        [HttpPost]
        public void Post(InventoryModel item)
        {
            InventoryData data = new InventoryData();
            data.SaveInvetory(item);
        }
    }
}
