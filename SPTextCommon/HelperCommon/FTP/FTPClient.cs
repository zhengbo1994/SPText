using System;
using System.Net;
using System.IO;
using System.Text;
using System.Net.Sockets;
using System.Threading;

namespace SPTextCommon.HelperCommon
{
    /// <summary>
    /// Ftp�ͻ��������
    /// </summary>
    public class FTPClient
    {
        public static object obj = new object();

        #region ���캯��
        /// <summary>
        /// ȱʡ���캯��
        /// </summary>
        public FTPClient()
        {
            strRemoteHost = "";
            strRemotePath = "";
            strRemoteUser = "";
            strRemotePass = "";
            strRemotePort = 21;
            bConnected = false;
        }

        /// <summary>
        /// ���캯��
        /// </summary>
        public FTPClient(string remoteHost, string remotePath, string remoteUser, string remotePass, int remotePort)
        {
            strRemoteHost = remoteHost;
            strRemotePath = remotePath;
            strRemoteUser = remoteUser;
            strRemotePass = remotePass;
            strRemotePort = remotePort;
            Connect();
        }
        #endregion

        #region �ֶ�
        private int strRemotePort;
        private Boolean bConnected;
        private string strRemoteHost;
        private string strRemotePass;
        private string strRemoteUser;
        private string strRemotePath;

        /// <summary>
        /// ���������ص�Ӧ����Ϣ(����Ӧ����)
        /// </summary>
        private string strMsg;
        /// <summary>
        /// ���������ص�Ӧ����Ϣ(����Ӧ����)
        /// </summary>
        private string strReply;
        /// <summary>
        /// ���������ص�Ӧ����
        /// </summary>
        private int iReplyCode;
        /// <summary>
        /// ���п������ӵ�socket
        /// </summary>
        private Socket socketControl;
        /// <summary>
        /// ����ģʽ
        /// </summary>
        private TransferType trType;
        /// <summary>
        /// ���պͷ������ݵĻ�����
        /// </summary>
        private static int BLOCK_SIZE = 512;
        /// <summary>
        /// ���뷽ʽ
        /// </summary>
        Encoding ASCII = Encoding.ASCII;
        /// <summary>
        /// �ֽ�����
        /// </summary>
        Byte[] buffer = new Byte[BLOCK_SIZE];
        #endregion

        #region ����
        /// <summary>
        /// FTP������IP��ַ
        /// </summary>
        public string RemoteHost
        {
            get
            {
                return strRemoteHost;
            }
            set
            {
                strRemoteHost = value;
            }
        }

        /// <summary>
        /// FTP�������˿�
        /// </summary>
        public int RemotePort
        {
            get
            {
                return strRemotePort;
            }
            set
            {
                strRemotePort = value;
            }
        }

        /// <summary>
        /// ��ǰ������Ŀ¼
        /// </summary>
        public string RemotePath
        {
            get
            {
                return strRemotePath;
            }
            set
            {
                strRemotePath = value;
            }
        }

        /// <summary>
        /// ��¼�û��˺�
        /// </summary>
        public string RemoteUser
        {
            set
            {
                strRemoteUser = value;
            }
        }

        /// <summary>
        /// �û���¼����
        /// </summary>
        public string RemotePass
        {
            set
            {
                strRemotePass = value;
            }
        }

        /// <summary>
        /// �Ƿ��¼
        /// </summary>
        public bool Connected
        {
            get
            {
                return bConnected;
            }
        }
        #endregion

        #region ����
        /// <summary>
        /// �������� 
        /// </summary>
        public void Connect()
        {
            lock (obj)
            {
                socketControl = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint ep = new IPEndPoint(IPAddress.Parse(RemoteHost), strRemotePort);
                try
                {
                    socketControl.Connect(ep);
                }
                catch (Exception)
                {
                    throw new IOException("��������ftp������");
                }
            }
            ReadReply();
            if (iReplyCode != 220)
            {
                DisConnect();
                throw new IOException(strReply.Substring(4));
            }
            SendCommand("USER " + strRemoteUser);
            if (!(iReplyCode == 331 || iReplyCode == 230))
            {
                CloseSocketConnect();
                throw new IOException(strReply.Substring(4));
            }
            if (iReplyCode != 230)
            {
                SendCommand("PASS " + strRemotePass);
                if (!(iReplyCode == 230 || iReplyCode == 202))
                {
                    CloseSocketConnect();
                    throw new IOException(strReply.Substring(4));
                }
            }
            bConnected = true;
            ChDir(strRemotePath);
        }

