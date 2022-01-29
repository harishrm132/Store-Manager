using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using StoreDataManager.Library.DataAccess;
using StoreDataManager.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreManagerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryData inventoryData;

        public InventoryController(IInventoryData inventoryData)
        {
            this.inventoryData = inventoryData;
        }

        [Authorize(Roles = "Manager,Admin")]
        [HttpGet]
        public List<InventoryModel> Get()
        {
            return inventoryData.GetInvetory();
        }

        //[Authorize(Roles = "WarehouseWorker")]
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public void Post(InventoryModel item)
        {
            inventoryData.SaveInvetory(item);
        }
    }
}
