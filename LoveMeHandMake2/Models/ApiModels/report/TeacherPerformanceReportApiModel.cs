using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Models.ApiModels.report
{
    public class TeacherPerformanceReportApiModel : BaseRequestApiModel
    {
        public int? SearchStoreID { get; set; }

        public int? SearchTeacherID { get; set; }

        public DateTime SearchYearMonth { get; set; }
    }
}