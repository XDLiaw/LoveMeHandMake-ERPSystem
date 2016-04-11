using LoveMeHandMake2.Models;
using LoveMeHandMake2.Models.ApiModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Services
{
    public class DepositService
    {
        private LoveMeHandMakeContext db;

        public DepositService() { 
            this.db = new LoveMeHandMakeContext();
        }

        public DepositService(LoveMeHandMakeContext db)
        {
            this.db = db;
        }

        public bool IsOrderIDExist(int id)
        {
            return db.DepositHistory.Where(x => x.ID == id).Count() > 0;
        }

        public bool IsOrderIDExist(string orderID)
        {
            return db.DepositHistory
                .Where(x => x.OrderID == orderID 
                    && x.OrderID != null)                
                .Count() > 0;
        }


        public DepositHistory TryCompute(DepositHistory dh)
        {
            dh.Member = db.Members
                .Where(x => (x.ID == dh.MemberID || x.MemberGuid == dh.MemberGuid) 
                    && x.ValidFlag == true).FirstOrDefault();
            if (dh.Member == null)
            {
                string msg = string.Format("Can't find member which ID is [{0}], GUID is [{1}]", dh.MemberID, dh.MemberGuid);
                throw new ArgumentException(msg);
            }

            dh.MemberGuid = dh.Member.MemberGuid;
            dh.DepositRewardRuleList = db.DepositRewardRule.Where(x => x.ValidFlag == true)
                .OrderBy(x => x.DepositAmount).ToList();
            dh.computeAll();
            return dh;
        }

        public DepositHistory Deposit(DepositHistory dh)
        {
            return Deposit(dh, true);
        }

        public DepositHistory Deposit(DepositHistory dh, bool withCompute)
        {
            // 資料正確性驗證
            if (IsOrderIDExist(dh.OrderID))
            {
                throw new ArgumentException("OrderID: [" + dh.OrderID + "] already exist!");
            }
            dh.DepositStore = db.Stores
                .Where(x => x.ID == dh.DepositStoreID && x.ValidFlag == true 
                    && (x.StopBusinessDate == null || x.StopBusinessDate > dh.DepostitDateTime))
                .FirstOrDefault();
            if (dh.DepositStore == null) {
                string msg = string.Format("Can't find DepositStore which ID is [{0}]", dh.DepositStoreID);
                throw new ArgumentException(msg);
            }
            dh.DepositTeacher = db.Teachers
                .Where(x => x.ID == dh.DepositTeacherID && x.ValidFlag == true
                    && (x.ResignDate == null || x.ResignDate > dh.DepostitDateTime))
                .FirstOrDefault();
            if (dh.DepositTeacher == null)
            {
                string msg = string.Format("Can't find DepositTeacher which ID is [{0}]", dh.DepositTeacherID);
                throw new ArgumentException(msg);
            }
            dh.Member = db.Members
                    .Where(x => (x.ID == dh.MemberID || x.MemberGuid == dh.MemberGuid)
                        && x.ValidFlag == true).FirstOrDefault();
            if (dh.Member == null)
            {
                string msg = string.Format("Can't find member which ID is [{0}], GUID is [{1}]", dh.MemberID, dh.MemberGuid);
                throw new ArgumentException(msg);
            }
            dh.MemberGuid = dh.Member.MemberGuid;

            //============ Update Member Point & AccumulateDeposit==========================================================
            dh.Create();
            if (withCompute)
            {
                dh = TryCompute(dh);
            }
            dh.Member.Point += dh.TotalPoint;
            dh.Member.AccumulateDeposit += dh.TotalDepositMoney;
            if (dh.AccumulateDepositRewardRule != null)
            {
                dh.Member.AccumulateDeposit -= dh.AccumulateDepositRewardRule.DepositAmount;
            }
            db.Entry(dh.Member).State = EntityState.Modified;
            db.SaveChanges();

            //=========================================================================================================
            db.DepositHistory.Add(dh);
            db.SaveChanges();

            // ===================== create data to HalfPointUsage =======================================================
            List<HalfPointUsage> pointUsageList = new List<HalfPointUsage>();
            // Each instance of this represent 0.5 point because sometime it will only use 0.5 point
            for (int i = 0; i < dh.TotalPoint * 2 ; i++)
            {
                HalfPointUsage pu = new HalfPointUsage()
                {
                    MemberID = dh.Member.ID,
                    DepositOrderID = dh.ID,
                    HalfPointValue = dh.AvgPointCost/2,
                    DepositTime = dh.DepostitDateTime
                };
                pointUsageList.Add(pu);
                
            }
            db.HalfPointUsage.AddRange(pointUsageList);
            db.SaveChanges();

            return dh;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns>Points that member have after cancel this deposit</returns>
        public double Cancel(int orderID)
        {
            DepositHistory dh = db.DepositHistory.Where(x => x.ID == orderID && x.ValidFlag == true).FirstOrDefault();
            if (dh == null)
            {
                throw new ArgumentException("DepositOrder doesn't exist!");
            }
            // DON'T check if cancelling this deposit will make member's point become negative or not

            // find all tradeOrder who had used points from this deposit and cancel these order first
            List<int?> TradeOrderIDList = db.HalfPointUsage
                .Where(x => x.MemberID == dh.MemberID 
                    && x.DepositOrderID == dh.ID 
                    && x.TradeOrderID != null)
                .Select(x => x.TradeOrderID)
                .Distinct().ToList();
          
            List<TradeOrderRequestApiModel> tradeOrderRequestList = new List<TradeOrderRequestApiModel>();
            TradeOrderService tradeOrderService = new TradeOrderService(db);
            foreach (int tradeOrderID in TradeOrderIDList)
            {
                TradeOrder tradeOrder = db.TradeOrder
                    .Where(x => x.ID == tradeOrderID && x.ValidFlag == true)
                    .FirstOrDefault();
                TradeOrderRequestApiModel order = new TradeOrderRequestApiModel(tradeOrder);
                List<TradePurchaseProduct> products = db.TradePurchaseProduct
                    .Where(x => x.OrderID == tradeOrder.ID && x.ValidFlag == true)
                    .ToList();
                order.ProductList = new List<PurchaseProductApiModel>();
                foreach (TradePurchaseProduct p in products)
                {
                    order.ProductList.Add(new PurchaseProductApiModel(p));
                }
                tradeOrderRequestList.Add(order);

                tradeOrderService.CancelTradeOrder(tradeOrder.OrderID);          
            }

            // mark this deposit's validValg to false as cancel
            dh.ValidFlag = false;
            db.Entry(dh).State = EntityState.Modified;

            // update [point] and [AccumulateDeposit] from member
            Member member = db.Members.Where(x => x.ID == dh.MemberID).FirstOrDefault();
            member.Point -= dh.TotalPoint;
            if (member.AccumulateDeposit - dh.TotalDepositMoney < 0 && dh.AccumulateDepositRewardRuleID != null)
            {
                member.AccumulateDeposit = member.AccumulateDeposit + dh.AccumulateDepositRewardRule.DepositAmount - dh.TotalDepositMoney;
            }
            else
            {
                member.AccumulateDeposit -= dh.TotalDepositMoney;
            }
            db.Entry(member).State = EntityState.Modified;

            // remove halfPointUsage those created because of this deposit
            db.HalfPointUsage.RemoveRange(db.HalfPointUsage.Where(x => x.DepositOrderID == orderID));
            db.SaveChanges();

            // re-create new tradeOrder which is cancelled before
            foreach (TradeOrderRequestApiModel newTradeOrder in tradeOrderRequestList)
            {
                tradeOrderService.NewTradeOrder(newTradeOrder, false);
            }

            
            return member.Point;
        }

    }
}