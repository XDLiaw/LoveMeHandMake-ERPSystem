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
    public class DepositReportController : Controller
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(DepositReportController));
        private LoveMeHandMakeContext db = new LoveMeHandMakeContext();

        // GET: DepositReport
        [HttpGet]
        public ActionResult Index()
        {
            DepositReportViewModel model = new DepositReportViewModel();
            ViewBag.StoreList = DropDownListHelper.GetStoreListWithEmpty(true);
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(DepositReportViewModel model)
        {
            DepositReportService service = new DepositReportService(this.db);
            model = service.GetModelData(model.SearchStoreID, model.SearchDateStart, model.SearchDateEnd);
            ViewBag.StoreList = DropDownListHelper.GetStoreListWithEmpty(true);
            return View(model);
        }
   
        [HttpGet]
        public ActionResult DownloadReport(int? SearchStoreID, DateTime? SearchDateStart, DateTime? SearchDateEnd)
        {
            DepositReportService service = new DepositReportService(this.db);
            DepositReportViewModel model = service.GetModelData(SearchStoreID, SearchDateStart, SearchDateEnd);
            MemoryStream memoryStream = new MemoryStream();
            try
            {
                DepositExcelReport report = new DepositExcelReport();
                IWorkbook wb = report.Create(model);
                wb.Write(memoryStream);
            }
            catch (Exception e)
            {
                log.Error(null, e);
            }

            return File(memoryStream.ToArray(), "application/vnd.ms-excel", "会员点数销售表＆通讯录.xlsx");
        }
    
    
    }
}