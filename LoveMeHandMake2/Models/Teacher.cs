using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Models
{
    public class Teacher : BaseModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "不能为空")]
        [StringLength(50)]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "不能为空")]
        public string AccountID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "不能为空")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "密码长度必须在{2} 和{1}之间")]
        public string Password { get; set; }

        [StringLength(20, MinimumLength = 6, ErrorMessage = "密码长度必须在{2} 和{1}之间")]
        public string PreviousPassword { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "不能为空")]
        public bool CanPublishPRCard { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "不能为空")]
        [Phone]
        public string Phone { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "不能为空")]
        public int BelongStoreID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "不能为空")]
        public DateTime ArriveDate { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "不能为空")]
        public DateTime Birthday { get; set; }

        public string Birthplace { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "不能为空")]
        public string IDNumber { get; set; }

        [EmailAddressAttribute]
        public string Email { get; set; }

        public string Address { get; set; }
    }
}