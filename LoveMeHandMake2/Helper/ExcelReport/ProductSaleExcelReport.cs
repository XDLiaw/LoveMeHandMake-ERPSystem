using log4net;
using LoveMeHandMake2.Models.ViewModels;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;

namespace LoveMeHandMake2.Helper.ExcelReport
{
    public class ProductSaleExcelReport : BaseExcelReport
    {
        public IWorkbook Create(ProductSaleReportViewModel arg)
        {
            //detail sheet
            ISheet detailSheet = this.workbook.CreateSheet("明細");
            int rowCount = 0;
            rowCount = createTitlePart(detailSheet, rowCount, arg.StoreName, arg.SearchDateStart, arg.SearchDateEnd);
            rowCount = createDataPart(detailSheet, rowCount, arg);
            for (int i = 0; i < 8; i++)
            {
                detailSheet.AutoSizeColumn(i);
            }
            detailSheet.SetColumnWidth(0, 30 * 256);
            detailSheet.CreateFreezePane(0, 5);

            // summary sheet 
            createSummerySheet(arg);

            return this.workbook;
        }


        private int createTitlePart(ISheet sheet, int rowCount, string storeName, DateTime? startTime, DateTime? endTime)
        {
            {
                IRow firstRow = sheet.CreateRow(rowCount++);
                sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 0, 7));
                ICell cell = base.CreateCell(firstRow, 0, storeName);
                IFont font = this.workbook.CreateFont();
                font.FontName = base.defaultFontName;
                font.FontHeightInPoints = 14;
                font.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
                cell.CellStyle.SetFont(font); 
            }
            {
                IRow row = sheet.CreateRow(rowCount);
                sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(1, 3, 0, 7));
                ICell cell = base.CreateCell(row, 0, String.Format("商品销售表（{0:yyyy/MM/dd}~{1:yyyy/MM/dd}）", startTime, endTime));
                rowCount = 4;
            }
            {
                int colCount = 0;
                IRow row = sheet.CreateRow(rowCount++);
                base.CreateCell(row, colCount++, "日期");
                base.CreateCell(row, colCount++, "商品名称");
                base.CreateCell(row, colCount++, "点数");
                base.CreateCell(row, colCount++, "豆数");
                base.CreateCell(row, colCount++, "会员卡号");
                base.CreateCell(row, colCount++, "金额");
                base.CreateCell(row, colCount++, "性别");
                base.CreateCell(row, colCount++, "指导老师");
            }
            return rowCount;
        }

        private int createDataPart(ISheet sheet, int rowCount, ProductSaleReportViewModel arg)
        {
            CultureInfo ci = new CultureInfo("zh-CN");
            foreach (ProductSaleRecord psr in arg.saleList)
            {
                for (int i = 0; i < psr.Amount; i++)
                {
                    int colCount = 0;
                    IRow row = sheet.CreateRow(rowCount++);
                    base.CreateCell(row, colCount++, psr.TradeDateTime);
                    base.CreateCell(row, colCount++, psr.ProductName);
                    base.CreateCell(row, colCount++, psr.UnitPoint.GetValueOrDefault());
                    base.CreateCell(row, colCount++, psr.UnitBean.GetValueOrDefault());
                    base.CreateCell(row, colCount++, String.IsNullOrWhiteSpace(psr.MemberCardID) ? "单做现金" : psr.MemberCardID);
                    base.CreateCell(row, colCount++, psr.Sum / psr.Amount);
                    base.CreateCell(row, colCount++, psr.Gender == null ? "" : (psr.Gender == true ? "男" : "女"));
                    base.CreateCell(row, colCount++, psr.TeacherName);
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
                    base.CreateCell(row, c, summary[r, c].ToString());
                }
            }
            summarySheet.AutoSizeColumn(0);
            summarySheet.AutoSizeColumn(1);
        }
    }
}