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
    public class DepositReportController : Controller
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(DepositReportController));
        private LoveMeHandMakeContext db = new LoveMeHandMakeContext();

        // GET: DepositReport
        public ActionResult Index()
        {
            ViewBag.StoreList = DropDownListHelper.GetStoreListWithEmpty(true);
            return View();
        }

        [HttpPost]
        public ActionResult Index(DepositReportViewModel model)
        {
            model = GetModelData(model.SearchStoreID, model.SearchDateStart, model.SearchDateEnd);
            ViewBag.StoreList = DropDownListHelper.GetStoreListWithEmpty(true);
            return View(model);
        }

        private DepositReportViewModel GetModelData(int? SearchStoreID, DateTime? SearchDateStart, DateTime? SearchDateEnd)
        {
            DepositReportViewModel model = new DepositReportViewModel();
            model.SearchStoreID = SearchStoreID;
            model.SearchDateStart = SearchDateStart;
            model.SearchDateEnd = SearchDateEnd;
            model.DepositList =
            (
                from dh in db.DepositHistory
                where (SearchStoreID == null ? true : dh.DepositStoreID == SearchStoreID)
                   && (SearchDateStart == null ? true : SearchDateStart <= dh.DepostitDateTime)
                   && (SearchDateEnd == null ? true : dh.DepostitDateTime <= SearchDateEnd)
                   && (dh.ValidFlag == true)
                orderby dh.DepostitDateTime
                select new DepositRecord { 
                    DepositTime = dh.DepostitDateTime,
                    MemberName = dh.Member.Name,
                    MemberBirthday = dh.Member.Birthday,
                    MemberGender = dh.Member.Gender,
                    Point = dh.DepositPoint,
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
                   && (SearchDateEnd == null ? true : dh.DepostitDateTime <= SearchDateEnd)
                   && (dh.ValidFlag == true)
                orderby dh.DepositTeacherID
                group new { Point = dh.DepositPoint } by new { t.ID, t.Name } into g
                select new TeacherSalesPerformance
                {
                    TeacherName = g.Key.Name,
                    Point = g.Sum(x => x.Point)
                }
            ).ToList();

            return model;
        }
   
        [HttpGet]
        public ActionResult DownloadReport(int? SearchStoreID, DateTime? SearchDateStart, DateTime? SearchDateEnd)
        {
            DepositReportViewModel model = GetModelData(SearchStoreID, SearchDateStart, SearchDateEnd);
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