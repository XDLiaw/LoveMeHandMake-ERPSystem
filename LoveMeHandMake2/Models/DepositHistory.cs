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
            this.RewardMoney = 0;
            this.RewardPoint = 0;

            LoveMeHandMakeContext db = new LoveMeHandMakeContext();
            SysParameter sp = db.SysParameter.Where(r => r.Key.Equals("PointValue")).First();
            this.PointUnitValue = Convert.ToInt32(sp.Value);
        }


        [Required]
        public int MemberID { get; set; }

        [Display(Name = "会员")]
        public virtual Member Member { get; set; }

        [Required]
        public int DepositStoreID { get; set; }

        [Display(Name = "储值分店")]
        public virtual Store DepositStore { get; set; }

        [Required]
        public int DepositTeacherID { get; set; }

        [Display(Name = "储值人员")]
        public virtual Teacher DepositTeacher { get; set; }

        [Display(Name = "现金")]
        public int? Cash { get; set; }

        [Display(Name = "信用卡")]
        public int? CreditCard { get; set; }

        [Display(Name = "商城卡")]
        public int? MallCard { get; set; }

        [Display(Name = "送金")]
        public int? RewardMoney { get; set; }

        [Display(Name = "送点")]
        public int? RewardPoint { get; set; }

        [Display(Name = "每\"点\"价值(人民币)")]
        public int PointUnitValue { get; set; }

        [NotMapped]
        public virtual List<DepositRewardRule> DepositRewardRule { get; set; }

        [Display(Name = "总储值金额")]
        public int TotalDepositMoney { get; set; }

        [Display(Name = "储值点数")]
        public int DepositPoint { get; set; }

        [Display(Name = "储值满额送点")]
        public int DepositRewardPoint { get; set; }

        // 有使用到的 累計儲值 滿額送點 規則
        [NotMapped]
        public DepositRewardRule AccumulateDepositRewardRule { get; set; }

         [Display(Name = "总新增点数")]
        public int TotalPoint { get; set; }

        [Display(Name = "每点平均成本")]
        public double AvgPointCost { get; set; }

        [Display(Name = "储值时间")]
        public DateTime DepostitDate { get; set; }

        //===========================================================================================================================================================

        public override void Create()
        {
            base.Create();
            this.DepostitDate = System.DateTime.Now;
        }

        public void computeAll()
        {
            computeTotalDepositMoney();
            computeDepositPoint();
            ComputeDepositRewardPoint();
            this.TotalPoint = this.DepositPoint + this.RewardPoint.GetValueOrDefault() + this.DepositRewardPoint;
            computeAvgCost();
        }

        private void computeTotalDepositMoney() {
            int cash = this.Cash.GetValueOrDefault();
            int credit = this.CreditCard.GetValueOrDefault();
            int mall = this.MallCard.GetValueOrDefault();
            this.TotalDepositMoney = cash + credit + mall;
        }

        private void computeDepositPoint()
        {
            if (this.TotalDepositMoney % this.PointUnitValue != 0)
            {
                int possibleDeposit = TotalDepositMoney - (TotalDepositMoney % this.PointUnitValue);
                throw new ArgumentException("只可储值可整除金额: " + possibleDeposit);
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
            foreach (DepositRewardRule rule in this.DepositRewardRule)
            {
                DateTime now = DateTime.Now;
                DateTime start = rule.ValidDateStart.GetValueOrDefault(now);
                DateTime end = rule.ValidDateEnd.GetValueOrDefault(now);
                if (IsInPeriod(now, start, end))
                {
                    if (rule.AccumulateFlag)
                    {
                        if (accumulateDeposit >= rule.DepositAmount)
                        {
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

        private bool IsInPeriod(DateTime t, DateTime start, DateTime end)
        {
            bool isAfterStart = DateTime.Compare(start, t) <= 0;
            bool isBeforeEnd = DateTime.Compare(t, end) <= 0;
            return isAfterStart && isBeforeEnd;
        }

        private void computeAvgCost()
        {
            try
            {
                int rewardMoney = this.RewardMoney.GetValueOrDefault();
                double avgCost = (this.TotalDepositMoney - rewardMoney) / (this.TotalPoint + 0.0);
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