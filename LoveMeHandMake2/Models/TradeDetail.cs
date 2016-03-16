using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Models
{
    public class TradeDetail : BaseModel
    {
        [Display(Name = "交易单号")]
        [Required]
        public int OrderID { get; set; }

        public virtual TradeList Order { get; set; }
       
        [Required]
        public int ProductID { get; set; }

        [Display(Name = "商品")]
        public virtual Product Product { get; set; }

        [Display(Name = "数量")]
        [Required]
        public int Amount { get; set; }

        [Display(Name = "单价(点)")]
        // How many points for each one of this product
        public int? UnitPoint { get; set; }

        [Display(Name = "单价(豆)")]
        // How many beans for each one of this product
        public int? UnitBean { get; set; }

        [Display(Name = "总价")]
        [Required]
        // Sum = Amount * (UnitPoint or UnitBean)
        public int Sum { get; set; }
    }
}