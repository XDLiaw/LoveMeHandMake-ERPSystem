using LoveMeHandMake2.Models;
using LoveMeHandMake2.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Services.report
{
    public class TeacherPerformanceReportService : BaseService
    {
        public TeacherPerformanceReportService() : base() { }

        public TeacherPerformanceReportService(LoveMeHandMakeContext db) : base(db) { }

        //public TeacherPerformanceReportViewModel GetModelData(int? SearchStoreID, int? SearchTeacherID, DateTime SearchYearMonth)
        //{
        //    DateTime start = new DateTime(SearchYearMonth.Year, SearchYearMonth.Month, 1);
        //    DateTime end = start.AddMonths(1).AddDays(-1);
        //    TeacherPerformanceReportViewModel model = GetModelData(SearchStoreID, SearchTeacherID, SearchYearMonth);
        //    model.SearchYearMonth = SearchYearMonth;
        //    return model;
        //}

        public TeacherPerformanceReportViewModel GetModelData(int? SearchStoreID, int? SearchTeacherID, DateTime SearchYearMonth)
        {
            TeacherPerformanceReportViewModel model = new TeacherPerformanceReportViewModel();
            model.SearchStoreID = SearchStoreID;
            model.SearchTeacherID = SearchTeacherID;
            model.SearchYearMonth = SearchYearMonth;
            model.SearchDateStart = new DateTime(SearchYearMonth.Year, SearchYearMonth.Month, 1);
            model.SearchDateEnd = model.SearchDateStart.AddMonths(1).AddDays(-1);
            try
            {
                //if (SearchDateStart == null)
                //{
                //    DateTime minDepositDate = db.DepositHistory.Where(x => x.ValidFlag == true).Min(x => x.DepostitDateTime);
                //    DateTime minTradeDate = db.TradeOrder.Where(x => x.ValidFlag == true).Min(x => x.TradeDateTime);
                //    SearchDateStart = minDepositDate < minTradeDate ? minDepositDate : minTradeDate;
                //    model.SearchDateStart = SearchDateStart.GetValueOrDefault().Date;
                //}
                //if (SearchDateEnd == null)
                //{
                //    DateTime maxDepsitDate = db.DepositHistory.Where(x => x.ValidFlag == true).Max(x => x.DepostitDateTime);
                //    DateTime maxTradeDate = db.TradeOrder.Where(x => x.ValidFlag == true).Max(x => x.TradeDateTime);
                //    SearchDateEnd = maxDepsitDate > maxTradeDate ? maxDepsitDate : maxTradeDate;
                //    model.SearchDateEnd = SearchDateEnd.GetValueOrDefault().Date;
                //}
                if (SearchStoreID != null)
                {
                    model.StoreName = db.Stores.Where(x => x.ID == SearchStoreID).Select(x => x.Name).FirstOrDefault();
                    getBonusSetting(model);
                }
                model.MultiTeacherPerformance = GetMultiTeacherPerformanceData(SearchStoreID, SearchTeacherID, model.SearchDateStart, model.SearchDateEnd);
                model.allTeacherPerformance = GetAllTeacherPerformance(SearchStoreID, SearchTeacherID, model.SearchDateStart, model.SearchDateEnd);
                model.Compute();
            }
            catch (Exception e)
            {
                log.Error(null, e);
            }
            return model;
        }

        private void getBonusSetting(TeacherPerformanceReportViewModel model)
        {
            int month = model.SearchYearMonth.Month;
            switch (month)
            {
                case 1:
                    model.ThresholdPoint = db.Stores.Where(x => x.ID == model.SearchStoreID).Select(x => x.ThresholdPoint1).FirstOrDefault();
                    model.OverThresholdBonus = db.Stores.Where(x => x.ID == model.SearchStoreID).Select(x => x.OverThresholdBonus1).FirstOrDefault();
                    break;
                case 2:
                    model.ThresholdPoint = db.Stores.Where(x => x.ID == model.SearchStoreID).Select(x => x.ThresholdPoint2).FirstOrDefault();
                    model.OverThresholdBonus = db.Stores.Where(x => x.ID == model.SearchStoreID).Select(x => x.OverThresholdBonus2).FirstOrDefault();
                    break;
                case 3:
                    model.ThresholdPoint = db.Stores.Where(x => x.ID == model.SearchStoreID).Select(x => x.ThresholdPoint3).FirstOrDefault();
                    model.OverThresholdBonus = db.Stores.Where(x => x.ID == model.SearchStoreID).Select(x => x.OverThresholdBonus3).FirstOrDefault();
                    break;
                case 4:
                    model.ThresholdPoint = db.Stores.Where(x => x.ID == model.SearchStoreID).Select(x => x.ThresholdPoint4).FirstOrDefault();
                    model.OverThresholdBonus = db.Stores.Where(x => x.ID == model.SearchStoreID).Select(x => x.OverThresholdBonus4).FirstOrDefault();
                    break;
                case 5:
                    model.ThresholdPoint = db.Stores.Where(x => x.ID == model.SearchStoreID).Select(x => x.ThresholdPoint5).FirstOrDefault();
                    model.OverThresholdBonus = db.Stores.Where(x => x.ID == model.SearchStoreID).Select(x => x.OverThresholdBonus5).FirstOrDefault();
                    break;
                case 6:
                    model.ThresholdPoint = db.Stores.Where(x => x.ID == model.SearchStoreID).Select(x => x.ThresholdPoint6).FirstOrDefault();
                    model.OverThresholdBonus = db.Stores.Where(x => x.ID == model.SearchStoreID).Select(x => x.OverThresholdBonus6).FirstOrDefault();
                    break;
                case 7:
                    model.ThresholdPoint = db.Stores.Where(x => x.ID == model.SearchStoreID).Select(x => x.ThresholdPoint7).FirstOrDefault();
                    model.OverThresholdBonus = db.Stores.Where(x => x.ID == model.SearchStoreID).Select(x => x.OverThresholdBonus7).FirstOrDefault();
                    break;
                case 8:
                    model.ThresholdPoint = db.Stores.Where(x => x.ID == model.SearchStoreID).Select(x => x.ThresholdPoint8).FirstOrDefault();
                    model.OverThresholdBonus = db.Stores.Where(x => x.ID == model.SearchStoreID).Select(x => x.OverThresholdBonus8).FirstOrDefault();
                    break;
                case 9:
                    model.ThresholdPoint = db.Stores.Where(x => x.ID == model.SearchStoreID).Select(x => x.ThresholdPoint9).FirstOrDefault();
                    model.OverThresholdBonus = db.Stores.Where(x => x.ID == model.SearchStoreID).Select(x => x.OverThresholdBonus9).FirstOrDefault();
                    break;
                case 10:
                    model.ThresholdPoint = db.Stores.Where(x => x.ID == model.SearchStoreID).Select(x => x.ThresholdPoint10).FirstOrDefault();
                    model.OverThresholdBonus = db.Stores.Where(x => x.ID == model.SearchStoreID).Select(x => x.OverThresholdBonus10).FirstOrDefault();
                    break;
                case 11:
                    model.ThresholdPoint = db.Stores.Where(x => x.ID == model.SearchStoreID).Select(x => x.ThresholdPoint11).FirstOrDefault();
                    model.OverThresholdBonus = db.Stores.Where(x => x.ID == model.SearchStoreID).Select(x => x.OverThresholdBonus11).FirstOrDefault();
                    break;
                case 12:
                    model.ThresholdPoint = db.Stores.Where(x => x.ID == model.SearchStoreID).Select(x => x.ThresholdPoint12).FirstOrDefault();
                    model.OverThresholdBonus = db.Stores.Where(x => x.ID == model.SearchStoreID).Select(x => x.OverThresholdBonus12).FirstOrDefault();
                    break;
            }
        }

        private List<TeacherPerformance> GetMultiTeacherPerformanceData(int? SearchStoreID, int? SearchTeacherID, DateTime? SearchDateStart, DateTime? SearchDateEnd)
        {
            List<TeacherPerformance> MultiTeacherPerformance = new List<TeacherPerformance>();
            DateTime SearchDateEnd_nextDay = SearchDateEnd.GetValueOrDefault().AddDays(1);
            var depositDatas =
            (
                from dh in db.DepositHistory
                where (SearchStoreID == null ? true : dh.DepositStoreID == SearchStoreID)
                   && (SearchTeacherID == null ? true : dh.DepositTeacherID == SearchTeacherID)
                   && (SearchDateStart == null ? true : SearchDateStart <= dh.DepostitDateTime)
                   && (SearchDateEnd == null ? true : dh.DepostitDateTime < SearchDateEnd_nextDay)
                   && (dh.ValidFlag == true)
                orderby dh.DepostitDateTime
                group dh by new
                {
                    TeacherID = dh.DepositTeacherID,
                    Year = dh.DepostitDateTime.Year,
                    Month = dh.DepostitDateTime.Month,
                    Day = dh.DepostitDateTime.Day
                } into g
                select new
                {
                    TeacherID = g.Key.TeacherID,
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Day = g.Key.Day,
                    SalesPoints = (double)g.Sum(x => x.DepositPoint)
                }
            ).ToList();
            var TradeDatas =
            (
                from tpp in db.TradePurchaseProduct
                join t in db.TradeOrder on tpp.OrderID equals t.ID
                where (SearchStoreID == null ? true : t.StoreID == SearchStoreID)
                   && (SearchTeacherID == null ? true : t.TeacherID == SearchTeacherID)
                   && (SearchDateStart == null ? true : SearchDateStart <= t.TradeDateTime)
                   && (SearchDateEnd == null ? true : t.TradeDateTime < SearchDateEnd_nextDay)
                   && (t.ValidFlag == true)
                orderby t.TradeDateTime
                group new { tpp.Amount, tpp.TotalPoint, t.MemberID } by new
                {
                    TeacherID = t.TeacherID,
                    Year = t.TradeDateTime.Year,
                    Month = t.TradeDateTime.Month,
                    Day = t.TradeDateTime.Day
                } into g
                select new
                {
                    TeacherID = g.Key.TeacherID,
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Day = g.Key.Day,
                    TeachTimes = g.Sum(x => x.Amount),
                    TeachPoints = g.Sum(x => x.TotalPoint),
                    PointsFromNonMember = g.Sum(x => x.MemberID.HasValue ? 0 : x.TotalPoint)
                }
            ).ToList();
            var TeacherList = db.Teachers.Where(x => x.ValidFlag && (SearchTeacherID == null ? true : x.ID == SearchTeacherID)).ToList();
            var defaultDepositData = new { TeacherID = -1, Year = -1, Month = -1, Day = -1, SalesPoints = 0.0 };
            var defaultTradeData = new { TeacherID = -1, Year = -1, Month = -1, Day = -1, TeachTimes = 0, TeachPoints = 0.0, PointsFromNonMember = 0.0 };
            foreach (Teacher t in TeacherList)
            {
                TeacherPerformance tp = new TeacherPerformance();
                tp.TeacherID = t.ID;
                tp.TeacherName = t.Name;
                tp.DailyPerformanceList = new List<TeacherDailyPerformance>();
                for (DateTime d = SearchDateStart.GetValueOrDefault(); d < SearchDateEnd_nextDay; d = d.AddDays(1))
                {
                    var depositData = depositDatas.Where(x => x.TeacherID == t.ID && x.Year == d.Year && x.Month == d.Month && x.Day == d.Day).FirstOrDefault();
                    var tradeData = TradeDatas.Where(x => x.TeacherID == t.ID && x.Year == d.Year && x.Month == d.Month && x.Day == d.Day).FirstOrDefault();
                    if (depositData == null) depositData = defaultDepositData;
                    if (tradeData == null) tradeData = defaultTradeData;
                    TeacherDailyPerformance tdp = new TeacherDailyPerformance();
                    tdp.Date = d;
                    tdp.TeachTimes = tradeData.TeachTimes;
                    tdp.TeachPoints = tradeData.TeachPoints;
                    tdp.SalesPoints = depositData.SalesPoints;
                    tdp.PointsFromNonMember = tradeData.PointsFromNonMember;
                    tp.DailyPerformanceList.Add(tdp);
                }
                tp.Compute();
                if (tp.TotalTeachTimes + tp.TotalTeachPoints + tp.TotalSalesPoints + tp.TotalPointsFromNonMember > 0)
                {
                    MultiTeacherPerformance.Add(tp);
                }
            }

            return MultiTeacherPerformance;
        }

        private AllTeacherPerformance GetAllTeacherPerformance(int? SearchStoreID, int? SearchTeacherID, DateTime? SearchDateStart, DateTime? SearchDateEnd)
        {
            AllTeacherPerformance atp = new AllTeacherPerformance();
            atp.DailyPerformanceList = new List<TeacherDailyPerformance>();
            DateTime SearchDateEnd_nextDay = SearchDateEnd.GetValueOrDefault().AddDays(1);
            var depositDatas =
            (
                from dh in db.DepositHistory
                where (SearchStoreID == null ? true : dh.DepositStoreID == SearchStoreID)
                   && (SearchTeacherID == null ? true : dh.DepositTeacherID == SearchTeacherID)
                   && (SearchDateStart == null ? true : SearchDateStart <= dh.DepostitDateTime)
                   && (SearchDateEnd == null ? true : dh.DepostitDateTime < SearchDateEnd_nextDay)
                   && (dh.ValidFlag == true)
                orderby dh.DepostitDateTime
                group dh by new
                {
                    Year = dh.DepostitDateTime.Year,
                    Month = dh.DepostitDateTime.Month,
                    Day = dh.DepostitDateTime.Day
                } into g
                select new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Day = g.Key.Day,
                    SalesPoints = (double)g.Sum(x => x.DepositPoint)
                }
            ).ToList();
            var TradeDatas =
            (
                from tpp in db.TradePurchaseProduct
                join t in db.TradeOrder on tpp.OrderID equals t.ID
                where (SearchStoreID == null ? true : t.StoreID == SearchStoreID)
                   && (SearchTeacherID == null ? true : t.TeacherID == SearchTeacherID)
                   && (SearchDateStart == null ? true : SearchDateStart <= t.TradeDateTime)
                   && (SearchDateEnd == null ? true : t.TradeDateTime < SearchDateEnd_nextDay)
                   && (t.ValidFlag == true)
                orderby t.TradeDateTime
                group new { tpp.Amount, tpp.TotalPoint, t.MemberID } by new
                {
                    Year = t.TradeDateTime.Year,
                    Month = t.TradeDateTime.Month,
                    Day = t.TradeDateTime.Day
                } into g
                select new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Day = g.Key.Day,
                    TeachTimes = g.Sum(x => x.Amount),
                    TeachPoints = g.Sum(x => x.TotalPoint),
                    PointsFromNonMember = g.Sum(x => x.MemberID.HasValue ? 0 : x.TotalPoint)
                }
            ).ToList();
            var defaultDepositData = new { Year = -1, Month = -1, Day = -1, SalesPoints = 0.0 };
            var defaultTradeData = new { Year = -1, Month = -1, Day = -1, TeachTimes = 0, TeachPoints = 0.0, PointsFromNonMember = 0.0 };
            for (DateTime d = SearchDateStart.GetValueOrDefault(); d < SearchDateEnd_nextDay; d = d.AddDays(1))
            {
                var depositData = depositDatas.Where(x => x.Year == d.Year && x.Month == d.Month && x.Day == d.Day).FirstOrDefault();
                var tradeData = TradeDatas.Where(x => x.Year == d.Year && x.Month == d.Month && x.Day == d.Day).FirstOrDefault();
                if (depositData == null) depositData = defaultDepositData;
                if (tradeData == null) tradeData = defaultTradeData;
                TeacherDailyPerformance tdp = new TeacherDailyPerformance();
                tdp.Date = d;
                tdp.TeachTimes = tradeData.TeachTimes;
                tdp.TeachPoints = tradeData.TeachPoints;
                tdp.SalesPoints = depositData.SalesPoints;
                tdp.PointsFromNonMember = tradeData.PointsFromNonMember;
                atp.DailyPerformanceList.Add(tdp);
            }
            atp.Compute();

            return atp;
        }


    }
}