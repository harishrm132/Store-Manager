using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using StoreDataManager.Library.DataAccess;
using StoreDataManager.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace StoreManagerApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize()]
    [ApiController]
    public class SaleController : ControllerBase
    {
        private readonly ISaleData saleData;

        public SaleController(ISaleData saleData)
        {
            this.saleData = saleData;
        }

        [Authorize(Roles = "Cashier")]
        [HttpPost]
        public void Post(SaleModel sale)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            saleData.SaveSale(sale, userId);
        }

        [Authorize(Roles = "Admin,Manager")]
        [Route("GetSalesReport")]
        [HttpGet]
        public List<SaleReportModel> GetSalesReport()
        {
            return saleData.GetSaleReport();
        }
    }
}
