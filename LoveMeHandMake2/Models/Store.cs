using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Models
{
    public class Store : BaseModel
    {
        [Display(Name = "门市代码")]
        [Required]    
        [StringLength(4)]
        public string StoreCode { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "不能为空")]
        [StringLength(50)]
        [Display(Name = "门市名称")]
        public string Name { get; set; }

        [Display(Name = "1月过槛奖金点数")]
        public int ThresholdPoint1 { get; set; }

        [Display(Name = "1月过槛每点直抽奖金")]
        public int OverThresholdBonus1 { get; set; }

        [Display(Name = "2月过槛奖金点数")]
        public int ThresholdPoint2 { get; set; }

        [Display(Name = "2月过槛每点直抽奖金")]
        public int OverThresholdBonus2 { get; set; }

        [Display(Name = "3月过槛奖金点数")]
        public int ThresholdPoint3 { get; set; }

        [Display(Name = "3月过槛每点直抽奖金")]
        public int OverThresholdBonus3 { get; set; }

        [Display(Name = "4月过槛奖金点数")]
        public int ThresholdPoint4 { get; set; }

        [Display(Name = "4月过槛每点直抽奖金")]
        public int OverThresholdBonus4 { get; set; }

        [Display(Name = "5月过槛奖金点数")]
        public int ThresholdPoint5 { get; set; }

        [Display(Name = "5月过槛每点直抽奖金")]
        public int OverThresholdBonus5 { get; set; }

        [Display(Name = "6月过槛奖金点数")]
        public int ThresholdPoint6 { get; set; }

        [Display(Name = "6月过槛每点直抽奖金")]
        public int OverThresholdBonus6 { get; set; }

        [Display(Name = "7月过槛奖金点数")]
        public int ThresholdPoint7 { get; set; }

        [Display(Name = "7月过槛每点直抽奖金")]
        public int OverThresholdBonus7 { get; set; }

        [Display(Name = "8月过槛奖金点数")]
        public int ThresholdPoint8 { get; set; }

        [Display(Name = "8月过槛每点直抽奖金")]
        public int OverThresholdBonus8 { get; set; }

        [Display(Name = "9月过槛奖金点数")]
        public int ThresholdPoint9 { get; set; }

        [Display(Name = "9月过槛每点直抽奖金")]
        public int OverThresholdBonus9 { get; set; }

        [Display(Name = "10月过槛奖金点数")]
        public int ThresholdPoint10 { get; set; }

        [Display(Name = "10月过槛每点直抽奖金")]
        public int OverThresholdBonus10 { get; set; }

        [Display(Name = "11月过槛奖金点数")]
        public int ThresholdPoint11 { get; set; }

        [Display(Name = "11月过槛每点直抽奖金")]
        public int OverThresholdBonus11 { get; set; }

        [Display(Name = "12月过槛奖金点数")]
        public int ThresholdPoint12 { get; set; }

        [Display(Name = "12月过槛每点直抽奖金")]
        public int OverThresholdBonus12 { get; set; }

        [Display(Name = "可销售产品分类")]
        [JsonIgnore]
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
    }
}