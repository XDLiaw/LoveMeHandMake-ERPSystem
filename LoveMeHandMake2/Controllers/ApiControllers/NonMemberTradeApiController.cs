using log4net;
using LoveMeHandMake2.Models;
using LoveMeHandMake2.Models.ApiModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LoveMeHandMake2.Controllers.ApiControllers
{
    public class NonMemberTradeApiController : ApiController
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(NonMemberTradeApiController));
        private LoveMeHandMakeContext db = new LoveMeHandMakeContext();

        [HttpPost]
        public NonMemberTradeResultApiModel NewRecord(NonMemberTradeApiModel arg)
        {
            log.Info(JsonConvert.SerializeObject(arg));
            NonMemberTradeResultApiModel res = new NonMemberTradeResultApiModel();
            res.ReceiveRequestTime = DateTime.Now;
            res.IsRequestSuccess = false;

            try
            {
                if (arg.IsValid() == false)
                {
                    log.Error(arg.GetInvalidReasons());
                    res.ErrMsgs.AddRange(arg.GetInvalidReasons());
                    return res;
                }
                NonMember nonMember = db.NonMembers.Where(x => x.Phone == arg.Phone).FirstOrDefault();
                if (nonMember == null)
                {
                    nonMember = new NonMember();
                    nonMember.Create(arg);
                    db.NonMembers.Add(nonMember);
                }
                else
                {
                    nonMember.Update(arg);
                    db.Entry(nonMember).State = System.Data.Entity.EntityState.Modified;
                }

                NonMemberTradeList trade = new NonMemberTradeList();
                trade.Create(arg);
                db.NonMemverTradeList.Add(trade);

                db.SaveChanges();
                res.IsRequestSuccess = true;
            }
            catch (Exception e)
            {
                log.Error(null, e);
                res.ErrMsgs.Add(e.Message);
                res.IsRequestSuccess = false;
            }
            return res;
        }
    }
}
