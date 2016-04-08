using Newtonsoft.Json;
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
                DeleteProductImage();
                SaveImage();            
            }
        }

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

        public byte[] GetImage() {
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

        public void SaveImage()
        {
            if (this.UploadImage == null) return;
            //===========================================================================
            string folder = WebConfigurationManager.AppSettings["ProductImageFolder"];
            this.ImageName = this.ProductCategory.Name + "_" + this.Name + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
            string imgPath = System.IO.Path.Combine(folder, this.ImageName);
            Image img = Image.FromStream(this.UploadImage.InputStream, true, true);
            Image thumbnails = ScaleImage(img, 100, 100);
            thumbnails.Save(imgPath, System.Drawing.Imaging.ImageFormat.Jpeg);
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
                string folder = WebConfigurationManager.AppSettings["ProductImageFolder"];
                System.IO.File.Delete(System.IO.Path.Combine(folder, this.ImageName));
            }
            catch (Exception e)
            {
                log.Error(null, e);
            }
        }
    }
}