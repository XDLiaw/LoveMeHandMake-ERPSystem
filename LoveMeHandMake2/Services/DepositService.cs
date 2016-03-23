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
            dh.MemberGuid = dh.Member.MemberGuid;
            dh.DepositRewardRuleList = db.DepositRewardRule.Where(x => x.ValidFlag == true)
                .OrderBy(x => x.DepositAmount).ToList();
            dh.computeAll();
            return dh;
        }

        public DepositHistory Deposit(DepositHistory dh)
        {
            dh.DepositStore = db.Stores
                .Where(x => x.ID == dh.DepositStoreID && x.ValidFlag == true 
                    && (x.StopBusinessDate == null || x.StopBusinessDate > dh.DepostitDateTime))
                .First();
            dh.DepositTeacher = db.Teachers
                .Where(x => x.ID == dh.DepositTeacherID && x.ValidFlag == true
                    && (x.ResignDate == null || x.ResignDate > dh.DepostitDateTime))
                .First();

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

            return dh;
        }

    }
}