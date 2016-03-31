using LoveMeHandMake2.Models;
using LoveMeHandMake2.Models.ApiModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Services
{
    public class TradeOrderService
    {
        private LoveMeHandMakeContext db;

        public TradeOrderService() { 
            db = new LoveMeHandMakeContext();
        }

        public TradeOrderService(LoveMeHandMakeContext db) {
            this.db = db;
        }

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
            TradeOrder tradeOrder = arg.ToTradeOrder();
            Member member = db.Members.Where(x => x.MemberGuid == arg.MemberGuid && x.ValidFlag == true).FirstOrDefault();
            if (member == null && arg.ChargeByPoint != 0)
            {
                throw new ArgumentException("Can't charge point from non-member!");
            }

            tradeOrder.TotalIncomeMoney = tradeOrder.ChargeByCash + tradeOrder.ChargeByCreditCard + tradeOrder.ChargeByMallCard;
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
                    .Take((int)(tradeOrder.ChargeByPoint * 2))
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
            db.Entry(tradeOrder).State = EntityState.Modified;

            // find realative TradePurchaseProducts and mark as invalid
            List<TradePurchaseProduct> products = db.TradePurchaseProduct.Where(x => x.OrderID == tradeOrder.ID && x.ValidFlag == true).ToList();
            products.ForEach(x => x.ValidFlag = false);

            if (tradeOrder.MemberID != null)
            {
                // find member who has this order and update points
                tradeOrder.Member.Point += tradeOrder.ChargeByPoint;
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