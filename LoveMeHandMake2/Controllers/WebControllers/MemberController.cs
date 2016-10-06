using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LoveMeHandMake2.Models;
using log4net;
using LoveMeHandMake2.Models.ViewModels;
using LoveMeHandMake2.Services;
using MvcPaging;


namespace LoveMeHandMake2.Controllers
{
    [Authorize]
    public class MemberController : Controller
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(MemberController));
        private LoveMeHandMakeContext db = new LoveMeHandMakeContext();

        // GET: Member
        public ActionResult Index()
        {
            List<Member> members = db.Members.Include(m => m.EnrollStore).Include(m => m.EnrollTeacher).Where(x => x.ValidFlag == true).ToList();
            return View(members.ToList());
        }

        [HttpPost]
        public ActionResult Index(FormCollection formCollection)
        {
            string searchName = formCollection["searchName"];
            string searchPhone = formCollection["searchPhone"];
            string searchCardID = formCollection["searchCardID"];

            List<Member> members = db.Members.Include(m => m.EnrollStore).Include(m => m.EnrollTeacher)
                .Where(x => x.ValidFlag == true)
                .Where(x => (string.IsNullOrEmpty(searchName) ? true : x.Name.Contains(searchName)))
                .Where(x => (string.IsNullOrEmpty(searchPhone) ? true : x.Phone.Equals(searchPhone)))
                .Where(x => (string.IsNullOrEmpty(searchCardID) ? true : x.CardID.Equals(searchCardID)))
                .ToList();

            return View(members);
        }

        // GET: Member/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Where(x=> x.ID == id && x.ValidFlag == true).First();
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // GET: Member/Create
        public ActionResult Create()
        {
            //ViewBag.EnrollStoreID = new SelectList(db.Stores, "ID", "StoreCode");            
            //ViewBag.EnrollTeacherID = new SelectList(db.Teachers, "ID", "Name");
            ViewBag.StoreList = DropDownListHelper.GetStoreList(false);
            ViewBag.TeacherList = DropDownListHelper.GetTeacherList(false);
            return View();
        }

        // POST: Member/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Member member)
        {
            if (ModelState.IsValid == false)
            {
                //ViewBag.EnrollStoreID = new SelectList(db.Stores, "ID", "StoreCode");            
                //ViewBag.EnrollTeacherID = new SelectList(db.Teachers, "ID", "Name");
                ViewBag.StoreList = DropDownListHelper.GetStoreList(false);
                ViewBag.TeacherList = DropDownListHelper.GetTeacherList(false);
                return View(member);
            }

            try
            {
                List<string> errMsgs = new MemberService().Create(member);
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                log.Error(null, e);
                ViewBag.ErrMsg = e.Message;
                ViewBag.StoreList = DropDownListHelper.GetStoreList(false);
                ViewBag.TeacherList = DropDownListHelper.GetTeacherList(false);
                return View(member);
            }

        }

        public ActionResult TransferPoint(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Where(x => x.ID == id && x.ValidFlag == true).First();
            if (member == null)
            {
                return HttpNotFound();
            }
            TransferPointViewModel model = new TransferPointViewModel();
            model.setMember(member);
            ViewBag.StoreList = DropDownListHelper.GetStoreList(false);
            ViewBag.TeacherList = DropDownListHelper.GetTeacherList(false);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TransferPoint([Bind(Exclude = "Member")]TransferPointViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string storeCode = db.Stores.Where(x => x.ID == model.DepositStoreID).Select(x => x.StoreCode).FirstOrDefault();
                    model.DepostitDateTime = System.DateTime.Now;
                    model.OrderID = string.Format("{0}{1:yyMMddHHmmss}", storeCode, model.DepostitDateTime);
                    new DepositService(db).transferPoint(model);
                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    log.Warn(e.Message);
                    ViewBag.ErrMsg = e.Message;
                }
            }
            else
            {
                string messages = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                log.Warn(messages);
                ViewBag.ErrMsg = messages;
            }
            //ViewBag.EnrollStoreID = new SelectList(db.Stores, "ID", "StoreCode");            
            //ViewBag.EnrollTeacherID = new SelectList(db.Teachers, "ID", "Name");
            ViewBag.StoreList = DropDownListHelper.GetStoreList(false);
            ViewBag.TeacherList = DropDownListHelper.GetTeacherList(false);
            return View(model);
        }

        // GET: Member/Deposite
        public ActionResult Deposit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Where(x => x.ID == id && x.ValidFlag == true).First();
            if (member == null)
            {
                return HttpNotFound();
            }
            DepositHistory depositHistory = new DepositHistory();
            depositHistory.MemberID = member.ID;
            depositHistory.Member = member;
            //ViewBag.EnrollStoreID = new SelectList(db.Stores, "ID", "StoreCode");            
            //ViewBag.EnrollTeacherID = new SelectList(db.Teachers, "ID", "Name");
            ViewBag.StoreList = DropDownListHelper.GetStoreList(false);
            ViewBag.TeacherList = DropDownListHelper.GetTeacherList(false);
            return View(depositHistory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Deposit([Bind(Exclude="Member")]DepositHistory dh)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string storeCode = db.Stores.Where(x => x.ID == dh.DepositStoreID).Select(x => x.StoreCode).FirstOrDefault();
                    dh.DepostitDateTime = System.DateTime.Now;
                    dh.OrderID = string.Format("{0}{1:yyMMddHHmmss}", storeCode, dh.DepostitDateTime);
                    dh = new DepositService().Deposit(dh);

                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    log.Warn(e.Message);
                    ViewBag.ErrMsg = e.Message;
                }
            }
            else
            {
                string messages = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                log.Warn(messages);
                ViewBag.ErrMsg = messages;
            }
            //ViewBag.EnrollStoreID = new SelectList(db.Stores, "ID", "StoreCode");            
            //ViewBag.EnrollTeacherID = new SelectList(db.Teachers, "ID", "Name");
            ViewBag.StoreList = DropDownListHelper.GetStoreList(false);
            ViewBag.TeacherList = DropDownListHelper.GetTeacherList(false);
            dh.Member = db.Members
                .Where(x => (x.ID == dh.MemberID || x.MemberGuid == dh.MemberGuid)
                    && x.ValidFlag == true).FirstOrDefault();
            return View(dh);
        }

        public ActionResult TryCompute(DepositHistory depositHistory)
        {
            try
            {
                depositHistory = new DepositService().TryCompute(depositHistory);

                var result = new
                {
                    MemberID = depositHistory.MemberID,
                    TotalDepositMoney = depositHistory.TotalDepositMoney,
                    DepositPoint = depositHistory.DepositPoint,
                    DepositRewardPoint = depositHistory.DepositRewardPoint,
                    TotalPoint = depositHistory.TotalPoint,
                    AvgPointCost = depositHistory.AvgPointCost,
                };
                return Json(result);
            }
            catch (Exception e)
            {
                log.Warn(null, e);
                var result = new { errorMsg = e.Message };
                return Json(result);
            }
        }

        // GET: Member/DepositHistory/5
        public ActionResult DepositHistory(int id)
        {
            MemberDepositHistoryViewModel model = new MemberDepositHistoryViewModel();
            model.member = db.Members.Where(x => x.ID == id && x.ValidFlag == true).First();
            if (model.member == null)
            {
                return HttpNotFound();
            }
            model.DepositHistoryList = db.DepositHistory
                .Where(x => x.MemberID == model.member.ID && x.ValidFlag == true)
                .OrderByDescending(x => x.DepostitDateTime)
                .ToPagedList(model.PageNumber-1, model.PageSize);
            return View(model);           
        }

        [HttpPost]
        public ActionResult DepositHistory(int id, MemberDepositHistoryViewModel model)
        {
            model.member = db.Members.Where(x => x.ID == id && x.ValidFlag == true).First();
            if (model.member == null)
            {
                return HttpNotFound();
            }
            model.DepositHistoryList = db.DepositHistory
                .Where(x => x.MemberID == model.member.ID && x.ValidFlag == true)
                .Where(x => model.SearchDateStart == null ? true : model.SearchDateStart <= x.DepostitDateTime)
                .Where(x => model.SearchDateEnd == null ? true : x.DepostitDateTime <= model.SearchDateEnd)
                .OrderByDescending(x => x.DepostitDateTime)
                .ToPagedList(model.PageNumber - 1, model.PageSize);
            return View(model);
        }

        public ActionResult CancelDeposit(int id)
        {
            try
            {
                if (db.DepositHistory.Where(x => x.ID == id).Count() == 0)
                {
                    return HttpNotFound();
                }
                DepositHistory dh = db.DepositHistory.Where(x => x.ID == id && x.ValidFlag).FirstOrDefault();
                return View(dh);
            }
            catch (Exception e) {
                log.Warn(null, e);
                ViewBag.ErrorMessage = e.Message;
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CancelDepositConfirmed(int id)
        {
            try 
            {
                new DepositService(db).Cancel(id);
                int memberID = db.DepositHistory.Where(x => x.ID == id).Select(x => x.MemberID).FirstOrDefault();
                return RedirectToAction("DepositHistory", new { id = memberID });
            }
            catch (Exception e)
            {
                log.Error(null, e);
                ViewBag.ErrMsg = e.Message;
                return RedirectToAction("Index");
            }
        }

        // GET: Member/TradeHistory/5
        public ActionResult TradeHistory(int id)
        {
            MemberTradeHistoryViewModel model = new MemberTradeHistoryViewModel();
            model.member = db.Members.Where(x => x.ID == id && x.ValidFlag == true).First();
            if (model.member == null)
            {
                return HttpNotFound();
            }
            
            model.TradeOrderList = db.TradeOrder
                .Where(x => x.MemberID == model.member.ID && x.ValidFlag == true)
                .OrderByDescending(x => x.TradeDateTime)
                .ToPagedList(model.PageNumber - 1, model.PageSize);
            return View(model);
        }

        [HttpPost]
        public ActionResult TradeHistory(int id, MemberTradeHistoryViewModel model)
        {
            model.member = db.Members.Where(x => x.ID == id && x.ValidFlag == true).First();
            if (model.member == null)
            {
                return HttpNotFound();
            }

            model.TradeOrderList = db.TradeOrder
                .Where(x => x.ValidFlag == true)
                .Where(x => x.MemberID == model.member.ID)
                .Where(x => model.SearchDateStart == null ? true : model.SearchDateStart <= x.TradeDateTime)
                .Where(x => model.SearchDateEnd == null ? true : x.TradeDateTime <= model.SearchDateEnd)
                .OrderByDescending(x => x.TradeDateTime)
                .ToPagedList(model.PageNumber - 1, model.PageSize);
            return View(model);
        }

        // GET: Member/TradeDetail/5
        public ActionResult TradeDetail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TradeOrder order = db.TradeOrder.Where(x=>x.ID == id && x.ValidFlag == true).First();
            if (order == null)
            {
                return HttpNotFound();
            }           
            List<TradePurchaseProduct> details = db.TradePurchaseProduct.Where(x => x.OrderID == order.ID && x.ValidFlag == true).ToList();
            MemberTradeDetailViewModel model = new MemberTradeDetailViewModel { Order = order, TradePurchaseProducts = details  };
            return View(model);
        }

        // GET: Member/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Where(x => x.ID == id && x.ValidFlag == true).First();
            if (member == null)
            {
                return HttpNotFound();
            }
            //ViewBag.EnrollStoreID = new SelectList(db.Stores, "ID", "StoreCode");            
            //ViewBag.EnrollTeacherID = new SelectList(db.Teachers, "ID", "Name");
            ViewBag.StoreList = DropDownListHelper.GetStoreList(false);
            ViewBag.TeacherList = DropDownListHelper.GetTeacherList(false);
            return View(member);
        }

        // POST: Member/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Member member)
        {
            if (ModelState.IsValid == false)
            {
                //ViewBag.EnrollStoreID = new SelectList(db.Stores, "ID", "StoreCode");            
                //ViewBag.EnrollTeacherID = new SelectList(db.Teachers, "ID", "Name");
                ViewBag.StoreList = DropDownListHelper.GetStoreList(false);
                ViewBag.TeacherList = DropDownListHelper.GetTeacherList(false);
                return View(member);
            }
            try
            {
                if (new MemberService().IsCardIDExistExceptCurrent(member.ID, member.CardID))
                {
                    ViewBag.StoreList = DropDownListHelper.GetStoreList(false);
                    ViewBag.TeacherList = DropDownListHelper.GetTeacherList(false);
                    ViewBag.ErrMsg = "卡号已存在!";
                    return View(member);
                }
                member.Update();
                db.Entry(member).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                log.Warn(null, e);
                ViewBag.ErrMsg = e.Message;
                return View(member);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
