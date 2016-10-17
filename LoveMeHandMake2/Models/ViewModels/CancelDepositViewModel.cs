using MvcPaging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Models.ViewModels
{
    public class CancelDepositViewModel
    {
        [Display(Name = "日期(起)")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? SearchDateStart { get; set; }

        [Display(Name = "日期(讫)")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? SearchDateEnd { get; set; }

        [Display(Name = "页码")]
        public int PageNumber { get; set; }

        [Display(Name = "每页资料笔数")]
        public int PageSize { get; private set; }

        // -----------------------------------------------------------------------------------------

        public IPagedList<DepositHistory> cancelPagedList { get; set; }

        public CancelDepositViewModel()
        {
            this.PageNumber = 1;
            this.PageSize = 100;
        }
    }
}