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

        // -----------------------------------------------------------------------------------------

        public List<DepositRecord> DepositList { get; set; }

        public List<TeacherSalesPerformance> TeacherSalesPerformanceList { get; set; }

        [Display(Name = "门市名称")]
        public string StoreName { get; set; }

        [Display(Name = "会员卡销售小计")]
        public double TotalPoint { get; set; }

        public DepositReportViewModel()
        {
            this.DepositList = new List<DepositRecord>();
            this.TeacherSalesPerformanceList = new List<TeacherSalesPerformance>();
        }

        public void ComputeTotalPoint()
        {
            TotalPoint = 0;
            foreach (TeacherSalesPerformance tsp in this.TeacherSalesPerformanceList)
            {
                TotalPoint += tsp.Point;
            }
        }
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

        //====================================================================

        [Display(Name = "现金")]
        [Range(0, int.MaxValue)]
        public int? Cash { get; set; }

        [Display(Name = "信用卡")]
        [Range(0, int.MaxValue)]
        public int? CreditCard { get; set; }

        [Display(Name = "商城卡")]
        [Range(0, int.MaxValue)]
        public int? MallCard { get; set; }

        [Display(Name = "支付宝")]
        [Range(0, int.MaxValue)]
        public int? Alipay { get; set; }

        [Display(Name = "微信支付")]
        [Range(0, int.MaxValue)]
        public int? WechatWallet { get; set; }

        [Display(Name = "其他支付")]
        [Range(0, int.MaxValue)]
        public int? OtherPay { get; set; }

        [Display(Name = "送点")]
        public double? RewardPoint { get; set; }

        [Display(Name = "送金")]
        public int? RewardMoney { get; set; }

        [Display(Name = "现/信/商/支/微/其")]
        public String PayWayString
        {
            get
            {
                String s = "";
                s += this.Cash.GetValueOrDefault(0) + "/";
                s += this.CreditCard.GetValueOrDefault(0) + "/";
                s += this.MallCard.GetValueOrDefault(0) + "/";
                s += this.Alipay.GetValueOrDefault(0) + "/";
                s += this.WechatWallet.GetValueOrDefault(0) + "/";
                s += this.OtherPay.GetValueOrDefault(0);
                return s;
            }
        }

        //====================================================================

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