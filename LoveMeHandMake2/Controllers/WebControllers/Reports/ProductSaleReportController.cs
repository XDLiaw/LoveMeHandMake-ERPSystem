﻿using log4net;
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
            ProductSaleReportService service = new ProductSaleReportService(this.db);
            model = service.GetModelData(model.SearchStoreID, model.SearchDateStart, model.SearchDateEnd);
            ViewBag.StoreList = DropDownListHelper.GetStoreListWithEmpty(true);
            return View(model);
        }

        [HttpGet]
        public ActionResult DownloadReport(int? SearchStoreID, DateTime? SearchDateStart, DateTime? SearchDateEnd)
        {
            ProductSaleReportService service = new ProductSaleReportService(this.db);
            ProductSaleReportViewModel model = service.GetModelData(SearchStoreID, SearchDateStart, SearchDateEnd);
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