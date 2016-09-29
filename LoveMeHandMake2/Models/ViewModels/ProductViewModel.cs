using MvcPaging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Models.ViewModels
{
    public class ProductViewModel
    {
        public int? productCategoryID { get; set; }

        [Display(Name = "页码")]
        public int PageNumber { get; set; }

        [Display(Name = "每页资料笔数")]
        public int PageSize { get; private set; }

        // ==========================================================================

        public IPagedList<Product> ProductList { get; set; }

        public ProductViewModel()
        {
            this.PageNumber = 1;
            this.PageSize = 100;
        }

    }
}