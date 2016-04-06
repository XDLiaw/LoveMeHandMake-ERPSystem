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
            base.Init();
            //detail sheet
            ISheet detailSheet = this.workbook.CreateSheet("明細");
            int rowCount = 0;
            rowCount = createTitlePart(detailSheet, rowCount, arg.StoreName, arg.SearchDateStart, arg.SearchDateEnd);
            rowCount = createDataPart(detailSheet, rowCount, arg);
            detailSheet.SetColumnWidth(0, 30 * 256);

            // summary sheet 
            createSummerySheet(arg);

            return this.workbook;
        }

        private int createTitlePart(ISheet sheet, int rowCount, string storeName, DateTime? startTime, DateTime? endTime)
        {
            {
                IRow row = sheet.CreateRow(rowCount++);
                sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(rowCount - 1, rowCount - 1, 0, 7));
                ICell cell = row.CreateCell(0);
                cell.SetCellValue(String.Format("{0} 人员销售纪录表（{1:yyyy/MM/dd}~{2:yyyy/MM/dd}）", storeName, startTime, endTime));
                cell.CellStyle = this.defaultCellStyle_Center;
            }
            {
                IRow row = sheet.CreateRow(rowCount++);
            }
            {
                IRow row = sheet.CreateRow(rowCount++);
            }            
            
            return rowCount;
        }

        private int createDataPart(ISheet sheet, int rowCount, TeacherPerformanceReportViewModel arg)
        {
            {
                int colCount = 0;
                sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(rowCount, rowCount + 1, colCount, colCount++));
                IRow row = sheet.CreateRow(rowCount++);
                foreach (TeacherPerformance tp in arg.MultiTeacherPerformance)
                {
                    sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(rowCount - 1, rowCount - 1, colCount, colCount + 3));
                    ICell cell = row.CreateCell(colCount);
                    cell.SetCellValue(tp.TeacherName);
                    cell.CellStyle = this.defaultCellStyle_Center;
                    colCount += 4;
                }
                colCount++;
                {
                    sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(rowCount - 1, rowCount - 1, colCount, colCount + 3));
                    ICell AllTeacherCell = row.CreateCell(colCount);
                    AllTeacherCell.SetCellValue("日统计");
                    AllTeacherCell.CellStyle = this.defaultCellStyle_Center;
                    colCount += 4;
                }
            }
            {
                int colCount = 1;
                IRow row = sheet.CreateRow(rowCount++);
                //XSSFColor colorToFill = new XSSFColor(Color.Black);
                foreach (TeacherPerformance tp in arg.MultiTeacherPerformance)
                {
                    ICell cell1 = row.CreateCell(colCount++);
                    cell1.SetCellValue("教学次数");
                    cell1.CellStyle = this.defaultCellStyle_Center;

                    ICell cell2 = row.CreateCell(colCount++);
                    cell2.SetCellValue("教学点数");
                    cell2.CellStyle = this.defaultCellStyle_Center;

                    ICell cell3 = row.CreateCell(colCount++);
                    cell3.SetCellValue("销售点数");
                    cell3.CellStyle = this.defaultCellStyle_Center;

                    ICell cell4 = row.CreateCell(colCount++);
                    cell4.SetCellValue("单做点数");
                    cell4.CellStyle = this.defaultCellStyle_Center;
                }
            }
            for (DateTime d = arg.SearchDateStart.GetValueOrDefault(); d <= arg.SearchDateEnd; d = d.AddDays(1))
            {
                int colCount = 0;
                IRow row = sheet.CreateRow(rowCount++);
                ICell dateCell = row.CreateCell(colCount++);
                dateCell.SetCellValue(d);
                dateCell.CellStyle = this.defaultCellStyle_Date;

                foreach (TeacherPerformance tp in arg.MultiTeacherPerformance)
                {
                    TeacherDailyPerformance tdp = tp.DailyPerformanceList.Where(x => x.Date == d).FirstOrDefault();
                    ICell cell1 = row.CreateCell(colCount++);
                    cell1.SetCellValue(tdp.TeachTimes);
                    cell1.CellStyle = this.defaultCellStyle_Center;

                    ICell cell2 = row.CreateCell(colCount++);
                    cell2.SetCellValue(tdp.TeachPoints);
                    cell2.CellStyle = this.defaultCellStyle_Center;

                    ICell cell3 = row.CreateCell(colCount++);
                    cell3.SetCellValue(tdp.SalesPoints);
                    cell3.CellStyle = this.defaultCellStyle_Center;

                    ICell cell4 = row.CreateCell(colCount++);
                    cell4.SetCellValue(tdp.PointsFromNonMember);
                    cell4.CellStyle = this.defaultCellStyle_Center;
                }
                colCount++;
                {//日统计
                    TeacherDailyPerformance tdp = arg.allTeacherPerformance.DailyPerformanceList.Where(x => x.Date == d).FirstOrDefault();
                    ICell cell1 = row.CreateCell(colCount++);
                    cell1.SetCellValue(tdp.TeachTimes);
                    cell1.CellStyle = this.defaultCellStyle_Center;

                    ICell cell2 = row.CreateCell(colCount++);
                    cell2.SetCellValue(tdp.TeachPoints);
                    cell2.CellStyle = this.defaultCellStyle_Center;

                    ICell cell3 = row.CreateCell(colCount++);
                    cell3.SetCellValue(tdp.SalesPoints);
                    cell3.CellStyle = this.defaultCellStyle_Center;

                    ICell cell4 = row.CreateCell(colCount++);
                    cell4.SetCellValue(tdp.PointsFromNonMember);
                    cell4.CellStyle = this.defaultCellStyle_Center;
                }
            }
            {//最下方總計列 
                int colCount = 0;
                IRow row = sheet.CreateRow(rowCount++);
                ICell dateCell = row.CreateCell(colCount++);
                dateCell.SetCellValue("总计");
                dateCell.CellStyle = this.defaultCellStyle_Center;

                foreach (TeacherPerformance tp in arg.MultiTeacherPerformance)
                {
                    ICell cell1 = row.CreateCell(colCount++);
                    cell1.SetCellValue(tp.TotalTeachTimes);
                    cell1.CellStyle = this.defaultCellStyle_Center;

                    ICell cell2 = row.CreateCell(colCount++);
                    cell2.SetCellValue(tp.TotalTeachPoints);
                    cell2.CellStyle = this.defaultCellStyle_Center;

                    ICell cell3 = row.CreateCell(colCount++);
                    cell3.SetCellValue(tp.TotalSalesPoints);
                    cell3.CellStyle = this.defaultCellStyle_Center;

                    ICell cell4 = row.CreateCell(colCount++);
                    cell4.SetCellValue(tp.TotalPointsFromNonMember);
                    cell4.CellStyle = this.defaultCellStyle_Center;
                }

                colCount++;
                {//日统计
                    ICell cell1 = row.CreateCell(colCount++);
                    cell1.SetCellValue(arg.allTeacherPerformance.TotalTeachTimes);
                    cell1.CellStyle = this.defaultCellStyle_Center;

                    ICell cell2 = row.CreateCell(colCount++);
                    cell2.SetCellValue(arg.allTeacherPerformance.TotalTeachPoints);
                    cell2.CellStyle = this.defaultCellStyle_Center;

                    ICell cell3 = row.CreateCell(colCount++);
                    cell3.SetCellValue(arg.allTeacherPerformance.TotalSalesPoints);
                    cell3.CellStyle = this.defaultCellStyle_Center;

                    ICell cell4 = row.CreateCell(colCount++);
                    cell4.SetCellValue(arg.allTeacherPerformance.TotalPointsFromNonMember);
                    cell4.CellStyle = this.defaultCellStyle_Center;
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
                    ICell cell = row.CreateCell(c);
                    cell.SetCellValue(summary[r, c].ToString());
                    cell.CellStyle = this.defaultCellStyle_Center;
                }
            }
            summarySheet.AutoSizeColumn(0);
            summarySheet.AutoSizeColumn(1);
        }
    }
}