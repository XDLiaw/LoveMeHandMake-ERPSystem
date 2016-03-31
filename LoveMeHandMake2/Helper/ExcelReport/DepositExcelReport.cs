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
            base.Init();

            //detail sheet
            ISheet detailSheet = this.workbook.CreateSheet("明細");
            int rowCount = 0;
            rowCount = createTitlePart(detailSheet, rowCount, arg.SearchDateStart,arg.SearchDateEnd);
            rowCount = createDataPart(detailSheet, rowCount, arg);
            for (int i = 0; i < 8; i++)
            {
                detailSheet.AutoSizeColumn(i);
            }
            detailSheet.SetColumnWidth(0, 30*256);
            detailSheet.SetColumnWidth(2, 30*256);

            // summary sheet 
            createSummerySheet(arg);

            return this.workbook;
        }



        private int createTitlePart(ISheet sheet, int rowCount, DateTime? startTime, DateTime? endTime)
        {
            {
                IRow firstRow = sheet.CreateRow(rowCount++);
                sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 0, 7));
                ICell cell = firstRow.CreateCell(0);
                cell.SetCellValue("国贸360 - 巧乐思");
                cell.CellStyle = this.defaultCellStyle_Center;
            }
            {
                IRow secondRow = sheet.CreateRow(rowCount++);
                sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(1, 3, 0, 7));
                ICell cell = secondRow.CreateCell(0);
                cell.SetCellValue(String.Format("国贸- 巧乐思 会员点数销售表＆通讯录（{0:yyyy/MM/dd}~{1:yyyy/MM/dd}）", startTime, endTime));
                cell.CellStyle = this.defaultCellStyle_Center;
                rowCount = 4;
            }
            {
                int colCount = 0;
                IRow titleRow = sheet.CreateRow(rowCount++);
                ICell dateCell = titleRow.CreateCell(colCount++);
                dateCell.SetCellValue("日期");
                dateCell.CellStyle = this.defaultCellStyle_Center;

                ICell productCell = titleRow.CreateCell(colCount++);
                productCell.SetCellValue("姓名");
                productCell.CellStyle = this.defaultCellStyle_Center;

                ICell pointCell = titleRow.CreateCell(colCount++);
                pointCell.SetCellValue("生日");
                pointCell.CellStyle = this.defaultCellStyle_Center;

                ICell beanCell = titleRow.CreateCell(colCount++);
                beanCell.SetCellValue("性别");
                beanCell.CellStyle = this.defaultCellStyle_Center;

                ICell memberCardCell = titleRow.CreateCell(colCount++);
                memberCardCell.SetCellValue("销售点数");
                memberCardCell.CellStyle = this.defaultCellStyle_Center;

                ICell priceCell = titleRow.CreateCell(colCount++);
                priceCell.SetCellValue("会员卡号");
                priceCell.CellStyle = this.defaultCellStyle_Center;

                ICell genderCell = titleRow.CreateCell(colCount++);
                genderCell.SetCellValue("销售人员");
                genderCell.CellStyle = this.defaultCellStyle_Center;

                ICell teacherCell = titleRow.CreateCell(colCount++);
                teacherCell.SetCellValue("电话");
                teacherCell.CellStyle = this.defaultCellStyle_Center;
            }
            return rowCount;
        }

        private int createDataPart(ISheet sheet, int rowCount, DepositReportViewModel arg)
        {
            foreach (DepositRecord dr in arg.DepositList)
            {
                int colCount = 0;
                IRow row = sheet.CreateRow(rowCount++);

                ICell tradeDateCell = row.CreateCell(colCount++);
                tradeDateCell.SetCellValue(dr.DepositTime);
                tradeDateCell.CellStyle = this.defaultCellStyle_Date;

                ICell nameCell = row.CreateCell(colCount++);
                nameCell.SetCellValue(dr.MemberName);
                nameCell.CellStyle = this.defaultCellStyle_Center;

                ICell birthdayCell = row.CreateCell(colCount++);
                birthdayCell.SetCellValue(dr.MemberBirthday);
                birthdayCell.CellStyle = this.defaultCellStyle_Date;

                ICell genderCell = row.CreateCell(colCount++);
                genderCell.SetCellValue(dr.MemberGender == null ? "" : (dr.MemberGender == true ? "男" : "女"));
                genderCell.CellStyle = this.defaultCellStyle_Center;

                ICell pointCell = row.CreateCell(colCount++);
                pointCell.SetCellValue(dr.Point);
                pointCell.CellStyle = this.defaultCellStyle_Center;

                ICell cardIDCell = row.CreateCell(colCount++);
                cardIDCell.SetCellValue(dr.MemberCardID);
                cardIDCell.CellStyle = this.defaultCellStyle_Center;

                ICell teacherCell = row.CreateCell(colCount++);
                teacherCell.SetCellValue(dr.TeacherName);
                teacherCell.CellStyle = this.defaultCellStyle_Center;

                ICell phoneCell = row.CreateCell(colCount++);
                phoneCell.SetCellValue(dr.MemberPhone);
                phoneCell.CellStyle = this.defaultCellStyle_Center;
            }

            return rowCount;
        }

        private void createSummerySheet(DepositReportViewModel arg)
        {
            ISheet summarySheet = this.workbook.CreateSheet("个人业绩销售小计");
            for (int r = 0; r < arg.TeacherSalesPerformanceList.Count; r ++) 
            {
                IRow row = summarySheet.CreateRow(r);

                ICell teacherNameCell = row.CreateCell(0);
                teacherNameCell.SetCellValue(arg.TeacherSalesPerformanceList.ElementAt(r).TeacherName);
                teacherNameCell.CellStyle = this.defaultCellStyle_Center;

                ICell pointCell = row.CreateCell(1);
                pointCell.SetCellValue(arg.TeacherSalesPerformanceList.ElementAt(r).Point);
                pointCell.CellStyle = this.defaultCellStyle_Center;
            }
            summarySheet.AutoSizeColumn(0);
            summarySheet.AutoSizeColumn(1);
        }
    }
}