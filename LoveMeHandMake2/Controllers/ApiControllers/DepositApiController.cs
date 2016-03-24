using log4net;
using LoveMeHandMake2.Models;
using LoveMeHandMake2.Models.ApiModels;
using LoveMeHandMake2.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LoveMeHandMake2.Controllers.ApiControllers
{
    public class DepositApiController : ApiController
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(StoreController));
        private LoveMeHandMakeContext db = new LoveMeHandMakeContext();

        [HttpPost]
        public DepositResultApiModel Deposit(DepositRequestApiModel arg)
        {
            log.Info(JsonConvert.SerializeObject(arg));
            DepositResultApiModel res = new DepositResultApiModel();
            if (arg.IsValid() == false)
            {
                log.Warn(arg.GetInvalidReasons());
                res.ErrMsg = arg.GetInvalidReasons().First();
                return res;
            }
            try
            {
                DepositHistory dh = arg.ToDepositHistory();
                dh = new DepositService().Deposit(dh);
                res.TotalDepositMoney = dh.TotalDepositMoney;
                res.DepositPoint = dh.DepositPoint;
                res.TotalPoint = dh.TotalPoint;
                res.DepositRewardPoint = dh.DepositRewardPoint;
                res.AvgPointCost = dh.AvgPointCost;

                return res;
            }
            catch (Exception e)
            {
                log.Error(null, e);
                res.ErrMsg = e.Message;
                return res;
            }
        }

        public DepositResultApiModel TryCompute(DepositRequestApiModel arg)
        {
            log.Info(JsonConvert.SerializeObject(arg));
            DepositResultApiModel res = new DepositResultApiModel();
            if (arg.IsValid() == false)
            {
                log.Warn(arg.GetInvalidReasons());
                res.ErrMsg = arg.GetInvalidReasons().First();
                return res;
            }
            DepositHistory dh = arg.ToDepositHistory();
            try
            {
                dh = new DepositService().TryCompute(dh);

                res.IsDepositSuccess = false;
                res.TotalDepositMoney = dh.TotalDepositMoney;
                res.DepositPoint = dh.DepositPoint;                
                res.DepositRewardPoint = dh.DepositRewardPoint;
                res.TotalPoint = dh.TotalPoint;
                res.AvgPointCost = dh.AvgPointCost;

                return res;
            }
            catch (Exception e)
            {
                log.Error(null, e);
                res.ErrMsg = e.Message;
                return res;
            }
        }
    }
}
