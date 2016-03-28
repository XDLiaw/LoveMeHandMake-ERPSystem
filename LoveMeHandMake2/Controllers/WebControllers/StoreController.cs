using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LoveMeHandMake2.Models;
using log4net;
using LoveMeHandMake2.Services;

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
        // GET: /Store/Create

        public ActionResult Create()
        {
            ViewBag.categories = db.ProductCategory.Where(x=> x.ValidFlag == true).ToList();
            return View();
        }

        //
        // POST: /Store/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Store store, FormCollection formCollection)
        {
            try {
                if (ModelState.IsValid == false)
                {
                    throw new ArgumentException("ModelState is invalid!");
                }

                if (new StoreService().IsStoreCodeExist(store.StoreCode))
                {
                    throw new ArgumentException(String.Format("门市代码 [{0}] 已存在", store.StoreCode));
                }

                store.Create();
                db.Stores.Add(store);

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
            catch (Exception e)
            {
                ViewBag.categories = db.ProductCategory.Where(x => x.ValidFlag == true).ToList();
                ViewBag.ErrMsg = e.Message;
                return View(store);
            }
        }

        //
        // GET: /Store/Edit/5

        public ActionResult Edit(int id = 0)
        {
            ViewBag.categories = db.ProductCategory.Where(x => x.ValidFlag == true).ToList();
            Store store = db.Stores.Include(x => x.StoreCanSellCategories).FirstOrDefault(r => r.ID == id && r.ValidFlag == true);
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

                // 先刪除該分店所有可販賣分類
                db.StoreCanSellCategory.RemoveRange(db.StoreCanSellCategory.Where(x => x.StoreID == store.ID));

                // 新增該分點可販賣分類
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


        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}