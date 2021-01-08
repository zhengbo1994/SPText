using Dapper;
using MySql.Data.MySqlClient;
using SPText.Common.DataHelper.Sql;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;

namespace SPText.Common.DataHelper.Dapper
{
    public partial class DapperHelper<T>
    {
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="sql">查询的sql</param>
        /// <param name="param">替换参数</param>
        /// <returns></returns>
        public static List<T> Query(string sql, object param)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                return con.Query<T>(sql, param).ToList();
            }
        }

        /// <summary>
        /// 查询第一个数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static T QueryFirst(string sql, object param)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                return con.QueryFirst<T>(sql, param);
            }
        }

        /// <summary>
        /// 查询第一个数据没有返回默认值
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static T QueryFirstOrDefault(string sql, object param)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                return con.QueryFirstOrDefault<T>(sql, param);
            }
        }

        /// <summary>
        /// 查询单条数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static T QuerySingle(string sql, object param)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                return con.QuerySingle<T>(sql, param);
            }
        }

        /// <summary>
        /// 查询单条数据没有返回默认值
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static T QuerySingleOrDefault(string sql, object param)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                return con.QuerySingleOrDefault<T>(sql, param);
            }
        }

        /// <summary>
        /// 增删改
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static int Execute(string sql, object param)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                return con.Execute(sql, param);
            }
        }

        /// <summary>
        /// Reader获取数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static IDataReader ExecuteReader(string sql, object param)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                return con.ExecuteReader(sql, param);
            }
        }

        /// <summary>
        /// Scalar获取数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static object ExecuteScalar(string sql, object param)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                return con.ExecuteScalar(sql, param);
            }
        }

        /// <summary>
        /// Scalar获取数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static T ExecuteScalarForT(string sql, object param)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                return con.ExecuteScalar<T>(sql, param);
            }
        }

        /// <summary>
        /// 带参数的存储过程
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static List<T> ExecutePro(string proc, object param)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                List<T> list = con.Query<T>(proc,
                    param,
                    null,
                    true,
                    null,
                    CommandType.StoredProcedure).ToList();
                return list;
            }
        }


        /// <summary>
        /// 事务1 - 全SQL
        /// </summary>
        /// <param name="sqlarr">多条SQL</param>
        /// <param name="param">param</param>
        /// <returns></returns>
        public static int ExecuteTransaction(string[] sqlarr)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (var transaction = con.BeginTransaction())
                {
                    try
                    {
                        int result = 0;
                        foreach (var sql in sqlarr)
                        {
                            result += con.Execute(sql, null, transaction);
                        }

                        transaction.Commit();
                        return result;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return 0;
                    }
                }
            }
        }

        /// <summary>
        /// 事务2 - 声明参数
        ///demo:
        ///dic.Add("Insert into Users values (@UserName, @Email, @Address)",
        ///        new { UserName = "jack", Email = "380234234@qq.com", Address = "上海" });
        /// </summary>
        /// <param name="Key">多条SQL</param>
        /// <param name="Value">param</param>
        /// <returns></returns>
        public static int ExecuteTransaction(Dictionary<string, object> dic)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (var transaction = con.BeginTransaction())
                {
                    try
                    {
                        int result = 0;
                        foreach (var sql in dic)
                        {
                            result += con.Execute(sql.Key, sql.Value, transaction);
                        }

                        transaction.Commit();
                        return result;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return 0;
                    }
                }
            }
        }
    }


    public partial class DapperHelper
    {
        /// 数据库连接名
        private static string _connection = string.Empty;

        /// 获取连接名        
        private static string Connection
        {
            get { return _connection; }
            //set { _connection = value; }
        }

        /// 返回连接实例        
        private static IDbConnection dbConnection = null;

        /// 静态变量保存类的实例        
        private static DapperHelper uniqueInstance;

        /// 定义一个标识确保线程同步        
        private static readonly object locker = new object();
        /// <summary>
        /// 私有构造方法，使外界不能创建该类的实例，以便实现单例模式
        /// </summary>
        private DapperHelper()
        {
            // 这里为了方便演示直接写的字符串，实例项目中可以将连接字符串放在配置文件中，再进行读取。
            _connection = @"server=.;uid=sa;pwd=sasasa;database=Dapper";
        }

        /// <summary>
        /// 获取实例，这里为单例模式，保证只存在一个实例
        /// </summary>
        /// <returns></returns>
        public static DapperHelper GetInstance()
        {
            // 双重锁定实现单例模式，在外层加个判空条件主要是为了减少加锁、释放锁的不必要的损耗
            if (uniqueInstance == null)
            {
                lock (locker)
                {
                    if (uniqueInstance == null)
                    {
                        uniqueInstance = new DapperHelper();
                    }
                }
            }
            return uniqueInstance;
        }


        /// <summary>
        /// 创建数据库连接对象并打开链接
        /// </summary>
        /// <returns></returns>
        public static IDbConnection OpenCurrentDbConnection()
        {
            if (dbConnection == null)
            {
                dbConnection = new SqlConnection(Connection);
            }
            //判断连接状态
            if (dbConnection.State == ConnectionState.Closed)
            {
                dbConnection.Open();
            }
            return dbConnection;
        }
    }


    public static class DbContext
    {
        // 获取开启数据库的连接
        private static IDbConnection Db
        {
            get
            {
                //创建单一实例
                DapperHelper.GetInstance();
                return DapperHelper.OpenCurrentDbConnection();
            }
        }

        /// <summary>
        /// 查出一条记录的实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static T QueryFirstOrDefault<T>(string sql, object param = null)
        {
            return Db.QueryFirstOrDefault<T>(sql, param);
        }

        public static Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param = null)
        {
            return Db.QueryFirstOrDefaultAsync<T>(sql, param);
        }
        /// <summary>
        /// 查出多条记录的实体泛型集合
        /// </summary>
        /// <typeparam name="T">泛型T</typeparam>
        /// <returns></returns>
        public static IEnumerable<T> Query<T>(string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            return Db.Query<T>(sql, param, transaction, buffered, commandTimeout, commandType);
        }

        public static Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return Db.QueryAsync<T>(sql, param, transaction, commandTimeout, commandType);
        }

        public static int Execute(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return Db.Execute(sql, param, transaction, commandTimeout, commandType);
        }

        public static Task<int> ExecuteAsync(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return Db.ExecuteAsync(sql, param, transaction, commandTimeout, commandType);
        }

        public static T ExecuteScalar<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return Db.ExecuteScalar<T>(sql, param, transaction, commandTimeout, commandType);
        }

        public static Task<T> ExecuteScalarAsync<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return Db.ExecuteScalarAsync<T>(sql, param, transaction, commandTimeout, commandType);
        }

        /// <summary>
        /// 同时查询多张表数据（高级查询）
        /// "select *from K_City;select *from K_Area";
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static SqlMapper.GridReader QueryMultiple(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return Db.QueryMultiple(sql, param, transaction, commandTimeout, commandType);
        }
        public static Task<SqlMapper.GridReader> QueryMultipleAsync(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return Db.QueryMultipleAsync(sql, param, transaction, commandTimeout, commandType);
        }
    }


    public partial class DBHelper
    {
        public static int DeleteModel<T>(ref T model)
        {
            return DeleteModel<T>(typeof(T).Name, ref model);
        }

        public static int DeleteModel<T>(ref T model, params string[] KeyColumn)
        {
            return DeleteModel<T>(typeof(T).Name, ref model, KeyColumn);
        }

        public static int DeleteModel<T>(string TableName, ref T model)
        {
            return DeleteModel<T>(TableName, ref model, new string[] { "ID" });
        }

        public static int DeleteModel<T>(string TableName, ref T model, params string[] KeyColumn)
        {
            int num = -1;
            string sql = "";
            StringBuilder builder = new StringBuilder("delete " + TableName);
            builder.Append(" where ");
            bool flag = false;
            foreach (string str2 in KeyColumn)
            {
                flag = true;
                builder.Append(str2 + "=?" + str2);
                builder.Append(" and ");
            }
            if (flag)
            {
                sql = builder.Remove(builder.Length - 5, 5).ToString();
            }
            else
            {
                sql = builder.ToString();
            }
            DynamicParameters param = new DynamicParameters();
            param.AddDynamicParams((T)model);
            using (IDbConnection connection = GetMySqlOpenConnection(false))
            {
                num = connection.Execute(sql, param, null, null, null);
                connection.Close();
            }
            return num;
        }

        public static int Execute(string sqlCommandText, object Params)
        {
            int num = -1;
            using (IDbConnection connection = GetMySqlOpenConnection(false))
            {
                num = connection.Execute(sqlCommandText, Params, null, null, null);
                connection.Close();
            }
            return num;
        }

        public static T ExecuteScaler<T>(string sqlCommandText, object Params)
        {
            DynamicParameters param = new DynamicParameters();
            param.AddDynamicParams(Params);
            using (IDbConnection connection = GetMySqlOpenConnection(false))
            {
                T local = connection.Query<T>(sqlCommandText, param, null, true, null, null).First<T>();
                connection.Close();
                return local;
            }
        }

        public static List<T> GetAllModelList<T>()
        {
            return GetModelListByModelTable<T>(null, null);
        }

        public static T GetModel<T>(object KeyColumn) where T : new()
        {
            return GetModel<T>((default(T) == null) ? Activator.CreateInstance<T>() : default(T), KeyColumn);
        }

        public static T GetModel<T>(T model, object KeyColumn) where T : new()
        {
            return GetModel<T>(typeof(T).Name, model, KeyColumn);
        }

        public static T GetModel<T>(string TableName, object KeyColumn) where T : new()
        {
            return GetModel<T>(TableName, (default(T) == null) ? Activator.CreateInstance<T>() : default(T), KeyColumn);
        }

        public static T GetModel<T>(string TableName, T model, object KeyColumn) where T : new()
        {
            T local = (default(T) == null) ? Activator.CreateInstance<T>() : default(T);
            StringBuilder builder = new StringBuilder("select ");
            builder.Append(" * ");
            builder.Append(" from " + TableName);
            if (KeyColumn != null)
            {
                Type type = KeyColumn.GetType();
                builder.Append(" where ");
                foreach (PropertyInfo info in type.GetProperties())
                {
                    string name = info.Name;
                    builder.Append(name.TrimStart(new char[] { '?' }) + "=?" + name + " and ");
                }
                builder.Remove(builder.Length - 5, 5);
            }
            List<T> modelList = GetModelList<T>(builder.ToString(), KeyColumn);
            if ((modelList != null) && (modelList.Count > 0))
            {
                return modelList[0];
            }
            return default(T);
        }

        public static List<T> GetModelList<T>(string sql)
        {
            List<T> list = new List<T>();
            using (IDbConnection connection = GetMySqlOpenConnection(false))
            {
                list = connection.Query<T>(sql, null, null, true, null, null).AsList<T>();
                connection.Close();
            }
            return list;
        }

        public static List<T> GetModelList<T>(string sql, object Param)
        {
            List<T> list = new List<T>();
            DynamicParameters param = new DynamicParameters();
            param.AddDynamicParams(Param);
            using (IDbConnection connection = GetMySqlOpenConnection(false))
            {
                list = connection.Query<T>(sql, param, null, true, null, null).AsList<T>();
                connection.Close();
            }
            return list;
        }

        public static List<T> GetModelList<U, V, T>(string sql, System.Func<U, V, T> map, object Param)
        {
            List<T> list = new List<T>();
            using (IDbConnection connection = GetMySqlOpenConnection(false))
            {
                list = connection.Query<U, V, T>(sql, map, Param, null, true, "Id", null, null).AsList<T>();
                connection.Close();
            }
            return list;
        }

        public static List<T> GetModelList<T>(string sql, object Param, CommandType Type)
        {
            List<T> list = new List<T>();
            DynamicParameters param = new DynamicParameters();
            param.AddDynamicParams(Param);
            using (IDbConnection connection = GetMySqlOpenConnection(false))
            {
                list = connection.Query<T>(sql, param, null, true, null, new CommandType?(Type)).AsList<T>();
                connection.Close();
            }
            return list;
        }

        public static List<T> GetModelList<T>(string TableName, string Where, object Param)
        {
            StringBuilder builder = new StringBuilder("select * from ");
            builder.Append(TableName);
            if ((Where != null) && (Where != ""))
            {
                builder.Append(" where ");
                builder.Append(Where);
            }
            return GetModelList<T>(builder.ToString(), Param);
        }

        public static List<T> GetModelList<T>(string TableName, string Where, object Param, CommandType Type)
        {
            StringBuilder builder = new StringBuilder("select * from ");
            builder.Append(TableName);
            if ((Where != null) && (Where != ""))
            {
                builder.Append(" where ");
                builder.Append(Where);
            }
            return GetModelList<T>(builder.ToString(), Param, Type);
        }

        public static List<T> GetModelListByModelTable<T>(string Where, object Param)
        {
            return GetModelList<T>(typeof(T).Name, Where, Param);
        }

        public static List<T> GetModelListPage<T>(string sqlCommandText, string OrderBy, int startIndex, int endIndex, object Params)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT * FROM ( ");
            builder.Append(" SELECT ROW_NUMBER() OVER (");
            if (string.IsNullOrEmpty(OrderBy.Trim()))
            {
                throw new Exception("排序字段不能为空");
            }
            builder.Append("order by T." + OrderBy);
            builder.Append(")AS Row, T.*  from (");
            builder.Append(sqlCommandText);
            builder.Append(") as T ) TT");
            builder.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return GetModelList<T>(builder.ToString(), Params);
        }

        public static List<T> GetModelListPageByTBName<T>(string Where, string OrderBy, int startIndex, int endIndex, object Params = null)
        {
            return GetModelListPageByTBName<T>(typeof(T).Name, Where, OrderBy, startIndex, endIndex, Params);
        }

        public static List<T> GetModelListPageByTBName<T>(string TableName, string Where, string OrderBy, int startIndex, int endIndex, object Params = null)
        {
            StringBuilder builder = new StringBuilder("select * from " + TableName);
            if ((Where != null) && (Where != ""))
            {
                builder.Append(" where ");
                builder.Append(Where);
            }
            builder.Append(" order by ");
            builder.Append(OrderBy);
            builder.Append(" limit ");
            builder.Append(startIndex);
            builder.Append(",");
            builder.Append((int)(endIndex - startIndex));
            return GetModelList<T>(builder.ToString(), Params);
        }

        public static int GetRecordCount(string sqlCommandText, object Params)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("select count(1) FROM (");
            builder.Append(sqlCommandText);
            builder.Append(") as TB");
            return ExecuteScaler<int>(builder.ToString(), Params);
        }

        public static int GetRecordCountByTBName<T>(string Where, object Params)
        {
            return GetRecordCountByTBName(typeof(T).Name, Where, Params);
        }

        public static int GetRecordCountByTBName(string TableName, string Where, object Params)
        {
            StringBuilder builder = new StringBuilder("select * from " + TableName);
            if ((Where != null) && (Where != ""))
            {
                builder.Append(" where ");
                builder.Append(Where);
            }
            return GetRecordCount(builder.ToString(), Params);
        }

        public static int InsertModel<T>(ref T model) where T : class
        {
            return InsertModel<T>(typeof(T).Name, ref model);
        }

        public static int InsertModel<T>(string[] AutoUpdateColumnName, ref T model) where T : class
        {
            return InsertModel<T>(typeof(T).Name, AutoUpdateColumnName, ref model);
        }

        public static int InsertModel<T>(string TableName, ref T model)
        {
            return InsertModel<T>(TableName, new string[] { "id" }, ref model);
        }

        public static int InsertModel<T>(string TableName, string[] AutoUpdateColumnName, ref T model)
        {
            int num = -1;
            string str = "";
            object obj2 = BMACache.Get("InsertSql");
            PropertyInfo[] properties = model.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            MethodInfo[] methods = model.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance);
            if ((obj2 == null) || !((Dictionary<MethodInfo[], string>)obj2).Keys.Contains<MethodInfo[]>(methods))
            {
                Dictionary<MethodInfo[], string> dictionary;
                StringBuilder builder = new StringBuilder("insert into " + TableName);
                StringBuilder builder2 = new StringBuilder();
                foreach (PropertyInfo info in properties)
                {
                    IEnumerable<Attribute> customAttributes = info.GetCustomAttributes(typeof(ModelAttribute));
                    if (((customAttributes.Count<Attribute>() > 0) && (customAttributes.First<Attribute>().GetType() == typeof(ModelAttribute))) && (((ModelAttribute)customAttributes.First<Attribute>()).Name == ModelAttributeType.TableColumn))
                    {
                        string str2 = info.Name.ToLower();
                        if ((AutoUpdateColumnName == null) || !AutoUpdateColumnName.Contains<string>(str2))
                        {
                            builder2.Append(str2);
                            builder2.Append(",");
                        }
                    }
                }
                builder.Append("(");
                builder.Append(builder2.ToString().TrimEnd(new char[] { ',' }));
                builder.Append(") values(?");
                builder.Append(builder2.ToString().TrimEnd(new char[] { ',' }).Replace(",", ",?"));
                builder.Append(")");
                if ((AutoUpdateColumnName != null) && (AutoUpdateColumnName.Length > 0))
                {
                    builder.Append(";select ?NewInsertID=SCOPE_IDENTITY()");//;select @@IDENTITY
                    //builder.Append(";select @@IDENTITY");
                }
                str = builder.ToString();
                if (obj2 == null)
                {
                    dictionary = new Dictionary<MethodInfo[], string>();
                }
                else
                {
                    dictionary = (Dictionary<MethodInfo[], string>)obj2;
                }
                dictionary.Add(methods, str);
                BMACache.Insert("InsertSql", dictionary);
            }
            else
            {
                str = ((Dictionary<MethodInfo[], string>)obj2)[methods];
            }
            DynamicParameters param = new DynamicParameters();
            param.AddDynamicParams((T)model);
            if ((AutoUpdateColumnName != null) && (AutoUpdateColumnName.Length > 0))
            {
                param.Add("?NewInsertID", null, DbType.Int32, ParameterDirection.Output, null);
            }
            using (IDbConnection connection = GetMySqlOpenConnection(false))
            {
                num = connection.Execute(str, param, null, null, null);
                //num = connection.Query<int>(str, param, null, true, null, null).First<int>();
                //num = connection.Query(str, param, null, null, null);
                connection.Close();
            }
            if (((AutoUpdateColumnName != null) && (AutoUpdateColumnName.Length > 0)) && (num > 0))
            {
                num = param.Get<int>("?NewInsertID");
            }
            return num;
        }

        public static int InsertModelUnAutoUpdate<T>(ref T model)
        {
            return InsertModelUnAutoUpdate<T>(typeof(T).Name, ref model);
        }

        public static int InsertModelUnAutoUpdate<T>(string TableName, ref T model)
        {
            return InsertModel<T>(TableName, null, ref model);
        }

        public static IEnumerable<TReturn> Query<TFirst, TSecond, TReturn>(string sql, System.Func<TFirst, TSecond, TReturn> map, object param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = new int?(), CommandType? commandType = new CommandType?())
        {
            using (IDbConnection connection = GetMySqlOpenConnection(false))
            {
                return connection.Query<TFirst, TSecond, TReturn>(sql, map, param, transaction, buffered, splitOn, commandTimeout, null);
            }
        }

        public static int UpdateModel<T>(ref T model)
        {
            return UpdateModel<T>(typeof(T).Name, ref model);
        }

        public static int UpdateModel<T>(string TableName, ref T model)
        {
            return UpdateModel<T>(TableName, ref model, new string[] { "id" }, new string[] { "ID" });
        }

        public static int UpdateModel<T>(ref T model, string[] AutoUpdate, params string[] KeyColumn)
        {
            return UpdateModel<T>(typeof(T).Name, ref model, AutoUpdate, KeyColumn);
        }

        public static int UpdateModel<T>(string TableName, ref T model, string[] AutoUpdate, params string[] KeyColumn)
        {
            int num = -1;
            string str = "";
            object obj2 = BMACache.Get("UpdateSql");
            PropertyInfo[] properties = model.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            MethodInfo[] methods = model.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance);
            if ((obj2 == null) || !((Dictionary<MethodInfo[], string>)obj2).Keys.Contains<MethodInfo[]>(methods))
            {
                Dictionary<MethodInfo[], string> dictionary;
                StringBuilder builder = new StringBuilder("update " + TableName + " set ");
                StringBuilder builder2 = new StringBuilder();
                foreach (PropertyInfo info in properties)
                {
                    IEnumerable<Attribute> customAttributes = info.GetCustomAttributes(typeof(ModelAttribute));
                    if (((customAttributes.Count<Attribute>() > 0) && (customAttributes.First<Attribute>().GetType() == typeof(ModelAttribute))) && (((ModelAttribute)customAttributes.First<Attribute>()).Name == ModelAttributeType.TableColumn))
                    {
                        string str2 = info.Name.ToLower();
                        if ((AutoUpdate == null) || !AutoUpdate.Contains<string>(str2))
                        {
                            builder2.Append(str2 + "=?" + str2 + ",");
                        }
                    }
                }
                builder.Append(builder2.ToString().TrimEnd(new char[] { ',' }));
                builder.Append(" where ");
                bool flag = false;
                foreach (string str3 in KeyColumn)
                {
                    flag = true;
                    builder.Append(str3 + "=?" + str3);
                    builder.Append(" and ");
                }
                if (flag)
                {
                    str = builder.Remove(builder.Length - 5, 5).ToString();
                }
                else
                {
                    str = builder.ToString();
                }
                if (obj2 == null)
                {
                    dictionary = new Dictionary<MethodInfo[], string>();
                }
                else
                {
                    dictionary = (Dictionary<MethodInfo[], string>)obj2;
                }
                dictionary.Add(methods, str);
                BMACache.Insert("UpdateSql", dictionary);
            }
            else
            {
                str = ((Dictionary<MethodInfo[], string>)obj2)[methods];
            }
            DynamicParameters param = new DynamicParameters();
            param.AddDynamicParams((T)model);
            using (IDbConnection connection = GetMySqlOpenConnection(false))
            {
                num = connection.Execute(str, param, null, null, null);
                connection.Close();
            }
            return num;
        }



        private static MySqlConnection GetMySqlOpenConnection(bool mars = false)
        {
            string sqlConnectionString = DBHelper.sqlConnectionString;
            if (mars)
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(sqlConnectionString)
                {
                    MultipleActiveResultSets = true
                };
                sqlConnectionString = builder.ConnectionString;
            }
            MySqlConnection connection = new MySqlConnection(sqlConnectionString);
            connection.Open();
            return connection;
        }

        private static IDbConnection GetOpenConnection(bool mars = false)
        {
            string sqlConnectionString = DBHelper.sqlConnectionString;
            if (mars)
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(sqlConnectionString)
                {
                    MultipleActiveResultSets = true
                };
                sqlConnectionString = builder.ConnectionString;
            }
            OleDbConnection connection = new OleDbConnection(sqlConnectionString);
            connection.Open();
            return connection;
        }

        private static string sqlConnectionString
        {
            get
            {
                return PubConstant.ConnectionString;
            }
        }

    }


    public static partial class BMACache
    {
        private static object _locker = new object();//锁对象
        private static CacheStrategy _icachestrategy = null;//缓存策略
        private static CacheByRegex _icachemanager = null;//缓存管理

        static BMACache()
        {
            _icachestrategy = new CacheStrategy();
            _icachemanager = new CacheByRegex();
        }

        /// <summary>
        /// 缓存过期时间
        /// </summary>
        public static int TimeOut
        {
            get
            {
                return _icachestrategy.TimeOut;
            }
            set
            {
                lock (_locker)
                {
                    _icachestrategy.TimeOut = value;
                }
            }
        }

        /// <summary>
        /// 获得指定键的缓存值
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>缓存值</returns>
        public static object Get(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                return null;
            return _icachestrategy.Get(_icachemanager.GenerateGetKey(key));
        }

        /// <summary>
        /// 将指定键的对象添加到缓存中
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="data">缓存值</param>
        public static void Insert(string key, object data)
        {
            if (string.IsNullOrWhiteSpace(key) || data == null)
                return;
            lock (_locker)
            {
                _icachestrategy.Insert(_icachemanager.GenerateInsertKey(key), data);
            }
        }

        /// <summary>
        /// 将指定键的对象添加到缓存中，并指定过期时间
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="data">缓存值</param>
        /// <param name="cacheTime">缓存过期时间</param>
        public static void Insert(string key, object data, int cacheTime)
        {
            if (string.IsNullOrWhiteSpace(key) || data == null)
                return;
            lock (_locker)
            {
                _icachestrategy.Insert(_icachemanager.GenerateInsertKey(key), data, cacheTime);
            }
        }

        /// <summary>
        /// 从缓存中移除指定键的缓存值
        /// </summary>
        /// <param name="key">缓存键</param>
        public static void Remove(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                return;
            lock (_locker)
            {
                foreach (string k in _icachemanager.GenerateRemoveKey(key))
                    _icachestrategy.Remove(k);
            }
        }

        /// <summary>
        /// 清空缓存所有对象
        /// </summary>
        public static void Clear()
        {
            lock (_locker)
            {
                _icachestrategy.Clear();
            }
        }

    }


    /// <summary>
    /// 基于正则的缓存管理
    /// </summary>
    public partial class CacheByRegex
    {
        private Hashtable _cachekeys = new Hashtable();//缓存键列表

        /// <summary>
        /// 保存缓存键到_cachekeys中
        /// </summary>
        /// <param name="key">缓存键</param>
        private void SaveKeyToCacheKeys(string key)
        {
            if (!_cachekeys.ContainsKey(key))
            {
                _cachekeys.Add(key, DateTime.Now.ToString());
            }
        }

        /// <summary>
        /// 生成获得时的缓存键
        /// </summary>
        /// <param name="key">原缓存键</param>
        /// <returns>新缓存键</returns>
        public string GenerateGetKey(string key)
        {
            return key;
        }

        /// <summary>
        /// 生成添加时的缓存键
        /// </summary>
        /// <param name="key">原缓存键</param>
        /// <returns>新缓存键</returns>
        public string GenerateInsertKey(string key)
        {
            SaveKeyToCacheKeys(key);
            return key;
        }

        /// <summary>
        /// 生成要移除的缓存键列表
        /// </summary>
        /// <returns></returns>
        public List<string> GenerateRemoveKey(string key)
        {
            List<string> matchedKeyList = new List<string>();
            Regex regex = new Regex(key, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            foreach (string k in _cachekeys.Keys)
            {
                if (regex.IsMatch(k))
                    matchedKeyList.Add(k);
            }
            return matchedKeyList;
        }
    }

    /// <summary>
    /// 基于Asp.Net内置缓存的缓存策略
    /// </summary>
    public partial class CacheStrategy
    {
        private Cache _cache;

        public CacheStrategy()
        {
            _cache = HttpRuntime.Cache;
        }

        /// <summary>
        /// 获得指定键的缓存值
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>缓存值</returns>
        public object Get(string key)
        {
            return _cache.Get(key);
        }

        /// <summary>
        /// 将指定键的对象添加到缓存中
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="data">缓存值</param>
        public void Insert(string key, object data)
        {
            _cache.Insert(key, data, null, DateTime.Now.AddSeconds(_timeout), Cache.NoSlidingExpiration, CacheItemPriority.High, null);
        }

        /// <summary>
        /// 将指定键的对象添加到缓存中，并指定过期时间
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="data">缓存值</param>
        /// <param name="cacheTime">缓存过期时间</param>
        public void Insert(string key, object data, int cacheTime)
        {
            _cache.Insert(key, data, null, DateTime.Now.AddSeconds(cacheTime), Cache.NoSlidingExpiration, CacheItemPriority.High, null);
        }

        /// <summary>
        /// 从缓存中移除指定键的缓存值
        /// </summary>
        /// <param name="key">缓存键</param>
        public void Remove(string key)
        {
            _cache.Remove(key);
        }

        /// <summary>
        /// 清空所有缓存对象
        /// </summary>
        public void Clear()
        {
            IDictionaryEnumerator cacheEnum = _cache.GetEnumerator();
            while (cacheEnum.MoveNext())
                _cache.Remove(cacheEnum.Key.ToString());
        }

        private int _timeout = 3600;//单位秒

        /// <summary>
        /// 缓存过期时间
        /// </summary>
        /// <value></value>
        public int TimeOut
        {
            get
            {
                return _timeout;
            }
            set
            {
                _timeout = value > 0 ? value : 3600;
            }
        }
    }


    public class ModelAttribute : Attribute
    {
        public ModelAttributeType Name { get; set; }
    }
    public enum ModelAttributeType : int
    {
        /// <summary>
        /// table列
        /// </summary>
        TableColumn = 1,
        /// <summary>
        /// 视图列
        /// </summary>
        ViewColumn = 2
    }

}
