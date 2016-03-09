using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Models
{
    public class StoreCanSellCategory : BaseModel
    {
        [Required]
        public int StoreID { get; set; }

        [Required]
        public int ProductCategoryID { get; set; }

        public virtual Store Store { get; set; }

        public virtual ProductCategory ProductCategory { get; set; }
    }
}