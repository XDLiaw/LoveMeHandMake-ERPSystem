using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Models
{
    public class Product : BaseModel
    {
        [Required(ErrorMessage = "不能为空")]
        public int CategoryID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "不能为空")]
        public string Name { get; set; }

        // Price unit can be "point" or "bean" depends on product category
        [Required(ErrorMessage = "不能为空")]
        public int Price { get; set; }

        public string ImagePath { get; set; }
    }
}