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
    [Authorize(Roles = "Cashier")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductData productData;

        public ProductController(IProductData productData)
        {
            this.productData = productData;
        }

        [HttpGet]
        public List<ProductModel> Get()
        {
            return productData.GetProducts();
        }
    }
}
