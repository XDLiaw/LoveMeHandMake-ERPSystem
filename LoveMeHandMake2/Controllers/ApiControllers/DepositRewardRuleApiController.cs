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
        public DepositRewardRuleSyncApiModel Synchronize(DateTime lastSynchronizeTime)
        {
            DepositRewardRuleSyncApiModel result = new DepositRewardRuleSyncApiModel();
            result.ReceiveRequestTime = DateTime.Now;
            result.NewList = db.DepositRewardRule
                .Where(x => x.CreateTime > lastSynchronizeTime 
                    && x.ValidFlag == true).ToList();
            result.ChangedList = db.DepositRewardRule
                .Where(x => x.CreateTime <= lastSynchronizeTime
                    && x.UpdateTime > lastSynchronizeTime 
                    && x.ValidFlag == true).ToList();
            result.RemovedList = db.DepositRewardRule
                .Where(x => x.CreateTime <= lastSynchronizeTime
                    && x.UpdateTime > lastSynchronizeTime
                    && x.ValidFlag == false).ToList();

            return result;
        }
    }
}
