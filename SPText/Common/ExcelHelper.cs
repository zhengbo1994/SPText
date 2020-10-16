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
using Microsoft.Office.Core;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Common
{
    public class ExcelHelper
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
                application = new PowerPoint.ApplicationClass();
                persentation = application.Presentations.Open(sourcePath, MsoTriState.msoTrue, MsoTriState.msoFalse, MsoTriState.msoFalse);
                persentation.SaveAs(targetPath, targetFileType, Microsoft.Office.Core.MsoTriState.msoTrue);

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
    }


}
