using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SPText.Common
{
    public class ExpdalHelper
    {
        /// <summary>
        /// 显示
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> GetTs<T>(string sql, CommandType cmdType, params SqlParameter[] pms) where T : new()
        {
            //获取Type对象，反射操作基本都是用Type进行的
            Type type = typeof(T);
            //string sql = "select * from Expense";
            SqlDataReader dr = DBHelper.ExecuteReader(sql, cmdType, pms);
            //获取Type对象的所有公共属性
            PropertyInfo[] info = type.GetProperties();
            List<T> modelList = new List<T>();
            //定义泛型对象
            T obj = default(T);
            while (dr.Read())
            {
                obj = new T();
                foreach (PropertyInfo item in info)
                {
                    item.SetValue(obj, dr[item.Name]);
                }
                modelList.Add(obj);
            }
            dr.Close();
            return modelList;
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="nodel"></param>
        /// <returns></returns>
        public static int AddExpense<T>(T nodel, CommandType cmdType, params SqlParameter[] pms) where T : new()
        {
            Type type = typeof(T);
            string sql = string.Format("insert into {0} ", type.Name);
            string val = "";
            string link = "'";
            //获取Type对象所有公共属性
            PropertyInfo[] info = type.GetProperties();
            foreach (PropertyInfo item in info)
            {
                val += link + item.GetValue(nodel) + link + ",";//定义字段变量
            }
            val = val.Substring(0, val.Length - 1);
            val += ")";
            sql += "values(" + val.Remove(0, 4);
            //调用执行方法
            return new DBHelper().ExecuteNonQuery(sql.ToString(), cmdType, pms);
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="nodel"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int UpdateExpense<T>(T nodel, int id, CommandType cmdType, params SqlParameter[] pms) where T : new()
        {
            Type type = typeof(T);
            string sql = string.Format("update {0} set  ", type.Name);

            PropertyInfo[] info = type.GetProperties();
            string link = "";
            foreach (PropertyInfo item in info)
            {
                object val = item.GetValue(nodel);
                if (val != null)
                {
                    sql += link + item.Name + "='" + val + "' ";
                    link = ",  ";
                }
            }
            sql = sql.Remove(18, 12);
            sql += string.Format(" where EId=" + id);
            return new DBHelper().ExecuteNonQuery(sql, cmdType, pms);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int DeleteByEId<T>(int id, CommandType cmdType, params SqlParameter[] pms) where T : new()
        {
            Type type = typeof(T);
            string sql = string.Format($"delete from {type.Name} where EId={id}");
            return new DBHelper().ExecuteNonQuery(sql, cmdType, pms);
        }
    }
}
