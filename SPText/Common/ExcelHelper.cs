using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.OleDb;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using Common.baseFn;
using Microsoft.Graph;
using Microsoft.Office.Interop.Excel;
using NLipsum.Core;
using System.Web;
using Microsoft.Office;
using Word = Microsoft.Office.Interop.Word;
using Excel = Microsoft.Office.Interop.Excel;
using PowerPoint = Microsoft.Office.Interop.PowerPoint;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using DataTable = System.Data.DataTable;
using System.Text;
using NPOI.HPSF;
using Common.Offices;
using System.Drawing;

namespace Common
{
    public partial class ExcelHelper
    {
        private string fileName = null; //文件名
        private IWorkbook workbook = null;
        private FileStream fs = null;
        private bool disposed;

        private const string CONST_CONNECTIONSTRING = "Provider=Microsoft.Jet.OLEDB.4.0;Extended Properties=Excel 8.0;data source={0}";

        public string ExcelFilePath { get; set; }

        public ExcelHelper()
        { }

        public ExcelHelper(string path)
        {
            if (!path.IsNull())
            {
                ExcelFilePath = path;
                this.fileName = path;
                disposed = false;
            }
        }

        public List<Cell> GetExcelValueList(string sheetName)
        {
            if (ExcelFilePath.IsNull())
            {
                throw new Exception("请指定Excel文件路径");
            }

            string conString = String.Format(CONST_CONNECTIONSTRING, ExcelFilePath);

            string sql = String.Format("select * from [{0}$]", sheetName);

            DataSet dataSet = new DataSet();
            OleDbDataAdapter adapter = new OleDbDataAdapter(sql, conString);
            adapter.Fill(dataSet);

            System.Data.DataTable table = dataSet.Tables[0];

            int rowNumber = 1;

            List<Cell> cellListResult = new List<Cell>();

            foreach (DataRow row in table.Rows)
            {
                foreach (DataColumn column in table.Columns)
                {
                    string columnName = column.ColumnName;
                    string columnValue = row[columnName].ToString();

                    Cell cell = new Cell()
                    {
                        RowNumber = rowNumber,
                        CellName = columnName,
                        CellValue = columnValue
                    };
                    cellListResult.Add(cell);
                }
                rowNumber += 1;
            }

            return cellListResult;
        }

        public void InsertExcelValueList(string sheetName, List<Cell> cellList)
        {
            const string script = "insert into [{0}$] ({1}) values ({2}) ";
            string sql = "";

            List<int> rowNumberList = cellList.Select(p => p.RowNumber).Distinct().ToList();

            foreach (int rowNumber in rowNumberList)
            {
                List<Cell> cellListForRow = cellList.Where(p => p.RowNumber == rowNumber).ToList();
                List<string> cellNameList = cellListForRow.Select(p => p.CellName).ToList();
                List<string> cellValueList = cellListForRow.Select(p => "'" + p.CellValue + "'").ToList();
                sql += String.Format(script, sheetName, String.Join(",", cellNameList), String.Join(",", cellValueList));
            }

            string conString = String.Format(CONST_CONNECTIONSTRING, ExcelFilePath);
            OleDbConnection con = new OleDbConnection(conString);
            OleDbCommand cmd = new OleDbCommand(sql, con);
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }

        }

        public class Cell
        {
            public int RowNumber { get; set; }

            public string CellName { get; set; }

            public string CellValue { get; set; }
        }

        #region 文件转PDF
        public bool ConvertToPDF(string sourcePath, string targetPath)
        {
            return true;
            //return ConvertToPDF(sourcePath, targetPath, XlFixedFormatType.xlTypePDF);
        }



        private bool ConvertToPDF(string sourcePath, string targetPath, XlFixedFormatType targetType)
        {
            //bool result;
            //object missing = Type.Missing;
            //Microsoft.Office.Interop.Excel.ApplicationClass application = null;
            //Microsoft.Graph.Workbook workBook = null;
            //try
            //{
            //    application = new Microsoft.Office.Interop.Excel.ApplicationClass();
            //    object target = targetPath;
            //    object type = targetType;
            //    workBook = (Microsoft.Graph.Workbook)application.Workbooks.Open(sourcePath, missing, missing, missing, missing, missing,
            //        missing, missing, missing, missing, missing, missing, missing, missing, missing);

            //    workBook.ExportAsFixedFormat(targetType, target, XlFixedFormatQuality.xlQualityStandard, true, false, missing, missing, missing, missing);
            //    result = true;
            //    return result;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            //finally
            //{
            //    if (workBook != null)
            //    {
            //        workBook.Close(true, missing, missing);
            //        workBook = null;
            //    }
            //    if (application != null)
            //    {
            //        application.Quit();
            //        application = null;
            //    }
            //    GC.Collect();
            //    GC.WaitForPendingFinalizers();
            //    GC.Collect();
            //    GC.WaitForPendingFinalizers();
            //}
            return true;
        }

