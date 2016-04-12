using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Models.ApiModels.report
{
    public class ReportResultApiModel : BaseResultApiModel
    {
        public byte[] file { get; set; }

        public string contentType { get; set; }

        public string fileDownloadName { get; set; }
    }
}