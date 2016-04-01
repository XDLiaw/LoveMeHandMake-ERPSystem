﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Models.ViewModels
{
    public class PopularProductViewModel
    {
        [Display(Name = "门市")]
        public int? SearchStoreID { get; set; }

        [Display(Name = "商品类别")]
        public int? SearchProductCategoryID { get; set; }

        [Display(Name = "日期(起)")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? SearchDateStart { get; set; }

        [Display(Name = "日期(讫)")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? SearchDateEnd { get; set; }

        public IQueryable<PopularProduct> productList { get; set; }
    }

    public class PopularProduct
    {
        [Display(Name = "商品类别")]
        public string CategoryName { get; set; }

        [Display(Name = "名称")]
        public string Name { get; set; }

        [Display(Name = "计价单位")]
        public int Unit { get; set; }

        [Display(Name = "售价")]
        public int Price { get; set; }

        [Display(Name = "销售数量")]
        public int Amount { get; set; }
    }
}