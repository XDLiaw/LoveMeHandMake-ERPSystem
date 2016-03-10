﻿using System;
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
    public class TeacherController : Controller
    {
        private LoveMeHandMakeContext db = new LoveMeHandMakeContext();

        // GET: Teacher
        public ActionResult Index()
        {
            var teachers = db.Teachers.Include(t => t.BelongStore);
            return View(teachers.ToList());
        }

        // GET: Teacher/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teacher teacher = db.Teachers.Find(id);
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
            SetStoreDropDownList();
            return View();
        }

        // POST: Teacher/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                teacher.Create();
                db.Teachers.Add(teacher);
                db.SaveChanges();
                return RedirectToAction("Index");
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
            Teacher teacher = db.Teachers.Find(id);
            if (teacher == null)
            {
                return HttpNotFound();
            }
            //ViewBag.BelongStoreID = new SelectList(db.Stores, "ID", "Name", teacher.BelongStoreID);
            SetStoreDropDownList();
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
            ViewBag.BelongStoreID = new SelectList(db.Stores, "ID", "StoreCode", teacher.BelongStoreID);
            return View(teacher);
        }

        // GET: Teacher/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teacher teacher = db.Teachers.Find(id);
            if (teacher == null)
            {
                return HttpNotFound();
            }
            return View(teacher);
        }

        // POST: Teacher/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Teacher teacher = db.Teachers.Find(id);
            db.Teachers.Remove(teacher);
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

        private void SetStoreDropDownList()
        {
            List<Store> storeList = db.Stores.ToList();
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (Store store in storeList)
            {
                items.Add(new SelectListItem() { 
                    Text = store.StoreCode + " - " + store.Name,
                    Value = store.ID.ToString()
                });
            }

            ViewBag.StoreList = items;
        }
    }
}
