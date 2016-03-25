using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Models.ApiModels
{
    public class TeacherModifyResultApiModel
    {
        public bool IsModifySuccess { get; set; }

        public List<string> ErrMsgs { get; private set; }

        public TeacherModifyResultApiModel()
        {
            this.IsModifySuccess = true;
            this.ErrMsgs = new List<string>();
        }
    }
}