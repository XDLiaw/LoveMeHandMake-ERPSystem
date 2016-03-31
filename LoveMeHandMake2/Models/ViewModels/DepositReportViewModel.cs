using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Models.ViewModels
{
    public class DepositReportViewModel
    {
        [Display(Name = "门市")]
        public int? SearchStoreID { get; set; }

        [Display(Name = "日期(起)")]
        public DateTime? SearchDateStart { get; set; }

        [Display(Name = "日期(讫)")]
        public DateTime? SearchDateEnd { get; set; }

        public List<DepositRecord> DepositList { get; set; }

        public List<TeacherSalesPerformance> TeacherSalesPerformanceList { get; set; }
    }

    public class DepositRecord
    {
        [Display(Name = "日期")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd dddd}")]
        public DateTime DepositTime { get; set; }

        [Display(Name = "姓名")]
        public string MemberName { get; set; }

        [Display(Name = "生日")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime MemberBirthday { get; set; }

        // 0 -> female, 1 -> male
        [Display(Name = "性别")]
        public bool MemberGender { get; set; }

        [Display(Name = "销售点数")]
        public double Point { get; set; }

        [Display(Name = "会员卡号")]
        public string MemberCardID { get; set; }

        [Display(Name = "销售人员")]
        public string TeacherName { get; set; }

        [Display(Name = "电话")]
        [Phone]
        public string MemberPhone { get; set; }
    }

    public class TeacherSalesPerformance 
    {
        [Display(Name = "销售人员")]
        public string TeacherName { get; set; }

        [Display(Name = "销售点数")]
        public double Point { get; set; }
    }
}