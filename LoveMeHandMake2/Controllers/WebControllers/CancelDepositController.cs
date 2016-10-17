using log4net;
using LoveMeHandMake2.Models;
using LoveMeHandMake2.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcPaging;

namespace LoveMeHandMake2.Controllers.WebControllers
{
    public class CancelDepositController : Controller
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(CancelDepositController));
        private LoveMeHandMakeContext db = new LoveMeHandMakeContext();

        // GET: CancelTransaction
        public ActionResult Index()
        {
            return Index(new CancelDepositViewModel());
        }

        [HttpPost]
        public ActionResult Index(CancelDepositViewModel model)
        {
            DateTime SearchDateEnd_nextDay = model.SearchDateEnd.GetValueOrDefault().AddDays(1);

            model.cancelPagedList = db.DepositHistory
                .Where(x => x.ValidFlag == false)
                .Where(x => model.SearchDateStart == null ? true : model.SearchDateStart <= x.DepostitDateTime)
                .Where(x => model.SearchDateEnd == null ? true : x.DepostitDateTime <= SearchDateEnd_nextDay)
                .OrderByDescending(x => x.DepostitDateTime).ToPagedList(model.PageNumber - 1, model.PageSize);

            return View(model);
        }

    }
}
