﻿using Caliburn.Micro;
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

        private BindingList<ProductModel> _cart;

        public BindingList<ProductModel> Cart
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


        public bool CanAddToCart
        { 
            get 
            {
                //TODO - Make Sure Sometyhing Selected & have Items
                return true;
            } 
        }

        public void AddToCart()
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