        /// <summary>
        /// �ر�����
        /// </summary>
        public void DisConnect()
        {
            if (socketControl != null)
            {
                SendCommand("QUIT");
            }
            CloseSocketConnect();
        }
        #endregion

        #region ����ģʽ
        /// <summary>
        /// ����ģʽ:���������͡�ASCII����
        /// </summary>
        public enum TransferType { Binary, ASCII };

        /// <summary>
        /// ���ô���ģʽ
        /// </summary>
        /// <param name="ttType">����ģʽ</param>
        public void SetTransferType(TransferType ttType)
        {
            if (ttType == TransferType.Binary)
            {
                SendCommand("TYPE I");//binary���ʹ���
            }
            else
            {
                SendCommand("TYPE A");//ASCII���ʹ���
            }
            if (iReplyCode != 200)
            {
                throw new IOException(strReply.Substring(4));
            }
            else
            {
                trType = ttType;
            }
        }

        /// <summary>
        /// ��ô���ģʽ
        /// </summary>
        /// <returns>����ģʽ</returns>
        public TransferType GetTransferType()
        {
            return trType;
        }
        #endregion

        #region �ļ�����
        /// <summary>
        /// ����ļ��б�
        /// </summary>
        /// <param name="strMask">�ļ�����ƥ���ַ���</param>
        public string[] Dir(string strMask)
        {
            if (!bConnected)
            {
                Connect();
            }
            Socket socketData = CreateDataSocket();
            SendCommand("NLST " + strMask);
            if (!(iReplyCode == 150 || iReplyCode == 125 || iReplyCode == 226))
            {
                throw new IOException(strReply.Substring(4));
            }
            strMsg = "";
            Thread.Sleep(2000);
            while (true)
            {
                int iBytes = socketData.Receive(buffer, buffer.Length, 0);
                strMsg += ASCII.GetString(buffer, 0, iBytes);
                if (iBytes < buffer.Length)
                {
                    break;
                }
            }
            char[] seperator = { '\n' };
            string[] strsFileList = strMsg.Split(seperator);
            socketData.Close(); //����socket�ر�ʱҲ���з�����
            if (iReplyCode != 226)
            {
                ReadReply();
                if (iReplyCode != 226)
                {

                    throw new IOException(strReply.Substring(4));
                }
            }
            return strsFileList;
        }

