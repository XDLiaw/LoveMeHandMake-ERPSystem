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

        // GET: NonMember
        public ActionResult Index()
        {
            List<NonMemberTradeList> tradeList = db.NonMemverTradeList.Where(x => x.ValidFlag == true).OrderByDescending(x => x.TradeDateTime).ToList();
            List<NonMemberTradeHistoryViewModel> resList = new List<NonMemberTradeHistoryViewModel>();
            foreach (NonMemberTradeList trade in tradeList)
            {
                NonMemberTradeHistoryViewModel t = new NonMemberTradeHistoryViewModel();
                t.SetTrade(trade);
                NonMember nonM = db.NonMembers.Where(x => x.Phone == trade.Phone && x.ValidFlag == true).FirstOrDefault();
                if (nonM != null)
                {
                    t.SetNonMember(nonM);
                }
                resList.Add(t);
            }

            return View(resList);
        }

        [HttpPost]
        public ActionResult Index(FormCollection formCollection)
        {
            string searchPhone = formCollection["searchPhone"];
            DateTime dateStart = DateTime.MinValue;
            DateTime dateEnd = DateTime.MaxValue;
            try
            {
                if (!String.IsNullOrWhiteSpace(formCollection["dateStart"]))
                {
                    dateStart = Convert.ToDateTime(formCollection["dateStart"]);
                }
            }
            catch (FormatException e)
            {
                log.Warn(null, e);
            }
            try
            {
                if (!String.IsNullOrWhiteSpace(formCollection["dateEnd"]))
                {
                    dateEnd = Convert.ToDateTime(formCollection["dateEnd"]);
                }
            }
            catch (FormatException e)
            {
                log.Warn(null, e);
            }

            List<NonMemberTradeList> tradeList = db.NonMemverTradeList
                .Where(x => x.ValidFlag == true)
                .Where(x => (string.IsNullOrEmpty(searchPhone) ? true : x.Phone.Equals(searchPhone)))
                .Where(x => (DateTime.Compare(dateStart, x.TradeDateTime) <= 0))
                .Where(x => (DateTime.Compare(x.TradeDateTime, dateEnd) <= 0))
                .OrderByDescending(x => x.TradeDateTime).ToList();
            List<NonMemberTradeHistoryViewModel> resList = new List<NonMemberTradeHistoryViewModel>();
            foreach (NonMemberTradeList trade in tradeList)
            {
                NonMemberTradeHistoryViewModel t = new NonMemberTradeHistoryViewModel();
                t.SetTrade(trade);
                NonMember nonM = db.NonMembers.Where(x => x.Phone == trade.Phone && x.ValidFlag == true).FirstOrDefault();
                if (nonM != null)
                {
                    t.SetNonMember(nonM);
                }
                resList.Add(t);
            }

            return View(resList);
        }
    }
}