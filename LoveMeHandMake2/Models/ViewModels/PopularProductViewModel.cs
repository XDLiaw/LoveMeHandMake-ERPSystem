using MvcPaging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Configuration;

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

        [Display(Name = "页码")]
        public int PageNumber { get; set; }

        [Display(Name = "每页资料笔数")]
        public int PageSize { get; private set; }

        // -----------------------------------------------------------------------------------------

        public IPagedList<PopularProduct> productPagedList { get; set; }

        public PopularProductViewModel()
        {
            this.PageNumber = 1;
            this.PageSize = 100;
        }
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

        [Display(Name = "图片")]
        [JsonIgnore]
        public string ImageName { get; set; }
    }
}