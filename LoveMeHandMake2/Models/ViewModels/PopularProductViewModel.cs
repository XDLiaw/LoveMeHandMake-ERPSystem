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
    public class PopularProductViewModel
    {
        [Display(Name = "门市")]
        public int? SearchStoreID { get; set; }

        [Display(Name = "商品类别")]
        public int? SearchProductCategoryID { get; set; }

        [Display(Name = "日期(起)")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? SearchDateStart { get; set; }

        [Display(Name = "日期(讫)")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? SearchDateEnd { get; set; }

        // -----------------------------------------------------------------------------------------

        public IQueryable<PopularProduct> productList { get; set; }
    }

    public class PopularProduct
    {
        [Display(Name = "商品类别")]
        public string CategoryName { get; set; }

        [Display(Name = "名称")]
        public string Name { get; set; }

        [Display(Name = "计价单位")]
        public int Unit { get; set; }

        [Display(Name = "售价")]
        public int Price { get; set; }

        [Display(Name = "销售数量")]
        public int Amount { get; set; }

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
            string imgPath = System.IO.Path.Combine(folder, this.ImageName);
            if (System.IO.File.Exists(imgPath) == false)
            {
                imgPath = System.IO.Path.Combine(folder, "logo.jpg");
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