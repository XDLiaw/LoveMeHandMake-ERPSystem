using LoveMeHandMake2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LoveMeHandMake2.Controllers.ApiControllers
{
    public class StoreApiController : ApiController
    {       
        private LoveMeHandMakeContext db = new LoveMeHandMakeContext();

        [HttpGet]
        public Store Synchronize(int StoreID)
        {
            Store res = db.Stores.Where(x => x.ID == StoreID && x.ValidFlag == true).FirstOrDefault();
            return res;
        }

    }
}
