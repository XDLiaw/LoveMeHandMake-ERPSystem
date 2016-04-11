using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Models.ApiModels
{
    public class DepositResultApiModel : BaseResultApiModel
    {
        [Display(Name = "总储值金额")]
        public int TotalDepositMoney { get; set; }

        [Display(Name = "储值点数")]
        public int DepositPoint { get; set; }

        [Display(Name = "储值满额送点")]
        public int DepositRewardPoint { get; set; }

        [Display(Name = "总新增点数")]
        public double TotalPoint { get; set; }

        [Display(Name = "每点平均成本")]
        public double AvgPointCost { get; set; }

        [Display(Name = "会员现有点数")]
        public double CurrentPoint { get; set; }
    }
}