using log4net;
using LoveMeHandMake2.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Models.ApiModels
{
    public class BaseRequestApiModel
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(BaseRequestApiModel));
        private static string Token = "LoveMeHandMake";
        
        public string EncryptedToken { get; set; }

        private List<string> InvalidReasons = new List<string>();

        public virtual bool IsValid()
        {
            if (string.IsNullOrEmpty(this.EncryptedToken))
            {
                this.AddInvalidReason("EncryptedToken can't be null or empty!");
                return false;
            }
            string decryptedString = AESEncrypter.Decrypt(this.EncryptedToken);
            bool isValid = decryptedString.Equals(Token);
            if (isValid == false)
            {
                InvalidReasons.Add("Token doesn't match!");
                log.Warn("Token doesn't match!");
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