using Caliburn.Micro;
using StoreManagerWindowsUI.Library.Api;
using StoreManagerWindowsUI.Library.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManagerWindowsUI.ViewModels
{
    public class SalesViewModel : Screen
    {

        private IProductEndPoint _productEndPoint;

        public SalesViewModel(IProductEndPoint productEndPoint)
        {
            _productEndPoint = productEndPoint;
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            await LoadProducts();
        }

        private async Task LoadProducts()
        {
            var productlist = await _productEndPoint.GetAll();
            Products = new BindingList<ProductModel>(productlist);
        }

        private BindingList<ProductModel> _products;

        public BindingList<ProductModel> Products
        {
            get { return _products; }
            set 
            { 
                _products = value;
                NotifyOfPropertyChange(() => Products);
            }
        }

        private ProductModel _selectedProduct;

        public ProductModel SelectedProduct
        {
            get { return _selectedProduct; }
            set 
            { 
                _selectedProduct = value;
                NotifyOfPropertyChange(() => SelectedProduct);
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }


        private BindingList<CartModel> _cart = new BindingList<CartModel>();

        public BindingList<CartModel> Cart
        {
            get { return _cart; }
            set 
            { 
                _cart = value;
                NotifyOfPropertyChange(() => Cart);
            }
        }


        private int _itemQuantity = 1;

        public int ItemQuantity
        {
            get { return _itemQuantity; }
            set 
            { 
                _itemQuantity = value;
                NotifyOfPropertyChange(() => ItemQuantity);
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }

        public string SubTotal
        {
            get             
            {
                decimal output = 0;
                foreach (var item in Cart)
                {
                    output += (item.Product.RetailPrice * item.QuantityInCart);
                }
                return output.ToString("C"); 
            }
        }

        public string Tax
        {
            get             
            { 
                //TODO  - Calculation
                return "$0.00"; 
            }
        }

        public string Total
        {
            get             
            { 
                //TODO  - Calculation
                return "$0.00"; 
            }
        }


        public bool CanAddToCart
        { 
            get 
            {
                //Make Sure Something Selected & have Item Quantity
                if (ItemQuantity > 0 && SelectedProduct?.QuantityInStock >= ItemQuantity)
                {
                    return true;
                }
                return false;
            } 
        }

        public void AddToCart()
        {
            CartModel extItem = Cart.FirstOrDefault(x => x.Product == SelectedProduct);
            if (extItem != null)
            {
                extItem.QuantityInCart += ItemQuantity;
                //TODO - Hack so find the better way for refreshing the cart
                Cart.Remove(extItem); Cart.Add(extItem);
            }
            else
            {
                CartModel item = new CartModel
                {
                    Product = SelectedProduct,
                    QuantityInCart = ItemQuantity
                };
                Cart.Add(item);
            }
            SelectedProduct.QuantityInStock -= ItemQuantity;
            ItemQuantity = 1;

            NotifyOfPropertyChange(() => SubTotal);
        }
        
        public bool CanRemoveFromCart
        {
            get
            {
                //TODO - Make Sure Sometyhing Selected & have Items
                return true;
            } 
        }

        public void RemoveFromCart()
        {
            NotifyOfPropertyChange(() => SubTotal);
        }

        public void CheckOut()
        {
            //TODO - Make sure Soem thing in Cart
        }
    }
}