        /// <summary>
        /// DOC 2 PDF
        /// </summary>
        /// <param name="sourcePath">源路径</param>
        /// <param name="targetPath">目标路径</param>
        /// <returns></returns>
        public bool DOCToPDF(string sourcePath, string targetPath)
        {
            bool result = false;
            Word.WdExportFormat exportFormat = Word.WdExportFormat.wdExportFormatPDF;
            object paramMissing = Type.Missing;
            Word.ApplicationClass wordApplication = new Word.ApplicationClass();
            Word.Document wordDocument = null;
            try
            {
                object paramSourceDocPath = sourcePath;
                string paramExportFilePath = targetPath;

                Word.WdExportFormat paramExportFormat = exportFormat;
                bool paramOpenAfterExport = false;
                Word.WdExportOptimizeFor paramExportOptimizeFor = Word.WdExportOptimizeFor.wdExportOptimizeForPrint;
                Word.WdExportRange paramExportRange = Word.WdExportRange.wdExportAllDocument;
                int paramStartPage = 0;
                int paramEndPage = 0;
                Word.WdExportItem paramExportItem = Word.WdExportItem.wdExportDocumentContent;
                bool paramIncludeDocProps = true;
                bool paramKeepIRM = true;
                Word.WdExportCreateBookmarks paramCreateBookmarks = Word.WdExportCreateBookmarks.wdExportCreateWordBookmarks;
                bool paramDocStructureTags = true;
                bool paramBitmapMissingFonts = true;
                bool paramUseISO19005_1 = false;

                wordDocument = wordApplication.Documents.Open(
                ref paramSourceDocPath, ref paramMissing, ref paramMissing,
                ref paramMissing, ref paramMissing, ref paramMissing,
                ref paramMissing, ref paramMissing, ref paramMissing,
                ref paramMissing, ref paramMissing, ref paramMissing,
                ref paramMissing, ref paramMissing, ref paramMissing,
                ref paramMissing);

                if (wordDocument != null)
                    wordDocument.ExportAsFixedFormat(paramExportFilePath,
                    paramExportFormat, paramOpenAfterExport,
                    paramExportOptimizeFor, paramExportRange, paramStartPage,
                    paramEndPage, paramExportItem, paramIncludeDocProps,
                    paramKeepIRM, paramCreateBookmarks, paramDocStructureTags,
                    paramBitmapMissingFonts, paramUseISO19005_1,
                    ref paramMissing);
                result = true;
            }
            catch
            {
                result = false;
            }
            finally
            {
                if (wordDocument != null)
                {
                    wordDocument.Close(ref paramMissing, ref paramMissing, ref paramMissing);
                    wordDocument = null;
                }
                if (wordApplication != null)
                {
                    wordApplication.Quit(ref paramMissing, ref paramMissing, ref paramMissing);
                    wordApplication = null;
                }
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            return result;
        }
        /// <summary>
        /// XLS 2 PDF
        /// </summary>
        /// <param name="sourcePath">源路径</param>
        /// <param name="targetPath">目标路径</param>
        /// <returns></returns>
        public bool XLSToPDF(string sourcePath, string targetPath)
        {
            bool result = false;
            Microsoft.Office.Interop.Excel.XlFixedFormatType targetType = Microsoft.Office.Interop.Excel.XlFixedFormatType.xlTypePDF;
            object missing = Type.Missing;
            Microsoft.Office.Interop.Excel.ApplicationClass application = null;
            Microsoft.Office.Interop.Excel.Workbook workBook = null;
            try
            {
                application = new Microsoft.Office.Interop.Excel.ApplicationClass();
                object target = targetPath;
                object type = targetType;
                workBook = application.Workbooks.Open(sourcePath, missing, missing, missing, missing, missing,
                        missing, missing, missing, missing, missing, missing, missing, missing, missing);

                workBook.ExportAsFixedFormat(targetType, target, Microsoft.Office.Interop.Excel.XlFixedFormatQuality.xlQualityStandard, true, false, missing, missing, missing, missing);
                result = true;
            }
            catch
            {
                result = false;
            }
            finally
            {
                if (workBook != null)
                {
                    workBook.Close(true, missing, missing);
                    workBook = null;
                }
                if (application != null)
                {
                    application.Quit();
                    application = null;
                }
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            return result;
        }
        /// <summary>
        /// PPT 2 PDF
        /// </summary>
        /// <param name="sourcePath">源路径</param>
        /// <param name="targetPath">目标路径</param>
        /// <returns></returns>
        public bool PPTToPDF(string sourcePath, string targetPath)
        {
            bool result;
            PowerPoint.PpSaveAsFileType targetFileType = PowerPoint.PpSaveAsFileType.ppSaveAsPDF;
            object missing = Type.Missing;
            PowerPoint.ApplicationClass application = null;
            PowerPoint.Presentation persentation = null;
            try
            {
                //application = new PowerPoint.ApplicationClass();
                //persentation = application.Presentations.Open(sourcePath, MsoTriState.msoTrue, MsoTriState.msoFalse, MsoTriState.msoFalse);
                //persentation.SaveAs(targetPath, targetFileType, Microsoft.Office.Core.MsoTriState.msoTrue);

                result = true;
            }
            catch
            {
                result = false;
            }
            finally
            {
                if (persentation != null)
                {
                    persentation.Close();
                    persentation = null;
                }
                if (application != null)
                {
                    application.Quit();
                    application = null;
                }
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            return result;
        }

        #endregion


        /// <summary>
        /// 将DataTable数据导入到excel中
        /// </summary>
        /// <param name="data">要导入的数据</param>
        /// <param name="isColumnWritten">DataTable的列名是否要导入</param>
        /// <param name="sheetName">要导入的excel的sheet的名称</param>
        /// <returns>导入数据行数(包含列名那一行)</returns>
        public int DataTableToExcel(System.Data.DataTable data, string sheetName, bool isColumnWritten)
        {
            int i = 0;
            int j = 0;
            int count = 0;
            ISheet sheet = null;

            fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            if (fileName.IndexOf(".xlsx") > 0) // 2007版本
                workbook = new XSSFWorkbook();
            else if (fileName.IndexOf(".xls") > 0) // 2003版本
                workbook = new HSSFWorkbook();

            try
            {
                if (workbook != null)
                {
                    sheet = workbook.CreateSheet(sheetName);
                }
                else
                {
                    return -1;
                }

                if (isColumnWritten == true) //写入DataTable的列名
                {
                    IRow row = sheet.CreateRow(0);
                    for (j = 0; j < data.Columns.Count; ++j)
                    {
                        row.CreateCell(j).SetCellValue(data.Columns[j].ColumnName);
                    }
                    count = 1;
                }
                else
                {
                    count = 0;
                }

                for (i = 0; i < data.Rows.Count; ++i)
                {
                    IRow row = sheet.CreateRow(count);
                    for (j = 0; j < data.Columns.Count; ++j)
                    {
                        row.CreateCell(j).SetCellValue(data.Rows[i][j].ToString());
                    }
                    ++count;
                }
                workbook.Write(fs); //写入到excel
                return count;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 将excel中的数据导入到DataTable中
        /// </summary>
        /// <param name="sheetName">excel工作薄sheet的名称</param>
        /// <param name="isFirstRowColumn">第一行是否是DataTable的列名</param>
        /// <returns>返回的DataTable</returns>
        public System.Data.DataTable ExcelToDataTable(string sheetName, bool isFirstRowColumn)
        {
            ISheet sheet = null;
            System.Data.DataTable data = new System.Data.DataTable();
            int startRow = 0;
            try
            {
                fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                if (fileName.IndexOf(".xlsx") > 0) // 2007版本
                    workbook = new XSSFWorkbook(fs);
                else if (fileName.IndexOf(".xls") > 0) // 2003版本
                    workbook = new HSSFWorkbook(fs);

                if (sheetName != null)
                {
                    sheet = workbook.GetSheet(sheetName);
                    if (sheet == null) //如果没有找到指定的sheetName对应的sheet，则尝试获取第一个sheet
                    {
                        sheet = workbook.GetSheetAt(0);
                    }
                }
                else
                {
                    sheet = workbook.GetSheetAt(0);
                }
                if (sheet != null)
                {
                    IRow firstRow = sheet.GetRow(0);
                    int cellCount = firstRow.LastCellNum; //一行最后一个cell的编号 即总的列数

                    if (isFirstRowColumn)
                    {
                        for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                        {
                            ICell cell = firstRow.GetCell(i);
                            if (cell != null)
                            {
                                string cellValue = cell.StringCellValue;
                                if (cellValue != null)
                                {
                                    DataColumn column = new DataColumn(cellValue);
                                    data.Columns.Add(column);
                                }
                            }
                        }
                        startRow = sheet.FirstRowNum + 1;
                    }
                    else
                    {
                        startRow = sheet.FirstRowNum;
                    }

                    //最后一列的标号
                    int rowCount = sheet.LastRowNum;
                    for (int i = startRow; i <= rowCount; ++i)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue; //没有数据的行默认是null　　　　　　　

                        DataRow dataRow = data.NewRow();
                        for (int j = row.FirstCellNum; j < cellCount; ++j)
                        {
                            if (row.GetCell(j) != null) //同理，没有数据的单元格都默认是null
                                dataRow[j] = row.GetCell(j).ToString();
                        }
                        data.Rows.Add(dataRow);
                    }
                }

                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return null;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    if (fs != null)
                        fs.Close();
                }

                fs = null;
                disposed = true;
            }
        }


        public static void ExcelToDataTable(string opnFileName, string dbName, ref DataSet dataSet, string dstablename)
        {
            double excelvr = GetExcelVerStr();
            string strConn;
            //HDR=YES表示第一行是表头，当作列名来使用。但是在这次的表格订单文件中不影响，所以都可以
            if (excelvr < 12.0)
            {
                strConn = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR=Yes;IMEX=1;'", opnFileName);
            }
            else if (excelvr == 12.0)
            {
                strConn = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 8.0;HDR=Yes;IMEX=1;'", opnFileName);
            }
            else
            {
                strConn = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 12.0;HDR=Yes;IMEX=1;'", opnFileName);
            }
            OleDbConnection conn = new OleDbConnection(strConn);
            conn.Open();
            System.Data.DataTable schemaTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[]
            {
                null,
                null,
                null,
                "TABLE"
            });
            foreach (DataRow row in schemaTable.Rows)
            {
                dbName = ((string)row["TABLE_NAME"]).Trim();
            }
            string strExcel = "select * from [" + dbName + "]";
            OleDbDataAdapter myCommand = new OleDbDataAdapter(strExcel, strConn);
            myCommand.Fill(dataSet, dstablename);
            conn.Close();
            conn.Dispose();
        }

        /// <summary>
        /// 返回主机的Excel版本
        /// </summary>
        /// <returns></returns>
        private static double GetExcelVerStr()
        {
            Type objExcelType = Type.GetTypeFromProgID("Excel.Application");
            double result;
            if (objExcelType == null)
            {
                result = 0.0;
            }
            else
            {
                object objApp = Activator.CreateInstance(objExcelType);
                if (objApp == null)
                {
                    result = 0.0;
                }
                else
                {
                    object objVer = objApp.GetType().InvokeMember("Version", System.Reflection.BindingFlags.GetProperty, null, objApp, null);
                    double iVer = Convert.ToDouble(objVer.ToString());
                    GC.Collect();
                    result = iVer;
                }
            }
            return result;
        }

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowThreadProcessId(IntPtr hwnd, out int pid);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="file_name"></param>
        /// <param name="sheet_name"></param>
        public static void DataTableToExcel(System.Data.DataTable dt, string file_name, string sheet_name)
        {
            Microsoft.Office.Interop.Excel.Application Myxls = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook Mywkb = Myxls.Workbooks.Add();
            Microsoft.Office.Interop.Excel.Worksheet MySht = (Worksheet)Mywkb.ActiveSheet;
            MySht.Name = sheet_name;
            Myxls.Visible = false;
            Myxls.DisplayAlerts = false;
            try
            {
                //写入表头
                object[] arrHeader = new object[dt.Columns.Count];
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    arrHeader[i] = dt.Columns[i].ColumnName;
                }
                MySht.Range[MySht.Cells[1, 1], MySht.Cells[1, dt.Columns.Count]].Value2 = arrHeader;
                //写入表体数据
                object[,] arrBody = new object[dt.Rows.Count, dt.Columns.Count];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        arrBody[i, j] = dt.Rows[i][j].ToString();
                    }
                }
                MySht.Range[MySht.Cells[2, 1], MySht.Cells[dt.Rows.Count + 1, dt.Columns.Count]].Value2 = arrBody;
                if (Mywkb != null)
                {
                    Mywkb.SaveAs(file_name);
                    Mywkb.Close(Type.Missing, Type.Missing, Type.Missing);
                    Mywkb = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "系统提示");
            }
            finally
            {
                //彻底关闭Excel进程
                if (Myxls != null)
                {
                    Myxls.Quit();
                    try
                    {
                        if (Myxls != null)
                        {
                            int pid;
                            GetWindowThreadProcessId(new IntPtr(Myxls.Hwnd), out pid);
                            System.Diagnostics.Process p = System.Diagnostics.Process.GetProcessById(pid);
                            p.Kill();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("结束当前EXCEL进程失败：" + ex.Message);
                    }
                    Myxls = null;
                }
                GC.Collect();
            }
        }

