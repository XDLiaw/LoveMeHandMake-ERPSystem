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
    [Authorize]
    public class NonMemberController : Controller
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(NonMemberController));
        private LoveMeHandMakeContext db = new LoveMeHandMakeContext();

        //private const int pageSize = 5;

        // GET: NonMember
        public ActionResult Index()
        {
            NonMemberTradeHistoryViewModel model = new NonMemberTradeHistoryViewModel();            
            return Index(model);
        }

        [HttpPost]
        public ActionResult Index(NonMemberTradeHistoryViewModel model)
        {
            DateTime SearchDateEnd_nextDay = model.SearchDateEnd.GetValueOrDefault().AddDays(1);
            model.NonMemberTradeRecordList =
            (
                from nmtl in db.NonMemverTradeList
                join nm in db.NonMembers on nmtl.Phone equals nm.Phone
                where (nmtl.ValidFlag == true)
                   && (string.IsNullOrEmpty(model.SearchPhone) ? true : nm.Phone.Equals(model.SearchPhone))
                   && (model.SearchDateStart == null ? true : model.SearchDateStart <= nmtl.TradeDateTime)
                   && (model.SearchDateEnd == null ? true : nmtl.TradeDateTime <= SearchDateEnd_nextDay)
                select new NonMemberTradeRecord
                {
                    Name = nm.Name,
                    Gender = nm.Gender,
                    Birthday = nm.Birthday,
                    Phone = nm.Phone,
                    StoreID = nmtl.StoreID,
                    store = nmtl.Store,
                    TeacherID = nmtl.TeacherID,
                    teacher = nmtl.Teacher,
                    Point = nmtl.Point,
                    TradeDateTime = nmtl.TradeDateTime
                }
            )
            .OrderByDescending(x => x.TradeDateTime)
            .ToPagedList(model.PageNumber - 1, model.PageSize);
            return View(model);
        }
    }
}