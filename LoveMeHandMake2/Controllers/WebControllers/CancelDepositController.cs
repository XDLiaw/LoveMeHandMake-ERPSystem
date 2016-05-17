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
    public class CancelDepositController : Controller
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(CancelDepositController));
        private LoveMeHandMakeContext db = new LoveMeHandMakeContext();

        // GET: CancelTransaction
        public ActionResult Index()
        {
            CancelDepositViewModel model = new CancelDepositViewModel();
            model.cancelList = db.DepositHistory.Where(x => x.ValidFlag == false).ToList();
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(CancelDepositViewModel arg)
        {
            arg.cancelList = db.DepositHistory
                .Where(x => x.ValidFlag == false
                    && (arg.SearchDateStart == null ? true : arg.SearchDateStart <= x.DepostitDateTime) 
                    && (arg.SearchDateEnd == null ? true : x.DepostitDateTime <= arg.SearchDateEnd)).ToList();
            return View(arg);
        }

    }
}
