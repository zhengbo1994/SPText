using Renci.SshNet;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Tamir.SharpSsh.jsch;
using WinSCP;

namespace SPText.Common
{
    public interface ISFTPHelper
    {
        bool Connect();
        void Disconnect();
        bool Put(string localPath, string remoteDir);
        bool Get(string remotePath, string localDir);
        bool Delete(string remoteFile);
        ArrayList GetFileList(string fTP_ORDER_PATH, string fileType);
        bool Rename(string oldPath, string newPath);
    }


    /// <summary>
    /// 通过用户名和密码的方式连接SFTP
    /// </summary>
    public partial class SFtpHelper : ISFTPHelper
    {
        private static Tamir.SharpSsh.jsch.Session m_session;
        private static Channel m_channel;
        private static ChannelSftp m_sftp;

        //host:sftp地址   user：用户名   pwd：密码    
        /// <summary>
        /// SFtpHelper
        /// </summary>
        /// <param name="host">主机</param>
        /// <param name="port">端口</param>
        /// <param name="user">用户名</param>
        /// <param name="pwd">密码</param>
        public SFtpHelper(string host, int port, string user, string pwd)
        {
            string[] arr = host.Split(':');
            string ip = arr[0];
            //int port = 1822;
            if (arr.Length > 1) port = Int32.Parse(arr[1]);

            JSch jsch = new JSch();
            m_session = jsch.getSession(user, ip, port);
            MyUserInfo ui = new MyUserInfo();
            ui.setPassword(pwd);
            m_session.setUserInfo(ui);

        }

        //SFTP连接状态        
        public bool Connected { get { return m_session.isConnected(); } }

