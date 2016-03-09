using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Models
{
    public class DepositeRewardRule : BaseModel
    {
        [Display(Name = "活动名称")]
        public string ActivityName { get; set; }

        [Required]
        [Display(Name = "储值金额")]
        public int DepositeAmount { get; set; }

        [Display(Name = "赠送点数")]
        public int? RewardPoint { get; set; }

        [Display(Name = "赠送现金")]
        public int? RewardMoney { get; set; }

        [Required]
        [Display(Name = "是否为累计储值优惠")]
        public bool AccumulateFlag { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        [Display(Name = "有效日(起)")]
        public DateTime? ValidDateStart { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        [Display(Name = "有效日(讫)")]
        public DateTime? ValidDateEnd { get; set; }
    }
}