using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using LoveMeHandMake2.Models;
using log4net;

namespace LoveMeHandMake2.Helper
{
    public class ProductImageHelper
    {
        protected static ILog log = LogManager.GetLogger(typeof(BaseModel));

        private static string folder = WebConfigurationManager.AppSettings["ProductImageFolder"];
        private static string defaultImageName = "logo.jpg";

        public static byte[] GetImageIfExist(String ImageName)
        {
            if (ImageName != null && System.IO.File.Exists(System.IO.Path.Combine(folder, ImageName)))
            {
                return GetImage(ImageName);
            }
            return null;
        }

        public static byte[] GetImage(String ImageName)
        {
            Bitmap img = null;
            string imgPath = null;
            if (ImageName == null)
            {
                imgPath = System.IO.Path.Combine(folder, defaultImageName);
            }
            else
            {
                imgPath = System.IO.Path.Combine(folder, ImageName);
                if (System.IO.File.Exists(imgPath) == false)
                {
                    imgPath = System.IO.Path.Combine(folder, defaultImageName);
                }
            }

            img = (Bitmap)Image.FromFile(imgPath, true);
            ImageConverter converter = new ImageConverter();
            byte[] imgByte = (byte[])converter.ConvertTo(img, typeof(byte[]));
            img.Dispose();
            return imgByte;
        }

        public static string GetImageBase64String(String ImageName)
        {
            return System.Convert.ToBase64String(GetImage(ImageName));
        }



        public static void DeleteImage(String ImageName)
        {
            try
            {
                System.IO.File.Delete(System.IO.Path.Combine(folder, ImageName));
            }
            catch (Exception e)
            {
                log.Warn(null, e);
            }
        }

        public static Image ScaleImage(Image image, int maxWidth, int maxHeight)
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
    }
}