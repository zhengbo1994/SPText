using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace SPTextWinForm.Public
{
    public class DataHelper<T> where T : class, new()
    {
        /// <summary>
        /// 把DataTable转换成指定类型的List
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static IList<T> DataModel(DataTable DT)
        {
            IList<T> ts = new List<T>();

            Type type = typeof(T);
            string tempName = "";
            foreach (DataRow dr in DT.Rows)
            {
                T t = new T();
                //获得此模型的公共属性
                PropertyInfo[] propertys = t.GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    tempName = pi.Name;

                    if (DT.Columns.Contains(tempName))
                    {
                        // 判断此属性是否有Setter

                        if (!pi.CanWrite)
                        {
                            continue;
                        }
                        object value = dr[tempName];
                        if (value != DBNull.Value)
                        {
                            pi.SetValue(t, value, null);
                        }
                    }
                }
                ts.Add(t);
            }
            return ts;
        }

        /// <summary>
        /// 把泛型List转换成DataTable
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static DataTable DataDataTable(List<T> list)
        {
            DataTable dt = new DataTable();
            // 获得此模型的公共属性      
            PropertyInfo[] propertys = typeof(T).GetProperties();
            foreach (PropertyInfo pi in propertys)
            {
                // 判断此属性是否有Getter      
                if (!pi.CanRead) continue;
                dt.Columns.Add(pi.Name, pi.PropertyType);
            }
            foreach (T item in list)
            {
                propertys = item.GetType().GetProperties();
                DataRow newRow = dt.NewRow();
                foreach (PropertyInfo pi in propertys)
                {
                    if (!pi.CanRead) continue;
                    newRow[pi.Name] = pi.GetValue(item);
                }
                dt.Rows.Add(newRow);
            }
            return dt;
        }
    }
}
