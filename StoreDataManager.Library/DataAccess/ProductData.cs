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
    public class ProductData : IProductData
    {
        private readonly ISqlDataAccess sql;

        public ProductData(ISqlDataAccess sql)
        {
            this.sql = sql;
        }

        public List<ProductModel> GetProducts()
        {
            var output = sql.LoadData<ProductModel, dynamic>("dbo.[spProduct_GetAll]", new { }, "StoreData");
            return output;
        }

        public ProductModel GetProductById(int productid)
        {
            var output = sql.LoadData<ProductModel, dynamic>("dbo.[spProduct_GetById]", new { Id = productid }, "StoreData").FirstOrDefault();
            return output;
        }
    }
}
