using log4net;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Helper.ExcelReport
{
    public class BaseExcelReport
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(ProductSaleExcelReport));

        protected IWorkbook workbook;
        protected ICellStyle defaultCellStyle_Center;
        protected ICellStyle defaultCellStyle_Date;

        protected void Init()
        {
            this.workbook = new XSSFWorkbook();

            this.defaultCellStyle_Center = this.workbook.CreateCellStyle();
            this.defaultCellStyle_Center.Alignment = HorizontalAlignment.Center;
            this.defaultCellStyle_Center.VerticalAlignment = VerticalAlignment.Center;

            this.defaultCellStyle_Date = this.workbook.CreateCellStyle();
            IDataFormat format = this.workbook.CreateDataFormat();
            this.defaultCellStyle_Date.DataFormat = format.GetFormat("yyyy/MM/dd (dddd)");
            this.defaultCellStyle_Date.Alignment = HorizontalAlignment.Center;
        }
    }
}