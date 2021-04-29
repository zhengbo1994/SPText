using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Data.SqlClient;
using NPOI.HSSF.Util;
using NPOI.XSSF.UserModel;
using System.Runtime.InteropServices;
using System.Data.OleDb;

namespace SPText.Common
{
    public class NOPIHelper
    {
        #region  调用
        public static void Show()
        {
            var list = new List<searchLogInfo>() {
                 new searchLogInfo{
                      AppName="应用程序名称0",
                      CreatedDateTime=DateTime.Now.ToString(),
                      ErrorMessage="错误信息0",
                      Name="名称0",
                      ObjectID="0",
                      ObjectInfo="对象信息0",
                      ObjectType="对象类型0",
                      Result="结果0",
                      Total="总数0"
                 },
                 new searchLogInfo{
                      AppName="应用程序名称1",
                      CreatedDateTime=DateTime.Now.ToString(),
                      ErrorMessage="错误信息1",
                      Name="名称1",
                      ObjectID="1",
                      ObjectInfo="对象信息1",
                      ObjectType="对象类型1",
                      Result="结果1",
                      Total="总数1"
                 },
            };
            {
                DataTable dt = ListToDataTable(list);//数据转化
                MemoryStream ms = ExportDataTableToExcel(dt);

                string pathName = $"{"前台日志" + "(" + DateTime.Now.ToString("yyyyMMdd hhmmss") + ")" + ".xls"}";
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, pathName);
                FileStream fs = new FileStream(path, FileMode.OpenOrCreate);
                BinaryWriter w = new BinaryWriter(fs);
                w.Write(ms.ToArray());
                fs.Close();
                ms.Close();

                DownloadFileInfo downloadFile = new DownloadFileInfo()
                {
                    FilePath = path,
                    FileName = pathName,
                    FileSize = 0
                };
                System.Diagnostics.Process.Start(downloadFile.FilePath);//打开文件
            }
            {
                DownloadFileInfo downloadFile = ConvertLogToExcel(list);//带样式个性化处理
                System.Diagnostics.Process.Start(downloadFile.FilePath);//打开文件
            }
            {
                DownloadFileInfo df = LogToExcel(list);//不带样式个性化处理
                System.Diagnostics.Process.Start(df.FilePath);//打开文件
            }
        }
        #endregion

        public static void show2()
        {
            string path = @"E:\AAA\SPText\SPText\File\AS928參數對照.xlsx";

            DataTable dataTable = ExcelToDataTableByNpoi(path, true);

            DataTable dt = ImportExcelFile(path);
            WriteExcel(dt, path);

        }



        #region   Asp.Net导入代码

        /// <summary>
        /// 通过路径获取DataTable（导入Excel文件）
        /// </summary>
        /// <param name="filePath">传入的路径</param>
        /// <returns></returns>
        public static DataTable ImportExcelFile(string filePath)
        {
            HSSFWorkbook hssfworkbook;
            //filePath = $"E:\\A-CompanyProject\\SPText\\SPText\\bin\\Debug\\前台日志(20200630 042526).xls";
            #region//初始化信息  
            try
            {
                using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    hssfworkbook = new HSSFWorkbook(file);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            #endregion

            ISheet sheet = hssfworkbook.GetSheetAt(0);
            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
            DataTable dt = new DataTable();
            for (int j = 0; j < (sheet.GetRow(0).LastCellNum); j++)
            {
                var tochar = Convert.ToChar((int)'A' + j).ToString();
                dt.Columns.Add(tochar);
            }
            while (rows.MoveNext())
            {
                HSSFRow row = (HSSFRow)rows.Current;
                DataRow dr = dt.NewRow();
                for (int i = 0; i < row.LastCellNum; i++)
                {
                    ICell cell = row.GetCell(i);
                    if (cell == null)
                    {
                        dr[i] = null;
                    }
                    else
                    {
                        dr[i] = cell.ToString();
                    }
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }

        /// <summary>
        /// 通过DataTable将数据转化为Excel（编写Excel）
        /// </summary>
        /// <param name="dt">数据源</param>
        /// <param name="filePath">保存的路径</param>
        public static void WriteExcel(DataTable dt, string filePath)
        {
            dt.TableName = "new";
            filePath = $"E:\\A-CompanyProject\\SPText\\SPText\\bin\\Debug\\前台日志新.xls";
            if (!string.IsNullOrEmpty(filePath) && null != dt && dt.Rows.Count > 0)
            {
                NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
                ISheet sheet = book.CreateSheet(dt.TableName);

                IRow row = sheet.CreateRow(0);
                //写入第一行数据（一般这行数据可以删除）
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    row.CreateCell(i).SetCellValue(dt.Columns[i].ColumnName);
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    IRow row2 = sheet.CreateRow(i + 1);
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        row2.CreateCell(j).SetCellValue(Convert.ToString(dt.Rows[i][j]));
                    }
                }

                // 写入到客户端  
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    book.Write(ms);
                    using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    {
                        byte[] data = ms.ToArray();
                        fs.Write(data, 0, data.Length);
                        fs.Flush();
                    }
                    book = null;
                }
            }
        }
        #endregion

        #region  NPOI操作

