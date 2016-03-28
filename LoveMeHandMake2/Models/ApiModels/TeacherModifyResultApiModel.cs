using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Models.ApiModels
{
    public class TeacherModifyResultApiModel
    {
        public bool IsModifySuccess { get; set; }

        public List<string> ErrMsgs { get; set; }

        public TeacherModifyResultApiModel()
        {
            this.ErrMsgs = new List<string>();
        }
    }
}