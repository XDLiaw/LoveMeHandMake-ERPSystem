using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Models
{
    public class NonMemberTradeList : BaseModel
    {
        [Required]
        public int? StoreID { get; set; }

        [Display(Name = "销售门市")]
        public virtual Store Store { get; set; }

        [Required]
        public int? TeacherID { get; set; }

        [Display(Name = "销售人员")]
        public virtual Teacher Teacher { get; set; }

        public string Phone { get; set; }

        [Display(Name = "消费点数")]
        [Required]
        public Double Point { get; set; }

        [Display(Name = "消费时间")]
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime TradeDate { get; set; }
    }
}