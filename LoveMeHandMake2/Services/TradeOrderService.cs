using LoveMeHandMake2.Models;
using LoveMeHandMake2.Models.ApiModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Services
{
    public class TradeOrderService : BaseService
    {
        public TradeOrderService() : base() { }

        public TradeOrderService(LoveMeHandMakeContext db) : base(db) { }

        /// <summary>
        ///     find out is the order exist or not no matter it is marked as invalid or not
        /// </summary>
        public bool IsOrderIDExist(string orderID)
        {
            return db.TradeOrder.Where(x => x.OrderID == orderID).Count() > 0;
        }

        public void NewTradeOrder(TradeOrderRequestApiModel arg, bool checkIsOrderIdExist)
        {
            if (new TradeOrderService().IsOrderIDExist(arg.OrderID) && checkIsOrderIdExist)
            {
                throw new ArgumentException("OrderID: [" + arg.OrderID + "] already exist!");
            }
            //check total charge is equal to purchase product price
            if (isValidTotalCharge(arg) == false)
            {
                throw new ArgumentException("Total charge doesn't match Purchase Products total price!");
            }

            TradeOrder tradeOrder = arg.ToTradeOrder();
            Member member = db.Members.Where(x => x.MemberGuid == arg.MemberGuid && x.ValidFlag == true).FirstOrDefault();
            if (member == null && arg.ChargeByPoint != 0)
            {
                throw new ArgumentException("Can't charge point from non-member!");
            }

            //should't put this into follow "if block", cause maybe this order came from non-member
            tradeOrder.TotalIncomeMoney = 
                tradeOrder.ChargeByCash + 
                tradeOrder.ChargeByCreditCard + 
                tradeOrder.ChargeByMallCard + 
                tradeOrder.ChargeByAlipay + 
                tradeOrder.ChargeByWechatWallet + 
                tradeOrder.ChargeByOtherPay;

            if (member != null)
            {
                // asign member to TradeOrder
                tradeOrder.MemberID = member.ID;
                tradeOrder.Member = member;

                // deduction point from member. (member point could be nagetive, this situation will happen when front-end systems not connect to back-end system, so point data are not sychnornized)
                tradeOrder.Member.Point -= tradeOrder.ChargeByPoint;
                tradeOrder.Member.Update();
                db.Entry(tradeOrder.Member).State = EntityState.Modified;

                // modify PointUsage (due to front-end & back-end systems sychnornize problem, this pointUsageList maybe not enough for [ChargeByPoint])
                List<HalfPointUsage> pointUsageList = db.HalfPointUsage
                    .Where(x => x.MemberID == tradeOrder.MemberID && x.TradeOrderID == null)
                    .OrderBy(x => x.DepositTime)
                    .Take((int)(tradeOrder.ChargeByPoint * 2))
                    .ToList();
                pointUsageList.ForEach(x => x.TradeOrderID = tradeOrder.ID);

                // add all [HalfPointUsage]'s [HalfPointValue] to [TotalIncomeMoney]
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
                    product.TotalPoint = product.Amount * product.UnitPoint.GetValueOrDefault();
                }
                else if (product.UnitBean != null && product.UnitBean != 0)
                {
                    product.TotalPoint = (double)(product.Amount * product.UnitBean.GetValueOrDefault()) / 2;
                }
                product.Sum = product.TotalPoint * pricePerPoint;
                products.Add(product);
            }
            db.TradePurchaseProduct.AddRange(products);

            // finish whole process so save changes to DB
            db.SaveChanges();
        }

        private bool isValidTotalCharge(TradeOrderRequestApiModel arg)
        {
            double requirePoints = arg.ProductList.Where(x => x.UnitPoint != null && x.UnitPoint > 0).Sum(x => x.Amount * x.UnitPoint).GetValueOrDefault();
            double requireBeans = arg.ProductList.Where(x => x.UnitBean != null && x.UnitBean > 0).Sum(x => x.Amount * x.UnitBean).GetValueOrDefault();
            double remainBeans = arg.ChargeByPoint * 2;
            if (remainBeans >= requireBeans)
            {
                remainBeans -= requireBeans;
                requireBeans = 0;
            }
            else
            {
                remainBeans = 0;
                requireBeans -= remainBeans;
            }
            double remainPoints = remainBeans / 2;
            if (remainPoints > requirePoints) return false;
            requirePoints -= remainPoints;

            double requireMoney = requirePoints * arg.PointUnitValue + requireBeans * arg.BeanUnitValue;
            return requireMoney == arg.ChargeByCash + arg.ChargeByCreditCard + arg.ChargeByMallCard + arg.ChargeByAlipay + arg.ChargeByWechatWallet + arg.ChargeByOtherPay + arg.RewardMoney;
        }

        public TradeOrder CancelTradeOrder(string orderID)
        {
            // find tradeOrder and mark as invalid
            TradeOrder tradeOrder = db.TradeOrder.Where(x => x.OrderID == orderID && x.ValidFlag == true).FirstOrDefault();
            if (tradeOrder == null)
            {
                throw new ArgumentException("OrderID: [" + orderID + "] doesn't exist!");
            }
            tradeOrder.ValidFlag = false;
            tradeOrder.Update();
            db.Entry(tradeOrder).State = EntityState.Modified;

            // find realative TradePurchaseProducts and mark as invalid
            List<TradePurchaseProduct> products = db.TradePurchaseProduct.Where(x => x.OrderID == tradeOrder.ID && x.ValidFlag == true).ToList();
            products.ForEach(x => x.ValidFlag = false);
            products.ForEach(x => x.Update());
            products.ForEach(x => db.Entry(x).State = EntityState.Modified);

            if (tradeOrder.MemberID != null)
            {
                // find member who has this order and update points
                tradeOrder.Member.Point += tradeOrder.ChargeByPoint;
                tradeOrder.Member.Update();
                db.Entry(tradeOrder.Member).State = EntityState.Modified;

                // find halfPointUsage and update their state
                List<HalfPointUsage> pointUsageList = db.HalfPointUsage.Where(x => x.TradeOrderID == tradeOrder.ID).ToList();
                pointUsageList.ForEach(x => x.TradeOrderID = null);
            }

            db.SaveChanges();
            return tradeOrder;
        }

        public TradeOrderRequestApiModel GetCanceledOrder(string orderID)
        {
            TradeOrder tradeOrder = db.TradeOrder.Where(x => x.OrderID == orderID && x.ValidFlag == false).FirstOrDefault();
            if (tradeOrder == null)
            {
                throw new ArgumentException("OrderID: [" + orderID + "] doesn't exist!");
            }
            TradeOrderRequestApiModel res = new TradeOrderRequestApiModel(tradeOrder);

            List<TradePurchaseProduct> products = db.TradePurchaseProduct.Where(x => x.OrderID == tradeOrder.ID && x.ValidFlag == false).ToList();
            res.ProductList = new List<PurchaseProductApiModel>();
            foreach (TradePurchaseProduct p in products)
            {
                res.ProductList.Add(new PurchaseProductApiModel(p));
            }

            return res;
        }

    }
}