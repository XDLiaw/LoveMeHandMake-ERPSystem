using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Models.ApiModels
{
    public class DepositResultApiModel
    {
        public bool IsDepositSuccess { get; set; }

        public List<string> ErrMsgs { get; set; }

        [Display(Name = "总储值金额")]
        public int TotalDepositMoney { get; set; }

        [Display(Name = "储值点数")]
        public int DepositPoint { get; set; }

        [Display(Name = "储值满额送点")]
        public int DepositRewardPoint { get; set; }

        [Display(Name = "总新增点数")]
        public int TotalPoint { get; set; }

        [Display(Name = "每点平均成本")]
        public double AvgPointCost { get; set; }


        public DepositResultApiModel()
        {
            this.ErrMsgs = new List<string>();
        }





    }
}