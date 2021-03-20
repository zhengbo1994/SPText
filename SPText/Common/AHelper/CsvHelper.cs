using SPText.Common.AHelper;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
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

		private static string[] GetRow1(string line, int cnt)
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

        /// <summary>
        /// 将DataTable中数据写入到CSV文件中
        /// </summary>
        /// <param name="dt">提供保存数据的DataTable</param>
        /// <param name="fileName">CSV的文件路径</param>
        public static bool SaveCSV(DataTable dt, string fullPath)
        {
            FileInfo fi = new FileInfo(fullPath);
            if (!fi.Directory.Exists)
            {
                fi.Directory.Create();
            }
            FileStream fs = new FileStream(fullPath, System.IO.FileMode.Create, System.IO.FileAccess.Write);
            //StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);
            StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8);
            string data = "";
            //写出列名称
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                data += dt.Columns[i].ColumnName.ToString();
                if (i < dt.Columns.Count - 1)
                {
                    data += ",";
                }
            }
            sw.WriteLine(data);
            //写出各行数据
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                data = "";
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string str = dt.Rows[i][j].ToString();
                    str = str.Replace("\"", "\"\"");//替换英文冒号 英文冒号需要换成两个冒号
                    if (str.Contains(",") || str.Contains("\"")
                        || str.Contains("\r") || str.Contains("\n")) //含逗号 冒号 换行符的需要放到引号中
                    {
                        str = string.Format("\"{0}\"", str);
                    }

                    data += str;
                    if (j < dt.Columns.Count - 1)
                    {
                        data += ",";
                    }
                }
                sw.WriteLine(data);
            }
            sw.Close();
            fs.Close();
            return true;
            //if (result == DialogResult.OK)
            //{
            //    System.Diagnostics.Process.Start("explorer.exe", Common.PATH_LANG);
            //}
        }

        /// <summary>
        /// 打开Csv订单文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static DataSet OpenCsvbyTxt(string filePath)
        {
            int n = 0;
            DataTable dt = new DataTable("Datas");
            string str = TxtHelper.Read(filePath);
            string[] strArr = str.Split('\n');
            for (int i = 0; i < strArr.Length; i++)
            {
                if (strArr[i].Length > 0 && strArr[i].Last() == '\r')
                {
                    strArr[i] = strArr[i].Substring(0, strArr[i].Length - 1);
                }
            }
            for (int j = 0; j < strArr.Length; j++)
            {
                string[] split = strArr[j].Split(',');
                if (split[0] == "")
                {
                    break;
                }
                List<string> list = new List<string>();
                string ss = "";
                for (int i = 0; i < split.Length; i++)
                {
                    if (split[i] == "")
                    {
                        list.Add(split[i]);
                        continue;
                    }
                    if (split[i][0] == '"' && split[i].Last() != '"')
                    {
                        ss = split[i];
                    }
                    else if (split[i][0] != '"' && split[i].Last() == '"')
                    {
                        ss = ss + "," + split[i];
                        list.Add(ss);
                    }
                    else
                    {
                        list.Add(split[i]);
                    }
                }

                if (n == 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        dt.Columns.Add(list[i].Replace("\"", ""), typeof(string));
                    }
                    n++;
                }
                else
                {
                    DataRow dr = dt.NewRow();

                    for (int i = 0; i < list.Count; i++)
                    {
                        dr[i] = list[i].ToString().Replace("\"", "");
                    }
                    dt.Rows.Add(dr);
                    n++;
                }
            }
            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            return ds;
        }

        public static DataSet OpenCsvByExcel(string filePath)
        {
            DataSet ds = null;
            try
            {
                string _path = filePath.Substring(0, filePath.LastIndexOf("\\"));
                string _name = filePath.Substring(filePath.LastIndexOf("\\") + 1);

                //office 2003 及以下
                string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + _path + ";Extended Properties='Text;HDR=Yes;IMEX=1;'";

                //office 2007 及以上
                //string strCon = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + _path + ";Extended Properties='Text;HDR=Yes;IMEX=1;'";

                OleDbConnection myConn = new OleDbConnection(strCon);
                string sqlOrders = "select * from [" + _name + "]";

                myConn.Open();
                OleDbDataAdapter daOrders = new OleDbDataAdapter(sqlOrders, myConn);

                ds = new DataSet();

                daOrders.Fill(ds, "orders");

                myConn.Close();

            }
            catch
            {
                try
                {
                    string _path = filePath.Substring(0, filePath.LastIndexOf("\\"));
                    string _name = filePath.Substring(filePath.LastIndexOf("\\") + 1);

                    //office 2003 及以下
                    //string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + _path + ";Extended Properties='Text;HDR=Yes;IMEX=1;'";

                    //office 2007 及以上
                    string strCon = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + _path + ";Extended Properties='Text;HDR=Yes;IMEX=1;'";

                    OleDbConnection myConn = new OleDbConnection(strCon);
                    string sqlOrders = "select * from [" + _name + "]";

                    myConn.Open();
                    OleDbDataAdapter daOrders = new OleDbDataAdapter(sqlOrders, myConn);

                    ds = new DataSet();

                    daOrders.Fill(ds, "orders");

                    myConn.Close();
                }
                catch
                {
                    ds = null;
                }

            }

            return ds;
        }

        /// <summary>
        /// 新的TXT读取方法，读取csv文件到DataTable
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public static DataTable ReadCsv1(string filepath)
        {
            DataTable dt = new DataTable("NewTable");
            DataRow row;

            //例子ORDER_ACCOUNT,ORDER_TYPE,ORDERS_ID,CUSTOMER_NUMBER,PATIENT_FIRST_NAME,PATIENT_LAST_NAME,BARCODE,DELIVERY_DATE,R_DIA,R_DIST_PD,R_NEAR_PD,R_HEIGHT,R_LENS,R_SPH,R_CYL,R_ADD,R_AXIS,R_QTY,R_ET,R_CT,R_BC,R_A,R_B,R_ED,R_PRISM1,R_BASIS1,R_PRISM2,R_BASIS2,L_DIA,L_DIST_PD,L_NEAR_PD,L_HEIGHT,L_LENS,L_SPH,L_CYL,L_ADD,L_AXIS,L_QTY,L_ET,L_CT,L_BC,L_A,L_B,L_ED,L_PRISM1,L_BASIS1,L_PRISM2,L_BASIS2,TINT_CODE,TINT_DESC,UV,HARD,CORRIDOR,DBL,SUPPLY_FRAME,FRAME_TYPE,Frame_Manufacturer,Frame_Color,REMARK,CREATION_DATE,Lens_Material,Material_Color,Lens_Options_1,Lens_Options_2,Lens_Options_3,Lens_Options_4,Lens_Options_5,\"Special Rx Instructions\",\"Recipient Company Name\",\"Recipient Address Line 1\",\"Recipient Address Line 2\",\"Recipient City\",\"Recipient State\",\"Recipient Zip Code\",\"Recipient Country\",Vertex,Panto,Wrap,REOCHght,LEOCHght,\"ADD TO B\",\"CLASS _ID (Buying Group)\",\"Frame Style (Name)\",\"Reference Order ID\"\nNA332,,45167,48451399,Lisa,Diamond,,,,31.5,,20.0,APCPR,0.50,-0.50,2.00,027,1,,,,52.0,35.5,56.9,,0,,0,,30.0,,20.0,APCPR,0.50,-0.50,2.00,134,1,,,,52.0,35.5,56.9,,0,,0,,,N,N,,18.0,818229021184,Supra,twelve84,\"Silver Coin\",,,P,,HMC,N,,,,\"1284\r1st PAL \",\"Woodbine Eye Care, PA\",\"5389 Woodbine Rd. \",,Pace,FL,32571,USA,,,,,,,,Lisbon,24755201\n

            //string[] lines = File.ReadAllLines(filepath, Encoding.UTF8);
            string str = File.ReadAllText(filepath);
            //string[] lines = str.Split("\r\n");
            string[] lines = str.Split(new string[] { "\n" }, StringSplitOptions.None);
            lines = lines.Where(s => !string.IsNullOrEmpty(s)).ToArray();

            //string[] lines3 = File.ReadAllLines(filepath);

            string[] head = lines[0].Split(',');
            int cnt = head.Length;
            for (int i = 0; i < cnt; i++)
            {
                dt.Columns.Add(head[i].Replace("\"", "").Trim());
            }
            for (int i = 1; i < lines.Length; i++)
            {
                if ((lines[i].Trim() == ""))
                {
                    continue;
                }
                try
                {
                    row = dt.NewRow();
                    row.ItemArray = GetRow(lines[i], cnt);
                    dt.Rows.Add(row);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return dt;
        }



        /// <summary>
        /// 解析字符串 获取 该行的数据 已经处理,及"号
        /// </summary>
        /// <param name="line">该行的内容</param>
        /// <param name="cnt">总的条目数</param>
        /// <returns></returns>
        static private string[] GetRow(string line, int cnt)
        {
            line.Replace("\"\"", "\"");
            string[] strs = line.Split(',');
            if (strs.Length == cnt)
            {
                for (int i = 0; i < strs.Length; i++)
                {
                    if (strs[i].Length > 0 && strs[i][0] == '\"' && strs[i][strs[i].Length - 1] == '\"')
                    {
                        strs[i] = strs[i].Remove(0, 1);
                        strs[i] = strs[i].Remove(strs[i].Length - 1, 1);
                    }
                }
                return strs;
            }
            List<string> list = new List<string>();
            int n = 0, begin = 0;
            bool flag = false;

            for (int i = 0; i < strs.Length; i++)
            {
                //没有引号 或者 中间有引号 直接添加
                if (strs[i].IndexOf("\"") == -1
                    || (flag == false && strs[i][0] != '\"'))
                {
                    list.Add(strs[i]);
                    continue;
                }
                //其实有引号，但该段没有,号，直接添加
                n = 0;
                foreach (char ch in strs[i])
                {
                    if (ch == '\"')
                    {
                        n++;
                    }
                }
                if (n % 2 == 0)
                {
                    if (strs[i][0] == '\"')
                    {
                        strs[i] = strs[i].Remove(0, 1);
                    }
                    if (strs[i][strs[i].Length - 1] == '\"')
                    {
                        strs[i] = strs[i].Remove(strs[i].Length - 1, 1);
                    }
                    list.Add(strs[i]);
                    continue;
                }
                //该段有引号 有 ,号，下一段增加后添加
                flag = true;
                begin = i;
                i++;
                for (i = begin + 1; i < strs.Length; i++)
                {
                    foreach (char ch in strs[i])
                    {
                        if (ch == '\"')
                        {
                            n++;
                        }
                    }
                    if (strs[i].Length > 0 && strs[i][strs[i].Length - 1] == '\"' && n % 2 == 0)
                    {
                        StringBuilder sb = new StringBuilder();
                        for (; begin <= i; begin++)
                        {
                            sb.Append(strs[begin]);
                            if (begin != i)
                            {
                                sb.Append(",");
                            }
                        }
                        string content = sb.ToString();
                        if (content[0] == '\"')
                        {
                            content = content.Remove(0, 1);
                        }
                        if (content[content.Length - 1] == '\"')
                        {
                            content = content.Remove(content.Length - 1, 1);
                        }
                        list.Add(content);
                        break;
                    }
                }
            }
            return list.ToArray();
        }

        #region  数据（正式上面bug修改备份）
        /// <summary>
        /// 新的TXT读取方法，读取csv文件到DataTable
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public static DataTable ReadCsv2(string filepath)
        {
            DataTable dt = new DataTable("NewTable");
            DataRow row;

            string str = File.ReadAllText(filepath);
            string[] lines = str.Split(new string[] { "\n" }, StringSplitOptions.None);
            lines = lines.Where(s => !string.IsNullOrEmpty(s)).ToArray();

            string[] lines3 = File.ReadAllLines(filepath);

            List<string> lineList = new List<string>();
            int iCount = 0;
            for (int i = 0; i < lines.Count(); i++)
            {
                var line = lines[i];
                if (line.StartsWith("NA332"))//列如已NA332开头，中间出现换行
                {
                    lineList.Add(line);
                    iCount++;
                }
                else
                {
                    if (i == 0)
                    {
                        lineList.Add(line);
                    }
                    else
                    {
                        lineList[iCount] = lineList[iCount] + "\n" + line;
                    }
                }
            }

            string[] head = lineList[0].Split(',');
            int cnt = head.Length;
            for (int i = 0; i < cnt; i++)
            {
                dt.Columns.Add(head[i].Replace("\"", "").Trim());
            }
            for (int i = 1; i < lineList.Count; i++)
            {
                if ((lineList[i].Trim() == ""))
                {
                    continue;
                }
                try
                {
                    row = dt.NewRow();
                    row.ItemArray = GetRow(lineList[i], cnt);
                    dt.Rows.Add(row);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return dt;
        }

        /// <summary>
        /// 解析字符串 获取 该行的数据 已经处理,及"号
        /// </summary>
        /// <param name="line">该行的内容</param>
        /// <param name="cnt">总的条目数</param>
        /// <returns></returns>
        static private string[] GetRow2(string line, int cnt)
        {
            line.Replace("\"\"", "\"");
            string[] strs = line.Split(',');
            if (strs.Length == cnt)
            {
                for (int i = 0; i < strs.Length; i++)
                {
                    if (strs[i].Length > 0 && strs[i][0] == '\"' && strs[i][strs[i].Length - 1] == '\"')
                    {
                        strs[i] = strs[i].Remove(0, 1);
                        strs[i] = strs[i].Remove(strs[i].Length - 1, 1);
                    }
                    if (strs[i].EndsWith("\r"))
                    {
                        strs[i] = strs[i].Replace("\r", "");
                    }
                }
                return strs;
            }
            List<string> list = new List<string>();
            int n = 0, begin = 0;
            bool flag = false;

            for (int i = 0; i < strs.Length; i++)
            {
                //没有引号 或者 中间有引号 直接添加
                if (strs[i].IndexOf("\"") == -1
                    || (flag == false && strs[i][0] != '\"'))
                {
                    if (strs[i].EndsWith("\r"))
                    {
                        strs[i] = strs[i].Replace("\r", "");
                    }
                    list.Add(strs[i]);
                    continue;
                }
                //其实有引号，但该段没有,号，直接添加
                n = 0;
                foreach (char ch in strs[i])
                {
                    if (ch == '\"')
                    {
                        n++;
                    }
                }
                if (n % 2 == 0)
                {
                    if (strs[i][0] == '\"')
                    {
                        strs[i] = strs[i].Remove(0, 1);
                    }
                    if (strs[i][strs[i].Length - 1] == '\"')
                    {
                        strs[i] = strs[i].Remove(strs[i].Length - 1, 1);
                    }
                    list.Add(strs[i]);
                    continue;
                }
                //该段有引号 有 ,号，下一段增加后添加
                flag = true;
                begin = i;
                i++;
                for (i = begin + 1; i < strs.Length; i++)
                {
                    foreach (char ch in strs[i])
                    {
                        if (ch == '\"')
                        {
                            n++;
                        }
                    }
                    if (strs[i].Length > 0 && strs[i][strs[i].Length - 1] == '\"' && n % 2 == 0)
                    {
                        StringBuilder sb = new StringBuilder();
                        for (; begin <= i; begin++)
                        {
                            sb.Append(strs[begin]);
                            if (begin != i)
                            {
                                sb.Append(",");
                            }
                        }
                        string content = sb.ToString();
                        if (content[0] == '\"')
                        {
                            content = content.Remove(0, 1);
                        }
                        if (content[content.Length - 1] == '\"')
                        {
                            content = content.Remove(content.Length - 1, 1);
                        }
                        list.Add(content);
                        break;
                    }
                }
            }
            return list.ToArray();
        }
        #endregion
    }
}
