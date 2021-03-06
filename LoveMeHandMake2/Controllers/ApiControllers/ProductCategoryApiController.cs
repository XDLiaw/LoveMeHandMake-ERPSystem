﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using LoveMeHandMake2.Models;
using LoveMeHandMake2.Models.ApiModels;

namespace LoveMeHandMake2.Controllers.ApiControllers
{
    public class ProductCategoryApiController : ApiController
    {
        private LoveMeHandMakeContext db = new LoveMeHandMakeContext();

        /// <summary>
        /// ~/api/ProductCategoryApi/Synchronize?storeID=12
        /// </summary>
        /// <param name="storeID"></param>
        /// <returns>傳回所有現在可賣的商品分類</returns>
        [HttpGet]
        public Object Synchronize(int storeID)
        {
            DateTime receiveRequestTime = DateTime.Now;
            List<ProductCategory> canSellList =
                (from A in db.ProductCategory
                 join B in db.StoreCanSellCategory
                 on A.ID equals B.ProductCategoryID
                 where B.StoreID == storeID
                 && A.ValidFlag == true
                 && B.ValidFlag == true
                 select A
                ).ToList();

            var res = new { 
                ReceiveRequestTime = receiveRequestTime,
                CanSellCategories = canSellList
            };

            return res;
        }
    }
}