using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Models
{
    public class TradeOrder : BaseModel
    {
        [Display(Name = "交易单号")]
        [Required]
        public string OrderID { get; set; }

        [Required]
        public int StoreID { get; set; }

        [Display(Name = "銷售門市")]
        public virtual Store Store { get; set; }

        [Required]
        public int TeacherID { get; set; }

        [Display(Name = "销售人员")]
        public virtual Teacher Teacher { get; set; }

        // if MemberID is null means this trade is not sell to a member
        public int? MemberID { get; set; }

        [Display(Name = "会员")]
        public virtual Member Member { get; set; }

        [Display(Name = "总交易点数")]
        [Required]
        // 交易點數 = 耗用點數 + 送點 + (現金 + 刷卡 + 商城卡 + 送金)所對應點數
        public int TotalPoint { get; set; }

        [Display(Name = "扣除点数")]
        public int? ChargeByPoint { get; set; }

        [Display(Name = "付现金额")]
        public int? ChargeByCash { get; set; }

        [Display(Name = "刷卡金额")]
        public int? ChargeByCreditCard { get; set; }

        [Display(Name = "商城卡金额")]
        public int? ChargeByMallCard { get; set; }

        [Display(Name = "送金")]
        public int? RewardMoney { get; set; }

        [Display(Name = "送點")]        
        public int? RewardPoint { get; set; }

        [Display(Name = "销售时间")]
        [Required]
        public DateTime TradeDateTime { get; set; }
    }
}