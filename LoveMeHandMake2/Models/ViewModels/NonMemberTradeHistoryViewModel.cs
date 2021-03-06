﻿using MvcPaging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Models.ViewModels
{
    public class NonMemberTradeHistoryViewModel
    {
        [Display(Name = "电话")]
        public string SearchPhone { get; set; }

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

        //----------------------------------------------------------------------------

        public IPagedList<NonMemberTradeRecord> NonMemberTradeRecordList { get; set; }

        public NonMemberTradeHistoryViewModel ()
        {
            this.PageNumber = 1;
            this.PageSize = 10;
        }

    }    
    
    public class NonMemberTradeRecord
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

        [Required]
        public int StoreID { get; set; }

        [Display(Name = "销售门市")]
        public Store store { get; set; }

        [Required]
        public int TeacherID { get; set; }

        [Display(Name = "销售人员")]
        public Teacher teacher { get; set; }

        [Display(Name = "消费点数")]
        [Required]
        public Double Point { get; set; }

        [Display(Name = "消费时间")]
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime TradeDateTime { get; set; }
    }
}