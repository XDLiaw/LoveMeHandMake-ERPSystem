﻿using log4net;
using LoveMeHandMake2.Models;
using LoveMeHandMake2.Models.ApiModels;
using LoveMeHandMake2.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LoveMeHandMake2.Controllers.ApiControllers
{
    public class MemberApiController : ApiController
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(MemberApiController));
        private LoveMeHandMakeContext db = new LoveMeHandMakeContext();

        [HttpGet]
        public Object Synchronize(DateTime lastSynchronizeTime)
        {           
            DateTime receiveRequestTime = DateTime.Now;
            List<Member> newMembers = db.Members
                .Where(x => x.CreateTime > lastSynchronizeTime
                    && x.ValidFlag == true).ToList();
            List<Member> changedMembers = db.Members
                .Where(x => x.CreateTime <= lastSynchronizeTime
                    && x.UpdateTime > lastSynchronizeTime
                    && x.ValidFlag == true).ToList();
            List<Member> removedMembers = db.Members
                .Where(x => x.CreateTime <= lastSynchronizeTime
                    && x.UpdateTime > lastSynchronizeTime
                    && x.ValidFlag == false).ToList();

            var res = new
            {
                ReceiveRequestTime = receiveRequestTime,
                NewMembers = newMembers,
                ChangedMembers = changedMembers,
                RemovedMembers = removedMembers
            };

            return res;
        }

        [HttpGet]
        public MemberPointResultApiModel GetPoint(Guid memberGuid)
        {
            log.Info(JsonConvert.SerializeObject(memberGuid));
            MemberPointResultApiModel res = new MemberPointResultApiModel();
            res.ReceiveRequestTime = DateTime.Now;
            Member m = db.Members.Where(x => x.MemberGuid == memberGuid && x.ValidFlag == true).FirstOrDefault();
            if (m == null)
            {
                log.Warn("MemberGuid: [" + memberGuid + "] doesn't exist!");
                res.ErrMsgs.Add("MemberGuid: [" + memberGuid + "] doesn't exist!");
                res.IsRequestSuccess = false;
            }
            else
            {
                res.Point = m.Point;
                res.IsRequestSuccess = true;
            }
            return res;
        }

        [HttpPost]
        public MemberResultApiModel Create(MemberRequestApiModel arg)
        {
            log.Info(JsonConvert.SerializeObject(arg));
            MemberResultApiModel res = new MemberResultApiModel();
            res.ReceiveRequestTime = DateTime.Now;
            res.IsRequestSuccess = false;
            try
            {
                if (arg.IsValid() == false)
                {
                    log.Error(arg.GetInvalidReasons());
                    res.ErrMsgs.AddRange(arg.GetInvalidReasons());
                    res.IsRequestSuccess = false;
                    return res;
                }
                if (new MemberService(this.db).IsGuidExist(arg.member.MemberGuid))
                {
                    string errMsg = "Guid: '" + arg.member.MemberGuid + "' already exist!";
                    log.Error(errMsg);
                    res.ErrMsgs.Add(errMsg);
                    res.IsRequestSuccess = false;
                    return res;
                }
                if (string.IsNullOrEmpty(arg.oldMemberDepositOrderID) == false && arg.oldMemberPoint != null && arg.oldMemberPointUnitValue != null)
                {// 舊會員資料登錄
                    List<string> errMsgs = new MemberService(this.db).Create(arg.member);
                    res.ErrMsgs.AddRange(errMsgs);
                    try
                    {//補上舊會員既有點數
                        DepositHistory dh = new DepositHistory();
                        dh.Create();
                        dh.OrderID = arg.oldMemberDepositOrderID;
                        dh.MemberID = arg.member.ID;
                        dh.MemberGuid = arg.member.MemberGuid;
                        dh.DepositStoreID = arg.member.EnrollStoreID;
                        dh.DepositTeacherID = arg.member.EnrollTeacherID;
                        dh.TotalPoint = arg.oldMemberPoint.GetValueOrDefault();
                        dh.AvgPointCost = arg.oldMemberPointUnitValue.GetValueOrDefault();
                        dh.DepostitDateTime = arg.member.EnrollDate;
                        new DepositService(this.db).Deposit(dh, false);
                    }
                    catch (Exception e)
                    {
                        log.Error(null, e);
                        res.ErrMsgs.Add(e.Message);
                    }
                }
                else
                {
                    List<string> errMsgs = new MemberService(this.db).Create(arg.member);
                    res.ErrMsgs.AddRange(errMsgs);
                }
                res.IsRequestSuccess = true;
                return res;
            }
            catch (Exception e)
            {
                log.Error(null, e);
                res.ErrMsgs.Add(e.Message);
                res.IsRequestSuccess = false;
                return res;
            }
        }

        public MemberResultApiModel Update(MemberRequestApiModel arg)
        {
            log.Info(JsonConvert.SerializeObject(arg));
            MemberResultApiModel res = new MemberResultApiModel();
            res.ReceiveRequestTime = DateTime.Now;
            res.IsRequestSuccess = false;
            try
            {
                if (arg.IsValid() == false)
                {
                    log.Error(arg.GetInvalidReasons());
                    res.ErrMsgs.AddRange(arg.GetInvalidReasons());
                    return res;
                }
                if (new MemberService().IsGuidExist(arg.member.MemberGuid) == false)
                {
                    string errMsg = "Guid: '" + arg.member.MemberGuid + "' doesn't exist!";
                    log.Error(errMsg);
                    res.ErrMsgs.Add(errMsg);
                    return res;
                }
                if (new StoreService().IsStoreExist(arg.member.EnrollStoreID) == false)
                {
                    string errMsg = "EnrollStoreID: " + arg.member.EnrollStoreID + " doesn't exist!";
                    log.Warn(errMsg);
                    res.ErrMsgs.Add(errMsg);
                }
                if (new TeacherService().IsTeacherExist(arg.member.EnrollTeacherID) == false)
                {
                    string errMsg = "EnrollTeacherID: " + arg.member.EnrollTeacherID + " doesn't exist!";
                    log.Warn(errMsg);
                    res.ErrMsgs.Add(errMsg);
                }
                if (new MemberService().IsCardIDExistExceptCurrent(arg.member.MemberGuid, arg.member.CardID))
                {
                    string errMsg = "CardID: " + arg.member.CardID + " already exist!";
                    log.Warn(errMsg);
                    res.ErrMsgs.Add(errMsg);
                }
                Member newMember = db.Members
                    .Where(x => x.MemberGuid == arg.member.MemberGuid 
                        && x.ValidFlag == true).FirstOrDefault();
                newMember.Update();
                newMember.UpdateBy(arg.member);
                db.Entry(newMember).State = EntityState.Modified;
                db.SaveChanges();
                res.IsRequestSuccess = true;
                return res;
            }
            catch (Exception e)
            {
                log.Error(null, e);
                res.ErrMsgs.Add(e.Message);
                return res;
            }
        }
    }
}
