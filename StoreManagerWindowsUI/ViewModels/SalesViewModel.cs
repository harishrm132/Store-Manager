using Caliburn.Micro;
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
        private BindingList<string> _products;

        public BindingList<string> Products
        {
            get { return _products; }
            set 
            { 
                _products = value;
                NotifyOfPropertyChange(() => Products);
            }
        }

        private BindingList<string> _cart;

        public BindingList<string> Cart
        {
            get { return _cart; }
            set 
            { 
                _cart = value;
                NotifyOfPropertyChange(() => Cart);
            }
        }


        private int _itemQuantity;

        public int ItemQuantity
        {
            get { return _itemQuantity; }
            set 
            { 
                _itemQuantity = value;
                NotifyOfPropertyChange(() => ItemQuantity);
            }
        }

        public string SubTotal
        {
            get             
            { 
                //TODO  - Calculation
                return "$0.00"; 
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


        public bool CanAddtoCart 
        { 
            get 
            {
                //TODO - Make Sure Sometyhing Selected & have Items
                return true;
            } 
        }

        public void AddtoCart()
        {

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

        }

        public void CheckOut()
        {
            //TODO - Make sure Soem thing in Cart
        }
    }
}
