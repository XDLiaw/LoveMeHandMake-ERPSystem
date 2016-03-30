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
    public class ProductSaleReportController : Controller
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ProductSaleReportController));
        private LoveMeHandMakeContext db = new LoveMeHandMakeContext();

        public ActionResult Index()
        {
            ViewBag.StoreList = DropDownListHelper.GetStoreListWithEmpty(true);
            return View();
        }

        [HttpPost]
        public ActionResult Index(ProductSaleReportViewModel model)
        {
            int? storeID = model.SearchStoreID;
            DateTime? dateStart = model.SearchDateStart;
            DateTime? dateEnd = model.SearchDateEnd;

            model = Index(storeID, dateStart, dateEnd);
            model.SearchStoreID = storeID;
            model.SearchDateStart = dateStart;
            model.SearchDateEnd = dateEnd;
            
            ViewBag.StoreList = DropDownListHelper.GetStoreListWithEmpty(true);
            return View(model);
        }

        private ProductSaleReportViewModel Index(int? storeID, DateTime? dateStart, DateTime? dateEnd)
        {
            ProductSaleReportViewModel model = new ProductSaleReportViewModel();
            model.saleList =
            (
                from tpp in db.TradePurchaseProduct
                join o in db.TradeOrder on tpp.OrderID equals o.ID
                join p in db.Products on tpp.ProductID equals p.ID               
                where (storeID == null ? true : o.StoreID == storeID)
                    && (dateStart == null ? true : dateStart <= o.TradeDateTime)
                    && (dateEnd == null ? true : o.TradeDateTime <= dateEnd)
                    && (o.ValidFlag == true)
                    && (tpp.ValidFlag == true)
                orderby o.TradeDateTime
                select new ProductSaleRecord
                { 
                    TradeDateTime = o.TradeDateTime,
                    ProductName = p.Name,
                    Amount = tpp.Amount,
                    UnitPoint = tpp.UnitPoint,
                    UnitBean = tpp.UnitBean,
                    MemberCardID = (o.Member == null ? null : o.Member.CardID),
                    Sum = tpp.Sum,
                    Gender = (o.Member == null ? null : (bool?)o.Member.Gender),
                    TeacherName = (o.Teacher == null ? null : o.Teacher.Name)
                }
            ).ToList();

            model.ComputeTotalPoint();
            model.ComputeTotalBean();
            model.ComputeTotalMoney();
            model.ComputeTradeTimes();
            model.ComputeAveragePrice();

            return model;
        }

        [HttpGet]
        public ActionResult DownloadReport(int? SearchStoreID, DateTime? SearchDateStart, DateTime? SearchDateEnd)
        {
            DateTime start = SearchDateStart == null ? DateTime.MinValue : SearchDateStart.GetValueOrDefault();
            DateTime end = SearchDateEnd == null ? DateTime.MaxValue : SearchDateEnd.GetValueOrDefault();

            ProductSaleReportViewModel model = Index(SearchStoreID, start, end);
            MemoryStream memoryStream = new MemoryStream();            
            try
            {
                ProductSaleReport report = new ProductSaleReport();
                IWorkbook wb = report.Create(model, start, end);
                wb.Write(memoryStream);
            }
            catch (Exception e)
            {
                log.Error(null, e);
            }

            return File(memoryStream.ToArray(), "application/vnd.ms-excel", "商品銷售表.xlsx");
        }


    }
}