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

namespace LoveMeHandMake2.Controllers
{
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
                ViewBag.ErrMsg = e.Message;
                ViewBag.StoreList = DropDownListHelper.GetStoreList(false);
                ViewBag.TeacherList = DropDownListHelper.GetTeacherList(false);
                return View(member);
            }

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
        public ActionResult DepositHistory(int? id)
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
            ViewBag.Member = member;
            List<DepositHistory> history = db.DepositHistory
                .Where(x => x.MemberID == member.ID && x.ValidFlag == true)
                .OrderByDescending(x => x.DepostitDateTime).ToList();
            return View(history);           
        }

        public ActionResult CancelDeposit(int id)
        {
            DepositService service = new DepositService(db);
            if (service.IsOrderIDExist(id) == false)
            {
                return HttpNotFound();
            }
            DepositHistory dh = db.DepositHistory.Find(id);
            return View(dh);
        }

        [HttpPost]
        public ActionResult CancelDepositConfirmed(int id)
        {
            try 
            {
                int memberID = db.DepositHistory.Where(x => x.ID == id).Select(x => x.MemberID).FirstOrDefault();
                new DepositService(db).Cancel(id);
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
        public ActionResult TradeHistory(int? id)
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
            ViewBag.Member = member;
            List<TradeOrder> history = db.TradeOrder
                .Where(x => x.MemberID == member.ID && x.ValidFlag == true)
                .OrderByDescending(x => x.TradeDateTime).ToList();
            return View(history);
        }

        [HttpPost]
        public ActionResult TradeHistory(int? id, FormCollection formCollection)
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
            ViewBag.Member = member;
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
 
            List<TradeOrder> history = db.TradeOrder
                .Where(x => x.ValidFlag == true)
                .Where(x => x.MemberID == member.ID)
                .Where(x => (DateTime.Compare(dateStart, x.TradeDateTime) <=0))
                .Where(x => (DateTime.Compare(x.TradeDateTime, dateEnd) <= 0))
                .OrderByDescending(x => x.TradeDateTime).ToList();
            return View(history);
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

        // GET: Member/Delete/5
        public ActionResult Delete(int? id)
        {
            log.Warn("Delete("+id+") method is called!");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Where(x => x.ID == id && x.ValidFlag == true).First();
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // POST: Member/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Member member = db.Members.Where(x => x.ID == id && x.ValidFlag == true).First();
            member.Delete();
            db.Entry(member).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
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
