using log4net;
using LoveMeHandMake2.Models.ViewModels;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Helper.ExcelReport
{
    public class DepositExcelReport : BaseExcelReport
    {
        public IWorkbook Create(DepositReportViewModel arg)
        {
            //detail sheet
            ISheet detailSheet = this.workbook.CreateSheet("明細");
            int rowCount = 0;
            rowCount = createTitlePart(detailSheet, rowCount, arg.StoreName, arg.SearchDateStart,arg.SearchDateEnd);
            rowCount = createDataPart(detailSheet, rowCount, arg);
            for (int i = 0; i < 8; i++)
            {
                detailSheet.AutoSizeColumn(i);
            }
            detailSheet.SetColumnWidth(0, 30*256);
            detailSheet.SetColumnWidth(2, 30*256);
            detailSheet.CreateFreezePane(0, 5);

            // summary sheet 
            createSummerySheet(arg);

            return this.workbook;
        }
        
        private int createTitlePart(ISheet sheet, int rowCount, string storeName, DateTime? startTime, DateTime? endTime)
        {
            {
                IRow row = sheet.CreateRow(rowCount++);
                sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 0, 7));
                ICell cell = base.CreateCell(row, 0, storeName);
                IFont font = this.workbook.CreateFont();
                font.FontName = base.defaultFontName;
                font.FontHeightInPoints = 18;
                font.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
                cell.CellStyle.SetFont(font); 
            }
            {
                IRow row = sheet.CreateRow(rowCount++);
                sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(1, 3, 0, 7));
                string cellVal = String.Format("{0} 会员点数销售表＆通讯录（{1:yyyy/MM/dd}~{2:yyyy/MM/dd}）", storeName, startTime, endTime);
                ICell cell = base.CreateCell(row, 0, cellVal);               
            }
            rowCount++;
            rowCount++;
            {
                int colCount = 0;
                IRow titleRow = sheet.CreateRow(rowCount++);
                base.CreateCell(titleRow, colCount++, "日期");
                base.CreateCell(titleRow, colCount++, "姓名");
                base.CreateCell(titleRow, colCount++, "生日");
                base.CreateCell(titleRow, colCount++, "性别");
                base.CreateCell(titleRow, colCount++, "销售点数");
                base.CreateCell(titleRow, colCount++, "会员卡号");
                base.CreateCell(titleRow, colCount++, "销售人员");
                base.CreateCell(titleRow, colCount++, "电话");
            }
            return rowCount;
        }

        private int createDataPart(ISheet sheet, int rowCount, DepositReportViewModel arg)
        {
            foreach (DepositRecord dr in arg.DepositList)
            {
                int colCount = 0;
                IRow row = sheet.CreateRow(rowCount++);
                base.CreateCell(row, colCount++, dr.DepositTime);
                base.CreateCell(row, colCount++, dr.MemberName);
                base.CreateCell(row, colCount++, dr.MemberBirthday, false);
                base.CreateCell(row, colCount++, dr.MemberGender == true ? "男" : "女");
                base.CreateCell(row, colCount++, dr.Point);
                base.CreateCell(row, colCount++, dr.MemberCardID);
                base.CreateCell(row, colCount++, dr.TeacherName);
                base.CreateCell(row, colCount++, dr.MemberPhone);
            }

            return rowCount;
        }

        private void createSummerySheet(DepositReportViewModel arg)
        {
            ISheet summarySheet = this.workbook.CreateSheet("业绩销售小计");
            int r = 0;
            for (; r < arg.TeacherSalesPerformanceList.Count; r ++) 
            {
                IRow row = summarySheet.CreateRow(r);
                base.CreateCell(row, 0, arg.TeacherSalesPerformanceList.ElementAt(r).TeacherName);
                base.CreateCell(row, 1, arg.TeacherSalesPerformanceList.ElementAt(r).Point);
            }
            IRow totalRow = summarySheet.CreateRow(r++);
            base.CreateCell(totalRow, 0, "会员卡销售小计");
            base.CreateCell(totalRow, 1, arg.TotalPoint);
            summarySheet.AutoSizeColumn(0);
            summarySheet.AutoSizeColumn(1);
        }
    }
}