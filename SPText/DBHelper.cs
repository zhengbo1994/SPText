using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Oracle.ManagedDataAccess.Client;
using System.Data.OleDb;
using System.Collections;
using SPText.Common.DataHelper.Other;

namespace SPText
{
    public partial class DBHelper
    {
        public static string strConn = ConfigurationManager.ConnectionStrings["DataContext"].ConnectionString;

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

        //insert into 表(列1, 列2,...) values(value1, value2,...) SELECT SCOPE_IDENTITY();  //返回主键Id
        public object ExecuteScalarA(string sql, CommandType commandType, params SqlParameter[] sqlParameters)
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

        public object GetInsertIdBySql(string sql, CommandType commandType, params SqlParameter[] sqlParameters)
        {
            using (SqlConnection conn = new SqlConnection(strConn))
            {
                sql = sql + " SELECT SCOPE_IDENTITY()";
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
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
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
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
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

    public partial class RunSqlRepository
    {
        public static string connectionString = ConfigurationManager.ConnectionStrings["DataContext"].ConnectionString;
        //执行192.168.80.100端口的数据操作
        public System.Data.DataTable runSql(string sql, params object[] pamrs)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand();
                PrepareCommand(cmd, connection, null, sql, pamrs);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();
                    try
                    {
                        da.Fill(ds, "ds");
                        cmd.Parameters.Clear();
                    }
                    catch (System.Data.SqlClient.SqlException ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    return ds.Tables[0];
                }
            }
        }

        public bool ExecuteCommand(string sql, string connectionString, params SqlParameter[] parameters)
        {
            return ExecuteCommandBase(sql, parameters, connectionString);
        }

        public bool ExecuteCommandByMDDB(string sql, string connectionString, params SqlParameter[] parameters)
        {
            return ExecuteCommandBase(sql, parameters, connectionString);
        }


        private static bool ExecuteCommandBase(string sql, SqlParameter[] parameters, string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand();
                PrepareCommand(cmd, connection, null, sql, parameters);
                return cmd.ExecuteNonQuery() > 0;
            }
        }



