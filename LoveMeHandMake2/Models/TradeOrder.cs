using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Models
{
    public class TradeOrder : BaseModel
    {
        /// <summary>
        /// 由前台POS機傳入，用以讓前台能夠查詢或刪除交易所使用
        /// </summary>
        [Display(Name = "交易单号")]
        [Required]
        public string OrderID { get; set; }

        [Required]
        public int StoreID { get; set; }

        [Display(Name = "销售门市")]
        [JsonIgnore]
        public virtual Store Store { get; set; }

        [Required]
        public int TeacherID { get; set; }

        [Display(Name = "销售人员")]
        [JsonIgnore]
        public virtual Teacher Teacher { get; set; }

        public Guid? MemberGuid { get; set; }

        // if MemberID is null means this trade is not sell to a member
        public int? MemberID { get; set; }

        [Display(Name = "会员")]
        [JsonIgnore]
        public virtual Member Member { get; set; }

        [Display(Name = "扣点")]
        public double ChargeByPoint { get; set; }

        [Display(Name = "付现")]
        public int ChargeByCash { get; set; }

        [Display(Name = "信用卡")]
        public int ChargeByCreditCard { get; set; }

        [Display(Name = "商城卡")]
        public int ChargeByMallCard { get; set; }

        [Display(Name = "支付宝")]
        public int ChargeByAlipay { get; set; }

        [Display(Name = "微信支付")]
        public int ChargeByWechatWallet { get; set; }

        [Display(Name = "其他支付")]
        public int ChargeByOtherPay { get; set; }

        [Display(Name = "送金")]
        public int RewardMoney { get; set; }

        [Display(Name = "送點")]        
        public double RewardPoint { get; set; }

        /// <summary>
        ///     系統所設定之每點價值，因為前台POS機可能斷線，因此需告知後台當時使用的每點價值
        /// </summary>
        [Display(Name = "每点人民币数")]
        public int PointUnitValue { get; set; }

        [Display(Name = "每豆人民币数")]
        public int BeanUnitValue { get; set; }

        /// <summary>
        ///     TotalValue = ChargeByCash + ChargeByCreditCard + ChargeByMallCard + ChargeByAlipay + ChargeByWechatWallet + ChargeByOtherPay + (Value of ChargeByPoint)
        /// </summary>
        [Display(Name = "实际收入总金额")]
        [DisplayFormat(DataFormatString = "{0:F2}")]
        public double TotalIncomeMoney { get; set; }

        [Display(Name = "销售时间")]
        [Required]
        public DateTime TradeDateTime { get; set; }

        [Display(Name = "錯誤訊息")]
        public String ErrorMsg { get; set; }
    }
}