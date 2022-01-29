using AutoMapper;
using Caliburn.Micro;
using StoreManagerWindowsUI.Library.Api;
using StoreManagerWindowsUI.Library.Helpers;
using StoreManagerWindowsUI.Library.Models;
using StoreManagerWindowsUI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace StoreManagerWindowsUI.ViewModels
{
    public class SalesViewModel : Screen
    {

        private IProductEndPoint _productEndPoint;
        private IConfigHelper _configHelper;
        private ISaleEndPoint _saleEndPoint;
        private IMapper _mapper;
        private readonly StatusInfoViewModel _status;
        private readonly IWindowManager _window;

        public SalesViewModel(IProductEndPoint productEndPoint, IConfigHelper configHelper, ISaleEndPoint saleEndPoint, IMapper mapper,
            StatusInfoViewModel status, IWindowManager window)
        {
            _productEndPoint = productEndPoint;
            _configHelper = configHelper;
            _saleEndPoint = saleEndPoint;
            _mapper = mapper;
            _status = status;
            _window = window;
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            try
            {
                await LoadProducts();
            }
            catch (Exception ex)
            {
                //TODO - Make message box better
                dynamic settings = new ExpandoObject();
                settings.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                settings.ResizeMode = ResizeMode.NoResize;
                settings.Title = "System Error";

                _status.UpdateMessage("Exception", ex.Message);
                await _window.ShowDialogAsync(_status, null, settings);

                TryCloseAsync();
            }
        }

        private async Task LoadProducts()
        {
            var productlist = await _productEndPoint.GetAll();
            var products = _mapper.Map<List<ProductDisplayModel>>(productlist);
            Products = new BindingList<ProductDisplayModel>(products);
        }

        private BindingList<ProductDisplayModel> _products;

        public BindingList<ProductDisplayModel> Products
        {
            get { return _products; }
            set 
            { 
                _products = value;
                NotifyOfPropertyChange(() => Products);
            }
        }

        private ProductDisplayModel _selectedProduct;

        public ProductDisplayModel SelectedProduct
        {
            get { return _selectedProduct; }
            set 
            { 
                _selectedProduct = value;
                NotifyOfPropertyChange(() => SelectedProduct);
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }


        private BindingList<CartDisplayModel> _cart = new BindingList<CartDisplayModel>();

        public BindingList<CartDisplayModel> Cart
        {
            get { return _cart; }
            set 
            { 
                _cart = value;
                NotifyOfPropertyChange(() => Cart);
            }
        }

        private CartDisplayModel _selectedCartItem;

        public CartDisplayModel SelectedCartItem
        {
            get { return _selectedCartItem; }
            set 
            { 
                _selectedCartItem = value;
                NotifyOfPropertyChange(() => SelectedCartItem);
                NotifyOfPropertyChange(() => CanRemoveFromCart);
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
                return CalcSubTotal().ToString("C");
            }
        }

        private decimal CalcSubTotal()
        {
            decimal output = 0;
            foreach (var item in Cart)
            {
                output += (item.Product.RetailPrice * item.QuantityInCart);
            }
            return output;
        }

        public string Tax
        {
            get
            {
                return CalcTax().ToString("C");
            }
        }

        private decimal CalcTax()
        {
            decimal output = 0;
            decimal taxRate = _configHelper.GetTaxRate()/100;
            output = Cart.Where(x => x.Product.IsTaxable).Sum(x => x.Product.RetailPrice * x.QuantityInCart * taxRate);
            return output;
        }

        public string Total
        {
            get             
            { 
                return (CalcSubTotal() + CalcTax()).ToString("C"); 
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
            CartDisplayModel extItem = Cart.FirstOrDefault(x => x.Product == SelectedProduct);
            if (extItem != null)
            {
                extItem.QuantityInCart += ItemQuantity;
            }
            else
            {
                CartDisplayModel item = new CartDisplayModel
                {
                    Product = SelectedProduct,
                    QuantityInCart = ItemQuantity
                };
                Cart.Add(item);
            }
            SelectedProduct.QuantityInStock -= ItemQuantity;
            ItemQuantity = 1;
            //Update product Box once the item is Added to Cart
            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => CanCheckOut);
        }
        
        public bool CanRemoveFromCart
        {
            get
            {
                if (SelectedCartItem != null && SelectedCartItem?.QuantityInCart > 0)
                {
                    return true;
                }
                return false; 
            } 
        }

        public void RemoveFromCart()
        {
            SelectedCartItem.Product.QuantityInStock += 1;
            if(SelectedCartItem.QuantityInCart > 1)
            {
                SelectedCartItem.QuantityInCart -= 1;
            }
            else
            {
                Cart.Remove(SelectedCartItem);
            }

            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => CanCheckOut);
            NotifyOfPropertyChange(() => CanAddToCart);
        }

        public bool CanCheckOut
        {
            get
            {
                if (Cart.Count > 0)
                {
                    return true;
                }
                return false;
            }
        }

        public async void CheckOut()
        {
            SaleModel sale = new SaleModel();
            foreach (var item in Cart)
            {
                sale.SaleDetails.Add(new SaleDetailModel
                {
                    ProductId = item.Product.Id,
                    Quantity = item.QuantityInCart
                });
            }

            await _saleEndPoint.PostSale(sale);

            //Reset
            await ResetSalesViewModel();
        }

        private async Task ResetSalesViewModel()
        {
            Cart = new BindingList<CartDisplayModel>();
            //TODO - add Clearing cart item if doesn't happen
            await LoadProducts();

            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => CanCheckOut);
        }
    }
}