        public System.Data.DataTable runSql2(string sql, string connectionString, params object[] pamrs)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand();
                PrepareCommand(cmd, connection, null, sql, pamrs);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();
                    try
                    {
                        da.Fill(ds, "ds");
                        cmd.Parameters.Clear();
                    }
                    catch (System.Data.SqlClient.SqlException ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    return ds.Tables[0];
                }
            }
        }



        //执行存储过程
        public bool ExecuteProp(string sql, string connectionString, params SqlParameter[] parameters)
        {
            return ExecuteProcedure(sql, parameters, connectionString);
        }

        private bool ExecuteProcedure(string sql, SqlParameter[] parameters, string connectionString)
        {
            int flag = -2;
            bool reslut = false;
            SqlConnection conn = new SqlConnection(connectionString);
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

        public object ExecuteScalar(string sql, string connectionString, params SqlParameter[] param)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand();
                    PrepareCommand(cmd, connection, null, sql, param);
                    object result = cmd.ExecuteScalar();
                    return result;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }

        }

        //执行存储过程(返回表)
        public DataTable ExecutePropbyTable(string sql, string connectionString, params SqlParameter[] parameters)
        {
            return GetTableByProp(connectionString, sql, parameters);

        }

        //执行MDGLASS存储过程(返回表)
        public DataTable ExecutePropbyTableMDGLASS(string sql, string connectionString, params SqlParameter[] parameters)
        {
            return GetTableByProp(connectionString, sql, parameters);

        }

        private DataTable GetTableByProp(string connectionString, string sql, SqlParameter[] parameters)
        {

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sql, connection);
                DataSet dataSet = new DataSet();
                connection.Open();
                SqlDataAdapter sqlDA = new SqlDataAdapter();

                sqlDA.SelectCommand = BuildQueryCommand(connection, sql, parameters);

                sqlDA.Fill(dataSet, "ds");
                connection.Close();
                return dataSet.Tables[0];
            }
        }

        /// <summary>
        /// 构建 SqlCommand 对象(用来返回一个结果集，而不是一个整数值)
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>SqlCommand</returns>
        private static SqlCommand BuildQueryCommand(SqlConnection connection, string storedProcName, SqlParameter[] parameters)
        {
            SqlCommand command = new SqlCommand(storedProcName, connection);
            command.CommandType = CommandType.StoredProcedure;
            foreach (SqlParameter parameter in parameters)
            {
                if (parameter != null)
                {
                    // 检查未分配值的输出参数,将其分配以DBNull.Value.
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    command.Parameters.Add(parameter);
                }
            }

            return command;
        }

        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, string cmdText, object[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;//cmdType;
            if (cmdParms != null)
            {
                foreach (SqlParameter parameter in cmdParms)
                {
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(parameter);
                }
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

    #region  手写ORM
    public class CustomDBHelper
    {
        public static string strConn = ConfigurationManager.ConnectionStrings["DataContext"].ConnectionString;
        public T Find<T>(int Id)
        {
            Type type = typeof(T);
            string sql = (new SqlBuilder<T>()).GetSql(SqlType.SelectSql);//泛型缓存

            IEnumerable<SqlParameter> paraList = new List<SqlParameter>() { new SqlParameter("@Id", Id) };
            //string columnStrings = string.Join(",", type.GetProperties().Select(p => $"[{p.GetName()}]"));
            //string sql = string.Format($"SELECT {columnStrings} FROM [{type.GetName()}] WHERE ID={0}", Id);


            return this.ExecuteSql<T>(sql, paraList, cmd =>
            {
                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    T t = (T)Activator.CreateInstance(type);

                    foreach (var prop in type.GetProperties())
                    {
                        string propName = prop.GetName();
                        prop.SetValue(t, reader[propName] is DBNull ? null : reader[propName]);
                    }
                    return t;

                }
                else
                {
                    throw new Exception("获取数据失败！");
                }
            });

            //using (SqlConnection conn = new SqlConnection(strConn))
            //{
            //    SqlCommand cmd = new SqlCommand(sql, conn);
            //    cmd.Parameters.AddRange(paraList.ToArray());
            //    conn.Open();

            //    var reader = cmd.ExecuteReader();
            //    if (reader.Read())
            //    {
            //        T t = (T)Activator.CreateInstance(type);

            //        foreach (var prop in type.GetProperties())
            //        {
            //            string propName = prop.GetName();
            //            prop.SetValue(t, reader[propName] is DBNull ? null : reader[propName]);
            //            //prop.SetValue(t, reader[prop.GetName()] is DBNull ? null : reader[prop.GetName()]);
            //        }
            //        return t;

            //    }
            //    else
            //    {
            //        throw new Exception("获取数据失败！");
            //    }
            //}
        }

        public List<T> FindAll<T>()
        {
            Type type = typeof(T);
            string sql = (new SqlBuilder<T>()).GetSql(SqlType.SelectAllSql);//泛型缓存

            //string columnStrings = string.Join(",", type.GetProperties().Select(p => $"[{p.GetName()}]"));
            //string sql = string.Format($"SELECT {columnStrings} FROM [{type.GetName()}]");

            List<T> tList = new List<T>();
            using (SqlConnection conn = new SqlConnection(strConn))
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();

                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    T t = (T)Activator.CreateInstance(type);

                    foreach (var prop in type.GetProperties())
                    {
                        string propName = prop.GetName();
                        prop.SetValue(t, reader[propName] is DBNull ? null : reader[propName]);
                    }
                    tList.Add(t);
                }
                return tList;
            }


        }

        public int Update<T>(T model) where T : Text
        {
            Type type = typeof(T);

            string sql = (new SqlBuilder<T>()).GetSql(SqlType.UpdateSql + model.Id);//泛型缓存
            IEnumerable<SqlParameter> paraList = type.GetProperties().Select(p => new SqlParameter($"@{p.Name}", p.GetValue(model) ?? DBNull.Value));
            //IEnumerable<SqlParameter> paraList = new List<SqlParameter>() { new SqlParameter("@Id", model.Id) };

            //string columnStrings = string.Join(",", type.GetProperties().Where(p => !p.Name.Equals("Id")).Select(p => $"[{p.Name}]=@[{p.Name}]"));
            //IEnumerable<SqlParameter> pareList = type.GetProperties().Select(p => new SqlParameter($"@{p.Name}", p.GetValue(model) ?? DBNull.Value));
            //string sql = $"UPDATE {type.Name} Set {columnStrings} WHERE Id={model.Id}";


            if (AttributcMapping.ValiData(type))
            {
                throw new Exception("数据检验不通过！");
            }

            return this.ExecuteSql<int>(sql, paraList, cmd =>
            {
                int ireturn = cmd.ExecuteNonQuery();
                if (ireturn < 1)
                {
                    throw new Exception("更新失败！");
                }
                else
                {
                    return ireturn;
                }
            });

            //using (SqlConnection conn = new SqlConnection(strConn))
            //{
            //    SqlCommand cmd = new SqlCommand(sql, conn);
            //    cmd.Parameters.AddRange(paraList.ToArray());
            //    conn.Open();
            //    int ireturn = cmd.ExecuteNonQuery();
            //    if (ireturn < 1)
            //    {
            //        throw new Exception("更新失败！");
            //    }
            //    else
            //    {
            //        return ireturn;
            //    }
            //}
        }

        public int Insert<T>(T model)
        {
            //新增后拿ID  在slq后面价格 select @@identity; new SqlCommand().ExecuteScascalar
            Type type = typeof(T);
            string sql = (new SqlBuilder<T>()).GetSql(SqlType.InsertSql);//泛型缓存
            IEnumerable<SqlParameter> paraList = type.GetProperties().Select(p => new SqlParameter($"@{p.Name}", p.GetValue(model) ?? DBNull.Value));

            //string columnNameStrings = string.Join(",", type.GetProperties().Where(p => !p.Name.Equals("Id")).Select(p => $"[{p.Name}]]"));
            ////string columnValueStrings = string.Join(",", type.GetProperties().Where(p => !p.Name.Equals("Id")).Select(p => $"'{p.GetValue(model)}'"));//写死
            //string columnValueStrings = string.Join(",", type.GetProperties().Where(p => !p.Name.Equals("Id")).Select(p => $"@{p.Name}"));//参数化模式
            //IEnumerable<SqlParameter> pareList = type.GetProperties().Select(p => new SqlParameter($"@{p.Name}", p.GetValue(model) ?? DBNull.Value));
            //string sql = $"INSERT INTO {type.Name}({columnNameStrings}) VALUE {columnValueStrings} ";

            if (AttributcMapping.ValiData(type))
            {
                throw new Exception("数据检验不通过！");
            }

            return this.ExecuteSql<int>(sql, paraList, cmd =>
            {
                int ireturn = cmd.ExecuteNonQuery();
                if (ireturn < 1)
                {
                    throw new Exception("新增失败！");
                }
                else
                {
                    return ireturn;
                }
            });

            //using (SqlConnection conn = new SqlConnection(strConn))
            //{
            //    SqlCommand cmd = new SqlCommand(sql, conn);
            //    cmd.Parameters.AddRange(pareList.ToArray());
            //    conn.Open();
            //    int ireturn = cmd.ExecuteNonQuery();
            //    if (ireturn < 1)
            //    {
            //        throw new Exception("新增失败！");
            //    }
            //    else
            //    {
            //        return ireturn;
            //    }
            //}
        }

        public int Delete<T>(T model) where T : Text
        {
            Type type = typeof(T);
            string sql = new SqlBuilder<T>().GetSql(SqlType.DeleteSql);//泛型缓存

            IEnumerable<SqlParameter> paraList = new List<SqlParameter>() { new SqlParameter("@Id", model.Id) };

            return this.ExecuteSql<int>(sql, paraList, cmd =>
            {
                int ireturn = cmd.ExecuteNonQuery();
                if (ireturn < 1)
                {
                    throw new Exception("删除失败！");
                }
                else
                {
                    return ireturn;
                }
            });
        }

        public int Delete<T>(int Id) where T : Text, new()
        {
            return this.Delete<T>(new T
            {
                Id = Id
            });
        }

        public int DeleteAllByIdList<T>(IEnumerable<int> tList) where T : Text
        {
            Type type = typeof(T);

            string IdList = string.Join(",", tList.Select(p => p.ToString()));
            string sql = $"DELETE FROM {type.GetName()} WHERE Id in{IdList}";
            IEnumerable<SqlParameter> paraList = new List<SqlParameter>() { };

            return this.ExecuteSql<int>(sql, paraList, cmd =>
            {
                int ireturn = cmd.ExecuteNonQuery();
                if (ireturn < 1)
                {
                    throw new Exception("删除失败！");
                }
                else
                {
                    return ireturn;
                }
            });
        }

        public int SQLPara<T>(string sqlPara)
        {
            Type type = typeof(T);

            string sql = $"{sqlPara}";
            IEnumerable<SqlParameter> paraList = new List<SqlParameter>() { };

            return this.ExecuteSql<int>(sql, paraList, cmd =>
            {
                int ireturn = cmd.ExecuteNonQuery();
                if (ireturn < 1)
                {
                    throw new Exception("操作失败！");
                }
                else
                {
                    return ireturn;
                }
            });
        }

        public T ExecuteSql<T>(string sql, IEnumerable<SqlParameter> paraList, Func<SqlCommand, T> func)
        {
            using (SqlConnection conn = new SqlConnection(strConn))
            {
                SqlTransaction tran = null;
                try
                {
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddRange(paraList.ToArray());
                    conn.Open();
                    tran = conn.BeginTransaction();
                    T t = func.Invoke(cmd);
                    tran.Commit();
                    return t;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    if (tran != null)
                        tran.Rollback();
                    throw ex;
                }

            }
        }
    }

    public enum SqlType
    {
        SelectSql,
        InsertSql,
        UpdateSql,
        DeleteSql,
        SelectAllSql
    }

    #region  缓存类、反射
    public class SqlBuilder<T>
    {
        private static string SelectSql = null;
        private static string InsertSql = null;
        private static string UpdateSql = null;
        private static string DeleteSql = null;
        private static string SelectAllSql = null;
        static SqlBuilder()
        {
            Type type = typeof(T);
            string columnSelectStrings = string.Join(",", type.GetProperties().Select(p => $"[{p.GetName()}]"));
            string tableName = type.GetName();
            SelectSql = $@"SELECT {columnSelectStrings} FROM [{tableName}] WHERE ID=@Id";

            SelectAllSql = $@"SELECT {columnSelectStrings} FROM [{tableName}]";

            string columnNameInsertStrings = string.Join(",", type.GetProperties().Where(p => !p.Name.Equals("Id")).Select(p => $"[{p.Name}]]"));
            string columnValueInsertStrings = string.Join(",", type.GetProperties().Where(p => !p.Name.Equals("Id")).Select(p => $"@{p.Name}"));
            InsertSql = $"INSERT INTO {type.Name}({columnNameInsertStrings}) VALUE {columnValueInsertStrings} ";

            string columnUpdateStrings = string.Join(",", type.GetProperties().Where(p => !p.Name.Equals("Id")).Select(p => $"[{p.Name}]=@[{p.Name}]"));
            UpdateSql = $"UPDATE {type.Name} Set {columnUpdateStrings} WHERE Id=@Id";

            DeleteSql = $"DELETE FROM {tableName} WHERE Id=@Id";
        }
        //public SqlBuilder(SqlType sqlType)
        //{
        //    Type type = typeof(T);
        //    switch (sqlType)
        //    {
        //        case SqlType.SelectSql:
        //            string columnSelectStrings = string.Join(",", type.GetProperties().Select(p => $"[{p.GetName()}]"));
        //            string tableName = type.GetName();
        //            SelectSql = $@"SELECT {columnSelectStrings} FROM [{tableName}] WHERE ID=@Id";
        //            break;
        //        case SqlType.InsertSql:
        //            string columnNameInsertStrings = string.Join(",", type.GetProperties().Where(p => !p.Name.Equals("Id")).Select(p => $"[{p.Name}]]"));
        //            string columnValueInsertStrings = string.Join(",", type.GetProperties().Where(p => !p.Name.Equals("Id")).Select(p => $"@{p.Name}"));
        //            InsertSql = $"INSERT INTO {type.Name}({columnNameInsertStrings}) VALUE {columnValueInsertStrings} ";
        //            break;
        //        case SqlType.UpdateSql:
        //            string columnUpdateStrings = string.Join(",", type.GetProperties().Where(p => !p.Name.Equals("Id")).Select(p => $"[{p.Name}]=@[{p.Name}]"));
        //            UpdateSql = $"UPDATE {type.Name} Set {columnUpdateStrings} WHERE Id=@Id";
        //            break;
        //        case SqlType.DeleteSql:
        //            string tableNameDelete = type.GetName();
        //            DeleteSql = $"DELETE FROM {tableNameDelete} WHERE Id=@Id";
        //            break;
        //        case SqlType.SelectAllSql:
        //            string columnSelectAllStrings = string.Join(",", type.GetProperties().Select(p => $"[{p.GetName()}]"));
        //            string tableNameAll = type.GetName();
        //            SelectAllSql = $@"SELECT {columnSelectAllStrings} FROM [{tableNameAll}]";
        //            break;
        //        default:
        //            break;
        //    }
        //}

        public string GetSql(SqlType sqlType)
        {
            switch (sqlType)
            {
                case SqlType.SelectSql:
                    return SelectSql;
                case SqlType.InsertSql:
                    return InsertSql;
                case SqlType.UpdateSql:
                    return UpdateSql;
                case SqlType.DeleteSql:
                    return DeleteSql;
                case SqlType.SelectAllSql:
                    return SelectAllSql;
                default:
                    throw new Exception("类型错误");
            }
        }

        #region  反射
        public static T Resolve()
        {
            string ConfigurationName = $"{typeof(T).Name}Assembly";
            string ConfigurationValue = ConfigurationManager.AppSettings[ConfigurationName].ToString();//获取配置文件信息

            Assembly assembly = Assembly.Load(ConfigurationValue.Split(',')[0]);
            Type type = assembly.GetType(ConfigurationValue.Split(',')[1]);
            return (T)Activator.CreateInstance(type);
        }
        #endregion
    }
    #endregion
    #endregion


    /// <summary>
    /// 数据库操作类
    /// </summary>
    public partial class DBHelper
    {

        //static Configuration cfg = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        //private static string ConnectionString { get { return cfg.ConnectionStrings.ConnectionStrings["sqlConnection"].ConnectionString; } }
        public static string ConnectionString { get { return ConfigurationManager.ConnectionStrings["sqlConnectionTWERPDB"].ConnectionString; } }
        //private static SqlConnection sqlConnection = null;


        /// <summary>
        /// 获得查询对象.
        /// </summary>
        private static SqlCommand GetCommand(SqlConnection sqlConnection)
        {
            SqlCommand sqlCommand = null;
            try
            {
                //if (sqlConnection == null) { sqlConnection = new SqlConnection(ConnectionString); }
                //if (sqlConnection.State == ConnectionState.Closed) { sqlConnection.Open(); }
                //if (sqlConnection.State == ConnectionState.Broken)
                //{ sqlConnection.Close(); sqlConnection.Open(); }
                if (sqlCommand == null)
                {
                    sqlCommand = new SqlCommand(string.Empty, sqlConnection);
                    sqlCommand.CommandTimeout = 200;
                }
                sqlCommand.Parameters.Clear();
                sqlCommand.Connection = sqlConnection;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return sqlCommand;
        }


        #region ExecuteCommand 增,删,改

        /// <summary>
        /// 增,删,改
        /// </summary>
        /// <param name="sql">SQL语句,或存储过程</param>
        /// <param name="commandType">语句类型</param>
        /// <param name="parameters">参数,只限用存储过程</param>
        /// <returns></returns>
        public static bool ExecuteCommand(string sql, CommandType commandType, params SqlParameter[] parameters)
        {
            bool flag = false;
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
                {
                    SqlCommand sqlCommand = null;
                    using (sqlCommand = GetCommand(sqlConnection))
                    {
                        sqlCommand.CommandText = sql;
                        sqlCommand.CommandType = commandType;
                        if (parameters != null) { sqlCommand.Parameters.AddRange(parameters); }
                        sqlConnection.Open();
                        flag = sqlCommand.ExecuteNonQuery() > 0;
                        sqlConnection.Close();
                    }
                }

            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("发生错误SQL语句:" + sql + "\r\n");
                if (parameters != null)
                {
                    foreach (SqlParameter sp in parameters)
                    { sb.Append(sp.ParameterName + "  " + sp.Value + "\r\n"); }
                }
                sb.Append("系统提示错误信息：" + ex.ToString() + "\r\n");

            }
            return flag;
        }

        /// <summary>
        /// 增,删,改
        /// </summary>
        /// <param name="sql">SQL语句</param>
        public static bool ExecuteCommand(string sql)
        { return ExecuteCommand(sql, CommandType.Text, null); }

        /// <summary>
        /// 增,删,改
        /// </summary>
        /// <param name="sql">SQL语句,或存储过程</param>
        /// <param name="commandType">语句类型</param>
        /// <returns></returns>
        public static bool ExecuteCommand(string sql, CommandType commandType)
        { return ExecuteCommand(sql, commandType, null); }

        /// <summary>
        /// 增,删,改
        /// </summary>
        /// <param name="sql">SQL语句,或存储过程</param>
        /// <param name="commandType">语句类型</param>
        /// <returns></returns>
        private static bool ExecuteCommand(SqlConnection conn, SqlTransaction tran, string sql, CommandType commandType, SqlParameter[] parameters)
        {

            bool flag = false;
            try
            {
                SqlCommand sqlCommand = null;
                using (sqlCommand = GetCommand(conn))
                {
                    sqlCommand.Transaction = tran;
                    sqlCommand.CommandText = sql;
                    sqlCommand.CommandType = commandType;
                    if (parameters != null) { sqlCommand.Parameters.AddRange(parameters); }
                    flag = sqlCommand.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("发生错误SQL语句:" + sql + "\r\n");
                if (parameters != null)
                {
                    foreach (SqlParameter sp in parameters)
                    { sb.Append(sp.ParameterName + "  " + sp.Value + "\r\n"); }
                }
                sb.Append("系统提示错误信息：" + ex.ToString() + "\r\n");
            }
            return flag;
        }

        #endregion



        #region ExecuteScalar 返回第一行第一列的值,null

        /// <summary>
        /// 返回第一行第一列的值.无返回值,反回null
        /// </summary>
        /// <param name="sql">sql语句或存储过程</param>
        /// <param name="commandType">解释语句的方式</param>
        /// <param name="parameters">传给该语句的传数</param>
        /// <returns>object</returns>
        public static object ExecuteScalar(string sql, CommandType commandType, params SqlParameter[] parameters)
        {
            object flag = null;
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
                {
                    SqlCommand sqlCommand = null;
                    using (sqlCommand = GetCommand(sqlConnection))
                    {
                        sqlCommand.CommandText = sql;
                        sqlCommand.CommandType = commandType;
                        if (parameters != null) { sqlCommand.Parameters.AddRange(parameters); }
                        sqlConnection.Open();
                        flag = sqlCommand.ExecuteScalar();
                        sqlConnection.Close();
                    }
                }

            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("发生错误SQL语句:" + sql + "\r\n");
                if (parameters != null)
                {
                    foreach (SqlParameter sp in parameters)
                    { sb.Append(sp.ParameterName + "  " + sp.Value + "\r\n"); }
                }
                sb.Append("系统提示错误信息：" + ex.ToString() + "\r\n");
            }
            return flag;
        }

        /// <summary>
        /// 返回第一行第一列的值.无返回值,反回null
        /// </summary>
        /// <param name="sql">sql语句或存储过程</param>
        /// <returns>object</returns>
        public static object ExecuteScalar(string sql)
        { return ExecuteScalar(sql, CommandType.Text, null); }

        /// <summary>
        /// 返回第一行第一列的值.无返回值,反回null
        /// </summary>
        /// <param name="sql">sql语句或存储过程</param>
        /// <param name="commandType">解释语句的方式</param>
        /// <returns>object</returns>
        public static object ExecuteScalar(string sql, CommandType commandType)
        { return ExecuteScalar(sql, commandType, null); }


        #endregion



        #region GetDataTable   执行查询命令,返回 DataTable .


        /// <summary>
        /// 执行查询命令,返回 DataTable .
        /// </summary>
        /// <param name="sql">sql语句或存储过程</param>
        /// <param name="commandType">解释语句的方式</param>
        /// <param name="parameters">传给该语句的传数</param>
        /// <returns></returns>
        public static DataTable GetDataTable(string sql, CommandType commandType, params SqlParameter[] parameters)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
                {
                    SqlCommand sqlCommand = null;
                    DataSet dataSet = new DataSet();
                    using (sqlCommand = GetCommand(sqlConnection))
                    {
                        sqlCommand.CommandText = sql;
                        sqlCommand.CommandType = commandType;
                        if (parameters != null) { sqlCommand.Parameters.AddRange(parameters); }
                        SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlCommand);
                        dataAdapter.Fill(dataSet, "dataSet");
                        dataAdapter.Dispose();
                    }

                    return dataSet.Tables.Count <= 0 ? new DataTable() : dataSet.Tables[0];
                }
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("发生错误SQL语句:" + sql + "\r\n");
                if (parameters != null)
                {
                    foreach (SqlParameter sp in parameters)
                    { sb.Append(sp.ParameterName + "  " + sp.Value + "\r\n"); }
                }
                sb.Append("系统提示错误信息：" + ex.ToString() + "\r\n");
            }

            return new DataTable(DateTime.Now.ToString("T_HHmmss"));
        }



        /// <summary>
        /// 执行查询命令,返回 DataTable .
        /// </summary>
        /// <param name="sql">sql语句或存储过程</param>
        /// <param name="commandType">解释语句的方式</param>
        /// <param name="parameters">传给该语句的传数</param>
        /// <returns></returns>
        public static DataTable GetDataTable(string sql)
        { return GetDataTable(sql, CommandType.Text, null); }
        /// <summary>
        /// 执行查询命令,返回 DataTable .
        /// </summary>
        /// <param name="sql">sql语句或存储过程</param>
        /// <param name="commandType">解释语句的方式</param>
        /// <param name="parameters">传给该语句的传数</param>
        /// <returns></returns>
        public static DataTable GetDataTable(string sql, CommandType commandType)
        { return GetDataTable(sql, commandType, null); }



        #endregion

        #region 返回数据集
        public static DataSet GetDataSet(string sql, CommandType commandType, params SqlParameter[] parameters)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
                {
                    SqlCommand sqlCommand = null;
                    DataSet dataSet = new DataSet();
                    using (sqlCommand = GetCommand(sqlConnection))
                    {
                        sqlCommand.CommandText = sql;
                        sqlCommand.CommandType = commandType;
                        if (parameters != null) { sqlCommand.Parameters.AddRange(parameters); }
                        SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlCommand);
                        dataAdapter.Fill(dataSet, "dataSet");
                    }

                    return dataSet == null ? new DataSet() : dataSet;
                }
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("发生错误SQL语句:" + sql + "\r\n");
                if (parameters != null)
                {
                    foreach (SqlParameter sp in parameters)
                    { sb.Append(sp.ParameterName + "  " + sp.Value + "\r\n"); }
                }
                sb.Append("系统提示错误信息：" + ex.ToString() + "\r\n");
            }

            return new DataSet(DateTime.Now.ToString("T_HHmmss"));
        }

        /// <summary>
        /// 执行查询命令,返回 DataTable .
        /// </summary>
        /// <param name="sql">sql语句或存储过程</param>
        /// <param name="commandType">解释语句的方式</param>
        /// <param name="parameters">传给该语句的传数</param>
        /// <returns></returns>
        public static DataSet GetDataSet(string sql)
        { return GetDataSet(sql, CommandType.Text, null); }
        /// <summary>
        /// 执行查询命令,返回 DataTable .
        /// </summary>
        /// <param name="sql">sql语句或存储过程</param>
        /// <param name="commandType">解释语句的方式</param>
        /// <param name="parameters">传给该语句的传数</param>
        /// <returns></returns>
        public static DataSet GetDataSet(string sql, CommandType commandType)
        { return GetDataSet(sql, commandType, null); }

        #endregion

        #region 对象操作

        /// <summary>
        /// 为指定类型对象赋值,并反回赋值后的数组.
        /// </summary>
        /// <param name="obj">指定类型对象</param>
        /// <returns></returns>
        private static SqlParameter[] GetParameter(object obj)
        {
            //获得当前对象所有属性.
            System.Reflection.PropertyInfo[] propertys = obj.GetType().GetProperties();
            SqlParameter[] parameters = new SqlParameter[propertys.Length];//申明该属性长度的数组.
            for (int i = 0; i < propertys.Length; i++)//循环赋值.
            {
                string parameterName = '@' + propertys[i].Name;
                parameters[i] = new SqlParameter(parameterName, propertys[i].GetValue(obj, null));
            }
            return parameters;
        }



        /// <summary>
        /// 给指定对象里的属性赋值,并返回赋值后的对象.
        /// </summary>
        /// <param name="obj">类的实例</param>
        /// <param name="dataReader">SqlDataReader 对象</param>
        /// <returns>反回赋值后的对象.</returns>
        private static object GetAttribute(object obj, SqlDataReader dataReader)
        {
            //System.Reflection.Assembly tempAssembly = System.Reflection.Assembly.GetExecutingAssembly();
            //string s=obj.GetType().ToString();
            //object o = tempAssembly.CreateInstance(s, true);

            //复制对象.
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            bFormatter.Serialize(stream, obj);
            stream.Seek(0, System.IO.SeekOrigin.Begin);
            object o = bFormatter.Deserialize(stream);


            //返回当前类里的所有公共属性.
            System.Reflection.PropertyInfo[] propertyInfos = obj.GetType().GetProperties();
            for (int i = 0; i < propertyInfos.Length; i++)  //给属性赋值.
            {
                string attribute = propertyInfos[i].Name;//获得属性名.
                propertyInfos[i].SetValue(o, dataReader[attribute], null);
            }
            return o;
        }


        #endregion



        #region 通用,统一数据操作


        /// <summary>
        /// 添加,修改一条数据.反回最后一次添加的主键.
        /// </summary>
        /// <param name="calling"></param>
        /// <returns></returns>
        public static object AddOrUpdate(object obj)
        {
            string sql = string.Format("Proc_{0}_AddOrUpdate", obj.GetType().Name);
            object objScalar = ExecuteScalar(sql, CommandType.StoredProcedure, DBHelper.GetParameter(obj));
            return objScalar is DBNull ? null : objScalar;
        }


        /// <summary>
        /// 通过ID删除一条数据.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool DeleteById(object obj, int id)
        {
            string sql = string.Format("Proc_{0}_DeleteById", obj.GetType().Name);
            SqlParameter[] parameters = { new SqlParameter("@id", id) };
            return ExecuteCommand(sql, CommandType.StoredProcedure, parameters);
        }
        /// <summary>
        /// 通过传入表名及条件返回一条记录
        /// </summary>
        /// <param name="strtable">表名</param>
        /// <param name="strwhere">筛选的条件。即where子句。参数中不能带where关键字。</param>
        /// <param name="strorderby">排序顺序</param>
        /// <returns></returns>
        public static object GetOneRecord(string strtable, string strwhere, string strorderby, object obj)
        {
            SqlDataReader dataReader = null;
            try
            {
                string sql;
                if (strwhere != "")
                {
                    sql = string.Format("select top 1 * from {0} where {1}", strtable, strwhere);
                }
                else
                {
                    sql = string.Format("select top 1 * from {0}", strtable);
                }
                if (strorderby != "")
                {
                    sql = sql + " order by " + strorderby;
                }

                using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand sqlCommand = GetCommand(sqlConnection))
                    {
                        sqlCommand.CommandText = sql;
                        sqlCommand.CommandType = CommandType.Text;


                        sqlConnection.Open();
                        dataReader = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
                        if (!dataReader.Read()) { dataReader.Close(); return null; }
                        obj = DBHelper.GetAttribute(obj, dataReader);
                        dataReader.Close();
                        sqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                if (dataReader != null)
                { dataReader.Close(); }
                return null;
            }
            return obj;

        }
        /// <summary>
        /// 通过传入表名及条件返回一条记录
        /// </summary>
        /// <param name="strtable">表名</param>
        /// <param name="strwhere">筛选的条件。即where子句。参数中不能带where关键字。</param>
        /// <returns></returns>
        public static object GetOneRecord(string strtable, string strwhere, object obj)
        {//该方法返回的对象可能是无效的结果。即当查询表中的数据为空时，返回的obj不为空。

            SqlDataReader dataReader = null;
            try
            {
                string sql;
                if (strwhere != "")
                {
                    sql = string.Format("select top 1 * from {0} where {1}", strtable, strwhere);
                }
                else
                {
                    sql = string.Format("select top 1 * from {0}", strtable);
                }

                using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand sqlCommand = GetCommand(sqlConnection))
                    {
                        sqlCommand.CommandText = sql;
                        sqlCommand.CommandType = CommandType.Text;

                        sqlConnection.Open();
                        dataReader = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
                        if (!dataReader.Read()) { dataReader.Close(); return null; }
                        obj = DBHelper.GetAttribute(obj, dataReader);
                        dataReader.Close();
                        sqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                if (dataReader != null)
                { dataReader.Close(); }
                return null;
            }
            return obj;
        }
        /// <summary>
        /// 通过ID返回一条数据.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static object GetById(object obj, int id)
        {
            SqlDataReader dataReader = null;
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
                {
                    string sql = string.Format("Proc_{0}_GetById", obj.GetType().Name);
                    SqlParameter[] parameters = { new SqlParameter("@id", id) };
                    using (SqlCommand sqlCommand = GetCommand(sqlConnection))
                    {
                        sqlCommand.CommandText = sql;
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddRange(parameters);

                        sqlConnection.Open();
                        dataReader = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
                        if (!dataReader.Read()) { dataReader.Close(); return null; }
                        obj = DBHelper.GetAttribute(obj, dataReader);
                        dataReader.Close();
                        sqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                if (dataReader != null)
                { dataReader.Close(); }
            }
            return obj;
        }

        public static object GetById(string tableName, int id, string procName, object obj)
        {
            SqlDataReader dataReader = null;
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
                {
                    string sql = string.Format("Proc_{0}_GetById", procName);
                    SqlParameter[] parameters = {
                        new SqlParameter("@id", id),
                        new SqlParameter("@TableName", tableName)

                    };
                    using (SqlCommand sqlCommand = GetCommand(sqlConnection))
                    {
                        sqlCommand.CommandText = sql;
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddRange(parameters);

                        sqlConnection.Open();
                        dataReader = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
                        if (!dataReader.Read()) { dataReader.Close(); return null; }
                        obj = DBHelper.GetAttribute(obj, dataReader);
                        dataReader.Close();
                        sqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                if (dataReader != null)
                { dataReader.Close(); }
            }
            return obj;
        }
        /// <summary>
        /// 获得表里的所有数据.
        /// </summary>
        /// <returns></returns>
        public static List<object> GetAll(object obj)
        {
            SqlDataReader dataReader = null;
            List<object> list = new List<object>();
            try
            {
                string sql = string.Format("Proc_{0}_GetAll", obj.GetType().Name);
                using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand sqlCommand = GetCommand(sqlConnection))
                    {
                        sqlCommand.CommandText = sql;
                        sqlCommand.CommandType = CommandType.StoredProcedure;

                        sqlConnection.Open();
                        dataReader = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
                        while (dataReader.Read()) { list.Add(DBHelper.GetAttribute(obj, dataReader)); }
                        dataReader.Close();
                        sqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                if (dataReader != null)
                { dataReader.Close(); }
            }
            return list;
        }


        /// <summary>
        /// 获得表里的所有数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static DataTable GetTable(object obj)
        {
            string sql = string.Format("Proc_{0}_GetAll", obj.GetType().Name);
            return DBHelper.GetDataTable(sql, CommandType.StoredProcedure);
        }



        /// <summary>
        /// 获得指定表的数据行数,where条件
        /// </summary>
        public static int GetRowCount(string sql)
        {

            object count = DBHelper.ExecuteScalar(sql, CommandType.Text);
            return count == null ? -1 : Convert.ToInt32(count);
        }
        /// <summary>
        /// 获得指定表的数据行数,where条件
        /// </summary>
        public static int GetRowCount(object obj, string where)
        {
            string sql = "Proc_GetRowCount";
            SqlParameter[] parameters = {
                                                            new SqlParameter("@tableName", obj.GetType().Name),
                                                            new SqlParameter("@where",where)
                                                    };
            object count = DBHelper.ExecuteScalar(sql, CommandType.StoredProcedure, parameters);
            return count == null ? -1 : Convert.ToInt32(count);
        }

        /// <summary>
        /// 获得指定表的数据行数
        /// </summary>
        public static int GetRowCount(object obj)
        {
            return GetRowCount(obj, string.Empty);
        }


        /// <summary>
        /// 获得,obj指定表,columnName指定排序列的,top指定几条数据,pageIndex第几页从零开始.true升,false降,where条件
        /// </summary>
        public static List<object> GetTopOrderBy(object obj, string columnName, int top, int pageIndex, bool order, string where)
        {
            SqlDataReader dataReader = null;
            string sql = "Proc_GetTopOrderBy";
            List<object> list = new List<object>();
            try
            {
                SqlParameter[] parameters ={
                        new SqlParameter("@tableName",obj.GetType().Name),
                        new SqlParameter("@columnName",columnName),
                        new SqlParameter("@top",top),
                        new SqlParameter("@pageIndex",pageIndex),
                        //new SqlParameter("@primarykey","id"),
                        new SqlParameter("@order",order?"asc":"desc"),
                        new SqlParameter("@where",where)};
                using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand sqlCommand = GetCommand(sqlConnection))
                    {
                        sqlCommand.CommandText = sql;
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddRange(parameters);

                        sqlConnection.Open();
                        dataReader = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
                        // SqlDataReader dataReader = DBHelper.GetDataReader(CommandType.StoredProcedure, parameters);
                        while (dataReader.Read()) { list.Add(DBHelper.GetAttribute(obj, dataReader)); }
                        dataReader.Close();
                        sqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                if (dataReader != null)
                { dataReader.Close(); }
            }
            return list;
        }

        /// <summary>
        /// 获得,obj指定表,columnName指定排序列的,top指定几条数据,pageIndex第几页从零开始.true升,false降
        /// </summary>
        public static List<object> GetTopOrderBy(object obj, string columnName, int top, int pageIndex, bool order)
        {
            return GetTopOrderBy(obj, columnName, top, pageIndex, order, string.Empty);
        }


        /// <summary>
        /// 获得表里的排序,并分页的数据,tableName指定表,columnName指定排序列的,top指定几条数据,pageIndex第几页从零开始.true升,false降,where条件
        /// </summary>
        public static DataTable GetTableTopOrderBy(string tableName, string columnName, int top, int pageIndex, bool order, string where)
        {
            string sql = "Proc_GetTopOrderBy";
            SqlParameter[] parameters ={
                        new SqlParameter("@tableName",tableName),
                        new SqlParameter("@columnName",columnName),
                        new SqlParameter("@top",top),
                        new SqlParameter("@pageIndex",pageIndex),
                        //new SqlParameter("@primarykey","id"),
                        new SqlParameter("@order",order?"asc":"desc"),
                        new SqlParameter("@where",where)};
            return DBHelper.GetDataTable(sql, CommandType.StoredProcedure, parameters);
        }

        /// <summary>
        /// 获得表里的排序,并分页的数据,tableName指定表,columnName指定排序列的,top指定几条数据,pageIndex第几页从零开始.true升,false降
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static DataTable GetTableTopOrderBy(string tableName, string columnName, int top, int pageIndex, bool order)
        {
            return GetTableTopOrderBy(tableName, columnName, top, pageIndex, order, string.Empty);
        }


        /// <summary>
        /// 获得表里的排序,并分页的数据,columnName指定排序列的,top指定几条数据,pageIndex第几页从零开始.true升,false降,parameters 扩展参数
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static DataTable GetTableTopOrderBy(string procedure, string columnName, int top, int pageIndex, bool order, params SqlParameter[] parameters)
        {
            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@columnName", columnName));
            list.Add(new SqlParameter("@top", top));
            list.Add(new SqlParameter("@pageIndex", pageIndex));
            list.Add(new SqlParameter("@order", order ? "asc" : "desc"));
            if (parameters.Length >= 1) { list.AddRange(parameters); }
            return DBHelper.GetDataTable(procedure, CommandType.StoredProcedure, list.ToArray());
        }


        /// <summary>
        /// 获得视图表的指定ID数据行.
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static DataTable GetTableById(string viewTableName, int id)
        {
            string sql = string.Format("Proc_{0}_GetById", viewTableName);
            SqlParameter[] parameters = { new SqlParameter("@id", id) };
            return DBHelper.GetDataTable(sql, CommandType.StoredProcedure, parameters);
        }

        #endregion

        #region DataTable批量单表添加(有事务) 
        /// <summary>   

        /// DataTable批量添加(有事务)   

        /// </summary> /  

        // <param name="Table">数据源</param>  

        /// <param name="Mapping">定义数据源和目标源列的关系集合</param>   

        /// <param name="DestinationTableName">目标表</param>   

        public static bool MySqlBulkCopy(DataTable Table, SqlBulkCopyColumnMapping[] Mapping, string DestinationTableName, string deletesql)
        {

            bool Bool = true;

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {

                con.Open();

                using (SqlTransaction Tran = con.BeginTransaction())
                {

                    using (SqlBulkCopy Copy = new SqlBulkCopy(con, SqlBulkCopyOptions.KeepIdentity, Tran))
                    {

                        Copy.DestinationTableName = DestinationTableName;//指定目标表   

                        if (Mapping != null)
                        { //如果有数据   

                            foreach (SqlBulkCopyColumnMapping Map in Mapping)
                            {

                                Copy.ColumnMappings.Add(Map);
                            }
                        }

                        try
                        {
                            Copy.WriteToServer(Table);//批量添加   

                            Tran.Commit();//提交事务
                        }

                        catch
                        {
                            Tran.Rollback();

                            //回滚事务
                            Bool = false;
                        }
                    }
                }
            }
            return Bool;
        }
        #endregion

        #region DataTable多个表同时批量添加(有事务)   

        /// <param name="Table">数据源</param>  

        /// <param name="Mapping">定义数据源和目标源列的关系集合</param>   

        /// <param name="DestinationTableName">目标表</param>   

        public static bool MySqlBulkCopyMultipleTab(DataSet ds, string[] DestinationTableName, string othersql)
        {
            bool Bool = true;
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                using (SqlTransaction Tran = con.BeginTransaction())
                {
                    try
                    {
                        if (othersql != "")
                        {
                            //bool sqlresult = ExecuteCommand(othersql);
                            bool sqlresult = ExecuteCommand(con, Tran, othersql, CommandType.Text, null);
                        }
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            using (SqlBulkCopy Copy = new SqlBulkCopy(con, SqlBulkCopyOptions.KeepIdentity, Tran))
                            {
                                Copy.DestinationTableName = DestinationTableName[i];//指定目标表 
                                DataTable dt = ds.Tables[i];
                                SqlBulkCopyColumnMapping[] Mapping = new SqlBulkCopyColumnMapping[dt.Columns.Count];
                                for (int u = 0; u < dt.Columns.Count; u++)
                                {
                                    string dtallcol = dt.Columns[u].ColumnName;
                                    Mapping[u] = new SqlBulkCopyColumnMapping(dtallcol, dtallcol);
                                }
                                if (Mapping != null)
                                {
                                    foreach (SqlBulkCopyColumnMapping Map in Mapping)
                                    {
                                        Copy.ColumnMappings.Add(Map);
                                    }
                                }
                                Copy.WriteToServer(dt);//批量添加   
                            }
                        }
                        Tran.Commit();//提交事务
                        Bool = true;
                    }
                    catch (Exception ex)
                    {
                        Bool = false;
                        string aa = "MySqlBulkCopyMultipleTab方法发生错误:" + ex.Message;
                    }
                }
            }
            return Bool;
        }
        #endregion

        #region 查询======>调用存储过程函数,存储过程名称,输入参数
        public static DataTable GetDataTable(string sql, SqlParameter[] parameters)
        {
            DataTable dt = new DataTable();
            SqlCommand sqlCommand = new SqlCommand();
            SqlConnection conn = new SqlConnection(ConnectionString);
            sqlCommand.Connection = conn;
            sqlCommand.Connection.Open();
            sqlCommand.CommandText = sql;
            sqlCommand.CommandType = CommandType.StoredProcedure;
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

        /// <summary>
        /// 循环model做sql插入语句
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool InsertTab(object model)
        {
            string sql = GetSql(model, false);
            return ExecuteCommand(sql);
        }
        /// <summary>
        /// 循环model做sql修改语句
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool UpdateTab(object model)
        {
            string sql = GetSql(model, true);
            return ExecuteCommand(sql);
        }

        public static string GetSql(object model, bool isupdate)
        {
            StringBuilder sql = new StringBuilder();
            if (!isupdate)
            {
                sql.Append("insert into ");
                sql.Append(model.GetType().Name);
                sql.Append("(");
                StringBuilder values = new StringBuilder();
                PropertyInfo[] properties = model.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
                foreach (PropertyInfo item in properties)
                {
                    //实体类字段名称
                    string name = item.Name;
                    if (name == "Id")
                    {
                        continue;
                    }
                    object value = item.GetValue(model, null);//该字段的值
                    sql.Append("[" + name + "]");
                    values.Append("'" + value + "'");
                    if (name != properties[properties.Length - 1].Name)
                    {
                        sql.Append(",");
                        values.Append(",");
                    }

                }
                sql.Append(")");
                sql.Append(" values (" + values + ")");
            }
            else
            {
                object id = 0;

                sql.Append("update ");
                sql.Append(model.GetType().Name);
                sql.Append(" set ");

                PropertyInfo[] properties = model.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
                foreach (PropertyInfo item in properties)
                {
                    //实体类字段名称
                    string name = item.Name;
                    if (name == "Id")
                    {
                        id = item.GetValue(model, null);//该字段的值
                        continue;
                    }
                    object value = item.GetValue(model, null);//该字段的值
                    sql.Append("[" + name + "]='" + value + "'");
                    if (name != properties[properties.Length - 1].Name)
                    {
                        sql.Append(",");
                    }
                }
                sql.Append(" where Id=" + id);
            }
            return sql.ToString();
        }

    }


    #region  泛型ORM
    public class BaseRepository<T> where T : class
    {
        public virtual bool Add(T Model)
        {
            StringBuilder stringBuilder = new StringBuilder();
            string tableName = GetTableName(Model);
            stringBuilder.Append("Insert into ");
            stringBuilder.Append(tableName);
            stringBuilder.Append("(");
            Type typeFromHandle = typeof(T);
            PropertyInfo[] properties = typeFromHandle.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            if (properties.Length <= 0)
            {
                throw new Exception("没有一个属性");
            }
            PropertyInfo[] array = properties;
            foreach (PropertyInfo propertyInfo in array)
            {
                if (!propertyInfo.IsDefined(typeof(KeyIdAttribute), inherit: true))
                {
                    stringBuilder.Append(propertyInfo.Name);
                    stringBuilder.Append(",");
                }
            }
            stringBuilder.Remove(stringBuilder.Length - 1, 1);
            stringBuilder.Append(") ");
            stringBuilder.Append("values (");
            array = properties;
            foreach (PropertyInfo propertyInfo in array)
            {
                if (!propertyInfo.IsDefined(typeof(KeyIdAttribute), inherit: true))
                {
                    stringBuilder.Append("@" + propertyInfo.Name);
                    stringBuilder.Append(",");
                }
            }
            stringBuilder.Remove(stringBuilder.Length - 1, 1);
            stringBuilder.Append(") ");
            List<SqlParameter> parameters = GetParameters(Model, properties);
            int num = DbHelperSQL.ExecuteSql(stringBuilder.ToString(), parameters.ToArray());
            if (num <= 0)
            {
                return false;
            }
            return true;
        }

        private static List<SqlParameter> GetParameters(T Model, PropertyInfo[] propertys)
        {
            List<SqlParameter> list = new List<SqlParameter>();
            foreach (PropertyInfo propertyInfo in propertys)
            {
                if (!propertyInfo.IsDefined(typeof(KeyIdAttribute), inherit: true))
                {
                    if (propertyInfo.GetValue(Model, null) == null)
                    {
                        list.Add(new SqlParameter("@" + propertyInfo.Name, DBNull.Value));
                    }
                    else
                    {
                        list.Add(new SqlParameter("@" + propertyInfo.Name, propertyInfo.GetValue(Model, null)));
                    }
                }
            }
            return list;
        }

        private static string GetTableName(T Model)
        {
            string[] array = Model.ToString().Split('.');
            return array[array.Length - 1];
        }

        private string GetObjectPropertyValue(T t, string propertyName)
        {
            Type typeFromHandle = typeof(T);
            PropertyInfo property = typeFromHandle.GetProperty(propertyName);
            if (property == null)
            {
                return string.Empty;
            }
            object value = property.GetValue(t, null);
            if (value == null)
            {
                return string.Empty;
            }
            return value.ToString();
        }

        public virtual bool Edit(T model)
        {
            StringBuilder stringBuilder = new StringBuilder();
            string tableName = GetTableName(model);
            stringBuilder.Append("Update ");
            stringBuilder.Append(tableName);
            stringBuilder.Append(" set ");
            Type typeFromHandle = typeof(T);
            PropertyInfo[] properties = typeFromHandle.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            if (properties.Length <= 0)
            {
                throw new Exception("没有一个属性");
            }
            string arg = "";
            int num = 0;
            PropertyInfo[] array = properties;
            foreach (PropertyInfo propertyInfo in array)
            {
                if (!propertyInfo.IsDefined(typeof(KeyIdAttribute), inherit: true))
                {
                    if (propertyInfo.GetValue(model, null) != null)
                    {
                        stringBuilder.Append(propertyInfo.Name + "=@" + propertyInfo.Name);
                        stringBuilder.Append(",");
                    }
                }
                else
                {
                    arg = propertyInfo.Name;
                    num = (int)propertyInfo.GetValue(model, null);
                }
            }
            stringBuilder.Remove(stringBuilder.Length - 1, 1);
            stringBuilder.Append(" where ");
            stringBuilder.Append(arg + " = " + num);
            List<SqlParameter> list = new List<SqlParameter>();
            array = properties;
            foreach (PropertyInfo propertyInfo in array)
            {
                if (!propertyInfo.IsDefined(typeof(KeyIdAttribute), inherit: true) && propertyInfo.GetValue(model, null) != null)
                {
                    list.Add(new SqlParameter("@" + propertyInfo.Name, propertyInfo.GetValue(model, null)));
                }
            }
            int num2 = DbHelperSQL.ExecuteSql(stringBuilder.ToString(), list.ToArray());
            if (num2 <= 0)
            {
                return false;
            }
            return true;
        }

        public virtual bool Del(T model)
        {
            StringBuilder stringBuilder = new StringBuilder();
            string tableName = GetTableName(model);
            stringBuilder.Append("delete " + tableName);
            stringBuilder.Append(" where ");
            Type typeFromHandle = typeof(T);
            PropertyInfo[] properties = typeFromHandle.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            if (properties.Length <= 0)
            {
                throw new Exception("没有一个属性");
            }
            PropertyInfo[] array = properties;
            foreach (PropertyInfo propertyInfo in array)
            {
                if (propertyInfo.PropertyType == typeof(string))
                {
                    if (propertyInfo.GetValue(model, null) != null)
                    {
                        stringBuilder.Append(" ");
                        stringBuilder.Append(propertyInfo.Name + "=@" + propertyInfo.Name);
                        stringBuilder.Append(" and");
                    }
                }
                else if (Convert.ToInt32(propertyInfo.GetValue(model, null)) != 0)
                {
                    stringBuilder.Append(" ");
                    stringBuilder.Append(propertyInfo.Name + "=@" + propertyInfo.Name);
                    stringBuilder.Append(" and");
                }
            }
            stringBuilder.Remove(stringBuilder.Length - 3, 3);
            List<SqlParameter> list = new List<SqlParameter>();
            array = properties;
            foreach (PropertyInfo propertyInfo in array)
            {
                if (propertyInfo.PropertyType == typeof(string))
                {
                    if (propertyInfo.GetValue(model, null) != null)
                    {
                        list.Add(new SqlParameter("@" + propertyInfo.Name, propertyInfo.GetValue(model, null)));
                    }
                }
                else if (Convert.ToInt32(propertyInfo.GetValue(model, null)) != 0)
                {
                    list.Add(new SqlParameter("@" + propertyInfo.Name, propertyInfo.GetValue(model, null)));
                }
            }
            int num = DbHelperSQL.ExecuteSql(stringBuilder.ToString(), list.ToArray());
            if (num <= 0)
            {
                return false;
            }
            return true;
        }

        public virtual DataTable QueryByPage(Page page)
        {
            StringBuilder stringBuilder = new StringBuilder();
            SqlParameter[] array = new SqlParameter[11]
            {
                new SqlParameter("@TableName", SqlDbType.VarChar, 200),
                new SqlParameter("@FieldList", SqlDbType.VarChar, 1500),
                new SqlParameter("@PageSize", SqlDbType.Int),
                new SqlParameter("@PageNumber", SqlDbType.Int),
                new SqlParameter("@SortFields", SqlDbType.VarChar, 1000),
                new SqlParameter("@EnabledSort", SqlDbType.Bit),
                new SqlParameter("@QueryCondition", SqlDbType.VarChar, 1500),
                new SqlParameter("@Primarykey", SqlDbType.VarChar, 50),
                new SqlParameter("@EnabledDistinct", SqlDbType.Bit),
                new SqlParameter("@PageCount", SqlDbType.Int),
                new SqlParameter("@RecordCount", SqlDbType.Int)
            };
            array[9].Direction = ParameterDirection.Output;
            array[10].Direction = ParameterDirection.Output;
            array[0].Value = page.TableName;
            array[1].Value = page.FieldList;
            array[2].Value = page.PageSize;
            array[3].Value = page.PageIndex;
            array[4].Value = page.SortFields;
            array[5].Value = page.EnabledSort;
            array[6].Value = page.QueryCondition;
            array[7].Value = page.Primarykey;
            array[8].Value = page.EnabledDistinct;
            DataTable result = DbHelperSQL.RunProcedureToDataSet("P_GridViewPager", array).Tables[0];
            page.PageCount = ((array[9].Value != null) ? ((int)array[9].Value) : 0);
            page.RecordCount = ((array[10].Value != null) ? ((int)array[10].Value) : 0);
            return result;
        }

        private static List<T> GetListEntrty(Type typeTableName, DataTable dt)
        {
            List<T> list = new List<T>();
            if (dt.Rows.Count >= 1)
            {
                foreach (DataRow row in dt.Rows)
                {
                    object obj = Activator.CreateInstance(typeTableName);
                    PropertyInfo[] properties = typeTableName.GetProperties();
                    PropertyInfo[] array = properties;
                    foreach (PropertyInfo propertyInfo in array)
                    {
                        if (row[propertyInfo.Name] != DBNull.Value)
                        {
                            propertyInfo.SetValue(obj, row[propertyInfo.Name], null);
                        }
                    }
                    list.Add(obj as T);
                }
                return list;
            }
            return null;
        }

        public virtual List<T> QueryWhere(Type typeTableName, string where)
        {
            StringBuilder stringBuilder = new StringBuilder();
            string name = typeTableName.Name;
            stringBuilder.Append("select ");
            Type typeFromHandle = typeof(T);
            PropertyInfo[] properties = typeFromHandle.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            if (properties.Length <= 0)
            {
                throw new Exception("没有一个属性");
            }
            PropertyInfo[] array = properties;
            foreach (PropertyInfo propertyInfo in array)
            {
                stringBuilder.Append(propertyInfo.Name);
                stringBuilder.Append(",");
            }
            stringBuilder.Remove(stringBuilder.Length - 1, 1);
            stringBuilder.Append(" from ");
            stringBuilder.Append(name);
            stringBuilder.Append(" ");
            stringBuilder.Append(where);
            DataTable dt = DbHelperSQL.Query(stringBuilder.ToString()).Tables[0];
            return GetListEntrty(typeTableName, dt);
        }
    }

    public abstract class DbHelperSQL
    {
        public static string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        public DbHelperSQL()
        {
        }

        public static bool ColumnExists(string tableName, string columnName)
        {
            string sQLString = "select count(1) from syscolumns where [id]=object_id('" + tableName + "') and [name]='" + columnName + "'";
            object single = GetSingle(sQLString);
            if (single == null)
            {
                return false;
            }
            return Convert.ToInt32(single) > 0;
        }

        public static int GetMaxID(string FieldName, string TableName)
        {
            string sQLString = "select max(" + FieldName + ")+1 from " + TableName;
            object single = GetSingle(sQLString);
            if (single == null)
            {
                return 1;
            }
            return int.Parse(single.ToString());
        }

        public static bool Exists(string strSql)
        {
            object single = GetSingle(strSql);
            if (object.Equals(single, null) || object.Equals(single, DBNull.Value) || int.Parse(single.ToString()) == 0)
            {
                return false;
            }
            return true;
        }

        public static bool TabExists(string TableName)
        {
            string sQLString = "select count(*) from sysobjects where id = object_id(N'[" + TableName + "]') and OBJECTPROPERTY(id, N'IsUserTable') = 1";
            object single = GetSingle(sQLString);
            if (object.Equals(single, null) || object.Equals(single, DBNull.Value) || int.Parse(single.ToString()) == 0)
            {
                return false;
            }
            return true;
        }

        public static bool Exists(string strSql, params SqlParameter[] cmdParms)
        {
            object single = GetSingle(strSql, cmdParms);
            if (object.Equals(single, null) || object.Equals(single, DBNull.Value) || int.Parse(single.ToString()) == 0)
            {
                return false;
            }
            return true;
        }

        public static int ExecuteSql(string SQLString)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(SQLString, sqlConnection))
                {
                    try
                    {
                        sqlConnection.Open();
                        return sqlCommand.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        sqlConnection.Close();
                        throw ex;
                    }
                }
            }
        }

        public static int ExecuteSqlByTime(string SQLString, int Times)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(SQLString, sqlConnection))
                {
                    try
                    {
                        sqlConnection.Open();
                        sqlCommand.CommandTimeout = Times;
                        return sqlCommand.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        sqlConnection.Close();
                        throw ex;
                    }
                }
            }
        }

        public static int ExecuteSqlTranThrow(List<string> SQLStringList)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.Connection = sqlConnection;
                SqlTransaction sqlTransaction2 = sqlCommand.Transaction = sqlConnection.BeginTransaction();
                try
                {
                    int num = 0;
                    for (int i = 0; i < SQLStringList.Count; i++)
                    {
                        string text = SQLStringList[i];
                        if (text.Trim().Length > 1)
                        {
                            sqlCommand.CommandText = text;
                            num += sqlCommand.ExecuteNonQuery();
                        }
                    }
                    sqlTransaction2.Commit();
                    return num;
                }
                catch (Exception ex)
                {
                    sqlTransaction2.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }

        public static int ExecuteSql(string SQLString, string content)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(SQLString, sqlConnection);
                SqlParameter sqlParameter = new SqlParameter("@content", SqlDbType.NText);
                sqlParameter.Value = content;
                sqlCommand.Parameters.Add(sqlParameter);
                try
                {
                    sqlConnection.Open();
                    return sqlCommand.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    sqlCommand.Dispose();
                    sqlConnection.Close();
                }
            }
        }

        public static object ExecuteSqlGet(string SQLString, string content)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(SQLString, sqlConnection);
                SqlParameter sqlParameter = new SqlParameter("@content", SqlDbType.NText);
                sqlParameter.Value = content;
                sqlCommand.Parameters.Add(sqlParameter);
                try
                {
                    sqlConnection.Open();
                    object obj = sqlCommand.ExecuteScalar();
                    if (object.Equals(obj, null) || object.Equals(obj, DBNull.Value))
                    {
                        return null;
                    }
                    return obj;
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    sqlCommand.Dispose();
                    sqlConnection.Close();
                }
            }
        }

        public static int ExecuteSqlInsertImg(string strSQL, byte[] fs)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(strSQL, sqlConnection);
                SqlParameter sqlParameter = new SqlParameter("@fs", SqlDbType.Image);
                sqlParameter.Value = fs;
                sqlCommand.Parameters.Add(sqlParameter);
                try
                {
                    sqlConnection.Open();
                    return sqlCommand.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    sqlCommand.Dispose();
                    sqlConnection.Close();
                }
            }
        }

        public static object GetSingle(string SQLString)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(SQLString, sqlConnection))
                {
                    try
                    {
                        sqlConnection.Open();
                        object obj = sqlCommand.ExecuteScalar();
                        if (object.Equals(obj, null) || object.Equals(obj, DBNull.Value))
                        {
                            return null;
                        }
                        return obj;
                    }
                    catch (SqlException ex)
                    {
                        sqlConnection.Close();
                        throw ex;
                    }
                }
            }
        }

        public static object GetSingle(string SQLString, int Times)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(SQLString, sqlConnection))
                {
                    try
                    {
                        sqlConnection.Open();
                        sqlCommand.CommandTimeout = Times;
                        object obj = sqlCommand.ExecuteScalar();
                        if (object.Equals(obj, null) || object.Equals(obj, DBNull.Value))
                        {
                            return null;
                        }
                        return obj;
                    }
                    catch (SqlException ex)
                    {
                        sqlConnection.Close();
                        throw ex;
                    }
                }
            }
        }

        public static SqlDataReader ExecuteReader(string strSQL)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            SqlCommand sqlCommand = new SqlCommand(strSQL, sqlConnection);
            try
            {
                sqlConnection.Open();
                return sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public static DataSet Query(string SQLString)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                DataSet dataSet = new DataSet();
                try
                {
                    sqlConnection.Open();
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(SQLString, sqlConnection);
                    sqlDataAdapter.Fill(dataSet, "ds");
                }
                catch (SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                return dataSet;
            }
        }

        public static DataSet Query(string SQLString, int Times)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                DataSet dataSet = new DataSet();
                try
                {
                    sqlConnection.Open();
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(SQLString, sqlConnection);
                    sqlDataAdapter.SelectCommand.CommandTimeout = Times;
                    sqlDataAdapter.Fill(dataSet, "ds");
                }
                catch (SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                return dataSet;
            }
        }

        public static int ExecuteSql(string SQLString, params SqlParameter[] cmdParms)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    try
                    {
                        PrepareCommand(sqlCommand, conn, null, SQLString, cmdParms);
                        int result = sqlCommand.ExecuteNonQuery();
                        sqlCommand.Parameters.Clear();
                        return result;
                    }
                    catch (SqlException ex)
                    {
                        throw ex;
                    }
                }
            }
        }

        public static int ExecuteSqlTran(List<CommandInfo> cmdList)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (SqlTransaction sqlTransaction = sqlConnection.BeginTransaction())
                {
                    SqlCommand sqlCommand = new SqlCommand();
                    try
                    {
                        int num = 0;
                        foreach (CommandInfo cmd in cmdList)
                        {
                            string commandText = cmd.CommandText;
                            SqlParameter[] parameters = cmd.Parameters;
                            PrepareCommand(sqlCommand, sqlConnection, sqlTransaction, commandText, parameters);
                            if (cmd.EffentNextType == EffentNextType.WhenHaveContine || cmd.EffentNextType == EffentNextType.WhenNoHaveContine)
                            {
                                if (cmd.CommandText.ToLower().IndexOf("count(") == -1)
                                {
                                    sqlTransaction.Rollback();
                                    return 0;
                                }
                                object obj = sqlCommand.ExecuteScalar();
                                bool flag = false;
                                if (obj == null && obj == DBNull.Value)
                                {
                                    flag = false;
                                }
                                flag = (Convert.ToInt32(obj) > 0);
                                if (cmd.EffentNextType == EffentNextType.WhenHaveContine && !flag)
                                {
                                    sqlTransaction.Rollback();
                                    return 0;
                                }
                                if (cmd.EffentNextType == EffentNextType.WhenNoHaveContine && flag)
                                {
                                    sqlTransaction.Rollback();
                                    return 0;
                                }
                            }
                            else
                            {
                                int num2 = sqlCommand.ExecuteNonQuery();
                                num += num2;
                                if (cmd.EffentNextType == EffentNextType.ExcuteEffectRows && num2 == 0)
                                {
                                    sqlTransaction.Rollback();
                                    return 0;
                                }
                                sqlCommand.Parameters.Clear();
                            }
                        }
                        sqlTransaction.Commit();
                        return num;
                    }
                    catch
                    {
                        sqlTransaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public static void ExecuteSqlTranWithIndentity(List<CommandInfo> SQLStringList)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (SqlTransaction sqlTransaction = sqlConnection.BeginTransaction())
                {
                    SqlCommand sqlCommand = new SqlCommand();
                    try
                    {
                        int num = 0;
                        foreach (CommandInfo SQLString in SQLStringList)
                        {
                            string commandText = SQLString.CommandText;
                            SqlParameter[] parameters = SQLString.Parameters;
                            SqlParameter[] array = parameters;
                            foreach (SqlParameter sqlParameter in array)
                            {
                                if (sqlParameter.Direction == ParameterDirection.InputOutput)
                                {
                                    sqlParameter.Value = num;
                                }
                            }
                            PrepareCommand(sqlCommand, sqlConnection, sqlTransaction, commandText, parameters);
                            int num2 = sqlCommand.ExecuteNonQuery();
                            array = parameters;
                            foreach (SqlParameter sqlParameter in array)
                            {
                                if (sqlParameter.Direction == ParameterDirection.Output)
                                {
                                    num = Convert.ToInt32(sqlParameter.Value);
                                }
                            }
                            sqlCommand.Parameters.Clear();
                        }
                        sqlTransaction.Commit();
                    }
                    catch
                    {
                        sqlTransaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public static object GetSingle(string SQLString, params SqlParameter[] cmdParms)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    try
                    {
                        PrepareCommand(sqlCommand, conn, null, SQLString, cmdParms);
                        object obj = sqlCommand.ExecuteScalar();
                        sqlCommand.Parameters.Clear();
                        if (object.Equals(obj, null) || object.Equals(obj, DBNull.Value))
                        {
                            return null;
                        }
                        return obj;
                    }
                    catch (SqlException ex)
                    {
                        throw ex;
                    }
                }
            }
        }

        public static SqlDataReader ExecuteReader(string SQLString, params SqlParameter[] cmdParms)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand sqlCommand = new SqlCommand();
            try
            {
                PrepareCommand(sqlCommand, conn, null, SQLString, cmdParms);
                SqlDataReader result = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
                sqlCommand.Parameters.Clear();
                return result;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public static DataSet Query(string SQLString, params SqlParameter[] cmdParms)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand sqlCommand = new SqlCommand();
                PrepareCommand(sqlCommand, conn, null, SQLString, cmdParms);
                using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
                {
                    DataSet dataSet = new DataSet();
                    try
                    {
                        sqlDataAdapter.Fill(dataSet, "ds");
                        sqlCommand.Parameters.Clear();
                    }
                    catch (SqlException ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    return dataSet;
                }
            }
        }

        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, string cmdText, SqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
            {
                cmd.Transaction = trans;
            }
            cmd.CommandType = CommandType.Text;
            if (cmdParms == null)
            {
                return;
            }
            foreach (SqlParameter sqlParameter in cmdParms)
            {
                if ((sqlParameter.Direction == ParameterDirection.InputOutput || sqlParameter.Direction == ParameterDirection.Input) && sqlParameter.Value == null)
                {
                    sqlParameter.Value = DBNull.Value;
                }
                cmd.Parameters.Add(sqlParameter);
            }
        }

        public static SqlDataReader RunProcedure(string storedProcName, IDataParameter[] parameters)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            SqlCommand sqlCommand = BuildQueryCommand(sqlConnection, storedProcName, parameters);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            return sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
        }

        public static DataSet RunProcedureToDataSet(string storedProcName, IDataParameter[] parameters)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                DataSet dataSet = new DataSet();
                sqlConnection.Open();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                sqlDataAdapter.SelectCommand = BuildQueryCommand(sqlConnection, storedProcName, parameters);
                sqlDataAdapter.Fill(dataSet, "ds");
                sqlConnection.Close();
                return dataSet;
            }
        }

        public static DataSet RunProcedure(string storedProcName, IDataParameter[] parameters, string tableName)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                DataSet dataSet = new DataSet();
                sqlConnection.Open();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                sqlDataAdapter.SelectCommand = BuildQueryCommand(sqlConnection, storedProcName, parameters);
                sqlDataAdapter.Fill(dataSet, tableName);
                sqlConnection.Close();
                return dataSet;
            }
        }

        public static DataSet RunProcedure(string storedProcName, IDataParameter[] parameters, string tableName, int Times)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                DataSet dataSet = new DataSet();
                sqlConnection.Open();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                sqlDataAdapter.SelectCommand = BuildQueryCommand(sqlConnection, storedProcName, parameters);
                sqlDataAdapter.SelectCommand.CommandTimeout = Times;
                sqlDataAdapter.Fill(dataSet, tableName);
                sqlConnection.Close();
                return dataSet;
            }
        }

        private static SqlCommand BuildQueryCommand(SqlConnection connection, string storedProcName, IDataParameter[] parameters)
        {
            SqlCommand sqlCommand = new SqlCommand(storedProcName, connection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            for (int i = 0; i < parameters.Length; i++)
            {
                SqlParameter sqlParameter = (SqlParameter)parameters[i];
                if (sqlParameter != null)
                {
                    if ((sqlParameter.Direction == ParameterDirection.InputOutput || sqlParameter.Direction == ParameterDirection.Input) && sqlParameter.Value == null)
                    {
                        sqlParameter.Value = DBNull.Value;
                    }
                    sqlCommand.Parameters.Add(sqlParameter);
                }
            }
            return sqlCommand;
        }

        public static int RunProcedure(string storedProcName, IDataParameter[] parameters, out int rowsAffected)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                SqlCommand sqlCommand = BuildIntCommand(sqlConnection, storedProcName, parameters);
                rowsAffected = sqlCommand.ExecuteNonQuery();
                return (int)sqlCommand.Parameters["ReturnValue"].Value;
            }
        }

        private static SqlCommand BuildIntCommand(SqlConnection connection, string storedProcName, IDataParameter[] parameters)
        {
            SqlCommand sqlCommand = BuildQueryCommand(connection, storedProcName, parameters);
            sqlCommand.Parameters.Add(new SqlParameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.ReturnValue, isNullable: false, 0, 0, string.Empty, DataRowVersion.Default, null));
            return sqlCommand;
        }
    }
    public enum EffentNextType
    {
        None,
        WhenHaveContine,
        WhenNoHaveContine,
        ExcuteEffectRows,
        SolicitationEvent
    }
    public class KeyIdAttribute : Attribute
    {
    }
    public sealed class Page
    {
        public string TableName { get; set; }

        public string FieldList { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public string SortFields { get; set; }

        public bool EnabledSort { get; set; }

        public string QueryCondition { get; set; }

        public string Primarykey { get; set; }

        public bool EnabledDistinct { get; set; }

        public int PageCount { get; set; }

        public int RecordCount { get; set; }
    }
    public class CommandInfo
    {
        public object ShareObject = null;

        public object OriginalData = null;

        public string CommandText;

        public SqlParameter[] Parameters;

        public EffentNextType EffentNextType = EffentNextType.None;

        private event EventHandler _solicitationEvent;

        public event EventHandler SolicitationEvent
        {
            add
            {
                _solicitationEvent += value;
            }
            remove
            {
                _solicitationEvent -= value;
            }
        }

        public void OnSolicitationEvent()
        {
            if (this._solicitationEvent != null)
            {
                this._solicitationEvent(this, new EventArgs());
            }
        }

        public CommandInfo()
        {
        }

        public CommandInfo(string sqlText, SqlParameter[] para)
        {
            CommandText = sqlText;
            Parameters = para;
        }

        public CommandInfo(string sqlText, SqlParameter[] para, EffentNextType type)
        {
            CommandText = sqlText;
            Parameters = para;
            EffentNextType = type;
        }
    }
    #endregion



    #region  AdoHelper
    //public class CommonUtils
    //{
    //    /// <summary>
    //    /// 用于字符串和枚举类型的转换
    //    /// </summary>
    //    /// <typeparam name="T"></typeparam>
    //    /// <param name="value"></param>
    //    /// <returns></returns>
    //    public static T EnumParse<T>(string value)
    //    {
    //        try
    //        {
    //            return (T)Enum.Parse(typeof(T), value);
    //        }
    //        catch
    //        {
    //            throw new Exception("传入的值与枚举值不匹配。");
    //        }
    //    }

    //    /// <summary>
    //    /// 根据传入的Key获取配置文件中的Value值
    //    /// </summary>
    //    /// <param name="Key"></param>
    //    /// <returns></returns>
    //    public static string GetConfigValueByKey(string Key)
    //    {
    //        try
    //        {
    //            return ConfigurationManager.AppSettings[Key].ToString();
    //        }
    //        catch
    //        {
    //            throw new Exception("web.config中 Key=\"" + Key + "\"未配置或配置错误！");
    //        }
    //    }

    //    public static Boolean IsNullOrEmpty(Object value)
    //    {
    //        if (value == null)
    //            return true;
    //        if (String.IsNullOrEmpty(value.ToString()))
    //            return true;
    //        return false;
    //    }
    //}
    //public class DbFactory
    //{
    //    /// <summary>
    //    /// 根据配置文件中所配置的数据库类型
    //    /// 来获取命令参数中的参数符号oracle为":",sqlserver为"@"
    //    /// </summary>
    //    /// <returns></returns>
    //    public static string CreateDbParmCharacter()
    //    {
    //        string character = string.Empty;

    //        switch (AdoHelper.DbType)
    //        {
    //            case DatabaseType.SqlServer:
    //                character = "@";
    //                break;
    //            case DatabaseType.Oracle:
    //                character = ":";
    //                break;
    //            case DatabaseType.Access:
    //                character = "@";
    //                break;
    //            default:
    //                throw new Exception("数据库类型目前不支持！");
    //        }

    //        return character;
    //    }

    //    /// <summary>
    //    /// 根据配置文件中所配置的数据库类型和传入的
    //    /// 数据库链接字符串来创建相应数据库连接对象
    //    /// </summary>
    //    /// <param name="connectionString"></param>
    //    /// <returns></returns>
    //    public static IDbConnection CreateDbConnection(string connectionString)
    //    {
    //        IDbConnection conn = null;
    //        switch (AdoHelper.DbType)
    //        {
    //            case DatabaseType.SqlServer:
    //                conn = new SqlConnection(connectionString);
    //                break;
    //            case DatabaseType.Oracle:
    //                conn = new OracleConnection(connectionString);
    //                break;
    //            case DatabaseType.Access:
    //                conn = new OleDbConnection(connectionString);
    //                break;
    //            default:
    //                throw new Exception("数据库类型目前不支持！");
    //        }

    //        return conn;
    //    }

    //    /// <summary>
    //    /// 根据配置文件中所配置的数据库类型
    //    /// 来创建相应数据库命令对象
    //    /// </summary>
    //    /// <returns></returns>
    //    public static IDbCommand CreateDbCommand()
    //    {
    //        IDbCommand cmd = null;
    //        switch (AdoHelper.DbType)
    //        {
    //            case DatabaseType.SqlServer:
    //                cmd = new SqlCommand();
    //                break;
    //            case DatabaseType.Oracle:
    //                cmd = new OracleCommand();
    //                break;
    //            case DatabaseType.Access:
    //                cmd = new OleDbCommand();
    //                break;
    //            default:
    //                throw new Exception("数据库类型目前不支持！");
    //        }

    //        return cmd;
    //    }

    //    /// <summary>
    //    /// 根据配置文件中所配置的数据库类型
    //    /// 来创建相应数据库适配器对象
    //    /// </summary>
    //    /// <returns></returns>
    //    public static IDbDataAdapter CreateDataAdapter()
    //    {
    //        IDbDataAdapter adapter = null;
    //        switch (AdoHelper.DbType)
    //        {
    //            case DatabaseType.SqlServer:
    //                adapter = new SqlDataAdapter();
    //                break;
    //            case DatabaseType.Oracle:
    //                adapter = new OracleDataAdapter();
    //                break;
    //            case DatabaseType.Access:
    //                adapter = new OleDbDataAdapter();
    //                break;
    //            default:
    //                throw new Exception("数据库类型目前不支持！");
    //        }

    //        return adapter;
    //    }

    //    /// <summary>
    //    /// 根据配置文件中所配置的数据库类型
    //    /// 和传入的命令对象来创建相应数据库适配器对象
    //    /// </summary>
    //    /// <returns></returns>
    //    public static IDbDataAdapter CreateDataAdapter(IDbCommand cmd)
    //    {
    //        IDbDataAdapter adapter = null;
    //        switch (AdoHelper.DbType)
    //        {
    //            case DatabaseType.SqlServer:
    //                adapter = new SqlDataAdapter((SqlCommand)cmd);
    //                break;
    //            case DatabaseType.Oracle:
    //                adapter = new OracleDataAdapter((OracleCommand)cmd);
    //                break;
    //            case DatabaseType.Access:
    //                adapter = new OleDbDataAdapter((OleDbCommand)cmd);
    //                break;
    //            default: throw new Exception("数据库类型目前不支持！");
    //        }

    //        return adapter;
    //    }

    //    /// <summary>
    //    /// 根据配置文件中所配置的数据库类型
    //    /// 来创建相应数据库的参数对象
    //    /// </summary>
    //    /// <returns></returns>
    //    public static IDbDataParameter CreateDbParameter()
    //    {
    //        IDbDataParameter param = null;
    //        switch (AdoHelper.DbType)
    //        {
    //            case DatabaseType.SqlServer:
    //                param = new SqlParameter();
    //                break;
    //            case DatabaseType.Oracle:
    //                param = new OracleParameter();
    //                break;
    //            case DatabaseType.Access:
    //                param = new OleDbParameter();
    //                break;
    //            default:
    //                throw new Exception("数据库类型目前不支持！");
    //        }

    //        return param;
    //    }

    //    /// <summary>
    //    /// 根据配置文件中所配置的数据库类型
    //    /// 和传入的参数来创建相应数据库的参数数组对象
    //    /// </summary>
    //    /// <returns></returns>
    //    public static IDbDataParameter[] CreateDbParameters(int size)
    //    {
    //        int i = 0;
    //        IDbDataParameter[] param = null;
    //        switch (AdoHelper.DbType)
    //        {
    //            case DatabaseType.SqlServer:
    //                param = new SqlParameter[size];
    //                while (i < size) { param[i] = new SqlParameter(); i++; }
    //                break;
    //            case DatabaseType.Oracle:
    //                param = new OracleParameter[size];
    //                while (i < size) { param[i] = new OracleParameter(); i++; }
    //                break;
    //            case DatabaseType.Access:
    //                param = new OleDbParameter[size];
    //                while (i < size) { param[i] = new OleDbParameter(); i++; }
    //                break;
    //            default:
    //                throw new Exception("数据库类型目前不支持！");

    //        }

    //        return param;
    //    }

    //    /// <summary>
    //    /// 根据配置文件中所配置的数据库类型
    //    /// 来创建相应数据库的事物对象
    //    /// </summary>
    //    /// <returns></returns>
    //    public static IDbTransaction CreateDbTransaction()
    //    {
    //        IDbConnection conn = CreateDbConnection(AdoHelper.ConnectionString);

    //        if (conn.State == ConnectionState.Closed)
    //        {
    //            conn.Open();
    //        }

    //        return conn.BeginTransaction();
    //    }
    //}
    //public class AdoHelper
    //{
    //    //获取数据库类型
    //    private static string strDbType = CommonUtils.GetConfigValueByKey("dbType").ToUpper();

    //    //将数据库类型转换成枚举类型
    //    public static DatabaseType DbType = DatabaseTypeEnumParse<DatabaseType>(strDbType);

    //    //获取数据库连接字符串
    //    public static string ConnectionString = GetConnectionString("connectionString");

    //    public static string DbParmChar = DbFactory.CreateDbParmCharacter();

    //    private static Hashtable parmCache = Hashtable.Synchronized(new Hashtable());

    //    /// <summary>
    //    ///通过提供的参数，执行无结果集的数据库操作命令
    //    /// 并返回执行数据库操作所影响的行数。
    //    /// </summary>
    //    /// <param name="connectionString">数据库连接字符串</param>
    //    /// <param name="commandType">执行命令的类型（存储过程或T-SQL，等等）</param>
    //    /// <param name="commandText">存储过程名称或者T-SQL命令行<</param>
    //    /// <param name="commandParameters">执行命令所需的参数数组</param>
    //    /// <returns>返回通过执行命令所影响的行数</returns>
    //    public static int ExecuteNonQuery(string connectionString, CommandType cmdType, string cmdText, params IDbDataParameter[] commandParameters)
    //    {
    //        IDbCommand cmd = DbFactory.CreateDbCommand();

    //        using (IDbConnection conn = DbFactory.CreateDbConnection(connectionString))
    //        {
    //            PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
    //            int val = cmd.ExecuteNonQuery();
    //            cmd.Parameters.Clear();
    //            return val;
    //        }
    //    }
    //    /// <summary>
    //    ///通过提供的参数，执行无结果集的数据库操作命令
    //    /// 并返回执行数据库操作所影响的行数。
    //    /// </summary>
    //    /// <param name="connectionString">数据库连接字符串</param>
    //    /// <param name="commandType">执行命令的类型（存储过程或T-SQL，等等）</param>
    //    /// <param name="commandText">存储过程名称或者T-SQL命令行<</param>
    //    /// <returns>返回通过执行命令所影响的行数</returns>
    //    public static int ExecuteNonQuery(string connectionString, CommandType cmdType, string cmdText)
    //    {
    //        IDbCommand cmd = DbFactory.CreateDbCommand();

    //        using (IDbConnection conn = DbFactory.CreateDbConnection(connectionString))
    //        {
    //            PrepareCommand(cmd, conn, null, cmdType, cmdText, null);
    //            int val = cmd.ExecuteNonQuery();
    //            cmd.Parameters.Clear();
    //            return val;
    //        }
    //    }

    //    /// <summary>
    //    ///通过提供的参数，执行无结果集返回的数据库操作命令
    //    ///并返回执行数据库操作所影响的行数。
    //    /// </summary>
    //    /// <remarks>
    //    /// e.g.:  
    //    ///  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
    //    /// </remarks>
    //    /// <param name="conn">数据库连接对象</param>
    //    /// <param name="commandType">执行命令的类型（存储过程或T-SQL，等等）</param>
    //    /// <param name="commandText">存储过程名称或者T-SQL命令行</param>
    //    /// <param name="commandParameters">执行命令所需的参数数组</param>
    //    /// <returns>返回通过执行命令所影响的行数</returns>
    //    public static int ExecuteNonQuery(IDbConnection connection, CommandType cmdType, string cmdText, params IDbDataParameter[] commandParameters)
    //    {
    //        IDbCommand cmd = DbFactory.CreateDbCommand();

    //        PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
    //        int val = cmd.ExecuteNonQuery();
    //        cmd.Parameters.Clear();
    //        return val;
    //    }
    //    /// <summary>
    //    ///通过提供的参数，执行无结果集返回的数据库操作命令
    //    ///并返回执行数据库操作所影响的行数。
    //    /// </summary>
    //    /// <remarks>
    //    /// e.g.:  
    //    ///  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
    //    /// </remarks>
    //    /// <param name="conn">数据库连接对象</param>
    //    /// <param name="commandType">执行命令的类型（存储过程或T-SQL，等等）</param>
    //    /// <param name="commandText">存储过程名称或者T-SQL命令行</param>
    //    /// <returns>返回通过执行命令所影响的行数</returns>
    //    public static int ExecuteNonQuery(IDbConnection connection, CommandType cmdType, string cmdText)
    //    {
    //        IDbCommand cmd = DbFactory.CreateDbCommand();

    //        PrepareCommand(cmd, connection, null, cmdType, cmdText, null);
    //        int val = cmd.ExecuteNonQuery();
    //        cmd.Parameters.Clear();
    //        return val;
    //    }

    //    /// <summary>
    //    ///通过提供的参数，执行无结果集返回的数据库操作命令
    //    ///并返回执行数据库操作所影响的行数。
    //    /// </summary>
    //    /// <remarks>
    //    /// e.g.:  
    //    ///  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
    //    /// </remarks>
    //    /// <param name="trans">sql事务对象</param>
    //    /// <param name="commandType">执行命令的类型（存储过程或T-SQL，等等）</param>
    //    /// <param name="commandText">存储过程名称或者T-SQL命令行<</param>
    //    /// <param name="commandParameters">执行命令所需的参数数组</param>
    //    /// <returns>返回通过执行命令所影响的行数</returns>
    //    public static int ExecuteNonQuery(IDbTransaction trans, CommandType cmdType, string cmdText, params IDbDataParameter[] commandParameters)
    //    {
    //        IDbConnection conn = null;
    //        if (trans == null)
    //        {
    //            conn = DbFactory.CreateDbConnection(ConnectionString);
    //        }
    //        else
    //        {
    //            conn = trans.Connection;
    //        }

    //        IDbCommand cmd = DbFactory.CreateDbCommand();
    //        PrepareCommand(cmd, conn, trans, cmdType, cmdText, commandParameters);
    //        int val = cmd.ExecuteNonQuery();
    //        cmd.Parameters.Clear();
    //        return val;
    //    }
    //    /// <summary>
    //    ///通过提供的参数，执行无结果集返回的数据库操作命令
    //    ///并返回执行数据库操作所影响的行数。
    //    /// </summary>
    //    /// <remarks>
    //    /// e.g.:  
    //    ///  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
    //    /// </remarks>
    //    /// <param name="trans">sql事务对象</param>
    //    /// <param name="commandType">执行命令的类型（存储过程或T-SQL，等等）</param>
    //    /// <param name="commandText">存储过程名称或者T-SQL命令行<</param>
    //    /// <returns>返回通过执行命令所影响的行数</returns>
    //    public static int ExecuteNonQuery(IDbTransaction trans, CommandType cmdType, string cmdText)
    //    {
    //        IDbCommand cmd = DbFactory.CreateDbCommand();
    //        PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, null);
    //        int val = cmd.ExecuteNonQuery();
    //        cmd.Parameters.Clear();
    //        return val;
    //    }
    //    /// <summary>
    //    /// 使用提供的参数，执行有结果集返回的数据库操作命令
    //    /// 并返回SqlDataReader对象
    //    /// </summary>
    //    /// <remarks>
    //    /// e.g.:  
    //    ///  SqlDataReader r = ExecuteReader(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
    //    /// </remarks>
    //    /// <param name="connectionString">数据库连接字符串</param>
    //    /// <param name="commandType">执行命令的类型（存储过程或T-SQL，等等）</param>
    //    /// <param name="commandText">存储过程名称或者T-SQL命令行<</param>
    //    /// <param name="commandParameters">执行命令所需的参数数组</param>
    //    /// <returns>返回SqlDataReader对象</returns>
    //    public static IDataReader ExecuteReader(string connectionString, CommandType cmdType, string cmdText, params IDbDataParameter[] commandParameters)
    //    {
    //        IDbCommand cmd = DbFactory.CreateDbCommand();
    //        IDbConnection conn = DbFactory.CreateDbConnection(connectionString);

    //        //我们在这里使用一个 try/catch,因为如果PrepareCommand方法抛出一个异常，我们想在捕获代码里面关闭
    //        //connection连接对象，因为异常发生datareader将不会存在，所以commandBehaviour.CloseConnection
    //        //将不会执行。
    //        try
    //        {
    //            PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
    //            IDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
    //            cmd.Parameters.Clear();
    //            return rdr;
    //        }
    //        catch
    //        {
    //            conn.Close();
    //            throw;
    //        }
    //    }
    //    /// <summary>
    //    ///使用提供的参数，执行有结果集返回的数据库操作命令
    //    /// 并返回SqlDataReader对象
    //    /// </summary>
    //    /// <param name="connectionString">数据库连接字符串</param>
    //    /// <param name="commandType">执行命令的类型（存储过程或T-SQL，等等）</param>
    //    /// <param name="commandText">存储过程名称或者T-SQL命令行<</param>
    //    /// <returns>返回SqlDataReader对象</returns>
    //    public static IDataReader ExecuteReader(string connectionString, CommandType cmdType, string cmdText)
    //    {
    //        IDbCommand cmd = DbFactory.CreateDbCommand();
    //        IDbConnection conn = DbFactory.CreateDbConnection(connectionString);

    //        //我们在这里使用一个 try/catch,因为如果PrepareCommand方法抛出一个异常，我们想在捕获代码里面关闭
    //        //connection连接对象，因为异常发生datareader将不会存在，所以commandBehaviour.CloseConnection
    //        //将不会执行。
    //        try
    //        {
    //            PrepareCommand(cmd, conn, null, cmdType, cmdText, null);
    //            IDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
    //            cmd.Parameters.Clear();
    //            return rdr;
    //        }
    //        catch
    //        {
    //            conn.Close();
    //            throw;
    //        }
    //    }

    //    /// <summary>
    //    /// 查询数据填充到数据集DataSet中
    //    /// </summary>
    //    /// <param name="connectionString">数据库连接字符串</param>
    //    /// <param name="cmdType">执行命令的类型（存储过程或T-SQL，等等）</param>
    //    /// <param name="cmdText">命令文本</param>
    //    /// <param name="commandParameters">参数数组</param>
    //    /// <returns>数据集DataSet对象</returns>
    //    public static DataSet dataSet(string connectionString, CommandType cmdType, string cmdText, params IDbDataParameter[] commandParameters)
    //    {
    //        DataSet ds = new DataSet();
    //        IDbCommand cmd = DbFactory.CreateDbCommand();
    //        IDbConnection conn = DbFactory.CreateDbConnection(connectionString);
    //        try
    //        {
    //            PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
    //            IDbDataAdapter sda = DbFactory.CreateDataAdapter(cmd);
    //            sda.Fill(ds);
    //            return ds;
    //        }
    //        catch
    //        {
    //            conn.Close();
    //            throw;
    //        }
    //        finally
    //        {
    //            conn.Close();
    //            cmd.Dispose();
    //        }
    //    }

    //    /// <summary>
    //    /// 查询数据填充到数据集DataSet中
    //    /// </summary>
    //    /// <param name="connectionString">数据库连接字符串</param>
    //    /// <param name="cmdType">执行命令的类型（存储过程或T-SQL，等等）</param>
    //    /// <param name="cmdText">命令文本</param>
    //    /// <returns>数据集DataSet对象</returns>
    //    public static DataSet dataSet(string connectionString, CommandType cmdType, string cmdText)
    //    {
    //        DataSet ds = new DataSet();
    //        IDbCommand cmd = DbFactory.CreateDbCommand();
    //        IDbConnection conn = DbFactory.CreateDbConnection(connectionString);
    //        try
    //        {
    //            PrepareCommand(cmd, conn, null, cmdType, cmdText, null);
    //            IDbDataAdapter sda = DbFactory.CreateDataAdapter(cmd);
    //            sda.Fill(ds);
    //            return ds;
    //        }
    //        catch
    //        {
    //            conn.Close();
    //            throw;
    //        }
    //        finally
    //        {
    //            conn.Close();
    //            cmd.Dispose();
    //        }
    //    }

    //    /// <summary>
    //    /// 依靠数据库连接字符串connectionString,
    //    /// 使用所提供参数，执行返回首行首列命令
    //    /// </summary>
    //    /// <remarks>
    //    /// e.g.:  
    //    ///  Object obj = ExecuteScalar(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
    //    /// </remarks>
    //    /// <param name="connectionString">数据库连接字符串</param>
    //    /// <param name="commandType">执行命令的类型（存储过程或T-SQL，等等）</param>
    //    /// <param name="commandText">存储过程名称或者T-SQL命令行</param>
    //    /// <param name="commandParameters">执行命令所需的参数数组</param>
    //    /// <returns>返回一个对象，使用Convert.To{Type}将该对象转换成想要的数据类型。</returns>
    //    public static object ExecuteScalar(string connectionString, CommandType cmdType, string cmdText, params IDbDataParameter[] commandParameters)
    //    {
    //        IDbCommand cmd = DbFactory.CreateDbCommand();

    //        using (IDbConnection connection = DbFactory.CreateDbConnection(connectionString))
    //        {
    //            PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
    //            object val = cmd.ExecuteScalar();
    //            cmd.Parameters.Clear();
    //            return val;
    //        }
    //    }
    //    /// <summary>
    //    /// 依靠数据库连接字符串connectionString,
    //    /// 使用所提供参数，执行返回首行首列命令
    //    /// </summary>
    //    /// <remarks>
    //    /// e.g.:  
    //    ///  Object obj = ExecuteScalar(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
    //    /// </remarks>
    //    /// <param name="connectionString">数据库连接字符串</param>
    //    /// <param name="commandType">执行命令的类型（存储过程或T-SQL，等等）</param>
    //    /// <param name="commandText">存储过程名称或者T-SQL命令行</param>
    //    /// <returns>返回一个对象，使用Convert.To{Type}将该对象转换成想要的数据类型。</returns>
    //    public static object ExecuteScalar(string connectionString, CommandType cmdType, string cmdText)
    //    {
    //        IDbCommand cmd = DbFactory.CreateDbCommand();

    //        using (IDbConnection connection = DbFactory.CreateDbConnection(connectionString))
    //        {
    //            PrepareCommand(cmd, connection, null, cmdType, cmdText, null);
    //            object val = cmd.ExecuteScalar();
    //            cmd.Parameters.Clear();
    //            return val;
    //        }
    //    }
    //    /// <summary>
    //    ///依靠数据库连接字符串connectionString,
    //    /// 使用所提供参数，执行返回首行首列命令
    //    /// </summary>
    //    /// <remarks>
    //    /// e.g.:  
    //    ///  Object obj = ExecuteScalar(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
    //    /// </remarks>
    //    /// <param name="conn">数据库连接对象</param>
    //    /// <param name="commandType">执行命令的类型（存储过程或T-SQL，等等）</param>
    //    /// <param name="commandText">存储过程名称或者T-SQL命令行</param>
    //    /// <param name="commandParameters">执行命令所需的参数数组</param>
    //    /// <returns>返回一个对象，使用Convert.To{Type}将该对象转换成想要的数据类型。</returns>
    //    public static object ExecuteScalar(IDbConnection connection, CommandType cmdType, string cmdText, params IDbDataParameter[] commandParameters)
    //    {
    //        IDbCommand cmd = DbFactory.CreateDbCommand();

    //        PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
    //        object val = cmd.ExecuteScalar();
    //        cmd.Parameters.Clear();
    //        return val;
    //    }
    //    /// <summary>
    //    ///依靠数据库连接字符串connectionString,
    //    /// 使用所提供参数，执行返回首行首列命令
    //    /// </summary>
    //    /// <remarks>
    //    /// e.g.:  
    //    ///  Object obj = ExecuteScalar(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
    //    /// </remarks>
    //    /// <param name="conn">数据库连接对象</param>
    //    /// <param name="commandType">执行命令的类型（存储过程或T-SQL，等等）</param>
    //    /// <param name="commandText">存储过程名称或者T-SQL命令行</param>
    //    /// <param name="commandParameters">执行命令所需的参数数组</param>
    //    /// <returns>返回一个对象，使用Convert.To{Type}将该对象转换成想要的数据类型。</returns>
    //    public static object ExecuteScalar(IDbConnection connection, CommandType cmdType, string cmdText)
    //    {
    //        IDbCommand cmd = DbFactory.CreateDbCommand();

    //        PrepareCommand(cmd, connection, null, cmdType, cmdText, null);
    //        object val = cmd.ExecuteScalar();
    //        cmd.Parameters.Clear();
    //        return val;
    //    }


    //    /// <summary>
    //    /// add parameter array to the cache
    //    /// </summary>
    //    /// <param name="cacheKey">Key to the parameter cache</param>
    //    /// <param name="cmdParms">an array of SqlParamters to be cached</param>
    //    public static void CacheParameters(string cacheKey, params IDbDataParameter[] commandParameters)
    //    {
    //        parmCache[cacheKey] = commandParameters;
    //    }

    //    /// <summary>
    //    /// 查询缓存参数
    //    /// </summary>
    //    /// <param name="cacheKey">使用缓存名称查找值</param>
    //    /// <returns>缓存参数数组</returns>
    //    public static IDbDataParameter[] GetCachedParameters(string cacheKey)
    //    {
    //        IDbDataParameter[] cachedParms = (IDbDataParameter[])parmCache[cacheKey];

    //        if (cachedParms == null)
    //            return null;

    //        IDbDataParameter[] clonedParms = new IDbDataParameter[cachedParms.Length];

    //        for (int i = 0, j = cachedParms.Length; i < j; i++)
    //            clonedParms[i] = (IDbDataParameter)((ICloneable)cachedParms[i]).Clone();

    //        return clonedParms;
    //    }

    //    /// <summary>
    //    /// 为即将执行准备一个命令
    //    /// </summary>
    //    /// <param name="cmd">SqlCommand对象</param>
    //    /// <param name="conn">SqlConnection对象</param>
    //    /// <param name="trans">IDbTransaction对象</param>
    //    /// <param name="cmdType">执行命令的类型（存储过程或T-SQL，等等）</param>
    //    /// <param name="cmdText">存储过程名称或者T-SQL命令行, e.g. Select * from Products</param>
    //    /// <param name="cmdParms">SqlParameters to use in the command</param>
    //    private static void PrepareCommand(IDbCommand cmd, IDbConnection conn, IDbTransaction trans, CommandType cmdType, string cmdText, IDbDataParameter[] cmdParms)
    //    {
    //        if (conn.State != ConnectionState.Open)
    //            conn.Open();

    //        cmd.Connection = conn;
    //        cmd.CommandText = cmdText;

    //        if (trans != null)
    //            cmd.Transaction = trans;

    //        cmd.CommandType = cmdType;

    //        if (cmdParms != null)
    //        {
    //            foreach (IDbDataParameter parm in cmdParms)
    //                cmd.Parameters.Add(parm);
    //        }
    //    }

    //    /// <summary>
    //    /// 根据传入的Key获取配置文件中
    //    /// 相应Key的数据库连接字符串
    //    /// </summary>
    //    /// <param name="Key"></param>
    //    /// <returns></returns>
    //    public static string GetConnectionString(string Key)
    //    {
    //        try
    //        {
    //            return CommonUtils.GetConfigValueByKey(Key);
    //        }
    //        catch
    //        {
    //            throw new Exception("web.config文件appSettings中数据库连接字符串未配置或配置错误，必须为Key=\"connectionString\"");
    //        }
    //    }

    //    /// <summary>
    //    /// 用于数据库类型的字符串枚举转换
    //    /// </summary>
    //    /// <typeparam name="T"></typeparam>
    //    /// <param name="value"></param>
    //    /// <returns></returns>
    //    public static T DatabaseTypeEnumParse<T>(string value)
    //    {
    //        try
    //        {
    //            return CommonUtils.EnumParse<T>(value);
    //        }
    //        catch
    //        {
    //            throw new Exception("数据库类型\"" + value + "\"错误，请检查！");
    //        }
    //    }
    //}

    //public class TypeUtils
    //{
    //    public static object ConvertForType(object value, Type type)
    //    {
    //        switch (type.FullName)
    //        {
    //            case "System.String":
    //                value = value.ToString();
    //                break;
    //            case "System.Boolean":
    //                value = bool.Parse(value.ToString());
    //                break;
    //            case "System.Int16":
    //            case "System.Int32":
    //            case "System.Int64":
    //                value = int.Parse(value.ToString());
    //                break;
    //            case "System.Double":
    //                value = double.Parse(value.ToString());
    //                break;
    //            case "System.Decimal":
    //                value = new decimal(double.Parse(value.ToString()));
    //                break;
    //        }

    //        return value;
    //    }
    //}
    #endregion


    #region  ORMDBhelper
    public partial class ORMDBhelper<T> where T : class, new()
    {
        //获取连接字符串
        private static string con = System.Configuration.ConfigurationManager.ConnectionStrings["con"].ConnectionString;


        #region 动态查询的方式使用泛型+反射
        //sql语句 select *from student
        public static List<T> Query(string where)
        {
            DataTable tb = new DataTable();
            List<T> list = new List<T>();

            //
            string sql = GetQuerySql();
            sql += where;
            //用反射赋值
            using (SqlConnection connection = new SqlConnection(con))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command);
                    sqlDataAdapter.Fill(tb);
                    //获取T的类型
                    Type type = typeof(T);
                    //循环循环行
                    for (int i = 0; i < tb.Rows.Count; i++)
                    {
                        //实例化T，每一次都需要实例化new 对象
                        Object obj = Activator.CreateInstance(type);
                        //循环列
                        for (int j = 0; j < tb.Columns.Count; j++)
                        {
                            //获取列的名称 student s_id  GetProperty("s_id")
                            PropertyInfo info = type.GetProperty(tb.Columns[j].ColumnName);//赋值了s_id

                            //判断类型 tb.Columns[j].DataType 获取数据库列的类型

                            #region 类型的判断并赋值
                            //int类型


                            if (tb.Columns[j].DataType == typeof(Int32))
                            {
                                //有没有可能空值？
                                if (tb.Rows[i][j] != null)
                                {
                                    //obj.setValue(info,12);
                                    info.SetValue(obj, int.Parse(tb.Rows[i][j].ToString()), null);
                                }
                                else
                                {
                                    //null值的情况
                                    info.SetValue(obj, 0, null);
                                }
                            }

                            //float类型
                            else if (tb.Columns[j].DataType == typeof(float))
                            {
                                //有没有可能空值？
                                if (tb.Rows[i][j] != null)
                                {
                                    info.SetValue(obj, float.Parse(tb.Rows[i][j].ToString()), null);
                                }
                                else
                                {
                                    //null值的情况
                                    info.SetValue(obj, 0.0f, null);
                                }
                            }

                            //datetime
                            else if (tb.Columns[j].DataType == typeof(DateTime))
                            {
                                //有没有可能空值？
                                if (tb.Rows[i][j] != null)
                                {
                                    info.SetValue(obj, DateTime.Parse(tb.Rows[i][j].ToString()), null);
                                }
                                else
                                {
                                    //null值的情况
                                    info.SetValue(obj, DateTime.Now, null);
                                }
                            }

                            //double
                            else if (tb.Columns[j].DataType == typeof(double))
                            {
                                //有没有可能空值？
                                if (tb.Rows[i][j] != null)
                                {
                                    info.SetValue(obj, double.Parse(tb.Rows[i][j].ToString()), null);
                                }
                                else
                                {
                                    //null值的情况
                                    info.SetValue(obj, 0.00, null);
                                }
                            }
                            //GUID
                            else if (tb.Columns[j].DataType == typeof(Guid))
                            {
                                //有没有可能空值？
                                if (tb.Rows[i][j] != null && !tb.Rows[i][j].ToString().Equals(""))
                                {
                                    info.SetValue(obj, Guid.Parse(tb.Rows[i][j].ToString()), null);
                                }
                                else
                                {
                                    //null值的情况
                                    info.SetValue(obj, Guid.Parse("00000000-0000-0000-0000-000000000000"), null);
                                }

                            }
                            else
                            {
                                //string

                                //有没有可能空值？
                                if (tb.Rows[i][j] != null)
                                {
                                    info.SetValue(obj, tb.Rows[i][j].ToString(), null);
                                }
                                else
                                {
                                    //null值的情况
                                    info.SetValue(obj, "", null);
                                }
                            }
                            #endregion



                        }
                        //将object 类型强转对应的类型
                        list.Add((T)obj);//（类型）强制转换
                    }
                }
            }
            return list;
        }
        //通过字符串方式进行查询
        public static List<T> Query(string where, string sql)
        {
            DataTable tb = new DataTable();
            List<T> list = new List<T>();

            sql += where;
            //用反射赋值
            using (SqlConnection connection = new SqlConnection(con))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command);
                    sqlDataAdapter.Fill(tb);
                    //获取T的类型
                    Type type = typeof(T);
                    //循环循环行
                    for (int i = 0; i < tb.Rows.Count; i++)
                    {
                        //实例化T，每一次都需要实例化new 对象
                        Object obj = Activator.CreateInstance(type);
                        //循环列
                        for (int j = 0; j < tb.Columns.Count; j++)
                        {
                            //获取列的名称 student s_id  GetProperty("s_id")
                            PropertyInfo info = type.GetProperty(tb.Columns[j].ColumnName);//赋值了s_id

                            //判断类型 tb.Columns[j].DataType 获取数据库列的类型

                            #region 类型的判断并赋值
                            //int类型
                            if (tb.Columns[j].DataType == typeof(Int32))
                            {
                                //有没有可能空值？
                                if (tb.Rows[i][j] != null)
                                {
                                    //obj.setValue(info,12);
                                    info.SetValue(obj, int.Parse(tb.Rows[i][j].ToString()), null);
                                }
                                else
                                {
                                    //null值的情况
                                    info.SetValue(obj, 0, null);
                                }
                            }

                            //float类型
                            else if (tb.Columns[j].DataType == typeof(float))
                            {
                                //有没有可能空值？
                                if (tb.Rows[i][j] != null)
                                {
                                    info.SetValue(obj, float.Parse(tb.Rows[i][j].ToString()), null);
                                }
                                else
                                {
                                    //null值的情况
                                    info.SetValue(obj, 0.0f, null);
                                }
                            }

                            //datetime
                            else if (tb.Columns[j].DataType == typeof(DateTime))
                            {
                                //有没有可能空值？
                                if (tb.Rows[i][j] != null && !tb.Rows[i][j].ToString().Equals(""))
                                {
                                    info.SetValue(obj, DateTime.Parse(tb.Rows[i][j].ToString()), null);
                                }
                                else
                                {
                                    //null值的情况
                                    info.SetValue(obj, DateTime.Now, null);
                                }
                            }

                            //double
                            else if (tb.Columns[j].DataType == typeof(double))
                            {
                                //有没有可能空值？
                                if (tb.Rows[i][j] != null)
                                {
                                    info.SetValue(obj, double.Parse(tb.Rows[i][j].ToString()), null);
                                }
                                else
                                {
                                    //null值的情况
                                    info.SetValue(obj, 0.00, null);
                                }
                            }
                            //GUID
                            else if (tb.Columns[j].DataType == typeof(Guid))
                            {
                                //有没有可能空值？
                                if (tb.Rows[i][j] != null && !tb.Rows[i][j].ToString().Equals(""))
                                {
                                    info.SetValue(obj, Guid.Parse(tb.Rows[i][j].ToString()), null);
                                }
                                else
                                {
                                    //null值的情况
                                    info.SetValue(obj, Guid.Parse("00000000-0000-0000-0000-000000000000"), null);
                                }

                            }
                            else
                            {
                                //string

                                //有没有可能空值？
                                if (tb.Rows[i][j] != null)
                                {
                                    info.SetValue(obj, tb.Rows[i][j].ToString(), null);
                                }
                                else
                                {
                                    //null值的情况
                                    info.SetValue(obj, "", null);
                                }
                            }
                            #endregion



                        }
                        //将object 类型强转对应的类型
                        list.Add((T)obj);//（类型）强制转换
                    }
                }
            }
            return list;
        }
        //使用T-sql查询
        public static string QuerySign(string sql)
        {
            DataTable tb = new DataTable();
            using (SqlConnection connection = new SqlConnection(con))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command);
                    sqlDataAdapter.Fill(tb);
                }
            }
            if (tb != null && tb.Rows.Count > 0)
            {
                return tb.Rows[0][0].ToString();
            }
            else
            {
                return "";
            }
        }

        //获取sql
        public static string GetQuerySql()
        {
            Type type = typeof(T);
            //type.Name获取类的名称
            //无需实例化
            string sql = "";
            //判断表是否包含Relation的名称
            sql = "select * from " + type.Name.Replace("Models", "") + " where 1=1 ";
            return sql;
        }
        #endregion

        #region 动态添加的操作
        public static int Insert(T models)
        {
            int flag = 0;
            //获取sql
            string sql = GetInsertSql(models);
            using (SqlConnection connection = new SqlConnection(con))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    flag = command.ExecuteNonQuery();
                }
            }
            return flag;
        }

        public static string GetInsertSql(T models)
        {
            //已实例化的实体用GetType，如果未实例化的我们需要使用typeof
            Type type = models.GetType();//new 过的对象
            //先获取所有的字段
            PropertyInfo[] info = type.GetProperties();
            //这里是字段
            string field = "";
            //获取值
            string value = "";
            for (int i = 0; i < info.Length; i++)
            {
                //有可能字段没有值，没有值的我们不添加info 是属性[i]第几个属性
                if (info[i].GetValue(models) != null)
                {
                    if (!info[i].Name.Contains("Id"))
                        //获取字段和值
                        if ((i + 1) == info.Length)//代表最后一个循环不要,
                        {
                            field += info[i].Name;
                            value += "'" + info[i].GetValue(models).ToString() + "'";//为什么没有用类型判断，
                        }
                        else
                        {
                            field += info[i].Name + ",";
                            value += "'" + info[i].GetValue(models).ToString() + "',";//为什么没有用类型判断，
                        }
                }
            }
            //生成了sql语句
            string sql = "insert into " + type.Name.Replace("Models", "") + "(" + field + ") values(" + value + ")";
            return sql;
        }
        #endregion

        #region 动态修改的操作
        public static int Update(string field, T models, string where)
        {
            int flag = 0;
            //获取sql
            string sql = GetUpdateSql(field, models, where);

            using (SqlConnection connection = new SqlConnection(con))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    flag = command.ExecuteNonQuery();
                }
            }
            return flag;
        }
        //修改
        public static int UpdateToField(string sql)
        {
            int flag = 0;
            //获取sql


            using (SqlConnection connection = new SqlConnection(con))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    flag = command.ExecuteNonQuery();
                }
            }
            return flag;
        }


        public static string GetUpdateSql(string field, T models, string where)
        {
            Type type = models.GetType();
            //获取所有的字段
            string updateStr = "";
            PropertyInfo[] propertyInfos = type.GetProperties();
            for (int i = 0; i < propertyInfos.Length; i++)
            {
                //包含字段再进入
                if (propertyInfos[i].Name.Equals(field))
                {
                    if (propertyInfos[i].GetValue(models) != null)
                    {
                        updateStr += propertyInfos[i].Name + "='" + propertyInfos[i].GetValue(models) + "',";

                    }
                }
            }
            updateStr = updateStr.Substring(0, updateStr.Length - 1);
            //update biao set ziduan =zhi where userNAME=
            string sql = "update " + type.Name + " set " + updateStr + " where 1=1 " + where;
            return sql;
        }

        #endregion

        #region 动态删除
        public static int Delete(T models, string where)
        {
            int flag = 0;
            //获取sql
            string sql = GetDeleteSql(models, where);

            using (SqlConnection connection = new SqlConnection(con))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    flag = command.ExecuteNonQuery();
                }
            }
            return flag;
        }

        //删除的sql语句
        public static string GetDeleteSql(T models, string where)
        {
            Type type = models.GetType();
            //获取所有的字段


            //delete from 表 where
            string sql = "delete from  " + type.Name + "  where 1=1 " + where;
            return sql;
        }

        #endregion

    }
    #endregion
}
