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
                if (new TradeOrderService().IsOrderIDExist(arg.OrderID))
                {
                    throw new ArgumentException("OrderID: [" + arg.OrderID + "] already exist!");
                }  

                // TODO: check charge is corrected or not


                TradeOrder tradeOrder = arg.ToTradeOrder();
                Member member = db.Members.Where(x => x.MemberGuid == arg.MemberGuid && x.ValidFlag == true).FirstOrDefault();
                if (member == null && arg.ChargeByPoint != 0) {
                    throw new ArgumentException("Can't charge point from non-member!");
                }

                tradeOrder.TotalIncomeMoney = tradeOrder.ChargeByCash + tradeOrder.ChargeByCreditCard;
                if (member != null)
                {                  
                    // asign member to TradeOrder
                    tradeOrder.MemberID = member.ID;
                    tradeOrder.Member = member;

                    // deduction point from member                    
                    tradeOrder.Member.Point -= tradeOrder.ChargeByPoint;
                    db.Entry(tradeOrder.Member).State = EntityState.Modified;

                    // modify PointUsage 
                    List<HalfPointUsage> pointUsageList = db.HalfPointUsage
                        .Where(x => x.MemberID == tradeOrder.MemberID && x.TradeOrderID == null)
                        .OrderBy(x => x.DepositTime)
                        .Take(tradeOrder.ChargeByPoint * 2)
                        .ToList();
                    pointUsageList.ForEach(x => x.TradeOrderID = tradeOrder.ID);

                    // re-compute Total Income Money
                    foreach (HalfPointUsage hpu in pointUsageList)
                    {
                        tradeOrder.TotalIncomeMoney += hpu.HalfPointValue;
                    }
                }

                db.TradeOrder.Add(tradeOrder);

                // insert TradePurchaseProducts to DB
                double pricePerPoint = tradeOrder.TotalIncomeMoney / arg.TotalProductsPoint();
                List<TradePurchaseProduct> products = new List<TradePurchaseProduct>();
                foreach (PurchaseProductApiModel p in arg.ProductList)
                {
                    TradePurchaseProduct product = p.ToTradePurchaseProduct(tradeOrder.ID);
                    if (product.UnitPoint != null && product.UnitPoint != 0)
                    {
                        product.Sum = product.Amount * product.UnitPoint.GetValueOrDefault() * pricePerPoint;
                    }
                    else if (product.UnitBean != null && product.UnitBean != 0)
                    {
                        product.Sum = product.Amount * product.UnitBean.GetValueOrDefault() * pricePerPoint / 2;
                    }
                    products.Add(product);
                }
                db.TradePurchaseProduct.AddRange(products);

                // finish whole process so save changes to DB
                db.SaveChanges();
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
            log.Info("CancelTradeOrder: " + arg.orderID);
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
