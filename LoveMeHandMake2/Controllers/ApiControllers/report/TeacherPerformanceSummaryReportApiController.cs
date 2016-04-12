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
    public class TeacherPerformanceSummaryReportApiController : ApiController
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(TeacherPerformanceSummaryReportApiController));

        [HttpPost]
        public ReportResultApiModel DownloadReport(TeacherPerformanceSummaryReportApiModel arg)
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
                TeacherPerformanceSummaryReportService service = new TeacherPerformanceSummaryReportService();
                TeacherPerformanceSummaryReportViewModel model = service.GetModelData(arg.SearchStoreID, arg.SearchDateStart, arg.SearchDateEnd);
                MemoryStream memoryStream = new MemoryStream();
                TeacherPerformanceSummaryExcelReport report = new TeacherPerformanceSummaryExcelReport();
                IWorkbook wb = report.Create(model);
                wb.Write(memoryStream);
                res.file = memoryStream.ToArray();
                res.contentType = "application/vnd.ms-excel";
                res.fileDownloadName = "人员销售统计表.xlsx";
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
