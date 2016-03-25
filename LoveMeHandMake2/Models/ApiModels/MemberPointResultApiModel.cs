using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Models.ApiModels
{
    public class MemberPointResultApiModel
    {
        public DateTime ReceiveRequestTime { get; set; }

        public bool IsRequestSuccess { get; set; }

        public List<string> ErrMsgs { get; private set; }

        public double Point { get; set; }

        public MemberPointResultApiModel()
        {
            this.ErrMsgs = new List<string>();
        }
    }
}