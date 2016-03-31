using log4net;
using LoveMeHandMake2.Models.ViewModels;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web;

namespace LoveMeHandMake2.Helper.ExcelReport
{
    public class ProductSaleExcelReport : BaseExcelReport
    {
        public IWorkbook Create(ProductSaleReportViewModel arg)
        {
            base.Init();
            //IFont font_15 = wb.CreateFont();
            //font_15.FontHeightInPoints = 15;
            //font_15.FontName = "新細明體";

            //detail sheet
            ISheet detailSheet = this.workbook.CreateSheet("明細");
            int rowCount = 0;
            rowCount = createTitlePart(detailSheet, rowCount, arg.SearchDateStart, arg.SearchDateEnd);
            rowCount = createDataPart(detailSheet, rowCount, arg);
            for (int i = 0; i < 8; i++)
            {
                detailSheet.AutoSizeColumn(i);
            }
            detailSheet.SetColumnWidth(0, 30 * 256);

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
                cell.SetCellValue(String.Format("商品销售表（{0:yyyy/MM/dd}~{1:yyyy/MM/dd}）", startTime, endTime));
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
                productCell.SetCellValue("商品名称");
                productCell.CellStyle = this.defaultCellStyle_Center;

                ICell pointCell = titleRow.CreateCell(colCount++);
                pointCell.SetCellValue("点数");
                pointCell.CellStyle = this.defaultCellStyle_Center;

                ICell beanCell = titleRow.CreateCell(colCount++);
                beanCell.SetCellValue("豆数");
                beanCell.CellStyle = this.defaultCellStyle_Center;

                ICell memberCardCell = titleRow.CreateCell(colCount++);
                memberCardCell.SetCellValue("会员卡号");
                memberCardCell.CellStyle = this.defaultCellStyle_Center;

                ICell priceCell = titleRow.CreateCell(colCount++);
                priceCell.SetCellValue("金额");
                priceCell.CellStyle = this.defaultCellStyle_Center;

                ICell genderCell = titleRow.CreateCell(colCount++);
                genderCell.SetCellValue("性别");
                genderCell.CellStyle = this.defaultCellStyle_Center;

                ICell teacherCell = titleRow.CreateCell(colCount++);
                teacherCell.SetCellValue("指导老师");
                teacherCell.CellStyle = this.defaultCellStyle_Center;
            }
            return rowCount;
        }

        private int createDataPart(ISheet sheet, int rowCount, ProductSaleReportViewModel arg)
        {
            foreach (ProductSaleRecord psr in arg.saleList)
            {
                for (int i = 0; i < psr.Amount; i++)
                {
                    int colCount = 0;
                    IRow row = sheet.CreateRow(rowCount++);

                    ICell tradeDateCell = row.CreateCell(colCount++);
                    tradeDateCell.SetCellValue(psr.TradeDateTime);
                    tradeDateCell.CellStyle = this.defaultCellStyle_Date;


                    ICell productCell = row.CreateCell(colCount++);
                    productCell.SetCellValue(psr.ProductName);
                    productCell.CellStyle = this.defaultCellStyle_Center;

                    ICell pointCell = row.CreateCell(colCount++);
                    pointCell.SetCellValue(psr.UnitPoint.GetValueOrDefault());
                    pointCell.CellStyle = this.defaultCellStyle_Center;

                    ICell beanCell = row.CreateCell(colCount++);
                    beanCell.SetCellValue(psr.UnitBean.GetValueOrDefault());
                    beanCell.CellStyle = this.defaultCellStyle_Center;

                    ICell memberCardCell = row.CreateCell(colCount++);
                    memberCardCell.SetCellValue(String.IsNullOrWhiteSpace(psr.MemberCardID) ? "单做现金" : psr.MemberCardID);
                    memberCardCell.CellStyle = this.defaultCellStyle_Center;

                    ICell priceCell = row.CreateCell(colCount++);
                    priceCell.SetCellValue(psr.Sum / psr.Amount);
                    priceCell.CellStyle = this.defaultCellStyle_Center;

                    ICell genderCell = row.CreateCell(colCount++);
                    genderCell.SetCellValue(psr.Gender == null ? "" : (psr.Gender == true ? "男" : "女"));
                    genderCell.CellStyle = this.defaultCellStyle_Center;

                    ICell teacherCell = row.CreateCell(colCount++);
                    teacherCell.SetCellValue(psr.TeacherName);
                    teacherCell.CellStyle = this.defaultCellStyle_Center;
                }
            }
            return rowCount;
        }

        private void createSummerySheet(ProductSaleReportViewModel arg)
        {
            ISheet summarySheet = this.workbook.CreateSheet("總計");
            object[,] summary = new object[,] 
                { 
                    { "总消费点数", arg.TotalPoint }, 
                    { "总消费豆数", arg.TotalBean }, 
                    { "总消费金额", arg.TotalMoney }, 
                    { "会员消费次数", arg.MemberTradeTimes }, 
                    { "单做消费次数", arg.NonMemberTradeTimes }, 
                    { "总消费次数", arg.TotalTradeTimes }, 
                    { "平均客單", arg.AvgPrice }, 
                    { "周一至周五平均客流量", arg.WeekdayTradeTimes },
                    { "周末平均客流量", arg.WeekendTradeTimes }
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