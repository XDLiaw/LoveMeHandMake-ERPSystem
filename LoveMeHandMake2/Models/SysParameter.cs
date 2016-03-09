using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Models
{
    public class SysParameter : BaseModel
    {
        [Required]
        [Display(Name = "参数名称")]
        public string Key { get; set; }

        [Required]
        [Display(Name = "参数值")]
        public string Value { get; set; }

        [Display(Name = "说明")]
        public string Note { get; set; }
    }
}