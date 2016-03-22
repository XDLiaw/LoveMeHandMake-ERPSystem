using LoveMeHandMake2.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Models.ApiModels
{
    public class TeacherSyncApiModel
    {
        public DateTime ReceiveRequestTime { get; set; }

        public List<Teacher> NewList { get; set; }

        public List<Teacher> ChangedList { get; set; }

        public List<Teacher> RemovedList { get; set; }

        public void EncryptPassword()
        {
            foreach (Teacher t in NewList)
            {
                t.Password = AESEncrypter.Encrypt(t.Password);
                t.PreviousPassword = AESEncrypter.Encrypt(t.PreviousPassword);
            }
            foreach (Teacher t in ChangedList)
            {
                t.Password = AESEncrypter.Encrypt(t.Password);
                t.PreviousPassword = AESEncrypter.Encrypt(t.PreviousPassword);
            }
            foreach (Teacher t in RemovedList)
            {
                t.Password = AESEncrypter.Encrypt(t.Password);
                t.PreviousPassword = AESEncrypter.Encrypt(t.PreviousPassword);
            }
        }
    }
}