using LoveMeHandMake2.Models;
using LoveMeHandMake2.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Services.report
{
    public class ProductSaleReportService : BaseService
    {
        public ProductSaleReportService() : base() { }

        public ProductSaleReportService(LoveMeHandMakeContext db) : base(db) { }

        public ProductSaleReportViewModel GetModelData(int? SearchStoreID, int? SearchProductCategoryID, DateTime? SearchDateStart, DateTime? SearchDateEnd)
        {
            ProductSaleReportViewModel model = new ProductSaleReportViewModel();
            model.SearchStoreID = SearchStoreID;
            model.SearchProductCategoryID = SearchProductCategoryID;
            model.SearchDateStart = SearchDateStart;
            model.SearchDateEnd = SearchDateEnd;
            DateTime SearchDateEnd_nextDay = SearchDateEnd.GetValueOrDefault().AddDays(1);
            try
            {
                if (SearchStoreID != null)
                {
                    model.StoreName = db.Stores.Where(x => x.ID == SearchStoreID).Select(x => x.Name).FirstOrDefault();
                }

                model.saleList =
                (
                    from tpp in db.TradePurchaseProduct
                    join o in db.TradeOrder on tpp.OrderID equals o.ID
                    join p in db.Products on tpp.ProductID equals p.ID
                    where (SearchStoreID == null ? true : o.StoreID == SearchStoreID)
                        && (SearchProductCategoryID == null ? true : p.ProductCategoryID == SearchProductCategoryID)
                        && (SearchDateStart == null ? true : SearchDateStart <= o.TradeDateTime)
                        && (SearchDateEnd == null ? true : o.TradeDateTime < SearchDateEnd_nextDay)
                        && (o.ValidFlag == true)
                        && (tpp.ValidFlag == true)
                    orderby o.TradeDateTime
                    select new ProductSaleRecord
                    {
                        TradeDateTime = o.TradeDateTime,
                        ProductName = p.Name,
                        Amount = tpp.Amount,
                        UnitPoint = tpp.UnitPoint,
                        UnitBean = tpp.UnitBean,
                        MemberCardID = (o.Member == null ? null : o.Member.CardID),
                        Sum = tpp.Sum,
                        Gender = (o.Member == null ? null : (bool?)o.Member.Gender),
                        TeacherName = (o.Teacher == null ? null : o.Teacher.Name),
                        ImageName = p.ImageName
                    }
                ).ToList();

                model.ComputeAll();
            }
            catch (Exception e)
            {
                log.Error(null, e);

            }

            return model;
        }

    }
}