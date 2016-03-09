using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Models
{
    public class TradeDetail : BaseModel
    {
        [Required]
        public int TradeID { get; set; }

        [Required]
        public int ProductID { get; set; }

        [Required]
        public int Amount { get; set; }

        // How many points for each one of this product
        public int? UnitPoint { get; set; }

        // How many beans for each one of this product
        public int? UnitBean { get; set; }

        [Required]
        public int Money { get; set; }
    }
}