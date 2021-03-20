using System.IO;
using System.Runtime.InteropServices;
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

        /// <summary>
        /// 删除文件目录下的所有文件
        /// </summary>
        /// <param name="srcPath"></param>
        public static void DelectDir(string srcPath)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(srcPath);
                FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();  //返回目录中所有文件和子目录
                foreach (FileSystemInfo i in fileinfo)
                {
                    if (i is DirectoryInfo)            //判断是否文件夹
                    {
                        DirectoryInfo subdir = new DirectoryInfo(i.FullName);
                        subdir.Delete(true);          //删除子目录和文件
                    }
                    else
                    {
                        File.Delete(i.FullName);      //删除指定文件
                    }
                }
                Directory.Delete(srcPath);
            }
            catch (Exception)
            {
                throw;
            }
        }


        [DllImport("kernel32.dll")]
        public static extern IntPtr _lopen(string lpPathName, int iReadWrite);

        [DllImport("kernel32.dll")]
        public static extern bool CloseHandle(IntPtr hObject);

        public const int OF_READWRITE = 2;
        public const int OF_SHARE_DENY_NONE = 0x40;
        public static readonly IntPtr HFILE_ERROR = new IntPtr(-1);
        /// <summary>
        /// 文件已打开
        /// </summary>
        /// <param name="vFileName"></param>
        /// <returns></returns>
        public static int FileIsOpen(string vFileName)
        {
            if (!File.Exists(vFileName))
            {
                return -1;
            }
            IntPtr vHandle = _lopen(vFileName, OF_READWRITE | OF_SHARE_DENY_NONE);
            if (vHandle == HFILE_ERROR)
            {

                return 1;
            }
            CloseHandle(vHandle);
            return 0;
        }
    }
}
