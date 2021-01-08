using System.Web;
using System.Configuration;

namespace SPTextCommon.HelperCommon
{
    /// <summary>
    /// ��Ƶ������
    /// </summary>
    public class VideoHelper : System.Web.UI.Page
    {
        #region ���캯��
        /// <summary>
        /// ���캯��
        /// </summary>
        public VideoHelper()
        { } 
        #endregion

        #region ˽�б���
        string[] strArrMencoder = new string[] { "wmv", "rmvb", "rm" };
        string[] strArrFfmpeg = new string[] { "asf", "avi", "mpg", "3gp", "mov" }; 
        #endregion

        #region ����
        public static string ffmpegtool = ConfigurationManager.AppSettings["ffmpeg"];
        public static string mencodertool = ConfigurationManager.AppSettings["mencoder"];
        public static string savefile = ConfigurationManager.AppSettings["savefile"] + "/";
        public static string sizeOfImg = ConfigurationManager.AppSettings["CatchFlvImgSize"];
        public static string widthOfFile = ConfigurationManager.AppSettings["widthSize"];
        public static string heightOfFile = ConfigurationManager.AppSettings["heightSize"];
        #endregion

        #region ��ȡ�ļ�������
        /// <summary>
        /// ��ȡ�ļ�������
        /// </summary>
        public static string GetFileName(string fileName)
        {
            int i = fileName.LastIndexOf("\\") + 1;
            string Name = fileName.Substring(i);
            return Name;
        }
        #endregion

        #region ��ȡ�ļ���չ��
        /// <summary>
        /// ��ȡ�ļ���չ��
        /// </summary>
        public static string GetExtension(string fileName)
        {
            int i = fileName.LastIndexOf(".") + 1;
            string Name = fileName.Substring(i);
            return Name;
        }
        #endregion

        #region ��ȡ�ļ�����
        /// <summary>
        /// ��ȡ�ļ�����
        /// </summary>
        public string CheckExtension(string extension)
        {
            string m_strReturn = "";
            foreach (string var in this.strArrFfmpeg)
            {
                if (var == extension)
                {
                    m_strReturn = "ffmpeg"; break;
                }
            }
            if (m_strReturn == "")
            {
                foreach (string var in strArrMencoder)
                {
                    if (var == extension)
                    {
                        m_strReturn = "mencoder"; break;
                    }
                }
            }
            return m_strReturn;
        }
        #endregion

        #region ��Ƶ��ʽתΪFlv
        /// <summary>
        /// ��Ƶ��ʽתΪFlv
        /// </summary>
        /// <param name="vFileName">ԭ��Ƶ�ļ���ַ</param>
        /// <param name="ExportName">���ɺ��Flv�ļ���ַ</param>
        public bool ConvertFlv(string vFileName, string ExportName)
        {
            if ((!System.IO.File.Exists(ffmpegtool)) || (!System.IO.File.Exists(HttpContext.Current.Server.MapPath(vFileName))))
            {
                return false;
            }
            vFileName = HttpContext.Current.Server.MapPath(vFileName);
            ExportName = HttpContext.Current.Server.MapPath(ExportName);
            string Command = " -i \"" + vFileName + "\" -y -ab 32 -ar 22050 -b 800000 -s  480*360 \"" + ExportName + "\""; //Flv��ʽ     
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = ffmpegtool;
            p.StartInfo.Arguments = Command;
            p.StartInfo.WorkingDirectory = HttpContext.Current.Server.MapPath("~/tools/");
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = false;
            p.Start();
            p.BeginErrorReadLine();
            p.WaitForExit();
            p.Close();
            p.Dispose();
            return true;
        }
        #endregion

        #region ����Flv��Ƶ������ͼ
        /// <summary>
        /// ����Flv��Ƶ������ͼ
        /// </summary>
        /// <param name="vFileName">��Ƶ�ļ���ַ</param>
        public string CatchImg(string vFileName)
        {
            if ((!System.IO.File.Exists(ffmpegtool)) || (!System.IO.File.Exists(HttpContext.Current.Server.MapPath(vFileName)))) return "";
            try
            {
                string flv_img_p = vFileName.Substring(0, vFileName.Length - 4) + ".jpg";
                string Command = " -i " + HttpContext.Current.Server.MapPath(vFileName) + " -y -f image2 -t 0.1 -s " + sizeOfImg + " " + HttpContext.Current.Server.MapPath(flv_img_p);
                System.Diagnostics.Process p = new System.Diagnostics.Process();
                p.StartInfo.FileName = ffmpegtool;
                p.StartInfo.Arguments = Command;
                p.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
                try
                {
                    p.Start();
                }
                catch
                {
                    return "";
                }
                finally
                {
                    p.Close();
                    p.Dispose();
                }
                System.Threading.Thread.Sleep(4000);

                //ע��:ͼƬ��ȡ�ɹ���,�������ڴ滺��д��������Ҫʱ��ϳ�,�����3,4����������;
                if (System.IO.File.Exists(HttpContext.Current.Server.MapPath(flv_img_p)))
                {
                    return flv_img_p;
                }
                return "";
            }
            catch
            {
                return "";
            }
        }
        #endregion

