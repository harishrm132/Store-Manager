﻿using StoreManagerWindowsUI.Library.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StoreManagerWindowsUI.Library.Api
{
    public interface IProductEndPoint
    {
        Task<List<ProductModel>> GetAll();
    }
}