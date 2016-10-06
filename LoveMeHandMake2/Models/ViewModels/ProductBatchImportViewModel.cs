using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Models.ViewModels
{
    public class ProductBatchImportViewModel
    {
        [Display(Name = "Excel檔")]
        public HttpPostedFileBase UploadFile { get; set; }

        [Display(Name = "匯入結果訊息")]
        public string resultMessage { get; set; }
    }
}