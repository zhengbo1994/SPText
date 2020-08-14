using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPTextWinForm
{
    /// <summary>
    /// 数据库帮助类
    /// </summary>
    public class DBHelper
    {

        private static string ConnStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        //private static string ConnStr = "server=.;uid=sa;pwd=123456;database=Customers";
        /// <summary>
        /// 执行添加、删除、修改的方法
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string sql, params SqlParameter[] paras)
        {
            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                //打开数据库连接
                conn.Open();
                //创建执行脚本的对象SQL
                SqlCommand command = new SqlCommand(sql, conn);
                command.Parameters.AddRange(paras);
                int result = command.ExecuteNonQuery();//执行
                return result;
            }
        }
        /// <summary>
        /// 执行SQL并返回第一行第一列
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public static object ExecuteScalar(string sql, params SqlParameter[] paras)
        {
            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(sql, conn);
                object obj = command.ExecuteScalar();

                return obj;
            }
        }
        /// <summary>
        /// 执行SQL返回SqlDataReader对象
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public static SqlDataReader ExecuteReader(string sql, params SqlParameter[] paras)
        {
            SqlConnection conn = new SqlConnection(ConnStr);
            conn.Open();
            SqlCommand command = new SqlCommand(sql, conn);
            command.Parameters.AddRange(paras);
            return command.ExecuteReader(CommandBehavior.CloseConnection);//命令行为

        }
        public static DataRow GetDataRow(string sql, params SqlParameter[] paras)
        {
            DataTable dt = null;
            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                SqlCommand command = new SqlCommand(sql, conn);
                command.Parameters.AddRange(paras);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                dt = new DataTable();
                adapter.Fill(dt);
            }
            if (dt.Rows.Count > 0)
                return dt.Rows[0];
            else
                return null;
        }
        public static DataTable GetDataTable(string sql, params SqlParameter[] paras)
        {
            DataTable dt = null;
            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                SqlCommand command = new SqlCommand(sql, conn);
                command.Parameters.AddRange(paras);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                dt = new DataTable();
                adapter.Fill(dt);
            }
            return dt;
        }
        public static DataSet GetDataSet(string sql, params SqlParameter[] paras)
        {
            DataSet ds = null;
            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                SqlCommand command = new SqlCommand(sql, conn);
                command.Parameters.AddRange(paras);
                //关联SqlServer 和 DataSet SqlCommandBuilder
                SqlDataAdapter adapter = new SqlDataAdapter(command);//适配器
                ds = new DataSet();
                adapter.Fill(ds);
            }
            return ds;
        }
        /// <summary>
        /// 获取适配器
        /// SqlCommandBuilder，SqlDataAdapter 组合使用
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public static SqlDataAdapter GetDataAdapter(string sql, params SqlParameter[] paras)
        {
            SqlConnection conn = new SqlConnection(ConnStr);
            SqlCommand command = new SqlCommand(sql, conn);
            command.Parameters.AddRange(paras);
            SqlDataAdapter adapter = new SqlDataAdapter(command);

            return adapter;
        }


    }
}
