using LoveMeHandMake2.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Models.ApiModels
{
    public class TeacherModifyRequestApiModel : BaseRequestApiModel
    {
        public int ID { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "密码长度必须在{2} 和{1}之间")]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        public override bool IsValid()
        {
            if(base.IsValid() == false) {                
                return false;
            }
            try
            {
                this.Password = AESEncrypter.Decrypt(this.Password);
            }
            catch (Exception e)
            {
                AddInvalidReason(e.Message);
                log.Warn(null, e);
                return false;
            }

            return true;
        }
    }
}