        public void newPutByGuid(string strFileName, string strGuid)
        {
            if (!bConnected)
            {
                Connect();
            }
            string str = strFileName.Substring(0, strFileName.LastIndexOf("\\"));
            string strTypeName = strFileName.Substring(strFileName.LastIndexOf("."));
            strGuid = str + "\\" + strGuid;
            Socket socketData = CreateDataSocket();
            SendCommand("STOR " + Path.GetFileName(strGuid));
            if (!(iReplyCode == 125 || iReplyCode == 150))
            {
                throw new IOException(strReply.Substring(4));
            }
            FileStream input = new FileStream(strGuid, FileMode.Open);
            input.Flush();
            int iBytes = 0;
            while ((iBytes = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                socketData.Send(buffer, iBytes, 0);
            }
            input.Close();
            if (socketData.Connected)
            {
                socketData.Close();
            }
            if (!(iReplyCode == 226 || iReplyCode == 250))
            {
                ReadReply();
                if (!(iReplyCode == 226 || iReplyCode == 250))
                {
                    throw new IOException(strReply.Substring(4));
                }
            }
        }

        /// <summary>
        /// ��ȡ�ļ���С
        /// </summary>
        /// <param name="strFileName">�ļ���</param>
        /// <returns>�ļ���С</returns>
        public long GetFileSize(string strFileName)
        {
            if (!bConnected)
            {
                Connect();
            }
            SendCommand("SIZE " + Path.GetFileName(strFileName));
            long lSize = 0;
            if (iReplyCode == 213)
            {
                lSize = Int64.Parse(strReply.Substring(4));
            }
            else
            {
                throw new IOException(strReply.Substring(4));
            }
            return lSize;
        }


        /// <summary>
        /// ��ȡ�ļ���Ϣ
        /// </summary>
        /// <param name="strFileName">�ļ���</param>
        /// <returns>�ļ���С</returns>
        public string GetFileInfo(string strFileName)
        {
            if (!bConnected)
            {
                Connect();
            }
            Socket socketData = CreateDataSocket();
            SendCommand("LIST " + strFileName);
            string strResult = "";
            if (!(iReplyCode == 150 || iReplyCode == 125
                || iReplyCode == 226 || iReplyCode == 250))
            {
                throw new IOException(strReply.Substring(4));
            }
            byte[] b = new byte[512];
            MemoryStream ms = new MemoryStream();

            while (true)
            {
                int iBytes = socketData.Receive(b, b.Length, 0);
                ms.Write(b, 0, iBytes);
                if (iBytes <= 0)
                {

                    break;
                }
            }
            byte[] bt = ms.GetBuffer();
            strResult = System.Text.Encoding.ASCII.GetString(bt);
            ms.Close();
            return strResult;
        }

        /// <summary>
        /// ɾ��
        /// </summary>
        /// <param name="strFileName">��ɾ���ļ���</param>
        public void Delete(string strFileName)
        {
            if (!bConnected)
            {
                Connect();
            }
            SendCommand("DELE " + strFileName);
            if (iReplyCode != 250)
            {
                throw new IOException(strReply.Substring(4));
            }
        }

        /// <summary>
        /// ������(������ļ����������ļ�����,�����������ļ�)
        /// </summary>
        /// <param name="strOldFileName">���ļ���</param>
        /// <param name="strNewFileName">���ļ���</param>
        public void Rename(string strOldFileName, string strNewFileName)
        {
            if (!bConnected)
            {
                Connect();
            }
            SendCommand("RNFR " + strOldFileName);
            if (iReplyCode != 350)
            {
                throw new IOException(strReply.Substring(4));
            }
            //  ������ļ�����ԭ���ļ�����,������ԭ���ļ�
            SendCommand("RNTO " + strNewFileName);
            if (iReplyCode != 250)
            {
                throw new IOException(strReply.Substring(4));
            }
        }
        #endregion

        #region �ϴ�������
        /// <summary>
        /// ����һ���ļ�
        /// </summary>
        /// <param name="strFileNameMask">�ļ�����ƥ���ַ���</param>
        /// <param name="strFolder">����Ŀ¼(������\����)</param>
        public void Get(string strFileNameMask, string strFolder)
        {
            if (!bConnected)
            {
                Connect();
            }
            string[] strFiles = Dir(strFileNameMask);
            foreach (string strFile in strFiles)
            {
                if (!strFile.Equals(""))//һ����˵strFiles�����һ��Ԫ�ؿ����ǿ��ַ���
                {
                    Get(strFile, strFolder, strFile);
                }
            }
        }

        /// <summary>
        /// ����һ���ļ�
        /// </summary>
        /// <param name="strRemoteFileName">Ҫ���ص��ļ���</param>
        /// <param name="strFolder">����Ŀ¼(������\����)</param>
        /// <param name="strLocalFileName">�����ڱ���ʱ���ļ���</param>
        public void Get(string strRemoteFileName, string strFolder, string strLocalFileName)
        {
            Socket socketData = CreateDataSocket();
            try
            {
                if (!bConnected)
                {
                    Connect();
                }
                SetTransferType(TransferType.Binary);
                if (strLocalFileName.Equals(""))
                {
                    strLocalFileName = strRemoteFileName;
                }
                SendCommand("RETR " + strRemoteFileName);
                if (!(iReplyCode == 150 || iReplyCode == 125 || iReplyCode == 226 || iReplyCode == 250))
                {
                    throw new IOException(strReply.Substring(4));
                }
                FileStream output = new FileStream(strFolder + "\\" + strLocalFileName, FileMode.Create);
                while (true)
                {
                    int iBytes = socketData.Receive(buffer, buffer.Length, 0);
                    output.Write(buffer, 0, iBytes);
                    if (iBytes <= 0)
                    {
                        break;
                    }
                }
                output.Close();
                if (socketData.Connected)
                {
                    socketData.Close();
                }
                if (!(iReplyCode == 226 || iReplyCode == 250))
                {
                    ReadReply();
                    if (!(iReplyCode == 226 || iReplyCode == 250))
                    {
                        throw new IOException(strReply.Substring(4));
                    }
                }
            }
            catch
            {
                socketData.Close();
                socketData = null;
                socketControl.Close();
                bConnected = false;
                socketControl = null;
            }
        }

        /// <summary>
        /// ����һ���ļ�
        /// </summary>
        /// <param name="strRemoteFileName">Ҫ���ص��ļ���</param>
        /// <param name="strFolder">����Ŀ¼(������\����)</param>
        /// <param name="strLocalFileName">�����ڱ���ʱ���ļ���</param>
        public void GetNoBinary(string strRemoteFileName, string strFolder, string strLocalFileName)
        {
            if (!bConnected)
            {
                Connect();
            }

            if (strLocalFileName.Equals(""))
            {
                strLocalFileName = strRemoteFileName;
            }
            Socket socketData = CreateDataSocket();
            SendCommand("RETR " + strRemoteFileName);
            if (!(iReplyCode == 150 || iReplyCode == 125 || iReplyCode == 226 || iReplyCode == 250))
            {
                throw new IOException(strReply.Substring(4));
            }
            FileStream output = new FileStream(strFolder + "\\" + strLocalFileName, FileMode.Create);
            while (true)
            {
                int iBytes = socketData.Receive(buffer, buffer.Length, 0);
                output.Write(buffer, 0, iBytes);
                if (iBytes <= 0)
                {
                    break;
                }
            }
            output.Close();
            if (socketData.Connected)
            {
                socketData.Close();
            }
            if (!(iReplyCode == 226 || iReplyCode == 250))
            {
                ReadReply();
                if (!(iReplyCode == 226 || iReplyCode == 250))
                {
                    throw new IOException(strReply.Substring(4));
                }
            }
        }

        /// <summary>
        /// �ϴ�һ���ļ�
        /// </summary>
        /// <param name="strFolder">����Ŀ¼(������\����)</param>
        /// <param name="strFileNameMask">�ļ���ƥ���ַ�(���԰���*��?)</param>
        public void Put(string strFolder, string strFileNameMask)
        {
            string[] strFiles = Directory.GetFiles(strFolder, strFileNameMask);
            foreach (string strFile in strFiles)
            {
                Put(strFile);
            }
        }

        /// <summary>
        /// �ϴ�һ���ļ�
        /// </summary>
        /// <param name="strFileName">�����ļ���</param>
        public void Put(string strFileName)
        {
            if (!bConnected)
            {
                Connect();
            }
            Socket socketData = CreateDataSocket();
            if (Path.GetExtension(strFileName) == "")
                SendCommand("STOR " + Path.GetFileNameWithoutExtension(strFileName));
            else
                SendCommand("STOR " + Path.GetFileName(strFileName));

            if (!(iReplyCode == 125 || iReplyCode == 150))
            {
                throw new IOException(strReply.Substring(4));
            }

            FileStream input = new FileStream(strFileName, FileMode.Open);
            int iBytes = 0;
            while ((iBytes = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                socketData.Send(buffer, iBytes, 0);
            }
            input.Close();
            if (socketData.Connected)
            {
                socketData.Close();
            }
            if (!(iReplyCode == 226 || iReplyCode == 250))
            {
                ReadReply();
                if (!(iReplyCode == 226 || iReplyCode == 250))
                {
                    throw new IOException(strReply.Substring(4));
                }
            }
        }


        /// <summary>
        /// �ϴ�һ���ļ�
        /// </summary>
        /// <param name="strFileName">�����ļ���</param>
        public void PutByGuid(string strFileName, string strGuid)
        {
            if (!bConnected)
            {
                Connect();
            }
            string str = strFileName.Substring(0, strFileName.LastIndexOf("\\"));
            string strTypeName = strFileName.Substring(strFileName.LastIndexOf("."));
            strGuid = str + "\\" + strGuid;
            System.IO.File.Copy(strFileName, strGuid);
            System.IO.File.SetAttributes(strGuid, System.IO.FileAttributes.Normal);
            Socket socketData = CreateDataSocket();
            SendCommand("STOR " + Path.GetFileName(strGuid));
            if (!(iReplyCode == 125 || iReplyCode == 150))
            {
                throw new IOException(strReply.Substring(4));
            }
            FileStream input = new FileStream(strGuid, FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);
            int iBytes = 0;
            while ((iBytes = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                socketData.Send(buffer, iBytes, 0);
            }
            input.Close();
            File.Delete(strGuid);
            if (socketData.Connected)
            {
                socketData.Close();
            }
            if (!(iReplyCode == 226 || iReplyCode == 250))
            {
                ReadReply();
                if (!(iReplyCode == 226 || iReplyCode == 250))
                {
                    throw new IOException(strReply.Substring(4));
                }
            }
        }
        #endregion

        #region Ŀ¼����
        /// <summary>
        /// ����Ŀ¼
        /// </summary>
        /// <param name="strDirName">Ŀ¼��</param>
        public void MkDir(string strDirName)
        {
            if (!bConnected)
            {
                Connect();
            }
            SendCommand("MKD " + strDirName);
            if (iReplyCode != 257)
            {
                throw new IOException(strReply.Substring(4));
            }
        }

        /// <summary>
        /// ɾ��Ŀ¼
        /// </summary>
        /// <param name="strDirName">Ŀ¼��</param>
        public void RmDir(string strDirName)
        {
            if (!bConnected)
            {
                Connect();
            }
            SendCommand("RMD " + strDirName);
            if (iReplyCode != 250)
            {
                throw new IOException(strReply.Substring(4));
            }
        }

        /// <summary>
        /// �ı�Ŀ¼
        /// </summary>
        /// <param name="strDirName">�µĹ���Ŀ¼��</param>
        public void ChDir(string strDirName)
        {
            if (strDirName.Equals(".") || strDirName.Equals(""))
            {
                return;
            }
            if (!bConnected)
            {
                Connect();
            }
            SendCommand("CWD " + strDirName);
            if (iReplyCode != 250)
            {
                throw new IOException(strReply.Substring(4));
            }
            this.strRemotePath = strDirName;
        }
        #endregion

        #region �ڲ�����
        /// <summary>
        /// ��һ��Ӧ���ַ�����¼��strReply��strMsg,Ӧ�����¼��iReplyCode
        /// </summary>
        private void ReadReply()
        {
            strMsg = "";
            strReply = ReadLine();
            iReplyCode = Int32.Parse(strReply.Substring(0, 3));
        }

        /// <summary>
        /// ���������������ӵ�socket
        /// </summary>
        /// <returns>��������socket</returns>
        private Socket CreateDataSocket()
        {
            SendCommand("PASV");
            if (iReplyCode != 227)
            {
                throw new IOException(strReply.Substring(4));
            }
            int index1 = strReply.IndexOf('(');
            int index2 = strReply.IndexOf(')');
            string ipData = strReply.Substring(index1 + 1, index2 - index1 - 1);
            int[] parts = new int[6];
            int len = ipData.Length;
            int partCount = 0;
            string buf = "";
            for (int i = 0; i < len && partCount <= 6; i++)
            {
                char ch = Char.Parse(ipData.Substring(i, 1));
                if (Char.IsDigit(ch))
                    buf += ch;
                else if (ch != ',')
                {
                    throw new IOException("Malformed PASV strReply: " + strReply);
                }
                if (ch == ',' || i + 1 == len)
                {
                    try
                    {
                        parts[partCount++] = Int32.Parse(buf);
                        buf = "";
                    }
                    catch (Exception)
                    {
                        throw new IOException("Malformed PASV strReply: " + strReply);
                    }
                }
            }
            string ipAddress = parts[0] + "." + parts[1] + "." + parts[2] + "." + parts[3];
            int port = (parts[4] << 8) + parts[5];
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ipAddress), port);
            try
            {
                s.Connect(ep);
            }
            catch (Exception)
            {
                throw new IOException("�޷�����ftp������");
            }
            return s;
        }

        /// <summary>
        /// �ر�socket����(���ڵ�¼��ǰ)
        /// </summary>
        private void CloseSocketConnect()
        {
            lock (obj)
            {
                if (socketControl != null)
                {
                    socketControl.Close();
                    socketControl = null;
                }
                bConnected = false;
            }
        }

        /// <summary>
        /// ��ȡSocket���ص������ַ���
        /// </summary>
        /// <returns>����Ӧ������ַ�����</returns>
        private string ReadLine()
        {
            lock (obj)
            {
                while (true)
                {
                    int iBytes = socketControl.Receive(buffer, buffer.Length, 0);
                    strMsg += ASCII.GetString(buffer, 0, iBytes);
                    if (iBytes < buffer.Length)
                    {
                        break;
                    }
                }
            }
            char[] seperator = { '\n' };
            string[] mess = strMsg.Split(seperator);
            if (strMsg.Length > 2)
            {
                strMsg = mess[mess.Length - 2];
            }
            else
            {
                strMsg = mess[0];
            }
            if (!strMsg.Substring(3, 1).Equals(" ")) //�����ַ�����ȷ������Ӧ����(��220��ͷ,�����һ�ո�,�ٽ��ʺ��ַ���)
            {
                return ReadLine();
            }
            return strMsg;
        }

        /// <summary>
        /// ���������ȡӦ��������һ��Ӧ���ַ���
        /// </summary>
        /// <param name="strCommand">����</param>
        public void SendCommand(String strCommand)
        {
            lock (obj)
            {
                Byte[] cmdBytes = Encoding.ASCII.GetBytes((strCommand + "\r\n").ToCharArray());
                socketControl.Send(cmdBytes, cmdBytes.Length, 0);
                Thread.Sleep(500);
                ReadReply();
            }
        }
        #endregion
    }
}