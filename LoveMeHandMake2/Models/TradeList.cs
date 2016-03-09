using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Models
{
    public class TradeList : BaseModel
    {
        [Required]
        public string OrderID { get; set; }

        [Required]
        public int StoreID { get; set; }

        [Required]
        public int TeacherID { get; set; }

        [Required]
        public int MemberID { get; set; }

        [Required]
        public int TotalPoint { get; set; }

        public int? ChargeByPoint { get; set; }

        public int? ChargeByCash { get; set; }

        public int? ChargeByCreditCard { get; set; }

        public int? ChargeByMallCard { get; set; }

        public int? RewardMoney { get; set; }

        public int? RewardPoint { get; set; }

        [Required]
        public DateTime TradeDateTime { get; set; }
    }
}