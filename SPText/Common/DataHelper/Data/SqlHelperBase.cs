using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace SPText.Common.DataHelper.Data
{
    public class SqlHelperBase
    {
        private string ConStr;

        public SqlHelperBase(string _ConStr)
        {
            //在构造函数中调用OnConfiguring，并将依赖注入的SqlHelperBuilder实例传给该方法
            //此时子类重写该方法是就可以使用依赖注入的SqlHelperBuilder的实例了
            ConStr = _ConStr;
            OnConfiguring(ConStr);
        }
        /// <summary>
        /// 虚方法，用于子类重写
        /// </summary>
        /// <param name="sqlHelperBuilder"></param>
        public virtual void OnConfiguring(string _ConStr)
        {
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="sqlText">Sql语句</param>
        /// <param name="sqlParameters">传入的参数</param>
        /// <returns></returns>
        public DataTable ExecuteTable(string sqlText, params SqlParameter[] sqlParameters)
        {
            //创建仓库钥匙
            using (SqlConnection conn = new SqlConnection(ConStr))
            {
                conn.Open();
                //告诉仓库管理员要干什么，即sqlText。
                //把钥匙交到管理员手上，即conn
                SqlCommand cmd = new SqlCommand(sqlText, conn);
                cmd.Parameters.AddRange(sqlParameters);
                //管理员推着车进入仓库
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                //把一个个集装箱放到推车里放满
                DataSet ds = new DataSet();
                sda.Fill(ds);
                //返回需要取得数据
                return ds.Tables[0];
            };

        }
        /// <summary>
        /// 增删改非查询操作
        /// </summary>
        /// <param name="sqlText">sql语句</param>
        /// <param name="sqlParameters">传入的参数</param>
        /// <returns></returns>
        public int ExecuteNoQuery(string sqlText, params SqlParameter[] sqlParameters)
        {
            //创建仓库钥匙
            using (SqlConnection conn = new SqlConnection(ConStr))
            {
                conn.Open();
                //告诉仓库管理员要干什么，即sqlText。
                //把钥匙交到管理员手上，即conn
                SqlCommand cmd = new SqlCommand(sqlText, conn);
                cmd.Parameters.AddRange(sqlParameters);
                int rows = cmd.ExecuteNonQuery();
                return rows;
            }
        }
        /// <summary>
        /// 执行sql并返回首行首列
        /// </summary>
        /// <param name="sqlText"></param>
        /// <param name="sqlParameters"></param>
        /// <returns></returns>
        public object ExecuteScalar(string sqlText, params SqlParameter[] sqlParameters)
        {
            //创建仓库钥匙
            using (SqlConnection conn = new SqlConnection(ConStr))
            {
                conn.Open();
                //告诉仓库管理员要干什么，即sqlText。
                //把钥匙交到管理员手上，即conn
                SqlCommand cmd = new SqlCommand(sqlText, conn);
                cmd.Parameters.AddRange(sqlParameters);
                object res = cmd.ExecuteScalar();
                return res;
            }
        }
        /// <summary>
        /// C#=>null 转换 DB=>DBNUll
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object ToDbValue(object value)
        {
            return value == null ? DBNull.Value : value;
        }
        /// <summary>
        ///  DB=>DBNUll 转换 C#=>null
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>

        public static object FromDbValue(object value)
        {
            return value == DBNull.Value ? null : value;
        }
    }
}
