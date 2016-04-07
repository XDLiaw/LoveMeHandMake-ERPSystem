using log4net;
using LoveMeHandMake2.Helper.ExcelReport;
using LoveMeHandMake2.Models;
using LoveMeHandMake2.Models.ViewModels;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LoveMeHandMake2.Controllers.WebControllers.Reports
{
    public class TeacherPerformanceSummaryReportController : Controller
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(TeacherPerformanceSummaryReportController));
        private LoveMeHandMakeContext db = new LoveMeHandMakeContext();

        // GET: TeacherPerformanceSummaryReport
        [HttpGet]
        public ActionResult Index()
        {
            TeacherPerformanceSummaryReportViewModel model = new TeacherPerformanceSummaryReportViewModel();
            model.SearchDateStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            model.SearchDateEnd = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            ViewBag.StoreList = DropDownListHelper.GetStoreListWithEmpty(true);
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(TeacherPerformanceSummaryReportViewModel model)
        {
            model = GetModelData(model.SearchStoreID, model.SearchDateStart, model.SearchDateEnd);
            ViewBag.StoreList = DropDownListHelper.GetStoreListWithEmpty(true);
            return View(model);
        }

        private TeacherPerformanceSummaryReportViewModel GetModelData(int? SearchStoreID, DateTime? SearchDateStart, DateTime? SearchDateEnd)
        {
            TeacherPerformanceSummaryReportViewModel model = new TeacherPerformanceSummaryReportViewModel();
            model.SearchStoreID = SearchStoreID;
            model.SearchDateStart = SearchDateStart.GetValueOrDefault().Date;
            model.SearchDateEnd = SearchDateEnd.GetValueOrDefault().Date;
            try
            {
                model.StoreName = db.Stores.Where(x => x.ID == model.SearchStoreID && x.ValidFlag == true).Select(x => x.Name).FirstOrDefault();
                var depositDatas =
                (
                    from dh in db.DepositHistory
                    where (SearchStoreID == null ? true : dh.DepositStoreID == SearchStoreID)
                       && (SearchDateStart == null ? true : SearchDateStart <= dh.DepostitDateTime)
                       && (SearchDateEnd == null ? true : dh.DepostitDateTime <= SearchDateEnd)
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
                       && (SearchDateEnd == null ? true : t.TradeDateTime <= SearchDateEnd)
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

        [HttpGet]
        public ActionResult DownloadReport(int? SearchStoreID, DateTime? SearchDateStart, DateTime? SearchDateEnd)
        {
            TeacherPerformanceSummaryReportViewModel model = GetModelData(SearchStoreID, SearchDateStart, SearchDateEnd);
            MemoryStream memoryStream = new MemoryStream();
            try
            {
                TeacherPerformanceSummaryExcelReport report = new TeacherPerformanceSummaryExcelReport();
                IWorkbook wb = report.Create(model);
                wb.Write(memoryStream);
            }
            catch (Exception e)
            {
                log.Error(null, e);
            }

            return File(memoryStream.ToArray(), "application/vnd.ms-excel", "人员销售统计表.xlsx");
        } 
    }
}