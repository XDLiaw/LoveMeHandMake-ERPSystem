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
        public DateTime? SearchDateStart { get; set; }

        [Display(Name = "日期(讫)")]
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
                if (psr.TradeDateTime.DayOfWeek == DayOfWeek.Saturday || psr.TradeDateTime.DayOfWeek == DayOfWeek.Sunday)
                {
                    //TODO
                }
                else
                {
                    //TODO
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

        public byte[] GetImageIfExist()
        {
            Bitmap img = null;
            string folder = WebConfigurationManager.AppSettings["ProductImageFolder"];
            string imgPath = System.IO.Path.Combine(folder, this.ImageName);
            if (System.IO.File.Exists(imgPath) == false)
            {
                return null;
            }
            img = (Bitmap)Image.FromFile(imgPath, true);
            ImageConverter converter = new ImageConverter();
            byte[] imgByte = (byte[])converter.ConvertTo(img, typeof(byte[]));
            img.Dispose();
            return imgByte;
        }

        public byte[] GetImage()
        {
            Bitmap img = null;
            string folder = WebConfigurationManager.AppSettings["ProductImageFolder"];
            string imgPath = null;
            if (this.ImageName == null)
            {
                imgPath = System.IO.Path.Combine(folder, "logo.jpg");
            }
            else
            {
                imgPath = System.IO.Path.Combine(folder, this.ImageName);
                if (System.IO.File.Exists(imgPath) == false)
                {
                    imgPath = System.IO.Path.Combine(folder, "logo.jpg");
                }
            }

            img = (Bitmap)Image.FromFile(imgPath, true);
            ImageConverter converter = new ImageConverter();
            byte[] imgByte = (byte[])converter.ConvertTo(img, typeof(byte[]));
            img.Dispose();
            return imgByte;
        }

        public string GetImageBase64String()
        {
            return System.Convert.ToBase64String(this.GetImage());
        }
    }


}