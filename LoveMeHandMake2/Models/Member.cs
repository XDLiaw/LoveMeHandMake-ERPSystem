using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Models
{
    public class Member : BaseModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "不能为空")]
        [StringLength(50)]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "不能为空")]
        [StringLength(10)]
        public string CardID { get; set; }

        // 0 -> female, 1 -> male
        [Required(AllowEmptyStrings = false, ErrorMessage = "不能为空")]
        public bool Gender { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "不能为空")]
        public DateTime Birthday { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "不能为空")]
        [Phone]
        public string Phone { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "不能为空")]
        public bool IsPRCard { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "不能为空")]
        public int EnrollTeacherID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "不能为空")]
        public int EnrollStoreID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "不能为空")]
        public DateTime EnrollDate { get; set; }

        public int Point { get; set; }

        public int AccumulateDeposite { get; set; }
    }
}