        //使用Aspose.Cells.dll, 可以不依靠Microsoft Excel也能灵活读写数据, 提供等同与Excel的功能
        //避免实际实施过程中office版本问题造成过多的Bug
        //且能读取csv格式
        /// <summary>
        /// 使用Aspose.Cells.dll, 可以不依靠Microsoft Excel也能灵活读写数据, 提供等同与Excel的功能
        /// </summary>
        /// <param name="fileFullPath"></param>
        /// <param name="HDR">表格内是否包含列名</param>
        /// <returns></returns>
        public static System.Data.DataTable ExcelToDataTableByAspose(string fileFullPath, bool HDR)
        {
            Aspose.Cells.Workbook workbook = new Aspose.Cells.Workbook(fileFullPath);
            Aspose.Cells.Worksheet worksheet = workbook.Worksheets[0];
            Aspose.Cells.Cells cells = worksheet.Cells;

            //该方法得到的表格中，如果存在单元格内容第一个字符为0的纯数字字符串会忽略0
            System.Data.DataTable dataTable = cells.ExportDataTable(0, 0, cells.MaxDataRow + 1, cells.MaxDataColumn + 1, HDR);

            //该方法得到的表格中，如果存在单元格内容为10个以上的纯数字会将其变成科学记数法
            //DataTable dataTable2= cells.ExportDataTableAsString(0, 0, cells.MaxDataRow + 1, cells.MaxDataColumn + 1, HDR);
            workbook = null;
            worksheet = null;
            cells = null;
            GC.Collect();
            return dataTable;
        }

