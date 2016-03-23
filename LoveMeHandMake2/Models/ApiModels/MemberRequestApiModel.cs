using LoveMeHandMake2.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Models.ApiModels
{
    public class MemberRequestApiModel : BaseRequestApiModel
    {
        public Member member { get; set; }
    }
}