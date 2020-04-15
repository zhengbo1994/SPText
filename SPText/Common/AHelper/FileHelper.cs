using System.IO;
using System.Text;

namespace System
{
	public class FileHelper
	{
		/// <summary>
		/// 文件写入
		/// </summary>
		/// <param name="path"></param>
		/// <param name="str"></param>
		/// <param name="encoding"></param>
		public static void Write(string path, string str, Encoding encoding)
		{
			string directoryPath = Path.GetDirectoryName(path);
			if (!Directory.Exists(directoryPath))
			{
				Directory.CreateDirectory(directoryPath);
			}
			using (FileStream fs = new FileStream(path, FileMode.Create))
			{
				using (StreamWriter sw = new StreamWriter(fs, encoding))
				{
					sw.Write(str);
				}
			}
		}

		/// <summary>
		/// 如果文件存在，则删除
		/// </summary>
		/// <param name="path"></param>
		public static void DeleteIfExists(string path)
		{
			if (File.Exists(path))
			{
				File.Delete(path);
			}
		}
	}
}
