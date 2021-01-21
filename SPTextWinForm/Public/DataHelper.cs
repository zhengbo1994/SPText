using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SPTextWinForm.Public
{
    public class DataHelper<T> where T : class, new()
    {
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
    }
}
