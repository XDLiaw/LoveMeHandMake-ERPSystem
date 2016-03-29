using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Models.ApiModels
{
    public class DepositCancelResultApiModel : BaseResultApiModel
    {
        [Display(Name = "会员现有点数")]
        public double CurrentPoint { get; set; }
    }
}