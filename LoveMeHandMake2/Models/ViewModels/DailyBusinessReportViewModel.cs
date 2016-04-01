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
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? SearchDateStart { get; set; }

        [Display(Name = "日期(讫)")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? SearchDateEnd { get; set; }

        [Display(Name = "门市名称")]
        public string StoreName { get; set; }

        [Display(Name = "总业绩")]
        public double TotalMoney { get; set; }

        public List<DailyBusinessRecord> DailyRecords { get; set; }

        public void computeTotalMoeny()
        {
            this.TotalMoney = 0;
            foreach (DailyBusinessRecord r in this.DailyRecords)
            {
                this.TotalMoney += r.Total;
            }
        }
    }

    public class DailyBusinessRecord 
    {
        public int Year {get; set;}

        public int Month { get; set; }

        public int Day { get; set; }

        [Display(Name = "日期")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd (dddd)}")]
        public DateTime Date { get; set; }

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