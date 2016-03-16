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
        public int StoreID { get; set; }

        [Display(Name = "销售门市")]
        public virtual Store Store { get; set; }

        [Required]
        public int TeacherID { get; set; }

        [Display(Name = "销售人员")]
        public virtual Teacher Teacher { get; set; }

        [Required]
        public int NonMemberID { get; set; }

        [Display(Name = "客户")]
        public virtual NonMember NonMember { get; set; }

        [Display(Name = "消费点数")]
        [Required]
        public int Point { get; set; }

        [Display(Name = "消费日")]
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime TradeDate { get; set; }
    }
}