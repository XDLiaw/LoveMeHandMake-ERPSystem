using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Models.ViewModels
{
    public class TransferPointViewModel
    {
        [Display(Name = "交易单号")]
        public string OrderID { get; set; }

        public Guid MemberGuid { get; set; }

        [Required]
        public int MemberID { get; set; }

        [Display(Name = "会员")]
        public virtual Member Member { get; set; }

        [Required]
        public int DepositStoreID { get; set; }

        [Display(Name = "储值门市")]
        public virtual Store DepositStore { get; set; }

        [Required]
        public int DepositTeacherID { get; set; }

        [Display(Name = "储值人员")]
        public virtual Teacher DepositTeacher { get; set; }

        [Display(Name = "点数")]
        [Range(0, Double.MaxValue)]
        public double Point { get; set; }

        [Display(Name = "每点平均成本")]
        [DisplayFormat(DataFormatString = "{0:F2}")]
        [Range(0, Double.MaxValue)]
        public double AvgPointCost { get; set; }

        [Display(Name = "储值时间")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime DepostitDateTime { get; set; }

        public void setMember(Member member)
        {
            this.MemberID = member.ID;
            this.MemberGuid = member.MemberGuid;
            this.Member = member;
        }

        public DepositHistory toDepositHistory()
        {
            DepositHistory dh = new DepositHistory();
            dh.OrderID = this.OrderID;
            dh.MemberID = this.MemberID;
            dh.MemberGuid = this.MemberGuid;
            dh.DepositStoreID = this.DepositStoreID;
            dh.DepositStore = this.DepositStore;
            dh.DepositTeacherID = this.DepositTeacherID;
            dh.DepositTeacher = this.DepositTeacher;
            dh.TotalPoint = this.Point;
            dh.AvgPointCost = this.AvgPointCost;
            dh.DepostitDateTime = this.DepostitDateTime;
            return dh;
        }
    }
}