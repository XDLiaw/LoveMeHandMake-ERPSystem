using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Models.ApiModels
{
    public class MemberResultApiModel
    {
        public DateTime ReceiveRequestTime { get; set; }

        public bool IsRequestSuccess { get; set; }

        public List<string> ErrMsgs { get; private set; }

        public MemberResultApiModel()
        {
            this.IsRequestSuccess = true;
            this.ErrMsgs = new List<string>();
        }

    }
}