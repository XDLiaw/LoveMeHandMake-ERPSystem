using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Configuration;

namespace LoveMeHandMake2.Services
{
    public class ProductImportService
    {
        public static string ImportExcel(Stream stream)
        {
            string imageDir = WebConfigurationManager.AppSettings["ProductImageFolder"];
            if (imageDir.EndsWith("\\") == false)
            {
                imageDir += "\\";
            }
            string connStr = ConfigurationManager.ConnectionStrings["LoveMeHandMakeContext"].ConnectionString;

            SqlConnection conn = null;
            try
            {
                conn = new SqlConnection(connStr);
                conn.Open();

                #region 讀分類編號跟名稱對照表

                Dictionary<string, string> categoryMap = new Dictionary<string, string>();
                using (SqlCommand cmd = new SqlCommand("SELECT ID, Name FROM ProductCategory", conn))
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        categoryMap.Add(reader["Name"].ToString().Trim(), reader["ID"].ToString().Trim());
                    }
                    reader.Close();
                }

                #endregion

                IWorkbook workbook = WorkbookFactory.Create(stream);

                XSSFDrawing drawing = null;
                List<XSSFShape> picList = null;
                ISheet sheet = null;
                IRow row = null;
                ICell cell = null;
                string catalog, catalogID, name, imageName;
                int price, count = 0;

                #region 檢查

                try
                {
                    for (int i = 0; i < workbook.NumberOfSheets; i++)
                    {
                        sheet = workbook.GetSheetAt(i);
                        drawing = (XSSFDrawing)sheet.CreateDrawingPatriarch();
                        picList = drawing.GetShapes();

                        int j = 0;
                        while (true)
                        {
                            row = sheet.GetRow(++j);
                            if (row == null) { break; }

                            #region 名稱

                            try
                            {
                                if ((cell = row.GetCell(1)) == null) { break; }
                                name = GetCellValue(cell).Replace("\r", "").Replace("\n", "");
                                if (name.Length == 0) { break; }
                            }
                            catch
                            {
                                break;
                            }

                            #endregion

                            #region 分類

                            try
                            {
                                if ((cell = row.GetCell(2)) == null) { return "页签：" + sheet.SheetName + "/第" + "列[分类]资料有问题"; }
                                catalog = GetCellValue(cell);
                                if (catalog.Length == 0) { return "页签：" + sheet.SheetName + "，第" + "列[分类]资料有问题"; }

                                if (!categoryMap.ContainsKey(catalog))
                                {
                                    return "页签：" + sheet.SheetName + "，第" + "列[分类]不存在";
                                }
                                else
                                {
                                    catalogID = categoryMap[catalog];
                                }
                            }
                            catch
                            {
                                return "页签：" + sheet.SheetName + "/第" + "列[分类]资料有问题";
                            }

                            #endregion

                            #region 點數

                            try
                            {
                                if ((cell = row.GetCell(3)) == null) { return "页签：" + sheet.SheetName + "/第" + "列[点数]资料有问题"; }
                                price = int.Parse(GetCellValue(cell).Replace("点", "").Replace("豆", ""));
                            }
                            catch
                            {
                                return "页签：" + sheet.SheetName + "/第" + "列[点数]资料有问题";
                            }

                            #endregion

                            #region 圖檔

                            bool find = false;
                            for (int k = picList.Count - 1; k > -1; k--)
                            {
                                XSSFPicture picture = (XSSFPicture)picList[k];
                                XSSFClientAnchor anchor = (XSSFClientAnchor)picture.GetPreferredSize();
                                if ((anchor.Row1 == j && anchor.Row2 == j + 1)
                                   || (anchor.Row1 == j - 1 && anchor.Row2 == j + 1)
                                   || (anchor.Row1 == j && anchor.Row2 == j + 2))
                                {
                                    find = true;
                                    picList.RemoveAt(k);
                                    break;
                                }
                            }

                            if (!find)
                            {
                                for (int k = picList.Count - 1; k > -1; k--)
                                {
                                    XSSFPicture picture = (XSSFPicture)picList[k];
                                    XSSFClientAnchor anchor = (XSSFClientAnchor)picture.GetPreferredSize();
                                    if (anchor.Row1 == j || anchor.Row2 == j + 1)
                                    {
                                        find = true;
                                        picList.RemoveAt(k);
                                        break;
                                    }
                                }
                            }

                            if (!find) { return "页签：" + sheet.SheetName + "，第" + "列[圖片]不存在或位置超出范围"; }

                            #endregion
                        }
                    }
                }
                catch (Exception exp)
                {
                    return exp.Message;
                }

