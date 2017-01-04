using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace LoveMeHandMake2.Models.ViewModels
{
    public class ProductSaleReportViewModel
    {
        [Display(Name = "门市")]
        public int? SearchStoreID { get; set; }

        [Display(Name = "产品类别")]
        public int? SearchProductCategoryID { get; set; }

        [Display(Name = "日期(起)")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? SearchDateStart { get; set; }

        [Display(Name = "日期(讫)")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? SearchDateEnd { get; set; }

        // ---------------------------------------------------------------------------------------------

        [Display(Name = "门市名称")]
        public string StoreName { get; set; }
       
        [Display(Name = "总消费点数")]
        public int TotalPoint { get; set; }

        [Display(Name = "总消费豆数")]
        public int TotalBean { get; set; }

        [Display(Name = "总消费金额")]
        public double TotalMoney { get; set; }

        [Display(Name = "会员消费次数")]
        public int MemberTradeTimes { get; set; }

        [Display(Name = "单做消费次数")]
        public int NonMemberTradeTimes { get; set; }

        [Display(Name = "总消费次数")]
        public int TotalTradeTimes { get; set; }

        [Display(Name = "平均客單")]
        public double AvgPrice { get; set; }

        [Display(Name = "周一至周五平均客流量")]
        public int WeekdayTradeTimes { get; set; }

        [Display(Name = "周末平均客流量")]
        public int WeekendTradeTimes { get; set; }

        public List<ProductSaleRecord> saleList { get; set; }

        public ProductSaleReportViewModel()
        {
            this.SearchDateStart = DateTime.Now.Date; 
            this.saleList = new List<ProductSaleRecord>();
        }

        public void ComputeAll()
        {
            ComputeTotalPoint();
            ComputeTotalBean();
            ComputeTotalMoney();
            ComputeTradeTimes();
            ComputeAveragePrice();
        }

        private void ComputeTotalPoint()
        {
            this.TotalPoint = 0;
            foreach (ProductSaleRecord psr in this.saleList)
            {
                this.TotalPoint += psr.Amount * psr.UnitPoint.GetValueOrDefault();
            }
        }

        private void ComputeTotalBean()
        {
            this.TotalBean = 0;
            foreach (ProductSaleRecord psr in this.saleList)
            {
                this.TotalBean += psr.Amount * psr.UnitBean.GetValueOrDefault();
            }
        }

        private void ComputeTotalMoney()
        {
            this.TotalMoney = 0;
            foreach (ProductSaleRecord psr in this.saleList)
            {
                this.TotalMoney += psr.Sum;
            }
        }

        private void ComputeTradeTimes()
        {
            this.NonMemberTradeTimes = 0;
            this.MemberTradeTimes = 0;
            this.WeekdayTradeTimes = 0;
            this.WeekendTradeTimes = 0;
            foreach (ProductSaleRecord psr in this.saleList)
            {
                if (string.IsNullOrEmpty(psr.MemberCardID))
                {
                    this.NonMemberTradeTimes += psr.Amount;
                }
                else
                {
                    this.MemberTradeTimes += psr.Amount;
                }
                //計算客流量
                if (psr.TradeDateTime.DayOfWeek == DayOfWeek.Saturday || psr.TradeDateTime.DayOfWeek == DayOfWeek.Sunday)
                {
                    WeekendTradeTimes += psr.Amount;
                }
                else
                {
                    WeekdayTradeTimes += psr.Amount;
                }
            }
            this.TotalTradeTimes = this.NonMemberTradeTimes + this.MemberTradeTimes;
        }

        private void ComputeAveragePrice()
        {
            this.AvgPrice = this.TotalMoney / this.TotalTradeTimes;
        }
    }

    public class ProductSaleRecord
    {
        [Display(Name = "销售时间")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd dddd HH:mm}")]
        public DateTime TradeDateTime { get; set; }

        [Display(Name = "商品名称")]
        public string ProductName { get; set; }

        [Display(Name = "数量")]
        [Required]
        public int Amount { get; set; }

        [Display(Name = "单价(点)")]
        // How many points for each one of this product
        public int? UnitPoint { get; set; }

        [Display(Name = "单价(豆)")]
        // How many beans for each one of this product
        public int? UnitBean { get; set; }

        [Display(Name = "会员卡号")]
        public string MemberCardID { get; set; }

        [Display(Name = "金额")]
        public double Sum { get; set; }

        // 0 -> female, 1 -> male
        [Display(Name = "性别")]
        public bool? Gender { get; set; }

        [Display(Name = "指导老师")]
        public string TeacherName { get; set; }

        [Display(Name = "图片")]
        [JsonIgnore]
        public string ImageName { get; set; }

        public int ChargeByCash { get; set; }

        public int ChargeByCreditCard { get; set; }

        public int ChargeByMallCard { get; set; }

        public int ChargeByAlipay { get; set; }

        public int ChargeByWechatWallet { get; set; }

        public int ChargeByOtherPay { get; set; }

        public int RewardMoney { get; set; }

        public double RewardPoint { get; set; }

        public string ChargeWay
        {
            get
            {
                if (String.IsNullOrWhiteSpace(this.MemberCardID))
                {
                    String s = "";
                    if (this.ChargeByCash > 0)
                    {
                        s += "單做現金;\r\n";
                    }
                    if (this.ChargeByCreditCard > 0)
                    {
                        s += "單做信用卡;\r\n";
                    }
                    if (this.ChargeByMallCard > 0)
                    {
                        s += "單做商城卡;\r\n";
                    }
                    if (this.ChargeByAlipay > 0)
                    {
                        s += "單做支付宝;\r\n";
                    }
                    if (this.ChargeByWechatWallet > 0)
                    {
                        s += "單做微信支付;\r\n";
                    }
                    if (this.ChargeByOtherPay > 0)
                    {
                        s += "單做其他支付;\r\n";
                    }
                    if (this.RewardMoney > 0)
                    {
                        s += "單做送金;\r\n";
                    }
                    if (this.RewardPoint > 0)
                    {
                        s += "單做送點;\r\n";
                    }

                    return s;
                }
                else
                {
                    return this.MemberCardID;
                }
            }
        }
    }


}