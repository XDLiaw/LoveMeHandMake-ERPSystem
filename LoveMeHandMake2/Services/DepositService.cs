using LoveMeHandMake2.Models;
using LoveMeHandMake2.Models.ApiModels;
using LoveMeHandMake2.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Services
{
    public class DepositService :BaseService
    {
        public DepositService() : base() { }

        public DepositService(LoveMeHandMakeContext db) : base(db) { }

        public DepositHistory TryCompute(DepositHistory dh)
        {
            checkAndSetMember(dh);
            dh.DepositRewardRuleList = db.DepositRewardRule.Where(x => x.ValidFlag == true).OrderBy(x => x.DepositAmount).ToList();
            dh.computeAll();
            return dh;
        }

        public DepositHistory Deposit(DepositHistory dh)
        {
            checkOrderIDExist(dh.OrderID);
            checkAndSetStore(dh);
            checkAndSetTeacher(dh);
            checkAndSetMember(dh);

            dh.DepositRewardRuleList = db.DepositRewardRule.Where(x => x.ValidFlag == true).OrderBy(x => x.DepositAmount).ToList();
            dh.computeAll();
            dh.Create();
            db.DepositHistory.Add(dh);
            db.SaveChanges();

            createHalfPointUsageData(dh);
            updateMemberPointAndAccumulateDeposit(dh);

            return dh;
        }

        public void transferPoint(TransferPointViewModel tpvm)
        {
            DepositHistory dh = tpvm.toDepositHistory();
            checkOrderIDExist(dh.OrderID);
            checkAndSetStore(dh);
            checkAndSetTeacher(dh);
            checkAndSetMember(dh);
            
            dh.Create();
            db.DepositHistory.Add(dh);
            db.SaveChanges();

            createHalfPointUsageData(dh);
            updateMemberPointAndAccumulateDeposit(dh);
        }

        private void checkOrderIDExist(string orderID)
        {
            if (db.DepositHistory.Where(x => x.OrderID == orderID && x.OrderID != null).Count() > 0)
            {
                throw new ArgumentException("OrderID: [" + orderID + "] already exist!");
            }
        }

        private void checkAndSetStore(DepositHistory dh)
        {
            dh.DepositStore = db.Stores
                .Where(x => x.ID == dh.DepositStoreID && x.ValidFlag == true && (x.StopBusinessDate == null || x.StopBusinessDate > dh.DepostitDateTime))
                .FirstOrDefault();
            if (dh.DepositStore == null)
            {
                string msg = string.Format("Can't find DepositStore which ID is [{0}]", dh.DepositStoreID);
                throw new ArgumentException(msg);
            }
        }

        private void checkAndSetTeacher(DepositHistory dh)
        {
            dh.DepositTeacher = db.Teachers
                .Where(x => x.ID == dh.DepositTeacherID && x.ValidFlag == true && (x.ResignDate == null || x.ResignDate > dh.DepostitDateTime))
                .FirstOrDefault();
            if (dh.DepositTeacher == null)
            {
                string msg = string.Format("Can't find DepositTeacher which ID is [{0}]", dh.DepositTeacherID);
                throw new ArgumentException(msg);
            }
        }

        private void checkAndSetMember(DepositHistory dh)
        {
            dh.Member = db.Members.Where(x => (x.ID == dh.MemberID || x.MemberGuid == dh.MemberGuid) && x.ValidFlag == true).FirstOrDefault();
            if (dh.Member == null)
            {
                string msg = string.Format("Can't find member which ID is [{0}], GUID is [{1}]", dh.MemberID, dh.MemberGuid);
                throw new ArgumentException(msg);
            }
            dh.MemberGuid = dh.Member.MemberGuid;
        }

        private void createHalfPointUsageData(DepositHistory dh)
        {
            double numOfHalfPoint = dh.TotalPoint * 2;
            if (dh.Member.Point < 0)
            {
                numOfHalfPoint = (dh.TotalPoint + dh.Member.Point) * 2; // if current Member point is nagetive, need to reduce some [HalfPointUsage] to fix this nage
            }
            List<HalfPointUsage> pointUsageList = new List<HalfPointUsage>();
            // Each instance of this represent 0.5 point because sometime it will only use 0.5 point
            for (int i = 0; i < numOfHalfPoint; i++)
            {
                HalfPointUsage pu = new HalfPointUsage()
                {
                    MemberID = dh.Member.ID,
                    DepositOrderID = dh.ID,
                    HalfPointValue = dh.AvgPointCost / 2,
                    DepositTime = dh.DepostitDateTime
                };
                pointUsageList.Add(pu);

            }
            db.HalfPointUsage.AddRange(pointUsageList);
            db.SaveChanges();
        }

        private void updateMemberPointAndAccumulateDeposit(DepositHistory dh)
        {
            dh.Member.Point += dh.TotalPoint;
            dh.Member.AccumulateDeposit += dh.TotalDepositMoney;
            if (dh.AccumulateDepositRewardRule != null)
            {
                dh.Member.AccumulateDeposit -= dh.AccumulateDepositRewardRule.DepositAmount;
            }
            dh.Member.Update();
            db.Entry(dh.Member).State = EntityState.Modified;
            db.SaveChanges();
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

            // mark this deposit's validFlag to false as cancel
            dh.ValidFlag = false;
            dh.Update();
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
            member.Update();
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