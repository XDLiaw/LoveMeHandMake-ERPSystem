using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Models
{
    public class NonMemberTradeList : BaseModel
    {
        [Required]
        public int StoreID { get; set; }

        [Required]
        public int TeacherID { get; set; }

        [Required]
        public int NonMemberID { get; set; }

        [Required]
        public int Point { get; set; }
    }
}