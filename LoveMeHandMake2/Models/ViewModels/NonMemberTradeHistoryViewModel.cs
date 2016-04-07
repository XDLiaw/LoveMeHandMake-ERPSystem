using MvcPaging;
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
        public int Page { get; set; }

        //----------------------------------------------------------------------------

        public List<NonMemberTradeRecord> NonMemberTradeRecordList { get; set; }

        public NonMemberTradeHistoryViewModel ()
        {
            this.Page = 0;
            this.NonMemberTradeRecordList = new List<NonMemberTradeRecord>();
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

        public void SetNonMember(NonMember arg)
        {
            this.Name = arg.Name;
            this.Gender = arg.Gender;
            this.Birthday = arg.Birthday;
            this.Phone = arg.Phone;
        }

        public void SetTrade(NonMemberTradeList arg)
        {
            this.StoreID = arg.StoreID;
            this.store = arg.Store;
            this.TeacherID = arg.TeacherID;
            this.teacher = arg.Teacher;
            this.Point = arg.Point;
            this.TradeDateTime = arg.TradeDateTime;
        }
    }
}