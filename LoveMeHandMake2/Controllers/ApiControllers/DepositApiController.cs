using LoveMeHandMake2.Models;
using LoveMeHandMake2.Models.ApiModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LoveMeHandMake2.Controllers.ApiControllers
{
    public class DepositApiController : ApiController
    {
        private LoveMeHandMakeContext db = new LoveMeHandMakeContext();

        [HttpPost]
        public DepositReturnApiModel Deposit(DepositRequestApiModel arg)
        {
            DepositReturnApiModel res = new DepositReturnApiModel();
            DepositHistory dh = arg.ToDepositHistory();
            try
            {
                dh.Member = db.Members.Where(x => x.ID == dh.MemberID && x.ValidFlag == true).First();
                if (dh.Member == null)
                {
                    res.ErrMsg = "Member doesn't exist!";
                    return res;
                }
                dh.DepositStore = db.Stores
                    .Where(x => x.ID == dh.DepositStoreID && x.ValidFlag == true
                        && (x.StopBusinessDate == null || x.StopBusinessDate > dh.DepostitDateTime)
                        ).First();
                if (dh.DepositStore == null)
                {
                    res.ErrMsg = "Store doesn't exist!";
                    return res;
                }
                dh.DepositTeacher = db.Teachers
                    .Where(x => x.ID == dh.DepositTeacherID && x.ValidFlag == true
                        && (x.ResignDate == null || x.ResignDate > dh.DepostitDateTime)
                        ).First();
                if (dh.DepositTeacher == null)
                {
                    res.ErrMsg = "Teacher doesn't exist!";
                    return res;
                }
                dh.DepositRewardRule = db.DepositRewardRule.Where(x => x.ValidFlag == true).OrderBy(x => x.DepositAmount).ToList();
                dh.computeAll();
                dh.Member.Point += dh.TotalPoint;
                dh.Member.AccumulateDeposit += dh.TotalDepositMoney;
                if (dh.AccumulateDepositRewardRule != null)
                {
                    dh.Member.AccumulateDeposit -= dh.AccumulateDepositRewardRule.DepositAmount;
                }
                db.Entry(dh.Member).State = EntityState.Modified;
                db.DepositHistory.Add(dh);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                res.ErrMsg = e.Message;
                return res;
            }

            res.TotalDepositMoney = dh.TotalDepositMoney;
            res.DepositPoint = dh.DepositPoint;
            res.TotalPoint = dh.TotalPoint;
            res.DepositRewardPoint = dh.DepositRewardPoint;
            res.AvgPointCost = dh.AvgPointCost;


            return res;
        }

        public DepositReturnApiModel TryCompute(DepositRequestApiModel arg)
        {
            DepositReturnApiModel res = new DepositReturnApiModel();
            DepositHistory dh = arg.ToDepositHistory();
            try
            {
                dh.Member = db.Members.Where(x => x.ID == dh.MemberID && x.ValidFlag == true).First();
                if (dh.Member == null)
                {
                    res.ErrMsg = "Member doesn't exist!";
                    return res;
                }
                dh.DepositRewardRule = db.DepositRewardRule.Where(x => x.ValidFlag == true).OrderBy(x => x.DepositAmount).ToList();
                dh.computeAll();
            }
            catch (Exception e)
            {
                res.ErrMsg = e.Message;
                return res;
            }
            res.IsDepositSuccess = false;
            res.TotalDepositMoney = dh.TotalDepositMoney;
            res.DepositPoint = dh.DepositPoint;
            res.TotalPoint = dh.TotalPoint;
            res.DepositRewardPoint = dh.DepositRewardPoint;
            res.AvgPointCost = dh.AvgPointCost;

            return res;
        }
    }
}
