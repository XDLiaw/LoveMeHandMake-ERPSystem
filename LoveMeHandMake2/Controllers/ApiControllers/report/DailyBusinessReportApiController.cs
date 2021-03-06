﻿using log4net;
using LoveMeHandMake2.Helper.ExcelReport;
using LoveMeHandMake2.Models;
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
    public class DailyBusinessReportApiController : ApiController
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(DailyBusinessReportApiController));

        [HttpPost]
        public ReportResultApiModel DownloadReport(DailyBusinessReportApiModel arg)
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
                DailyBusinessReportService service = new DailyBusinessReportService();
                DailyBusinessReportViewModel model = service.GetModelData(arg.SearchStoreID, arg.SearchDateStart, arg.SearchDateEnd);
                MemoryStream memoryStream = new MemoryStream();
                DailyBusinessExcelReport report = new DailyBusinessExcelReport();
                IWorkbook wb = report.Create(model);
                wb.Write(memoryStream);
                res.file = memoryStream.ToArray();
                res.contentType = "application/vnd.ms-excel";
                res.fileDownloadName = "营业日报表.xlsx";
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
