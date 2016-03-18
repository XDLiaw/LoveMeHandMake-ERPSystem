using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Models
{
    public class Store : BaseModel
    {
        [Required]
        [Display(Name = "门市代码")]
        public string StoreCode { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "不能为空")]
        [StringLength(50)]
        [Display(Name = "门市名称")]
        public string Name { get; set; }

        [Display(Name = "过槛每点直抽奖金")]
        public int OverThresholdBonus { get; set; }

        [Display(Name = "可销售产品分类")]
        public virtual ICollection<StoreCanSellCategory> StoreCanSellCategories { get; set; }

        [Display(Name = "歇业日期")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? StopBusinessDate { get; set; }

        public bool IsSellableCategory(int categoryID)
        {
            foreach (StoreCanSellCategory cat in this.StoreCanSellCategories)
            {
                if (categoryID == cat.ProductCategoryID) return true;
            }
            return false;
        }

        //public override void Update()
        //{
        //    base.Update();
        //    if (this.StopBusinessDate != null)
        //    {
        //        this.ValidFlag = false;
        //    }
        //}

    }
}