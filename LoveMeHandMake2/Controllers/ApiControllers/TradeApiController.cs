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
                new TradeOrderService().NewTradeOrder(arg, true);
                res.IsRequestSuccess = true;
            }
            catch (Exception e)
            {
                log.Error(null, e);
                res.ErrMsgs.Add(e.Message);
                res.IsRequestSuccess = false;
            }
            return res;
        }

        [HttpPost]
        public TradeOrderResultApiModel CancelTradeOrder(TradeOrderCancelRequestApiModel arg)
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

                new TradeOrderService().CancelTradeOrder(arg.orderID);
                res.IsRequestSuccess = true;
            }
            catch (Exception e)
            {
                log.Error(null, e);
                res.ErrMsgs.Add(e.Message);
                res.IsRequestSuccess = false;
            }
            return res;
        }





    }
}
