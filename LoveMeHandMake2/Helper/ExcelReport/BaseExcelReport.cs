using log4net;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Helper.ExcelReport
{
    public class BaseExcelReport
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(ProductSaleExcelReport));

        protected static CultureInfo cultureInfo = new System.Globalization.CultureInfo("zh-CN");

        protected IWorkbook workbook;
        protected CellStyleFactory cellStyleFactory;
        protected ICellStyle defaultCellStyle_Center;
        protected ICellStyle defaultCellStyle_Date;
        protected IFont defaultFont;
        protected string defaultFontName = "新細明體";
        protected short defaultFontHeightInPoints = 14;

        public BaseExcelReport()
        {
            Init();
        }

        private void Init()
        {
            this.workbook = new XSSFWorkbook();
            this.cellStyleFactory = new CellStyleFactory(this.workbook);

            this.defaultCellStyle_Center = this.workbook.CreateCellStyle();
            this.defaultCellStyle_Center.Alignment = HorizontalAlignment.Center;
            this.defaultCellStyle_Center.VerticalAlignment = VerticalAlignment.Center;

            this.defaultCellStyle_Date = this.workbook.CreateCellStyle();
            IDataFormat format = this.workbook.CreateDataFormat();
            this.defaultCellStyle_Date.DataFormat = format.GetFormat("yyyy/MM/dd (dddd)");
            this.defaultCellStyle_Date.Alignment = HorizontalAlignment.Center;

            this.defaultFont = this.workbook.CreateFont();
            this.defaultFont.FontHeightInPoints = this.defaultFontHeightInPoints;
            this.defaultFont.FontName = defaultFontName;
            this.defaultFont.Color = NPOI.HSSF.Util.HSSFColor.Black.Index;
        }

        protected ICell CreateCell(IRow row, int columnIndex, string cellValue)
        {
            ICell cell = row.CreateCell(columnIndex);
            cell.SetCellValue(cellValue);
            cell.CellStyle = this.cellStyleFactory.Create();
            cell.CellStyle.SetFont(this.defaultFont);
            return cell;
        }

        protected ICell CreateCell(IRow row, int columnIndex, double cellValue)
        {
            ICell cell = row.CreateCell(columnIndex);
            cell.SetCellValue(cellValue);
            cell.CellStyle = this.cellStyleFactory.Create();
            cell.CellStyle.SetFont(this.defaultFont);
            return cell;
        }

        protected ICell CreateCell(IRow row, int columnIndex, DateTime cellValue)
        {
            return CreateCell(row, columnIndex, cellValue, true);
        }

        protected ICell CreateCell(IRow row, int columnIndex, DateTime cellValue, bool includeDayOfWeek)
        {
            if (includeDayOfWeek)
            {
                return CreateCell(row, columnIndex, cellValue.ToString("yyyy/MM/dd (dddd)", cultureInfo));                
            }
            else
            {
                return CreateCell(row, columnIndex, cellValue.ToString("yyyy/MM/dd"));
            }     
        }

        protected ICell CreateCell(IRow row, int columnIndex, DateTime cellValue, string format)
        {
            return CreateCell(row, columnIndex, cellValue.ToString(format, cultureInfo));
        }


        protected ICell CreateCell(IRow row, int columnIndex, Object cellValue)
        {
            return CreateCell(row, columnIndex, cellValue.ToString());
        }
    }

    public class CellStyleFactory
    {
        protected IWorkbook workbook;

        public CellStyleFactory(IWorkbook workbook)
        {
            this.workbook = workbook;
        }

        public ICellStyle Create()
        {
            ICellStyle cellStyle = this.workbook.CreateCellStyle();
            cellStyle.Alignment = HorizontalAlignment.Center;
            cellStyle.VerticalAlignment = VerticalAlignment.Center;
            return cellStyle;
        }
    }
}