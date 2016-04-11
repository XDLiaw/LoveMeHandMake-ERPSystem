using LoveMeHandMake2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Services
{
    public class StoreService : BaseService
    {
        public StoreService() : base() { }

        public StoreService(LoveMeHandMakeContext db) : base(db) { }

        public bool IsStoreExist(int storeID)
        {
            return db.Stores.Where(x => x.ID == storeID && x.ValidFlag == true).Count() > 0;
        }

        public bool IsStoreExist(Store store)
        {
            return IsStoreExist(store.ID);
        }

        public bool IsStoreCodeExist(string storeCode)
        {
            return db.Stores.Where(x => x.StoreCode == storeCode && x.ValidFlag == true).Count() > 0;
        }
    }
}