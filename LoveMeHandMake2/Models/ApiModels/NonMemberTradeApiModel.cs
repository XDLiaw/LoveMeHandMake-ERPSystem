using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Models.ApiModels
{
    public class NonMemberTradeApiModel : BaseRequestApiModel
    {
        [Display(Name = "姓名")]
        public string Name { get; set; }

        // 0 -> female, 1 -> male
        [Display(Name = "性别")]
        public bool? Gender { get; set; }

        [Display(Name = "生日")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? Birthday { get; set; }

        [Display(Name = "电话")]
        [Required(AllowEmptyStrings = false)]
        [Phone]
        public string Phone { get; set; }

        [Display(Name = "销售门市")]
        [Required]
        public int StoreID { get; set; }

        [Display(Name = "销售人员")]
        [Required]
        public int TeacherID { get; set; }

        [Display(Name = "消费点数")]
        [Required]
        public Double Point { get; set; }

        [Display(Name = "消费时间")]
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime TradeDateTime { get; set; }
    }
}