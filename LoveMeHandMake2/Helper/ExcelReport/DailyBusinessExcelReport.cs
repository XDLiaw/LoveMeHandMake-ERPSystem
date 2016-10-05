using LoveMeHandMake2.Models.ViewModels;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Helper.ExcelReport
{
    public class DailyBusinessExcelReport : BaseExcelReport
    {
        public IWorkbook Create(DailyBusinessReportViewModel arg)
        {
            ISheet sheet = this.workbook.CreateSheet();
            int rowCount = 0;
            rowCount = createTitlePart(sheet, rowCount, arg.StoreName);
            rowCount = createDataPart(sheet, rowCount, arg);
            for (int i = 0; i < 5; i++)
            {
                sheet.AutoSizeColumn(i);
            }
            sheet.SetColumnWidth(0, 30 * 256);
            sheet.CreateFreezePane(0, 4);

            return this.workbook;
        }

        private int createTitlePart(ISheet sheet, int rowCount, string storeName)
        {
            {
                IRow firstRow = sheet.CreateRow(rowCount++);
                sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 0, 4));
                ICell cell = base.CreateCell(firstRow, 0, string.Format("{0} 营业日报表", storeName));
                IFont font = this.workbook.CreateFont();
                font.FontName = base.defaultFontName;
                font.FontHeightInPoints = 18;
                font.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
                cell.CellStyle.SetFont(font);
            }
            sheet.CreateRow(rowCount++);
            sheet.CreateRow(rowCount++);
            {
                int colCount = 0;
                IRow titleRow = sheet.CreateRow(rowCount++);
                base.CreateCell(titleRow, colCount++, "日期");
                base.CreateCell(titleRow, colCount++, "现金");
                base.CreateCell(titleRow, colCount++, "信用卡");
                base.CreateCell(titleRow, colCount++, "商城卡");
                base.CreateCell(titleRow, colCount++, "支付宝");
                base.CreateCell(titleRow, colCount++, "微信支付");
                base.CreateCell(titleRow, colCount++, "其他支付");
                base.CreateCell(titleRow, colCount++, "当日业绩");
            }

            return rowCount;
        }

        public int createDataPart(ISheet sheet, int rowCount, DailyBusinessReportViewModel arg)
        {
            foreach (DailyBusinessRecord dbr in arg.DailyRecords)
            {
                int colCount = 0;
                IRow row = sheet.CreateRow(rowCount++);
                base.CreateCell(row, colCount++, dbr.Date);
                base.CreateCell(row, colCount++, dbr.Cash);
                base.CreateCell(row, colCount++, dbr.CreditCard);
                base.CreateCell(row, colCount++, dbr.MallCard);
                base.CreateCell(row, colCount++, dbr.Alipay);
                base.CreateCell(row, colCount++, dbr.WechatWallet);
                base.CreateCell(row, colCount++, dbr.OtherPay);
                base.CreateCell(row, colCount++, dbr.Total);
            }
            {
                IRow totalRow = sheet.CreateRow(rowCount);
                sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(rowCount, rowCount, 0, 3));
                rowCount++;

                base.CreateCell(totalRow, 0, "总业绩");
                base.CreateCell(totalRow, 4, arg.TotalMoney);
            }

            return rowCount;
        }


    }
}