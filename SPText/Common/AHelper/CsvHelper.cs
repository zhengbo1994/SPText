using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace System
{
	public class CsvHelper
	{
		/// <summary>
		/// 配置文件的安装信息
		/// </summary>
		public class CsvInstallInfo
		{
			/// <summary>
			/// csv配置文件路径
			/// </summary>
			public string Filepath { get; set; }
			/// <summary>
			/// 类中的字段与配置表中的字段是否相匹配（如果该值为false，则先请更新类的字段）
			/// </summary>
			public bool IsFieldMatch { get; set; }
		}
		/// <summary>
		/// 获取配置文件的列名称
		/// </summary>
		/// <param name="filepath"></param>
		/// <returns></returns>
		public static List<string> GetTableColumnNameList(string filepath)
		{
			using (DataTable dt = ReadCsv(filepath))
			{
				var dtColList = new List<string>();
				for (int i = 0; i < dt.Columns.Count; i++)
				{
					dtColList.Add(dt.Columns[i].ToString());
				}
				return dtColList;
			}
		}
		/// <summary>
		/// 通用 csv list 安装配置方法
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="filepath"></param>
		/// <param name="list"></param>
		public static void InstallList<T>(string filepath, out List<T> list, List<CsvInstallInfo> installInfoList) where T : class, new()
		{
			list = new List<T>();
			using (DataTable dt = ReadCsv(filepath))
			{
				var typePropNameList = typeof(T).GetFields().Select(m => m.Name).ToList();
				var dtColList = GetTableColumnNameList(filepath);
				var except1 = typePropNameList.Except(dtColList).ToList();
				var except2 = dtColList.Except(typePropNameList).ToList();

				//只是粗略的判断
				if (typePropNameList.Count() < dtColList.Count())
				{
					installInfoList.Add(new CsvInstallInfo { Filepath = filepath, IsFieldMatch = false });
					return;
				}

				foreach (DataRow dr in dt.Rows)
				{
					T model = new T();
					var properties = model.GetType().GetFields().ToList();
					foreach (DataColumn dc in dt.Columns)
					{
						foreach (var prop in properties)
						{
							if (dc.ColumnName == prop.Name)
							{
								prop.SetValue(model, dr[prop.Name]);
							}
						}
					}
					list.Add(model);
				}
			}
		}

		/// <summary>
		/// 通用 csv dict 安装配置方法
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="filepath"></param>
		/// <param name="list"></param>
		public static void InstallDict<T>(string filepath, out Dictionary<string, T> dict, List<CsvInstallInfo> installInfoList) where T : class, new()
		{
			dict = new Dictionary<string, T>();
			using (DataTable dt = ReadCsv(filepath))
			{
				var typePropNameList = typeof(T).GetFields().Select(m => m.Name).ToList();
				var dtColList = GetTableColumnNameList(filepath);
				var except1 = typePropNameList.Except(dtColList).ToList();
				var except2 = dtColList.Except(typePropNameList).ToList();

				//只是粗略的判断
				if (typePropNameList.Count() < dtColList.Count())
				{
					installInfoList.Add(new CsvInstallInfo { Filepath = filepath, IsFieldMatch = false });
					return;
				}

				foreach (DataRow dr in dt.Rows)
				{
					string id = "";
					T model = new T();
					var properties = model.GetType().GetFields().ToList();
					foreach (DataColumn dc in dt.Columns)
					{
						foreach (var prop in properties)
						{
							if (dc.ColumnName == prop.Name)
							{
								if (id.IsNullOrWhiteSpace())
								{
									id = dr[prop.Name].ToString();
								}
								prop.SetValue(model, dr[prop.Name]);
							}
						}
					}
					dict.Add(id, model);
				}
			}
		}


		/// <summary>
		/// 读取CSV
		/// </summary>
		/// <param name="filepath"></param>
		/// <returns></returns>
		private static DataTable ReadCsv(string filepath)
		{
			if (filepath == "") return null;
			filepath = Path.Combine(Directory.GetCurrentDirectory(), filepath);

			DataTable dt = new DataTable("NewTable");
			DataRow row;

			string[] lines = File.ReadAllLines(filepath, Encoding.UTF8);
			string[] head = lines[0].Split(',');
			int cnt = head.Length;
			for (int i = 0; i < cnt; i++)
				dt.Columns.Add(head[i]);

			for (int i = 1; i < lines.Length; i++)
			{
				if (string.IsNullOrWhiteSpace(lines[i]) || lines[i][0] == ',')
					continue;

				try
				{
					row = dt.NewRow();
					row.ItemArray = GetRow(lines[i], cnt);
					dt.Rows.Add(row);
				}
				catch { }
			}

			return dt;
		}

		private static string[] GetRow(string line, int cnt)
		{
			line.Replace("\"\"", "\"");
			string[] strs = line.Split(',');
			if (strs.Length == cnt)
				return strs;

			List<string> lst = new List<string>();
			int n = 0, begin = 0;
			bool flag = false;

			for (int i = 0; i < strs.Length; i++)
			{
				if (strs[i].IndexOf("\"") == -1
					|| (!flag && strs[i][0] != '\"'))
				{
					lst.Add(strs[i]);
					continue;
				}

				n = 0;
				foreach (char ch in strs[i])
				{
					if (ch == '\"')
						n++;
				}
				if (n % 2 == 0)
				{
					lst.Add(strs[i]);
					continue;
				}

				flag = true;
				begin = i;
				i++;
				for (i = begin + 1; i < strs.Length; i++)
				{
					foreach (char ch in strs[i])
					{
						if (ch == '\"')
							n++;
					}

					if (strs[i][strs[i].Length - 1] == '\"' && n % 2 == 0)
					{
						StringBuilder sb = new StringBuilder();
						for (; begin <= i; begin++)
						{
							sb.Append(strs[begin]);
							if (begin != i)
								sb.Append(",");
						}
						lst.Add(sb.ToString());
						break;
					}
				}
			}
			return lst.ToArray();
		}
	}
}
