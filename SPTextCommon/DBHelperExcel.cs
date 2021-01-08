using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPTextCommon
{
    public class DBHelperExcel
    {
        #region 变量
        //Excel 2007文件
        private static string _connStringmdf = string.Empty;
        private static string strmdf = @"Provider=Microsoft.ACE.OLEDB.12.0;data source={0};";

        private static string _connString2007 = string.Empty;
        private static string str2007 = @"Provider=Microsoft.ACE.OLEDB.12.0;data source={0};Extended Properties='Excel 12.0;HDR=Yes;IMEX=1'";

        //Excel 2003文件
        private static string _connString2003 = string.Empty;
        private static string str2003 = @"Provider=Microsoft.Jet.OleDb.4.0;data source={0};Extended Properties='Excel 8.0;HDR=Yes;IMEX=1'";

        #endregion

        #region 连接excel对象
        /// <summary>
        /// 连接excel对象
        /// </summary>
        /// <param name="dbName"></param>
        public OleDbConnection OpenExcelDB(string dbName)
        {
            FileInfo fileInfo = new FileInfo(dbName);
            if (fileInfo.Exists)
            {
                switch (fileInfo.Extension.ToLower())
                {
                    case ".xls":
                        _connString2003 = string.Format(str2003, dbName);
                        return new OleDbConnection(_connString2003);
                    case ".xlsx":
                        _connString2007 = string.Format(str2007, dbName);
                        return new OleDbConnection(_connString2007);
                    case ".mdb":
                        _connStringmdf = string.Format(strmdf, dbName);
                        return new OleDbConnection(_connStringmdf);
                }

            }
            throw new NotImplementedException();
        }
        #endregion

        //public static void CloseExcelDB()
        //{
        //    if (_oleDBConn != null && _oleDBConn.State == ConnectionState.Open)
        //    {
        //        //打开连接
        //        _oleDBConn.Close();
        //    }
        //    Dispose();
        //}

        //public static void Dispose()
        //{
        //    if (_oleDBConn != null)
        //    {
        //        _oleDBConn.Dispose();
        //    }
        //}

        #region 获取数据集合
        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <returns>返回查询到的监测水位集合</returns>
        public DataSet GetDataSet(string dbName)
        {
            DataSet objDataset1 = new DataSet();
            try
            {
                using (var _oleDBConn = OpenExcelDB(dbName))
                {
                    //打开连接
                    _oleDBConn.Open();

                    DataTable schemaTable = _oleDBConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });

                    OleDbCommand objCmdSelect = new OleDbCommand();

                    OleDbDataAdapter objAdapter1 = new OleDbDataAdapter();

                    for (int i = 0; i < schemaTable.Rows.Count; i++)
                    {
                        DataRow dr = schemaTable.Rows[i];
                        objCmdSelect.CommandText = string.Format("SELECT * FROM [{0}]", dr["TABLE_NAME"].ToString().Trim('\''));
                        objCmdSelect.Connection = _oleDBConn;
                        objAdapter1.SelectCommand = objCmdSelect;

                        //将Excel中数据填充到数据集
                        objAdapter1.Fill(objDataset1);
                        objDataset1.Tables[i].TableName = dr["TABLE_NAME"].ToString().TrimEnd('$');
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objDataset1;
        }
        #endregion

        //#region 根据工作表名称，获取数据
        ///// <summary>
        ///// 根据工作表名称，获取数据
        ///// </summary>
        ///// <param name="tablename"></param>
        ///// <returns></returns>
        //public static DataTable GetDataTable(string tablename, int rowNum)
        //{
        //    DataTable newdt = new DataTable();
        //    DataTable dt = null;
        //    if (_oleDBConn != null)
        //    {
        //        try
        //        {
        //            //打开连接
        //            _oleDBConn.Open();

        //            OleDbCommand objCmdSelect = new OleDbCommand(string.Format("SELECT * FROM [{0}]", tablename), _oleDBConn);
        //            objCmdSelect.Connection = _oleDBConn;

        //            OleDbDataAdapter objAdapter1 = new OleDbDataAdapter();

        //            objAdapter1.SelectCommand = objCmdSelect;

        //            DataSet objDataset1 = new DataSet();

        //            //将Excel中数据填充到数据集
        //            objAdapter1.Fill(objDataset1);

        //            dt = objDataset1.Tables[0];
        //            //newdt = dt.Clone();
        //            if (dt != null && dt.Rows.Count > 0)
        //            {
        //                ////循环第一行,判断是否有值,有 保留,没有 去掉列
        //                //int row = 0;
        //                //for (int j = 0; j < dt.Columns.Count; j++)
        //                //{
        //                //    if (dt.Rows[row][j] == null || string.IsNullOrEmpty(dt.Rows[row][j].ToString()))
        //                //    {
        //                //        dt.Columns.RemoveAt(j);
        //                //        j--;
        //                //    }
        //                //}
        //                //newdt.Columns.Clear();

        //                if (rowNum > 0)
        //                {
        //                    for (int i = 0; i < dt.Columns.Count; i++)
        //                    {
        //                        newdt.Columns.Add(dt.Rows[0][i].ToString().Replace(" ", string.Empty), typeof(string));
        //                    }
        //                }
        //                else
        //                {
        //                    foreach (var item in dt.Columns)
        //                    {
        //                        newdt.Columns.Add(item.ToString(), typeof(string));
        //                    }
        //                }
        //                for (int i = rowNum; i < dt.Rows.Count; i++)
        //                {
        //                    newdt.Rows.Add(GetNewRows(newdt, dt, i));
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //        finally
        //        {
        //            CloseExcelDB();
        //        }
        //    }
        //    return newdt;
        //}

        //private static DataRow GetNewRows(DataTable newdt, DataTable dataTable, int i)
        //{
        //    DataRow row = newdt.NewRow();
        //    try
        //    {
        //        int y = 0;
        //        foreach (var item in dataTable.Rows[i].ItemArray)
        //        {
        //            if (item != null && !string.IsNullOrEmpty(item.ToString()))
        //            {
        //                row[y] = item.ToString();
        //            }
        //            y++;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return row;
        //}
        //#endregion

        //#region 根据工作表名称，获取数据
        ///// <summary>
        ///// 根据工作表名称，获取数据
        ///// </summary>
        ///// <param name="tablename"></param>
        ///// <returns></returns>
        //public static DataTable GetNewDataTable(string tablename)
        //{
        //    DataTable newdt = null;
        //    DataTable dt = null;
        //    if (_oleDBConn != null)
        //    {
        //        try
        //        {
        //            //打开连接
        //            _oleDBConn.Open();

        //            OleDbCommand objCmdSelect = new OleDbCommand(string.Format("SELECT * FROM [{0}]", tablename), _oleDBConn);
        //            objCmdSelect.Connection = _oleDBConn;

        //            OleDbDataAdapter objAdapter1 = new OleDbDataAdapter();

        //            objAdapter1.SelectCommand = objCmdSelect;

        //            DataSet objDataset1 = new DataSet();

        //            //将Excel中数据填充到数据集
        //            objAdapter1.Fill(objDataset1);

        //            dt = objDataset1.Tables[0];
        //            newdt = dt.Clone();
        //            if (dt != null && dt.Rows.Count > 0)
        //            {
        //                //循环第一行,判断是否有值,有 保留,没有 去掉列
        //                int row = 0;
        //                for (int j = 0; j < dt.Columns.Count; j++)
        //                {
        //                    if (dt.Rows[row][j] == null || string.IsNullOrEmpty(dt.Rows[row][j].ToString()))
        //                    {
        //                        dt.Columns.RemoveAt(j);
        //                        j--;
        //                    }
        //                }
        //                newdt.Columns.Clear();
        //                for (int i = 0; i < dt.Columns.Count; i++)
        //                {
        //                    newdt.Columns.Add(dt.Rows[0][i].ToString().Replace(" ", string.Empty), typeof(string));
        //                }
        //                for (int i = 1; i < dt.Rows.Count; i++)
        //                {
        //                    newdt.Rows.Add(GetNewRows(newdt, dt, i));
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //        finally
        //        {
        //            CloseExcelDB();
        //        }
        //    }
        //    return newdt;
        //}
        //#endregion

        //#region 获取工作表所有名称
        ///// <summary>
        ///// 获取工作表所有名称
        ///// </summary>
        ///// <returns></returns>
        //public static Dictionary<string, string> GetDataTableNames()
        //{
        //    Dictionary<string, string> _workSheetsDict = new Dictionary<string, string>();
        //    try
        //    {
        //        //打开连接
        //        _oleDBConn.Open();
        //        DataTable schemaTable = _oleDBConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });

        //        OleDbCommand objCmdSelect = new OleDbCommand();

        //        OleDbDataAdapter objAdapter1 = new OleDbDataAdapter();

        //        if (schemaTable != null && schemaTable.Rows.Count > 0)
        //        {
        //            for (int i = 0; i < schemaTable.Rows.Count; i++)
        //            {
        //                DataRow dr = schemaTable.Rows[i];
        //                _workSheetsDict.Add(dr["TABLE_NAME"].ToString().Replace("\'", string.Empty).TrimEnd('$'), dr["TABLE_NAME"].ToString().Replace("\'", string.Empty).TrimEnd('$'));
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        CloseExcelDB();
        //    }
        //    return _workSheetsDict;
        //}
        //#endregion

        //#region 根据工作表，获取excel第二列的数据
        ///// <summary>
        ///// 根据工作表，获取excel第二列的数据
        ///// </summary>
        ///// <param name="tablename"></param>
        ///// <returns></returns>
        //public static Dictionary<string, string> GetDataTableColumns(string tablename, int row = 0)
        //{
        //    Dictionary<string, string> _tableNamesDict = new Dictionary<string, string>();
        //    if (_oleDBConn != null)
        //    {
        //        try
        //        {
        //            //打开连接
        //            _oleDBConn.Open();

        //            OleDbCommand objCmdSelect = new OleDbCommand(string.Format("SELECT * FROM [{0}]", tablename), _oleDBConn);
        //            objCmdSelect.Connection = _oleDBConn;

        //            OleDbDataAdapter objAdapter = new OleDbDataAdapter();

        //            objAdapter.SelectCommand = objCmdSelect;

        //            DataSet objDataset = new DataSet();

        //            //将Excel中数据填充到数据集
        //            objAdapter.Fill(objDataset);

        //            if (objDataset != null && objDataset.Tables[0] != null && objDataset.Tables[0].Rows.Count > 0)
        //            {
        //                int i = 0;
        //                if (row > 0)
        //                {
        //                    //获取行
        //                    foreach (var item in objDataset.Tables[0].Rows[row - 1].ItemArray)
        //                    {
        //                        if (item != null && !string.IsNullOrWhiteSpace(item.ToString()))
        //                        {
        //                            _tableNamesDict.Add(i.ToString(), item.ToString().Replace(" ", string.Empty));
        //                        }
        //                        i++;
        //                    }
        //                }
        //                else
        //                {
        //                    //获取列
        //                    foreach (var item in objDataset.Tables[0].Columns)
        //                    {
        //                        if (item != null && !string.IsNullOrWhiteSpace(item.ToString()))
        //                        {
        //                            _tableNamesDict.Add(i.ToString(), item.ToString().Replace(" ", string.Empty));
        //                        }
        //                        i++;
        //                    }
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //        finally
        //        {
        //            CloseExcelDB();
        //        }
        //    }
        //    return _tableNamesDict;
        //}
        //#endregion

        //#region 将Datable中的数据导出到Excel表中
        /////// <summary>
        /////// 将Datable中的数据导出到Excel表中
        /////// </summary>
        /////// <param name="dt"></param>
        /////// <param name="path">地址</param>
        ////public static void DataTableWriteToExcel(DataTable dt, string path)
        ////{                
        ////    Microsoft.Office.Interop.Excel.Application xlapp = new Microsoft.Office.Interop.Excel.Application();
        ////    Microsoft.Office.Interop.Excel.Workbook xlbook = xlapp.Workbooks.Add(Type.Missing);
        ////    Microsoft.Office.Interop.Excel.Worksheet xlsheet = (Microsoft.Office.Interop.Excel.Worksheet)xlbook.ActiveSheet;
        ////    Microsoft.Office.Interop.Excel.Range xlrange;
        ////    try
        ////    {
        ////        if (dt != null && dt.Rows.Count > 0)
        ////        {                    
        ////            xlrange = xlsheet.get_Range("A1", IndexToColumn(dt.Columns.Count) + (dt.Rows.Count + 1));
        ////            xlrange.NumberFormat = "@";
        ////            for (int i = 0; i < dt.Columns.Count; i++) //行
        ////            {
        ////                xlsheet.Cells[1, i + 1] = dt.Columns[i].ColumnName;
        ////            }
        ////            for (int i = 0; i < dt.Rows.Count; i++) //行
        ////            {
        ////                for (int j = 0; j < dt.Columns.Count; j++) //列
        ////                {
        ////                    xlsheet.Cells[i + 2, j + 1] = dt.Rows[i][j].ToString();
        ////                }
        ////            }
        ////            xlbook.Saved = true;
        ////            //xlsheet.SaveAs(path, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
        ////            xlbook.SaveCopyAs(path);
        ////        }
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        throw ex;
        ////    }
        ////    finally
        ////    {
        ////        xlapp.Quit();
        ////        xlapp = null;
        ////        GC.Collect();
        ////        //KillProcess("EXCEL");
        ////    }
        ////}

        /////// <summary>
        /////// 清除excel内存
        /////// </summary>
        /////// <param name="processName"></param>
        ////private static void KillProcess(string processName)
        ////{
        ////    //System.Diagnostics.Process[] excelProc = System.Diagnostics.Process.GetProcessesByName("EXCEL");
        ////    //System.DateTime startTime = new DateTime();
        ////    //int i;
        ////    //int killId = 0;
        ////    //for (i = 0; i < excelProc.Length; i++)
        ////    //{
        ////    //    if (startTime < excelProc[i].StartTime)
        ////    //    {
        ////    //        startTime = excelProc[i].StartTime;
        ////    //        killId = i;
        ////    //    }
        ////    //}
        ////    //if (excelProc[i].HasExited == false)
        ////    //{
        ////    //    excelProc[killId].Kill();
        ////    //}

        ////    System.Diagnostics.Process[] myprocs = System.Diagnostics.Process.GetProcessesByName("EXCEL");
        ////    try
        ////    {
        ////        foreach (System.Diagnostics.Process item in myprocs)
        ////        {
        ////            if (item.CloseMainWindow())
        ////            {
        ////                item.Kill();   
        ////            }
        ////        }
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        throw ex;
        ////    }
        ////}        

        /////// <summary>
        /////// 根据数字获取相应的字母
        /////// </summary>
        /////// <param name="index"></param>
        /////// <returns></returns>
        ////private static string IndexToColumn(int index)
        ////{
        ////    try
        ////    {
        ////        if (index <= 0)
        ////        {
        ////            throw new Exception("索引不能小于等于0");
        ////        }
        ////        index--;
        ////        string column = string.Empty;
        ////        do
        ////        {
        ////            if (column.Length > 0)
        ////            {
        ////                index--;
        ////            }
        ////            column = ((char)(index % 26 + (int)'A')).ToString() + column;
        ////            index = (int)((index - index % 26) % 26);
        ////        } while (index > 0);
        ////        return column;
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        throw ex;
        ////    }
        ////}
        //#endregion
    }
}
