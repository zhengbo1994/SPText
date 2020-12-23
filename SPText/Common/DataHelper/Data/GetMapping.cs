using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SPText.Common.DataHelper.Data
{
    public static class GetMapping
    {
        /// <summary>
        /// 获取特性标注的表名
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetTableName(this Type type)
        {
            //在派生类中重写时，指示是否将指定类型或其派生类型的一个或多个特性应用于此成员
            //翻译：查找在改类中是否标注了此特性 
            if (type.IsDefined(typeof(TableAttribute), true))
            {
                //检索应用于指定元素的自定义属性
                //翻译：从传入的type中获取到指定的特性
                TableAttribute attribute = (TableAttribute)type.GetCustomAttribute(typeof(TableAttribute), true);
                return attribute.TableName;
            }
            else
                return type.Name;
        }
        /// <summary>
        /// 获取特性标注的属性名
        /// </summary>
        /// <param name="prop"></param>
        /// <returns></returns>
        public static string GetColumnName(this PropertyInfo prop)
        {
            //在派生类中重写时，指示是否将指定类型或其派生类型的一个或多个特性应用于此成员
            //翻译：查找在改类中是否标注了此特性 
            if (prop.IsDefined(typeof(ColumnAttribute), true))
            {
                //检索应用于指定元素的自定义属性
                //翻译：从传入的type中获取到指定的特性
                ColumnAttribute attribute = (ColumnAttribute)prop.GetCustomAttribute(typeof(ColumnAttribute), true);
                return attribute.GetColumn;
            }
            else
                return prop.Name;
        }
    }
}
