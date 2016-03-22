using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Models.ApiModels
{
    public class TeacherModifyResultApiModel
    {
        public bool IsModifySuccess { get; set; }

        private string _ErrMsg;

        public string ErrMsg
        {
            get
            {
                return this._ErrMsg;
            }
            set
            {
                this._ErrMsg = value;
                this.IsModifySuccess = false;
            }
        }

        public TeacherModifyResultApiModel()
        {
            this.IsModifySuccess = true;
        }
    }
}