using log4net;
using LoveMeHandMake2.Helper.ExcelReport;
using LoveMeHandMake2.Models;
using LoveMeHandMake2.Models.ViewModels;
using LoveMeHandMake2.Services.report;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LoveMeHandMake2.Controllers.WebControllers.Reports
{
    [Authorize]
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
            try
            {
                TeacherPerformanceSummaryReportService service = new TeacherPerformanceSummaryReportService(this.db);
                model = service.GetModelData(model.SearchStoreID, model.SearchDateStart, model.SearchDateEnd);
            }
            catch (Exception e)
            {
                log.Error(null, e);
                ViewBag.ErrorMessage = e.Message;
            }

            ViewBag.StoreList = DropDownListHelper.GetStoreListWithEmpty(true);
            return View(model);
        }

        [HttpGet]
        public ActionResult DownloadReport(int? SearchStoreID, DateTime? SearchDateStart, DateTime? SearchDateEnd)
        {
            MemoryStream memoryStream = new MemoryStream();
            try
            {
                TeacherPerformanceSummaryReportService service = new TeacherPerformanceSummaryReportService(this.db);
                TeacherPerformanceSummaryReportViewModel model = service.GetModelData(SearchStoreID, SearchDateStart, SearchDateEnd);
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