using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SPText.Common
{
    public class FtpHelper
    {
        string ftpServerIP;
        string ftpRemotePath;
        string ftpUserID;
        string ftpPassword;
        string ftpURI;

        /// <summary>
        /// 连接FTP
        /// </summary>
        /// <param name="FtpServerIP">FTP连接地址</param>
        /// <param name="FtpRemotePath">指定FTP连接成功后的当前目录, 如果不指定即默认为根目录</param>
        /// <param name="FtpUserID">用户名</param>
        /// <param name="FtpPassword">密码</param>
        public FtpHelper(string FtpServerIP, string FtpRemotePath, string FtpUserID, string FtpPassword)
        {
            ftpServerIP = FtpServerIP;
            ftpRemotePath = FtpRemotePath;
            ftpUserID = FtpUserID;
            ftpPassword = FtpPassword;
            //ftpURI = "ftp://" + ftpServerIP + "/" + ftpRemotePath + "/";
            ftpURI = CheckDirectory();
        }

        /// <summary>
        /// 上传
        /// </summary>
        /// <param name="filename"></param>
        public bool Upload(string filename, out string errorMsg)
        {
            errorMsg = "";
            FileInfo fileInf = new FileInfo(filename);
            string uri = ftpURI + fileInf.Name;
            FtpWebRequest reqFTP;

            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
            reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
            reqFTP.KeepAlive = false;
            reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
            reqFTP.UseBinary = true;
            reqFTP.ContentLength = fileInf.Length;
            int buffLength = 2048; //开辟2KB缓存区
            byte[] buff = new byte[buffLength];
            int contentLen;
            FileStream fs = fileInf.OpenRead();
            try
            {
                Stream strm = reqFTP.GetRequestStream();
                contentLen = fs.Read(buff, 0, buffLength);
                while (contentLen != 0)
                {
                    strm.Write(buff, 0, contentLen);
                    contentLen = fs.Read(buff, 0, buffLength);
                }
                strm.Close();
                fs.Close();
                return true;
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 下载
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="fileName"></param>
        public bool Download(string filePath, string fileName, out string errorMsg)
        {
            errorMsg = "";
            FtpWebRequest reqFTP;
            try
            {
                FileStream outputStream = new FileStream(filePath + "//" + fileName, FileMode.Create);

                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpURI + fileName));
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                long cl = response.ContentLength;
                int bufferSize = 2048;
                int readCount;
                byte[] buffer = new byte[bufferSize];

                readCount = ftpStream.Read(buffer, 0, bufferSize);
                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }

                ftpStream.Close();
                outputStream.Close();
                response.Close();
                return true;
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return false;
            }
        }
        #region 私有
        /// <summary>
        /// 获得+建立文件所在物理路径
        /// </summary>
        /// <returns></returns>
        private string CheckDirectory()
        {
            string dir = "ftp://" + ftpServerIP + "/";//根
            if (!string.IsNullOrWhiteSpace(ftpRemotePath))
            {
                var folderArr = ftpRemotePath.Split(new string[] { @"\", @"/" }, StringSplitOptions.RemoveEmptyEntries);
                if (folderArr != null && folderArr.Length > 0)
                {
                    foreach (var folder in folderArr)
                    {
                        dir = dir + folder + "/";
                        MakeDir(dir);
                    }
                }
            }
            return dir;
        }
        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        private string MakeDir(string dir)
        {
            string errorMsg = "";
            FtpWebRequest reqFTP;
            try
            {
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(dir));
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                reqFTP.Method = WebRequestMethods.Ftp.MakeDirectory;
                reqFTP.KeepAlive = false;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                response.Close();
            }
            catch (Exception ex)
            {
                errorMsg = "创建文件失败，原因: " + ex.Message;
            }
            return errorMsg;
        }
        #endregion


        /// <summary>
        /// 上传文件
        /// </summary> /
        // <param name="fileinfo">需要上传的文件</param>
        /// <param name="targetDir">目标路径</param>
        /// <param name="hostname">ftp地址</param> /
        // <param name="username">ftp用户名</param> /
        // <param name="password">ftp密码</param>
        public static void UploadFile(FileInfo fileinfo, string targetDir, string hostname, string username, string password)
        { //1. check target
            string target;
            if (targetDir.Trim() == "")
            { return; }
            target = Guid.NewGuid().ToString();
            //使用临时文件名
            string URI = "FTP://" + hostname + "/" + targetDir + "/" + target;
            ///WebClient webcl = new WebClient();
            System.Net.FtpWebRequest ftp = GetRequest(URI, username, password);
            //设置FTP命令 设置所要执行的FTP命令，
            //ftp.Method = System.Net.WebRequestMethods.Ftp.ListDirectoryDetails;//假设此处为显示指定路径下的文件列表
            ftp.Method = System.Net.WebRequestMethods.Ftp.UploadFile;
            //指定文件传输的数据类型
            ftp.UseBinary = true;
            ftp.UsePassive = true; //告诉ftp文件大小
            ftp.ContentLength = fileinfo.Length;
            //缓冲大小设置为2KB
            const int BufferSize = 2048;
            byte[] content = new byte[BufferSize - 1 + 1];
            int dataRead; //打开一个文件流 (System.IO.FileStream) 去读上传的文件
            using (FileStream fs = fileinfo.OpenRead())
            {
                try
                { //把上传的文件写入流
                    using (Stream rs = ftp.GetRequestStream())
                    {
                        do
                        { //每次读文件流的2KB
                            dataRead = fs.Read(content, 0, BufferSize); rs.Write(content, 0, dataRead);
                        }
                        while (!(dataRead < BufferSize)); rs.Close();
                    }
                }
                catch (Exception ex) { }
                finally { fs.Close(); }
            }
            ftp = null; //设置FTP命令
            ftp = GetRequest(URI, username, password);
            ftp.Method = System.Net.WebRequestMethods.Ftp.Rename; //改名
            ftp.RenameTo = fileinfo.Name; try { ftp.GetResponse(); }
            catch (Exception ex)
            {
                ftp = GetRequest(URI, username, password); ftp.Method = System.Net.WebRequestMethods.Ftp.DeleteFile; //删除
                ftp.GetResponse(); throw ex;
            }
            finally
            {
                //fileinfo.Delete(); } // 可以记录一个日志 "上传" + fileinfo.FullName + "上传到" + "FTP://" + hostname + "/" + targetDir + "/" + fileinfo.Name + "成功." );
                ftp = null;
                #region
                /***** *FtpWebResponse * ****/ //FtpWebResponse ftpWebResponse = (FtpWebResponse)ftp.GetResponse();
                #endregion
            }
        }
        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="localDir">下载至本地路径</param>
        /// <param name="FtpDir">ftp目标文件路径</param>
        /// <param name="FtpFile">从ftp要下载的文件名</param>
        /// <param name="hostname">ftp地址即IP</param>
        /// <param name="username">ftp用户名</param>
        /// <param name="password">ftp密码</param>
        public static void DownloadFile(string localDir, string FtpDir, string FtpFile, string hostname, string username, string password)
        {
            string URI = "FTP://" + hostname + "/" + FtpDir + "/" + FtpFile;
            string tmpname = Guid.NewGuid().ToString();
            string localfile = localDir + @"\" + tmpname;

            System.Net.FtpWebRequest ftp = GetRequest(URI, username, password);
            ftp.Method = System.Net.WebRequestMethods.Ftp.DownloadFile;
            ftp.UseBinary = true;
            ftp.UsePassive = false;

            using (FtpWebResponse response = (FtpWebResponse)ftp.GetResponse())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    //loop to read & write to file
                    using (FileStream fs = new FileStream(localfile, FileMode.CreateNew))
                    {
                        try
                        {
                            byte[] buffer = new byte[2048];
                            int read = 0;
                            do
                            {
                                read = responseStream.Read(buffer, 0, buffer.Length);
                                fs.Write(buffer, 0, read);
                            } while (!(read == 0));
                            responseStream.Close();
                            fs.Flush();
                            fs.Close();
                        }
                        catch (Exception)
                        {
                            //catch error and delete file only partially downloaded
                            fs.Close();
                            //delete target file as it's incomplete
                            File.Delete(localfile);
                            throw;
                        }
                    }

                    responseStream.Close();
                }

                response.Close();
            }



            try
            {
                File.Delete(localDir + @"\" + FtpFile);
                File.Move(localfile, localDir + @"\" + FtpFile);


                ftp = null;
                ftp = GetRequest(URI, username, password);
                ftp.Method = System.Net.WebRequestMethods.Ftp.DeleteFile;
                ftp.GetResponse();

            }
            catch (Exception ex)
            {
                File.Delete(localfile);
                throw ex;
            }

            // 记录日志 "从" + URI.ToString() + "下载到" + localDir + @"\" + FtpFile + "成功." );
            ftp = null;
        }

        /// <summary>
        /// 搜索远程文件
        /// </summary>
        /// <param name="targetDir"></param>
        /// <param name="hostname"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="SearchPattern"></param>
        /// <returns></returns>
        public static List<string> ListDirectory(string targetDir, string hostname, string username, string password, string SearchPattern)
        {
            List<string> result = new List<string>();
            try
            {
                string URI = "FTP://" + hostname + "/" + targetDir + "/" + SearchPattern;

                System.Net.FtpWebRequest ftp = GetRequest(URI, username, password);
                ftp.Method = System.Net.WebRequestMethods.Ftp.ListDirectory;
                ftp.UsePassive = true;
                ftp.UseBinary = true;


                string str = GetStringResponse(ftp);
                str = str.Replace("\r\n", "\r").TrimEnd('\r');
                str = str.Replace("\n", "\r");
                if (str != string.Empty)
                    result.AddRange(str.Split('\r'));

                return result;
            }
            catch { }
            return null;
        }

        private static string GetStringResponse(FtpWebRequest ftp)
        {
            //Get the result, streaming to a string
            string result = "";
            using (FtpWebResponse response = (FtpWebResponse)ftp.GetResponse())
            {
                long size = response.ContentLength;
                using (Stream datastream = response.GetResponseStream())
                {
                    using (StreamReader sr = new StreamReader(datastream, System.Text.Encoding.Default))
                    {
                        result = sr.ReadToEnd();
                        sr.Close();
                    }

                    datastream.Close();
                }

                response.Close();
            }

            return result;
        }
        /// 在ftp服务器上创建目录
        /// </summary>
        /// <param name="dirName">创建的目录名称</param>
        /// <param name="ftpHostIP">ftp地址</param>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        public void MakeDir(string dirName, string ftpHostIP, string username, string password)
        {
            try
            {
                string uri = "ftp://" + ftpHostIP + "/" + dirName;
                System.Net.FtpWebRequest ftp = GetRequest(uri, username, password);
                ftp.Method = WebRequestMethods.Ftp.MakeDirectory;

                FtpWebResponse response = (FtpWebResponse)ftp.GetResponse();
                response.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 删除目录
        /// </summary>
        /// <param name="dirName">创建的目录名称</param>
        /// <param name="ftpHostIP">ftp地址</param>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        public void delDir(string dirName, string ftpHostIP, string username, string password)
        {
            try
            {
                string uri = "ftp://" + ftpHostIP + "/" + dirName;
                System.Net.FtpWebRequest ftp = GetRequest(uri, username, password);
                ftp.Method = WebRequestMethods.Ftp.RemoveDirectory;
                FtpWebResponse response = (FtpWebResponse)ftp.GetResponse();
                response.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 文件重命名
        /// </summary>
        /// <param name="currentFilename">当前目录名称</param>
        /// <param name="newFilename">重命名目录名称</param>
        /// <param name="ftpServerIP">ftp地址</param>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        public void Rename(string currentFilename, string newFilename, string ftpServerIP, string username, string password)
        {
            try
            {

                FileInfo fileInf = new FileInfo(currentFilename);
                string uri = "ftp://" + ftpServerIP + "/" + fileInf.Name;
                System.Net.FtpWebRequest ftp = GetRequest(uri, username, password);
                ftp.Method = WebRequestMethods.Ftp.Rename;

                ftp.RenameTo = newFilename;
                FtpWebResponse response = (FtpWebResponse)ftp.GetResponse();

                response.Close();
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }
        private static FtpWebRequest GetRequest(string URI, string username, string password)
        {
            //根据服务器信息FtpWebRequest创建类的对象
            FtpWebRequest result = (FtpWebRequest)FtpWebRequest.Create(URI);
            //提供身份验证信息
            result.Credentials = new System.Net.NetworkCredential(username, password);
            //设置请求完成之后是否保持到FTP服务器的控制连接，默认值为true
            result.KeepAlive = false;
            return result;
        }





        #region 构造函数

        /// <summary>
        /// FTP帮助类
        /// </summary>
        /// <param name="ip">FTP的IP地址</param>
        /// <param name="port">FTP的端口号</param>
        /// <param name="user">FTP的用户名</param>
        /// <param name="pwd">FTP的密码</param>
        /// <param name="usePassived">被动模式(true)/主动模式(false)</param>
        public FtpHelper(string ip, int port, string user, string pwd, bool usePassived = true)
        {
            this.ip = ip;
            this.port = port;
            this.user = user;
            this.pwd = pwd;
            this.usePassived = usePassived;
        }

        /// <summary>
        /// FTP帮助类
        /// </summary>
        /// <param name="ip">FTP的IP地址</param>
        /// <param name="port">FTP的端口号</param>
        /// <param name="user">FTP的用户名</param>
        /// <param name="pwd">FTP的密码</param>
        public FtpHelper(string ip, int port, string user, string pwd)
        {
            this.ip = ip;
            this.port = port;
            this.user = user;
            this.pwd = pwd;
            this.usePassived = true;//默认使用被动模式
        }
        #endregion

        #region 共有属性
        private string ip;
        public string Ip
        {
            get { return this.ip; }
        }

        private int port;
        public int Port
        {
            get { return this.port; }
        }

        private string user;
        public string User
        {
            get { return this.user; }
        }

        private string pwd;
        public string Pwd
        {
            get { return this.pwd; }
        }

        private bool usePassived;
        public bool UserPassived
        {
            get { return this.usePassived; }
        }
        #endregion

        #region 公有方法
        /// <summary>
        /// 从ftp服务器上获取指定路径下的列表(包含文件夹)
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="returnResult"></param>
        /// <returns></returns>
        public string[] GetFiles(string dir, out string returnResult)
        {
            StringBuilder result = new StringBuilder();
            FtpWebRequest request;
            try
            {
                string uri = @"ftp://" + this.ip + ":" + this.port + "/" + dir;
                request = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
                request.UseBinary = true;
                request.Credentials = new NetworkCredential(this.user, this.pwd);//设置用户名和密码
                request.Method = WebRequestMethods.Ftp.ListDirectory;
                request.UseBinary = true;

                request.UsePassive = usePassived;

                WebResponse response = request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());

                string line = reader.ReadLine();
                while (line != null)
                {
                    result.Append(line);
                    result.Append("\n");
                    Console.WriteLine(line);
                    line = reader.ReadLine();
                }
                // to remove the trailing '\n'
                result.Remove(result.ToString().LastIndexOf('\n'), 1);
                reader.Close();
                response.Close();
                returnResult = "succeed";
                return result.ToString().Split('\n');
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 从ftp服务器上获得文件详细列表, 不包含文件夹
        /// </summary>
        /// <param name="RequedstPath">服务器下的相对路径</param>
        /// <returns></returns>
        public List<string> GetFileDetails(string RequedstPath, out string returnResult)
        {
            List<string> strs = new List<string>();
            try
            {
                string uri = @"ftp://" + this.ip + ":" + this.port + "/" + RequedstPath;   //目标路径 path为服务器地址
                FtpWebRequest reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
                // ftp用户名和密码
                reqFTP.Credentials = new NetworkCredential(this.user, this.pwd);
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

                reqFTP.UsePassive = usePassived;

                WebResponse response = reqFTP.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());//中文文件名

                string line = reader.ReadLine();
                while (line != null)
                {
                    if (!line.Contains("<DIR>"))
                    {
                        string msg = line.Substring(39).Trim();
                        strs.Add(msg);
                    }
                    line = reader.ReadLine();
                }
                reader.Close();
                response.Close();
                returnResult = "succeed";
                return strs;
            }
            catch (Exception ex)
            {
                Console.WriteLine("获取文件出错：" + ex.Message);
            }
            returnResult = "error";
            return strs;
        }

        /// <summary>
        /// 获取文件大小
        /// </summary>
        /// <param name="file">ip服务器下的相对路径</param>
        /// <returns>文件大小</returns>
        public int GetFileSize(string file)
        {
            StringBuilder result = new StringBuilder();
            FtpWebRequest request;
            try
            {
                string uri = @"ftp://" + this.ip + ":" + this.port + "/" + file;
                request = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
                request.UseBinary = true;
                request.Credentials = new NetworkCredential(this.user, this.pwd);//设置用户名和密码
                request.Method = WebRequestMethods.Ftp.GetFileSize;

                request.UsePassive = usePassived;

                int dataLength = (int)request.GetResponse().ContentLength;

                return dataLength;
            }
            catch (Exception ex)
            {
                Console.WriteLine("获取文件大小出错：" + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="filePath">原路径（绝对路径）包括文件名</param>
        /// <param name="objPath">目标文件夹：服务器下的相对路径</param>
        public bool FileUpLoad(string filePath, string objPath)
        {
            try
            {
                string url = @"ftp://" + this.ip + ":" + this.port + "/";
                if (objPath != "")
                    url += objPath + "/";
                try
                {

                    FtpWebRequest reqFTP = null;
                    //待上传的文件 （全路径）
                    try
                    {
                        FileInfo fileInfo = new FileInfo(filePath);
                        using (FileStream fs = fileInfo.OpenRead())
                        {
                            long length = fs.Length;
                            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(url + fileInfo.Name));

                            //设置连接到FTP的帐号密码
                            reqFTP.Credentials = new NetworkCredential(this.user, this.pwd);
                            //设置请求完成后是否保持连接
                            reqFTP.KeepAlive = false;
                            //指定执行命令
                            reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
                            //指定数据传输类型
                            reqFTP.UseBinary = true;

                            reqFTP.UsePassive = usePassived;

                            using (Stream stream = reqFTP.GetRequestStream())
                            {
                                //设置缓冲大小
                                int BufferLength = 5120;
                                byte[] b = new byte[BufferLength];
                                int i;
                                while ((i = fs.Read(b, 0, BufferLength)) > 0)
                                {
                                    stream.Write(b, 0, i);
                                }
                                Console.WriteLine("上传文件成功");
                                return true;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("上传文件失败错误为" + ex.Message);
                        return false;
                    }
                    finally
                    {

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("上传文件失败错误为" + ex.Message);
                    return false;
                }
                finally
                {

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("上传文件失败错误为" + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="localDir">本地路径:"D:\Orders\"</param>
        /// <param name="FtpDir">Ftp路径:"Orders\"</param>
        /// <param name="fileName">文件名:"XXX.txt"</param>
        public bool Download(string localDir, string FtpDir, string fileName)
        {
            FtpWebRequest reqFTP;
            try
            {
                FileStream outputStream = new FileStream(localDir + fileName, FileMode.Create);

                string path = @"ftp://" + this.ip + ":" + this.port + "/";
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(path + FtpDir + "/" + fileName));
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFTP.UseBinary = true;
                reqFTP.UsePassive = usePassived;
                reqFTP.Credentials = new NetworkCredential(this.user, this.pwd);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                long cl = response.ContentLength;
                int bufferSize = 2048;
                int readCount;
                byte[] buffer = new byte[bufferSize];

                readCount = ftpStream.Read(buffer, 0, bufferSize);
                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }

                ftpStream.Close();
                outputStream.Close();
                response.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="fileName">服务器下的相对路径 包括文件名</param>
        public bool DeleteFileName(string fileName)
        {
            try
            {
                string uri = @"ftp://" + this.ip + ":" + this.port + "/" + fileName;
                FtpWebRequest reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
                // 指定数据传输类型
                reqFTP.UseBinary = true;
                // ftp用户名和密码
                reqFTP.Credentials = new NetworkCredential(this.user, this.pwd);
                // 默认为true，连接不会被关闭
                // 在一个命令之后被执行
                reqFTP.KeepAlive = false;

                reqFTP.UsePassive = usePassived;

                // 指定执行什么命令
                reqFTP.Method = WebRequestMethods.Ftp.DeleteFile;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                response.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 新建目录 上一级必须先存在
        /// </summary>
        /// <param name="dirName">服务器下的相对路径</param>
        public bool MakeDir(string dirName)
        {
            try
            {
                string uri = @"ftp://" + this.ip + ":" + this.port + "/" + dirName;
                FtpWebRequest reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
                // 指定数据传输类型
                reqFTP.UseBinary = true;
                // ftp用户名和密码
                reqFTP.Credentials = new NetworkCredential(this.user, this.pwd);
                reqFTP.Method = WebRequestMethods.Ftp.MakeDirectory;

                reqFTP.UsePassive = usePassived;

                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                response.Close();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("创建目录出错：" + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 删除目录 上一级必须先存在
        /// </summary>
        /// <param name="dirName">服务器下的相对路径</param>
        public bool DelDir(string dirName)
        {
            try
            {
                string uri = @"ftp://" + this.ip + ":" + this.port + "/" + dirName;
                FtpWebRequest reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
                // ftp用户名和密码
                reqFTP.Credentials = new NetworkCredential(this.user, this.pwd);
                reqFTP.Method = WebRequestMethods.Ftp.RemoveDirectory;

                reqFTP.UsePassive = usePassived;

                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                response.Close();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("删除目录出错：" + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 移动文件
        /// </summary>
        /// <param name="currentFile">当前文件的全路径</param>
        /// <param name="newDir">新的文件夹路径(包含文件夹最后的“/”，如“/Inbound/Archive/”)</param>
        /// <returns></returns>
        public bool MoveFile(string currentFile, string newDir)
        {
            try
            {
                int randomNum = new System.Random(DateTime.Now.Millisecond).Next(0, 9999);
                //if (this.GetFileSize(newDir) > 0)
                if (this.GetFileSize(newDir + currentFile.Substring(currentFile.LastIndexOf('/') + 1, currentFile.Length - currentFile.LastIndexOf('/') - 1)) > 0)
                {
                    this.DeleteFileName(newDir + currentFile.Substring(currentFile.LastIndexOf('/') + 1, currentFile.Length - currentFile.LastIndexOf('/') - 1));
                    //this.Rename(currentFile, newDir + randomNum.ToString() + ".xml");
                }
                if (this.Rename(currentFile, newDir + currentFile.Substring(currentFile.LastIndexOf('/') + 1, currentFile.Length - currentFile.LastIndexOf('/') - 1)))
                //if (this.Rename(currentFile, newDir))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 文件重命名，包括ftp服务器内移动，但目标目录若已有同名文件，则会发生错误，原文件会被删除
        /// </summary>
        /// <param name="currentFileName"></param>
        /// <param name="newFileName"></param>
        /// <returns></returns>
        public bool Rename(string currentFileName, string newFileName)
        {
            try
            {
                string uri = @"ftp://" + this.ip + ":" + this.port + "/" + currentFileName;
                FtpWebRequest reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
                // ftp用户名和密码
                reqFTP.Credentials = new NetworkCredential(this.user, this.pwd);

                reqFTP.UsePassive = usePassived;
                reqFTP.Method = System.Net.WebRequestMethods.Ftp.Rename;
                reqFTP.RenameTo = newFileName;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();

                if (ftpStream != null) ftpStream.Close();

                bool success = response.StatusCode == FtpStatusCode.CommandOK || response.StatusCode == FtpStatusCode.FileActionOK;
                response.Close();
                reqFTP = null;
                return success;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        #endregion

        #region 静态方法
        /// <summary>
        /// 获取ftp上面的文件和文件夹, 推荐使用这个
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static string[] GetFiles(string dir, out string returnResult, string ftpIp, int ftpPort, string userName, string password, bool usePassived)
        {
            StringBuilder result = new StringBuilder();
            FtpWebRequest request;
            try
            {
                string uri = @"ftp://" + ftpIp + ":" + ftpPort + "/" + dir;
                request = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
                request.UseBinary = true;
                request.Credentials = new NetworkCredential(userName, password);//设置用户名和密码
                request.Method = WebRequestMethods.Ftp.ListDirectory;
                request.UsePassive = usePassived;

                WebResponse response = request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string line = reader.ReadLine();
                while (line != null)
                {
                    result.Append(line);
                    result.Append("\n");
                    Console.WriteLine(line);
                    line = reader.ReadLine();
                }
                if (result.ToString() != "")
                {
                    // to remove the trailing '\n'
                    result.Remove(result.ToString().LastIndexOf('\n'), 1);
                    returnResult = "succeed";
                }
                else
                {
                    returnResult = "empty";//空文件夹
                }

                reader.Close();
                response.Close();
                return result.ToString().Split('\n');
            }
            catch (Exception ex)
            {
                Console.WriteLine("获取ftp上面的文件和文件夹：" + ex.Message);
                returnResult = ex.Message.ToString();
                return null;
            }
        }

        /// <summary>
        /// 从ftp服务器上获得文件详细列表, 不包含文件夹
        /// </summary>
        /// <param name="RequedstPath">服务器下的相对路径</param>
        /// <returns></returns>
        public static List<string> GetFileDetails(string RequedstPath, string ftpIp, int ftpPort, string userName, string password, bool usePassived)
        {
            List<string> strs = new List<string>();
            try
            {
                string uri = @"ftp://" + ftpIp + ":" + ftpPort + "/" + RequedstPath;   //目标路径 path为服务器地址
                FtpWebRequest reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
                // ftp用户名和密码
                reqFTP.Credentials = new NetworkCredential(userName, password);
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

                reqFTP.UsePassive = usePassived;

                WebResponse response = reqFTP.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());//中文文件名

                string line = reader.ReadLine();
                while (line != null)
                {
                    if (!line.Contains("<DIR>"))
                    {
                        string msg = line.Substring(39).Trim();
                        strs.Add(msg);
                    }
                    line = reader.ReadLine();
                }
                reader.Close();
                response.Close();
                return strs;
            }
            catch (Exception ex)
            {
                Console.WriteLine("获取文件出错：" + ex.Message);
            }
            return strs;
        }

        /// <summary>
        /// 获取文件大小
        /// </summary>
        /// <param name="file">ip服务器下的相对路径</param>
        /// <returns>文件大小</returns>
        public static int GetFileSize(string file, string ftpIp, int ftpPort, string userName, string password, bool usePassived)
        {
            StringBuilder result = new StringBuilder();
            FtpWebRequest request;
            try
            {
                string uri = @"ftp://" + ftpIp + ":" + ftpPort + "/" + file;
                request = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
                request.UseBinary = true;
                request.Credentials = new NetworkCredential(userName, password);//设置用户名和密码
                request.Method = WebRequestMethods.Ftp.GetFileSize;

                request.UsePassive = usePassived;

                int dataLength = (int)request.GetResponse().ContentLength;

                return dataLength;
            }
            catch (Exception ex)
            {
                Console.WriteLine("获取文件大小出错：" + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="filePath">原路径（绝对路径）包括文件名</param>
        /// <param name="objPath">目标文件夹：服务器下的相对路径</param>
        public static bool FileUpLoad(string filePath, string objPath, string ftpIp, int ftpPort, string userName, string password, bool usePassived)
        {
            try
            {
                string url = @"ftp://" + ftpIp + ":" + ftpPort + "/";
                if (objPath != "")
                    url += objPath + "/";
                try
                {

                    FtpWebRequest reqFTP = null;
                    //待上传的文件 （全路径）
                    try
                    {
                        FileInfo fileInfo = new FileInfo(filePath);
                        using (FileStream fs = fileInfo.OpenRead())
                        {
                            long length = fs.Length;
                            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(url + fileInfo.Name));

                            //设置连接到FTP的帐号密码
                            reqFTP.Credentials = new NetworkCredential(userName, password);
                            //设置请求完成后是否保持连接
                            reqFTP.KeepAlive = false;
                            //指定执行命令
                            reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
                            //指定数据传输类型
                            reqFTP.UseBinary = true;

                            reqFTP.UsePassive = usePassived;

                            using (Stream stream = reqFTP.GetRequestStream())
                            {
                                //设置缓冲大小
                                int BufferLength = 5120;
                                byte[] b = new byte[BufferLength];
                                int i;
                                while ((i = fs.Read(b, 0, BufferLength)) > 0)
                                {
                                    stream.Write(b, 0, i);
                                }
                                Console.WriteLine("上传文件成功");
                                return true;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("上传文件失败错误为" + ex.Message);
                        return false;
                    }
                    finally
                    {

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("上传文件失败错误为" + ex.Message);
                    return false;
                }
                finally
                {

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("上传文件失败错误为" + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="localDir">本地路径:"D:\Orders\"</param>
        /// <param name="FtpDir">Ftp路径:"Orders\"</param>
        /// <param name="fileName">文件名:"XXX.txt"</param>
        public static bool Download(string localDir, string FtpDir, string fileName, string ftpIp, int ftpPort, string userName, string password, bool usePassived)
        {
            FtpWebRequest reqFTP;
            try
            {
                FileStream outputStream = new FileStream(localDir + fileName, FileMode.Create);

                string path = @"ftp://" + ftpIp + ":" + ftpPort + "/";
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(path + FtpDir + "/" + fileName));
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFTP.UseBinary = true;
                reqFTP.UsePassive = usePassived;
                reqFTP.Credentials = new NetworkCredential(userName, password);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                long cl = response.ContentLength;
                int bufferSize = 2048;
                int readCount;
                byte[] buffer = new byte[bufferSize];

                readCount = ftpStream.Read(buffer, 0, bufferSize);
                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }

                ftpStream.Close();
                outputStream.Close();
                response.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="fileName">服务器下的相对路径 包括文件名</param>
        public static bool DeleteFileName(string fileName, string ftpIp, int ftpPort, string userName, string password, bool usePassived)
        {
            try
            {
                string uri = @"ftp://" + ftpIp + ":" + ftpPort + "/" + fileName;
                FtpWebRequest reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
                // 指定数据传输类型
                reqFTP.UseBinary = true;
                // ftp用户名和密码
                reqFTP.Credentials = new NetworkCredential(userName, password);
                // 默认为true，连接不会被关闭
                // 在一个命令之后被执行
                reqFTP.KeepAlive = false;

                reqFTP.UsePassive = usePassived;

                // 指定执行什么命令
                reqFTP.Method = WebRequestMethods.Ftp.DeleteFile;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                response.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 新建目录 上一级必须先存在
        /// </summary>
        /// <param name="dirName">服务器下的相对路径</param>
        public static bool MakeDir(string dirName, string ftpIp, int ftpPort, string userName, string password, bool usePassived)
        {
            try
            {
                string uri = @"ftp://" + ftpIp + ":" + ftpPort + "/" + dirName;
                FtpWebRequest reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
                // 指定数据传输类型
                reqFTP.UseBinary = true;
                // ftp用户名和密码
                reqFTP.Credentials = new NetworkCredential(userName, password);
                reqFTP.Method = WebRequestMethods.Ftp.MakeDirectory;

                reqFTP.UsePassive = usePassived;

                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                response.Close();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("创建目录出错：" + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 删除目录 上一级必须先存在
        /// </summary>
        /// <param name="dirName">服务器下的相对路径</param>
        public static bool DelDir(string dirName, string ftpIp, int ftpPort, string userName, string password, bool usePassived)
        {
            try
            {
                string uri = @"ftp://" + ftpIp + ":" + ftpPort + "/" + dirName;
                FtpWebRequest reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
                // ftp用户名和密码
                reqFTP.Credentials = new NetworkCredential(userName, password);
                reqFTP.Method = WebRequestMethods.Ftp.RemoveDirectory;

                reqFTP.UsePassive = usePassived;

                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                response.Close();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("删除目录出错：" + ex.Message);
                return false;
            }
        }


        /// <summary>
        /// 文件重命名,包括ftp服务器内移动，但目标目录若已有同名文件，则会发生错误，原文件会被删除
        /// 524         /// </summary>
        /// <param name="currentFilename">当前目录[不含IP]</param>
        /// <param name="newFilename">新目录[不含IP]</param>
        public static bool Rename(string currentFilename, string newFilename, string ftpIp, int ftpPort, string userName, string password, bool usePassived)
        {
            try
            {
                FileInfo fileInf = new FileInfo(currentFilename);
                string uri = @"ftp://" + ftpIp + ":" + ftpPort + "/" + currentFilename;
                FtpWebRequest reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
                // ftp用户名和密码
                reqFTP.Credentials = new NetworkCredential(userName, password);

                reqFTP.UsePassive = usePassived;
                reqFTP.Method = System.Net.WebRequestMethods.Ftp.Rename;
                reqFTP.RenameTo = newFilename;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();

                if (ftpStream != null) ftpStream.Close();

                bool success = response.StatusCode == FtpStatusCode.CommandOK || response.StatusCode == FtpStatusCode.FileActionOK;
                response.Close();
                reqFTP = null;
                return success;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;

            }

        }


        /// <summary>
        /// 移动文件
        /// </summary>
        /// <param name="currentFilename"></param>
        /// <param name="newFilename"></param>
        public static bool MovieFile(string currentFilename, string newDirectory, string ftpIp, int ftpPort, string userName, string password, bool usePassived)
        {

            try
            {
                if (FtpHelper.Rename(currentFilename, newDirectory, ftpIp, ftpPort, userName, password, usePassived))
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("移动目录出错：" + ex.Message);
                return false;
            }
        }



        /// <summary>
        /// 获取文件的创建时间
        /// </summary>
        /// <param name="ftpServerIP"></param>
        /// <param name="ftpFolder"></param>
        /// <param name="ftpUserID"></param>
        /// <param name="ftpPwd"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static DateTime GetFileModifyDateTime(string ftpServerIP, int ftpPort, string ftpFolder, string ftpUserID, string ftpPwd, string fileName, bool usePassived)
        {
            FtpWebRequest reqFTP = null;
            try
            {
                if (ftpFolder != "")
                {
                    ftpFolder = ftpFolder.Replace("/", "").Replace("\\", "");
                }
                string ftpPath = "ftp://" + ftpServerIP + ":" + ftpPort + "/" + ftpFolder + "/" + fileName;

                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpPath));
                reqFTP.UseBinary = true;
                //reqFTP.UsePassive = usePassived;
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPwd);
                reqFTP.Method = WebRequestMethods.Ftp.GetDateTimestamp;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                DateTime dt = response.LastModified;

                response.Close();
                response = null;
                reqFTP = null;
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