                #endregion

                #region 新增

                SqlTransaction transaction = conn.BeginTransaction();
                try
                {
                    for (int i = 0; i < workbook.NumberOfSheets; i++)
                    {
                        sheet = workbook.GetSheetAt(i);
                        drawing = (XSSFDrawing)sheet.CreateDrawingPatriarch();
                        picList = drawing.GetShapes();

                        int j = 0;
                        while (true)
                        {
                            row = sheet.GetRow(++j);
                            if (row == null) { break; }

                            #region 名稱

                            try
                            {
                                if ((cell = row.GetCell(1)) == null) { break; }
                                name = GetCellValue(cell).Replace("\r", "").Replace("\n", "");
                                if (name.Length == 0) { break; }
                            }
                            catch
                            {
                                break;
                            }

                            #endregion

                            #region 分類

                            catalog = GetCellValue(row.GetCell(2));
                            catalogID = categoryMap[catalog];

                            #endregion

                            #region 點數

                            price = int.Parse(GetCellValue(row.GetCell(3)).Replace("点", "").Replace("豆", ""));

                            #endregion

                            #region 圖檔

                            imageName = catalog + "_" + name + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
                            if (File.Exists(imageDir + imageName))
                            {
                                Thread.Sleep(1000);
                                imageName = catalog + "_" + name + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
                            }

                            bool find = false;
                            for (int k = picList.Count - 1; k > -1; k--)
                            {
                                XSSFPicture picture = (XSSFPicture)picList[k];
                                XSSFClientAnchor anchor = (XSSFClientAnchor)picture.GetPreferredSize();
                                if ((anchor.Row1 == j && anchor.Row2 == j + 1)
                                   || (anchor.Row1 == j - 1 && anchor.Row2 == j + 1)
                                   || (anchor.Row1 == j && anchor.Row2 == j + 2))
                                {
                                    byte[] data = picture.PictureData.Data;
                                    BinaryWriter writer = new BinaryWriter(File.OpenWrite(imageDir + imageName));
                                    writer.Write(data);
                                    writer.Flush();
                                    writer.Close();
                                    find = true;
                                    picList.RemoveAt(k);
                                    break;
                                }
                            }

                            if (!find)
                            {
                                for (int k = picList.Count - 1; k > -1; k--)
                                {
                                    XSSFPicture picture = (XSSFPicture)picList[k];
                                    XSSFClientAnchor anchor = (XSSFClientAnchor)picture.GetPreferredSize();
                                    if (anchor.Row1 == j || anchor.Row2 == j + 1)
                                    {
                                        byte[] data = picture.PictureData.Data;
                                        BinaryWriter writer = new BinaryWriter(File.OpenWrite(imageDir + imageName));
                                        writer.Write(data);
                                        writer.Flush();
                                        writer.Close();
                                        find = true;
                                        picList.RemoveAt(k);
                                        break;
                                    }
                                }
                            }

                            #endregion

                            #region 新增資料

                            using (SqlCommand cmd = new SqlCommand("INSERT INTO Product ( Name, Price, ProductCategoryID, ImageName ) VALUES ( @Name, @Price, @ProductCategoryID, @ImageName )", conn, transaction))
                            {
                                cmd.Parameters.Add(new SqlParameter("@Name", name));
                                cmd.Parameters.Add(new SqlParameter("@Price", price));
                                cmd.Parameters.Add(new SqlParameter("@ProductCategoryID", catalogID));
                                cmd.Parameters.Add(new SqlParameter("@ImageName", imageName));
                                cmd.ExecuteNonQuery();
                                count++;
                            }

                            #endregion
                        }
                    }
                    transaction.Commit();
                    return "成功汇入" + count + "笔数据";
                }
                catch (Exception exp)
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception ex)
                    {
                        return ex.Message + "\t" + exp.Message;
                    }
                    return exp.Message;
                }

                #endregion
            }
            catch (Exception exp)
            {
                return exp.Message;
            }
            finally
            {
                if (stream != null) { stream.Close(); }
            }
        }

        private static string GetCellValue(ICell cell)
        {
            string result;

            if (cell == null)
            {
                result = "";
            }
            else if (cell.CellType == CellType.Formula)
            {
                if (cell.CachedFormulaResultType == CellType.Numeric)
                {
                    result = cell.NumericCellValue.ToString();
                }
                else
                {
                    result = cell.RichStringCellValue.ToString().Trim();
                }
            }
            else
            {
                result = cell.ToString().Trim();
            }

            return result;
        }
    }
}