using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Models
{
    public class Member : BaseModel
    {
        [Required]
        public Guid MemberGuid { get; set; }

        [Display(Name = "姓名")]
        [Required(AllowEmptyStrings = false)]
        [StringLength(50)]
        public string Name { get; set; }

        [Display(Name = "卡号")]
        [Required(AllowEmptyStrings = false)]
        [StringLength(10)]
        public string CardID { get; set; }

        // 0 -> female, 1 -> male
        [Display(Name = "性别")]
        [Required(AllowEmptyStrings = false)]
        public bool Gender { get; set; }

        [Display(Name = "生日")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        [Required]
        public DateTime Birthday { get; set; }

        [Display(Name = "电话")]
        [Required(AllowEmptyStrings = false)]
        [Phone]
        public string Phone { get; set; }

        [Display(Name = "是否为公关卡")]
        [Required(AllowEmptyStrings = false)]
        public bool IsPRCard { get; set; }
        
        [Required]
        public int EnrollTeacherID { get; set; }

        [Display(Name = "办卡人员")]
        [JsonIgnore]
        public virtual Teacher EnrollTeacher { get; set; }

        [Required]
        public int EnrollStoreID { get; set; }

        [Display(Name = "办卡门市")]
        [JsonIgnore]
        public virtual Store EnrollStore { get; set; }

        [Display(Name = "办卡日期")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        [Required(AllowEmptyStrings = false)]
        public DateTime EnrollDate { get; set; }

        [Display(Name = "剩余点数")]
        public double Point { get; set; }

        [Display(Name = "累计储值金额")]
        public int AccumulateDeposit { get; set; }

        public override void Create()
        {
            base.Create();
            //if (string.IsNullOrEmpty(this.CommunicateGuid))
            //{
            //    this.CommunicateGuid = Guid.NewGuid().ToString();
            //}
            if (this.MemberGuid == null || this.MemberGuid.Equals(Guid.Empty))
            {
                this.MemberGuid = Guid.NewGuid();
                log.Debug("Create a CommunicateGuid=" + MemberGuid);
            }
            this.Point = 0;
            this.AccumulateDeposit = 0;
        }



    }
}