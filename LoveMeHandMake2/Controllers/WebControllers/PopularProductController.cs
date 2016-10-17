using log4net;
using LoveMeHandMake2.Models;
using LoveMeHandMake2.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcPaging;

namespace LoveMeHandMake2.Controllers.WebControllers
{
    [Authorize]
    public class PopularProductController : Controller
    {
        private LoveMeHandMakeContext db = new LoveMeHandMakeContext();
        private static readonly ILog log = LogManager.GetLogger(typeof(StoreController));

        // GET: PropularProduct
        public ActionResult Index()
        {
            return Index(new PopularProductViewModel());
        }

        [HttpPost]
        public ActionResult Index(PopularProductViewModel arg)
        {
            PopularProductViewModel model = new PopularProductViewModel();
            model.SearchProductCategoryID = arg.SearchProductCategoryID;
            model.SearchStoreID = arg.SearchStoreID;
            model.SearchDateStart = arg.SearchDateStart;
            model.SearchDateEnd = arg.SearchDateEnd;
            DateTime SearchDateEnd_nextDay = model.SearchDateEnd.GetValueOrDefault().AddDays(1);
            model.productPagedList =
            (
                from tpp in db.TradePurchaseProduct
                join o in db.TradeOrder on tpp.OrderID equals o.ID
                join p in db.Products on tpp.ProductID equals p.ID
                join pc in db.ProductCategory on p.ProductCategoryID equals pc.ID
                where (tpp.ValidFlag == true )
                   && (arg.SearchStoreID == null ? true : o.StoreID == arg.SearchStoreID)
                   && (arg.SearchProductCategoryID == null ? true : pc.ID == arg.SearchProductCategoryID)
                   && (arg.SearchDateStart == null ? true : arg.SearchDateStart <= o.TradeDateTime)
                   && (arg.SearchDateEnd == null ? true : o.TradeDateTime <= SearchDateEnd_nextDay)
                group new { tpp.Amount } by new { tpp.ProductID, Name = p.Name, p.Price, CategoryName = pc.Name, pc.Unit, p.ImageName } into g                
                select new PopularProduct 
                { 
                    CategoryName = g.Key.CategoryName,
                    Name = g.Key.Name,
                    Unit = g.Key.Unit,
                    Price = g.Key.Price,
                    Amount = g.Sum(x => x.Amount),
                    ImageName = g.Key.ImageName
                }
                
            ).OrderByDescending(x => x.Amount).ToPagedList(model.PageNumber - 1, model.PageSize);

            ViewBag.ProductCategoryList = DropDownListHelper.GetProductCategoryListWithEmpty();
            ViewBag.StoreList = DropDownListHelper.GetStoreListWithEmpty(true);
            return View(model);
        }
    }
}