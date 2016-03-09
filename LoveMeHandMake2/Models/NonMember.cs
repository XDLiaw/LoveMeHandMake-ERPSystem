using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Models
{
    public class NonMember : BaseModel
    {
        public string Name { get; set; }

        // 0 -> female, 1 -> male
        public bool? Gender { get; set; }

        public DateTime? Birthday { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "不能为空")]
        [Phone]
        public string Phone { get; set; }
    }
}