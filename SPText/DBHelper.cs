using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;


namespace SPText
{
    public class DBHelper
    {
        public static string strConn = ConfigurationManager.AppSettings["DefaultConnection"];

        public int ExecuteNonQuery(string sql, CommandType commandType, params SqlParameter[] sqlParameters)
        {
            using (SqlConnection conn = new SqlConnection(strConn))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = commandType;
                    if (sqlParameters != null)
                    {
                        cmd.Parameters.AddRange(sqlParameters);
                    }
                    conn.Open();
                    int i = cmd.ExecuteNonQuery();


                    if (i == 0)
                        throw new Exception("发生错误！");
                    return i;
                };
            };
        }
        public object ExecuteScalar(string sql, CommandType commandType, params SqlParameter[] sqlParameters)
        {
            using (SqlConnection conn = new SqlConnection(strConn))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = commandType;
                    if (sqlParameters != null)
                    {
                        cmd.Parameters.AddRange(sqlParameters);
                    }
                    conn.Open();
                    object i = cmd.ExecuteScalar();

                    return i;
                };
            }
        }


        public DataTable DataTable(string sql, CommandType commandType, params SqlParameter[] sqlParameters)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(strConn))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand.CommandType = commandType;
                    if (sqlParameters != null)
                    {
                        da.SelectCommand.Parameters.AddRange(sqlParameters);
                    }
                    da.Fill(dt);
                    return dt;
                };
            }
        }

        public DataSet DataSet(string sql, CommandType commandType, params SqlParameter[] sqlParameters)
        {
            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(strConn))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand.CommandType = commandType;
                    if (sqlParameters != null)
                    {
                        da.SelectCommand.Parameters.AddRange(sqlParameters);
                    }
                    da.Fill(ds);
                    return ds;
                };
            }
        }

        //执行返回DataReader
        public static SqlDataReader ExecuteReader(string sql, CommandType cmdType, params SqlParameter[] pms)
        {
            SqlConnection con = new SqlConnection(strConn);
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
    }
}
