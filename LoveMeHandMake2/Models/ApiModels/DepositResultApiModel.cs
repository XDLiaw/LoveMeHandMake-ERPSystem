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

        private string _ErrMsg;

        public string ErrMsg
        {
            get
            {
                return this._ErrMsg;
            }
            set
            {
                this._ErrMsg = value;
                this.IsDepositSuccess = false;
            }
        }

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
            this.IsDepositSuccess = true;
        }





    }
}