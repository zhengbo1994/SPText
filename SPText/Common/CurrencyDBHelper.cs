using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.Configuration;
using System.Data.Common;
using System.Collections.Generic;

namespace SPText.Common
{
    /// <summary>
    /// 数据访问基础类
    /// </summary>
    public class CurrencyDBHelper
    {
        protected static string connectionString = "";
        DbProviderFactory provider;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ProviderName">提供程序名称</param>
        /// <param name="connString">连接字符串</param>
        public CurrencyDBHelper(DbProviderType providerType, string connString)
        {
            string providerName = ProviderFactory.providerInvariantNames[providerType];
            provider = DbProviderFactories.GetFactory(providerName);
            connectionString = connString;
        }

        #region  执行简单SQL语句

        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public int ExecuteSql(string SQLString)
        {
            using (DbConnection connection = provider.CreateConnection())
            {
                connection.ConnectionString = connectionString;
                using (DbCommand cmd = provider.CreateCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = SQLString;
                    try
                    {
                        connection.Open();
                        int rows = cmd.ExecuteNonQuery();
                        return rows;
                    }
                    catch (DbException E)
                    {
                        connection.Close();
                        connection.Dispose();
                        throw new Exception(E.Message);
                    }
                }
            }
        }

        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">多条SQL语句</param>        
        public void ExecuteSqlTran(ArrayList SQLStringList)
        {
            using (DbConnection conn = provider.CreateConnection())
            {
                conn.ConnectionString = connectionString;
                conn.Open();
                using (DbCommand cmd = provider.CreateCommand())
                {
                    cmd.Connection = conn;
                    using (DbTransaction tx = conn.BeginTransaction())
                    {
                        cmd.Transaction = tx;
                        try
                        {
                            for (int n = 0; n < SQLStringList.Count; n++)
                            {
                                string strsql = SQLStringList[n].ToString();
                                if (strsql.Trim().Length > 1)
                                {
                                    cmd.CommandText = strsql;
                                    cmd.ExecuteNonQuery();
                                }
                            }
                            tx.Commit();
                        }
                        catch (DbException ex)
                        {
                            tx.Rollback();
                            conn.Close();
                            conn.Dispose();
                            throw ex;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="SQLString">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
        public object GetSingle(string SQLString)
        {
            using (DbConnection connection = provider.CreateConnection())
            {
                connection.ConnectionString = connectionString;
                using (DbCommand cmd = provider.CreateCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = SQLString;
                    try
                    {
                        connection.Open();
                        object obj = cmd.ExecuteScalar();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (DbException e)
                    {
                        connection.Close();
                        connection.Dispose();
                        throw new Exception(e.Message);
                    }
                }
            }
        }
        /// <summary>
        /// 执行查询语句，返回SqlDataReader
        /// </summary>
        /// <param name="strSQL">查询语句</param>
        /// <returns>SqlDataReader</returns>
        public DbDataReader ExecuteReader(string strSQL)
        {
            DbConnection connection = provider.CreateConnection();
            connection.ConnectionString = connectionString;
            DbCommand cmd = provider.CreateCommand();
            cmd.Connection = connection;
            cmd.CommandText = strSQL;
            try
            {
                connection.Open();
                DbDataReader myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return myReader;
            }
            catch (System.Data.Common.DbException e)
            {
                connection.Close();
                connection.Dispose();
                throw new Exception(e.Message);
            }

        }
        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataSet</returns>
        public DataSet GetDataSet(string SQLString)
        {
            using (DbConnection connection = provider.CreateConnection())
            {
                connection.ConnectionString = connectionString;
                using (DbCommand cmd = provider.CreateCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = SQLString;
                    try
                    {
                        DataSet ds = new DataSet();
                        DbDataAdapter adapter = provider.CreateDataAdapter();
                        adapter.SelectCommand = cmd;
                        adapter.Fill(ds, "ds");
                        return ds;
                    }
                    catch (DbException ex)
                    {
                        connection.Close();
                        connection.Dispose();
                        throw new Exception(ex.Message);
                    }
                }
            }
        }

        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataTable</returns>
        public DataTable GetDataTable(string SQLString)
        {
            using (DbConnection connection = provider.CreateConnection())
            {
                connection.ConnectionString = connectionString;
                using (DbCommand cmd = provider.CreateCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = SQLString;
                    try
                    {
                        DataTable dt = new DataTable();
                        DbDataAdapter adapter = provider.CreateDataAdapter();
                        adapter.SelectCommand = cmd;
                        adapter.Fill(dt);
                        return dt;
                    }
                    catch (DbException ex)
                    {
                        connection.Close();
                        connection.Dispose();
                        throw new Exception(ex.Message);
                    }
                }
            }
        }
        #endregion

        #region 执行带参数的SQL语句

        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public int ExecuteSql(string SQLString, DbParameter[] cmdParms)
        {
            using (DbConnection connection = provider.CreateConnection())
            {
                connection.ConnectionString = connectionString;
                using (DbCommand cmd = provider.CreateCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = SQLString;
                    try
                    {
                        PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                        int rows = cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                        return rows;
                    }
                    catch (DbException E)
                    {
                        connection.Close();
                        connection.Dispose();
                        throw new Exception(E.Message);
                    }
                }
            }
        }

        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">SQL语句的哈希表（key为sql语句，value是该语句的SqlParameter[]）</param>
        public void ExecuteSqlTran(Hashtable SQLStringList)
        {
            using (DbConnection conn = provider.CreateConnection())
            {
                conn.ConnectionString = connectionString;
                conn.Open();
                using (DbTransaction trans = conn.BeginTransaction())
                {
                    using (DbCommand cmd = provider.CreateCommand())
                    {
                        try
                        {
                            //循环
                            foreach (DictionaryEntry myDE in SQLStringList)
                            {
                                string cmdText = myDE.Key.ToString();
                                DbParameter[] cmdParms = (DbParameter[])myDE.Value;
                                PrepareCommand(cmd, conn, trans, cmdText, cmdParms);
                                int val = cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                            }
                            trans.Commit();
                        }
                        catch (DbException ex)
                        {
                            trans.Rollback();
                            conn.Close();
                            conn.Dispose();
                            throw ex;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）,返回首行首列的值;
        /// </summary>
        /// <param name="SQLString">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
        public object GetSingle(string SQLString, DbParameter[] cmdParms)
        {
            using (DbConnection connection = provider.CreateConnection())
            {
                connection.ConnectionString = connectionString;
                using (DbCommand cmd = provider.CreateCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                        object obj = cmd.ExecuteScalar();
                        cmd.Parameters.Clear();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (DbException e)
                    {
                        connection.Close();
                        connection.Dispose();
                        throw new Exception(e.Message);
                    }
                }
            }
        }

        /// <summary>
        /// 执行查询语句，返回SqlDataReader
        /// </summary>
        /// <param name="strSQL">查询语句</param>
        /// <returns>SqlDataReader</returns>
        public DbDataReader ExecuteReader(string SQLString, DbParameter[] cmdParms)
        {
            DbConnection connection = provider.CreateConnection();
            connection.ConnectionString = connectionString;
            DbCommand cmd = provider.CreateCommand();
            try
            {
                PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                DbDataReader myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return myReader;
            }
            catch (DbException e)
            {
                connection.Close();
                connection.Dispose();
                throw new Exception(e.Message);
            }

        }

        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataSet</returns>
        public DataSet GetDataSet(string SQLString, DbParameter[] cmdParms)
        {
            using (DbConnection connection = provider.CreateConnection())
            {
                connection.ConnectionString = connectionString;
                using (DbCommand cmd = provider.CreateCommand())
                {
                    using (DbDataAdapter da = provider.CreateDataAdapter())
                    {
                        PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                        da.SelectCommand = cmd;
                        DataSet ds = new DataSet();
                        try
                        {
                            da.Fill(ds, "ds");
                            cmd.Parameters.Clear();
                            return ds;
                        }
                        catch (DbException ex)
                        {
                            connection.Close();
                            connection.Dispose();
                            throw new Exception(ex.Message);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataSet</returns>
        public DataTable GetDataTable(string SQLString, DbParameter[] cmdParms)
        {
            using (DbConnection connection = provider.CreateConnection())
            {
                connection.ConnectionString = connectionString;
                using (DbCommand cmd = provider.CreateCommand())
                {
                    using (DbDataAdapter da = provider.CreateDataAdapter())
                    {
                        PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                        da.SelectCommand = cmd;
                        DataTable dt = new DataTable();
                        try
                        {
                            da.Fill(dt);
                            cmd.Parameters.Clear();
                            return dt;
                        }
                        catch (DbException ex)
                        {
                            connection.Close();
                            connection.Dispose();
                            throw new Exception(ex.Message);
                        }
                    }
                }
            }
        }

        private void PrepareCommand(DbCommand cmd, DbConnection conn, DbTransaction trans, string cmdText, DbParameter[] cmdParms)
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
            cmd.CommandType = CommandType.Text;//cmdType;
            if (cmdParms != null)
            {
                foreach (DbParameter parm in cmdParms)
                {
                    cmd.Parameters.Add(parm);
                }
            }
        }

        #endregion

        #region 存储过程操作
        /// <summary>
        /// 执行存储过程;
        /// </summary>
        /// <param name="storeProcName">存储过程名</param>
        /// <param name="parameters">所需要的参数</param>
        /// <returns>返回受影响的行数</returns>
        public int RunProcedureExecuteSql(string storeProcName, DbParameter[] parameters)
        {
            using (DbConnection connection = provider.CreateConnection())
            {
                connection.ConnectionString = connectionString;
                DbCommand cmd = BuildQueryCommand(connection, storeProcName, parameters);
                int rows = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                connection.Close();
                return rows;
            }
        }

        /// <summary>
        /// 执行存储过程,返回首行首列的值
        /// </summary>
        /// <param name="storeProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>返回首行首列的值</returns>
        public Object RunProcedureGetSingle(string storeProcName, DbParameter[] parameters)
        {
            using (DbConnection connection = provider.CreateConnection())
            {
                connection.ConnectionString = connectionString;
                try
                {
                    DbCommand cmd = BuildQueryCommand(connection, storeProcName, parameters);
                    object obj = cmd.ExecuteScalar();
                    cmd.Parameters.Clear();
                    if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                    {
                        return null;
                    }
                    else
                    {
                        return obj;
                    }
                }
                catch (DbException e)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new Exception(e.Message);
                }
            }
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>SqlDataReader</returns>
        public DbDataReader RunProcedureGetDataReader(string storedProcName, DbParameter[] parameters)
        {
            DbConnection connection = provider.CreateConnection();
            connection.ConnectionString = connectionString;
            DbDataReader returnReader;
            DbCommand cmd = BuildQueryCommand(connection, storedProcName, parameters);
            cmd.CommandType = CommandType.StoredProcedure;
            returnReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            cmd.Parameters.Clear();
            return returnReader;
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>DataSet</returns>
        public DataSet RunProcedureGetDataSet(string storedProcName, DbParameter[] parameters)
        {
            using (DbConnection connection = provider.CreateConnection())
            {
                connection.ConnectionString = connectionString;
                DataSet dataSet = new DataSet();
                DbDataAdapter sqlDA = provider.CreateDataAdapter();
                sqlDA.SelectCommand = BuildQueryCommand(connection, storedProcName, parameters);
                sqlDA.Fill(dataSet);
                sqlDA.SelectCommand.Parameters.Clear();
                sqlDA.Dispose();
                return dataSet;
            }
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>DataTable</returns>
        public DataTable RunProcedureGetDataTable(string storedProcName, DbParameter[] parameters)
        {
            using (DbConnection connection = provider.CreateConnection())
            {
                connection.ConnectionString = connectionString;
                DataTable dataTable = new DataTable();
                DbDataAdapter sqlDA = provider.CreateDataAdapter();
                sqlDA.SelectCommand = BuildQueryCommand(connection, storedProcName, parameters);
                sqlDA.Fill(dataTable);
                sqlDA.SelectCommand.Parameters.Clear();
                sqlDA.Dispose();
                return dataTable;
            }
        }

        /// <summary>
        /// 执行多个存储过程，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">存储过程的哈希表（value为存储过程语句，key是该语句的DbParameter[]）</param>
        public bool RunProcedureTran(Hashtable SQLStringList)
        {
            using (DbConnection connection = provider.CreateConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();
                using (DbTransaction trans = connection.BeginTransaction())
                {
                    using (DbCommand cmd = provider.CreateCommand())
                    {
                        try
                        {
                            //循环
                            foreach (DictionaryEntry myDE in SQLStringList)
                            {
                                cmd.Connection = connection;
                                string storeName = myDE.Value.ToString();
                                DbParameter[] cmdParms = (DbParameter[])myDE.Key;

                                cmd.Transaction = trans;
                                cmd.CommandText = storeName;
                                cmd.CommandType = CommandType.StoredProcedure;
                                if (cmdParms != null)
                                {
                                    foreach (DbParameter parameter in cmdParms)
                                    {
                                        cmd.Parameters.Add(parameter);
                                    }
                                }
                                int val = cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                            }
                            trans.Commit();
                            return true;
                        }
                        catch
                        {
                            trans.Rollback();
                            connection.Close();
                            connection.Dispose();
                            return false;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 构建 SqlCommand 对象(用来返回一个结果集，而不是一个整数值)
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>SqlCommand</returns>
        private DbCommand BuildQueryCommand(DbConnection connection, string storedProcName, DbParameter[] parameters)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            DbCommand command = provider.CreateCommand();
            command.CommandText = storedProcName;
            command.Connection = connection;
            command.CommandType = CommandType.StoredProcedure;
            if (parameters != null)
            {
                foreach (DbParameter parameter in parameters)
                {
                    command.Parameters.Add(parameter);
                }
            }
            return command;
        }
        #endregion




    }

    /// <summary>
    /// 通用数据库访问类，封装了对数据库的常见操作
    ///</summary>
    public sealed class DbUtility
    {
        public string ConnectionString { get; set; }
        private DbProviderFactory providerFactory;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="providerType">数据库类型枚举，参见<paramref name="providerType"/></param>
        public DbUtility(string connectionString, DbProviderType providerType)
        {
            ConnectionString = connectionString;
            providerFactory = ProviderFactory.GetDbProviderFactory(providerType);
            if (providerFactory == null)
            {
                throw new ArgumentException("Can't load DbProviderFactory for given value of providerType");
            }
        }
        /// <summary>   
        /// 对数据库执行增删改操作，返回受影响的行数。   
        /// </summary>   
        /// <param name="sql">要执行的增删改的SQL语句</param>   
        /// <param name="parameters">执行增删改语句所需要的参数</param>
        /// <returns></returns>  
        public int ExecuteNonQuery(string sql, IList<DbParameter> parameters)
        {
            return ExecuteNonQuery(sql, parameters, CommandType.Text);
        }
        /// <summary>   
        /// 对数据库执行增删改操作，返回受影响的行数。   
        /// </summary>   
        /// <param name="sql">要执行的增删改的SQL语句</param>   
        /// <param name="parameters">执行增删改语句所需要的参数</param>
        /// <param name="commandType">执行的SQL语句的类型</param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sql, IList<DbParameter> parameters, CommandType commandType)
        {
            using (DbCommand command = CreateDbCommand(sql, parameters, commandType))
            {
                command.Connection.Open();
                int affectedRows = command.ExecuteNonQuery();
                command.Connection.Close();
                return affectedRows;
            }
        }

        /// <summary>   
        /// 执行一个查询语句，返回一个关联的DataReader实例   
        /// </summary>   
        /// <param name="sql">要执行的查询语句</param>   
        /// <param name="parameters">执行SQL查询语句所需要的参数</param>
        /// <returns></returns> 
        public DbDataReader ExecuteReader(string sql, IList<DbParameter> parameters)
        {
            return ExecuteReader(sql, parameters, CommandType.Text);
        }

        /// <summary>   
        /// 执行一个查询语句，返回一个关联的DataReader实例   
        /// </summary>   
        /// <param name="sql">要执行的查询语句</param>   
        /// <param name="parameters">执行SQL查询语句所需要的参数</param>
        /// <param name="commandType">执行的SQL语句的类型</param>
        /// <returns></returns> 
        public DbDataReader ExecuteReader(string sql, IList<DbParameter> parameters, CommandType commandType)
        {
            DbCommand command = CreateDbCommand(sql, parameters, commandType);
            command.Connection.Open();
            return command.ExecuteReader(CommandBehavior.CloseConnection);
        }

        /// <summary>   
        /// 执行一个查询语句，返回一个包含查询结果的DataTable   
        /// </summary>   
        /// <param name="sql">要执行的查询语句</param>   
        /// <param name="parameters">执行SQL查询语句所需要的参数</param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string sql, IList<DbParameter> parameters)
        {
            return ExecuteDataTable(sql, parameters, CommandType.Text);
        }
        /// <summary>   
        /// 执行一个查询语句，返回一个包含查询结果的DataTable   
        /// </summary>   
        /// <param name="sql">要执行的查询语句</param>   
        /// <param name="parameters">执行SQL查询语句所需要的参数</param>
        /// <param name="commandType">执行的SQL语句的类型</param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string sql, IList<DbParameter> parameters, CommandType commandType)
        {
            using (DbCommand command = CreateDbCommand(sql, parameters, commandType))
            {
                using (DbDataAdapter adapter = providerFactory.CreateDataAdapter())
                {
                    adapter.SelectCommand = command;
                    DataTable data = new DataTable();
                    adapter.Fill(data);
                    return data;
                }
            }
        }
        public DataSet ExecuteDataSet(string sql, IList<DbParameter> parameters, CommandType commandType)
        {
            using (DbCommand command = CreateDbCommand(sql, parameters, commandType))
            {
                using (DbDataAdapter adapter = providerFactory.CreateDataAdapter())
                {
                    adapter.SelectCommand = command;
                    DataSet data = new DataSet();
                    adapter.Fill(data,"ds");
                    return data;
                }
            }
        }

        /// <summary>   
        /// 执行一个查询语句，返回查询结果的第一行第一列   
        /// </summary>   
        /// <param name="sql">要执行的查询语句</param>   
        /// <param name="parameters">执行SQL查询语句所需要的参数</param>   
        /// <returns></returns>   
        public Object ExecuteScalar(string sql, IList<DbParameter> parameters)
        {
            return ExecuteScalar(sql, parameters, CommandType.Text);
        }

        /// <summary>   
        /// 执行一个查询语句，返回查询结果的第一行第一列   
        /// </summary>   
        /// <param name="sql">要执行的查询语句</param>   
        /// <param name="parameters">执行SQL查询语句所需要的参数</param>   
        /// <param name="commandType">执行的SQL语句的类型</param>
        /// <returns></returns>   
        public Object ExecuteScalar(string sql, IList<DbParameter> parameters, CommandType commandType)
        {
            using (DbCommand command = CreateDbCommand(sql, parameters, commandType))
            {
                command.Connection.Open();
                object result = command.ExecuteScalar();
                command.Connection.Close();
                return result;
            }
        }

        public DbParameter CreateDbParameter(string name, object value)
        {
            return CreateDbParameter(name, ParameterDirection.Input, value);
        }

        public DbParameter CreateDbParameter(string name, ParameterDirection parameterDirection, object value)
        {
            DbParameter parameter = providerFactory.CreateParameter();
            parameter.ParameterName = name;
            parameter.Value = value;
            parameter.Direction = parameterDirection;
            return parameter;
        }

        /// <summary>
        /// 创建一个DbCommand对象
        /// </summary>
        /// <param name="sql">要执行的查询语句</param>   
        /// <param name="parameters">执行SQL查询语句所需要的参数</param>
        /// <param name="commandType">执行的SQL语句的类型</param>
        /// <returns></returns>
        private DbCommand CreateDbCommand(string sql, IList<DbParameter> parameters, CommandType commandType)
        {
            DbConnection connection = providerFactory.CreateConnection();
            DbCommand command = providerFactory.CreateCommand();
            connection.ConnectionString = ConnectionString;
            command.CommandText = sql;
            command.CommandType = commandType;
            command.Connection = connection;
            if (!(parameters == null || parameters.Count == 0))
            {
                foreach (DbParameter parameter in parameters)
                {
                    command.Parameters.Add(parameter);
                }
            }
            return command;
        }
    }
    /// <summary>
    /// 数据库类型枚举
    /// </summary>
    public enum DbProviderType : byte
    {
        SqlServer,
        MySql,
        SQLite,
        Oracle,
        ODBC,
        OleDb,
        Firebird,
        PostgreSql,
        DB2,
        Informix,
        SqlServerCe
    }
    /// <summary>
    /// DbProviderFactory工厂类
    /// </summary>
    public class ProviderFactory
    {
        internal static Dictionary<DbProviderType, string> providerInvariantNames = new Dictionary<DbProviderType, string>();
        internal static Dictionary<DbProviderType, DbProviderFactory> providerFactoies = new Dictionary<DbProviderType, DbProviderFactory>(20);
        static ProviderFactory()
        {
            //加载已知的数据库访问类的程序集
            providerInvariantNames.Add(DbProviderType.SqlServer, "System.Data.SqlClient");
            providerInvariantNames.Add(DbProviderType.OleDb, "System.Data.OleDb");
            providerInvariantNames.Add(DbProviderType.ODBC, "System.Data.ODBC");
            providerInvariantNames.Add(DbProviderType.Oracle, "Oracle.DataAccess.Client");
            providerInvariantNames.Add(DbProviderType.MySql, "MySql.Data.MySqlClient");
            providerInvariantNames.Add(DbProviderType.SQLite, "System.Data.SQLite");
            providerInvariantNames.Add(DbProviderType.Firebird, "FirebirdSql.Data.Firebird");
            providerInvariantNames.Add(DbProviderType.PostgreSql, "Npgsql");
            providerInvariantNames.Add(DbProviderType.DB2, "IBM.Data.DB2.iSeries");
            providerInvariantNames.Add(DbProviderType.Informix, "IBM.Data.Informix");
            providerInvariantNames.Add(DbProviderType.SqlServerCe, "System.Data.SqlServerCe");
        }
        /// <summary>
        /// 获取指定数据库类型对应的程序集名称
        /// </summary>
        /// <param name="providerType">数据库类型枚举</param>
        /// <returns></returns>
        public static string GetProviderInvariantName(DbProviderType providerType)
        {
            return providerInvariantNames[providerType];
        }
        /// <summary>
        /// 获取指定类型的数据库对应的DbProviderFactory
        /// </summary>
        /// <param name="providerType">数据库类型枚举</param>
        /// <returns></returns>
        public static DbProviderFactory GetDbProviderFactory(DbProviderType providerType)
        {
            //如果还没有加载，则加载该DbProviderFactory
            if (!providerFactoies.ContainsKey(providerType))
            {
                providerFactoies.Add(providerType, ImportDbProviderFactory(providerType));
            }
            return providerFactoies[providerType];
        }
        /// <summary>
        /// 加载指定数据库类型的DbProviderFactory
        /// </summary>
        /// <param name="providerType">数据库类型枚举</param>
        /// <returns></returns>
        private static DbProviderFactory ImportDbProviderFactory(DbProviderType providerType)
        {
            string providerName = providerInvariantNames[providerType];
            DbProviderFactory factory = null;
            try
            {
                //从全局程序集中查找
                factory = DbProviderFactories.GetFactory(providerName);
            }
            catch (ArgumentException e)
            {
                factory = null;
            }
            return factory;
        }
    }

}