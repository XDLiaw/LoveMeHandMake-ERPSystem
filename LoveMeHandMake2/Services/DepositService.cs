using LoveMeHandMake2.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Services
{
    public class DepositService
    {
        private LoveMeHandMakeContext db = new LoveMeHandMakeContext();

        public bool IsOrderIDExist(string orderID)
        {
            return db.DepositHistory.Where(x => x.OrderID == orderID && x.ValidFlag == true).Count() > 0;
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
            if (new DepositService().IsOrderIDExist(dh.OrderID))
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

            dh.Create();
            dh = TryCompute(dh);
            dh.Member.Point += dh.TotalPoint;
            dh.Member.AccumulateDeposit += dh.TotalDepositMoney;
            if (dh.AccumulateDepositRewardRule != null)
            {
                dh.Member.AccumulateDeposit -= dh.AccumulateDepositRewardRule.DepositAmount;
            }
            db.Entry(dh.Member).State = EntityState.Modified;
            db.DepositHistory.Add(dh);
            db.SaveChanges();

            List<PointUsage> pointUsageList = new List<PointUsage>();
            for (int i = 0; i < dh.TotalPoint; i++)
            {
                PointUsage pu = new PointUsage()
                {
                    MemberID = dh.Member.ID,
                    DepositOrderID = dh.ID,
                    PointValue = dh.AvgPointCost,
                    DepositTime = dh.DepostitDateTime
                };
                pointUsageList.Add(pu);
                
            }
            db.PointUsage.AddRange(pointUsageList);
            db.SaveChanges();

            return dh;
        }

    }
}