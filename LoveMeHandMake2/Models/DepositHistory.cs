using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Models
{
    public class DepositHistory : BaseModel
    {
        private LoveMeHandMakeContext db = new LoveMeHandMakeContext();

        public DepositHistory()
        {
            this.Cash = 0;
            this.CreditCard = 0;
            this.MallCard = 0;
            this.RewardMoney = 0;
            this.RewardPoint = 0;
            settingPointUnitValue();
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

        private int _TotalDepositMoney;

        // TotalDepositeMoney = Cash + CreditCard + MallCard
        [Display(Name = "总储值金额")]
        [NotMapped]
        public int TotalDepositMoney
        {
            get
            {
                //int cash = this.Cash.GetValueOrDefault(0);
                //int credit = this.CreditCard.GetValueOrDefault(0);
                //int mall = this.MallCard.GetValueOrDefault(0);
                //return cash + credit + mall;
                return this._TotalDepositMoney;
            }
            set
            {
                if (this.TotalDepositMoney % this.PointUnitValue == 0)
                {
                    this._TotalDepositMoney = value;
                }
                else
                {
                    int onlyNeedMoney = value - (value % this.PointUnitValue);
                    throw new Exception("Only need to deposit $" + onlyNeedMoney);
                }
            }
        }

        [Display(Name = "储值点数")]
        public int DepositPoint { get; set; }

        // TotalPoint = DepositePoint + RewardPoint + DepositRewardRulePoint
        [Display(Name = "总新增点数")]
        [NotMapped]
        public int TotalPoint { get; set; }

        // AvgPointCost = ( TotalDepositeMoney - RewardMoney ) / ( DepositePoint + RewardPoint )
        [Display(Name = "每点平均成本")]
        public double AvgPointCost
        {
            get
            {
                try
                {
                    int rewardMoney = this.RewardMoney.GetValueOrDefault();
                    int rewardPoint = this.RewardPoint.GetValueOrDefault();
                    double avgCost = (this.TotalDepositMoney - rewardMoney) / (this.DepositPoint + rewardPoint + 0.0);
                    return Double.IsNaN(avgCost) ? 0 : avgCost;
                }
                catch (DivideByZeroException e)
                {
                    return 0;
                }
            }          
        }

        [Display(Name = "储值时间")]
        [Required]
        public DateTime DepostitDate { get; set; }

        public override void Create()
        {
            base.Create();
            this.DepostitDate = System.DateTime.Now;
        }

        public int settingPointUnitValue()
        {
            SysParameter sp = db.SysParameter.Where(r => r.Key.Equals("PointValue")).First();
            this.PointUnitValue = Convert.ToInt32(sp.Value);
            return this.PointUnitValue;
        }

        //public int computeDepositRewardPoint()
        //{
            
        //}
    }
}