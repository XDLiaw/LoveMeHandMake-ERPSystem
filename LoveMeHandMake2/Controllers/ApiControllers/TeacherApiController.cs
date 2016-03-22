using LoveMeHandMake2.Models;
using LoveMeHandMake2.Models.ApiModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LoveMeHandMake2.Controllers.ApiControllers
{
    public class TeacherApiController : ApiController
    {
        private LoveMeHandMakeContext db = new LoveMeHandMakeContext();

        [HttpGet]
        public TeacherSyncApiModel Synchronize(DateTime lastSynchronizeTime)
        {
            TeacherSyncApiModel res = new TeacherSyncApiModel();
            res.ReceiveRequestTime = DateTime.Now;
            res.NewList = db.Teachers
                .Where(x => x.CreateTime > lastSynchronizeTime
                    && x.ValidFlag == true).ToList();
            res.ChangedList = db.Teachers
                .Where(x => x.CreateTime <= lastSynchronizeTime
                    && x.UpdateTime > lastSynchronizeTime
                    && x.ValidFlag == true).ToList();
            res.RemovedList = db.Teachers
                .Where(x => x.CreateTime <= lastSynchronizeTime
                    && x.UpdateTime > lastSynchronizeTime
                    && x.ValidFlag == false).ToList();
            res.EncryptPassword();

            return res;
        }

        public TeacherModifyResultApiModel ModifyPassword(TeacherModifyRequestApiModel arg)
        {
            TeacherModifyResultApiModel res = new TeacherModifyResultApiModel();
            if (arg.IsValid() == false)
            {
                res.ErrMsg  = arg.GetInvalidReasons().First();
                return res;
            }
            Teacher t = db.Teachers.Where(x => x.ID == arg.ID && x.ValidFlag == true).FirstOrDefault();
            if (t == null)
            {
                res.ErrMsg = "Teacher doesn't exist!";
                return res;
            }
            t.PreviousPassword = t.Password;
            t.Password = arg.Password;
            t.Update();
            db.Entry(t).State = EntityState.Modified;
            db.SaveChanges();

            return res;
        }
    }
}
