using log4net;
using LoveMeHandMake2.Models;
using LoveMeHandMake2.Models.ApiModels;
using LoveMeHandMake2.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LoveMeHandMake2.Controllers.ApiControllers
{
    public class TradeApiController : ApiController
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(TradeApiController));
        private LoveMeHandMakeContext db = new LoveMeHandMakeContext();

        [HttpPost]
        public TradeOrderResultApiModel NewTradeOrder(TradeOrderRequestApiModel arg)
        {
            log.Info(JsonConvert.SerializeObject(arg));
            TradeOrderResultApiModel res = new TradeOrderResultApiModel();
            res.ReceiveRequestTime = DateTime.Now;
            res.IsRequestSuccess = false;
            try
            {
                if (arg.IsValid() == false)
                {
                    log.Error(arg.GetInvalidReasons());
                    res.ErrMsgs.AddRange(arg.GetInvalidReasons());
                    return res;
                }
                if (new TradeOrderService().IsOrderIDExist(arg.OrderID))
                {
                    throw new ArgumentException("OrderID: [" + arg.OrderID + "] already exist!");
                }
                                
                // 1. insert TradeOrder to DB
                Member member = db.Members.Where(x => x.MemberGuid == arg.MemberGuid && x.ValidFlag == true).FirstOrDefault();
                
                
                TradeOrder tradeOrder = arg.ToTradeOrder();




                // 2. insert TradePurchaseProducts to DB





                // 3. if this trade is belong's to some member, 
                //    then deduction point from member and modify PointUsage 







                res.IsRequestSuccess = true;
                return res;
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                res.ErrMsgs.Add(e.Message);
                res.IsRequestSuccess = false;
                return res;
            }
        }

        





    }
}
