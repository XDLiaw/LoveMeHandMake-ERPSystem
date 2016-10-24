using LoveMeHandMake2.Models;
using LoveMeHandMake2.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Services.report
{
    public class DepositReportService : BaseService
    {
        public DepositReportService() : base() { }

        public DepositReportService(LoveMeHandMakeContext db) : base(db) { }

        public DepositReportViewModel GetModelData(int? SearchStoreID, DateTime? SearchDateStart, DateTime? SearchDateEnd)
        {
            DepositReportViewModel model = new DepositReportViewModel();
            model.SearchStoreID = SearchStoreID;
            model.SearchDateStart = SearchDateStart;
            model.SearchDateEnd = SearchDateEnd;
            DateTime SearchDateEnd_nextDay = SearchDateEnd.GetValueOrDefault().AddDays(1);
            try
            {
                if (SearchStoreID != null)
                {
                    model.StoreName = db.Stores.Where(x => x.ID == SearchStoreID).Select(x => x.Name).FirstOrDefault();
                }

                model.DepositList =
                (
                    from dh in db.DepositHistory
                    where (SearchStoreID == null ? true : dh.DepositStoreID == SearchStoreID)
                       && (SearchDateStart == null ? true : SearchDateStart <= dh.DepostitDateTime)
                       && (SearchDateEnd == null ? true : dh.DepostitDateTime < SearchDateEnd_nextDay)
                       && (dh.ValidFlag == true)
                    orderby dh.DepostitDateTime
                    select new DepositRecord
                    {
                        DepositTime = dh.DepostitDateTime,
                        MemberName = dh.Member.Name,
                        MemberBirthday = dh.Member.Birthday,
                        MemberGender = dh.Member.Gender,
                        Point = dh.DepositPoint,
                        RewardPoint = dh.RewardPoint,
                        RewardMoney = dh.RewardMoney,
                        MemberCardID = dh.Member.CardID,
                        TeacherName = dh.DepositTeacher.Name,
                        MemberPhone = dh.Member.Phone
                    }
                ).ToList();

                // First Way to group by data
                //model.TeacherSalesPerformanceList =
                //(
                //    from dh in db.DepositHistory
                //    where (SearchStoreID == null ? true : dh.DepositStoreID == SearchStoreID)
                //       && (SearchDateStart == null ? true : SearchDateStart <= dh.DepostitDateTime)
                //       && (SearchDateEnd == null ? true : dh.DepostitDateTime <= SearchDateEnd)
                //       && (dh.ValidFlag == true)
                //    orderby dh.DepositTeacherID
                //    group dh by new { dh.DepositTeacherID, Name = dh.DepositTeacher.Name } into g
                //    select new TeacherSalesPerformance
                //    {
                //        TeacherName = g.Key.Name,
                //        Point = g.Sum(x => x.DepositPoint)
                //    }
                //).ToList();

                // Second Way to group by data
                model.TeacherSalesPerformanceList =
                (
                    from dh in db.DepositHistory
                    join t in db.Teachers on dh.DepositTeacherID equals t.ID
                    where (SearchStoreID == null ? true : dh.DepositStoreID == SearchStoreID)
                       && (SearchDateStart == null ? true : SearchDateStart <= dh.DepostitDateTime)
                       && (SearchDateEnd == null ? true : dh.DepostitDateTime < SearchDateEnd_nextDay)
                       && (dh.ValidFlag == true)
                    orderby dh.DepositTeacherID
                    group new { Point = dh.DepositPoint } by new { t.ID, t.Name } into g
                    select new TeacherSalesPerformance
                    {
                        TeacherName = g.Key.Name,
                        Point = g.Sum(x => x.Point)
                    }
                ).ToList();
                model.ComputeTotalPoint();
            }
            catch (Exception e)
            {
                log.Error(null, e);
            }
            return model;
        }

    }
}