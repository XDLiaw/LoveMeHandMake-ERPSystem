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
    public class DepositRewardRuleController : Controller
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(StoreController)); 
        private LoveMeHandMakeContext db = new LoveMeHandMakeContext();

        // GET: DepositRewardRule
        public ActionResult Index()
        {
            return View(db.DepositRewardRule.ToList());
        }

        // GET: DepositRewardRule/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DepositRewardRule/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DepositRewardRule depositRewardRule)
        {
            if (ModelState.IsValid)
            {
                depositRewardRule.Create();
                db.DepositRewardRule.Add(depositRewardRule);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(depositRewardRule);
        }

        // GET: DepositRewardRule/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DepositRewardRule depositRewardRule = db.DepositRewardRule.Find(id);
            if (depositRewardRule == null)
            {
                return HttpNotFound();
            }
            return View(depositRewardRule);
        }

        // POST: DepositRewardRule/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DepositRewardRule depositRewardRule)
        {
            if (ModelState.IsValid)
            {
                depositRewardRule.Update();
                db.Entry(depositRewardRule).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(depositRewardRule);
        }

        // GET: DepositRewardRule/Delete/5
        public ActionResult Delete(int? id)
        {
            log.Warn("Delete(" + id + ") method is called!");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DepositRewardRule depositRewardRule = db.DepositRewardRule.Find(id);
            if (depositRewardRule == null)
            {
                return HttpNotFound();
            }
            return View(depositRewardRule);
        }

        // POST: DepositRewardRule/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DepositRewardRule depositRewardRule = db.DepositRewardRule.Find(id);
            db.DepositRewardRule.Remove(depositRewardRule);
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
