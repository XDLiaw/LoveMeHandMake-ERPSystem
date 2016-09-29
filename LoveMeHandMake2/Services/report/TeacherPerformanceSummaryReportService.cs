using LoveMeHandMake2.Models;
using LoveMeHandMake2.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Services.report
{
    public class TeacherPerformanceSummaryReportService : BaseService
    {
        public TeacherPerformanceSummaryReportService() : base() { }

        public TeacherPerformanceSummaryReportService(LoveMeHandMakeContext db) : base(db) { }

        public TeacherPerformanceSummaryReportViewModel GetModelData(int? SearchStoreID, DateTime? SearchDateStart, DateTime? SearchDateEnd)
        {
            TeacherPerformanceSummaryReportViewModel model = new TeacherPerformanceSummaryReportViewModel();
            model.SearchStoreID = SearchStoreID;
            model.SearchDateStart = SearchDateStart;
            model.SearchDateEnd = SearchDateEnd;
            DateTime SearchDateEnd_nextDay = SearchDateEnd.GetValueOrDefault().AddDays(1);
            try
            {
                model.StoreName = db.Stores.Where(x => x.ID == model.SearchStoreID && x.ValidFlag == true).Select(x => x.Name).FirstOrDefault();
                var depositDatas =
                (
                    from dh in db.DepositHistory
                    where (SearchStoreID == null ? true : dh.DepositStoreID == SearchStoreID)
                       && (SearchDateStart == null ? true : SearchDateStart <= dh.DepostitDateTime)
                       && (SearchDateEnd == null ? true : dh.DepostitDateTime < SearchDateEnd_nextDay)
                       && (dh.ValidFlag == true)
                    group dh by new { dh.DepositTeacherID, dh.DepositTeacher.Name } into g
                    select new TeacherPerformanceSummary
                    {
                        TeacherID = g.Key.DepositTeacherID,
                        TeacherName = g.Key.Name,
                        SalesPoints = g.Sum(x => x.DepositPoint)
                    }
                ).ToList();
                var tradeDatas =
                (
                    from tpp in db.TradePurchaseProduct
                    join t in db.TradeOrder on tpp.OrderID equals t.ID
                    where (SearchStoreID == null ? true : t.StoreID == SearchStoreID)
                       && (SearchDateStart == null ? true : SearchDateStart <= t.TradeDateTime)
                       && (SearchDateEnd == null ? true : t.TradeDateTime < SearchDateEnd_nextDay)
                       && (t.ValidFlag == true)
                    group tpp by new { tpp.Order.TeacherID, tpp.Order.Teacher.Name } into g
                    select new TeacherPerformanceSummary
                    {
                        TeacherID = g.Key.TeacherID,
                        TeacherName = g.Key.Name,
                        TeachTimes = g.Sum(x => x.Amount),
                        TeachPoints = g.Sum(x => x.TotalPoint),
                        PointsFromNonMember = g.Sum(x => x.Order.MemberID.HasValue ? 0 : x.TotalPoint),
                        TotalPrice = g.Sum(x => x.Sum)
                    }
                );
                TeacherPerformanceSummary defaultTPS = new TeacherPerformanceSummary
                {
                    SalesPoints = 0,
                    TeachTimes = 0,
                    TeachPoints = 0,
                    PointsFromNonMember = 0,
                    TotalPrice = 0
                };
                List<Teacher> TeachList = db.Teachers.Where(x => x.ValidFlag).ToList();
                model.TeacherPerformanceSummaryList = new List<TeacherPerformanceSummary>();
                foreach (Teacher t in TeachList)
                {
                    var depositData = depositDatas.Where(x => x.TeacherID == t.ID).FirstOrDefault();
                    var tradeData = tradeDatas.Where(x => x.TeacherID == t.ID).FirstOrDefault();
                    if (depositData == null) depositData = defaultTPS;
                    if (tradeData == null) tradeData = defaultTPS;
                    TeacherPerformanceSummary tps = new TeacherPerformanceSummary();
                    tps.TeacherID = t.ID;
                    tps.TeacherName = t.Name;
                    tps.TeachTimes = tradeData.TeachTimes;
                    tps.TeachPoints = tradeData.TeachPoints;
                    tps.SalesPoints = depositData.SalesPoints;
                    tps.PointsFromNonMember = tradeData.PointsFromNonMember;
                    tps.TotalPrice = tradeData.TotalPrice;
                    model.TeacherPerformanceSummaryList.Add(tps);
                }
                model.Compute();
            }
            catch (Exception e)
            {
                log.Error(null, e);
            }
            return model;
        }
    }
}