using log4net;
using LoveMeHandMake2.Helper.ExcelReport;
using LoveMeHandMake2.Models.ApiModels.report;
using LoveMeHandMake2.Models.ViewModels;
using LoveMeHandMake2.Services.report;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LoveMeHandMake2.Controllers.ApiControllers.report
{
    public class ProductSaleReportApiController : ApiController
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ProductSaleReportApiController));

        [HttpPost]
        public ReportResultApiModel DownloadReport(ProductSaleReportApiModel arg)
        {
            ReportResultApiModel res = new ReportResultApiModel();
            res.ReceiveRequestTime = DateTime.Now;
            try
            {
                if (arg.IsValid() == false)
                {
                    log.Warn(arg.GetInvalidReasons());
                    res.ErrMsgs.AddRange(arg.GetInvalidReasons());
                    res.IsRequestSuccess = false;
                    return res;
                }
                ProductSaleReportService service = new ProductSaleReportService();
                ProductSaleReportViewModel model = service.GetModelData(arg.SearchStoreID, null, arg.SearchDateStart, arg.SearchDateEnd);
                MemoryStream memoryStream = new MemoryStream();
                ProductSaleExcelReport report = new ProductSaleExcelReport();
                IWorkbook wb = report.Create(model);
                wb.Write(memoryStream);
                res.file = memoryStream.ToArray();
                res.contentType = "application/vnd.ms-excel";
                res.fileDownloadName = "商品销售表.xlsx";
                res.IsRequestSuccess = true;
            }
            catch (Exception e)
            {
                log.Error(null, e);
                res.ErrMsgs.Add(e.Message);
                res.IsRequestSuccess = false;
            }
            return res;
        }
    }
}
