using LoveMeHandMake2.Models;
using LoveMeHandMake2.Models.ApiModels;
using System;
using System.Collections.Generic;
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
        public TeacherApiModelO Synchronize(DateTime lastSynchronozeTime)
        {
            TeacherApiModelO res = new TeacherApiModelO();
            res.ReceiveRequestTime = DateTime.Now;
            res.NewTeachers = db.Teachers
                .Where(x => x.CreateTime > lastSynchronozeTime
                    && x.ValidFlag == true).ToList();
            res.ChangedTeachers = db.Teachers
                .Where(x => x.CreateTime <= lastSynchronozeTime
                    && x.UpdateTime > lastSynchronozeTime
                    && x.ValidFlag == true).ToList();
            res.RemovedTeachers = db.Teachers
                .Where(x => x.CreateTime <= lastSynchronozeTime
                    && x.UpdateTime > lastSynchronozeTime
                    && x.ValidFlag == false).ToList();

            return res;
        }


    }
}
