﻿using LoveMeHandMake2.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.ViewModels
{
    public class MemberTradeDetailViewModel
    {
        [Display(Name = "订单")]
        public TradeList Order { get; set; }

        //This is only used to View's table head, not for data!
        public TradeDetail Detail { get; set; }

        [Display(Name = "订单明细")]
        public List<TradeDetail> Details { get; set; }
    }
}