using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using LoveMeHandMake2.Helper;

namespace LoveMeHandMake2.Models
{
    public class Product : BaseModel
    {
        [Required(ErrorMessage = "不能为空")]
        public int ProductCategoryID { get; set; }

        [Display(Name = "产品類別")]
        [JsonIgnore]
        public virtual ProductCategory ProductCategory { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "不能为空")]
        [Display(Name = "产品名称")]
        public string Name { get; set; }

        // Price unit can be "point" or "bean" depends on product category
        [Required(ErrorMessage = "不能为空")]
        [Display(Name = "价格")]
        public int Price { get; set; }

        [Display(Name = "图片")]
        [JsonIgnore]
        public string ImageName { get; set; }

        [NotMapped]
        [JsonIgnore]
        public HttpPostedFileBase UploadImage { get; set; }

        [NotMapped]
        public byte[] ImageByteArray { get; set; }

        [Display(Name = "已下架")]
        public bool IsPullFromShelves { get; set; }

        [Display(Name = "备注")]
        public string Remark { get; set; }

        public override void Create()
        {
            base.Create();
            SaveImage();
        }

        public override void Update()
        {
            base.Update();
            if (this.UploadImage != null) {
                ProductImageHelper.DeleteImage(this.ImageName);
                SaveImage();            
            }
        }

        private void SaveImage()
        {
            if (this.UploadImage == null) return;
            //===========================================================================
            string folder = WebConfigurationManager.AppSettings["ProductImageFolder"];
            this.ImageName = this.ProductCategory.Name + "_" + this.Name + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
            string imgPath = System.IO.Path.Combine(folder, this.ImageName);
            Image img = Image.FromStream(this.UploadImage.InputStream, true, true);
            Image thumbnails = ProductImageHelper.ScaleImage(img, 100, 100);
            thumbnails.Save(imgPath, System.Drawing.Imaging.ImageFormat.Jpeg);
        }
    }
}