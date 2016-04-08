using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LoveMeHandMake2.Models;

namespace LoveMeHandMake2.Controllers
{
    [Authorize]
    public class SysParameterController : Controller
    {
        private LoveMeHandMakeContext db = new LoveMeHandMakeContext();

        // GET: SysParameter
        public ActionResult Index()
        {
            return View(db.SysParameter.ToList());
        }

        // GET: SysParameter/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SysParameter sysParameter = db.SysParameter.Where(x => x.ID == id && x.ValidFlag == true).First();
            if (sysParameter == null)
            {
                return HttpNotFound();
            }
            return View(sysParameter);
        }

        // POST: SysParameter/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SysParameter sysParameter)
        {
            if (ModelState.IsValid)
            {
                sysParameter.Update();
                db.Entry(sysParameter).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sysParameter);
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
