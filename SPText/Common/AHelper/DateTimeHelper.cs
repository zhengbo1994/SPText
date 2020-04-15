namespace System
{
	public static class DateTimeHelper
	{
		/// <summary>
		/// 输出默认格式的日期字符串（yyyy-MM-dd HH:mm:ss）
		/// </summary>
		/// <param name="time"></param>
		/// <returns></returns>
		public static string ToStr(this DateTime time)
		{
			return time.ToString("yyyy-MM-dd HH:mm:ss");
		}

		public static uint GetUtcUIntFromTime(DateTime time)
		{
			return (uint)((time.AddHours(-8) - DateTimeConst.UnixDateTimeZeroPoint).TotalSeconds);
		}

		public static DateTime GetTimeFromUtcUInt(uint timeUint)
		{
			return DateTimeConst.UnixDateTimeZeroPoint.Add(TimeSpan.FromSeconds(timeUint)).AddHours(8);
		}

		/// <summary>
		/// 获取今年已经过了多少秒
		/// </summary>
		/// <returns></returns>
		public static int GetTotalSecondsIntOfThisYear()
		{
		    //return Convert.ToInt32((DateTimeOffset1.Now - new DateTimeOffset1(new DateTime(DateTimeOffset1.Now.Year, 1, 1))).TotalSeconds);
			return Convert.ToInt32((DateTime.Now - new DateTime(DateTime.Now.Year, 1, 1)).TotalSeconds);
		}
		/// <summary>
		/// 获取指定时间的Unix时间戳字符串
		/// </summary>
		/// <returns></returns>
		public static string GetTimeStamp(this DateTime time)
		{
			return ConvertDateTimeToInt(time).ToString();
		}
		/// <summary>  
		/// 将c# DateTime时间格式转换为Unix时间戳格式  
		/// </summary>  
		/// <param name="time">时间</param>  
		/// <returns>long</returns>  
		public static long ConvertDateTimeToInt(DateTime time)
		{
			DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0, 0));
			return (time.Ticks - startTime.Ticks) / 10000000;   //除10000调整为13位      
		}

		public class DateTimeConst
		{
			/// <summary>
			/// 默认时间
			/// </summary>
			//public static DateTimeOffset1 DefaultTime = new DateTimeOffset1(new DateTime(1900, 1, 1));
			public static DateTime DefaultTime = new DateTime(1900, 1, 1);

			//public static DateTimeOffset1 UnixDateTimeZeroPoint = new DateTimeOffset1(new DateTime(1970, 01, 01, 0, 0, 0, DateTimeKind.Utc));
            public static DateTime UnixDateTimeZeroPoint = new DateTime(1970, 01, 01, 0, 0, 0, DateTimeKind.Utc);

		}
	}
}
