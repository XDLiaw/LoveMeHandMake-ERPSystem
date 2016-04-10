using LoveMeHandMake2.Models.ViewModels;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Helper.ExcelReport
{
    public class TeacherPerformanceSummaryExcelReport : BaseExcelReport
    {
        public IWorkbook Create(TeacherPerformanceSummaryReportViewModel arg)
        {
            ISheet sheet = this.workbook.CreateSheet();
            int rowCount = 0;
            rowCount = createTitlePart(sheet, rowCount, arg.StoreName, arg.SearchDateStart, arg.SearchDateEnd);
            rowCount = createDataPart(sheet, rowCount, arg);
            for (int c = 0; c <= sheet.GetRow(rowCount-1).Cells.Count; c++)
            {
                sheet.AutoSizeColumn(c);
            }
            rowCount = createSummaryPart(sheet, rowCount, arg);
            sheet.SetColumnWidth(0, 30 * 256);

            return this.workbook;
        }

        private int createTitlePart(ISheet sheet, int rowCount, string storeName, DateTime? startTime, DateTime? endTime)
        {
            {
                IRow row = sheet.CreateRow(rowCount++);
                sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 0, 4));
                ICell cell = base.CreateCell(row, 0, String.Format("{0} 人员销售统计表（{1:yyyy/MM/dd}~{2:yyyy/MM/dd}）", storeName, startTime, endTime));
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
                base.CreateCell(titleRow, colCount++, "人员");
                base.CreateCell(titleRow, colCount++, "教学次数");
                base.CreateCell(titleRow, colCount++, "教学点数");
                base.CreateCell(titleRow, colCount++, "销售点数");
                base.CreateCell(titleRow, colCount++, "单做点数");
            }
            return rowCount;
        }

        private int createDataPart(ISheet sheet, int rowCount, TeacherPerformanceSummaryReportViewModel arg)
        {
            foreach (TeacherPerformanceSummary tps in arg.TeacherPerformanceSummaryList)
            {
                int colCount = 0;
                IRow row = sheet.CreateRow(rowCount++);
                base.CreateCell(row, colCount++, tps.TeacherName);
                base.CreateCell(row, colCount++, tps.TeachTimes);
                base.CreateCell(row, colCount++, tps.TeachPoints);
                base.CreateCell(row, colCount++, tps.SalesPoints);
                base.CreateCell(row, colCount++, tps.PointsFromNonMember);
            }
            {
                int colCount = 0;
                IRow row = sheet.CreateRow(rowCount++);
                base.CreateCell(row, colCount++, arg.AvgTeacherPerformanceSummary.TeacherName);
                base.CreateCell(row, colCount++, arg.AvgTeacherPerformanceSummary.TeachTimes);
                base.CreateCell(row, colCount++, arg.AvgTeacherPerformanceSummary.TeachPoints);
                base.CreateCell(row, colCount++, arg.AvgTeacherPerformanceSummary.SalesPoints);
                base.CreateCell(row, colCount++, arg.AvgTeacherPerformanceSummary.PointsFromNonMember);

                IFont font = this.workbook.CreateFont();
                font.Color = NPOI.HSSF.Util.HSSFColor.Blue.Index;
                font.FontName = base.defaultFontName;
                font.FontHeightInPoints = base.defaultFontHeightInPoints;
                foreach (ICell cell in row.Cells)
                {
                    cell.CellStyle.SetFont(font);
                }
            }


            return rowCount;
        }

        private int createSummaryPart(ISheet sheet, int rowCount, TeacherPerformanceSummaryReportViewModel arg)
        {
            {
                IRow row = sheet.CreateRow(rowCount++);
                base.CreateCell(row, 0, "来客数");

                sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(rowCount - 1, rowCount - 1, 1, 2));
                base.CreateCell(row, 1, arg.TotalTeachTimes);

                IFont font = this.workbook.CreateFont();
                font.FontHeightInPoints = 18;
                font.FontName = base.defaultFontName;
                font.Color = NPOI.HSSF.Util.HSSFColor.Black.Index;
                foreach (ICell cell in row.Cells)
                {
                    cell.CellStyle.SetFont(font);
                }
            }
            {
                IRow row = sheet.CreateRow(rowCount++);
                base.CreateCell(row, 0, "平均客单价");

                sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(rowCount - 1, rowCount - 1, 1, 2));
                base.CreateCell(row, 1, arg.AvgPrice);

                IFont font = this.workbook.CreateFont();
                font.FontHeightInPoints = 18;
                font.FontName = base.defaultFontName;
                font.Color = NPOI.HSSF.Util.HSSFColor.Blue.Index;
                foreach (ICell cell in row.Cells)
                {
                    cell.CellStyle.SetFont(font);
                }
            }
            {
                IRow row = sheet.CreateRow(rowCount++);
                base.CreateCell(row, 0, "办卡率");

                sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(rowCount - 1, rowCount - 1, 1, 2));
                base.CreateCell(row, 1, arg.MemberConsumptionPercentage);

                IFont font = this.workbook.CreateFont();
                font.FontHeightInPoints = 18;
                font.FontName = base.defaultFontName;
                font.Color = NPOI.HSSF.Util.HSSFColor.Red.Index;
                foreach (ICell cell in row.Cells)
                {
                    cell.CellStyle.SetFont(font);
                }
            }

            return rowCount;
        }


    }
}