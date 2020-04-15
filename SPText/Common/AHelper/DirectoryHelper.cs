using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
	public class DirectoryHelper
	{
		/// <summary>
		/// 创建文件夹前先删除其文件夹以及内部的所有文件和文件夹
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public static DirectoryInfo CreateDirectoryBeforeDelete(string path)
		{
			//清空指定目（csvFolder）下的所有文件以及文件夹
			if (Directory.Exists(path))
			{
				Directory.Delete(path, true);
			}
			return Directory.CreateDirectory(path);
		}
	}
}