        /// <summary>
        /// DataTable转换成Excel文档流(导出数据量超出65535条,分sheet)（将数据表导出到Excel）
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static MemoryStream ExportDataTableToExcel(DataTable sourceTable)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            MemoryStream ms = new MemoryStream();
            int dtRowsCount = sourceTable.Rows.Count;
            int SheetCount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(dtRowsCount) / 65536));
            int SheetNum = 1;
            int rowIndex = 1;
            int tempIndex = 1; //标示 
            ISheet sheet = workbook.CreateSheet("sheet" + SheetNum);
            for (int i = 0; i < dtRowsCount; i++)
            {
                if (i == 0 || tempIndex == 1)
                {
                    IRow headerRow = sheet.CreateRow(0);
                    foreach (DataColumn column in sourceTable.Columns)
                        headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                }
                HSSFRow dataRow = (HSSFRow)sheet.CreateRow(tempIndex);
                foreach (DataColumn column in sourceTable.Columns)
                {
                    dataRow.CreateCell(column.Ordinal).SetCellValue(sourceTable.Rows[i][column].ToString());
                }
                if (tempIndex == 65535)
                {
                    SheetNum++;
                    sheet = workbook.CreateSheet("sheet" + SheetNum);//
                    tempIndex = 0;
                }
                rowIndex++;
                tempIndex++;
                //AutoSizeColumns(sheet);
            }

            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;
            sheet = null;
            // headerRow = null;
            workbook = null;
            return ms;
        }
        /// <summary>
        /// 将集合转换为Excel（操作样式）
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static DownloadFileInfo ConvertLogToExcel(List<searchLogInfo> list)
        {
            //var lis = list.ConvertAll(l => l as LogEntity).ToList();
            //typeof(T).GetCustomAttributes()
            //删除以前的日志文件
            string[] files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory);
            string pattern = "\\d{4}\\d{2}\\d{2}.+\\.xls$";
            Regex r = new Regex(pattern);

            foreach (string file in files)
            {
                if (r.IsMatch(file))
                {
                    FileInfo fi = new FileInfo(file);
                    fi.Delete();
                }
            }


            DownloadFileInfo info = new DownloadFileInfo();
            try
            {
                //操作时间 用户名 操作类型 描述 
                IWorkbook workbook = new HSSFWorkbook();


                ISheet sheet = workbook.CreateSheet("前台日志");
                IRow row0 = sheet.CreateRow(0);
                int dtRowsCount = list.Count;
                int SheetCount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(dtRowsCount) / 65536));
                int SheetNum = 1;
                int rowIndex = 1;
                int tempIndex = 1; //标示 

                row0.CreateCell(0).SetCellValue("应用模式名称");
                row0.CreateCell(1).SetCellValue("操作业务名称");
                row0.CreateCell(2).SetCellValue("日志信息");
                row0.CreateCell(3).SetCellValue("日志时间");
                row0.CreateCell(4).SetCellValue("日志报错");
                row0.CreateCell(5).SetCellValue("日志状态");




                #region  个性化样式处理模板
                bool flag = true;
                if (flag)
                {
                    HSSFCellStyle style = (HSSFCellStyle)workbook.CreateCellStyle();
                    HSSFDataFormat format = (HSSFDataFormat)workbook.CreateDataFormat();
                    style.DataFormat = format.GetFormat("yyyy-MM-dd HH:mm:ss");

                    for (int i = 0; i < list.Count; i++)
                    {
                        IRow row = sheet.CreateRow(i + 1);
                        //row.CreateCell(0).SetCellValue(list[i].OperateTime);
                        ICell cell = row.CreateCell(0);
                        cell.CellStyle = style;
                        //cell.SetCellValue(list[i].OperateTime);
                        cell.Sheet.SetColumnWidth(0, 18 * 256);
                        row.CreateCell(0).SetCellValue(list[i].AppName);
                        row.CreateCell(1).SetCellValue(list[i].Name);
                        row.CreateCell(2).SetCellValue(list[i].ObjectInfo);
                        row.CreateCell(3).SetCellValue(list[i].CreatedDateTime);
                        row.CreateCell(4).SetCellValue(list[i].ErrorMessage);
                        row.CreateCell(5).SetCellValue(list[i].Result);
                    }
                }
                else
                {
                    //加粗
                    IFont font = workbook.CreateFont();//创建字体
                    font.FontHeightInPoints = 10;//字体高度点
                    font.FontName = "Arial";//字体
                    font.Boldweight = (short)FontBoldWeight.Bold;//粗体

                    //字体样式调整
                    ICellStyle cellStyle = workbook.CreateCellStyle();//创建单元格样式
                    cellStyle.VerticalAlignment = VerticalAlignment.Center;//垂直对齐
                    cellStyle.Alignment = HorizontalAlignment.Center;//水平对齐
                    cellStyle.SetFont(font);//设置字体样式
                    cellStyle.FillPattern = FillPattern.SolidForeground;//填充图案=实心前景
                    cellStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Coral.Index;//填充底色=红色
                    SetColor(0, 0, 6, 7, sheet, cellStyle, BorderStyle.Medium);//将cellStyle样式绑定到sheet

                    sheet.SetColumnWidth(0, 13 * 256);//设置宽度
                    sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 1, 3));//合并单元格（开始列，结束列，开始行，结束行）
                }
                #endregion

                string fileName = "前台日志" + "(" + DateTime.Now.ToString("yyyyMMdd hhmmss") + ")" + ".xls";
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
                info.FilePath = path;
                info.FileName = fileName;
                using (FileStream fs = System.IO.File.OpenWrite(path))
                {
                    info.FileSize = fs.Length;
                    workbook.Write(fs);
                }
            }
            catch (Exception)
            {

                throw;
            }
            return info;

        }
        /// <summary>
        /// Log到Excel
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static DownloadFileInfo LogToExcel(List<searchLogInfo> list)
        {
            //var lis = list.ConvertAll(l => l as LogEntity).ToList();
            //typeof(T).GetCustomAttributes()
            //删除以前的日志文件
            string[] files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory);
            string pattern = "\\d{4}\\d{2}\\d{2}.+\\.xls$";
            Regex r = new Regex(pattern);

            foreach (string file in files)
            {
                if (r.IsMatch(file))
                {
                    FileInfo fi = new FileInfo(file);
                    fi.Delete();
                }
            }
            DownloadFileInfo info = new DownloadFileInfo();
            try
            {
                //操作时间 用户名 操作类型 描述 
                IWorkbook workbook = new HSSFWorkbook();
                //ISheet sheet = workbook.CreateSheet("前台日志");
                //IRow row0 = sheet.CreateRow(0);
                int dtRowsCount = list.Count;
                int SheetCount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(dtRowsCount) / 65536));
                int SheetNum = 1;
                int rowIndex = 1;
                int tempIndex = 1; //标示 
                ISheet sheet = workbook.CreateSheet("sheet" + SheetNum);
                for (int i = 0; i < dtRowsCount; i++)
                {
                    if (i == 0 || tempIndex == 1)
                    {
                        IRow row0 = sheet.CreateRow(0);
                        row0.CreateCell(0).SetCellValue("应用模式名称");
                        row0.CreateCell(1).SetCellValue("操作业务名称");
                        row0.CreateCell(2).SetCellValue("日志信息");
                        row0.CreateCell(3).SetCellValue("日志时间");
                        row0.CreateCell(4).SetCellValue("日志报错");
                        row0.CreateCell(5).SetCellValue("日志状态");
                    }
                    HSSFRow row = (HSSFRow)sheet.CreateRow(tempIndex);
                    //HSSFCellStyle style = (HSSFCellStyle)workbook.CreateCellStyle();
                    //HSSFDataFormat format = (HSSFDataFormat)workbook.CreateDataFormat();
                    //style.DataFormat = format.GetFormat("yyyy-MM-dd HH:mm:ss");
                    var j = tempIndex + (SheetNum - 1) * tempIndex;
                    row.CreateCell(0).SetCellValue(list[i].AppName);
                    row.CreateCell(1).SetCellValue(list[i].Name);
                    row.CreateCell(2).SetCellValue(list[i].ObjectInfo);
                    row.CreateCell(3).SetCellValue(list[i].CreatedDateTime);
                    row.CreateCell(4).SetCellValue(list[i].ErrorMessage);
                    row.CreateCell(5).SetCellValue(list[i].Result);
                    if (tempIndex == 65535)
                    {
                        SheetNum++;
                        sheet = workbook.CreateSheet("sheet" + SheetNum);
                        tempIndex = 0;
                    }
                    rowIndex++;
                    tempIndex++;
                }

                string fileName = "前台日志" + "(" + DateTime.Now.ToString("yyyyMMdd hhmmss") + ")" + ".xls";
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
                info.FilePath = path;
                info.FileName = fileName;
                using (FileStream fs = System.IO.File.OpenWrite(path))
                {
                    info.FileSize = fs.Length;
                    workbook.Write(fs);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return info;
        }




        #region  样式个性化处理
        private static void SetRangeBorder(int firstRow, int endRow, int firstCloumn, int endCloumn, ISheet sheet, IWorkbook workbook, BorderStyle borderStyle)
        {
            for (int i = firstRow; i <= endRow; i++)
            {
                for (int j = firstCloumn; j <= endCloumn; j++)
                {
                    WhenIsNull(sheet, workbook, i, j);
                    if (i == firstRow)
                    {

                        sheet.GetRow(i).GetCell(j).CellStyle.BorderTop = borderStyle;
                    }
                    if (i == endRow)
                    {
                        sheet.GetRow(i).GetCell(j).CellStyle.BorderBottom = borderStyle;
                    }
                    if (j == firstCloumn)
                    {

                        sheet.GetRow(i).GetCell(j).CellStyle.BorderLeft = borderStyle;
                    }
                    if (j == endCloumn)
                    {
                        sheet.GetRow(i).GetCell(j).CellStyle.BorderRight = borderStyle;
                    }
                }
            }
        }

        private static void WhenIsNull(ISheet sheet, IWorkbook workbook, int i, int j)
        {
            if (sheet.GetRow(i) == null)
            {
                sheet.CreateRow(i);
            }
            if (sheet.GetRow(i).GetCell(j) == null)
            {
                sheet.GetRow(i).CreateCell(j);
            }
            if (sheet.GetRow(i).GetCell(j).CellStyle == null)
            {
                sheet.GetRow(i).GetCell(j).CellStyle = workbook.CreateCellStyle();
            }
        }

        private static void SetColor(int firstRow, int endRow, int firstCloumn, int endCloumn, ISheet sheetSummary, ICellStyle s, BorderStyle borderStyle)
        {
            for (int i = firstRow; i <= endRow; i++)
            {
                for (int j = firstCloumn; j <= endCloumn; j++)
                {
                    if (sheetSummary.GetRow(i) == null)
                    {
                        sheetSummary.CreateRow(i);
                    }
                    if (sheetSummary.GetRow(i).GetCell(j) == null)
                    {
                        sheetSummary.GetRow(i).CreateCell(j);
                    }
                    sheetSummary.GetRow(i).GetCell(j).CellStyle = s;
                    sheetSummary.GetRow(i).GetCell(j).CellStyle.BorderTop = borderStyle;
                    sheetSummary.GetRow(i).GetCell(j).CellStyle.BorderBottom = borderStyle;
                    sheetSummary.GetRow(i).GetCell(j).CellStyle.BorderLeft = borderStyle;
                    sheetSummary.GetRow(i).GetCell(j).CellStyle.BorderRight = borderStyle;
                }
            }
        }
        #endregion

        #region  辅助数据
        /// <summary>
        /// 下载时保存的信息
        /// </summary>
        public class DownloadFileInfo
        {
            /// <summary>
            /// 文件路径
            /// </summary>
            public string FilePath { get; set; }
            /// <summary>
            /// 文件名称
            /// </summary>
            public string FileName { get; set; }
            /// <summary>
            /// 文件大小
            /// </summary>
            public Int64 FileSize { get; set; }
        }
        /// <summary>
        /// 保存的集合信息
        /// </summary>
        public class searchLogInfo
        {
            public string AppName { get; set; }
            public string Name { get; set; }
            public string ObjectID { get; set; }
            public string ObjectType { get; set; }
            public string ObjectInfo { get; set; }
            public string Result { get; set; }
            public string ErrorMessage { get; set; }
            public string CreatedDateTime { get; set; }
            public string Total { get; set; }
        }
        #endregion
        #endregion

        #region  List和DataTable互转
        #region  ListToDataTable
        public static DataTable ListToDataTable<T>(List<T> list)
        {
            Type tp = typeof(T);
            PropertyInfo[] proInfos = tp.GetProperties();
            DataTable dt = new DataTable();
            foreach (var item in proInfos)
            {
                //解决DataSet不支持System.Nullable<>问题
                Type colType = item.PropertyType;
                if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                {
                    colType = colType.GetGenericArguments()[0];
                }
                //添加列明及对应类型 
                dt.Columns.Add(item.Name, colType);
            }
            foreach (var item in list)
            {
                DataRow dr = dt.NewRow();
                foreach (var proInfo in proInfos)
                {
                    object obj = proInfo.GetValue(item);
                    if (obj == null)
                    {
                        continue;
                    }
                    if (proInfo.PropertyType == typeof(DateTime) && Convert.ToDateTime(obj) < Convert.ToDateTime("1753-01-01"))
                    {
                        continue;
                    }
                    dr[proInfo.Name] = obj;
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }
        #endregion

        #region  DataTableToList
        public static IList<T> DataTableToList<T>(DataTable table)
        {
            if (table == null)
            {
                return null;
            }

            List<DataRow> rows = new List<DataRow>();

            foreach (DataRow row in table.Rows)
            {
                rows.Add(row);
            }

            return ConvertTo<T>(rows);
        }

        public static IList<T> ConvertTo<T>(IList<DataRow> rows)
        {
            IList<T> list = null;

            if (rows != null)
            {
                list = new List<T>();

                foreach (DataRow row in rows)
                {
                    T item = CreateItem<T>(row);
                    list.Add(item);
                }
            }

            return list;
        }

        public static T CreateItem<T>(DataRow row)
        {
            T obj = default(T);
            if (row != null)
            {
                obj = Activator.CreateInstance<T>();

                foreach (DataColumn column in row.Table.Columns)
                {
                    PropertyInfo prop = obj.GetType().GetProperty(column.ColumnName);
                    try
                    {
                        object value = row[column.ColumnName];
                        prop.SetValue(obj, value, null);
                    }
                    catch
                    {  //You can log something here     
                       //throw;    
                    }
                }
            }

            return obj;
        }
        #endregion
        #endregion

        #region  Excel操作
        /// <summary>
        /// 将DataTable呈现到Excel
        /// </summary>
        /// <param name="SourceTable"></param>
        /// <returns></returns>
        public static Stream RenderDataTableToExcel(DataTable SourceTable)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            MemoryStream ms = new MemoryStream();
            ISheet sheet = workbook.CreateSheet();
            IRow headerRow = sheet.CreateRow(0);
            // handling header.
            foreach (DataColumn column in SourceTable.Columns)
            {
                headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
            }
            // handling value.
            int rowIndex = 1;
            foreach (DataRow row in SourceTable.Rows)
            {
                IRow dataRow = sheet.CreateRow(rowIndex);
                foreach (DataColumn column in SourceTable.Columns)
                {
                    dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());


                }
                rowIndex++;
            }
            //列宽自适应，只对英文和数字有效  
            //for (int i = 0; i <= SourceTable.Rows.Count; i++)
            //{
            //    sheet.AutoSizeColumn(i);
            //}
            //获取当前列的宽度，然后对比本列的长度，取最大值  
            for (int columnNum = 0; columnNum <= SourceTable.Columns.Count; columnNum++)
            {

                int columnWidth = sheet.GetColumnWidth(columnNum) / 256;
                for (int rowNum = 1; rowNum <= sheet.LastRowNum; rowNum++)
                {
                    IRow currentRow;
                    //当前行未被使用过  
                    if (sheet.GetRow(rowNum) == null)
                    {
                        currentRow = sheet.CreateRow(rowNum);
                    }
                    else
                    {
                        currentRow = sheet.GetRow(rowNum);
                    }

                    if (currentRow.GetCell(columnNum) != null)
                    {
                        ICell currentCell = currentRow.GetCell(columnNum);
                        int length = Encoding.Default.GetBytes(currentCell.ToString()).Length;
                        if (columnWidth < length)
                        {
                            columnWidth = length;
                        }
                    }
                }
                sheet.SetColumnWidth(columnNum, (columnWidth + 5) * 256);
            }

            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;
            sheet = null;
            headerRow = null;
            workbook = null;
            return ms;
        }

        /// <summary>
        /// 将DataTable呈现到Excel
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static Stream RenderDataTableToExcelMore(DataTable dt)
        {

            HSSFWorkbook workbook = new HSSFWorkbook();
            MemoryStream ms = new MemoryStream();
            ISheet sheet;
            IRow headerRow;
            //1.0计算出table的数量
            int rownum = dt.Rows.Count;

            //2.0 10条数据为一张表
            double ceilNum = rownum / 20000.0;
            int ceilnum = (int)Math.Ceiling(ceilNum);

            //10行的指标
            int tempvalue = 20000;
            int stratNum = 0;
            for (int k = 1; k <= ceilnum; k++)
            {
                sheet = workbook.CreateSheet("sheet" + k);
                headerRow = sheet.CreateRow(0);
                // handling header.
                foreach (DataColumn column in dt.Columns)
                {
                    headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                }
                int rowIndex = 1;

                for (int columnNum = (k - 1) * tempvalue; columnNum <= k * tempvalue - 1; columnNum++)
                {
                    if (columnNum == rownum - 1)
                    {
                        break;
                    }
                    DataRow row = dt.Rows[columnNum];
                    IRow dataRow = sheet.CreateRow(rowIndex);
                    foreach (DataColumn column in dt.Columns)
                    {
                        dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                    }
                    rowIndex++;
                    stratNum = stratNum + 1;
                }

                for (int columnNum = 0; columnNum <= dt.Columns.Count; columnNum++)
                {

                    int columnWidth = sheet.GetColumnWidth(columnNum) / 256;
                    for (int rowNum = 1; rowNum <= sheet.LastRowNum; rowNum++)
                    {
                        IRow currentRow;
                        //当前行未被使用过  
                        if (sheet.GetRow(rowNum) == null)
                        {
                            currentRow = sheet.CreateRow(rowNum);
                        }
                        else
                        {
                            currentRow = sheet.GetRow(rowNum);
                        }

                        if (currentRow.GetCell(columnNum) != null)
                        {
                            ICell currentCell = currentRow.GetCell(columnNum);
                            int length = Encoding.Default.GetBytes(currentCell.ToString()).Length;
                            if (columnWidth < length)
                            {
                                columnWidth = length;
                            }
                        }
                    }
                    sheet.SetColumnWidth(columnNum, (columnWidth + 5) * 256);
                }

                stratNum = stratNum + 1;


            }


            // handling value.




            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;
            sheet = null;
            headerRow = null;
            workbook = null;
            return ms;
        }


        /// <summary>
        /// 将DataSet呈现到Excel
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static Stream RenderDataSetToExcel(DataSet ds)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            MemoryStream ms = new MemoryStream();
            ISheet sheet;
            IRow headerRow;
            for (int i = 0; i < ds.Tables.Count; i++)
            {

                var item = i + 1;
                sheet = workbook.CreateSheet("sheet" + item);
                headerRow = sheet.CreateRow(0);
                // handling header.
                foreach (DataColumn column in ds.Tables[i].Columns)
                {
                    headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                }
                // handling value.
                int rowIndex = 1;
                foreach (DataRow row in ds.Tables[i].Rows)
                {
                    IRow dataRow = sheet.CreateRow(rowIndex);
                    foreach (DataColumn column in ds.Tables[i].Columns)
                    {
                        dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());


                    }
                    rowIndex++;
                }

                for (int columnNum = 0; columnNum <= ds.Tables[i].Columns.Count; columnNum++)
                {

                    int columnWidth = sheet.GetColumnWidth(columnNum) / 256;
                    for (int rowNum = 1; rowNum <= sheet.LastRowNum; rowNum++)
                    {
                        IRow currentRow;
                        //当前行未被使用过  
                        if (sheet.GetRow(rowNum) == null)
                        {
                            currentRow = sheet.CreateRow(rowNum);
                        }
                        else
                        {
                            currentRow = sheet.GetRow(rowNum);
                        }

                        if (currentRow.GetCell(columnNum) != null)
                        {
                            ICell currentCell = currentRow.GetCell(columnNum);
                            int length = Encoding.Default.GetBytes(currentCell.ToString()).Length;
                            if (columnWidth < length)
                            {
                                columnWidth = length;
                            }
                        }
                    }
                    sheet.SetColumnWidth(columnNum, (columnWidth + 5) * 256);
                }
            }

            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;
            sheet = null;
            headerRow = null;
            workbook = null;
            return ms;
        }

        /// <summary>
        /// 将数据表呈现为Excel XLSX
        /// </summary>
        /// <param name="SourceTable"></param>
        /// <returns></returns>
        public static Stream RenderDataTableToExcelXLSX(DataTable SourceTable)
        {
            XSSFWorkbook workbook = new XSSFWorkbook();
            MemoryStream ms = new MemoryStream();
            ISheet sheet = workbook.CreateSheet();
            IRow headerRow = sheet.CreateRow(0);
            // handling header.
            foreach (DataColumn column in SourceTable.Columns)
            {
                headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
            }
            // handling value.
            int rowIndex = 1;
            foreach (DataRow row in SourceTable.Rows)
            {
                IRow dataRow = sheet.CreateRow(rowIndex);
                foreach (DataColumn column in SourceTable.Columns)
                {
                    dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());


                }
                rowIndex++;
            }

            for (int columnNum = 0; columnNum <= SourceTable.Columns.Count; columnNum++)
            {

                int columnWidth = sheet.GetColumnWidth(columnNum) / 256;
                for (int rowNum = 1; rowNum <= sheet.LastRowNum; rowNum++)
                {
                    IRow currentRow;
                    //当前行未被使用过  
                    if (sheet.GetRow(rowNum) == null)
                    {
                        currentRow = sheet.CreateRow(rowNum);
                    }
                    else
                    {
                        currentRow = sheet.GetRow(rowNum);
                    }

                    if (currentRow.GetCell(columnNum) != null)
                    {
                        ICell currentCell = currentRow.GetCell(columnNum);
                        int length = Encoding.Default.GetBytes(currentCell.ToString()).Length;
                        if (columnWidth < length)
                        {
                            columnWidth = length;
                        }
                    }
                }
                sheet.SetColumnWidth(columnNum, (columnWidth + 5) * 256);
            }

            workbook.Write(ms);
            ms.Flush();
            //ms.Position = 0;
            sheet = null;
            headerRow = null;
            workbook = null;
            return ms;
        }


        /// <summary>
        /// xlsx 可以超过6万多条数据（将数据表呈现为Excel类型XLSX）
        /// </summary>
        /// <param name="SourceTable"></param>
        /// <returns></returns>
        public static Stream RenderDataTableToExcelTypeXLSX(DataTable SourceTable)
        {
            XSSFWorkbook workbook = new XSSFWorkbook();
            MemoryStream ms = new MemoryStream();
            ISheet sheet = workbook.CreateSheet();
            IRow headerRow = sheet.CreateRow(0);

            // handling header.
            foreach (DataColumn column in SourceTable.Columns)
            {
                headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
            }
            // handling value.
            int rowIndex = 1;
            foreach (DataRow row in SourceTable.Rows)
            {
                IRow dataRow = sheet.CreateRow(rowIndex);
                int i = 0;
                foreach (DataColumn column in SourceTable.Columns)
                {

                    ConvertTypeToCell(row, dataRow, column);
                }
                rowIndex++;
            }
            //列宽自适应，只对英文和数字有效  
            //for (int i = 0; i <= SourceTable.Rows.Count; i++)
            //{
            //    sheet.AutoSizeColumn(i);
            //}
            //获取当前列的宽度，然后对比本列的长度，取最大值  
            for (int columnNum = 0; columnNum <= SourceTable.Columns.Count; columnNum++)
            {

                int columnWidth = sheet.GetColumnWidth(columnNum) / 256;
                for (int rowNum = 1; rowNum <= sheet.LastRowNum; rowNum++)
                {
                    IRow currentRow;
                    //当前行未被使用过  
                    if (sheet.GetRow(rowNum) == null)
                    {
                        currentRow = sheet.CreateRow(rowNum);
                    }
                    else
                    {
                        currentRow = sheet.GetRow(rowNum);
                    }

                    if (currentRow.GetCell(columnNum) != null)
                    {
                        ICell currentCell = currentRow.GetCell(columnNum);
                        int length = Encoding.Default.GetBytes(currentCell.ToString()).Length;
                        if (columnWidth < length)
                        {
                            columnWidth = length;
                        }
                    }
                }
                sheet.SetColumnWidth(columnNum, (columnWidth + 1) * 256);
            }

            workbook.Write(ms);
            ms.Flush();
            //ms.Position = 0;
            sheet = null;
            headerRow = null;
            workbook = null;
            return ms;
        }


        /// <summary>
        /// 将数据表呈现为Excel类型
        /// </summary>
        /// <param name="SourceTable"></param>
        /// <returns></returns>
        public static Stream RenderDataTableToExcelType(DataTable SourceTable)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            MemoryStream ms = new MemoryStream();
            ISheet sheet = workbook.CreateSheet();
            IRow headerRow = sheet.CreateRow(0);

            // handling header.
            foreach (DataColumn column in SourceTable.Columns)
            {
                headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
            }
            // handling value.
            int rowIndex = 1;
            foreach (DataRow row in SourceTable.Rows)
            {
                IRow dataRow = sheet.CreateRow(rowIndex);
                int i = 0;
                foreach (DataColumn column in SourceTable.Columns)
                {

                    ConvertTypeToCell(row, dataRow, column);
                }
                rowIndex++;
            }
            //列宽自适应，只对英文和数字有效  
            //for (int i = 0; i <= SourceTable.Rows.Count; i++)
            //{
            //    sheet.AutoSizeColumn(i);
            //}
            //获取当前列的宽度，然后对比本列的长度，取最大值  
            for (int columnNum = 0; columnNum <= SourceTable.Columns.Count; columnNum++)
            {

                int columnWidth = sheet.GetColumnWidth(columnNum) / 256;
                for (int rowNum = 1; rowNum <= sheet.LastRowNum; rowNum++)
                {
                    IRow currentRow;
                    //当前行未被使用过  
                    if (sheet.GetRow(rowNum) == null)
                    {
                        currentRow = sheet.CreateRow(rowNum);
                    }
                    else
                    {
                        currentRow = sheet.GetRow(rowNum);
                    }

                    if (currentRow.GetCell(columnNum) != null)
                    {
                        ICell currentCell = currentRow.GetCell(columnNum);
                        int length = Encoding.Default.GetBytes(currentCell.ToString()).Length;
                        if (columnWidth < length)
                        {
                            columnWidth = length;
                        }
                    }
                }
                sheet.SetColumnWidth(columnNum, (columnWidth + 1) * 256);
            }

            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;
            sheet = null;
            headerRow = null;
            workbook = null;
            return ms;
        }

        /// <summary>
        /// 将数据表呈现为Excel类型
        /// </summary>
        /// <param name="SourceTable"></param>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        public static Stream RenderDataTableToExcelType(DataTable SourceTable, string sheetName)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            MemoryStream ms = new MemoryStream();
            ISheet sheet = workbook.CreateSheet(sheetName);
            IRow headerRow = sheet.CreateRow(0);

            // handling header.
            foreach (DataColumn column in SourceTable.Columns)
            {
                headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
            }
            // handling value.
            int rowIndex = 1;
            foreach (DataRow row in SourceTable.Rows)
            {
                IRow dataRow = sheet.CreateRow(rowIndex);
                int i = 0;
                foreach (DataColumn column in SourceTable.Columns)
                {

                    ConvertTypeToCell(row, dataRow, column);
                }
                rowIndex++;
            }
            for (int columnNum = 0; columnNum <= SourceTable.Columns.Count; columnNum++)
            {

                int columnWidth = sheet.GetColumnWidth(columnNum) / 256;
                for (int rowNum = 1; rowNum <= sheet.LastRowNum; rowNum++)
                {
                    IRow currentRow;
                    //当前行未被使用过  
                    if (sheet.GetRow(rowNum) == null)
                    {
                        currentRow = sheet.CreateRow(rowNum);
                    }
                    else
                    {
                        currentRow = sheet.GetRow(rowNum);
                    }

                    if (currentRow.GetCell(columnNum) != null)
                    {
                        ICell currentCell = currentRow.GetCell(columnNum);
                        int length = Encoding.Default.GetBytes(currentCell.ToString()).Length;
                        if (columnWidth < length)
                        {
                            columnWidth = length;
                        }
                    }
                }
                sheet.SetColumnWidth(columnNum, (columnWidth + 1) * 256);
            }

            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;
            sheet = null;
            headerRow = null;
            workbook = null;
            return ms;
        }

        /// <summary>
        /// 将类型转换为单元格
        /// </summary>
        /// <param name="row"></param>
        /// <param name="dataRow"></param>
        /// <param name="column"></param>
        private static void ConvertTypeToCell(DataRow row, IRow dataRow, DataColumn column)
        {

            if (column.DataType == typeof(Int32))
            {
                var cell = dataRow.CreateCell(column.Ordinal);
                cell.SetCellValue(row[column] != DBNull.Value ? Convert.ToInt32(row[column]) : 0);
            }
            //    else if(column.DataType == typeof(DateTime))
            //{
            //    var cell = dataRow.CreateCell(column.Ordinal);
            //    if (row[column] != DBNull.Value)
            //    {
            //        cell.SetCellValue(Convert.ToDateTime(row[column]));
            //    }
            //    else
            //    {
            //        cell.SetCellValue(string.Empty);
            //    }
            //}
            else if (column.DataType == typeof(Boolean))
            {
                var cell = dataRow.CreateCell(column.Ordinal);
                if (row[column] != DBNull.Value)
                {
                    cell.SetCellValue(Convert.ToBoolean(row[column]));
                }
                else
                {
                    cell.SetCellValue(string.Empty);
                }
            }
            else
            {
                var cell = dataRow.CreateCell(column.Ordinal);
                cell.SetCellValue(row[column].ToString());
            }
        }


        /// <summary>
        /// 将数据表呈现到Excel
        /// </summary>
        /// <param name="SourceTable"></param>
        /// <param name="FileName"></param>
        public static void RenderDataTableToExcel(DataTable SourceTable, string FileName)
        {
            MemoryStream ms = RenderDataTableToExcel(SourceTable) as MemoryStream;
            FileStream fs = new FileStream(FileName, FileMode.Create, FileAccess.Write);
            byte[] data = ms.ToArray();
            fs.Write(data, 0, data.Length);
            fs.Flush();
            fs.Close();
            data = null;
            ms = null;
            fs = null;
        }

        /// <summary>
        /// 从Excel呈现数据表
        /// </summary>
        /// <param name="ExcelFileStream"></param>
        /// <param name="SheetName"></param>
        /// <param name="HeaderRowIndex"></param>
        /// <returns></returns>
        public static DataTable RenderDataTableFromExcel(Stream ExcelFileStream, string SheetName, int HeaderRowIndex)
        {
            HSSFWorkbook workbook = new HSSFWorkbook(ExcelFileStream);
            ISheet sheet = workbook.GetSheet(SheetName);
            DataTable table = new DataTable();
            IRow headerRow = sheet.GetRow(HeaderRowIndex);
            int cellCount = headerRow.LastCellNum;
            for (int i = headerRow.FirstCellNum; i < cellCount; i++)
            {
                DataColumn column = new
                DataColumn(headerRow.GetCell(i).StringCellValue);
                table.Columns.Add(column);
            }
            int rowCount = sheet.LastRowNum;

            for (int i = (sheet.FirstRowNum + 1); i < sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                DataRow dataRow = table.NewRow();
                for (int j = row.FirstCellNum; j < cellCount; j++)
                    dataRow[j] = row.GetCell(j).ToString();
            }
            ExcelFileStream.Close();
            workbook = null;
            sheet = null;
            return table;
        }


        /// <summary>
        /// 从Excel呈现数据表
        /// </summary>
        /// <param name="ExcelFileStream"></param>
        /// <param name="SheetIndex"></param>
        /// <param name="HeaderRowIndex"></param>
        /// <returns></returns>
        public static DataTable RenderDataTableFromExcel(Stream ExcelFileStream, int SheetIndex, int HeaderRowIndex)
        {
            HSSFWorkbook workbook = new HSSFWorkbook(ExcelFileStream);
            ISheet sheet = workbook.GetSheetAt(SheetIndex);
            DataTable table = new DataTable();
            IRow headerRow = sheet.GetRow(HeaderRowIndex);
            int cellCount = headerRow.LastCellNum;
            for (int i = headerRow.FirstCellNum; i < cellCount; i++)
            {
                DataColumn column = new
                DataColumn(headerRow.GetCell(i).StringCellValue);
                table.Columns.Add(column);
            }

            int rowCount = sheet.LastRowNum;
            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                DataRow dataRow = table.NewRow();
                for (int j = row.FirstCellNum; j < cellCount; j++)
                {
                    if (row.GetCell(j) != null)
                        dataRow[j] = row.GetCell(j).ToString();
                }
                table.Rows.Add(dataRow);
            }
            ExcelFileStream.Close();
            workbook = null;
            sheet = null;
            return table;
        }

        /// <summary>
        /// 从Excel XLS呈现数据表
        /// </summary>
        /// <param name="ExcelFileStream"></param>
        /// <param name="SheetIndex"></param>
        /// <param name="HeaderRowIndex"></param>
        /// <returns></returns>
        public static DataTable RenderDataTableFromExcelXLS(Stream ExcelFileStream, int SheetIndex, int HeaderRowIndex)
        {
            XSSFWorkbook workbook = new XSSFWorkbook(ExcelFileStream);
            ISheet sheet = workbook.GetSheetAt(SheetIndex);
            DataTable table = new DataTable();
            IRow headerRow = sheet.GetRow(HeaderRowIndex);
            int cellCount = headerRow.LastCellNum;
            for (int i = headerRow.FirstCellNum; i < cellCount; i++)
            {
                DataColumn column = new
                DataColumn(headerRow.GetCell(i).StringCellValue);
                table.Columns.Add(column);
            }

            int rowCount = sheet.LastRowNum;
            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);



                DataRow dataRow = table.NewRow();
                if (row != null)
                {
                    for (int j = row.FirstCellNum; j < cellCount; j++)
                    {
                        if (row.GetCell(j) != null)
                        {
                            if (row.GetCell(j).CellType == CellType.Numeric)
                            {
                                if (HSSFDateUtil.IsCellDateFormatted(row.GetCell(j)))
                                {
                                    dataRow[j] = row.GetCell(j).DateCellValue;
                                }
                                else
                                {
                                    dataRow[j] = row.GetCell(j);
                                }
                            }
                            else
                                dataRow[j] = GetCellValue(row.GetCell(j));
                        }
                    }
                }
                table.Rows.Add(dataRow);
            }
            ExcelFileStream.Close();
            workbook = null;
            sheet = null;
            return table;
        }

        #region  支持.xls格式文件（封装的有点问题后期再调准）
        public static DataSet RenderDataSetFromExcel(Stream ExcelFileStream, int HeaderRowIndex)
        {
            HSSFWorkbook workbook = new HSSFWorkbook(ExcelFileStream);
            DataSet ds = new DataSet();
            for (int x = 0; x < workbook.Count; x++)
            {
                //ISheet sheet = workbook.GetSheetAt(x);
                //DataTable table = new DataTable();
                //IRow headerRow = sheet.GetRow(HeaderRowIndex);
                //int cellCount = headerRow.LastCellNum;

                ISheet sheet = workbook.GetSheetAt(x);
                DataTable table = new DataTable();
                table.TableName = sheet.SheetName;
                IRow headerRow = sheet.GetRow(HeaderRowIndex);
                int cellCount = headerRow.LastCellNum;

                for (int i = headerRow.FirstCellNum; i < cellCount; i++)
                {
                    DataColumn column = new
                    DataColumn(headerRow.GetCell(i).StringCellValue);
                    table.Columns.Add(column);
                }

                int rowCount = sheet.LastRowNum;
                for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                {
                    IRow row = sheet.GetRow(i);

                    DataRow dataRow = table.NewRow();
                    if (row != null)
                    {
                        for (int j = row.FirstCellNum; j < cellCount; j++)
                        {
                            if (row.FirstCellNum != -1)
                            {

                                if (row.GetCell(j) != null)
                                {
                                    if (row.GetCell(j).CellType == CellType.Numeric)
                                    {
                                        if (HSSFDateUtil.IsCellDateFormatted(row.GetCell(j)))
                                        {
                                            dataRow[j] = row.GetCell(j).DateCellValue;
                                        }
                                        else
                                        {
                                            dataRow[j] = row.GetCell(j);
                                        }
                                    }
                                    else
                                        dataRow[j] = GetCellValue(row.GetCell(j));
                                }

                            }
                        }
                    }
                    table.Rows.Add(dataRow);
                }
                //workbook = null;
                //sheet = null;
                ds.Tables.Add(table);
            }
            ExcelFileStream.Close();
            return ds;
        }
        #endregion

        #region  支持.xlsx格式文件
        public static DataSet RenderDataSetFromExcelXlsx(Stream ExcelFileStream, int HeaderRowIndex)
        {
            XSSFWorkbook workbook = new XSSFWorkbook(ExcelFileStream);
            DataSet ds = new DataSet();
            for (int x = 0; x < workbook.Count; x++)
            {
                ISheet sheet = workbook.GetSheetAt(x);
                DataTable table = new DataTable();
                table.TableName = sheet.SheetName;
                IRow headerRow = sheet.GetRow(HeaderRowIndex);
                int cellCount = headerRow.LastCellNum;
                for (int i = headerRow.FirstCellNum; i < cellCount; i++)
                {
                    DataColumn column = new
                    DataColumn(headerRow.GetCell(i).StringCellValue);
                    table.Columns.Add(column);
                }

                int rowCount = sheet.LastRowNum;
                for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                {
                    IRow row = sheet.GetRow(i);



                    DataRow dataRow = table.NewRow();
                    if (row != null)
                    {
                        for (int j = row.FirstCellNum; j < cellCount; j++)
                        {
                            if (row.FirstCellNum != -1)
                            {

                                if (row.GetCell(j) != null)
                                {
                                    if (row.GetCell(j).CellType == CellType.Numeric)
                                    {
                                        if (HSSFDateUtil.IsCellDateFormatted(row.GetCell(j)))
                                        {
                                            dataRow[j] = row.GetCell(j).DateCellValue;
                                        }
                                        else
                                        {
                                            dataRow[j] = row.GetCell(j);
                                        }
                                    }
                                    else
                                        dataRow[j] = GetCellValue(row.GetCell(j));
                                }

                            }
                        }
                    }
                    table.Rows.Add(dataRow);
                }
                ds.Tables.Add(table);
                //workbook = null;
                //sheet = null;
            }
            ExcelFileStream.Close();

            return ds;
        }
        #endregion


        /// <summary>
        /// 根据Excel列类型获取列的值
        /// </summary>
        /// <param name="cell">Excel列</param>
        /// <returns></returns>
        private static string GetCellValue(ICell cell)
        {
            if (cell == null) { return string.Empty; }
            switch (cell.CellType)
            {
                case CellType.Blank:
                    return string.Empty;
                case CellType.Boolean:
                    return cell.BooleanCellValue.ToString();
                case CellType.Error:
                    return cell.ErrorCellValue.ToString();
                case CellType.Numeric:
                case CellType.Unknown:
                default:
                    return ToChineseConvertTraditional(cell.ToString());//This is a trick to get the correct value of the cell. NumericCellValue will return a numeric value no matter the cell value is a date or a number
                case CellType.String:
                    return ToChineseConvertTraditional(cell.StringCellValue);
                case CellType.Formula:
                    try
                    {
                        HSSFFormulaEvaluator e = new HSSFFormulaEvaluator(cell.Sheet.Workbook);
                        e.EvaluateInCell(cell);
                        return cell.ToString();
                    }
                    catch
                    {
                        return cell.NumericCellValue.ToString();
                    }
            }
        }
        /// <summary>
        /// 设置单元格样式
        /// </summary>
        /// <param name="workbook"></param>
        /// <param name="cell"></param>
        private static void setCellStyle(HSSFWorkbook workbook, ICell cell)
        {
            HSSFCellStyle fCellStyle = (HSSFCellStyle)workbook.CreateCellStyle();
            HSSFFont ffont = (HSSFFont)workbook.CreateFont();
            ffont.FontHeight = 20 * 20;
            ffont.FontName = "宋体";
            ffont.Color = HSSFColor.Red.Index;
            fCellStyle.SetFont(ffont);

            fCellStyle.VerticalAlignment = VerticalAlignment.Center;//垂直对齐
            fCellStyle.Alignment = HorizontalAlignment.Center;//水平对齐
            cell.CellStyle = fCellStyle;
        }

        /// <summary>
        /// DataTable到服务器
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="tableName"></param>
        public static void DataTableToServer(DataTable dt, string tableName)
        {
            string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection destinationConnection = new SqlConnection(connStr))
            {
                destinationConnection.Open();

                using (SqlTransaction transaction = destinationConnection.BeginTransaction())
                {
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(destinationConnection, SqlBulkCopyOptions.Default,
                               transaction))
                    {
                        SqlCommand cmd = new SqlCommand(); //第四:声明 SqlCommand
                        cmd.Connection = destinationConnection;
                        cmd.Transaction = transaction;
                        //string strCmdDel = "";
                        //先删除之前的表数据
                        //strCmdDel = string.Format("truncate table {0}", tableName);
                        //cmd.CommandText = strCmdDel.ToString();
                        //cmd.Parameters.Clear();
                        //cmd.ExecuteScalar();
                        bulkCopy.BatchSize = dt.Rows.Count;
                        bulkCopy.DestinationTableName = tableName;

                        try
                        {
                            bulkCopy.WriteToServer(dt);
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw new Exception(ex.Message);
                        }
                        finally
                        {
                            dt.Clear();
                        }
                    }
                }
            }
        }


        /// <summary>
        /// 简体转换繁体
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToChineseConvertTraditional(string str)
        {
            return (Microsoft.VisualBasic.Strings.StrConv(str as String, Microsoft.VisualBasic.VbStrConv.TraditionalChinese, 2052));
        }


        /// <summary>
        /// 繁体转换为简体
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToTraditionalConvertChinese(string str)
        {
            return (Microsoft.VisualBasic.Strings.StrConv(str as String, Microsoft.VisualBasic.VbStrConv.SimplifiedChinese, 0));
        }

        #endregion


        #region  将excel文件内容读取到DataTable数据表中/将文件流读取到DataTable数据表中
        /// <summary>
        /// 将文件流读取到DataTable数据表中
        /// </summary>
        /// <param name="fileStream">文件流</param>
        /// <param name="sheetName">指定读取excel工作薄sheet的名称</param>
        /// <param name="isFirstRowColumn">第一行是否是DataTable的列名：true=是，false=否</param>
        /// <returns>DataTable数据表</returns>
        public static DataTable ReadStreamToDataTable(Stream fileStream, string sheetName = null, bool isFirstRowColumn = true)
        {
            //定义要返回的datatable对象
            DataTable data = new DataTable();
            //excel工作表
            ISheet sheet = null;
            //数据开始行(排除标题行)
            int startRow = 0;
            try
            {
                //根据文件流创建excel数据结构,NPOI的工厂类WorkbookFactory会自动识别excel版本，创建出不同的excel数据结构
                IWorkbook workbook = WorkbookFactory.Create(fileStream);
                //如果有指定工作表名称
                if (!string.IsNullOrEmpty(sheetName))
                {
                    sheet = workbook.GetSheet(sheetName);
                    //如果没有找到指定的sheetName对应的sheet，则尝试获取第一个sheet
                    if (sheet == null)
                    {
                        sheet = workbook.GetSheetAt(0);
                    }
                }
                else
                {
                    //如果没有指定的sheetName，则尝试获取第一个sheet
                    sheet = workbook.GetSheetAt(0);
                }
                if (sheet != null)
                {
                    IRow firstRow = sheet.GetRow(0);
                    //一行最后一个cell的编号 即总的列数
                    int cellCount = firstRow.LastCellNum;
                    //如果第一行是标题列名
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
                        if (row == null || row.FirstCellNum < 0) continue; //没有数据的行默认是null

                        DataRow dataRow = data.NewRow();
                        for (int j = row.FirstCellNum; j < cellCount; ++j)
                        {
                            //同理，没有数据的单元格都默认是null
                            ICell cell = row.GetCell(j);
                            if (cell != null)
                            {
                                if (cell.CellType == CellType.Numeric)
                                {
                                    //判断是否日期类型
                                    if (DateUtil.IsCellDateFormatted(cell))
                                    {
                                        dataRow[j] = row.GetCell(j).DateCellValue;
                                    }
                                    else
                                    {
                                        dataRow[j] = row.GetCell(j).ToString().Trim();
                                    }
                                }
                                else
                                {
                                    dataRow[j] = row.GetCell(j).ToString().Trim();
                                }
                            }
                        }
                        data.Rows.Add(dataRow);
                    }
                }
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 将excel文件内容读取到DataTable数据表中
        /// </summary>
        /// <param name="fileName">文件完整路径名</param>
        /// <param name="sheetName">指定读取excel工作薄sheet的名称</param>
        /// <param name="isFirstRowColumn">第一行是否是DataTable的列名：true=是，false=否</param>
        /// <returns>DataTable数据表</returns>
        public static DataTable ReadExcelToDataTable(string fileName, string sheetName = null, bool isFirstRowColumn = true)
        {
            //定义要返回的datatable对象
            DataTable data = new DataTable();
            //excel工作表
            ISheet sheet = null;
            //数据开始行(排除标题行)
            int startRow = 0;
            try
            {
                if (!File.Exists(fileName))
                {
                    return null;
                }
                //根据指定路径读取文件
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                //根据文件流创建excel数据结构
                IWorkbook workbook = WorkbookFactory.Create(fs);
                //IWorkbook workbook = new HSSFWorkbook(fs);
                //如果有指定工作表名称
                if (!string.IsNullOrEmpty(sheetName))
                {
                    sheet = workbook.GetSheet(sheetName);
                    //如果没有找到指定的sheetName对应的sheet，则尝试获取第一个sheet
                    if (sheet == null)
                    {
                        sheet = workbook.GetSheetAt(0);
                    }
                }
                else
                {
                    //如果没有指定的sheetName，则尝试获取第一个sheet
                    sheet = workbook.GetSheetAt(0);
                }
                if (sheet != null)
                {
                    IRow firstRow = sheet.GetRow(0);
                    //一行最后一个cell的编号 即总的列数
                    int cellCount = firstRow.LastCellNum;
                    //如果第一行是标题列名
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
                throw ex;
            }
        }
        #endregion


        #region  ExcelHelper
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
            DataTable schemaTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[]
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
        public static void DataTableToExcel(DataTable dt, string file_name, string sheet_name)
        {
            Microsoft.Office.Interop.Excel.Application Myxls = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook Mywkb = Myxls.Workbooks.Add();
            Microsoft.Office.Interop.Excel.Worksheet MySht = (Microsoft.Office.Interop.Excel.Worksheet)Mywkb.ActiveSheet;
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
                Console.WriteLine(ex.Message);
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
                        Console.WriteLine("结束当前EXCEL进程失败：" + ex.Message);
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
        public static DataTable ExcelToDataTableByAspose(string fileFullPath, bool HDR)
        {
            Aspose.Cells.Workbook workbook = new Aspose.Cells.Workbook(fileFullPath);
            Aspose.Cells.Worksheet worksheet = workbook.Worksheets[0];
            Aspose.Cells.Cells cells = worksheet.Cells;

            //该方法得到的表格中，如果存在单元格内容第一个字符为0的纯数字字符串会忽略0
            DataTable dataTable = cells.ExportDataTable(0, 0, cells.MaxDataRow + 1, cells.MaxDataColumn + 1, HDR);

            //该方法得到的表格中，如果存在单元格内容为10个以上的纯数字会将其变成科学记数法
            //DataTable dataTable2= cells.ExportDataTableAsString(0, 0, cells.MaxDataRow + 1, cells.MaxDataColumn + 1, HDR);
            workbook = null;
            worksheet = null;
            cells = null;
            GC.Collect();
            return dataTable;
        }

        /// <summary>
        /// 使用NPOI的组件来获取Excel数据到DataTable对象中
        /// </summary>
        /// <param name="filePath">文件全路径</param>
        /// <param name="isFirstRowColumn">第一行是否为列名称</param>
        /// <returns>DataTable对象</returns>
        public static DataTable ExcelToDataTableByNpoi(string filePath, bool isFirstRowColumn)
        {
            string sheetName = "sheet";
            ISheet sheet = null;
            DataTable data = new DataTable();
            int startRow = 0;
            try
            {
                IWorkbook workbook = null;
                FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);

                if (Path.GetExtension(filePath).ToLower() == ".xlsx") // 2007版本
                    try
                    {
                        workbook = new XSSFWorkbook(fs);
                    }
                    catch
                    {
                        fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                        workbook = new HSSFWorkbook(fs);
                    }
                else if (Path.GetExtension(filePath).ToLower() == ".xls") // 2003版本
                    try
                    {
                        workbook = new HSSFWorkbook(fs);
                    }
                    catch
                    {
                        fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                        workbook = new XSSFWorkbook(fs);
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

                //赋值名
                data.TableName = sheet.SheetName;

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

        #region NPOI DataTable2Excel
        public static void ExportExcel(DataTable table, string path)
        {
            byte[] data = DataTable2Excel(table, table.TableName?.Length > 0 ? table.TableName : "VTable");
            if (File.Exists(path)) File.Delete(path);
            FileStream fs = new FileStream(path, FileMode.CreateNew);
            fs.Write(data, 0, data.Length);
            fs.Close();
            fs.Dispose();
        }

        /// <summary>
        /// 将DataTable转换为excel2003格式。
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static byte[] DataTable2Excel(DataTable dt, string sheetName)
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
        public static MemoryStream DataTable2ExcelRetrunMemoryStream(DataTable dt, string sheetName)
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

        private static void DataWrite2Sheet(DataTable dt, int startRow, int endRow, IWorkbook book, string sheetName)
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
        #endregion
    }
}
