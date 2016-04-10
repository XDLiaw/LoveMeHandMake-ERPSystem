using LoveMeHandMake2.Models.ViewModels;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Helper.ExcelReport
{
    public class TeacherPerformanceExcelReport : BaseExcelReport
    {

        public IWorkbook Create(TeacherPerformanceReportViewModel arg)
        {
            //detail sheet
            ISheet detailSheet = base.workbook.CreateSheet("明細");
            int rowCount = 0;
            rowCount = createTitlePart(detailSheet, rowCount, arg.StoreName, arg.SearchDateStart, arg.SearchDateEnd);
            rowCount = createDataPart(detailSheet, rowCount, arg);
            for (int c = 0; c <= detailSheet.GetRow(rowCount-1).Cells.Count; c++)
            {
                detailSheet.AutoSizeColumn(c);
            }
            detailSheet.SetColumnWidth(0, 30 * 256);
            detailSheet.CreateFreezePane(1, 5);

            // summary sheet 
            createSummerySheet(arg);

            return base.workbook;
        }

        private int createTitlePart(ISheet sheet, int rowCount, string storeName, DateTime? startTime, DateTime? endTime)
        {
            {
                IRow row = sheet.CreateRow(rowCount++);
                sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(rowCount - 1, rowCount - 1, 0, 10));
                string cellValue = String.Format("{0} 人员销售纪录表（{1:yyyy/MM/dd}~{2:yyyy/MM/dd}）", storeName, startTime, endTime);
                ICell cell = base.CreateCell(row, 0, cellValue);
                IFont font = base.workbook.CreateFont();
                font.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
                font.FontName = base.defaultFontName;
                font.FontHeightInPoints = 18;
                cell.CellStyle.SetFont(font);                
            }
            sheet.CreateRow(rowCount++);
            sheet.CreateRow(rowCount++);           
            
            return rowCount;
        }

        private int createDataPart(ISheet sheet, int rowCount, TeacherPerformanceReportViewModel arg)
        {
            {//老師名字列
                int colCount = 0;
                sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(rowCount, rowCount + 1, colCount, colCount++));
                IRow row = sheet.CreateRow(rowCount++);
                foreach (TeacherPerformance tp in arg.MultiTeacherPerformance)
                {
                    sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(rowCount - 1, rowCount - 1, colCount, colCount + 3));
                    base.CreateCell(row, colCount, tp.TeacherName);
                    colCount += 4;
                }
                colCount++;
                {
                    sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(rowCount - 1, rowCount - 1, colCount, colCount + 3));
                    base.CreateCell(row, colCount, "日统计");
                    colCount += 4;
                }
            }
            IFont fontBlack = base.workbook.CreateFont();
            fontBlack.Color = NPOI.HSSF.Util.HSSFColor.Black.Index;
            fontBlack.FontHeightInPoints = base.defaultFontHeightInPoints;
            fontBlack.FontName = base.defaultFontName;
            IFont fontBlue = base.workbook.CreateFont();
            fontBlue.Color = NPOI.HSSF.Util.HSSFColor.Blue.Index;
            fontBlue.FontHeightInPoints = base.defaultFontHeightInPoints;
            IFont fontRed = base.workbook.CreateFont();
            fontRed.Color = NPOI.HSSF.Util.HSSFColor.Red.Index;
            fontRed.FontHeightInPoints = base.defaultFontHeightInPoints;
            fontRed.FontName = base.defaultFontName;
            IFont fontGreen = base.workbook.CreateFont();
            fontGreen.Color = NPOI.HSSF.Util.HSSFColor.Green.Index;
            fontGreen.FontHeightInPoints = base.defaultFontHeightInPoints;
            fontGreen.FontName = base.defaultFontName;
            {// title 列
                int colCount = 1;
                IRow row = sheet.CreateRow(rowCount++);
                foreach (TeacherPerformance tp in arg.MultiTeacherPerformance)
                {
                    base.CreateCell(row, colCount++, "教学次数").CellStyle.SetFont(fontBlack);
                    base.CreateCell(row, colCount++, "教学点数").CellStyle.SetFont(fontBlue);
                    base.CreateCell(row, colCount++, "销售点数").CellStyle.SetFont(fontRed);
                    base.CreateCell(row, colCount++, "单做点数").CellStyle.SetFont(fontGreen);
                }
                colCount++;
                {
                    base.CreateCell(row, colCount++, "教学次数").CellStyle.SetFont(fontBlack);
                    base.CreateCell(row, colCount++, "教学点数").CellStyle.SetFont(fontBlue);
                    base.CreateCell(row, colCount++, "销售点数").CellStyle.SetFont(fontRed);
                    base.CreateCell(row, colCount++, "单做点数").CellStyle.SetFont(fontGreen);
                }
            }
            // 資料數值部分
            for (DateTime d = arg.SearchDateStart.GetValueOrDefault(); d <= arg.SearchDateEnd; d = d.AddDays(1))
            {
                int colCount = 0;
                IRow row = sheet.CreateRow(rowCount++);
                ICell dateCell = base.CreateCell(row, colCount++, d);

                foreach (TeacherPerformance tp in arg.MultiTeacherPerformance)
                {//各老師 資料
                    TeacherDailyPerformance tdp = tp.DailyPerformanceList.Where(x => x.Date == d).FirstOrDefault();
                    ICell cell1 = base.CreateCell(row, colCount++, tdp.TeachTimes);
                    cell1.CellStyle.SetFont(fontBlack);

                    ICell cell2 = base.CreateCell(row, colCount++, tdp.TeachPoints);
                    cell2.CellStyle.SetFont(fontBlue);

                    ICell cell3 = base.CreateCell(row, colCount++, tdp.SalesPoints);
                    cell3.CellStyle.SetFont(fontRed);

                    ICell cell4 = base.CreateCell(row, colCount++, tdp.PointsFromNonMember);
                    cell4.CellStyle.SetFont(fontGreen);
                }
                colCount++;
                {//日统计 資料
                    TeacherDailyPerformance tdp = arg.allTeacherPerformance.DailyPerformanceList.Where(x => x.Date == d).FirstOrDefault();
                    ICell cell1 = base.CreateCell(row, colCount++, tdp.TeachTimes);
                    cell1.CellStyle.SetFont(fontBlack);

                    ICell cell2 = base.CreateCell(row, colCount++, tdp.TeachPoints);
                    cell2.CellStyle.SetFont(fontBlue);

                    ICell cell3 = base.CreateCell(row, colCount++, tdp.SalesPoints);
                    cell3.CellStyle.SetFont(fontRed);

                    ICell cell4 = base.CreateCell(row, colCount++, tdp.PointsFromNonMember);
                    cell4.CellStyle.SetFont(fontGreen);
                }
            }
            {//最下方總計列 
                int colCount = 0;
                IRow row = sheet.CreateRow(rowCount++);
                ICell dateCell = base.CreateCell(row, colCount++, "总计");

                foreach (TeacherPerformance tp in arg.MultiTeacherPerformance)
                {
                    ICell cell1 = base.CreateCell(row, colCount++, tp.TotalTeachTimes);
                    ICell cell2 = base.CreateCell(row, colCount++, tp.TotalTeachPoints);
                    ICell cell3 = base.CreateCell(row, colCount++, tp.TotalSalesPoints);
                    ICell cell4 = base.CreateCell(row, colCount++, tp.TotalPointsFromNonMember);
                }
                colCount++;
                {//日统计
                    ICell cell1 = base.CreateCell(row, colCount++, arg.allTeacherPerformance.TotalTeachTimes);
                    ICell cell2 = base.CreateCell(row, colCount++, arg.allTeacherPerformance.TotalTeachPoints);
                    ICell cell3 = base.CreateCell(row, colCount++, arg.allTeacherPerformance.TotalSalesPoints);
                    ICell cell4 = base.CreateCell(row, colCount++, arg.allTeacherPerformance.TotalPointsFromNonMember);
                }
            }
            return rowCount;
        }

        private void createSummerySheet(TeacherPerformanceReportViewModel arg)
        {
            ISheet summarySheet = this.workbook.CreateSheet("總計");
            object[,] summary = new object[,] 
            { 
                { "总教学点数", arg.TotalTeachPoints }, 
                { "總教學點數百分比", String.Format("{0:N2}%", arg.TeachPointProportion*100) },
                { "总销售点数", arg.TotalSalesPoints }, 
                { "总单做点数", arg.TotalPointsFromNonMember },
                { "总销售比例百分比", String.Format("{0:N2}%", arg.SalesPointProportion*100) }, 
                { "总教学、销售、单做点数", arg.TotalPoints },             
                { "过槛奖金点数", arg.ThresholdPoint.GetValueOrDefault() }, 
                { "过槛每点直抽奖金", arg.OverThresholdBonus.GetValueOrDefault() },
                { "总奖金", arg.TotalBonus.GetValueOrDefault() }
            };
            for (int r = 0; r < summary.GetLength(0); r++)
            {
                IRow row = summarySheet.CreateRow(r);
                for (int c = 0; c < summary.GetLength(1); c++)
                {
                    base.CreateCell(row, c, summary[r, c].ToString());
                }
            }
            summarySheet.AutoSizeColumn(0);
            summarySheet.AutoSizeColumn(1);
        }
    }
}