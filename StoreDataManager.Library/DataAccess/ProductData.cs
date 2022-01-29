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
    public class ProductData
    {
        private readonly IConfiguration configuration;

        public ProductData(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public List<ProductModel> GetProducts()
        {
            SqlDataAccess sql = new SqlDataAccess(configuration);

            var output = sql.LoadData<ProductModel, dynamic>("dbo.[spProduct_GetAll]", new { }, "StoreData");
            return output;
        }

        public ProductModel GetProductById(int productid)
        {
            SqlDataAccess sql = new SqlDataAccess(configuration);

            var output = sql.LoadData<ProductModel, dynamic>("dbo.[spProduct_GetById]", new { Id = productid }, "StoreData").FirstOrDefault();
            return output;
        }
    }
}
