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
    [Authorize]
    public class TeacherController : Controller
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(StoreController));
        private LoveMeHandMakeContext db = new LoveMeHandMakeContext();

        // GET: Teacher
        public ActionResult Index()
        {
            var teachers = db.Teachers.Where(x => x.ValidFlag == true).Include(t => t.BelongStore).OrderBy(x => x.BelongStoreID);
            return View(teachers.ToList());
        }

        // GET: Teacher/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teacher teacher = db.Teachers.Where(x => x.ID == id && x.ValidFlag == true).First();
            if (teacher == null)
            {
                return HttpNotFound();
            }
            return View(teacher);
        }

        // GET: Teacher/Create
        public ActionResult Create()
        {
            //ViewBag.BelongStoreID = new SelectList(db.Stores, "ID", "Name");
            ViewBag.StoreList = DropDownListHelper.GetStoreList(false);
            return View();
        }

        // POST: Teacher/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Teacher teacher)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    teacher.Create();
                    List<int> accountIdList = new List<int>();
                    accountIdList.Add(0);
                    accountIdList.AddRange(db.Teachers.Where(x => x.ValidFlag).Select(x => x.AccountID));
                    teacher.AccountID = accountIdList.Max() + 1;
                    db.Teachers.Add(teacher);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                log.Error(null, e);
                ViewBag.ErrMsg = e.Message;
            }

            ViewBag.BelongStoreID = new SelectList(db.Stores, "ID", "Name", teacher.BelongStoreID);
            return View(teacher);
        }

        // GET: Teacher/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teacher teacher = db.Teachers.Where(x => x.ID == id && x.ValidFlag == true).First();
            if (teacher == null)
            {
                return HttpNotFound();
            }
            //ViewBag.BelongStoreID = new SelectList(db.Stores, "ID", "Name", teacher.BelongStoreID);
            ViewBag.StoreList = DropDownListHelper.GetStoreList(false);
            return View(teacher);
        }

        // POST: Teacher/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                teacher.Update();
                db.Entry(teacher).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.StoreList = DropDownListHelper.GetStoreList(false);
            return View(teacher);
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
