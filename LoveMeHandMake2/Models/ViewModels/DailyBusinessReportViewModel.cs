﻿using System;
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

        // -----------------------------------------------------------------------------------------

        [Display(Name = "门市名称")]
        public string StoreName { get; set; }

        [Display(Name = "总现金业绩")]
        public double TotalCash { get; set; }

        [Display(Name = "总信用卡业绩")]
        public double TotalCreditCard { get; set; }

        [Display(Name = "总商城卡业绩")]
        public double TotalMallCard { get; set; }

        [Display(Name = "总支付宝业绩")]
        public double TotalAlipay { get; set; }

        [Display(Name = "总微信支付业绩")]
        public double TotalWechatWallet { get; set; }

        [Display(Name = "总其他支付业绩")]
        public double TotalOtherPay { get; set; }

        [Display(Name = "总业绩")]
        public double TotalMoney { get; set; }

        public List<DailyBusinessRecord> DailyRecords { get; set; }

        public DailyBusinessReportViewModel()
        {
            this.DailyRecords = new List<DailyBusinessRecord>();
        }

        public void computeTotalMoeny()
        {
            this.TotalCash = DailyRecords.Sum(x => x.Cash);
            this.TotalCreditCard = DailyRecords.Sum(x => x.CreditCard);
            this.TotalMallCard = DailyRecords.Sum(x => x.MallCard);
            this.TotalAlipay = DailyRecords.Sum(x => x.Alipay);
            this.TotalWechatWallet = DailyRecords.Sum(x => x.WechatWallet);
            this.TotalOtherPay = DailyRecords.Sum(x => x.OtherPay);
            this.TotalMoney = this.TotalCash + this.TotalCreditCard + this.TotalMallCard + this.TotalAlipay + this.TotalWechatWallet + this.TotalOtherPay;
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

        [Display(Name = "支付宝")]
        public double Alipay { get; set; }

        [Display(Name = "微信支付")]
        public double WechatWallet { get; set; }

        [Display(Name = "其他支付")]
        public double OtherPay { get; set; }

        [Display(Name = "当日业绩")]
        public double Total { get; set; }
    }
}