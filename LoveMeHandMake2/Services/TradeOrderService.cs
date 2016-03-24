using LoveMeHandMake2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Services
{
    public class TradeOrderService
    {
        private LoveMeHandMakeContext db = new LoveMeHandMakeContext();

        public bool IsOrderIDExist(string orderID)
        {
            return db.TradeOrder.Where(x => x.OrderID == orderID && x.ValidFlag == true).Count() > 0;
        }

        

    }
}