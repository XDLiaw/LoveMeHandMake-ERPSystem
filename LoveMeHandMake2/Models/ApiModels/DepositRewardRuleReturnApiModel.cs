using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Models.ApiModels
{
    public class DepositRewardRuleReturnApiModel
    {
        public DateTime ReceiveRequestTime { get; set; }

        public List<DepositRewardRule> NewRules { get; set; }

        public List<DepositRewardRule> ChangedRules { get; set; }

        public List<DepositRewardRule> RemovedRules { get; set; }
    }
}