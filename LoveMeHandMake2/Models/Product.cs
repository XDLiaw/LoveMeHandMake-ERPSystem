using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace LoveMeHandMake2.Models
{
    public class Product : BaseModel
    {
        [Required(ErrorMessage = "不能为空")]
        public int ProductCategoryID { get; set; }

        [Display(Name = "产品類別")]
        public virtual ProductCategory ProductCategory { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "不能为空")]
        [Display(Name = "产品名称")]
        public string Name { get; set; }

        // Price unit can be "point" or "bean" depends on product category
        [Required(ErrorMessage = "不能为空")]
        [Display(Name = "价格")]
        public int Price { get; set; }

        [Display(Name = "图片")]
        public string ImagePath { get; set; }

        [NotMapped]
        public HttpPostedFileBase UploadImage { get; set; }

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
                DeleteProductImage();
                SaveImage();            
            }
        }

        public byte[] GetImage() {
            Bitmap img = null;
            if (System.IO.File.Exists(this.ImagePath))
            {
                img = (Bitmap)Image.FromFile(this.ImagePath, true);
            }
            else
            {
                string folder = WebConfigurationManager.AppSettings["ProductImageFolder"];
                string fileName = "logo.jpg";
                string imgPath = System.IO.Path.Combine(folder, fileName);
                img = (Bitmap)Image.FromFile(imgPath, true);
            }
            ImageConverter converter = new ImageConverter();
            byte[] imgByte = (byte[])converter.ConvertTo(img, typeof(byte[]));
            img.Dispose();
            return imgByte;
        }

        public string GetImageBase64String()
        {
            return System.Convert.ToBase64String(this.GetImage());
        }

        public void SaveImage()
        {
            if (this.UploadImage == null) return;
            //===========================================================================
            string folder = WebConfigurationManager.AppSettings["ProductImageFolder"];
            string fileName = this.ProductCategory.Name + "_" + this.Name + "_" + DateTime.Now.ToString("yyyyMMddHHmm") + "_" + this.UploadImage.FileName;
            this.ImagePath = System.IO.Path.Combine(folder, fileName);
            //this.UploadImage.SaveAs(this.ImagePath);

            Image img = Image.FromStream(this.UploadImage.InputStream, true, true);
            Image thumbnails = ScaleImage(img, 100, 100);
            thumbnails.Save(this.ImagePath);

        }

        private static Image ScaleImage(Image image, int maxWidth, int maxHeight)
        {
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);

            using (var graphics = Graphics.FromImage(newImage))
                graphics.DrawImage(image, 0, 0, newWidth, newHeight);

            return newImage;
        }

        public void DeleteProductImage()
        {
            try
            {
                System.IO.File.Delete(this.ImagePath);
            }
            catch (Exception e)
            {
                log.Error(null, e);
            }
        }
    }
}