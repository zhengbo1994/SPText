using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SPTextWinForm
{
    public partial class EmailSend : Form
    {
        string strSendEmailTO = ConfigurationManager.AppSettings["SendEmailTO"].ToString();
        string strSendEmailCC = ConfigurationManager.AppSettings["SendEmailCC"].ToString();
        public EmailSend()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 发送邮件按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSend_Click(object sender, EventArgs e)
        {
            string title = this.txtTitle.Text;
            string mailContent = this.txtContent.Text;
            List<string> strSendEmailTOList = strSendEmailTO.Replace("；", ";").Split(';').ToList();
            List<string> strSendEmailCCList = strSendEmailCC.Replace("；", ";").Split(';').ToList();
            List<string> emailTO = strSendEmailTOList;
            List<string> emailCC = strSendEmailCCList;
            this.SendEmail(title, mailContent, emailTO, emailCC);
        }




        /// <summary>
        /// 发送邮件（没有附件）
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="mailContent">内容</param>
        /// <returns></returns>
        public bool SendEmail(string title, string mailContent, List<string> emailTO, List<string> emailCC)
        {
            return this.SendEmailFJ(title, mailContent, string.Empty, emailTO, emailCC);
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="mailContent">内容</param>
        /// <param name="path">附件路径</param>
        /// <param name="emailTO">收件人</param>
        /// <param name="emailCC">抄送</param>
        /// <returns></returns>
        public bool SendEmailFJ(string title, string mailContent, string path, List<string> emailTO, List<string> emailCC)
        {
            MailMessage msg = new MailMessage();
            foreach (var to in emailTO)
            {
                if (!string.IsNullOrEmpty(to))
                {
                    int IndexOfTo = to.IndexOf(";");
                    if (IndexOfTo > 0)
                    {
                        msg.To.Add(to.Substring(0, IndexOfTo));
                    }
                    else
                    {
                        msg.To.Add(to.Substring(0));
                    }

                }
            }
            foreach (var cc in emailCC)
            {
                if (!string.IsNullOrEmpty(cc))
                {
                    int IndexOfTo = cc.IndexOf(";");
                    if (IndexOfTo > 0)
                    {

                        msg.CC.Add(cc.Substring(0, IndexOfTo));
                    }
                    else
                    {
                        msg.CC.Add(cc.Substring(0));
                    }

                }
            }
            if (path != string.Empty)
            {
                //添加附件
                System.Net.Mail.Attachment myAttachment = new System.Net.Mail.Attachment(path, System.Net.Mime.MediaTypeNames.Application.Octet);
                //MIME协议下的一个对象，用以设置附件的创建时间，修改时间以及读取时间
                System.Net.Mime.ContentDisposition disposition = myAttachment.ContentDisposition;
                //用smtpclient对象里attachments属性，添加上面设置好的myattachment
                msg.Attachments.Add(myAttachment);
            }


            msg.From = new MailAddress("no_reply@hkoptlens.com", "HK-IT", System.Text.Encoding.UTF8);
            msg.Subject = title;//邮件标题 
            msg.SubjectEncoding = System.Text.Encoding.UTF8;//邮件标题编码
            msg.Body = mailContent;//邮件内容          
            msg.BodyEncoding = System.Text.Encoding.UTF8;//邮件内容编码     
            msg.Priority = MailPriority.Normal;//邮件优先级  
            msg.IsBodyHtml = true;//是否是HTML邮件     

            SmtpClient client = new SmtpClient();
            client.UseDefaultCredentials = true;
            // client.Credentials = new System.Net.NetworkCredential("it-service@hkolens.com", "IT2016@"); //注册的邮箱和密码   
            client.Credentials = new System.Net.NetworkCredential("no_reply@hkoptlens.com", "a@T1sG5xa977"); //注册的邮箱和密码   
            //client.Host = "smtp.qiye.163.com";
            client.Host = "mail.hkoptlens.com";
            // client.Port = 25;
            client.Port = 6025;
            //client.EnableSsl = true;
            client.EnableSsl = false;


            object userState = msg;

            try
            {
                client.Send(msg);
                if (path != string.Empty)
                {
                    foreach (Attachment item in msg.Attachments)
                    {
                        item.Dispose();   //一定要释放该对象,否则无法删除附件
                    }
                    File.Delete(path);
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="emailAcount">发件人邮箱地址</param>
        /// <param name="emailPassword">发件人邮箱密码</param>
        /// <param name="emailRecipientList">收件人列表</param>
        /// <param name="ccRecipientList">抄送列表</param>
        /// <param name="title">邮件标题</param>
        /// <param name="content">邮件内容</param>
        /// <param name="attachmentPathList">邮件附件列表</param>
        /// <param name="smtpHost">服务器主机地址</param>
        /// <param name="smtpPort">服务器端口号</param>
        /// <returns></returns>
        public static bool SendMail(string emailAcount, string emailPassword, List<string> emailRecipientList, List<string> ccRecipientList, string title, string content, List<string> attachmentPathList, string smtpHost, int? smtpPort, bool EnableSsl = true)
        {
            MailMessage message = new MailMessage();
            MailAddress fromAddr = new MailAddress(emailAcount);
            message.From = fromAddr;
            foreach (string emailRecipient in emailRecipientList)
            {
                if (!string.IsNullOrEmpty(emailRecipient))
                {
                    message.To.Add(emailRecipient.Trim());
                }
            }
            if (ccRecipientList != null && ccRecipientList.Count > 0)
            {
                foreach (string ccRecipient in ccRecipientList)
                {
                    if (!string.IsNullOrEmpty(ccRecipient))
                    {
                        message.CC.Add(ccRecipient.Trim());
                    }
                }
            }


            //设置邮件标题
            message.Subject = title;
            //设置邮件内容
            message.Body = content;
            message.IsBodyHtml = true;
            //设置邮件附件
            if (attachmentPathList != null && attachmentPathList.Count > 0)
            {
                foreach (var item in attachmentPathList)
                {
                    string SUpFile = item;
                    if (SUpFile != null && SUpFile != "")
                    {
                        //将文件进行转换成Attachments
                        Attachment data = new Attachment(SUpFile, System.Net.Mime.MediaTypeNames.Application.Octet);
                        // Add time stamp information for the file.
                        System.Net.Mime.ContentDisposition disposition = data.ContentDisposition;
                        //disposition.CreationDate = System.IO.File.GetCreationTime(SUpFile);
                        //disposition.ModificationDate = System.IO.File.GetLastWriteTime(SUpFile);
                        //disposition.ReadDate = System.IO.File.GetLastAccessTime(SUpFile);

                        message.Attachments.Add(data);
                        //System.Net.Mime.ContentType ctype = new System.Net.Mime.ContentType();
                    }
                }
            }

            //设置邮件发送服务器,服务器根据你使用的邮箱而不同,可以到相应的邮箱管理后台查看
            if (smtpHost == null || smtpHost.Trim() == "")
            {
                smtpHost = "smtp.qiye.163.com";
            }
            if (smtpPort == null || smtpPort == 0)
            {
                smtpPort = 25;
            }
            SmtpClient client = new SmtpClient(smtpHost, (int)smtpPort);
            //设置发送人的邮箱账号和密码
            client.Credentials = new System.Net.NetworkCredential(emailAcount, emailPassword);
            //启用ssl,也就是安全发送
            client.EnableSsl = EnableSsl;
            //发送邮件
            try
            {
                client.Send(message);
                foreach (Attachment item in message.Attachments)
                {
                    item.Dispose();
                }
                message = null;
                GC.Collect();
                return true;
            }
            catch (System.Net.Mail.SmtpException ex)
            {
                Console.WriteLine(ex.Message);
                message.Attachments.Clear();
                //MessageBox.Show(ex.ToString());
                message = null;
                GC.Collect();
                return false;
            }

        }
    }
}
