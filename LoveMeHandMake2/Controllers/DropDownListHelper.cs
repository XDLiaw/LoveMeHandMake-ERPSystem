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

        public static List<SelectListItem> GetStoreList(bool includeStopBusiness)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            List<Store> storeList;
            if (includeStopBusiness)
            {
                storeList = db.Stores.Where(x => x.ValidFlag == true).ToList();
            }
            else
            {
                storeList = db.Stores.Where(x => 
                    (x.StopBusinessDate > DateTime.Now || x.StopBusinessDate == null) 
                    && x.ValidFlag == true)
                    .ToList();
            }
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

        public static List<SelectListItem> GetProductCategoryList()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            List<ProductCategory> categoryList = db.ProductCategory.Where(x => x.ValidFlag == true).ToList();
            foreach (ProductCategory pc in categoryList)
            {
                items.Add(new SelectListItem()
                {
                    Text = pc.Name,
                    Value = pc.ID.ToString()
                });
            }

            return items;
        }

        public static List<SelectListItem> GetTeacherList(bool includeResign)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            List<Teacher> teacherList;
            if (includeResign)
            {
                teacherList = db.Teachers.Where(x => x.ValidFlag).ToList();
            }
            else
            {
                teacherList = db.Teachers.Where(
                    x => (x.ResignDate > DateTime.Now || x.ResignDate == null)
                    && x.ValidFlag).ToList();
            }

            foreach (Teacher t in teacherList)
            {
                items.Add(new SelectListItem()
                {
                    Text = t.Name,
                    Value = t.ID.ToString()
                });
            }

            return items;
        }

    }
}