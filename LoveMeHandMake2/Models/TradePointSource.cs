using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Models
{
    /// <summary>
    /// 此表格主要用來記錄每筆消費訂單中，所使用的點數分別來自於哪一筆儲值
    /// 用以計算此次消費的成本
    /// </summary>
    public class TradePointSource : BaseModel
    {
        [Required]        
        public int OrderID { get; set; }

        [Display(Name = "订单")]
        public virtual TradeOrder Order { get; set; }

        [Required]
        public int DepositRecordID { get; set; }

        public DepositHistory DepositRecord { get; set; }

        public int UsedPoint { get; set; }

        public int DollarsPerPoint { get; set; }

    }
}