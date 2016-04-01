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
            base.Init();
            ISheet sheet = this.workbook.CreateSheet();
            int rowCount = 0;
            rowCount = createTitlePart(sheet, rowCount, arg.StoreName);
            rowCount = createDataPart(sheet, rowCount, arg);
            for (int i = 0; i < 5; i++)
            {
                sheet.AutoSizeColumn(i);
            }
            sheet.SetColumnWidth(0, 30 * 256);

            return this.workbook;
        }

        private int createTitlePart(ISheet sheet, int rowCount, string storeName)
        {
            {
                IRow firstRow = sheet.CreateRow(rowCount++);
                sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 0, 4));
                ICell cell = firstRow.CreateCell(0);
                cell.SetCellValue(string.Format("{0} 营业日报表", storeName));
                cell.CellStyle = this.defaultCellStyle_Center;
            }
            sheet.CreateRow(rowCount++);
            sheet.CreateRow(rowCount++);
            {
                int colCount = 0;
                IRow titleRow = sheet.CreateRow(rowCount++);

                ICell dateCell = titleRow.CreateCell(colCount++);
                dateCell.SetCellValue("日期");
                dateCell.CellStyle = this.defaultCellStyle_Center;

                ICell productCell = titleRow.CreateCell(colCount++);
                productCell.SetCellValue("现金");
                productCell.CellStyle = this.defaultCellStyle_Center;

                ICell pointCell = titleRow.CreateCell(colCount++);
                pointCell.SetCellValue("信用卡");
                pointCell.CellStyle = this.defaultCellStyle_Center;

                ICell beanCell = titleRow.CreateCell(colCount++);
                beanCell.SetCellValue("商城卡");
                beanCell.CellStyle = this.defaultCellStyle_Center;

                ICell memberCardCell = titleRow.CreateCell(colCount++);
                memberCardCell.SetCellValue("当日业绩");
                memberCardCell.CellStyle = this.defaultCellStyle_Center;
            }

            return rowCount;
        }

        public int createDataPart(ISheet sheet, int rowCount, DailyBusinessReportViewModel arg)
        {
            foreach (DailyBusinessRecord dbr in arg.DailyRecords)
            {
                int colCount = 0;
                IRow row = sheet.CreateRow(rowCount++);

                ICell dateCell = row.CreateCell(colCount++);
                dateCell.SetCellValue(dbr.Date);
                dateCell.CellStyle = this.defaultCellStyle_Date;

                ICell cashCell = row.CreateCell(colCount++);
                cashCell.SetCellValue(dbr.Cash);
                cashCell.CellStyle = this.defaultCellStyle_Center;

                ICell creditCardCell = row.CreateCell(colCount++);
                creditCardCell.SetCellValue(dbr.CreditCard);
                creditCardCell.CellStyle = this.defaultCellStyle_Center;

                ICell MallCardCell = row.CreateCell(colCount++);
                MallCardCell.SetCellValue(dbr.MallCard);
                MallCardCell.CellStyle = this.defaultCellStyle_Center;

                ICell totalCell = row.CreateCell(colCount++);
                totalCell.SetCellValue(dbr.Total);
                totalCell.CellStyle = this.defaultCellStyle_Center;
            }
            {
                IRow totalRow = sheet.CreateRow(rowCount);
                sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(rowCount, rowCount, 0, 3));

                ICell cell = totalRow.CreateCell(0);
                cell.CellStyle = this.defaultCellStyle_Center;
                cell.SetCellValue("总业绩");

                ICell totalCell = totalRow.CreateCell(4);
                totalCell.SetCellValue(arg.TotalMoney);
                totalCell.CellStyle = this.defaultCellStyle_Center;
                rowCount++;
            }

            return rowCount;
        }


    }
}