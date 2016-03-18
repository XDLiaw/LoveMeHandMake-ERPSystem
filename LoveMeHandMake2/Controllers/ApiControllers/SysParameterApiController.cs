using LoveMeHandMake2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LoveMeHandMake2.Controllers.ApiControllers
{
    public class SysParameterApiController : ApiController
    {
        private LoveMeHandMakeContext db = new LoveMeHandMakeContext();

        [HttpGet]
        public List<SysParameter> Synchronize()
        {
            List<SysParameter> result = db.SysParameter.Where(x => x.ValidFlag == true).ToList();
            return result;
        }

    }
}
