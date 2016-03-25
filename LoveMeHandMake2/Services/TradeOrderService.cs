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
        private LoveMeHandMakeContext db = new LoveMeHandMakeContext();

        /// <summary>
        ///     find out is the order exist or not no matter it is marked as invalid or not
        /// </summary>
        public bool IsOrderIDExist(string orderID)
        {
            return db.TradeOrder.Where(x => x.OrderID == orderID).Count() > 0;
        }

        public void CancelTradeOrder(string orderID)
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