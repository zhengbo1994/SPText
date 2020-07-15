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
        //public static string strConn = ConfigurationManager.AppSettings["DataContext"];

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
}
