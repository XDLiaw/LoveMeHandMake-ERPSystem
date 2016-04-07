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
    public class DailyBusinessReportController : Controller
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(DailyBusinessReportController));
        private LoveMeHandMakeContext db = new LoveMeHandMakeContext();

        // GET: DailyBusinessReport
        public ActionResult Index()
        {
            DailyBusinessReportViewModel model = new DailyBusinessReportViewModel();
            model.SearchDateStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            model.SearchDateEnd = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            ViewBag.StoreList = DropDownListHelper.GetStoreListWithEmpty(true);
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(DailyBusinessReportViewModel model)
        {
            model = GetModelData(model.SearchStoreID, model.SearchDateStart, model.SearchDateEnd);
            ViewBag.StoreList = DropDownListHelper.GetStoreListWithEmpty(true);
            return View(model);
        }

        private DailyBusinessReportViewModel GetModelData(int? SearchStoreID, DateTime? SearchDateStart, DateTime? SearchDateEnd)
        {
            DailyBusinessReportViewModel model = new DailyBusinessReportViewModel();
            model.SearchStoreID = SearchStoreID;
            model.SearchDateStart = SearchDateStart;
            model.SearchDateEnd = SearchDateEnd;
            try
            {
                if (SearchStoreID != null)
                {
                    model.StoreName = db.Stores.Where(x => x.ID == SearchStoreID).Select(x => x.Name).FirstOrDefault();
                }
                if (SearchDateStart == null)
                {
                    DateTime minDepositDate = db.DepositHistory.Where(x => x.ValidFlag == true).Min(x => x.DepostitDateTime);
                    DateTime minTradeDate = db.TradeOrder.Where(x => x.ValidFlag == true).Min(x => x.TradeDateTime);
                    SearchDateStart = minDepositDate < minTradeDate ? minDepositDate : minTradeDate;
                    model.SearchDateStart = SearchDateStart.GetValueOrDefault().Date;
                }
                if (SearchDateEnd == null)
                {
                    DateTime maxDepsitDate = db.DepositHistory.Where(x => x.ValidFlag == true).Max(x => x.DepostitDateTime);
                    DateTime maxTradeDate = db.TradeOrder.Where(x => x.ValidFlag == true).Max(x => x.TradeDateTime);
                    SearchDateEnd = maxDepsitDate > maxTradeDate ? maxDepsitDate : maxTradeDate;
                    model.SearchDateEnd = SearchDateEnd.GetValueOrDefault().Date;
                }

                var depositDatas =
                (
                    from dh in db.DepositHistory
                    where (SearchStoreID == null ? true : dh.DepositStoreID == SearchStoreID)
                       && (model.SearchDateStart == null ? true : model.SearchDateStart <= dh.DepostitDateTime)
                       && (model.SearchDateEnd == null ? true : dh.DepostitDateTime <= model.SearchDateEnd)
                       && (dh.ValidFlag == true)
                    orderby dh.DepostitDateTime
                    group dh by new { Year = dh.DepostitDateTime.Year, Month = dh.DepostitDateTime.Month, Day = dh.DepostitDateTime.Day } into g
                    select new DailyBusinessRecord
                    {
                        Year = g.Key.Year,
                        Month = g.Key.Month,
                        Day = g.Key.Day,
                        Cash = g.Sum(x => x.Cash.HasValue ? x.Cash.Value : 0),
                        CreditCard = g.Sum(x => x.CreditCard.HasValue ? x.CreditCard.Value : 0),
                        MallCard = g.Sum(x => x.MallCard.HasValue ? x.MallCard.Value : 0),
                    }
                ).ToList();
                var tradeDatas =
                (
                    from t in db.TradeOrder
                    where (SearchStoreID == null ? true : t.StoreID == SearchStoreID)
                       && (model.SearchDateStart == null ? true : model.SearchDateStart <= t.TradeDateTime)
                       && (model.SearchDateEnd == null ? true : t.TradeDateTime <= model.SearchDateEnd)
                       && (t.ValidFlag == true)
                    orderby t.TradeDateTime
                    group t by new { Year = t.TradeDateTime.Year, Month = t.TradeDateTime.Month, Day = t.TradeDateTime.Day } into g
                    select new DailyBusinessRecord {
                        Year = g.Key.Year,
                        Month = g.Key.Month,
                        Day = g.Key.Day,
                        Cash = g.Sum(x => x.ChargeByCash),
                        CreditCard = g.Sum(x => x.ChargeByCreditCard),
                        MallCard = g.Sum(x => x.ChargeByMallCard)
                    }
                ).ToList();
                model.DailyRecords = new List<DailyBusinessRecord>();
                DailyBusinessRecord defaultRecord = new DailyBusinessRecord { Cash = 0, CreditCard = 0, Month = 0 };
                for (DateTime d = model.SearchDateStart.GetValueOrDefault(); d <= model.SearchDateEnd; d = d.AddDays(1))
                {
                    DailyBusinessRecord dr = depositDatas.Where(x => x.Year == d.Year && x.Month == d.Month && x.Day == d.Day).FirstOrDefault();
                    DailyBusinessRecord tr = tradeDatas.Where(x => x.Year == d.Year && x.Month == d.Month && x.Day == d.Day).FirstOrDefault();
                    if (dr == null) dr = defaultRecord;
                    if (tr == null) tr = defaultRecord;
                    DailyBusinessRecord record = new DailyBusinessRecord();
                    record.Date = d;
                    record.Cash = dr.Cash + tr.Cash;
                    record.CreditCard = dr.CreditCard + tr.CreditCard;
                    record.MallCard = dr.MallCard + tr.MallCard;
                    record.Total = record.Cash + record.CreditCard + record.MallCard;
                    model.DailyRecords.Add(record);
                }
                model.computeTotalMoeny();
            }
            catch (Exception e) {
                log.Error(null, e);
            }
            return model;
        }


        [HttpGet]
        public ActionResult DownloadReport(int? SearchStoreID, DateTime? SearchDateStart, DateTime? SearchDateEnd)
        {
            DailyBusinessReportViewModel model = GetModelData(SearchStoreID, SearchDateStart, SearchDateEnd);
            MemoryStream memoryStream = new MemoryStream();
            try
            {
                DailyBusinessExcelReport report = new DailyBusinessExcelReport();
                IWorkbook wb = report.Create(model);
                wb.Write(memoryStream);
            }
            catch (Exception e)
            {
                log.Error(null, e);
            }

            return File(memoryStream.ToArray(), "application/vnd.ms-excel", "营业日报表.xlsx");
        }

    }
}