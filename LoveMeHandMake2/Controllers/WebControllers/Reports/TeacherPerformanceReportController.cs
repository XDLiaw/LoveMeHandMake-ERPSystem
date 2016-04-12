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
    public class TeacherPerformanceReportController : Controller
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(TeacherPerformanceReportController));
        private LoveMeHandMakeContext db = new LoveMeHandMakeContext();

        // GET: TeacherPerformanceReport
        public ActionResult Index()
        {
            TeacherPerformanceReportViewModel model = new TeacherPerformanceReportViewModel();
            model.SearchYearMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            model.SearchDateStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            model.SearchDateEnd = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            ViewBag.TeacherList = DropDownListHelper.GetTeacherListWithEmpty(true);
            ViewBag.StoreList = DropDownListHelper.GetStoreListWithEmpty(true);
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(TeacherPerformanceReportViewModel model)
        {
            try
            {
                TeacherPerformanceReportService service = new TeacherPerformanceReportService(this.db);
                model = service.GetModelData(model.SearchStoreID, model.SearchTeacherID, model.SearchYearMonth);
            }
            catch (Exception e)
            {
                log.Error(null, e);
                ViewBag.ErrorMessage = e.Message;
            }  
            
            ViewBag.TeacherList = DropDownListHelper.GetTeacherListWithEmpty(true);
            ViewBag.StoreList = DropDownListHelper.GetStoreListWithEmpty(true);
            return View(model);
        }

        [HttpGet]
        public ActionResult DownloadReport(int? SearchStoreID, int? SearchTeacherID, DateTime SearchYearMonth)
        {
            MemoryStream memoryStream = new MemoryStream();
            try
            {
                TeacherPerformanceReportService service = new TeacherPerformanceReportService(this.db);
                TeacherPerformanceReportViewModel model = service.GetModelData(SearchStoreID, SearchTeacherID, SearchYearMonth);
                TeacherPerformanceExcelReport report = new TeacherPerformanceExcelReport();
                IWorkbook wb = report.Create(model);
                wb.Write(memoryStream);
            }
            catch (Exception e)
            {
                log.Error(null, e);
            }

            return File(memoryStream.ToArray(), "application/vnd.ms-excel", "人员销售纪录表.xlsx");
        }
    }
}