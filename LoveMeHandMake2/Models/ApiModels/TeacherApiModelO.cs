using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Models.ApiModels
{
    public class TeacherApiModelO
    {
        public DateTime ReceiveRequestTime { get; set; }

        public List<Teacher> NewTeachers { get; set; }

        public List<Teacher> ChangedTeachers { get; set; }

        public List<Teacher> RemovedTeachers { get; set; }
    }
}