        #region ����FFMpeg����Ƶ����(����·��)
        /// <summary>
        /// ת���ļ���������ָ���ļ�����
        /// </summary>
        /// <param name="fileName">�ϴ���Ƶ�ļ���·����ԭ�ļ���</param>
        /// <param name="playFile">ת������ļ���·�������粥���ļ���</param>
        /// <param name="imgFile">����Ƶ�ļ���ץȡ��ͼƬ·��</param>
        /// <returns>�ɹ�:����ͼƬ�����ַ;ʧ��:���ؿ��ַ���</returns>
        public string ChangeFilePhy(string fileName, string playFile, string imgFile)
        {
            string ffmpeg = Server.MapPath(VideoHelper.ffmpegtool);
            if ((!System.IO.File.Exists(ffmpeg)) || (!System.IO.File.Exists(fileName)))
            {
                return "";
            }
            string flv_file = System.IO.Path.ChangeExtension(playFile, ".flv");
            string FlvImgSize = VideoHelper.sizeOfImg;
            System.Diagnostics.ProcessStartInfo FilestartInfo = new System.Diagnostics.ProcessStartInfo(ffmpeg);
            FilestartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            FilestartInfo.Arguments = " -i " + fileName + " -ab 56 -ar 22050 -b 500 -r 15 -s " + widthOfFile + "x" + heightOfFile + " " + flv_file;
            try
            {
                System.Diagnostics.Process.Start(FilestartInfo);//ת��
                CatchImg(fileName, imgFile); //��ͼ
            }
            catch
            {
                return "";
            }
            return "";
        }

        public string CatchImg(string fileName, string imgFile)
        {
            string ffmpeg = Server.MapPath(VideoHelper.ffmpegtool);
            string flv_img = imgFile + ".jpg";
            string FlvImgSize = VideoHelper.sizeOfImg;
            System.Diagnostics.ProcessStartInfo ImgstartInfo = new System.Diagnostics.ProcessStartInfo(ffmpeg);
            ImgstartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            ImgstartInfo.Arguments = "   -i   " + fileName + "  -y  -f  image2   -ss 2 -vframes 1  -s   " + FlvImgSize + "   " + flv_img;
            try
            {
                System.Diagnostics.Process.Start(ImgstartInfo);
            }
            catch
            {
                return "";
            }
            if (System.IO.File.Exists(flv_img))
            {
                return flv_img;
            }
            return "";
        }
        #endregion

        #region ����FFMpeg����Ƶ����(���·��)
        /// <summary>
        /// ת���ļ���������ָ���ļ�����
        /// </summary>
        /// <param name="fileName">�ϴ���Ƶ�ļ���·����ԭ�ļ���</param>
        /// <param name="playFile">ת������ļ���·�������粥���ļ���</param>
        /// <param name="imgFile">����Ƶ�ļ���ץȡ��ͼƬ·��</param>
        /// <returns>�ɹ�:����ͼƬ�����ַ;ʧ��:���ؿ��ַ���</returns>
        public string ChangeFileVir(string fileName, string playFile, string imgFile)
        {
            string ffmpeg = Server.MapPath(VideoHelper.ffmpegtool);
            if ((!System.IO.File.Exists(ffmpeg)) || (!System.IO.File.Exists(fileName)))
            {
                return "";
            }
            string flv_img = System.IO.Path.ChangeExtension(Server.MapPath(imgFile), ".jpg");
            string flv_file = System.IO.Path.ChangeExtension(Server.MapPath(playFile), ".flv");
            string FlvImgSize = VideoHelper.sizeOfImg;

            System.Diagnostics.ProcessStartInfo ImgstartInfo = new System.Diagnostics.ProcessStartInfo(ffmpeg);
            ImgstartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            ImgstartInfo.Arguments = "   -i   " + fileName + "   -y   -f   image2   -t   0.001   -s   " + FlvImgSize + "   " + flv_img;

            System.Diagnostics.ProcessStartInfo FilestartInfo = new System.Diagnostics.ProcessStartInfo(ffmpeg);
            FilestartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            FilestartInfo.Arguments = " -i " + fileName + " -ab 56 -ar 22050 -b 500 -r 15 -s " + widthOfFile + "x" + heightOfFile + " " + flv_file;
            try
            {
                System.Diagnostics.Process.Start(FilestartInfo);
                System.Diagnostics.Process.Start(ImgstartInfo);
            }
            catch
            {
                return "";
            }

            ///ע��:ͼƬ��ȡ�ɹ���,�������ڴ滺��д��������Ҫʱ��ϳ�,�����3,4����������;   
            ///�����Ҫ��ʱ���ټ��,�ҷ�������ʱ8��,���������8��ͼƬ�Բ�����,��Ϊ��ͼʧ��;    
            if (System.IO.File.Exists(flv_img))
            {
                return flv_img;
            }
            return "";
        }
        #endregion

        #region ����mencoder����Ƶ������ת��(����·��)
        /// <summary>
        /// ����mencoder����Ƶ������ת��
        /// </summary>
        public string MChangeFilePhy(string vFileName, string playFile, string imgFile)
        {
            string tool = Server.MapPath(VideoHelper.mencodertool);
            if ((!System.IO.File.Exists(tool)) || (!System.IO.File.Exists(vFileName)))
            {
                return "";
            }
            string flv_file = System.IO.Path.ChangeExtension(playFile, ".flv");
            string FlvImgSize = VideoHelper.sizeOfImg;
            System.Diagnostics.ProcessStartInfo FilestartInfo = new System.Diagnostics.ProcessStartInfo(tool);
            FilestartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            FilestartInfo.Arguments = " " + vFileName + " -o " + flv_file + " -of lavf -lavfopts i_certify_that_my_video_stream_does_not_use_b_frames -oac mp3lame -lameopts abr:br=56 -ovc lavc -lavcopts vcodec=flv:vbitrate=200:mbd=2:mv0:trell:v4mv:cbp:last_pred=1:dia=-1:cmp=0:vb_strategy=1 -vf scale=" + widthOfFile + ":" + heightOfFile + " -ofps 12 -srate 22050";
            try
            {
                System.Diagnostics.Process.Start(FilestartInfo);
                CatchImg(flv_file, imgFile);
            }
            catch
            {
                return "";
            }
            return "";
        }
        #endregion
    }
}