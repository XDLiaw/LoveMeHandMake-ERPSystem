using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Models
{
    public class Member : BaseModel
    {
        [Display(Name = "姓名")]
        [Required(AllowEmptyStrings = false)]
        [StringLength(50)]
        public string Name { get; set; }

        [Display(Name = "卡号")]
        [Required(AllowEmptyStrings = false)]
        [StringLength(10)]
        public string CardID { get; set; }

        // 0 -> female, 1 -> male
        [Display(Name = "性别")]
        [Required(AllowEmptyStrings = false)]
        public bool Gender { get; set; }

        [Display(Name = "生日")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        [Required]
        public DateTime Birthday { get; set; }

        [Display(Name = "电话")]
        [Required(AllowEmptyStrings = false)]
        [Phone]
        public string Phone { get; set; }

        [Display(Name = "是否为公关卡")]
        [Required(AllowEmptyStrings = false)]
        public bool IsPRCard { get; set; }
        
        [Required]
        public int EnrollTeacherID { get; set; }

        [Display(Name = "办卡人员")]
        public virtual Teacher EnrollTeacher { get; set; }

        [Required]
        public int EnrollStoreID { get; set; }

        [Display(Name = "办卡门市")]
        public virtual Store EnrollStore { get; set; }

        [Display(Name = "办卡日期")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        [Required(AllowEmptyStrings = false)]
        public DateTime EnrollDate { get; set; }

        [Display(Name = "剩余点数")]
        public int Point { get; set; }

        [Display(Name = "累计储值金额")]
        public int AccumulateDeposite { get; set; }
    }
}