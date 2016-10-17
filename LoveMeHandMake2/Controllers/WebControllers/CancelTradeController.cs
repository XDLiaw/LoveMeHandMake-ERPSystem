using log4net;
using LoveMeHandMake2.Models;
using LoveMeHandMake2.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MvcPaging;

namespace LoveMeHandMake2.Controllers.WebControllers
{
    public class CancelTradeController : Controller
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(CancelTradeController));
        private LoveMeHandMakeContext db = new LoveMeHandMakeContext();

        public ActionResult Index()
        {
            return Index(new CancelTradeViewModel());
        }

        [HttpPost]
        public ActionResult Index(CancelTradeViewModel model)
        {
            DateTime SearchDateEnd_nextDay = model.SearchDateEnd.GetValueOrDefault().AddDays(1);

            model.cancelPagedList = db.TradeOrder
                .Where(x => x.ValidFlag == false)
                .Where(x => model.SearchDateStart == null ? true : model.SearchDateStart <= x.TradeDateTime)
                .Where(x => model.SearchDateEnd == null ? true : x.TradeDateTime <= SearchDateEnd_nextDay)
                .OrderByDescending(x => x.TradeDateTime).ToPagedList(model.PageNumber - 1, model.PageSize);

            return View(model);
        }
    }
}
