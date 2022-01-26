using StoreDataManager.Library.Internal.DataAccess;
using StoreDataManager.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreDataManager.Library.DataAccess
{
    public class SaleData
    {

        public void SaveSale(SaleModel saleInfo, string cashierId)
        {
            //TODO - Make this method Better
            //Start Filling in the model we will save to the database
            List<SaleDetailDBModel> details = new List<SaleDetailDBModel>();
            ProductData products = new ProductData();
            var taxRate = ConfigHelper.GetTaxRate()/100;

            foreach (var item in saleInfo.SaleDetails)
            {
                var detail = new SaleDetailDBModel()
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                };

                //Get the information about this product
                var productInfo = products.GetProductById(detail.ProductId);
                if (productInfo == null)
                {
                    throw new Exception($"The Product Id {detail.ProductId} could not be found in DB.");
                }
                detail.PurchasePrice = (productInfo.RetailPrice * detail.Quantity);
                if (productInfo.IsTaxable)
                {
                    detail.Tax = (productInfo.RetailPrice * taxRate);
                }

                details.Add(detail);
            }

            //Create the sale model
            SaleDBModel sale = new SaleDBModel
            {
                SubTotal = details.Sum(x => x.PurchasePrice),
                Tax = details.Sum(x => x.Tax),
                CashierId = cashierId
            };
            sale.Total = sale.SubTotal + sale.Tax;

            //Save the sale model
            using (SqlDataAccess sql = new SqlDataAccess())
            {
                try
                {
                    sql.StartTransaction("StoreData");
                    sql.SaveDataInTransaction<SaleDBModel>("dbo.spSale_Insert", sale);

                    //Get the ID from Sale Model
                    sale.Id = sql.LoadDataInTransaction<int, dynamic>("dbo.spSale_Lookup",
                        new { CashierId = sale.CashierId, SaleDate = sale.SaleDate }).FirstOrDefault();

                    //Finish filling in sale details model
                    foreach (var item in details)
                    {
                        item.SaleId = sale.Id;
                        //Save the sale detail model
                        sql.SaveDataInTransaction<SaleDetailDBModel>("dbo.spSaleDetail_Insert", item);
                    }
                    //sql.CommitTransaction();
                }
                catch
                {
                    sql.RollBackTransaction();
                    throw;
                }
            }
            
            
        }
    }
}
