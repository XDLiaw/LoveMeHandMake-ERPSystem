using LoveMeHandMake2.Models.ApiModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Models
{
    public class NonMember : BaseModel
    {
        [Display(Name = "姓名")]
        public string Name { get; set; }

        // 0 -> female, 1 -> male
        [Display(Name = "性别")]
        public bool? Gender { get; set; }

        [Display(Name = "生日")]
        public DateTime? Birthday { get; set; }

        [Display(Name = "电话")]
        [Key, Column(Order = 1)]
        [Required(AllowEmptyStrings = false)]
        [Phone]
        public string Phone { get; set; }

        [Display(Name = "累积消费点数")]
        public double AccumulateConsumePoint { get; set; }

        public void Create(NonMemberTradeApiModel arg)
        {
            base.Create();
            SetBy(arg);
            this.AccumulateConsumePoint = arg.Point;
        }

        public void Update(NonMemberTradeApiModel arg)
        {
            base.Update();
            SetBy(arg);
            this.AccumulateConsumePoint += arg.Point;
        }

        private void SetBy(NonMemberTradeApiModel arg)
        {
            this.Name = arg.Name;
            this.Gender = arg.Gender;
            this.Birthday = arg.Birthday;
            this.Phone = arg.Phone;
        }
    }
}