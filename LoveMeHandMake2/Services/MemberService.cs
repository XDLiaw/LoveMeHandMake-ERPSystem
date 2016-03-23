using LoveMeHandMake2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Services
{
    public class MemberService
    {
        private LoveMeHandMakeContext db = new LoveMeHandMakeContext();

        public bool IsCardIDExist(string id)
        {
            return db.Members.Where(x => x.CardID == id).Count() > 0;
        }

        public bool IsCardIDExistExceptCurrent(int currentMemberID, string cardId)
        {
            return db.Members.Where(x => x.ID != currentMemberID && x.CardID == cardId).Count() > 0;
        }

        public bool IsCardIDExistExceptCurrent(Guid currentMemberGuid, string cardId)
        {
            return db.Members.Where(x => x.MemberGuid != currentMemberGuid && x.CardID == cardId).Count() > 0;
        }

        public bool IsGuidExist(Guid guid)
        {
            return db.Members.Where(x => x.MemberGuid == guid).Count() > 0;
        }

    }
}