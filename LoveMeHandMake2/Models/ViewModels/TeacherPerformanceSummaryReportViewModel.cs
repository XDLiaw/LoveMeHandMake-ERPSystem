using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Models.ViewModels
{
    public class TeacherPerformanceSummaryReportViewModel
    {
        [Display(Name = "门市")]
        public int? SearchStoreID { get; set; }

        [Display(Name = "日期(起)")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? SearchDateStart { get; set; }

        [Display(Name = "日期(讫)")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? SearchDateEnd { get; set; }

        //--------------------------------------------------------------------------

        [Display(Name = "门市名称")]
        public string StoreName { get; set; }        

        public List<TeacherPerformanceSummary> TeacherPerformanceSummaryList { get; set; }

        [Display(Name = "来客数")]
        public int TotalTeachTimes { get; set; }

        // 平均客單價 = 總消費金額 / 來客數
        [Display(Name = "平均客单价")]
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        public double AvgPrice { get; set; }

        // 办卡率 = 會員消費點數 / (會員消費點數 + 非換元消費點數)
        [Display(Name = "办卡率")]
        public double MemberConsumptionPercentage { get; set; }

        public TeacherPerformanceSummaryReportViewModel()
        {
            this.TeacherPerformanceSummaryList = new List<TeacherPerformanceSummary>();
        }

        public void Compute()
        {
            this.TotalTeachTimes = this.TeacherPerformanceSummaryList.Sum(x => x.TeachTimes);
            this.AvgPrice = this.TeacherPerformanceSummaryList.Sum(x => x.TotalPrice) / this.TotalTeachTimes;
            double NonMemberConsumptionPoint = this.TeacherPerformanceSummaryList.Sum(x => x.PointsFromNonMember);
            double TotalPoint = this.TeacherPerformanceSummaryList.Sum(x => x.TeachPoints);
            double MemberConsumptionPoint = TotalPoint - NonMemberConsumptionPoint;
            this.MemberConsumptionPercentage = MemberConsumptionPoint / TotalPoint;
        }
    }

    public class TeacherPerformanceSummary 
    {
        public int TeacherID { get; set; }

        [Display(Name = "人员")]
        public string TeacherName { get; set; }

        [Display(Name = "教学次数")]
        public int TeachTimes { get; set; }

        [Display(Name = "教学点数")]
        [DisplayFormat(DataFormatString = "{0:F1}", ApplyFormatInEditMode = true)]
        public double TeachPoints { get; set; }

        [Display(Name = "销售点数")]
        [DisplayFormat(DataFormatString = "{0:F1}", ApplyFormatInEditMode = true)]
        public double SalesPoints { get; set; }

        [Display(Name = "单做点数")]
        [DisplayFormat(DataFormatString = "{0:F1}", ApplyFormatInEditMode = true)]
        public double PointsFromNonMember { get; set; }

        //所有教學點數所對應到的實際金額(會根據儲值時優惠不同而使每點價值有所不同)
        public double TotalPrice { get; set; }
    }
}