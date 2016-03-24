using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Models
{
    /// <summary>
    ///     This table is used to record the usage of every point, 
    ///     such like this point is created because of which deposit
    ///     and be used to which trade order.
    /// </summary>
    public class PointUsage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid guid { get; set; }

        public int MemberID { get; set; }

        /// <summary>
        ///     Deposit order which create this point
        /// </summary>
        public int DepositOrderID { get; set; }

        /// <summary>
        ///     The trade order who used this point.
        ///     If this property is null, it means this point hasn't been used yet.
        /// </summary>
        public int? TradeOrderID { get; set; }

        /// <summary>
        ///     During deposit process would calculate out the value of each point in current deposit, so we record it here as well
        /// </summary>
        public double PointValue { get; set; }

        public DateTime DepositTime { get; set; }
    }
}