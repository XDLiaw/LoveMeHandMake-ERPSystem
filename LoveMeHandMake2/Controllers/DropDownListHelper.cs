using LoveMeHandMake2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LoveMeHandMake2.Controllers
{
    public class DropDownListHelper
    {
        private static LoveMeHandMakeContext db = new LoveMeHandMakeContext();

        public static List<SelectListItem> GetStoreList()
        {
            List<Store> storeList = db.Stores.ToList();
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (Store store in storeList)
            {
                items.Add(new SelectListItem()
                {
                    Text = store.StoreCode + " - " + store.Name,
                    Value = store.ID.ToString()
                });
            }

            return items;
        }

    }
}