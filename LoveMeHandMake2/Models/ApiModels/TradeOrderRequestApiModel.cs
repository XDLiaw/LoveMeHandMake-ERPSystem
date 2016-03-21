using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Models.ApiModels
{
    public class TradeOrderRequestApiModel
    {
        [Display(Name = "交易单号")]
        public string OrderID { get; set; }

        [Required]
        public int StoreID { get; set; }

        [Required]
        public int TeacherID { get; set; }

        // if MemberID is null means this trade is not sell to a member
        public int? MemberID { get; set; }

        [Display(Name = "扣除点数")]
        public int ChargeByPoint { get; set; }

        [Display(Name = "付现金额")]
        public int ChargeByCash { get; set; }

        [Display(Name = "刷卡金额")]
        public int ChargeByCreditCard { get; set; }

        [Display(Name = "商城卡金额")]
        public int ChargeByMallCard { get; set; }

        [Display(Name = "送金")]
        public int RewardMoney { get; set; }

        [Display(Name = "送點")]
        public int RewardPoint { get; set; }

        [Display(Name = "每点人民币数")]
        public int PointUnitValue { get; set; }

        [Display(Name = "销售时间")]
        [Required]
        public DateTime TradeDateTime { get; set; }

        public List<PurchaseProductApiModel> ProductList { get; set; }

        public TradeOrder ToTradeOrder()
        {
            TradeOrder res = new TradeOrder();
            res.Create();
            res.OrderID = this.OrderID;
            res.StoreID = this.StoreID;
            res.TeacherID = this.TeacherID;
            res.MemberID = this.MemberID;
            res.ChargeByPoint = this.ChargeByPoint;
            res.ChargeByCash = this.ChargeByCash;
            res.ChargeByCreditCard = this.ChargeByCreditCard;
            res.ChargeByMallCard = this.ChargeByMallCard;
            res.RewardMoney = this.RewardMoney;
            res.RewardPoint = this.RewardPoint;
            return res;
        }

        
        public bool CheckPoint()
        {
            //TODO

            return true;
        }
    }

    public class PurchaseProductApiModel
    {
        public int ProductID { get; set; }

        [Display(Name = "数量")]
        [Required]
        public int Amount { get; set; }

        [Display(Name = "单价(点)")]
        // How many points for each one of this product
        public int? UnitPoint { get; set; }

        [Display(Name = "单价(豆)")]
        // How many beans for each one of this product
        public int? UnitBean { get; set; }

        public TradePurchaseProduct ToTradePurchaseProduct(int orderID)
        {
            TradePurchaseProduct res = new TradePurchaseProduct();
            res.Create();
            res.OrderID = orderID;
            res.ProductID = this.ProductID;
            res.Amount = this.Amount;
            res.UnitPoint = this.UnitPoint;
            res.UnitBean = this.UnitBean;
            return res;
        }


    }
}