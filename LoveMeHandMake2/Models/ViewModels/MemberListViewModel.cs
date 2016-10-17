using MvcPaging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Models.ViewModels
{
    public class MemberListViewModel
    {
        [Display(Name = "姓名")]
        public string searchName { get; set; }

        [Display(Name = "电话")]
        public string searchPhone { get; set; }

        [Display(Name = "卡号")]
        public string searchCardID { get; set; }

        [Display(Name = "页码")]
        public int PageNumber { get; set; }

        [Display(Name = "每页资料笔数")]
        public int PageSize { get; private set; }

        // -----------------------------------------------------------------------------------------

        public IPagedList<Member> memberPagedList { get; set; }

        public MemberListViewModel()
        {
            this.PageNumber = 1;
            this.PageSize = 100;
        }
    }
}