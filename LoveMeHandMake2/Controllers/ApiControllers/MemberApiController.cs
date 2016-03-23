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
        public Object Synchronize(DateTime lastSynchronizeTime)
        {
            DateTime receiveRequestTime = DateTime.Now;
            List<Member> newMembers = db.Members
                .Where(x => x.CreateTime > lastSynchronizeTime
                    && x.ValidFlag == true).ToList();
            List<Member> changedMembers = db.Members
                .Where(x => x.CreateTime <= lastSynchronizeTime
                    && x.UpdateTime > lastSynchronizeTime
                    && x.ValidFlag == true).ToList();
            List<Member> removedMembers = db.Members
                .Where(x => x.CreateTime <= lastSynchronizeTime
                    && x.UpdateTime > lastSynchronizeTime
                    && x.ValidFlag == false).ToList();

            var res = new
            {
                ReceiveRequestTime = receiveRequestTime,
                NewMembers = newMembers,
                ChangedMembers = changedMembers,
                RemovedMembers = removedMembers
            };

            return res;
        }
    }
}
