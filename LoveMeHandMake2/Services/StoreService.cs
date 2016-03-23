using LoveMeHandMake2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Services
{
    public class StoreService
    {
        private LoveMeHandMakeContext db = new LoveMeHandMakeContext();

        public bool IsStoreExist(int storeID)
        {
            return db.Stores.Where(x => x.ID == storeID && x.ValidFlag == true).Count() > 0;
        }

        public bool IsStoreExist(Store store)
        {
            return IsStoreExist(store.ID);
        }
    }
}