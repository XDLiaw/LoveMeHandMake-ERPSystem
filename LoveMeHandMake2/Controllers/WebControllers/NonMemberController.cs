using log4net;
using LoveMeHandMake2.Models;
using LoveMeHandMake2.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LoveMeHandMake2.Controllers.WebControllers
{
    public class NonMemberController : Controller
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(NonMemberController));
        private LoveMeHandMakeContext db = new LoveMeHandMakeContext();

        private const int pageSize = 5;

        // GET: NonMember
        public ActionResult Index()
        {
            NonMemberTradeHistoryViewModel model = new NonMemberTradeHistoryViewModel();
            List<NonMemberTradeList> tradeList = db.NonMemverTradeList.Where(x => x.ValidFlag == true).OrderByDescending(x => x.TradeDateTime).ToList();
            foreach (NonMemberTradeList trade in tradeList)
            {
                NonMemberTradeRecord t = new NonMemberTradeRecord();
                t.SetTrade(trade);
                NonMember nonM = db.NonMembers.Where(x => x.Phone == trade.Phone && x.ValidFlag == true).FirstOrDefault();
                if (nonM != null)
                {
                    t.SetNonMember(nonM);
                }
                model.NonMemberTradeRecordList.Add(t);
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(NonMemberTradeHistoryViewModel model)
        {
            List<NonMemberTradeList> tradeList = db.NonMemverTradeList
                .Where(x => x.ValidFlag == true)
                .Where(x => (string.IsNullOrEmpty(model.SearchPhone) ? true : x.Phone.Equals(model.SearchPhone)))
                .Where(x => model.SearchDateStart == null ? true : model.SearchDateStart <= x.TradeDateTime )
                .Where(x => model.SearchDateEnd == null ? true : x.TradeDateTime <= model.SearchDateEnd)                
                .OrderByDescending(x => x.TradeDateTime).ToList();
            foreach (NonMemberTradeList trade in tradeList)
            {
                NonMemberTradeRecord t = new NonMemberTradeRecord();
                t.SetTrade(trade);
                NonMember nonM = db.NonMembers.Where(x => x.Phone == trade.Phone && x.ValidFlag == true).FirstOrDefault();
                if (nonM != null)
                {
                    t.SetNonMember(nonM);
                }
                model.NonMemberTradeRecordList.Add(t);
            }

            return View(model);
        }
    }
}