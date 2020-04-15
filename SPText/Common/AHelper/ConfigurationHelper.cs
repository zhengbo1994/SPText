//using System.Configuration;

//namespace System
//{
//	/// <summary>
//	/// 配置文件帮助类
//	/// </summary>
//	public class ConfigurationHelper
//	{
//		/// <summary>
//		/// 获取 ConnectionString value
//		/// </summary>
//		/// <param name="str"></param>
//		/// <returns></returns>
//		public static string GetConnStr(string str)
//		{
//			var theStr = ConfigurationManager.ConnectionStrings[str] != null ? ConfigurationManager.ConnectionStrings[str].ToString() : "";
//			if (theStr.IsNullOrWhiteSpace())
//			{
//				throw new Exception("ConnectionStrings不能为空，请更新配置文件");
//			}
//			return theStr;
//		}

//		/// <summary>
//		/// 获取 AppSetting value
//		/// </summary>
//		/// <param name="str"></param>
//		/// <returns></returns>
//		public static string GetAppSettingStr(string str)
//		{
//			var theStr = ConfigurationManager.AppSettings[str] ?? "";
//			if (theStr.IsNullOrWhiteSpace())
//			{
//				throw new Exception("AppSettings不能为空，请更新配置文件");
//			}
//			return theStr;
//		}
//	}
//}
