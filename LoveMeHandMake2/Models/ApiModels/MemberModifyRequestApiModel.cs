using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Models.ApiModels
{
    public class MemberModifyRequestApiModel : BaseRequestApiModel
    {
        public Guid CommunicateGuid { get; set; }

        [Display(Name = "姓名")]
        [Required(AllowEmptyStrings = false)]
        [StringLength(50)]
        public string Name { get; set; }
    }
}