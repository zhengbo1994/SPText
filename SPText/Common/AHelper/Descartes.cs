using System.Collections.Generic;
using System.Linq;

namespace System
{
	/// <summary>
	/// 笛卡尔积
	/// </summary>
	public static class Descartes
	{
		/// <summary>
		/// 笛卡尔积
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="dimvalue"></param>
		/// <param name="result"></param>
		/// <param name="curstring"></param>
		/// <param name="layer"></param>
		public static void CartesianProduct<T>(this List<List<T>> dimvalue, ref List<List<T>> result, List<T> curstring = default(List<T>), int layer = 0)
		{
			if (curstring == null)
			{
				curstring = new List<T>();
			}
			if (layer < dimvalue.Count - 1)
			{
				if (dimvalue[layer].Count == 0)
				{
					CartesianProduct(dimvalue, ref result, curstring, layer + 1);
				}
				else
				{
					for (int i = 0; i < dimvalue[layer].Count; i++)
					{
						List<T> li = new List<T>();
						li.AddRange(curstring);
						li.Add(dimvalue[layer][i]);
						CartesianProduct(dimvalue, ref result, li, layer + 1);
					}
				}
			}
			else if (layer == dimvalue.Count - 1)
			{
				if (dimvalue[layer].Count == 0)
				{
					result.Add(curstring);
				}
				else
				{
					for (int i = 0; i < dimvalue[layer].Count; i++)
					{
						result.Add(curstring.Concat(new List<T> { dimvalue[layer][i] }).ToList());
					}
				}
			}
		}
	}
}
