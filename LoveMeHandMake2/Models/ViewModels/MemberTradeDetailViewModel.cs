using LoveMeHandMake2.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Models.ViewModels
{
    public class MemberTradeDetailViewModel
    {
        [Display(Name = "订单")]
        public TradeOrder Order { get; set; }

        [Display(Name = "订单明细")]
        public List<TradePurchaseProduct> TradePurchaseProducts { get; set; }
    }
}