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
        [Display(Name = "姓名")]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "不能为空")]
        [Display(Name = "帐号/员工编号")]
        public string AccountID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "不能为空")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "密码长度必须在{2} 和{1}之间")]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [StringLength(20, MinimumLength = 6, ErrorMessage = "密码长度必须在{2} 和{1}之间")]
        [Display(Name = "旧密码")]
        public string PreviousPassword { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "不能为空")]
        [Display(Name = "是否可发公关卡")]
        public bool CanPublishPRCard { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "不能为空")]
        [Phone]
        [Display(Name = "电话")]
        public string Phone { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "不能为空")]
        public int BelongStoreID { get; set; }

        [Display(Name = "所属门市")]
        public virtual Store BelongStore { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "不能为空")]
        [Display(Name = "到职日")]
        public DateTime ArriveDate { get; set; }

        [Display(Name = "离职日")]
        public DateTime ResignDate { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "不能为空")]
        [Display(Name = "生日")]
        public DateTime Birthday { get; set; }

        [Display(Name = "籍贯")]
        public string Birthplace { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "不能为空")]
        [Display(Name = "身分证号")]
        public string IDNumber { get; set; }

        [EmailAddressAttribute]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "地址")]
        public string Address { get; set; }
    }
}