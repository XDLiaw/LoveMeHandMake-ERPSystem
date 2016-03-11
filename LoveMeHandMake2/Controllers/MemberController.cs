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

namespace LoveMeHandMake2.Controllers
{
    public class MemberController : Controller
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(StoreController));
        private LoveMeHandMakeContext db = new LoveMeHandMakeContext();

        // GET: Member
        public ActionResult Index()
        {
            var members = db.Members.Include(m => m.EnrollStore).Include(m => m.EnrollTeacher);
            return View(members.ToList());
        }

        // GET: Member/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // GET: Member/Create
        public ActionResult Create()
        {
            ViewBag.StoreList = DropDownListHelper.GetStoreList();
            //ViewBag.EnrollStoreID = new SelectList(db.Stores, "ID", "StoreCode");
            ViewBag.EnrollTeacherID = new SelectList(db.Teachers, "ID", "Name");
            return View();
        }

        // POST: Member/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Member member)
        {
            if (ModelState.IsValid)
            {
                member.Create();
                db.Members.Add(member);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.StoreList = DropDownListHelper.GetStoreList();
            //ViewBag.EnrollStoreID = new SelectList(db.Stores, "ID", "StoreCode", member.EnrollStoreID);
            ViewBag.EnrollTeacherID = new SelectList(db.Teachers, "ID", "Name", member.EnrollTeacherID);
            return View(member);
        }

        // GET: Member/Deposite
        public ActionResult Deposit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            return View();
        }

        // GET: Member/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            ViewBag.StoreList = DropDownListHelper.GetStoreList();
            //ViewBag.EnrollStoreID = new SelectList(db.Stores, "ID", "StoreCode", member.EnrollStoreID);
            ViewBag.EnrollTeacherID = new SelectList(db.Teachers, "ID", "Name", member.EnrollTeacherID);
            return View(member);
        }

        // POST: Member/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Member member)
        {
            if (ModelState.IsValid)
            {
                member.Update();
                db.Entry(member).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.StoreList = DropDownListHelper.GetStoreList();
            //ViewBag.EnrollStoreID = new SelectList(db.Stores, "ID", "StoreCode", member.EnrollStoreID);
            ViewBag.EnrollTeacherID = new SelectList(db.Teachers, "ID", "Name", member.EnrollTeacherID);
            return View(member);
        }

        // GET: Member/Delete/5
        public ActionResult Delete(int? id)
        {
            log.Warn("Delete("+id+") method is called!");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
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
            Member member = db.Members.Find(id);
            db.Members.Remove(member);
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
