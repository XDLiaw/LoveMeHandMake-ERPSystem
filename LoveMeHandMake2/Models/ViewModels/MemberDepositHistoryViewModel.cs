using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Models.ViewModels
{
    public class MemberDepositHistoryViewModel
    {
        [Display(Name = "日期(起)")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? SearchDateStart { get; set; }

        [Display(Name = "日期(讫)")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? SearchDateEnd { get; set; }

        // ---------------------------------------------------------------------------------------

        public Member member { get; set; }

        public List<DepositHistory> DepositHistoryList { get; set; }

        public MemberDepositHistoryViewModel()
        {
            this.DepositHistoryList = new List<DepositHistory>();
        }
    }
}