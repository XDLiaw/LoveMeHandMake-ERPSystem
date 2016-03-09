using System;
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
    public class StoreController : Controller
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(StoreController));
        private LoveMeHandMakeContext db = new LoveMeHandMakeContext();

        //
        // GET: /Store/

        public ActionResult Index()
        {
            List<Store> stores = db.Stores
                .Where(x => x.ValidFlag == true)
                .Include(x => x.StoreCanSellCategories)
                .ToList();
            return View(stores);
        }

        //
        // GET: /Store/Details/5

        //public ActionResult Details(int id = 0)
        //{
        //    Store store = db.Stores.Include(x => x.StoreCanSellCategories).FirstOrDefault(r => r.ID == id);
        //    if (store == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(store);
        //}

        //
        // GET: /Store/Create

        public ActionResult Create()
        {
            ViewBag.categories = db.ProductCategory.ToList();
            return View();
        }

        //
        // POST: /Store/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Store store, FormCollection formCollection)
        {
            if (ModelState.IsValid)
            {
                store.Create();
                db.Stores.Add(store);
                
                List<StoreCanSellCategory> categories = new List<StoreCanSellCategory>();
                foreach (string key in formCollection.AllKeys) {                    
                    if (key.StartsWith("category_"))
                    {
                        int categoryID = Convert.ToInt32(formCollection[key]);
                        StoreCanSellCategory category = new StoreCanSellCategory { StoreID = store.ID, ProductCategoryID = categoryID };
                        category.Create();
                        categories.Add(category);
                    }
                }
                db.StoreCanSellCategory.AddRange(categories);
                
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(store);
        }

        //
        // GET: /Store/Edit/5

        public ActionResult Edit(int id = 0)
        {
            ViewBag.categories = db.ProductCategory.ToList();
            Store store = db.Stores.Include(x => x.StoreCanSellCategories).FirstOrDefault(r => r.ID == id);
            if (store == null)
            {
                return HttpNotFound();
            }
            return View(store);
        }

        //
        // POST: /Store/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Store store, FormCollection formCollection)
        {
            if (ModelState.IsValid)
            {          
                store.Update();
                db.Entry(store).State = EntityState.Modified;

                db.StoreCanSellCategory.RemoveRange(db.StoreCanSellCategory.Where(x => x.StoreID == store.ID));

                List<StoreCanSellCategory> categories = new List<StoreCanSellCategory>();
                foreach (string key in formCollection.AllKeys)
                {
                    if (key.StartsWith("category_"))
                    {
                        int categoryID = Convert.ToInt32(formCollection[key]);
                        StoreCanSellCategory category = new StoreCanSellCategory { StoreID = store.ID, ProductCategoryID = categoryID };
                        category.Create();
                        categories.Add(category);
                    }
                }
                db.StoreCanSellCategory.AddRange(categories);

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(store);
        }

        //
        // GET: /Store/Delete/5

        public ActionResult Delete(int id = 0)
        {
            ViewBag.categories = db.ProductCategory.ToList();
            //Store store = db.Stores.Find(id);
            Store store = db.Stores.Include(x => x.StoreCanSellCategories).FirstOrDefault(r => r.ID == id);
            if (store == null)
            {
                return HttpNotFound();
            }
            return View(store);
        }

        //
        // POST: /Store/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Store store = db.Stores.Find(id);
            db.Stores.Remove(store);
            db.StoreCanSellCategory.RemoveRange(db.StoreCanSellCategory.Where(x => x.StoreID == store.ID));
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}