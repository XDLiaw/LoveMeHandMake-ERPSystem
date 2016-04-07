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
            base.Init();
            ISheet sheet = this.workbook.CreateSheet();
            int rowCount = 0;
            rowCount = createTitlePart(sheet, rowCount, arg.StoreName, arg.SearchDateStart, arg.SearchDateEnd);
            rowCount = createDataPart(sheet, rowCount, arg);
            rowCount = createSummaryPart(sheet, rowCount, arg);
            sheet.SetColumnWidth(0, 30 * 256);

            return this.workbook;
        }

        private int createTitlePart(ISheet sheet, int rowCount, string storeName, DateTime? startTime, DateTime? endTime)
        {
            {
                IRow row = sheet.CreateRow(rowCount++);
                sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 0, 4));
                ICell cell = row.CreateCell(0);
                cell.SetCellValue(String.Format("{0} 人员销售统计表（{1:yyyy/MM/dd}~{2:yyyy/MM/dd}）", storeName, startTime, endTime));
                cell.CellStyle = this.defaultCellStyle_Center;
            }
            sheet.CreateRow(rowCount++);
            sheet.CreateRow(rowCount++);
            {
                int colCount = 0;
                IRow titleRow = sheet.CreateRow(rowCount++);

                ICell teacherCell = titleRow.CreateCell(colCount++);
                teacherCell.SetCellValue("人员");
                teacherCell.CellStyle = this.defaultCellStyle_Center;

                ICell teachTimesCell = titleRow.CreateCell(colCount++);
                teachTimesCell.SetCellValue("教学次数");
                teachTimesCell.CellStyle = this.defaultCellStyle_Center;

                ICell teachPointCell = titleRow.CreateCell(colCount++);
                teachPointCell.SetCellValue("教学点数");
                teachPointCell.CellStyle = this.defaultCellStyle_Center;

                ICell salePointCell = titleRow.CreateCell(colCount++);
                salePointCell.SetCellValue("销售点数");
                salePointCell.CellStyle = this.defaultCellStyle_Center;

                ICell pointFromNonMemberCell = titleRow.CreateCell(colCount++);
                pointFromNonMemberCell.SetCellValue("单做点数");
                pointFromNonMemberCell.CellStyle = this.defaultCellStyle_Center;
            }
            return rowCount;
        }

        private int createDataPart(ISheet sheet, int rowCount, TeacherPerformanceSummaryReportViewModel arg)
        {
            foreach (TeacherPerformanceSummary tps in arg.TeacherPerformanceSummaryList)
            {
                int colCount = 0;
                IRow row = sheet.CreateRow(rowCount++);

                ICell teacherCell = row.CreateCell(colCount++);
                teacherCell.SetCellValue(tps.TeacherName);
                teacherCell.CellStyle = this.defaultCellStyle_Center;

                ICell teachTimesCell = row.CreateCell(colCount++);
                teachTimesCell.SetCellValue(tps.TeachTimes);
                teachTimesCell.CellStyle = this.defaultCellStyle_Center;

                ICell teachPointCell = row.CreateCell(colCount++);
                teachPointCell.SetCellValue(tps.TeachPoints);
                teachPointCell.CellStyle = this.defaultCellStyle_Center;

                ICell salePointCell = row.CreateCell(colCount++);
                salePointCell.SetCellValue(tps.SalesPoints);
                salePointCell.CellStyle = this.defaultCellStyle_Center;

                ICell pointFromNonMemberCell = row.CreateCell(colCount++);
                pointFromNonMemberCell.SetCellValue(tps.PointsFromNonMember);
                pointFromNonMemberCell.CellStyle = this.defaultCellStyle_Center;
            }

            return rowCount;
        }

        private int createSummaryPart(ISheet sheet, int rowCount, TeacherPerformanceSummaryReportViewModel arg)
        {
            {
                IRow row = sheet.CreateRow(rowCount++);
                ICell cell = row.CreateCell(0);
                cell.SetCellValue("来客数");
                cell.CellStyle = this.defaultCellStyle_Center;

                sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(rowCount - 1, rowCount - 1, 1, 2));
                ICell valueCell = row.CreateCell(1);
                valueCell.SetCellValue(arg.TotalTeachTimes);
                valueCell.CellStyle = this.defaultCellStyle_Center;
            }
            {
                IRow row = sheet.CreateRow(rowCount++);
                ICell cell = row.CreateCell(0);
                cell.SetCellValue("平均客单价");
                cell.CellStyle = this.defaultCellStyle_Center;

                sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(rowCount - 1, rowCount - 1, 1, 2));
                ICell valueCell = row.CreateCell(1);
                valueCell.SetCellValue(arg.AvgPrice);
                valueCell.CellStyle = this.defaultCellStyle_Center;
            }
            {
                IRow row = sheet.CreateRow(rowCount++);
                ICell cell = row.CreateCell(0);
                cell.SetCellValue("办卡率");
                cell.CellStyle = this.defaultCellStyle_Center;

                sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(rowCount - 1, rowCount - 1, 1, 2));
                ICell valueCell = row.CreateCell(1);
                valueCell.SetCellValue(arg.MemberConsumptionPercentage);
                valueCell.CellStyle = this.defaultCellStyle_Center;
            }

            return rowCount;
        }


    }
}