using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.EnterpriseServices;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace SPText.Common.DataHelper.Sql
{

    public partial class SqlHelper
    {
        private static readonly string conStr = ConfigurationManager.ConnectionStrings["DataContext"].ConnectionString;

        //insert delete update
        public static int ExecuteNonQuery(string sql, CommandType cmdType, params SqlParameter[] pms)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.CommandType = cmdType;
                    if (pms != null)
                    {
                        cmd.Parameters.AddRange(pms);
                    }
                    con.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        //返回单个值
        public static object ExecuteScalar(string sql, CommandType cmdType, params SqlParameter[] pms)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.CommandType = cmdType;
                    if (pms != null)
                    {
                        cmd.Parameters.AddRange(pms);
                    }
                    con.Open();
                    return cmd.ExecuteScalar();
                }
            }
        }

        //执行返回DataReader
        public static SqlDataReader ExecuteReader(string sql, CommandType cmdType, params SqlParameter[] pms)
        {
            SqlConnection con = new SqlConnection(conStr);
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                cmd.CommandType = cmdType;
                if (pms != null)
                {
                    cmd.Parameters.AddRange(pms);
                }
                //con.Open();
                try
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    return cmd.ExecuteReader(CommandBehavior.CloseConnection);
                }
                catch
                {
                    con.Close();
                    con.Dispose();
                    throw;
                }
            }
        }

        public static DataTable ExecuteDataTable(string sql, CommandType cmdType, params SqlParameter[] pms)
        {
            DataTable dt = new DataTable();
            using (SqlDataAdapter adapter = new SqlDataAdapter(sql, conStr))
            {
                adapter.SelectCommand.CommandType = cmdType;
                if (pms != null)
                {
                    adapter.SelectCommand.Parameters.AddRange(pms);
                }
                adapter.Fill(dt);
            }
            return dt;
        }

        public static DataSet ExecuteDataSet(string sql, CommandType cmdType)
        {
            DataSet ds = new DataSet();
            using (SqlDataAdapter adapter = new SqlDataAdapter(sql, conStr))
            {
                adapter.SelectCommand.CommandType = cmdType;
                adapter.Fill(ds);
            }
            return ds;
        }



        /// <summary>
        /// 使用SqlBulkCopy类批量复制大数据
        /// </summary>
        /// <param name="connectionString">目标连接字符</param>
        /// <param name="TableName">目标表</param>
        /// <param name="dt">源数据</param>
        private static void SqlBulkCopyByDatatable(string conStr, string TableName, DataTable dt)
        {
            using (SqlConnection conn = new SqlConnection(conStr))
            {
                using (SqlBulkCopy sqlbulkcopy = new SqlBulkCopy(conStr, SqlBulkCopyOptions.UseInternalTransaction))
                {
                    try
                    {
                        sqlbulkcopy.DestinationTableName = TableName;//服务器上目标表的名称。
                        //sqlbulkcopy.BulkCopyTimeout = 100;//超时之前操作完成所允许的秒数。
                        //sqlbulkcopy.BatchSize = 0;//每一批次中的行数。在每一批次结束时，将该批次中的行发送到服务器。
                        //sqlbulkcopy.NotifyAfter = 8000;  //每8千条事件触发一次
                        //sqlbulkcopy.SqlRowsCopied += new SqlRowsCopiedEventHandler((object sender, SqlRowsCopiedEventArgs e) =>
                        //{
                        //    Console.Write("已导入完毕八千条数据！");
                        //});
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            sqlbulkcopy.ColumnMappings.Add(dt.Columns[i].ColumnName, dt.Columns[i].ColumnName);
                        }
                        sqlbulkcopy.WriteToServer(dt);
                    }
                    catch (System.Exception ex)
                    {
                        throw ex;
                    }
                }
            }
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
    }


    #region  DTC(Distributed Transaction Coordinator) 分布式事务处理（常用）
    public class SqlHelperTransaction2
    {
        SqlConnection conn1 = new SqlConnection("data source=.;Initial Catalog=DataMedicine;Integrated Security=SSPI");
        SqlConnection conn2 = new SqlConnection("data source=.;Initial Catalog=DataMedicine;Integrated Security=SSPI");
        CommittableTransaction committran = new CommittableTransaction();
        public SqlHelperTransaction2()
        {
            DisplayTransactioninfo.Display(committran);
            conn1.Open();
            conn1.EnlistTransaction(committran);
            conn2.Open();
            conn2.EnlistTransaction(committran);
            DisplayTransactioninfo.Display(committran);
        }
        public void Add4()
        {
            try
            {
                SqlCommand command1 = new SqlCommand("insert into test2 values(111)", conn1);
                command1.ExecuteNonQuery();
                SqlCommand command2 = new SqlCommand("insert into test values(222)", conn2);
                command2.ExecuteNonQuery();
                committran.Commit();
            }
            catch (Exception err)
            {
                Console.WriteLine(err);
                committran.Rollback();
            }
        }
    }


    #endregion

    #region  EnterpriseService(COM+)自动化事务处理
    [System.Runtime.InteropServices.ComVisible(true)]
    //COM+是在COM的基础上发展起来的，需要将.NET程序集中的类型公开为COM组件。  
    [System.EnterpriseServices.Transaction(TransactionOption.Required)]//始终需要事务处理域  
    public class SqlHelperTransaction : ServicedComponent
    {
        public SqlHelperTransaction() { }
        SqlConnection conn = new SqlConnection("data source=.;Initial Catalog=DataMedicine;Integrated Security=SSPI");
        [AutoComplete(true)]
        //如果在方法的执行体类没有出现错误，那么将自动设置事务处理的结果  
        public void Add2()
        {
            conn.Open();
            SqlCommand command = new SqlCommand();
            try
            {
                command.Connection = conn;
                command.CommandText = "insert into test2 values(111)";
                command.ExecuteNonQuery();
                command.CommandText = "insert into test values(222)";
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                System.EnterpriseServices.ContextUtil.SetAbort();
            }
        }
    }
    #endregion

    #region  DependentTransaction跨线程事务处理
    public class SqlHelperTransaction1
    {
        CommittableTransaction commit = new CommittableTransaction();
        SqlConnection conn1 = new SqlConnection("data source=.;Initial Catalog=DataMedicine;Integrated Security=SSPI");
        public SqlHelperTransaction1()
        {
            conn1.Open();
            conn1.EnlistTransaction(commit);
        }
        public void Add6()
        {
            try
            {
                DisplayTransactioninfo.Display(commit);
                SqlCommand command = new SqlCommand("insert into test2 values(111)", conn1);
                command.ExecuteNonQuery();
                Thread thread = new Thread(SqlHelperTransaction1.CommitThread);
                thread.Start(commit.DependentClone(DependentCloneOption.BlockCommitUntilComplete));
                commit.Commit();
            }
            catch (Exception err)
            {
                commit.Rollback();
            }
        }
        public static void CommitThread(object co)
        {
            DependentTransaction commit = co as DependentTransaction;
            SqlConnection conn2 = new SqlConnection("data source=.;Initial Catalog=DataMedicine;Integrated Security=SSPI");
            conn2.Open();
            conn2.EnlistTransaction(commit as DependentTransaction);
            DisplayTransactioninfo.Display(commit);
            SqlCommand command = new SqlCommand("insert into test values(111)", conn2);
            try
            {
                command.ExecuteNonQuery();
                commit.Complete();
            }
            catch (Exception err) { Console.WriteLine(err); commit.Rollback(); }
        }


    }
    #endregion

    public class DisplayTransactioninfo
    {
        public static void Display(System.Transactions.Transaction tr)
        {
            if (tr != null)
            {
                Console.WriteLine("Createtime:" + tr.TransactionInformation.CreationTime);
                Console.WriteLine("Status:" + tr.TransactionInformation.Status);
                Console.WriteLine("Local ID:" + tr.TransactionInformation.LocalIdentifier);
                Console.WriteLine("Distributed ID:" + tr.TransactionInformation.DistributedIdentifier);
                Console.WriteLine();
            }
        }
    }

    public partial class SqlHelper
    {
        public SqlHelper(string dbHost, string dbName, string dbUser, string dbPwd)
        {
            connectionString = "Data Source=" + dbHost + ";Initial Catalog=" + dbName + ";User Id=" + dbUser + ";Password=" + dbPwd + ";";
            this.DbHost = dbHost;
            this.DbName = dbName;
            this.DbUser = dbUser;
            this.DbPwd = dbPwd;
        }

        public string connectionString;
        public string DbHost { get; set; }
        public string DbName { get; set; }
        public string DbUser { get; set; }
        public string DbPwd { get; set; }

        public DataSet Query(string sql, params SqlParameter[] parameters)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.AddRange(parameters);
                        cmd.CommandTimeout = 0;
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataSet dataSet = new DataSet();
                            adapter.Fill(dataSet);
                            return dataSet;
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
                //    if (ex.Message == "在向服务器发送请求时发生传输级错误。 (provider: TCP Provider, error: 0 - 远程主机强迫关闭了一个现有的连接。)" || ex.Message.ToString() == "A transport-level error has occurred when receiving results from the server. (provider: TCP Provider, error: 0 - The semaphore timeout period has expired.)")
                //    {
                //        return Query(sql, parameters);
                //    }
                //    else
                //    {
                //        throw;
                //    }
            }
        }

        public DataSet StoreProcedure(string spName, params SqlParameter[] parameters)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(spName, conn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddRange(parameters);
                        cmd.CommandTimeout = 0;
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataSet dataSet = new DataSet();
                            adapter.Fill(dataSet);
                            cmd.Parameters.Clear();

                            return dataSet;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public int ExecuteNonQuery(string sql, params SqlParameter[] sp)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        if (sp != null)
                        {
                            cmd.Parameters.AddRange(sp);
                        }
                        cmd.CommandTimeout = 0;
                        conn.Open();
                        return cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public object ExecuteScalar(string sql, params SqlParameter[] sp)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        if (sp != null)
                        {
                            cmd.Parameters.AddRange(sp);
                        }
                        cmd.CommandTimeout = 0;
                        conn.Open();
                        return cmd.ExecuteScalar();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public partial class SqlHelper
    {
        private static readonly string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;

        public static DataSet GetDataSet(string sql, params SqlParameter[] sqlParameters)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    if (sqlParameters != null && sqlParameters.Count() > 0)
                    {
                        cmd.Parameters.AddRange(sqlParameters);
                    }
                    cmd.CommandTimeout = 0;
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataSet dataSet = new DataSet();
                        adapter.Fill(dataSet);
                        return dataSet;
                    }
                }
            }
        }

        public static int ExecuteNotQuery(string sql, params SqlParameter[] sqlParameters)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddRange(sqlParameters);
                    cmd.CommandTimeout = 0;
                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        public static object ExecuteObject(string sql, params SqlParameter[] sqlParameters)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    if (sqlParameters.Count() > 0)
                    {
                        cmd.Parameters.AddRange(sqlParameters);
                    }
                    cmd.CommandTimeout = 0;
                    conn.Open();
                    return cmd.ExecuteScalar();
                }
            }
        }

        public static DataSet ExecuteProcedure(string procedure, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand(procedure, conn))
                {
                    if (parameters.Count() > 0)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    cmd.CommandTimeout = 0;
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataSet dataSet = new DataSet();
                        adapter.Fill(dataSet);
                        return dataSet;
                    }
                }
            }
        }

        public static bool ExecuteProcedureNotGetData(string sql, params SqlParameter[] parameters)
        {
            int flag = -2;
            bool reslut = false;
            SqlConnection conn = new SqlConnection(connStr);
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = conn;
            sqlCommand.Connection.Open();
            sqlCommand.CommandText = sql;
            sqlCommand.CommandType = CommandType.StoredProcedure;
            try
            {
                if (parameters != null)
                {
                    sqlCommand.Parameters.AddRange(parameters);
                }
                flag = sqlCommand.ExecuteNonQuery();
                if (flag <= 0)
                {
                    reslut = false;
                }
                else
                {
                    reslut = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sqlCommand.Connection.Close();
            }
            return reslut;
        }
    }
}
