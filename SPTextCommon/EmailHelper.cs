using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace SPTextCommon
{
    /// <summary>
    /// EmailHelper 的摘要说明
    /// </summary>
    public class EmailHelper
    {

        public string EmailAccount { get; set; }
        public string EmailPassword { get; set; }
        public string SmtpHost { get; set; }
        public int? SmtpPort { get; set; }
        public bool EnableSsl { get; set; }

        public EmailHelper()
        {
            this.EmailAccount = "it-service@hkolens.com";
            this.EmailPassword = "IT2016@";
            string SmtpHost = System.Configuration.ConfigurationManager.AppSettings["SmtpHost"];
            int? SmtpPort = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SmtpPort"]);
            bool EnableSsl = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["EnableSsl"]);
        }

        public EmailHelper(string emailAccount, string emailPassword, string smtpHost, int? smtpPort, bool enableSsl)
        {
            this.EmailAccount = emailAccount;
            this.EmailPassword = emailPassword;
            this.SmtpHost = smtpHost;
            this.SmtpPort = smtpPort;
            this.EnableSsl = enableSsl;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="emailRecipientList">收件人</param>
        /// <param name="ccRecipientList">抄送人</param>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="attachmentPathList">邮件附件</param>
        /// <returns></returns>
        public bool SendMail(List<string> emailRecipientList, List<string> ccRecipientList, string title, string content, List<string> attachmentPathList)
        {
            MailMessage message = new MailMessage();
            MailAddress fromAddr = new MailAddress(this.EmailAccount);
            message.From = fromAddr;
            foreach (string emailRecipient in emailRecipientList)
            {
                message.To.Add(emailRecipient.Trim());
            }
            if (ccRecipientList != null && ccRecipientList.Count > 0)
            {
                foreach (string ccRecipient in ccRecipientList)
                {
                    message.CC.Add(ccRecipient.Trim());
                }
            }

            //设置邮件标题
            message.Subject = title;
            //设置邮箱内容
            message.Body = content;
            //html
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
                        Attachment data = new Attachment(SUpFile, MediaTypeNames.Application.Octet); ;
                        //Add time stamp information for the file.
                        ContentDisposition disposition = data.ContentDisposition;
                        disposition.CreationDate = System.IO.File.GetCreationTime(SUpFile);
                        disposition.ModificationDate = System.IO.File.GetLastWriteTime(SUpFile);
                        disposition.ReadDate = System.IO.File.GetLastAccessTime(SUpFile);

                        message.Attachments.Add(data);
                    }
                }
            }

            //设置发送服务器，服务器根据你使用的邮箱而不用，可以到相应的邮箱管理后台查看
            if (SmtpHost == null || SmtpHost.Trim() == "")
            {
                SmtpHost = "smtp.qiye.163.com";
            }
            if (SmtpPort == null || SmtpPort == 0)
            {
                SmtpPort = 587;
            }

            SmtpClient client = new SmtpClient(SmtpHost, (int)SmtpPort);
            //设置发送人的邮箱账户和密码
            client.Credentials = new NetworkCredential(EmailAccount, EmailPassword);
            //启动ssl,也就是安全发送
            client.EnableSsl = EnableSsl;

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
            catch (Exception ex)
            {
                message.Attachments.Clear();
                message = null;
                GC.Collect();
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        internal void SendMail(string title, string content, List<string> attachmentPathList)
        {

            string emailRecipient = System.Configuration.ConfigurationManager.AppSettings["EmailRecipientList"];
            List<string> emailRecipientList = emailRecipient.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList<string>();
            string ccRecipient = System.Configuration.ConfigurationManager.AppSettings["CcRecipientList"];
            List<string> ccRecipientList = ccRecipient.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList<string>();

            SendMail(emailRecipientList, ccRecipientList, title, content, attachmentPathList);
        }



        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="mailContent">内容</param>
        /// <param name="mailContent">路径</param>
        /// <returns></returns>
        public bool SendEmailff(string title, string content, List<string> attachmentPathList)
        {
            string emailRecipient = System.Configuration.ConfigurationManager.AppSettings["EmailRecipientList"];
            List<string> emailRecipientList = emailRecipient.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList<string>();
            string ccRecipient = System.Configuration.ConfigurationManager.AppSettings["CcRecipientList"];
            List<string> ccRecipientList = ccRecipient.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList<string>();


            MailMessage msg = new MailMessage();

            foreach (string e in emailRecipientList)
            {
                msg.To.Add(e.Trim());
            }
            if (ccRecipientList != null && ccRecipientList.Count > 0)
            {
                foreach (string c in ccRecipientList)
                {
                    msg.CC.Add(c.Trim());
                }
            }

            MailAddress fromAddr = new MailAddress(this.EmailAccount);
            msg.From = fromAddr;
            // msg.From = new MailAddress("it-service@hkolens.com", "邮件", System.Text.Encoding.UTF8);
            msg.Subject = title;//邮件标题 

            msg.SubjectEncoding = System.Text.Encoding.UTF8;//邮件标题编码
            msg.Body = content;//邮件内容          
            msg.BodyEncoding = System.Text.Encoding.UTF8;//邮件内容编码     
            msg.IsBodyHtml = false;//是否是HTML邮件          
            msg.Priority = MailPriority.Normal;//邮件优先级      


            if (attachmentPathList != null && attachmentPathList.Count > 0)
            {
                foreach (var item in attachmentPathList)
                {
                    string SUpFile = item;
                    if (SUpFile != null && SUpFile != "")
                    {
                        //将文件进行转换成Attachments
                        Attachment data = new Attachment(SUpFile, MediaTypeNames.Application.Octet); ;
                        //Add time stamp information for the file.
                        ContentDisposition disposition = data.ContentDisposition;
                        disposition.CreationDate = System.IO.File.GetCreationTime(SUpFile);
                        disposition.ModificationDate = System.IO.File.GetLastWriteTime(SUpFile);
                        disposition.ReadDate = System.IO.File.GetLastAccessTime(SUpFile);

                        msg.Attachments.Add(data);
                    }
                }
            }



            SmtpClient client = new SmtpClient();
            client.UseDefaultCredentials = true;
            // client.Credentials = new System.Net.NetworkCredential("it-service@hkolens.com", "IT2016@"); //注册的邮箱和密码 
            client.Credentials = new NetworkCredential(EmailAccount, EmailPassword);
            client.Host = "smtp.qiye.163.com";
            client.Port = 587;
            client.EnableSsl = true;
            object userState = msg;
            try
            {
                client.Send(msg);
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
        /// <param name="title">标题</param>
        /// <param name="mailContent">内容</param>
        /// <param name="mailContent">路径</param>
        /// <returns></returns>
        public bool SendEmailff(string title, string mailContent, string path)
        {
            string emailRecipient = System.Configuration.ConfigurationManager.AppSettings["EmailRecipientList"];
            List<string> emailRecipientList = emailRecipient.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList<string>();
            string ccRecipient = System.Configuration.ConfigurationManager.AppSettings["CcRecipientList"];
            List<string> ccRecipientList = ccRecipient.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList<string>();

            MailMessage msg = new MailMessage();

            foreach (string e in emailRecipientList)
            {
                msg.To.Add(e.Trim());
            }
            if (ccRecipientList != null && ccRecipientList.Count > 0)
            {
                foreach (string c in ccRecipientList)
                {
                    msg.CC.Add(c.Trim());
                }
            }

            if (path != string.Empty)
            {
                //添加附件
                System.Net.Mail.Attachment myAttachment = new System.Net.Mail.Attachment(
               path, System.Net.Mime.MediaTypeNames.Application.Octet);
                //MIME协议下的一个对象，用以设置附件的创建时间，修改时间以及读取时间
                System.Net.Mime.ContentDisposition disposition = myAttachment.ContentDisposition;
                //用smtpclient对象里attachments属性，添加上面设置好的myattachment
                msg.Attachments.Add(myAttachment);
            }

            MailAddress fromAddr = new MailAddress(this.EmailAccount);
            msg.From = fromAddr;
            // msg.From = new MailAddress("it-service@hkolens.com", "邮件", System.Text.Encoding.UTF8);
            msg.Subject = title;//邮件标题 

            msg.SubjectEncoding = System.Text.Encoding.UTF8;//邮件标题编码
            msg.Body = mailContent;//邮件内容          
            msg.BodyEncoding = System.Text.Encoding.UTF8;//邮件内容编码     
            msg.IsBodyHtml = false;//是否是HTML邮件          
            msg.Priority = MailPriority.Normal;//邮件优先级      
            SmtpClient client = new SmtpClient();
            client.UseDefaultCredentials = true;
            client.Credentials = new System.Net.NetworkCredential("it-service@hkolens.com", "IT2016@"); //注册的邮箱和密码   
            client.Host = "smtp.qiye.163.com";
            client.Port = 587;
            client.EnableSsl = true;

            object userState = msg;

            try
            {
                //client.SendAsync(msg, userState);
                client.Send(msg);
                // msg.Attachments.Dispose();
                // File.Delete(path);
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
                        Attachment data = new Attachment(SUpFile, MediaTypeNames.Application.Octet);
                        // Add time stamp information for the file.
                        ContentDisposition disposition = data.ContentDisposition;
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
            client.Credentials = new NetworkCredential(emailAcount, emailPassword);
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

        public static bool SendMail(string emailAcount, string emailPassword, string emailRecipient, string ccRecipient, string title, string content, string attachmentPath, string smtpHost, int? smtpPort)
        {

            return SendMail(emailAcount, emailPassword, new List<string>() { emailRecipient }, new List<string>() { ccRecipient }, title, content, new List<string>() { attachmentPath }, smtpHost, smtpPort);
        }

        public static bool SendMail(string emailAcount, string emailPassword, string emailRecipient, string ccRecipient, string title, string content, List<string> attachmentPathList, string smtpHost, int? smtpPort)
        {

            return SendMail(emailAcount, emailPassword, new List<string>() { emailRecipient }, new List<string>() { ccRecipient }, title, content, attachmentPathList, smtpHost, smtpPort);
        }

        public static bool SendMail(string emailAcount, string emailPassword, string emailRecipient, string title, string content, string attachmentPath, string smtpHost, int? smtpPort)
        {

            return SendMail(emailAcount, emailPassword, new List<string>() { emailRecipient }, null, title, content, new List<string>() { attachmentPath }, smtpHost, smtpPort);
        }

        public static bool SendMail(string emailAcount, string emailPassword, string emailRecipient, string title, string content, List<string> attachmentPathList, string smtpHost, int? smtpPort)
        {

            return SendMail(emailAcount, emailPassword, new List<string>() { emailRecipient }, null, title, content, attachmentPathList, smtpHost, smtpPort);
        }

        public static bool SendMail(string emailAcount, string emailPassword, string emailRecipient, string ccRecipient, string title, string content, string attachmentPath)
        {
            return SendMail(emailAcount, emailPassword, new List<string>() { emailRecipient }, new List<string>() { ccRecipient }, title, content, new List<string>() { emailRecipient }, null, null);
        }

        public static bool SendMail(string emailAcount, string emailPassword, string emailRecipient, string ccRecipient, string title, string content, List<string> attachmentPathList)
        {
            return SendMail(emailAcount, emailPassword, new List<string>() { emailRecipient }, new List<string>() { ccRecipient }, title, content, attachmentPathList, null, null);
        }

        public static bool SendMail(string emailAcount, string emailPassword, string emailRecipient, string title, string content, string attachmentPath)
        {
            return SendMail(emailAcount, emailPassword, new List<string>() { emailRecipient }, null, title, content, new List<string>() { emailRecipient }, null, null);
        }

        public static bool SendMail(string emailAcount, string emailPassword, string emailRecipient, string title, string content, List<string> attachmentPathList)
        {
            return SendMail(emailAcount, emailPassword, new List<string>() { emailRecipient }, null, title, content, attachmentPathList, null, null);
        }

        public static bool SendMail(string emailAcount, string emailPassword, string emailRecipient, string title, string content)
        {
            return SendMail(emailAcount, emailPassword, new List<string>() { emailRecipient }, null, title, content, null, null, null);
        }

        public static bool SendMail(string emailAcount, string emailPassword, List<string> emailRecipientList, string title, string content)
        {
            return SendMail(emailAcount, emailPassword, emailRecipientList, null, title, content, null, null, null);
        }
    }



    public class UowDBContext : DbContext
    {
        public DbContext _DbContext { get; set; }
        public UowDBContext() : base("name=DBContext")
        {
            CreateDbContext("DBContext");
        }

        public UowDBContext(string connection) : base(connection)
        {
            CreateDbContext(connection);
        }

        public void CreateDbContext(string connetion)
        {
            this._DbContext = new DbContext(connetion);
            this._DbContext.Configuration.ProxyCreationEnabled = false;
            this._DbContext.Configuration.ValidateOnSaveEnabled = false;
        }

        public IQueryable<T> GetAll<T>() where T : class
        {
            return _DbContext.Set<T>();
        }

        public T GetById<T>(int Id) where T : class
        {
            return _DbContext.Set<T>().Find(Id);
        }

        public T GetById<T>(params int[] Ids) where T : class
        {
            return _DbContext.Set<T>().Find(Ids);
        }

        public void Add<T>(T t) where T : class
        {
            var entry = _DbContext.Entry(t);
            if (entry.State == EntityState.Detached)
            {
                entry.State = EntityState.Added;
            }
            else
            {
                _DbContext.Set<T>().Attach(t);
                _DbContext.Set<T>().Add(t);
            }
        }

        public void Update<T>(T t) where T : class
        {
            var entry = _DbContext.Entry(t);
            _DbContext.Set<T>().Attach(t);
            entry.State = EntityState.Modified;
        }

        public void Delete<T>(T t) where T : class
        {
            var entry = _DbContext.Entry(t);
            if (entry.State == EntityState.Detached)
            {
                _DbContext.Set<T>().Attach(t);
                entry.State = EntityState.Deleted;
            }
            else
            {
                _DbContext.Set<T>().Attach(t);
                _DbContext.Set<T>().Remove(t);
            }
        }

        public int ExecuteSqlCommand<T>(string sql, params object[] parameters)
        {
            return _DbContext.Database.ExecuteSqlCommand(sql, parameters);
        }

        public IQueryable<T> SqlQuery<T>(string sql, params object[] parameters)
        {
            return _DbContext.Database.SqlQuery<T>(sql, parameters).AsQueryable();
        }

        public void Commit()
        {
            _DbContext.SaveChanges();
        }

        protected override void Dispose(bool disposing)
        {
            GC.SuppressFinalize(_DbContext);
            base.Dispose(disposing);
        }

        public DbSet<EmailHelper> emailHelpers { get; set; }
    }
}
