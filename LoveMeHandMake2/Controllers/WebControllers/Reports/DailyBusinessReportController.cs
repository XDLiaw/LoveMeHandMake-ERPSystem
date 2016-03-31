using log4net;
using LoveMeHandMake2.Models;
using LoveMeHandMake2.Models.ViewModels;
using System;
using System.Collections.Generic;
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
            ViewBag.StoreList = DropDownListHelper.GetStoreListWithEmpty(true);
            return View();
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




            throw new NotImplementedException();
        }




    }
}