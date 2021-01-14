using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace System.Data
{
	/// <summary>
	/// DataTable 帮助类
	/// </summary>
	public static class DataTableHelper
	{
        /// <summary>
        /// 根据DataTable得到Model字符串
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static string GetModelStr(this DataTable dataTable)
        {
            var clsName = $"DataTable_{Guid.NewGuid().ToString("N")}";
            var columns = dataTable.Columns;
            StringBuilder sbr = new StringBuilder();
            sbr.AppendLine($"namespace Model");
            sbr.AppendLine($"{{");
            sbr.AppendLine($"{StringHelper.GetTab(1)}[System.Serializable]");
            sbr.AppendLine($"{StringHelper.GetTab(1)}public partial class {clsName}");
            sbr.AppendLine($"{StringHelper.GetTab(1)}{{");
            sbr.AppendLine($"{StringHelper.GetTab(2)}public {clsName}()");
            sbr.AppendLine($"{StringHelper.GetTab(2)}{{");
            foreach (DataColumn item in columns)
            {
                sbr.AppendLine($"{StringHelper.GetTab(3)}{item.Caption} = default({item.DataType.FullName});");
            }
            sbr.AppendLine($"{StringHelper.GetTab(2)}}}");
            foreach (DataColumn item in columns)
            {
                //sbr.AppendLine($"{StringHelper.GetTab(2)}public {item.DataType} {item.ColumnName};"); //字段
                sbr.AppendLine($"{StringHelper.GetTab(2)}public {item.DataType.FullName} {item.ColumnName} {{ get; set; }}"); //属性
            }
            sbr.AppendLine($"{StringHelper.GetTab(1)}}}");
            sbr.AppendLine($"}}");
            return sbr.ToString();
        }
        /// <summary>
        /// datatable转list
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<T> ToList<T>(this DataTable dt) where T : new()
		{
			List<T> ts = new List<T>();// 定义集合
			Type type = typeof(T); // 获得此模型的类型
			string tempName = "";
			foreach (DataRow dr in dt.Rows)
			{
				T t = new T();
				PropertyInfo[] propertys = t.GetType().GetProperties();// 获得此模型的公共属性
				foreach (PropertyInfo pi in propertys)
				{
					tempName = pi.Name;
					if (dt.Columns.Contains(tempName))
					{
						if (!pi.CanWrite) continue;
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
