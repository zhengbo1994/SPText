using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace SPText
{
    public class DBHelper
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
}
