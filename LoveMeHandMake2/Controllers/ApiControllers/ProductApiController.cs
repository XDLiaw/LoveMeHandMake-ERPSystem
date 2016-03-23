using LoveMeHandMake2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

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
                p.ImageByteArray = p.GetImage();
            }
            foreach (Product p in changedProducts)
            {
                p.ImageByteArray = p.GetImage();
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
    }
}
