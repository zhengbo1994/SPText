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

    public static class SqlHelper
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
            catch (Exception err) { 
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
}
