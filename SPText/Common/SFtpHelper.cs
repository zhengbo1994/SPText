﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tamir.SharpSsh.jsch;

namespace SPText.Common
{
    public partial class SFtpHelper
    {
        private static Session m_session;
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
            catch
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
        public static bool Put(string localPath, string remotePath)
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
}
