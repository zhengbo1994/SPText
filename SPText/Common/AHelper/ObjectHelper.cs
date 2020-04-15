using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace System
{
	/// <summary>
	/// 对象帮助类
	/// </summary>
	public static class ObjectHelper
	{
		/// <summary>
		/// 对象深拷贝（利用Json序列化与反序列化实现）
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static T DeepCopy<T>(this T obj) where T : class
		{
			return obj.JsonSerialize().JsonDeserialize<T>();
		}

		/// <summary>
		/// 获取类的所有公有属性名称
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="model"></param>
		/// <param name="exceptKeys">要剔除的属性名称</param>
		/// <returns></returns>
		public static IEnumerable<string> GetAllPropKeys<T>(this T model, List<string> exceptKeys = null) where T : class
		{
			var keys = model.GetType().GetProperties().ToList().Select(m => m.Name).ToList();
			if (exceptKeys != null && exceptKeys.Any())
			{
				keys = keys.Except(exceptKeys).ToList();
			}
			return keys;
		}

		/// <summary>
		/// 获取类的所有公有字段名称
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="model"></param>
		/// <param name="exceptKeys">要剔除的属性名称</param>
		/// <returns></returns>
		public static IEnumerable<string> GetAllFieldKeys<T>(this T model, List<string> exceptKeys = null) where T : class
		{
			var keys = model.GetType().GetFields().ToList().Select(m => m.Name).ToList();
			if (exceptKeys != null && exceptKeys.Any())
			{
				keys = keys.Except(exceptKeys).ToList();
			}
			return keys;
		}

		#region Json序列化与反序列化
		/// <summary>
		/// Json序列化为字符串
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="jsonObj"></param>
		/// <returns></returns>
		public static string JsonSerialize<T>(this T jsonObj) where T : class
		{
			if (jsonObj == null)
			{
				return null;
			}
			//Jil.JSON.SetDefaultOptions(jilOptions);
			return JsonConvert.SerializeObject(jsonObj);
			//return Jil.JSON.Serialize(jsonObj, new Jil.Options(includeInherited: true));
			//return Jil.JSON.Serialize(jsonObj, jilOptions);
			//return Jil.JSON.Serialize(jsonObj, jilOptions);
		}

		//private static Jil.Options jilOptions = new Jil.Options(includeInherited: true);
		/// <summary>
		/// Json反序列化为对象
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="jsonStr"></param>
		/// <returns></returns>
		public static T JsonDeserialize<T>(this string jsonStr)
		{
			if (jsonStr.IsNullOrWhiteSpace())
			{
				return default(T);
			}
			//Jil.JSON.SetDefaultOptions(jilOptions);
			return JsonConvert.DeserializeObject<T>(jsonStr);
			//return Jil.JSON.Deserialize<T>(jsonStr, new Jil.Options(dateFormat: Jil.DateTimeFormat.MillisecondsSinceUnixEpoch, includeInherited: true));
			//return Jil.JSON.Deserialize<T>(jsonStr, new Jil.Options(includeInherited: true));
			//return Jil.JSON.Deserialize<T>(jsonStr, jilOptions);
			//return Jil.JSON.Deserialize<T>(jsonStr, jilOptions);
		}
		#endregion

		#region Xml序列化与反序列化
		/// <summary>
		/// Xml序列化为字符串
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="xmlObj"></param>
		/// <returns></returns>
		public static string XmlSerialize<T>(this T xmlObj)
		{
			using (var memory = new MemoryStream())
			{
				var serial = new XmlSerializer(xmlObj.GetType());
				var setting = new XmlWriterSettings()
				{
					Indent = false,
					OmitXmlDeclaration = true,
					Encoding = Encoding.UTF8
				};
				var writer = XmlWriter.Create(memory, setting);
				var space = new XmlSerializerNamespaces(new XmlQualifiedName[] { new XmlQualifiedName(string.Empty) });
				serial.Serialize(writer, xmlObj, space);
				memory.Position = 0;
				using (var reader = new StreamReader(memory))
				{
					return reader.ReadToEnd();
				}
			}
		}

		/// <summary>  
		/// Xml字符串反序列化为对象
		/// </summary>
		public static T XmlDeserialize<T>(this string xmlStr) where T : class
		{
			using (var reader = new StringReader(xmlStr))
			{
				var temp = XmlReader.Create(reader);
				return new XmlSerializer(typeof(T)).Deserialize(temp) as T;
			}
		}

		/// <summary>
		/// Xml流反序列化为对象
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="stream"></param>
		/// <returns></returns>
		public static T XmlDeserialize<T>(this Stream stream) where T : class
		{
			return new XmlSerializer(typeof(T)).Deserialize(stream) as T;
		}
		#endregion

		/// <summary>
		/// 获取对象的属性名或者描述和值的键值对列表
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="obj"></param>
		/// <param name="isToDesc"></param>
		/// <returns></returns>
		public static Dictionary<string, string> GetPropOrDescKeyValueDict<T>(this T obj, bool isToDesc = false) where T : class
		{
			Dictionary<string, string> dict = new Dictionary<string, string>();
			var allProoKeys = obj.GetAllFieldKeys();
			foreach (var propKeyName in allProoKeys)
			{
				Type type = obj.GetType(); //获取类型
				Reflection.PropertyInfo propertyInfo = type.GetProperty(propKeyName); //获取指定名称的属性
				var value = propertyInfo.GetValue(obj, null).ToString(); //获取属性值

				string desc = "";

				if (isToDesc)
				{
					object[] objs = typeof(T).GetProperty(propKeyName).GetCustomAttributes(typeof(DescriptionAttribute), true);
					if (objs.Length > 0)
					{
						desc = ((DescriptionAttribute)objs[0]).Description;
					}
					else
					{
						desc = propKeyName;
					}
				}
				else
				{
					desc = propKeyName;
				}

				dict.Add(desc, value);
			}
			return dict;
		}
	}
}
