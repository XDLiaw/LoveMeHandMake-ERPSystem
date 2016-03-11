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
    public class DepositeRewardRuleController : Controller
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(StoreController)); 
        private LoveMeHandMakeContext db = new LoveMeHandMakeContext();

        // GET: DepositeRewardRule
        public ActionResult Index()
        {
            return View(db.DepositeRewardRule.ToList());
        }

        // GET: DepositeRewardRule/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DepositeRewardRule/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DepositeRewardRule depositeRewardRule)
        {
            if (ModelState.IsValid)
            {
                depositeRewardRule.Create();
                db.DepositeRewardRule.Add(depositeRewardRule);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(depositeRewardRule);
        }

        // GET: DepositeRewardRule/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DepositeRewardRule depositeRewardRule = db.DepositeRewardRule.Find(id);
            if (depositeRewardRule == null)
            {
                return HttpNotFound();
            }
            return View(depositeRewardRule);
        }

        // POST: DepositeRewardRule/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DepositeRewardRule depositeRewardRule)
        {
            if (ModelState.IsValid)
            {
                depositeRewardRule.Update();
                db.Entry(depositeRewardRule).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(depositeRewardRule);
        }

        // GET: DepositeRewardRule/Delete/5
        public ActionResult Delete(int? id)
        {
            log.Warn("Delete(" + id + ") method is called!");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DepositeRewardRule depositeRewardRule = db.DepositeRewardRule.Find(id);
            if (depositeRewardRule == null)
            {
                return HttpNotFound();
            }
            return View(depositeRewardRule);
        }

        // POST: DepositeRewardRule/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DepositeRewardRule depositeRewardRule = db.DepositeRewardRule.Find(id);
            db.DepositeRewardRule.Remove(depositeRewardRule);
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
