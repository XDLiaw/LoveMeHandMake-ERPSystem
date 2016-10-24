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
using System.Web.Configuration;
using LoveMeHandMake2.Models.ViewModels;
using MvcPaging;
using LoveMeHandMake2.Helper;
using LoveMeHandMake2.Services;

namespace LoveMeHandMake2.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private LoveMeHandMakeContext db = new LoveMeHandMakeContext();
        private static readonly ILog log = LogManager.GetLogger(typeof(StoreController));


        // GET: Product
        public ActionResult Index()
        {
            ProductViewModel model = new ProductViewModel();
            return Index(model);
        }

        [HttpPost]
        public ActionResult Index(ProductViewModel model)
        {
            IQueryable<Product> query = db.Products.Where(x => model.productCategoryID == null ? true : x.ProductCategoryID == model.productCategoryID);
            if (String.IsNullOrWhiteSpace(model.searchName) == false) {
                query = query.Where(x => x.Name.Contains(model.searchName));
            }
            if (model.searchPrice != null)
            {
                int price = model.searchPrice.GetValueOrDefault();
                query = query.Where(x => x.Price == price);
            }            
            query = query.Where(x => x.ValidFlag == true);              
                
            model.ProductList = query.OrderBy(x => x.ProductCategoryID).ThenBy(x => x.Name).ToPagedList(model.PageNumber - 1, model.PageSize);

            ViewBag.ProductCategoryList = DropDownListHelper.GetProductCategoryListWithEmpty();
            return View(model);
        }


        // GET: Product/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Where(x => x.ID == id && x.ValidFlag == true).First();
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Product/Create
        public ActionResult Create()
        {
            //ViewBag.ProductCategoryID = new SelectList(db.ProductCategory, "ID", "Name");
            ViewBag.ProductCategoryList = DropDownListHelper.GetProductCategoryList();
            return View();
        }

        // POST: Product/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                product.ProductCategory = db.ProductCategory.Where(x => x.ID == product.ProductCategoryID && x.ValidFlag == true).First();
                product.Create();
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //ViewBag.ProductCategoryID = new SelectList(db.ProductCategory, "ID", "Name", product.ProductCategoryID);
            ViewBag.ProductCategoryList = DropDownListHelper.GetProductCategoryList();
            return View(product);
        }

        // GET: Product/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Include(x => x.ProductCategory).FirstOrDefault(r => r.ID == id && r.ValidFlag == true);
            if (product == null)
            {
                return HttpNotFound();
            }
            //ViewBag.ProductCategoryID = new SelectList(db.ProductCategory, "ID", "Name", product.ProductCategoryID);
            ViewBag.ProductCategoryList = DropDownListHelper.GetProductCategoryList();
            return View(product);
        }

        // POST: Product/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                product.ProductCategory = db.ProductCategory.Where(x => x.ID == product.ProductCategoryID && x.ValidFlag == true).First();
                product.Update();
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.ProductCategoryID = new SelectList(db.ProductCategory, "ID", "Name", product.ProductCategoryID);
            ViewBag.ProductCategoryList = DropDownListHelper.GetProductCategoryList();
            return View(product);
        }

        // GET: Product/Delete/5
        public ActionResult Delete(int? id)
        {
            log.Warn("Delete(" + id + ") method is called!");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Where(x => x.ID == id && x.ValidFlag == true).First();
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Where(x => x.ID == id && x.ValidFlag == true).First();
            product.Delete();
            db.Entry(product).State = EntityState.Modified;
            db.SaveChanges();
            ProductImageHelper.DeleteImage(product.ImageName);
            return RedirectToAction("Index");
        }

        public ActionResult BatchImport()
        {
            return View(new ProductBatchImportViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BatchImport(ProductBatchImportViewModel model)
        {
            model.resultMessage = ProductImportService.ImportExcel(model.UploadFile.InputStream);
            return View(model);
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
