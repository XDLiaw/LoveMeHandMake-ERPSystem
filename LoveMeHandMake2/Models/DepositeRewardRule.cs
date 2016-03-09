using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Models
{
    public class DepositeRewardRule : BaseModel
    {
        [Required]
        public int DepositeAmount { get; set; }

        public int RewardPoint { get; set; }

        public int RewardMoney { get; set; }

        [Required]
        public bool AccumulateFlag { get; set; }

        public DateTime ValidDateStart { get; set; }

        public DateTime ValidDateEnd { get; set; }
    }
}