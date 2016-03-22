using LoveMeHandMake2.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Models.ApiModels
{
    public class BaseApiModelI
    {
        private static string Token = "LoveMeHandMake";
        private static string Key = "SYSVIN";
        
        public string EncryptedToken { get; set; }

        private List<string> InvalidReasons = new List<string>();

        public virtual bool IsValid()
        {
            if (string.IsNullOrEmpty(this.EncryptedToken))
            {
                this.AddInvalidReason("EncryptedToken can't be null or empty!");
                return false;
            }
            string decryptedString = AESEncrypter.Decrypt(this.EncryptedToken, Key);
            bool isValid = decryptedString.Equals(Token);
            if (isValid == false)
            {
                InvalidReasons.Add("Token doesn't match!");
            }
            return isValid;
        }

        public List<string> GetInvalidReasons()
        {
            return this.InvalidReasons;
        }

        protected void AddInvalidReason(string reason)
        {
            this.InvalidReasons.Add(reason);
        }
    }
}