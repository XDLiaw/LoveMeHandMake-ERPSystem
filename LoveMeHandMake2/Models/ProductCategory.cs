using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Models
{
    public class ProductCategory : BaseModel
    {
        public enum PriceUnit { Point = 1, Bean = 2 }

        [Required(AllowEmptyStrings = false, ErrorMessage = "不能为空")]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "不能为空")]
        //public String Unit { get; set; }

        private int _Unit;

        public int Unit
        {
            get { return this._Unit; }
            set
            {
                if (value == (int)PriceUnit.Bean || value == (int)PriceUnit.Point)
                {
                    this._Unit = value;
                }
                else
                {
                    throw new ArgumentException("Non exist unit!");
                }
            }
        }
    }
}