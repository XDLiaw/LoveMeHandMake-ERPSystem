using log4net;
using LoveMeHandMake2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Services
{
    public class MemberService : BaseService
    {
        public MemberService() : base() { }

        public MemberService(LoveMeHandMakeContext db) : base(db) { }

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

        public List<string> Create(Member m)
        {
            List<string> errMsgs = new List<string>();
            if (new MemberService().IsCardIDExist(m.CardID))
            {
                string errMsg = "CardID: " + m.CardID + " already exist!";
                log.Error(errMsg);
                throw new ArgumentException(errMsg);
            }
            if (new StoreService().IsStoreExist(m.EnrollStoreID) == false)
            {
                string errMsg = "EnrollStoreID: " + m.EnrollStoreID + " doesn't exist!";
                errMsgs.Add(errMsg);
                log.Warn(errMsg);
            }
            if (new TeacherService().IsTeacherExist(m.EnrollTeacherID) == false)
            {
                string errMsg = "EnrollTeacherID: " + m.EnrollTeacherID + " doesn't exist!";
                errMsgs.Add(errMsg);
                log.Warn(errMsg);
            }

            //這裡new 一個新Member是為了下面還要做公關儲點
            Member newMember = new Member();
            newMember.CreateBy(m);
            db.Members.Add(newMember);
            db.SaveChanges();

            // if it is PR card then deposit point for this new member
            if (m.IsPRCard && m.Point > 0)
            {
                Store EnrollStore = db.Stores.Where(x => x.ID == m.EnrollStoreID && x.ValidFlag == true).FirstOrDefault();  
                DepositHistory depositHistory = new DepositHistory()
                {
                    OrderID = string.Format("{0}{1:yyMMddHHmmss}", EnrollStore.StoreCode, newMember.EnrollDate),
                    DepositStoreID = newMember.EnrollStoreID,
                    DepositTeacherID = newMember.EnrollTeacherID,
                    MemberGuid = newMember.MemberGuid,
                    RewardPoint = (int)m.Point,
                    DepostitDateTime = newMember.EnrollDate
                };
                new DepositService(db).Deposit(depositHistory);
            }

            return errMsgs;
        }
    }
}