        //连接SFTP        
        public bool Connect()
        {
            try
            {
                if (!Connected)
                {
                    m_session.connect();
                    m_channel = m_session.openChannel("sftp");
                    m_channel.connect();
                    m_sftp = (ChannelSftp)m_channel;
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        //断开SFTP        
        public void Disconnect()
        {
            if (Connected)
            {
                m_channel.disconnect();
                m_session.disconnect();
            }
        }

        /// <summary>
        /// SFTP存放文件
        /// </summary>
        /// <param name="localPath">本地路径</param>
        /// <param name="remotePath">远程路径</param>
        /// <returns></returns>
        public bool Put(string localPath, string remotePath)
        {
            try
            {
                Tamir.SharpSsh.java.String src = new Tamir.SharpSsh.java.String(localPath);
                Tamir.SharpSsh.java.String dst = new Tamir.SharpSsh.java.String(remotePath);
                m_sftp.put(src, dst);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// SFTP获取文件 
        /// </summary>
        /// <param name="localPath">本地路径</param>
        /// <param name="remotePath">远程路径</param>
        /// <returns></returns>
        public bool Get(string remotePath, string localPath)
        {
            try
            {
                Tamir.SharpSsh.java.String src = new Tamir.SharpSsh.java.String(remotePath);
                Tamir.SharpSsh.java.String dst = new Tamir.SharpSsh.java.String(localPath);
                m_sftp.get(src, dst);
                return true;
            }
            catch
            {
                return false;
            }
        }
        //删除SFTP文件
        public bool Delete(string remoteFile)
        {
            try
            {
                m_sftp.rm(remoteFile);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public string CheckConnectyAndGetFileList()
        {
            try
            {
                //首先连接
                if (!Connected)
                {
                    m_session.connect();
                    m_channel = m_session.openChannel("sftp");
                    m_channel.connect();
                    m_sftp = (ChannelSftp)m_channel;
                }
                //其次获取根目录
                Tamir.SharpSsh.java.util.Vector vvv = m_sftp.ls("/");
                ArrayList objList = new ArrayList();
                foreach (Tamir.SharpSsh.jsch.ChannelSftp.LsEntry qqq in vvv)
                {
                    objList.Add(qqq.getFilename());
                }
                this.Disconnect();
                return "Succeed";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        //获取SFTP文件列表
        public ArrayList GetFileList(string remotePath)
        {
            try
            {
                Tamir.SharpSsh.java.util.Vector vvv = m_sftp.ls(remotePath);
                ArrayList objList = new ArrayList();
                foreach (Tamir.SharpSsh.jsch.ChannelSftp.LsEntry qqq in vvv)
                {
                    objList.Add(qqq.getFilename());
                }

                return objList;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 获取SFTP文件列表
        /// </summary>
        /// <param name="remotePath">远程地址</param>
        /// <param name="fileType">文件类型</param>
        /// <returns></returns>
        public ArrayList GetFileList(string remotePath, string fileType)
        {
            try
            {
                Tamir.SharpSsh.java.util.Vector vvv = m_sftp.ls(remotePath);
                ArrayList objList = new ArrayList();
                foreach (Tamir.SharpSsh.jsch.ChannelSftp.LsEntry qqq in vvv)
                {
                    string sss = qqq.getFilename();
                    if (sss.Length > (fileType.Length + 1) && fileType == sss.Substring(sss.Length - fileType.Length))
                    { objList.Add(sss); }
                    else { continue; }
                }

                return objList;
            }
            catch
            {
                return null;
            }
        }

        public bool JudgeDirExit(string remotePath, string proName, string dirName)
        {
            Tamir.SharpSsh.java.util.Vector vvv = m_sftp.ls(remotePath + proName);
            foreach (Tamir.SharpSsh.jsch.ChannelSftp.LsEntry fileName in vvv)
            {
                string name = fileName.getFilename();
                if (name == dirName)
                {
                    return true;

                }

            }
            return false;

        }
        public bool Rename(string oldPath, string newPath)
        {
            try
            {
                m_sftp.rename(oldPath, newPath);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        //登录验证信息        
        public class MyUserInfo : UserInfo
        {
            String passwd;
            public String getPassword() { return passwd; }
            public void setPassword(String passwd) { this.passwd = passwd; }

            public String getPassphrase() { return null; }
            public bool promptPassphrase(String message) { return true; }

            public bool promptPassword(String message) { return true; }
            public bool promptYesNo(String message) { return true; }
            public void showMessage(String message) { }
        }
    }

    /// <summary>
    /// 通过秘钥的方式进行连接SFTP
    /// </summary>
    public partial class SftpWinScpHelper : ISFTPHelper
    {
        private SessionOptions _sessionOptions;
        private WinSCP.Session _session;

        public SftpWinScpHelper(string host, int port, string user)
        {
            _sessionOptions = new SessionOptions
            {
                Protocol = Protocol.Sftp,
                HostName = host,
                UserName = user,
                PortNumber = port
            };
            _session = new WinSCP.Session();
        }

        public SftpWinScpHelper(string host, int port, string user, string pwd, string key)
        {
            _sessionOptions = new SessionOptions
            {
                Protocol = Protocol.Sftp,
                HostName = host,
                PortNumber = port,
                UserName = user,
                Password = pwd,
                SshHostKeyFingerprint = key
            };
            _session = new WinSCP.Session();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="host">主机</param>
        /// <param name="port">端口</param>
        /// <param name="user">用户名</param>
        /// <param name="sshHostKeyFingerprint">ssh主机密钥指纹</param>
        /// <param name="sshPrivateKeyPath">ssh私钥路径</param>
        /// <param name="sshPrivateKeyPassphrase">ssh私钥密码短语</param>
        public SftpWinScpHelper(string host, int port, string user, string sshHostKeyFingerprint, string sshPrivateKeyPath, string privateKeyPassphrase)
        {
            _sessionOptions = new SessionOptions
            {
                Protocol = Protocol.Sftp,
                HostName = host,
                PortNumber = port,
                UserName = user,
                SshHostKeyFingerprint = sshHostKeyFingerprint,
                SshPrivateKeyPath = sshPrivateKeyPath,
                PrivateKeyPassphrase = privateKeyPassphrase
            };
            _session = new WinSCP.Session();
        }

        public bool Connected { get { return _session.Opened; } }

        public bool Connect()
        {
            try
            {
                if (!Connected)
                {
                    _session.Open(_sessionOptions);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void Disconnect()
        {
            if (Connected)
            {
                _session.Close();
            }
        }

        ~SftpWinScpHelper()
        {
            if (Connected)
            {
                _session.Close();
            }

            _session.Dispose();
            _session = null;
            _sessionOptions = null;
        }

        public bool Put(string localPath, string remoteDir)
        {
            try
            {
                //TransferOperationResult transferOperationResult = _session.PutFiles(localPath, remoteDir, false);
                //return transferOperationResult.IsSuccess;


                var args = _session.PutFileToDirectory(localPath, remoteDir, false);
                return args.Error == null;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Get(string remotePath, string localDir)
        {
            try
            {
                var result = _session.GetFiles(remotePath, localDir, false);
                return result.IsSuccess;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Delete(string remoteFile)
        {
            try
            {
                var result = _session.RemoveFile(remoteFile);
                return result.Error is null;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public ArrayList GetFileList(string fTP_ORDER_PATH, string fileType)
        {
            try
            {
                var list = _session.ListDirectory(fTP_ORDER_PATH);
                ArrayList objList = new ArrayList();
                foreach (var fileInfo in list.Files.ToList())
                {
                    if (Path.GetExtension(fileInfo.Name) == fileType)
                    {
                        objList.Add(fileInfo.Name);
                    }
                }
                return objList;
            }
            catch
            {
                return null;
            }
        }

        public bool Rename(string oldPath, string newPath)
        {
            try
            {
                _session.MoveFile(oldPath, newPath);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }

    public class SFtpRSAHelper : ISFTPHelper
    {
        SftpClient sftp = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="host">sftp服务器名或IP</param>
        /// <param name="port">端口，默认22</param>
        /// <param name="user">用户名</param>
        /// <param name="privateKey">私钥</param>
        /// <param name="passPhrase">通行短语</param>
        public SFtpRSAHelper(string host, int? port, string user, string privateKey, string passPhrase)
        {
            PrivateKeyFile keyFile = null;

            if (string.IsNullOrEmpty(passPhrase))
            {
                keyFile = new PrivateKeyFile(privateKey);
            }
            else
            {
                keyFile = new PrivateKeyFile(privateKey, passPhrase);
            }

            if (port.HasValue)
            {
                sftp = new SftpClient(host, port.Value, user, keyFile);
            }
            else
            {
                sftp = new SftpClient(host, user, keyFile);
            }


            if (sftp != null)
            {
                sftp.ConnectionInfo.RetryAttempts = 5;
                sftp.ConnectionInfo.Timeout = new TimeSpan(0, 3, 0);
            }
        }

        public SFtpRSAHelper(string ip, string port, string user, string pwd)
        {
            sftp = new SftpClient(ip, Int32.Parse(port), user, pwd);
        }

        public bool Connect()
        {
            if (sftp == null)
            {
                return false;
            }

            if (sftp.IsConnected)
            {
                return true;
            }

            try
            {
                sftp.Connect();
                return true;
            }
            catch (Exception ex)
            {
                string server = string.Format("{0}:{1}", sftp.ConnectionInfo.Username, sftp.ConnectionInfo.Host);
                // 我用的是nLog来记录错误日志。
                // logger.Error("[{0}] SFTP连接发生错误。", server, ex);
                return false;
            }
        }

        public void DisConnect()
        {
            if (sftp == null)
            {
                return;
            }
            if (!sftp.IsConnected)
            {
                return;
            }

            try
            {
                sftp.Disconnect();
                sftp.Dispose();
                sftp = null;
            }
            catch (Exception ex)
            {
                //logger.Error("SFTP断开连接发生错误。", ex);
            }
        }

        /// <summary>
        /// 取得文件列表
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns></returns>
        public List<string> ListFiles(string path)
        {

            if (!Connect())
            {
                return null;
            }

            List<string> files = new List<string>();
            try
            {
                sftp.ChangeDirectory("/");
                sftp.ListDirectory(path).ToList().ForEach(f =>
                {

                    files.Add(f.FullName);
                });

                return files;
            }
            catch (Exception ex)
            {
                // logger.Error("[{0}]　取得文件列表发生错误。", Path, ex);
                return null;
            }
        }

        /// <summary>
        /// 下载文件 
        /// </summary>
        /// <param name="remoteFileName">包含全路径的服务器端文件名</param>
        /// <param name="localFileName">本地保存的文件名</param>
        /// <returns></returns>
        public bool Download(string remoteFileName, string localFileName)
        {
            if (!Connect())
            {
                return false;
            }

            try
            {
                sftp.ChangeDirectory("/");
                FileStream fs = File.OpenWrite(localFileName);
                sftp.DownloadFile(remoteFileName, fs);
                fs.Close();
                return true;
            }
            catch (Exception ex)
            {
                //logger.Error("[{0}]　文件下载发生错误。", remoteFileName, ex);
                return false;
            }
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="localFileName">待上传的文件</param>
        /// <param name="remoteFileName">服务器端文件名</param>
        /// <returns></returns>
        public bool Upload(string localFileName, string remoteFileName)
        {
            if (!Connect())
            {
                return false;
            }

            try
            {
                sftp.ChangeDirectory("/");

                FileStream fs = File.OpenRead(localFileName);
                sftp.UploadFile(fs, remoteFileName, true);
                fs.Close();
                Thread.Sleep(1000);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 文件改名
        /// </summary>
        /// <param name="localFileName">包含全路径的源文件名</param>
        /// <param name="remoteFileName">包含全路径的新文件名</param>
        /// <returns></returns>
        public bool Rename(string orgFileName, string newFileName)
        {
            if (!Connect())
            {
                return false;
            }

            try
            {
                sftp.ChangeDirectory("/");

                sftp.RenameFile(orgFileName, newFileName);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="orgFileName"></param>
        /// <param name="newFileName"></param>
        /// <returns></returns>
        public bool Delete(string fileName)
        {
            if (!Connect())
            {
                return false;
            }

            try
            {
                sftp.ChangeDirectory("/");

                sftp.DeleteFile(fileName);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        #region 断开SFTP
        /// <summary>
        /// 断开SFTP
        /// </summary> 
        public void Disconnect()
        {
            try
            {
                if (sftp != null)
                {
                    sftp.Disconnect();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("断开SFTP失败，原因：{0}", ex.Message));
            }
        }

        #endregion


        #region SFTP上传文件
        /// <summary>
        /// SFTP上传文件
        /// </summary>
        /// <param name="localPath">本地路径</param>
        /// <param name="remotePath">远程路径</param>
        public bool Put(string localPath, string remotePath)
        {
            try
            {
                using (var file = File.OpenRead(localPath))
                {
                    Connect();
                    sftp.UploadFile(file, remotePath);
                    Disconnect();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("SFTP文件上传失败，原因：{0}", ex.Message));
            }
        }

        #endregion


        #region SFTP获取文件
        /// <summary>
        /// SFTP获取文件
        /// </summary>
        /// <param name="remotePath">远程路径</param>
        /// <param name="localPath">本地路径</param>
        public bool Get(string remotePath, string localPath)
        {
            try
            {
                Connect();
                var byt = sftp.ReadAllBytes(remotePath);
                Disconnect();
                File.WriteAllBytes(localPath, byt);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("SFTP文件获取失败，原因：{0}", ex.Message));
            }
        }
        #endregion


        #region 获取SFTP文件列表
        /// <summary>
        /// 获取SFTP文件列表
        /// </summary>
        /// <param name="remotePath">远程目录</param>
        /// <param name="fileSuffix">文件后缀</param>
        /// <returns></returns>
        public ArrayList GetFileList(string remotePath, string fileSuffix)
        {
            try
            {
                Connect();
                var files = sftp.ListDirectory(remotePath);
                Disconnect();
                var objList = new ArrayList();
                foreach (var file in files)
                {
                    string name = file.Name;
                    if (name.Length > (fileSuffix.Length + 1) && fileSuffix == name.Substring(name.Length - fileSuffix.Length))
                    {
                        objList.Add(name);
                    }
                }
                return objList;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("SFTP文件列表获取失败，原因：{0}", ex.Message));
            }
        }

        #endregion


        #region 移动SFTP文件
        /// <summary>
        /// 移动SFTP文件
        /// </summary>
        /// <param name="oldRemotePath">旧远程路径</param>
        /// <param name="newRemotePath">新远程路径</param>
        public void Move(string oldRemotePath, string newRemotePath)
        {
            try
            {
                Connect();
                sftp.RenameFile(oldRemotePath, newRemotePath);
                Disconnect();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("SFTP文件移动失败，原因：{0}", ex.Message));
            }
        }

        #endregion

    }
}

