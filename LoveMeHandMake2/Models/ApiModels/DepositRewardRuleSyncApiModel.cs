using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Models.ApiModels
{
    public class DepositRewardRuleSyncApiModel
    {
        public DateTime ReceiveRequestTime { get; set; }

        public List<DepositRewardRule> NewList { get; set; }

        public List<DepositRewardRule> ChangedList { get; set; }

        public List<DepositRewardRule> RemovedList { get; set; }
    }
}