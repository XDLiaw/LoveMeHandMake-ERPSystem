﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LoveMeHandMake2.Models;
using log4net;

namespace LoveMeHandMake2.Controllers
{
    public class ProductCategoryController : Controller
    {
        private LoveMeHandMakeContext db = new LoveMeHandMakeContext();
        private static readonly ILog log = LogManager.GetLogger(typeof(StoreController));

        //
        // GET: /ProductCategory/

        public ActionResult Index()
        {
            return View(db.ProductCategory.ToList());
        }

        //
        // GET: /ProductCategory/Details/5

        public ActionResult Details(int id = 0)
        {
            ProductCategory productcategory = db.ProductCategory.Find(id);
            if (productcategory == null)
            {
                return HttpNotFound();
            }
            return View(productcategory);
        }

        //
        // GET: /ProductCategory/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /ProductCategory/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductCategory productcategory)
        {
            if (ModelState.IsValid)
            {
                productcategory.Create();
                db.ProductCategory.Add(productcategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(productcategory);
        }

        //
        // GET: /ProductCategory/Edit/5

        public ActionResult Edit(int id = 0)
        {
            ProductCategory productcategory = db.ProductCategory.Find(id);
            if (productcategory == null)
            {
                return HttpNotFound();
            }
            return View(productcategory);
        }

        //
        // POST: /ProductCategory/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductCategory productcategory)
        {
            if (ModelState.IsValid)
            {
                productcategory.Update();
                db.Entry(productcategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(productcategory);
        }

        //
        // GET: /ProductCategory/Delete/5

        public ActionResult Delete(int id = 0)
        {
            ProductCategory productcategory = db.ProductCategory.Find(id);
            if (productcategory == null)
            {
                return HttpNotFound();
            }

            if (HasProductUnderCategory(id))
            {
                return RedirectToAction("Index");
            }

            return View(productcategory);
        }

        //
        // POST: /ProductCategory/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductCategory productcategory = db.ProductCategory.Find(id);
            if (HasProductUnderCategory(id))
            {
                return RedirectToAction("Index");
            }

            db.ProductCategory.Remove(productcategory);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        private bool HasProductUnderCategory(int productCategoryID)
        {
            int productNum = db.Products.Where(x => x.ProductCategoryID == productCategoryID).Count();
            return productNum > 0 ? true : false;
        }

        public ActionResult HasProductUnderCategoryAjax(int productCategoryID)
        {
            log.Debug("HasProductUnderCategoryAjax("+productCategoryID+")");
            bool hasProduct = HasProductUnderCategory(productCategoryID);
            return Json(hasProduct);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}