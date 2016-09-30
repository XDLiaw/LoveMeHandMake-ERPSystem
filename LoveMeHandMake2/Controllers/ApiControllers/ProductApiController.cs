using LoveMeHandMake2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MvcPaging;

namespace LoveMeHandMake2.Controllers.ApiControllers
{
    public class ProductApiController : ApiController
    {
        private LoveMeHandMakeContext db = new LoveMeHandMakeContext();

        [HttpGet]
        public Object Synchronize(DateTime lastSynchronizeTime)
        {
            DateTime receiveRequestTime = DateTime.Now;
            List<Product> newProducts = db.Products
                .Where(x => x.CreateTime > lastSynchronizeTime
                    && x.ValidFlag == true).ToList();
            List<Product> changedProducts = db.Products
                .Where(x => x.CreateTime <= lastSynchronizeTime
                    && x.UpdateTime > lastSynchronizeTime
                    && x.ValidFlag == true).ToList();
            List<Product> removedProducts = db.Products
                .Where(x => x.CreateTime <= lastSynchronizeTime
                    && x.UpdateTime > lastSynchronizeTime
                    && x.ValidFlag == false).ToList();

            foreach (Product p in newProducts)
            {
                p.ImageByteArray = p.GetImageIfExist();
            }
            foreach (Product p in changedProducts)
            {
                p.ImageByteArray = p.GetImageIfExist();
            }

            var res = new
            {
                receiveRequestTime = receiveRequestTime,
                NewProducts = newProducts,
                ChangedProducts = changedProducts,
                RemovedProducts = removedProducts
            };

            return res;
        }

        [HttpGet]
        public Object SynchronizeAll(int pageNumber, int pageSize)
        {
            DateTime receiveRequestTime = DateTime.Now;
            IPagedList<Product> newProducts = db.Products
                .Where(x => x.ValidFlag == true)
                .OrderBy(x => x.ID)
                .ToPagedList(pageNumber - 1, pageSize);

            foreach (Product p in newProducts)
            {
                p.ImageByteArray = p.GetImageIfExist();
            }

            var res = new
            {
                receiveRequestTime = receiveRequestTime,
                NewProducts = newProducts,
                PageCount = newProducts.PageCount,
                TotalItemCount = newProducts.TotalItemCount
            };

            return res;
        }
    }
}
