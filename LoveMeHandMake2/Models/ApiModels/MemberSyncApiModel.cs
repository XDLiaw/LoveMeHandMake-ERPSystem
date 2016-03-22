using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Models.ApiModels
{
    public class MemberSyncApiModel
    {
        public DateTime ReceiveRequestTime { get; set; }

        public List<Member> NewList { get; set; }

        public List<Member> ChangedList { get; set; }

        public List<Member> RemovedList { get; set; }


    }
}