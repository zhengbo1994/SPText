using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SPCoreText.Common
{
    /// <summary>
    /// 静态类
    /// </summary>
    public partial class SqlHelper
    {
        private static readonly string connStr;

        static SqlHelper()
        {
            connStr = ConfigManager.GetConfig("ConnectionStrings:ConnStr");
        }

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


        #region 查询======>调用SQL语句
        public static DataTable GetDataTable(string sql, params SqlParameter[] parameters)
        {
            DataTable dt = new DataTable();
            SqlConnection conn = new SqlConnection(connStr);
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = conn;
            sqlCommand.Connection.Open();
            sqlCommand.CommandText = sql;
            sqlCommand.CommandType = CommandType.Text;
            if (parameters != null)
            {
                sqlCommand.Parameters.AddRange(parameters);
            }
            try
            {
                SqlDataAdapter da = new SqlDataAdapter(sqlCommand);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sqlCommand.Connection.Close();
            }
            return dt;
        }
        #endregion
    }
}
