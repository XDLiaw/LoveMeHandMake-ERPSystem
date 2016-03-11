using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Models
{
    public class DepositHistory : BaseModel
    {
        [Required]
        public int StoreID { get; set; }

        [Required]
        public int TeacherID { get; set; }

        [Required]
        public int MemberID { get; set; }

        [Required]
        public int DepositPoint { get; set; }

        public int Cash { get; set; }

        public int CreditCard { get; set; }

        public int MallCard { get; set; }

        public int RewardMoney { get; set; }

        public int RewardPoint { get; set; }

        // AvgPointCost = ( Cash + CreditCard + MallCard - RewardMoney ) / ( DepositePoint + RewardPoint )
        public int AvgPointCost { get; set; }

        [Required]
        public DateTime DepostitDate { get; set; }
    }
}