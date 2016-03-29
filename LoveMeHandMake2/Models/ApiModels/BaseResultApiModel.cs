using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Models.ApiModels
{
    public class BaseResultApiModel
    {
        public DateTime ReceiveRequestTime { get; set; }

        public bool IsRequestSuccess { get; set; }

        public List<string> ErrMsgs { get; set; }

        public BaseResultApiModel()
        {
            this.ErrMsgs = new List<string>();
        }
    }
}