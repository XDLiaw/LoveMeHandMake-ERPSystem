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
    public class TeacherPerformanceReportApiController : ApiController
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(DailyBusinessReportApiController));

        [HttpPost]
        public ReportResultApiModel DownloadReport(TeacherPerformanceReportApiModel arg)
        {
            ReportResultApiModel res = new ReportResultApiModel();
            res.ReceiveRequestTime = DateTime.Now;            
            try
            {
                TeacherPerformanceReportService service = new TeacherPerformanceReportService();
                TeacherPerformanceReportViewModel model = service.GetModelData(arg.SearchStoreID, arg.SearchTeacherID, arg.SearchYearMonth);
                TeacherPerformanceExcelReport report = new TeacherPerformanceExcelReport();
                IWorkbook wb = report.Create(model);
                MemoryStream memoryStream = new MemoryStream();
                wb.Write(memoryStream);
                res.file = memoryStream.ToArray();
                res.contentType = "application/vnd.ms-excel";
                res.fileDownloadName = "人员销售纪录表.xlsx";
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
