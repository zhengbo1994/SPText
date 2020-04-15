using System.Linq;

namespace System.Collections.Generic
{
	/// <summary>
	/// list帮助类
	/// </summary>
	public static class ListHelper
	{
		/// <summary>
		/// 根据list再分组，得到矩阵列表
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source"></param>
		/// <param name="groupCol">每组数据个数</param>
		/// <returns></returns>
		public static IEnumerable<List<T>> GetGroupList<T>(this IEnumerable<T> source, int groupCol)
		{
			for (int i = 0; i < source.Count(); i = i + groupCol)
			{
				yield return source.Skip(i).Take(groupCol).ToList();
			}
		}
		/// <summary>
		/// list随机排序
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source"></param>
		/// <returns></returns>
		public static List<T> GetRandomList<T>(this List<T> source)
		{
			if (!source.Any())
			{
				return null;
			}
			Random random = new Random();
			List<T> newList = new List<T>();
			foreach (T item in source)
			{
				newList.Insert(random.Next(newList.Count + 1), item);
			}
			return newList;
		}

		/// <summary>
		/// 根据条件筛选，如果func为空，则返回默认原来未经筛选处理的list
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="func">筛选条件，可空</param>
		/// <returns></returns>
		public static List<T> WhereOrOriginalList<T>(this List<T> list, Func<T, bool> func = null)
		{
			if (func == null)
			{
				return list;
			}
			else
			{
				return list.Where(func).ToList();
			}
		}

		/// <summary>
		/// 在指定范围内得到n个不相等的随机数列表
		/// </summary>
		/// <param name="randomList">随机数的返回集合</param>
		/// <param name="minVal">生成的随机数可以包含该数字</param>
		/// <param name="maxVal">生成的随机数不包含该数字</param>
		/// <param name="num">产生随机数的个数，num的值要小于等于（maxVal-minVal）</param>
		/// <param name="seedInt">一般不传参，作为产生随机数的种子使用，但不全是作为种子，内部会累加</param>
		public static void RandomNotEqualList(ref List<int> randomList, int minVal, int maxVal, int num, int seedInt = 0)
		{
			if (randomList == null)
			{
				randomList = new List<int>();
			}
			if (maxVal < minVal)
			{
				throw new Exception(" RandomHelper.GetRandomNotEqualList() 参数不合法！");
			}
			else if (maxVal == minVal)
			{
				if (num <= 0 || num > 1)
				{
					throw new Exception("maxVal == minVal 时， num <= 0 || num > 1 参数不合法");
				}
				randomList = new List<int> { minVal };
				return;
			}
			else
			{
				//maxVal > minVal
				if (num > maxVal + 1 - minVal)
				{
					throw new Exception("maxVal > minVal 时， num > maxVal + 1 - minVal 参数不合法");
				}
				else if (num == maxVal + 1 - minVal)
				{
					for (int i = minVal; i < maxVal + 1; i++)
					{
						randomList.Add(i);
					}
					return;
				}
				else
				{
					if (seedInt >= int.MaxValue - 2)
					{
						seedInt = 0;
					}
					Random r = new Random(DateTimeHelper.GetTotalSecondsIntOfThisYear() + seedInt);

					randomList.Add(r.Next(minVal, maxVal + 1));
					randomList = randomList.Distinct().ToList();
					if (randomList.Count() < num)
					{
						RandomNotEqualList(ref randomList, minVal, maxVal, num, seedInt + 1);
					}
				}
			}
		}
	}
}
