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
        public string Key { get; set; }

        [Required]
        public string Value { get; set; }

        public string Note { get; set; }
    }
}