        public static System.Data.DataTable ExcelToDataTableByNpoi(string fileName, bool isFirstRowColumn, string customerName = null)
        {
            string sheetName = "sheet";
            ISheet sheet = null;
            System.Data.DataTable data = new System.Data.DataTable();
            int startRow = 0;
            try
            {
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                IWorkbook workbook = null;

                if (customerName != null)
                {
                    if (customerName == "MotionGlobal")
                    {
                        workbook = new XSSFWorkbook(fs);
                    }
                    else
                    {
                        if (Path.GetExtension(fileName).ToLower() == ".xlsx") // 2007版本
                            workbook = new XSSFWorkbook(fs);
                        else if (Path.GetExtension(fileName).ToLower() == ".xls") // 2003版本
                            workbook = new HSSFWorkbook(fs);
                    }
                }
                else
                {
                    if (Path.GetExtension(fileName).ToLower() == ".xlsx") // 2007版本
                        workbook = new XSSFWorkbook(fs);
                    else if (Path.GetExtension(fileName).ToLower() == ".xls") // 2003版本
                        workbook = new HSSFWorkbook(fs);
                }

                if (sheetName != null)
                {
                    sheet = workbook.GetSheet(sheetName);
                    if (sheet == null) //如果没有找到指定的sheetName对应的sheet，则尝试获取第一个sheet
                    {
                        sheet = workbook.GetSheetAt(0);
                    }
                }
                else
                {
                    sheet = workbook.GetSheetAt(0);
                }
                if (sheet != null)
                {
                    IRow firstRow = sheet.GetRow(0);
                    int cellCount = firstRow.LastCellNum; //一行最后一个cell的编号 即总的列数

                    if (isFirstRowColumn)
                    {
                        for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                        {
                            ICell cell = firstRow.GetCell(i);
                            if (cell != null)
                            {
                                string cellValue = cell.StringCellValue;
                                if (cellValue != null)
                                {
                                    DataColumn column = new DataColumn(cellValue);
                                    data.Columns.Add(column);
                                }
                            }
                        }
                        startRow = sheet.FirstRowNum + 1;
                    }
                    else
                    {
                        startRow = sheet.FirstRowNum;
                    }

                    //最后一列的标号
                    int rowCount = sheet.LastRowNum;
                    for (int i = startRow; i <= rowCount; ++i)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue; //没有数据的行默认是null　　　　　　　

                        DataRow dataRow = data.NewRow();
                        for (int j = row.FirstCellNum; j < cellCount; ++j)
                        {
                            if (row.GetCell(j) != null) //同理，没有数据的单元格都默认是null
                                dataRow[j] = row.GetCell(j).ToString();
                        }
                        data.Rows.Add(dataRow);
                    }
                }
                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                throw;
            }
        }

        #region NOPI DataTable2Excel
        public static void ExportExcel(System.Data.DataTable table, string path)
        {
            byte[] data = DataTable2Excel(table, "VTable");
            if (!System.IO.File.Exists(path))
            {
                FileStream fs = new FileStream(path, FileMode.CreateNew);
                fs.Write(data, 0, data.Length);
                fs.Close();
                fs.Dispose();
            }
        }

        /// <summary>
        /// 将DataTable转换为excel2003格式。
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static byte[] DataTable2Excel(System.Data.DataTable dt, string sheetName)
        {
            int EXCEL03_MaxRow = 65535;

            IWorkbook book = new HSSFWorkbook();
            if (dt.Rows.Count < EXCEL03_MaxRow)
                DataWrite2Sheet(dt, 0, dt.Rows.Count - 1, book, sheetName);
            else
            {
                int page = dt.Rows.Count / EXCEL03_MaxRow;
                for (int i = 0; i < page; i++)
                {
                    int start = i * EXCEL03_MaxRow;
                    int end = (i * EXCEL03_MaxRow) + EXCEL03_MaxRow - 1;
                    DataWrite2Sheet(dt, start, end, book, sheetName + i.ToString());
                }
                int lastPageItemCount = dt.Rows.Count % EXCEL03_MaxRow;
                DataWrite2Sheet(dt, dt.Rows.Count - lastPageItemCount, lastPageItemCount, book, sheetName + page.ToString());
            }
            MemoryStream ms = new MemoryStream();
            book.Write(ms);
            return ms.ToArray();
        }

        /// <summary>
        /// 将DataTable转换为excel2003格式。
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static MemoryStream DataTable2ExcelRetrunMemoryStream(System.Data.DataTable dt, string sheetName)
        {
            int EXCEL03_MaxRow = 65535;

            IWorkbook book = new HSSFWorkbook();
            if (dt.Rows.Count < EXCEL03_MaxRow)
                DataWrite2Sheet(dt, 0, dt.Rows.Count - 1, book, sheetName);
            else
            {
                int page = dt.Rows.Count / EXCEL03_MaxRow;
                for (int i = 0; i < page; i++)
                {
                    int start = i * EXCEL03_MaxRow;
                    int end = (i * EXCEL03_MaxRow) + EXCEL03_MaxRow - 1;
                    DataWrite2Sheet(dt, start, end, book, sheetName + i.ToString());
                }
                int lastPageItemCount = dt.Rows.Count % EXCEL03_MaxRow;
                DataWrite2Sheet(dt, dt.Rows.Count - lastPageItemCount, lastPageItemCount, book, sheetName + page.ToString());
            }
            MemoryStream ms = new MemoryStream();
            book.Write(ms);
            return ms;
        }

        private static void DataWrite2Sheet(System.Data.DataTable dt, int startRow, int endRow, IWorkbook book, string sheetName)
        {
            ISheet sheet = book.CreateSheet(sheetName);
            IRow header = sheet.CreateRow(0);
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                ICell cell = header.CreateCell(i);
                string val = dt.Columns[i].Caption ?? dt.Columns[i].ColumnName;
                cell.SetCellValue(val);
            }
            int rowIndex = 1;
            for (int i = startRow; i <= endRow; i++)
            {
                DataRow dtRow = dt.Rows[i];
                IRow excelRow = sheet.CreateRow(rowIndex++);
                for (int j = 0; j < dtRow.ItemArray.Length; j++)
                {
                    excelRow.CreateCell(j).SetCellValue(dtRow[j].ToString());
                }
            }

        }
        #endregion

        /// <summary>
        /// 将excel中的数据导入到DataTable中
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="sheetName">excel工作薄sheet的名称</param>
        /// <param name="isFirstRowColumn">第一行是否是DataTable的列名</param>
        /// <returns>返回的DataTable</returns>
        public static System.Data.DataTable ExcelToDataTable(string filePath, string sheetName = null, bool isFirstRowColumn = true)
        {
            ISheet sheet = null;
            System.Data.DataTable data = new System.Data.DataTable();
            IWorkbook workbook = null;
            FileStream fs = null;
            int startRow = 0;
            try
            {
                fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                if (filePath.IndexOf(".xlsx") > 0) // 2007版本
                    workbook = new XSSFWorkbook(fs);
                else if (filePath.IndexOf(".xls") > 0) // 2003版本
                    workbook = new HSSFWorkbook(fs);

                if (sheetName != null)
                {
                    sheet = workbook.GetSheet(sheetName);
                    if (sheet == null) //如果没有找到指定的sheetName对应的sheet，则尝试获取第一个sheet
                    {
                        sheet = workbook.GetSheetAt(0);
                    }
                }
                else
                {
                    sheet = workbook.GetSheetAt(0);
                }
                if (sheet != null)
                {
                    IRow firstRow = sheet.GetRow(0);
                    int cellCount = firstRow.LastCellNum; //一行最后一个cell的编号 即总的列数

                    if (isFirstRowColumn)
                    {
                        for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                        {
                            ICell cell = firstRow.GetCell(i);
                            if (cell != null)
                            {
                                string cellValue = cell.StringCellValue;
                                if (cellValue != null)
                                {
                                    DataColumn column = new DataColumn(cellValue);
                                    data.Columns.Add(column);
                                }
                            }
                        }
                        startRow = sheet.FirstRowNum + 1;
                    }
                    else
                    {
                        startRow = sheet.FirstRowNum;
                    }

                    //最后一列的标号
                    int rowCount = sheet.LastRowNum;
                    for (int i = startRow; i <= rowCount; ++i)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue; //没有数据的行默认是null　　　　　　　

                        DataRow dataRow = data.NewRow();
                        for (int j = row.FirstCellNum; j < cellCount; ++j)
                        {
                            if (row.GetCell(j) != null) //同理，没有数据的单元格都默认是null
                                dataRow[j] = row.GetCell(j).ToString();
                        }
                        data.Rows.Add(dataRow);
                    }
                }

                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return null;
            }
        }
    }

    /// <summary>
    ///描 述：NPOI Excel DataTable操作类
    public partial class ExcelHelper
    {
        #region 新增的方法

        /// <summary>
        /// Excel导出下载（只需DataTable）
        /// </summary>
        /// <param name="dtSource">DataTable数据源</param>
        /// <param name="excelConfig">导出设置包含文件名、标题、列设置</param>
        public static void ExcelDownloadOnlyDT(DataTable dtSource, string strHeaderText, string fileName)
        {
            HttpContext curContext = HttpContext.Current;
            // 设置编码和附件格式
            curContext.Response.ContentType = "application/ms-excel";
            curContext.Response.ContentEncoding = Encoding.UTF8;
            curContext.Response.Charset = "";
            curContext.Response.AppendHeader("Content-Disposition",
                "attachment;filename=" + HttpUtility.UrlEncode(fileName, Encoding.UTF8));
            //调用导出具体方法Export()
            curContext.Response.BinaryWrite(ExportDT(dtSource, strHeaderText).GetBuffer());
            curContext.Response.End();
        }

        /// <summary>
        /// DataTable导出到Excel的MemoryStream
        /// </summary>
        /// <param name="dtSource">源DataTable</param>
        /// <param name="strHeaderText">表头文本</param>
        private static MemoryStream ExportDT(DataTable dtSource, string strHeaderText)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet();

            #region 右击文件 属性信息
            {
                DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
                dsi.Company = "NPOI";
                workbook.DocumentSummaryInformation = dsi;

                SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
                si.Author = "文件作者信息"; //填加xls文件作者信息
                si.ApplicationName = "创建程序信息"; //填加xls文件创建程序信息
                si.LastAuthor = "最后保存者信息"; //填加xls文件最后保存者信息
                si.Comments = "作者信息"; //填加xls文件作者信息
                si.Title = "标题信息"; //填加xls文件标题信息
                si.Subject = "主题信息";//填加文件主题信息
                si.CreateDateTime = DateTime.Now;
                workbook.SummaryInformation = si;
            }
            #endregion

            HSSFCellStyle dateStyle = (HSSFCellStyle)workbook.CreateCellStyle();
            HSSFDataFormat format = (HSSFDataFormat)workbook.CreateDataFormat();
            dateStyle.DataFormat = format.GetFormat("yyyy-mm-dd");

            //取得列宽
            int[] arrColWidth = new int[dtSource.Columns.Count];
            foreach (DataColumn item in dtSource.Columns)
            {
                arrColWidth[item.Ordinal] = Encoding.GetEncoding(936).GetBytes(item.ColumnName.ToString()).Length;
            }
            for (int i = 0; i < dtSource.Rows.Count; i++)
            {
                for (int j = 0; j < dtSource.Columns.Count; j++)
                {
                    if (dtSource.Rows[i][j] == null)
                    {
                        continue;
                    }
                    int intTemp = Encoding.GetEncoding(936).GetBytes(dtSource.Rows[i][j].ToString()).Length;
                    if (intTemp > arrColWidth[j])
                    {
                        arrColWidth[j] = intTemp;
                    }
                }
            }
            int rowIndex = 0;
            foreach (DataRow row in dtSource.Rows)
            {
                #region 新建表，填充表头，填充列头，样式
                if (rowIndex == 65535 || rowIndex == 0)
                {
                    if (rowIndex != 0)
                    {
                        sheet = (HSSFSheet)workbook.CreateSheet();
                    }

                    #region 表头及样式
                    {
                        HSSFRow headerRow = (HSSFRow)sheet.CreateRow(0);
                        headerRow.HeightInPoints = 25;
                        headerRow.CreateCell(0).SetCellValue(strHeaderText);

                        HSSFCellStyle headStyle = (HSSFCellStyle)workbook.CreateCellStyle();
                        headStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                        HSSFFont font = (HSSFFont)workbook.CreateFont();
                        font.FontHeightInPoints = 20;
                        font.Boldweight = 700;
                        headStyle.SetFont(font);
                        headerRow.GetCell(0).CellStyle = headStyle;
                        sheet.AddMergedRegion(new NPOI.SS.Util.Region(0, 0, 0, dtSource.Columns.Count - 1));
                        //headerRow.Dispose();
                    }
                    #endregion


                    #region 列头及样式
                    {
                        HSSFRow headerRow = (HSSFRow)sheet.CreateRow(1);
                        HSSFCellStyle headStyle = (HSSFCellStyle)workbook.CreateCellStyle();
                        headStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                        HSSFFont font = (HSSFFont)workbook.CreateFont();
                        font.FontHeightInPoints = 10;
                        font.Boldweight = 700;
                        headStyle.SetFont(font);
                        foreach (DataColumn column in dtSource.Columns)
                        {
                            headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                            headerRow.GetCell(column.Ordinal).CellStyle = headStyle;

                            if ((arrColWidth[column.Ordinal] + 1) * 256 >= 10000)
                            {
                                sheet.SetColumnWidth(column.Ordinal, 10000);
                            }
                            else
                            {
                                //设置列宽
                                sheet.SetColumnWidth(column.Ordinal, (arrColWidth[column.Ordinal] + 1) * 256);
                            }
                        }
                        //headerRow.Dispose();
                    }
                    #endregion

                    rowIndex = 2;
                }
                #endregion


                #region 填充内容
                HSSFRow dataRow = (HSSFRow)sheet.CreateRow(rowIndex);
                foreach (DataColumn column in dtSource.Columns)
                {
                    HSSFCell newCell = (HSSFCell)dataRow.CreateCell(column.Ordinal);

                    string drValue = row[column].ToString();

                    switch (column.DataType.ToString())
                    {
                        case "System.String"://字符串类型
                            newCell.SetCellValue(drValue);
                            break;
                        case "System.DateTime"://日期类型
                            DateTime dateV;
                            DateTime.TryParse(drValue, out dateV);
                            newCell.SetCellValue(dateV);

                            newCell.CellStyle = dateStyle;//格式化显示
                            break;
                        case "System.Boolean"://布尔型
                            bool boolV = false;
                            bool.TryParse(drValue, out boolV);
                            newCell.SetCellValue(boolV);
                            break;
                        case "System.Int16"://整型
                        case "System.Int32":
                        case "System.Int64":
                        case "System.Byte":
                            int intV = 0;
                            int.TryParse(drValue, out intV);
                            newCell.SetCellValue(intV);
                            break;
                        case "System.Decimal"://浮点型
                        case "System.Double":
                            double doubV = 0;
                            double.TryParse(drValue, out doubV);
                            newCell.SetCellValue(doubV);
                            break;
                        case "System.DBNull"://空值处理
                            newCell.SetCellValue("");
                            break;
                        default:
                            newCell.SetCellValue("");
                            break;
                    }

                }
                #endregion

                rowIndex++;
            }
            MemoryStream ms = new MemoryStream();

            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;
            return ms;
        }

        #endregion

        #region Excel导出方法 ExcelDownload
        /// <summary>
        /// Excel导出下载
        /// </summary>
        /// <param name="dtSource">DataTable数据源</param>
        /// <param name="excelConfig">导出设置包含文件名、标题、列设置</param>
        public static void ExcelDownload(DataTable dtSource, ExcelConfig excelConfig)
        {
            HttpContext curContext = HttpContext.Current;
            // 设置编码和附件格式
            curContext.Response.ContentType = "application/ms-excel";
            curContext.Response.ContentEncoding = Encoding.UTF8;
            curContext.Response.Charset = "";
            curContext.Response.AppendHeader("Content-Disposition",
                "attachment;filename=" + HttpUtility.UrlEncode(excelConfig.FileName, Encoding.UTF8));
            //调用导出具体方法Export()
            curContext.Response.BinaryWrite(ExportMemoryStream(dtSource, excelConfig).GetBuffer());
            curContext.Response.End();
        }
        /// <summary>
        /// Excel导出下载
        /// </summary>
        /// <param name="list">数据源</param>
        /// <param name="templdateName">模板文件名</param>
        /// <param name="newFileName">文件名</param>
        public static void ExcelDownload(List<TemplateMode> list, string templdateName, string newFileName)
        {
            HttpResponse response = System.Web.HttpContext.Current.Response;
            response.Clear();
            response.Charset = "UTF-8";
            response.ContentType = "application/vnd-excel";//"application/vnd.ms-excel";
            System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", string.Format("attachment; filename=" + newFileName));
            System.Web.HttpContext.Current.Response.BinaryWrite(ExportListByTempale(list, templdateName).ToArray());
        }
        #endregion

        #region DataTable导出到Excel文件excelConfig中FileName设置为全路径
        /// <summary>
        /// DataTable导出到Excel文件 Export()
        /// </summary>
        /// <param name="dtSource">DataTable数据源</param>
        /// <param name="excelConfig">导出设置包含文件名、标题、列设置</param>
        public static void ExcelExport(DataTable dtSource, ExcelConfig excelConfig)
        {
            using (MemoryStream ms = ExportMemoryStream(dtSource, excelConfig))
            {
                using (FileStream fs = new FileStream(excelConfig.FileName, FileMode.Create, FileAccess.Write))
                {
                    byte[] data = ms.ToArray();
                    fs.Write(data, 0, data.Length);
                    fs.Flush();
                }
            }
        }
        #endregion

        #region DataTable导出到Excel的MemoryStream
        /// <summary>
        /// DataTable导出到Excel的MemoryStream Export()
        /// </summary>
        /// <param name="dtSource">DataTable数据源</param>
        /// <param name="excelConfig">导出设置包含文件名、标题、列设置</param>
        public static MemoryStream ExportMemoryStream(DataTable dtSource, ExcelConfig excelConfig)
        {
            int colint = 0;
            for (int i = 0; i < dtSource.Columns.Count;)
            {
                DataColumn column = dtSource.Columns[i];
                if (excelConfig.ColumnEntity[colint].Column != column.ColumnName)
                {
                    dtSource.Columns.Remove(column.ColumnName);
                }
                else
                {
                    i++;
                    if (colint < excelConfig.ColumnEntity.Count - 1)
                    {
                        colint++;
                    }
                }
            }

            HSSFWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet();
            #region 右击文件 属性信息
            {
                DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
                dsi.Company = "NPOI";
                workbook.DocumentSummaryInformation = dsi;

                SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
                si.Author = "刘晓雷"; //填加xls文件作者信息
                si.ApplicationName = "信息"; //填加xls文件创建程序信息
                si.LastAuthor = "刘晓雷"; //填加xls文件最后保存者信息
                si.Comments = "刘晓雷"; //填加xls文件作者信息
                si.Title = "标题信息"; //填加xls文件标题信息
                si.Subject = "主题信息";//填加文件主题信息
                si.CreateDateTime = System.DateTime.Now;
                workbook.SummaryInformation = si;
            }
            #endregion

            #region 设置标题样式
            ICellStyle headStyle = workbook.CreateCellStyle();
            int[] arrColWidth = new int[dtSource.Columns.Count];
            string[] arrColName = new string[dtSource.Columns.Count];//列名
            ICellStyle[] arryColumStyle = new ICellStyle[dtSource.Columns.Count];//样式表
            headStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center; // ------------------
            if (excelConfig.Background != new System.Drawing.Color())
            {
                if (excelConfig.Background != new System.Drawing.Color())
                {
                    headStyle.FillPattern = FillPattern.SolidForeground;
                    headStyle.FillForegroundColor = GetXLColour(workbook, excelConfig.Background);
                }
            }
            NPOI.SS.UserModel.IFont font = workbook.CreateFont();
            font.FontHeightInPoints = excelConfig.TitlePoint;
            if (excelConfig.ForeColor != new System.Drawing.Color())
            {
                font.Color = GetXLColour(workbook, excelConfig.ForeColor);
            }
            font.Boldweight = 700;
            headStyle.SetFont(font);
            #endregion

            #region 列头及样式
            ICellStyle cHeadStyle = workbook.CreateCellStyle();
            cHeadStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center; // ------------------
            NPOI.SS.UserModel.IFont cfont = workbook.CreateFont();
            cfont.FontHeightInPoints = excelConfig.HeadPoint;
            cHeadStyle.SetFont(cfont);
            #endregion

            #region 设置内容单元格样式
            foreach (DataColumn item in dtSource.Columns)
            {
                ICellStyle columnStyle = workbook.CreateCellStyle();
                columnStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                arrColWidth[item.Ordinal] = Encoding.GetEncoding(936).GetBytes(item.ColumnName.ToString()).Length;
                arrColName[item.Ordinal] = item.ColumnName.ToString();
                if (excelConfig.ColumnEntity != null)
                {
                    ColumnEntity columnentity = excelConfig.ColumnEntity.Find(t => t.Column == item.ColumnName);
                    if (columnentity != null)
                    {
                        arrColName[item.Ordinal] = columnentity.ExcelColumn;
                        if (columnentity.Width != 0)
                        {
                            arrColWidth[item.Ordinal] = columnentity.Width;
                        }
                        if (columnentity.Background != new Color())
                        {
                            if (columnentity.Background != new Color())
                            {
                                columnStyle.FillPattern = FillPattern.SolidForeground;
                                columnStyle.FillForegroundColor = GetXLColour(workbook, columnentity.Background);
                            }
                        }
                        if (columnentity.Font != null || columnentity.Point != 0 || columnentity.ForeColor != new Color())
                        {
                            NPOI.SS.UserModel.IFont columnFont = workbook.CreateFont();
                            columnFont.FontHeightInPoints = 10;
                            if (columnentity.Font != null)
                            {
                                columnFont.FontName = columnentity.Font;
                            }
                            if (columnentity.Point != 0)
                            {
                                columnFont.FontHeightInPoints = columnentity.Point;
                            }
                            if (columnentity.ForeColor != new Color())
                            {
                                columnFont.Color = GetXLColour(workbook, columnentity.ForeColor);
                            }
                            columnStyle.SetFont(font);
                        }
                        columnStyle.Alignment = getAlignment(columnentity.Alignment);
                    }
                }
                arryColumStyle[item.Ordinal] = columnStyle;
            }
            if (excelConfig.IsAllSizeColumn)
            {
                #region 根据列中最长列的长度取得列宽
                for (int i = 0; i < dtSource.Rows.Count; i++)
                {
                    for (int j = 0; j < dtSource.Columns.Count; j++)
                    {
                        if (arrColWidth[j] != 0)
                        {
                            int intTemp = Encoding.GetEncoding(936).GetBytes(dtSource.Rows[i][j].ToString()).Length;
                            if (intTemp > arrColWidth[j])
                            {
                                arrColWidth[j] = intTemp;
                            }
                        }

                    }
                }
                #endregion
            }
            #endregion

            int rowIndex = 0;

            #region 表头及样式
            if (excelConfig.Title != null)
            {
                IRow headerRow = sheet.CreateRow(rowIndex);
                rowIndex++;
                if (excelConfig.TitleHeight != 0)
                {
                    headerRow.Height = (short)(excelConfig.TitleHeight * 20);
                }
                headerRow.HeightInPoints = 25;
                headerRow.CreateCell(0).SetCellValue(excelConfig.Title);
                headerRow.GetCell(0).CellStyle = headStyle;
                sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 0, dtSource.Columns.Count - 1)); // ------------------
            }
            #endregion

            #region 列头及样式
            {
                IRow headerRow = sheet.CreateRow(rowIndex);
                rowIndex++;
                #region 如果设置了列标题就按列标题定义列头，没定义直接按字段名输出
                foreach (DataColumn column in dtSource.Columns)
                {
                    headerRow.CreateCell(column.Ordinal).SetCellValue(arrColName[column.Ordinal]);
                    headerRow.GetCell(column.Ordinal).CellStyle = cHeadStyle;
                    //设置列宽
                    sheet.SetColumnWidth(column.Ordinal, (arrColWidth[column.Ordinal] + 1) * 256);
                }
                #endregion
            }
            #endregion

            ICellStyle dateStyle = workbook.CreateCellStyle();
            IDataFormat format = workbook.CreateDataFormat();
            dateStyle.DataFormat = format.GetFormat("yyyy-mm-dd");

            foreach (DataRow row in dtSource.Rows)
            {
                #region 新建表，填充表头，填充列头，样式
                if (rowIndex == 65535)
                {
                    sheet = workbook.CreateSheet();
                    rowIndex = 0;
                    #region 表头及样式
                    {
                        if (excelConfig.Title != null)
                        {
                            IRow headerRow = sheet.CreateRow(rowIndex);
                            rowIndex++;
                            if (excelConfig.TitleHeight != 0)
                            {
                                headerRow.Height = (short)(excelConfig.TitleHeight * 20);
                            }
                            headerRow.HeightInPoints = 25;
                            headerRow.CreateCell(0).SetCellValue(excelConfig.Title);
                            headerRow.GetCell(0).CellStyle = headStyle;
                            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 0, dtSource.Columns.Count - 1)); // ------------------
                        }

                    }
                    #endregion

                    #region 列头及样式
                    {
                        IRow headerRow = sheet.CreateRow(rowIndex);
                        rowIndex++;
                        #region 如果设置了列标题就按列标题定义列头，没定义直接按字段名输出
                        foreach (DataColumn column in dtSource.Columns)
                        {
                            headerRow.CreateCell(column.Ordinal).SetCellValue(arrColName[column.Ordinal]);
                            headerRow.GetCell(column.Ordinal).CellStyle = cHeadStyle;
                            //设置列宽
                            sheet.SetColumnWidth(column.Ordinal, (arrColWidth[column.Ordinal] + 1) * 256);
                        }
                        #endregion
                    }
                    #endregion
                }
                #endregion

                #region 填充内容
                IRow dataRow = sheet.CreateRow(rowIndex);
                foreach (DataColumn column in dtSource.Columns)
                {
                    ICell newCell = dataRow.CreateCell(column.Ordinal);
                    newCell.CellStyle = arryColumStyle[column.Ordinal];
                    string drValue = row[column].ToString();
                    SetCell(newCell, dateStyle, column.DataType, drValue);
                }
                #endregion
                rowIndex++;
            }
            using (MemoryStream ms = new MemoryStream())
            {
                workbook.Write(ms);
                ms.Flush();
                ms.Position = 0;
                return ms;
            }
        }
        #endregion

        #region ListExcel导出(加载模板)
        /// <summary>
        /// List根据模板导出ExcelMemoryStream
        /// </summary>
        /// <param name="list"></param>
        /// <param name="templdateName"></param>
        public static MemoryStream ExportListByTempale(List<TemplateMode> list, string templdateName)
        {
            try
            {

                string templatePath = HttpContext.Current.Server.MapPath("/") + "/Resource/ExcelTemplate/";
                string templdateName1 = string.Format("{0}{1}", templatePath, templdateName);

                FileStream fileStream = new FileStream(templdateName1, FileMode.Open, FileAccess.Read);
                ISheet sheet = null;
                if (templdateName.IndexOf(".xlsx") == -1)//2003
                {
                    HSSFWorkbook hssfworkbook = new HSSFWorkbook(fileStream);
                    sheet = hssfworkbook.GetSheetAt(0);
                    SetPurchaseOrder(sheet, list);
                    sheet.ForceFormulaRecalculation = true;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        hssfworkbook.Write(ms);
                        ms.Flush();
                        return ms;
                    }
                }
                else//2007
                {
                    XSSFWorkbook xssfworkbook = new XSSFWorkbook(fileStream);
                    sheet = xssfworkbook.GetSheetAt(0);
                    SetPurchaseOrder(sheet, list);
                    sheet.ForceFormulaRecalculation = true;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        xssfworkbook.Write(ms);
                        ms.Flush();
                        return ms;
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 赋值单元格
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="list"></param>
        private static void SetPurchaseOrder(ISheet sheet, List<TemplateMode> list)
        {
            try
            {
                foreach (var item in list)
                {
                    IRow row = null;
                    ICell cell = null;
                    row = sheet.GetRow(item.row);
                    if (row == null)
                    {
                        row = sheet.CreateRow(item.row);
                    }
                    cell = row.GetCell(item.cell);
                    if (cell == null)
                    {
                        cell = row.CreateCell(item.cell);
                    }
                    cell.SetCellValue(item.value);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region 设置表格内容
        private static void SetCell(ICell newCell, ICellStyle dateStyle, Type dataType, string drValue)
        {
            switch (dataType.ToString())
            {
                case "System.String"://字符串类型
                    newCell.SetCellValue(drValue);
                    break;
                case "System.DateTime"://日期类型
                    System.DateTime dateV;
                    if (System.DateTime.TryParse(drValue, out dateV))
                    {
                        newCell.SetCellValue(dateV);
                    }
                    else
                    {
                        newCell.SetCellValue("");
                    }
                    newCell.CellStyle = dateStyle;//格式化显示
                    break;
                case "System.Boolean"://布尔型
                    bool boolV = false;
                    bool.TryParse(drValue, out boolV);
                    newCell.SetCellValue(boolV);
                    break;
                case "System.Int16"://整型
                case "System.Int32":
                case "System.Int64":
                case "System.Byte":
                    int intV = 0;
                    int.TryParse(drValue, out intV);
                    newCell.SetCellValue(intV);
                    break;
                case "System.Decimal"://浮点型
                case "System.Double":
                    double doubV = 0;
                    double.TryParse(drValue, out doubV);
                    newCell.SetCellValue(doubV);
                    break;
                case "System.DBNull"://空值处理
                    newCell.SetCellValue("");
                    break;
                default:
                    newCell.SetCellValue("");
                    break;
            }
        }
        #endregion

        #region 从Excel导入
        /// <summary>
        /// 读取excel ,默认第一行为标头
        /// </summary>
        /// <param name="strFileName">excel文档路径</param>
        /// <returns></returns>
        public static DataTable ExcelImport(string strFileName)
        {
            DataTable dt = new DataTable();

            ISheet sheet = null;
            using (FileStream file = new FileStream(strFileName, FileMode.Open, FileAccess.Read))
            {
                if (strFileName.IndexOf(".xlsx") == -1)//2003
                {
                    HSSFWorkbook hssfworkbook = new HSSFWorkbook(file);
                    sheet = hssfworkbook.GetSheetAt(0);
                }
                else//2007
                {
                    XSSFWorkbook xssfworkbook = new XSSFWorkbook(file);
                    sheet = xssfworkbook.GetSheetAt(0);
                }
            }

            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();

            IRow headerRow = sheet.GetRow(0);
            int cellCount = headerRow.LastCellNum;

            for (int j = 0; j < cellCount; j++)
            {
                ICell cell = headerRow.GetCell(j);
                dt.Columns.Add(cell.ToString());
            }

            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                DataRow dataRow = dt.NewRow();

                for (int j = row.FirstCellNum; j < cellCount; j++)
                {
                    if (row.GetCell(j) != null)
                        dataRow[j] = row.GetCell(j).ToString();
                }

                dt.Rows.Add(dataRow);
            }
            return dt;
        }
        /// <summary>
        /// 读取excel ,默认第一行为标头 
        /// </summary>
        /// <param name="fileStream">文件数据流</param>
        /// <returns></returns>
        public static DataTable ExcelImport(Stream fileStream, string flieType)
        {
            DataTable dt = new DataTable();
            ISheet sheet = null;
            if (flieType == ".xls")
            {
                HSSFWorkbook hssfworkbook = new HSSFWorkbook(fileStream);
                sheet = hssfworkbook.GetSheetAt(0);
            }
            else
            {
                XSSFWorkbook xssfworkbook = new XSSFWorkbook(fileStream);
                sheet = xssfworkbook.GetSheetAt(0);
            }
            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
            IRow headerRow = sheet.GetRow(0);
            int cellCount = headerRow.LastCellNum;
            for (int j = 0; j < cellCount; j++)
            {
                ICell cell = headerRow.GetCell(j);
                dt.Columns.Add(cell.ToString());
            }
            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                DataRow dataRow = dt.NewRow();

                for (int j = row.FirstCellNum; j < cellCount; j++)
                {
                    if (row.GetCell(j) != null)
                        dataRow[j] = row.GetCell(j).ToString();
                }

                dt.Rows.Add(dataRow);
            }
            return dt;
        }
        #endregion

        #region RGB颜色转NPOI颜色
        private static short GetXLColour(HSSFWorkbook workbook, Color SystemColour)
        {
            short s = 0;
            HSSFPalette XlPalette = workbook.GetCustomPalette();
            NPOI.HSSF.Util.HSSFColor XlColour = XlPalette.FindColor(SystemColour.R, SystemColour.G, SystemColour.B);
            if (XlColour == null)
            {
                if (NPOI.HSSF.Record.PaletteRecord.STANDARD_PALETTE_SIZE < 255)
                {
                    XlColour = XlPalette.FindSimilarColor(SystemColour.R, SystemColour.G, SystemColour.B);
                    s = XlColour.Indexed;
                }

            }
            else
                s = XlColour.Indexed;
            return s;
        }
        #endregion

        #region 设置列的对齐方式
        /// <summary>
        /// 设置对齐方式
        /// </summary>
        /// <param name="style"></param>
        /// <returns></returns>
        private static NPOI.SS.UserModel.HorizontalAlignment getAlignment(string style)
        {
            switch (style)
            {
                case "center":
                    return NPOI.SS.UserModel.HorizontalAlignment.Center;
                case "left":
                    return NPOI.SS.UserModel.HorizontalAlignment.Left;
                case "right":
                    return NPOI.SS.UserModel.HorizontalAlignment.Right;
                case "fill":
                    return NPOI.SS.UserModel.HorizontalAlignment.Fill;
                case "justify":
                    return NPOI.SS.UserModel.HorizontalAlignment.Justify;
                case "centerselection":
                    return NPOI.SS.UserModel.HorizontalAlignment.CenterSelection;
                case "distributed":
                    return NPOI.SS.UserModel.HorizontalAlignment.Distributed;
            }
            return NPOI.SS.UserModel.HorizontalAlignment.General;


        }

        #endregion
    }

    public partial class ExcelAdoDotNetHelper : IDisposable
    {
        private string _connectionString { get; set; }
        private OleDbConnection _conn = null;
        private OleDbCommand _cmd = null;
        public ExcelAdoDotNetHelper()
            : this("Microsoft.ACE.OLEDB.12.0", "Excel 8.0;HDR=yes;IMEX=2")
        {

        }
        public ExcelAdoDotNetHelper(string provider, string extendedProperties)
        {
            _connectionString = "Provider={provider};Data Source={0};Extended Properties='{extendedProperties}';"
                .Replace("{provider}", provider)
                .Replace("{extendedProperties}", extendedProperties);
        }

        public void Open(string filePath)
        {
            this.Close();
            string connStr = string.Format(_connectionString, filePath);
            _conn = new OleDbConnection(connStr);
            _cmd = new OleDbCommand();
            _cmd.Connection = _conn;
            _cmd.CommandTimeout = _conn.ConnectionTimeout;
            _conn.Open();
        }

        public int ExecuteNonQuery(string sql, Dictionary<string, object> para = null)
        {
            _cmd.CommandType = System.Data.CommandType.Text;
            _cmd.CommandText = sql;
            _cmd.Parameters.Clear();
            if (para != null)
            {
                foreach (KeyValuePair<string, object> kv in para)
                {
                    _cmd.Parameters.Add(kv.Key, kv.Value);
                }
            }
            return _cmd.ExecuteNonQuery();
        }

        public void Close()
        {
            try
            {
                if (null != _cmd)
                {
                    _cmd.Dispose();
                }
                if (null != _conn)
                {
                    if (_conn.State == System.Data.ConnectionState.Open)
                    {
                        _conn.Close();
                    }
                }
            }
            finally
            {
                _cmd = null;
                _conn = null;
                GC.Collect();
            }
        }

        public void Dispose()
        {
            this.Close();
        }
    }
}
