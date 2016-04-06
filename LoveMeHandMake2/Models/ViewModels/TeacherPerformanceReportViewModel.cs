using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Models.ViewModels
{
    public class TeacherPerformanceReportViewModel
    {
        [Display(Name = "门市")]
        public int? SearchStoreID { get; set; }

        [Display(Name = "老师")]
        public int? SearchTeacherID { get; set; }

        [Display(Name = "日期(起)")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? SearchDateStart { get; set; }

        [Display(Name = "日期(讫)")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? SearchDateEnd { get; set; }

        [Display(Name = "日期")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM}")]
        public DateTime SearchYearMonth { get; set; }

        //---------------------------------------------------------------------------------------

        [Display(Name = "门市名称")]
        public string StoreName { get; set; }

        public List<TeacherPerformance> MultiTeacherPerformance { get; set; }

        public AllTeacherPerformance allTeacherPerformance { get; set; }

        [Display(Name = "总教学点数")]
        public double TotalTeachPoints { get; set; }

        [Display(Name = "总销售点数")]
        public double TotalSalesPoints { get; set; }

        [Display(Name = "总单做点数")]
        public double TotalPointsFromNonMember { get; set; }

        [Display(Name = "总教学、销售、单做点数")]
        public double TotalPoints { get; set; }

        [Display(Name = "總教學點數百分比")]
        [DisplayFormat(DataFormatString = "{0:F3}", ApplyFormatInEditMode = true)]
        public double TeachPointProportion { get; set; }

        [Display(Name = "总销售比例百分比")]
        [DisplayFormat(DataFormatString = "{0:F3}", ApplyFormatInEditMode = true)]
        public double SalesPointProportion { get; set; }

        [Display(Name = "过槛奖金点数")]
        public int? ThresholdPoint { get; set; }

        [Display(Name = "过槛每点直抽奖金")]
        public int? OverThresholdBonus { get; set; }

        [Display(Name = "总奖金")]
        public double? TotalBonus { get; set; }

        public void Compute()
        {
            foreach (TeacherPerformance tp in this.MultiTeacherPerformance)
            {
                tp.Compute();
            }
            this.allTeacherPerformance.Compute();
            this.TotalTeachPoints = this.allTeacherPerformance.TotalTeachPoints;
            this.TotalSalesPoints = this.allTeacherPerformance.TotalSalesPoints;
            this.TotalPointsFromNonMember = this.allTeacherPerformance.TotalPointsFromNonMember;
            this.TotalPoints = this.TotalTeachPoints + this.TotalSalesPoints + this.TotalPointsFromNonMember;
            this.TeachPointProportion = this.TotalTeachPoints / this.TotalPoints;
            this.SalesPointProportion = (this.TotalSalesPoints + this.TotalPointsFromNonMember) / this.TotalPoints;
            if (this.SearchStoreID == null)
            {
                this.ThresholdPoint = null;
                this.OverThresholdBonus = null;
                this.TotalBonus = null;
            }
            else
            {
                this.TotalBonus = (this.TotalPoints - this.ThresholdPoint) * this.OverThresholdBonus;
            }            
        }

    }

    public class TeacherPerformance
    {
        public int TeacherID { get; set; }

        [Display(Name = "老师")]
        public string TeacherName { get; set; }

        public List<TeacherDailyPerformance> DailyPerformanceList { get; set; }

        [Display(Name = "总教学次数")]
        public int TotalTeachTimes { get; set; }

        [Display(Name = "总教学点数")]
        [DisplayFormat(DataFormatString = "{0:F1}", ApplyFormatInEditMode = true)]
        public double TotalTeachPoints { get; set; }

        [Display(Name = "总销售点数")]
        [DisplayFormat(DataFormatString = "{0:F1}", ApplyFormatInEditMode = true)]
        public double TotalSalesPoints { get; set; }

        [Display(Name = "总单做点数")]
        [DisplayFormat(DataFormatString = "{0:F1}", ApplyFormatInEditMode = true)]
        public double TotalPointsFromNonMember { get; set; }

        public void Compute()
        {
            this.TotalTeachTimes = this.DailyPerformanceList.Sum(x => x.TeachTimes);
            this.TotalTeachPoints = this.DailyPerformanceList.Sum(x => x.TeachPoints);
            this.TotalSalesPoints = this.DailyPerformanceList.Sum(x => x.SalesPoints);
            this.TotalPointsFromNonMember = this.DailyPerformanceList.Sum(x => x.PointsFromNonMember);
        }
    }

    public class AllTeacherPerformance
    {
        public List<TeacherDailyPerformance> DailyPerformanceList { get; set; }

        [Display(Name = "总教学次数")]
        public int TotalTeachTimes { get; set; }

        [Display(Name = "总教学点数")]
        [DisplayFormat(DataFormatString = "{0:F1}", ApplyFormatInEditMode = true)]
        public double TotalTeachPoints { get; set; }

        [Display(Name = "总销售点数")]
        [DisplayFormat(DataFormatString = "{0:F1}", ApplyFormatInEditMode = true)]
        public double TotalSalesPoints { get; set; }

        [Display(Name = "总单做点数")]
        [DisplayFormat(DataFormatString = "{0:F1}", ApplyFormatInEditMode = true)]
        public double TotalPointsFromNonMember { get; set; }

        public void Compute()
        {
            this.TotalTeachTimes = this.DailyPerformanceList.Sum(x => x.TeachTimes);
            this.TotalTeachPoints = this.DailyPerformanceList.Sum(x => x.TeachPoints);
            this.TotalSalesPoints = this.DailyPerformanceList.Sum(x => x.SalesPoints);
            this.TotalPointsFromNonMember = this.DailyPerformanceList.Sum(x => x.PointsFromNonMember);
        }
    }

    public class TeacherDailyPerformance
    {
        //public int? TeacherID { get; set; }

        //[Display(Name = "老师")]
        //public string TeacherName { get; set; }

        //public int Year { get; set; }

        //public int Month { get; set; }

        //public int Day { get; set; }

        [Display(Name = "日期")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd (dddd)}")]
        public DateTime Date { get; set; }

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

    }
}