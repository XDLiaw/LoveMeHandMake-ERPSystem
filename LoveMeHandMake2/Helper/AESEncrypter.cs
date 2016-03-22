using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace LoveMeHandMake2.Helper
{
    public class AESEncrypter
    {
        private static string DefaultKey = "SYSVIN";

        public static string Encrypt(string plainText, string key)
        {
            plainText = DateTime.Now.ToString() + plainText;
            byte[] plainTextData = Encoding.Unicode.GetBytes(plainText);
            RijndaelManaged AES = new RijndaelManaged();
            MD5CryptoServiceProvider MD5 = new MD5CryptoServiceProvider();
            byte[] keyData = MD5.ComputeHash(Encoding.Unicode.GetBytes(key));
            ICryptoTransform transform = AES.CreateEncryptor(keyData, keyData);
            byte[] outputData = transform.TransformFinalBlock(plainTextData, 0, plainTextData.Length);
            return Convert.ToBase64String(outputData);
        }

        public static string Encrypt(string plainText)
        {
            return Encrypt(plainText, DefaultKey);
        }

        public static string Decrypt(string cipherText, string key)
        {
            byte[] cipherTextData = Convert.FromBase64String(cipherText);
            RijndaelManaged AES = new RijndaelManaged();
            MD5CryptoServiceProvider MD5 = new MD5CryptoServiceProvider();
            byte[] keyData = MD5.ComputeHash(Encoding.Unicode.GetBytes(key));
            ICryptoTransform transform = AES.CreateDecryptor(keyData, keyData);
            byte[] outputData = transform.TransformFinalBlock(cipherTextData, 0, cipherTextData.Length);
            return Encoding.Unicode.GetString(outputData).Substring(DateTime.Now.ToString().Length);
        }

        public static string Decrypt(string cipherText)
        {
            return Decrypt(cipherText, DefaultKey);
        }

    }
}