using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Models.ApiModels.report
{
    public class ProductSaleReportApiModel :BaseRequestApiModel
    {
        public int? SearchStoreID { get; set; }

        public DateTime? SearchDateStart { get; set; }

        public DateTime? SearchDateEnd { get; set; }
    }
}