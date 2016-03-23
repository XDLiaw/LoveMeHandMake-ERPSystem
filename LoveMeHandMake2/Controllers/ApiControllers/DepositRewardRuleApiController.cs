using LoveMeHandMake2.Models;
using LoveMeHandMake2.Models.ApiModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LoveMeHandMake2.Controllers.ApiControllers
{
    public class DepositRewardRuleApiController : ApiController
    {
        private LoveMeHandMakeContext db = new LoveMeHandMakeContext();

        [HttpGet]
        public Object Synchronize(DateTime lastSynchronizeTime)
        {
            DateTime receiveRequestTime = DateTime.Now;
            List<DepositRewardRule> newRules = db.DepositRewardRule
                .Where(x => x.CreateTime > lastSynchronizeTime 
                    && x.ValidFlag == true).ToList();
            List<DepositRewardRule> changedRules = db.DepositRewardRule
                .Where(x => x.CreateTime <= lastSynchronizeTime
                    && x.UpdateTime > lastSynchronizeTime 
                    && x.ValidFlag == true).ToList();
            List<DepositRewardRule> removedRules = db.DepositRewardRule
                .Where(x => x.CreateTime <= lastSynchronizeTime
                    && x.UpdateTime > lastSynchronizeTime
                    && x.ValidFlag == false).ToList();

            var res = new
            {
                ReceiveRequestTime = receiveRequestTime,
                NewRules = newRules,
                ChangedRules = changedRules,
                RemovedRules = removedRules
            };

            return res;
        }
    }
}
