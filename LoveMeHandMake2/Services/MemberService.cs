using LoveMeHandMake2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Services
{
    public class MemberService
    {
        public bool IsCardIDExist(string id)
        {
            LoveMeHandMakeContext db = new LoveMeHandMakeContext();
            return db.Members.Where(x => x.CardID == id).Count() > 0;
        }

        public bool IsCardIDExistExceptCurrent(int currentMemberID, string cardId)
        {
            LoveMeHandMakeContext db = new LoveMeHandMakeContext();
            return db.Members.Where(x => x.ID != currentMemberID && x.CardID == cardId).Count() > 0;
        }

        public bool IsCardIDExistExceptCurrent(Guid currentMemberGuid, string cardId)
        {
            LoveMeHandMakeContext db = new LoveMeHandMakeContext();
            return db.Members.Where(x => x.MemberGuid != currentMemberGuid && x.CardID == cardId).Count() > 0;
        }

        public bool IsGuidExist(Guid guid)
        {
            LoveMeHandMakeContext db = new LoveMeHandMakeContext();
            return db.Members.Where(x => x.MemberGuid == guid).Count() > 0;
        }

    }
}