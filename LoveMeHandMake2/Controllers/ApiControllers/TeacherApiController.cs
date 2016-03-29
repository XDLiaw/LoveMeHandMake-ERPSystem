using log4net;
using LoveMeHandMake2.Helper;
using LoveMeHandMake2.Models;
using LoveMeHandMake2.Models.ApiModels;
using Newtonsoft.Json;
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
        private static readonly ILog log = LogManager.GetLogger(typeof(TeacherApiController));
        private LoveMeHandMakeContext db = new LoveMeHandMakeContext();

        [HttpGet]
        public Object Synchronize(DateTime lastSynchronizeTime)
        {
            DateTime receiveRequestTime = DateTime.Now;
            List<Teacher> newTeachers = db.Teachers
                .Where(x => x.CreateTime > lastSynchronizeTime
                    && x.ValidFlag == true).ToList();
            List<Teacher> changedTeachers = db.Teachers
                .Where(x => x.CreateTime <= lastSynchronizeTime
                    && x.UpdateTime > lastSynchronizeTime
                    && x.ValidFlag == true).ToList();
            List<Teacher> removedTeachers = db.Teachers
                .Where(x => x.CreateTime <= lastSynchronizeTime
                    && x.UpdateTime > lastSynchronizeTime
                    && x.ValidFlag == false).ToList();

            foreach (Teacher t in newTeachers)
            {
                t.Password = AESEncrypter.Encrypt(t.Password);
                t.PreviousPassword = AESEncrypter.Encrypt(t.PreviousPassword);
            }


            var res = new { 
                ReceiveRequestTime = receiveRequestTime,
                NewTeachers = newTeachers,
                ChangedTeachers = changedTeachers,
                RemovedTeachers = removedTeachers
            };

            return res;
        }

        public TeacherModifyResultApiModel ModifyPassword(TeacherModifyRequestApiModel arg)
        {
            log.Info(JsonConvert.SerializeObject(arg));
            TeacherModifyResultApiModel res = new TeacherModifyResultApiModel();
            if (arg.IsValid() == false)
            {
                res.ErrMsgs.AddRange(arg.GetInvalidReasons());
                res.IsRequestSuccess = false;
                return res;
            }
            try {
                Teacher t = db.Teachers.Where(x => x.ID == arg.ID && x.ValidFlag == true).FirstOrDefault();
                if (t == null)
                {
                    res.ErrMsgs.Add("Teacher doesn't exist!");
                    return res;
                }
                t.PreviousPassword = t.Password;
                t.Password = AESEncrypter.Decrypt(arg.Password);
                t.Update();
                db.Entry(t).State = EntityState.Modified;
                db.SaveChanges();
                res.IsRequestSuccess = true;
                return res;
            }
            catch (Exception e)
            {
                log.Error(null, e);
                res.ErrMsgs.Add(e.Message);
                res.IsRequestSuccess = false;
                return res;
            }

        }
    }
}
