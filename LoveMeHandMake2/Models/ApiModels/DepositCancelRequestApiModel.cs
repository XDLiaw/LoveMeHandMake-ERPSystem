﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Models.ApiModels
{
    public class DepositCancelRequestApiModel: BaseRequestApiModel
    {
        public string OrderID { get; set; }
    }
}