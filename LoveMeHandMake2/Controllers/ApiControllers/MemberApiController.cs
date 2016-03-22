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
    public class MemberApiController : ApiController
    {
        private LoveMeHandMakeContext db = new LoveMeHandMakeContext();

        [HttpGet]
        public MemberSyncApiModel Synchronize(DateTime lastSynchronizeTime)
        {
            MemberSyncApiModel res = new MemberSyncApiModel();
            res.ReceiveRequestTime = DateTime.Now;
            res.NewList = db.Members
                .Where(x => x.CreateTime > lastSynchronizeTime
                    && x.ValidFlag == true).ToList();
            res.ChangedList = db.Members
                .Where(x => x.CreateTime <= lastSynchronizeTime
                    && x.UpdateTime > lastSynchronizeTime
                    && x.ValidFlag == true).ToList();
            res.RemovedList = db.Members
                .Where(x => x.CreateTime <= lastSynchronizeTime
                    && x.UpdateTime > lastSynchronizeTime
                    && x.ValidFlag == false).ToList();

            return res;
        }

    }
}
