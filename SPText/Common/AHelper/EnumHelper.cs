﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace System
{
	public static class EnumHelper
	{
		/// <summary>
		/// 获取指定枚举类型的描述
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="en"></param>
		/// <returns></returns>
		public static string GetDescription<T>(this T en) where T : struct
		{
			Type type = en.GetType();
			MemberInfo[] memInfo = type.GetMember(en.ToString());
			if (memInfo != null && memInfo.Length > 0)
			{
				object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
				if (attrs != null && attrs.Length > 0)
					return ((DescriptionAttribute)attrs[0]).Description;
			}
			return en.ToString();
		}

		/// <summary>
		/// 获取指定枚举实例类型的所有描述集合
		/// </summary>
		/// <typeparam name="TEnum">枚举类型</typeparam>
		/// <param name="obj">指定枚举实例</param>
		/// <returns></returns>
		public static Dictionary<int, string> GetEnumDesxriptionDict<TEnum>(this TEnum obj) where TEnum : struct
		{
			var enumDic = new Dictionary<int, string>();
			Type enumType = typeof(TEnum);
			Array arrays = Enum.GetValues(enumType);
			for (int i = 0; i < arrays.LongLength; i++)
			{
				TEnum t = (TEnum)arrays.GetValue(i);
				FieldInfo fieldInfo = t.GetType().GetField(t.ToString());
				var desc = t.ToString();
				object[] attribArray = fieldInfo.GetCustomAttributes(false);
				if (attribArray.Count() > 0)
				{
					DescriptionAttribute attr = attribArray[0] as DescriptionAttribute;
					desc = attr.Description;
				}

				enumDic.Add(Convert.ToInt32(t), desc);
			}
			return enumDic;
		}

		/// <summary>
		/// 获取指定枚举类型的所有枚举值
		/// </summary>
		/// <typeparam name="TEnum"></typeparam>
		/// <returns></returns>
		public static List<TEnum> GetAllEnumList<TEnum>() where TEnum : struct
		{
			var array = Enum.GetValues(typeof(TEnum));
			List<TEnum> list = new List<TEnum>();
			foreach (var item in array)
			{
				list.Add((TEnum)item);
			}
			return list;
		}


		///// <summary>
		///// 获取枚举值的描述信息
		///// </summary>
		///// <typeparam name="TEnum">枚举类型</typeparam>
		///// <param name="obj">指定枚举实例</param>
		///// <param name="enumValIndex">枚举整型值</param>
		///// <returns></returns>
		//public static string GetEnumDesxription<TEnum>(this TEnum obj, int enumValIndex) where TEnum : struct
		//{
		//	var desc = GetEnumDesxriptionDict(obj).FirstOrDefault(m => m.Key == enumValIndex).Value;
		//	if (string.IsNullOrWhiteSpace(desc))
		//	{
		//		desc = "";
		//	}
		//	return desc;
		//}
	}
}
