using StoreDataManager.Library.Models;
using System.Collections.Generic;

namespace StoreDataManager.Library.DataAccess
{
    public interface IProductData
    {
        ProductModel GetProductById(int productid);
        List<ProductModel> GetProducts();
    }
}