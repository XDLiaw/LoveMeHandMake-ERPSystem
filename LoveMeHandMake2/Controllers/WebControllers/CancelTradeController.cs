using log4net;
using LoveMeHandMake2.Models;
using LoveMeHandMake2.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace LoveMeHandMake2.Controllers.WebControllers
{
    public class CancelTradeController : Controller
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(CancelTradeController));
        private LoveMeHandMakeContext db = new LoveMeHandMakeContext();

        public ActionResult Index()
        {
            CancelTradeViewModel model = new CancelTradeViewModel();
            model.cancelList = db.TradeOrder.Where(x => x.ValidFlag == false).ToList();
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(CancelTradeViewModel arg)
        {
            arg.cancelList = db.TradeOrder.Where(x => x.ValidFlag == false 
                && (arg.SearchDateStart == null ? true : arg.SearchDateStart <= x.TradeDateTime) 
                && (arg.SearchDateEnd == null ? true : x.TradeDateTime <= arg.SearchDateEnd)).ToList();
            return View(arg);
        }
    }
}
