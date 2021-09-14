using ICSharpCode.SharpZipLib.Zip;
using NameSpaceCommon;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ExcelSummarySheet
{
    public partial class ExcelSummary : Form
    {
        private static string localPath = AppDomain.CurrentDomain.BaseDirectory;
        public ExcelSummary()
        {
            InitializeComponent();
        }

        private void butzipImport_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Multiselect = true;
                ofd.DefaultExt = "zip";
                ofd.Filter = "Zip(*.zip)|*.zip";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    string[] fileNames = ofd.FileNames;
                    this.labzipCount.Text = fileNames.Count().ToString();
                    if (fileNames.Count() > 0)
                    {
                        this.labzipCount.ForeColor = Color.Red;
                    }


                    string recordPath = Path.Combine(localPath, "Record");
                    string recordZipPath = Path.Combine(recordPath, "Zip");//zip原始路径
                    string recordUnZipPath = Path.Combine(recordPath, "TempUnZip");//zip解压路径

                    CreateFilePath(recordPath);
                    CreateFilePath(recordZipPath);
                    CreateFilePath(recordUnZipPath);

                    foreach (string fileName in fileNames)
                    {
                        string zipFileName = fileName;
                        this.texzipFileInPath.Text = zipFileName;
                        FileInfo zipFileInfo = new FileInfo(zipFileName);
                        if (zipFileInfo.Extension.ToLower() != ".zip")
                        {
                            MessageBox.Show(string.Format("文件{0}，后缀不是ZIP文件", zipFileInfo.FullName));
                            return;
                        }

                        zipFileInfo.CopyTo(Path.Combine(recordZipPath, zipFileInfo.Name), true);
                        DirectoryInfo directoryInfo = new DirectoryInfo(recordUnZipPath);
                        NewUnZip(zipFileName, recordUnZipPath);
                    }
                    GetSeekUnZipFilePath(recordUnZipPath, recordUnZipPath);


                    string fileNotHaveSuffix = DateTime.Now.ToString("yyyyMMddHHmmss");
                    GetFile(fileNotHaveSuffix, recordPath, recordUnZipPath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("错误:{0}", ex.Message));
                return;
            }
        }


        private void butxlsImport_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Multiselect = true;
                ofd.DefaultExt = "xls";
                ofd.Filter = "Excel 文件|*.xls";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    string[] oldFileNames = ofd.FileNames;
                    this.labxlsCount.Text = oldFileNames.Count().ToString();
                    if (oldFileNames.Count() > 0)
                    {
                        this.labxlsCount.ForeColor = Color.Red;
                    }


                    string recordPath = Path.Combine(localPath, "Record");
                    string recordUnZipPath = Path.Combine(recordPath, "TempUnZip");//zip解压路径

                    foreach (string oldFileName in oldFileNames)
                    {
                        string xlsFileName = oldFileName;
                        this.texxlsFileInPath.Text = xlsFileName;
                        FileInfo xlsFileInfo = new FileInfo(xlsFileName);
                        if (xlsFileInfo.Extension.ToLower() != ".xls")
                        {
                            MessageBox.Show(string.Format("文件{0}，后缀不是XLS文件", xlsFileInfo.FullName));
                            return;
                        }

                        xlsFileInfo.CopyTo(Path.Combine(recordUnZipPath, xlsFileInfo.Name));
                    }


                    string fileNotHaveSuffix = DateTime.Now.ToString("yyyyMMddHHmmss");
                    GetFile(fileNotHaveSuffix, recordPath, recordUnZipPath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("错误:{0}", ex.Message));
                return;
            }
        }

        /// <summary>
        /// 生成xls文件
        /// </summary>
        /// <param name="oldfileName">文件名称</param>
        /// <param name="recordPath">临时文件路径</param>
        /// <param name="recordUnZipPath">临时文件夹路径</param>
        private void GetFile(string oldfileName, string recordPath, string recordUnZipPath)
        {
            DataSet ds = new DataSet();
            string fileNotHaveSuffix = oldfileName;

            OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter();

            string templatePath = Path.Combine(localPath, @"Template\CNXSSDModels.xls");


            string fn = string.Format("Temp_{0}.Xls", DateTime.Now.ToString("yyyyMMddHHmmss"));
            string exFnameDirectory = Path.Combine(recordPath, @"TemporaryFile");
            CreateFilePath(exFnameDirectory);
            string ExFname = Path.Combine(exFnameDirectory, fn);

            System.IO.File.Copy(templatePath, ExFname, true);

            ExcelToDataTable2(ExFname, "VTable", ref ds, "VTable", ref oleDbDataAdapter);

            string[] fileNames = Directory.GetFiles(recordUnZipPath);
            fileNames = fileNames.Where(p => p.ToLower().EndsWith(".xls")).ToArray();
            for (int i = 0; i < fileNames.Count(); i++)
            {
                string fileName = fileNames[i];

                FileInfo fileInfo = new FileInfo(fileName);

                if (fileInfo.Extension == ".xls")
                {
                    DataSet iDs = new DataSet();

                    ExcelToDataTable1(fileInfo.FullName, "VTable", ref iDs, "VTable");
                    if (ds.Tables.Count == 0)
                    {
                        ds = iDs.Copy();
                    }
                    else
                    {
                        if (iDs != null && iDs.Tables.Count > 0 && iDs.Tables[0].Rows.Count > 0)//长度是一致的，否则就报格式错误
                        {
                            if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0].ItemArray.Count() != iDs.Tables[0].Rows[0].ItemArray.Count())
                            {
                                MessageBox.Show(string.Format("文件{0}，格式不完整", fileName));
                                return;
                            }
                            DataTable dataTable = iDs.Tables[0];
                            if (dataTable.Rows.Count > 0 || dataTable.Rows.Count == 1)
                            {
                                foreach (DataRow dataRow in dataTable.Rows)
                                {
                                    DataRow newRow = ds.Tables[0].NewRow();
                                    newRow.ItemArray = dataRow.ItemArray;
                                    ds.Tables[0].Rows.Add(newRow);
                                }
                            }
                            else
                            {
                                MessageBox.Show(string.Format("文件{0}，格式不完整", fileName));
                                return;
                            }
                        }
                        else
                        {
                            MessageBox.Show(string.Format("文件{0}，格式不完整", fileName));
                            return;
                        }
                    }
                }
            }

            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.DefaultExt = "xls";
            saveDialog.Filter = "Excel 文件|*.xls";
            saveDialog.FileName = fileNotHaveSuffix + "_Convert";
            if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.texzipFileOutPath.Text = saveDialog.FileName;

                oleDbDataAdapter.Update(ds, "VTable");

                MessageBox.Show(string.Format("文件生成成功"));

                File.Copy(ExFname, saveDialog.FileName, true);

                if (Directory.Exists(recordUnZipPath))
                {
                    DelectDir(recordUnZipPath);
                }

                if (Directory.Exists(exFnameDirectory))
                {
                    DelectDir(exFnameDirectory);
                }

                return;
            }
        }

        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="path"></param>
        private void CreateFilePath(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="srcPath"></param>
        private static void DelectDir(string srcPath)
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
            }
            catch (Exception e)
            {
                throw;
            }
        }

        /// <summary>
        /// 递归对路径进行查询，如是文件夹则找下层，如不是则转移到指定路径下
        /// </summary>
        /// <param name="filePath">文件读取路径</param>
        /// <param name="UnZipFilePath">文件存放路径</param>
        /// <param name="index">读取层级</param>
        private void GetSeekUnZipFilePath(string filePath, string UnZipFilePath, int index = 0)
        {
            index++;
            bool isFile = GetIsFile(filePath);
            if (isFile)
            {
                if (index == 1)//当要解析文件是第一层级时，就不需要文件转移
                {
                    return;
                }
                else
                {
                    FileProcessing(filePath);
                }
            }
            else
            {
                string[] fileSystemEntries = Directory.GetFileSystemEntries(filePath);
                foreach (string fileSystemEntrie in fileSystemEntries)
                {
                    isFile = GetIsFile(fileSystemEntrie);

                    if (isFile)
                    {
                        FileProcessing(fileSystemEntrie);
                    }
                    else
                    {
                        string[] subFileSystemEntries = Directory.GetFileSystemEntries(fileSystemEntrie);
                        foreach (string subFileSystemEntrie in subFileSystemEntries)
                        {
                            isFile = GetIsFile(subFileSystemEntrie);
                            if (isFile)
                            {
                                FileProcessing(subFileSystemEntrie);
                            }
                            else
                            {
                                GetSeekUnZipFilePath(fileSystemEntrie, UnZipFilePath, index);
                            }
                        }
                    }
                }
            }

            void FileProcessing(string file)
            {
                if (File.Exists(file))
                {
                    FileInfo fileInfo = new FileInfo(file);
                    string newFilePath = Path.Combine(UnZipFilePath, fileInfo.Name);
                    if (file != newFilePath)
                    {
                        if (File.Exists(newFilePath))
                        {
                            File.Delete(newFilePath);
                        }
                        File.Move(file, newFilePath);
                    }
                }
            }
        }

        /// <summary>
        /// 判断是文件还是文件夹
        /// </summary>
        /// <param name="filePath">路径</param>
        /// <returns></returns>
        private bool GetIsFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public void ExcelToDataTable1(string opnFileName, string dbName, ref DataSet ds, string dstablename)
        {
            #region  读取Excel模板-POI
            string ExFname = opnFileName;
            OleDbConnection OleDbConnNew = ClsDB.ConnExcel(ExFname);
            OleDbDataAdapter OleDbAderNew;
            OleDbAderNew = new OleDbDataAdapter();
            OleDbAderNew.SelectCommand = new OleDbCommand("SELECT * from [VTable]", OleDbConnNew);
            OleDbCommandBuilder ScbldNew = new OleDbCommandBuilder(OleDbAderNew);
            ClsDB.ExcelToDsTable(ExFname, dbName, ref ds, dstablename);
            #endregion
        }

        public void ExcelToDataTable2(string opnFileName, string dbName, ref DataSet ds, string dstablename, ref OleDbDataAdapter oleDbDataAdapter)
        {
            #region  读取Excel模板-POI
            string ExFname = opnFileName;
            OleDbConnection OleDbConnNew = ClsDB.ConnExcel(ExFname);
            OleDbDataAdapter OleDbAderNew;
            OleDbAderNew = new OleDbDataAdapter();
            OleDbAderNew.SelectCommand = new OleDbCommand("SELECT * from [VTable]", OleDbConnNew);
            OleDbCommandBuilder ScbldNew = new OleDbCommandBuilder(OleDbAderNew);
            ClsDB.ExcelToDsTable(ExFname, dbName, ref ds, dstablename);
            #endregion

            oleDbDataAdapter = OleDbAderNew;
        }
        /// <summary>
        /// 解压释放资源
        /// </summary>
        /// <param name="FileToUpZip">待解压的文件</param>
        /// <param name="ZipedFolder">解压目标存放目录</param>
        public static void NewUnZip(string FileToUpZip, string ZipedFolder)
        {
            if (!File.Exists(FileToUpZip))
            {
                return;
            }
            if (!Directory.Exists(ZipedFolder))
            {
                Directory.CreateDirectory(ZipedFolder);
            }
            ZipInputStream s = null;
            ZipEntry theEntry = null;
            string fileName;
            try
            {
                s = new ZipInputStream(File.OpenRead(FileToUpZip));
                while ((theEntry = s.GetNextEntry()) != null)
                {
                    if (theEntry.Name != String.Empty)
                    {
                        fileName = Path.Combine(ZipedFolder, theEntry.Name);
                        if (fileName.EndsWith("/") || fileName.EndsWith("\\"))
                        {
                            Directory.CreateDirectory(fileName);
                            continue;
                        }
                        using (FileStream streamWriter = File.Create(fileName))
                        {
                            int size = 2048;
                            byte[] data = new byte[2048];
                            while (true)
                            {
                                size = s.Read(data, 0, data.Length);
                                if (size > 0)
                                {
                                    streamWriter.Write(data, 0, size);
                                }
                                else
                                {
                                    break;
                                }
                            }
                        };

                    }
                }
            }
            finally
            {
                if (theEntry != null)
                {
                    theEntry = null;
                }
                if (s != null)
                {
                    s.Close();
                    s = null;
                }
                GC.Collect();
                GC.Collect(1);
            }
        }
    }
}
