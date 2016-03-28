using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Models.ApiModels
{
    public class DepositCancelResultApiModel
    {
        public DateTime ReceiveRequestTime { get; set; }

        public bool IsRequestSuccess { get; set; }

        public List<string> ErrMsgs { get; set; }

        public double Point { get; set; }

        public DepositCancelResultApiModel()
        {
            this.ErrMsgs = new List<string>();
        }
    }
}