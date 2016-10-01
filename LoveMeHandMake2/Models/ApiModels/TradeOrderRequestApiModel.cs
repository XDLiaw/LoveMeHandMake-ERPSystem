using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Models.ApiModels
{
    public class TradeOrderRequestApiModel : BaseRequestApiModel
    {
        [Display(Name = "交易单号")]
        public string OrderID { get; set; }

        [Display(Name = "销售门市")]
        [Required]
        public int StoreID { get; set; }

        [Display(Name = "销售人员")]
        [Required]
        public int TeacherID { get; set; }

        // if MemberID is null means this trade is not sell to a member
        [Display(Name = "会员")]
        public Guid? MemberGuid { get; set; }

        [Display(Name = "扣除点数")]
        public double ChargeByPoint { get; set; }

        [Display(Name = "付现金额")]
        public int ChargeByCash { get; set; }

        [Display(Name = "刷卡金额")]
        public int ChargeByCreditCard { get; set; }

        [Display(Name = "商城卡金额")]
        public int ChargeByMallCard { get; set; }

        [Display(Name = "送金")]
        public int RewardMoney { get; set; }

        [Display(Name = "送點")]
        public double RewardPoint { get; set; }

        [Display(Name = "每点人民币数")]
        public int PointUnitValue { get; set; }

        [Display(Name = "每豆人民币数")]
        public int BeanUnitValue { get; set; }

        [Display(Name = "销售时间")]
        [Required]
        public DateTime TradeDateTime { get; set; }

        public List<PurchaseProductApiModel> ProductList { get; set; }

        public TradeOrderRequestApiModel() { }

        public TradeOrderRequestApiModel(TradeOrder order) 
        {
            this.OrderID = order.OrderID;
            this.StoreID = order.StoreID;
            this.TeacherID = order.TeacherID;
            this.MemberGuid = order.MemberGuid;
            this.ChargeByPoint = order.ChargeByPoint;
            this.ChargeByCash = order.ChargeByCash;
            this.ChargeByCreditCard = order.ChargeByCreditCard;
            this.ChargeByMallCard = order.ChargeByMallCard;
            this.RewardMoney = order.RewardMoney;
            this.RewardPoint = order.RewardPoint;
            this.PointUnitValue = order.PointUnitValue;
            this.BeanUnitValue = order.BeanUnitValue;
            this.TradeDateTime = order.TradeDateTime;
        }

        /// <summary>
        ///     p.s. the return TradeOrder instance hasn't set it's memberID yet
        /// </summary>
        /// <returns></returns>
        public TradeOrder ToTradeOrder()
        {
            TradeOrder res = new TradeOrder();
            res.Create();
            res.OrderID = this.OrderID;
            res.StoreID = this.StoreID;
            res.TeacherID = this.TeacherID;
            res.MemberGuid = this.MemberGuid;
            res.ChargeByPoint = this.ChargeByPoint;
            res.ChargeByCash = this.ChargeByCash;
            res.ChargeByCreditCard = this.ChargeByCreditCard;
            res.ChargeByMallCard = this.ChargeByMallCard;
            res.RewardMoney = this.RewardMoney;
            res.RewardPoint = this.RewardPoint;
            res.PointUnitValue = this.PointUnitValue;
            res.BeanUnitValue = this.BeanUnitValue;
            res.TradeDateTime = this.TradeDateTime;
            return res;
        }

        /// <summary>
        ///     return sum point according to each product's UnitPoint or UnitBean
        /// </summary>
        public double TotalProductsPoint()
        {
            double sum = 0;
            foreach (PurchaseProductApiModel p in this.ProductList)
            {
                if (p.UnitPoint != null && p.UnitPoint != 0)
                {
                    sum += p.UnitPoint.GetValueOrDefault() * p.Amount;
                }
                else if (p.UnitBean != null && p.UnitBean != 0)
                {
                    sum += (double)(p.UnitBean.GetValueOrDefault() * p.Amount) / 2;
                }
            }
            return sum;
        }
    }

    public class PurchaseProductApiModel
    {
        [Required]
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

        public PurchaseProductApiModel() {}

        public PurchaseProductApiModel(TradePurchaseProduct arg)
        {
            this.ProductID = arg.ProductID;
            this.Amount = arg.Amount;
            this.UnitPoint = arg.UnitPoint;
            this.UnitBean = arg.UnitBean;
        }

        public TradePurchaseProduct ToTradePurchaseProduct(int orderID)
        {
            if (this.UnitBean == null && this.UnitPoint == null)
            {
                throw new ArgumentException("UnitBean and UnitPoint can't be both zero!");
            }
            TradePurchaseProduct res = new TradePurchaseProduct();
            res.Create();
            res.OrderID = orderID;
            res.ProductID = this.ProductID;
            res.Amount = this.Amount;
            res.UnitPoint = this.UnitPoint;
            res.UnitBean = this.UnitBean;
            return res;
        }

        public int Sum()
        {
            if (this.UnitBean == null && this.UnitPoint == null)
            {
                throw new ArgumentException("UnitBean and UnitPoint can't be both zero!");
            }
            else if (UnitPoint != null && UnitPoint != 0)
            {
                return this.Amount * this.UnitPoint.GetValueOrDefault();
            }
            else if (UnitBean != null && UnitBean != 0)
            {
                return this.Amount * this.UnitBean.GetValueOrDefault();
            }
            return 0;
        }
    }
}