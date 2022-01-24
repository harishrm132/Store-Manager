using StoreManagerWindowsUI.Library.Models;
using System.Threading.Tasks;

namespace StoreManagerWindowsUI.Library.Api
{
    public interface ISaleEndPoint
    {
        Task PostSale(SaleModel sale);
    }
}