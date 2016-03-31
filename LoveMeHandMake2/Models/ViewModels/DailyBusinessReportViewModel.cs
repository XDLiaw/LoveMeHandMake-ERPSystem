using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Models.ViewModels
{
    public class DailyBusinessReportViewModel
    {
        [Display(Name = "门市")]
        public int? SearchStoreID { get; set; }

        [Display(Name = "日期(起)")]
        public DateTime? SearchDateStart { get; set; }

        [Display(Name = "日期(讫)")]
        public DateTime? SearchDateEnd { get; set; }

        public List<DailyRecord> DailyRecords { get; set; }
    }

    public class DailyRecord 
    {
        [Display(Name = "日期")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime Day { get; set; }

        [Display(Name = "现金")]
        public double Cash { get; set; }

        [Display(Name = "信用卡")]
        public double CreditCard { get; set; }

        [Display(Name = "商城卡")]
        public double MallCard { get; set; }

        [Display(Name = "当日业绩")]
        public double Total { get; set; }
    }
}