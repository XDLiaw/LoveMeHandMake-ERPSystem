using LoveMeHandMake2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Services
{
    public class TeacherService : BaseService
    {
        public TeacherService() : base() { }

        public TeacherService(LoveMeHandMakeContext db) : base(db) { }

        public bool IsTeacherExist(int teacherID)
        {
            return db.Teachers.Where(x => x.ID == teacherID && x.ValidFlag == true).Count() > 0;
        }

        public bool IsTeacherExist(Teacher teacher)
        {
            return IsTeacherExist(teacher.ID);
        }
    }
}