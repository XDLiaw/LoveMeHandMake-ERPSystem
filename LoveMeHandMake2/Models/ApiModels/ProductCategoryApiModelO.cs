﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Models.ApiModels
{
    public class ProductCategoryApiModelO
    {
        public DateTime ReceiveRequestTime { get; set; }

        public List<ProductCategory> CanCellList { get; set; }
    }
}