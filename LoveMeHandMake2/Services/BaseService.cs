using log4net;
using LoveMeHandMake2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Services
{
    public class BaseService
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(MemberService));
        protected LoveMeHandMakeContext db;

        public BaseService()
        {
            this.db = new LoveMeHandMakeContext();
        }

        public BaseService(LoveMeHandMakeContext db)
        {
            this.db = db;
        }
    }
}