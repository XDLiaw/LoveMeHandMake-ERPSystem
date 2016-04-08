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
    [Authorize]
    public class ProductSaleReportController : Controller
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ProductSaleReportController));
        private LoveMeHandMakeContext db = new LoveMeHandMakeContext();

        public ActionResult Index()
        {
            ProductSaleReportViewModel model = new ProductSaleReportViewModel();
            ViewBag.StoreList = DropDownListHelper.GetStoreListWithEmpty(true);
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(ProductSaleReportViewModel model)
        {
            model = GetModelData(model.SearchStoreID, model.SearchDateStart, model.SearchDateEnd);
            ViewBag.StoreList = DropDownListHelper.GetStoreListWithEmpty(true);
            return View(model);
        }

        private ProductSaleReportViewModel GetModelData(int? storeID, DateTime? dateStart, DateTime? dateEnd)
        {
            ProductSaleReportViewModel model = new ProductSaleReportViewModel();
            model.SearchStoreID = storeID;
            model.SearchDateStart = dateStart;
            model.SearchDateEnd = dateEnd;
            try
            {
                if (storeID != null)
                {
                    model.StoreName = db.Stores.Where(x => x.ID == storeID).Select(x => x.Name).FirstOrDefault();
                }
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

                model.ComputeAll();
            }
            catch (Exception e) {
                log.Error(null, e);
            
            }

            return model;
        }

        [HttpGet]
        public ActionResult DownloadReport(int? SearchStoreID, DateTime? SearchDateStart, DateTime? SearchDateEnd)
        {
            ProductSaleReportViewModel model = GetModelData(SearchStoreID, SearchDateStart, SearchDateEnd);
            MemoryStream memoryStream = new MemoryStream();            
            try
            {
                ProductSaleExcelReport report = new ProductSaleExcelReport();
                IWorkbook wb = report.Create(model);
                wb.Write(memoryStream);
            }
            catch (Exception e)
            {
                log.Error(null, e);
            }

            return File(memoryStream.ToArray(), "application/vnd.ms-excel", "商品销售表.xlsx");
        }


    }
}