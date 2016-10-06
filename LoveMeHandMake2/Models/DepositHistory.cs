using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Models
{
    public class DepositHistory : BaseModel
    {

        public DepositHistory()
        {
            this.Cash = 0;
            this.CreditCard = 0;
            this.MallCard = 0;
            this.Alipay = 0;
            this.WechatWallet = 0;
            this.OtherPay = 0;
            this.RewardMoney = 0;
            this.RewardPoint = 0;

            LoveMeHandMakeContext db = new LoveMeHandMakeContext();
            SysParameter sp = db.SysParameter.Where(r => r.Key.Equals("PointValue")).First();
            this.PointUnitValue = Convert.ToInt32(sp.Value);
        }

        [Display(Name = "交易单号")]
        [Key, Column(Order = 1)]
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

        //-------------------------------------------------------------------------------------

        [Display(Name = "现金")]
        public int? Cash { get; set; }

        [Display(Name = "信用卡")]
        public int? CreditCard { get; set; }

        [Display(Name = "商城卡")]
        public int? MallCard { get; set; }

        [Display(Name = "支付宝")]
        public int? Alipay { get; set; }

        [Display(Name = "微信支付")]
        public int? WechatWallet { get; set; }

        [Display(Name = "其他支付")]
        public int? OtherPay { get; set; }

        [Display(Name = "送金")]
        public int? RewardMoney { get; set; }

        [Display(Name = "送点")]
        public int? RewardPoint { get; set; }

        //-------------------------------------------------------------------------------------

        [Display(Name = "每点人民币数")]
        public int PointUnitValue { get; set; }

        [NotMapped]
        public virtual List<DepositRewardRule> DepositRewardRuleList { get; set; }

        [Display(Name = "总储值金额")]
        public int TotalDepositMoney { get; set; }

        [Display(Name = "储值点数")]
        public int DepositPoint { get; set; }

        [Display(Name = "储值满额送点")]
        public int DepositRewardPoint { get; set; }

        [ScaffoldColumn(false)]
        public int? AccumulateDepositRewardRuleID { get; set; }

        // 有使用到的 累計儲值 滿額送點 規則
        public virtual DepositRewardRule AccumulateDepositRewardRule { get; set; }

        [Display(Name = "总新增点数")]
        public double TotalPoint { get; set; }

        [Display(Name = "每点平均成本")]
        [DisplayFormat(DataFormatString = "{0:F2}")]
        public double AvgPointCost { get; set; }

        [Display(Name = "储值时间")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime DepostitDateTime { get; set; }

        //===========================================================================================================================================================

        public void computeAll()
        {
            computeTotalDepositMoney();
            computeDepositPoint();
            ComputeDepositRewardPoint();
            this.TotalPoint = this.DepositPoint + this.RewardPoint.GetValueOrDefault() + this.DepositRewardPoint;
            computeAvgCost();
        }

        private void computeTotalDepositMoney() {
            this.TotalDepositMoney = 
                this.Cash.GetValueOrDefault() +
                this.CreditCard.GetValueOrDefault() +
                this.MallCard.GetValueOrDefault() + 
                this.Alipay.GetValueOrDefault() + 
                this.WechatWallet.GetValueOrDefault() +
                this.OtherPay.GetValueOrDefault() +
                this.RewardMoney.GetValueOrDefault();
        }

        private void computeDepositPoint()
        {
            if (this.TotalDepositMoney % this.PointUnitValue != 0)
            {
                int possibleDeposit = this.TotalDepositMoney - (this.TotalDepositMoney % this.PointUnitValue);
                if (possibleDeposit < this.PointUnitValue)
                {
                    possibleDeposit = this.PointUnitValue;
                }
                throw new ArgumentException("'总金额'必须可整除'每点人民币数', 建议储值: " + possibleDeposit);
            }
            else
            {
                this.DepositPoint = TotalDepositMoney / this.PointUnitValue;
            }
        }

        private void ComputeDepositRewardPoint()
        {
            int accumulateDeposit = this.Member.AccumulateDeposit + this.TotalDepositMoney;
            int accumulateRewardPoint = 0;
            int nonAccumulateRewardPoint = 0;
            foreach (DepositRewardRule rule in this.DepositRewardRuleList)
            {
                DateTime now = DateTime.Now;
                DateTime start = rule.ValidDateStart.GetValueOrDefault(DateTime.MinValue);
                DateTime end = rule.ValidDateEnd.GetValueOrDefault(DateTime.MaxValue);
                bool isAfterStart = DateTime.Compare(start, now) <= 0;
                bool isBeforeEnd = DateTime.Compare(now, end) < 0;
                if (isAfterStart && isBeforeEnd)
                {
                    if (rule.AccumulateFlag)
                    {
                        if (accumulateDeposit >= rule.DepositAmount)
                        {
                            this.AccumulateDepositRewardRuleID = rule.ID;
                            this.AccumulateDepositRewardRule = rule;
                            accumulateRewardPoint = rule.RewardPoint;
                        }
                    }
                    else
                    {
                        if (this.TotalDepositMoney >= rule.DepositAmount)
                        {
                            nonAccumulateRewardPoint = rule.RewardPoint;
                        }
                    }
                }
            }
            this.DepositRewardPoint = accumulateRewardPoint + nonAccumulateRewardPoint;
        }

        private void computeAvgCost()
        {
            try
            {
                double avgCost = (this.TotalDepositMoney - this.RewardMoney.GetValueOrDefault()) / (this.TotalPoint + 0.0);
                this.AvgPointCost =  Double.IsNaN(avgCost) ? 0 : avgCost;
            }
            catch (DivideByZeroException e)
            {
                log.Warn(null, e);
                this.AvgPointCost = 0;
            }
        }

    }
}