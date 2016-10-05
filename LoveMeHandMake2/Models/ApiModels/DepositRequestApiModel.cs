using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Models.ApiModels
{
    public class DepositRequestApiModel : BaseRequestApiModel
    {
        public string OrderID { get; set; }

        public int StoreID { get; set; }

        public int TeacherID { get; set; }

        public Guid MemberGuid { get; set; }

        public int Cash { get; set; }

        public int CreditCard { get; set; }

        public int MallCard { get; set; }

        public int Alipay { get; set; }

        public int WechatWallet { get; set; }

        public int OtherPay { get; set; }

        public int RewardMoney { get; set; }

        public int RewardPoint { get; set; }

        public int PointUnitValue { get; set; }

        public DateTime DepostitTime { get; set; }

        public DepositHistory ToDepositHistory()
        {
            DepositHistory dh = new DepositHistory();
            dh.Create();
            dh.OrderID = this.OrderID;
            dh.DepositStoreID = this.StoreID;
            dh.DepositTeacherID = this.TeacherID;
            dh.MemberGuid = this.MemberGuid;
            dh.Cash = this.Cash;
            dh.CreditCard = this.CreditCard;
            dh.MallCard = this.MallCard;
            dh.Alipay = this.Alipay;
            dh.WechatWallet = this.WechatWallet;
            dh.OtherPay = this.OtherPay;
            dh.RewardMoney = this.RewardMoney;
            dh.RewardPoint = this.RewardPoint;
            dh.PointUnitValue = this.PointUnitValue;
            dh.DepostitDateTime = this.DepostitTime;
            return dh;
        }

    }
}