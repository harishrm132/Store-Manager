using Microsoft.AspNet.Identity;
using StoreDataManager.Library.DataAccess;
using StoreDataManager.Library.Models;
using StoreDataManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace StoreDataManager.Controllers
{
    [Authorize()]
    public class SaleController : ApiController
    {
        [Authorize(Roles = "Cashier")]
        [HttpPost]
        public void Post(SaleModel sale)
        {
            string userId = RequestContext.Principal.Identity.GetUserId();
            SaleData saleData = new SaleData();
            saleData.SaveSale(sale, userId);
        }

        [Authorize(Roles = "Admin,Manager")]
        [Route("GetSalesReport")]
        public List<SaleReportModel> GetSalesReport()
        {
            SaleData data = new SaleData();
            return data.GetSaleReport();
        }
    }
}
