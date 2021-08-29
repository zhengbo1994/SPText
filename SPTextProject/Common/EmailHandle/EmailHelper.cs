using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;

namespace SPTextProject.Common.EmailHandle
{
    public class EmailHelper
    {
        //private static readonly SmtpClient smtp;
        //private static readonly string emailAccount;

        //static EmailHelper()
        //{
        //    emailAccount = ConfigManager.GetConfig("Email:EmailAccount");
        //    string emailPassword = ConfigManager.GetConfig("Email:EmailPassword");

        //    string smtpHost = ConfigManager.GetConfig("Email:SmtpHost");
        //    int smtpPort = Convert.ToInt32(ConfigManager.GetConfig("Email:SmtpPort"));

        //    bool EnableSsl = Convert.ToBoolean(ConfigManager.GetConfig("Email:EnableSsl"));

        //    smtp = new SmtpClient
        //    {
        //        Host = smtpHost,
        //        UseDefaultCredentials = false,
        //        DeliveryMethod = SmtpDeliveryMethod.Network,
        //        Port = smtpPort,
        //        Credentials = new NetworkCredential(emailAccount, emailPassword)
        //    };
        //}

        ///// <summary>
        ///// 发送邮件
        ///// </summary>
        ///// <param name="title">标题</param>
        ///// <param name="mailContent">内容</param>
        ///// <param name="mailContent">路径</param>
        ///// <returns></returns>
        //public static bool SendEmail(string[] emailRecipientList, string[] ccRecipientList, string title, string content, CustomAttachment attachment = null)
        //{
        //    //设置默认发送信息
        //    Email.DefaultSender = new SmtpSender(smtp);

        //    var messageTo = new List<Address>();
        //    var messageCC = new List<Address>();

        //    foreach (string emailRecipient in emailRecipientList)
        //    {
        //        if (!emailRecipient.IsNullOrEmpty())
        //        {
        //            messageTo.Add(new Address(emailRecipient.Trim()));
        //        }
        //    }
        //    if (ccRecipientList != null && ccRecipientList.Length > 0)
        //    {
        //        foreach (string ccRecipient in ccRecipientList)
        //        {
        //            if (!string.IsNullOrEmpty(ccRecipient))
        //            {
        //                messageCC.Add(new Address(ccRecipient.Trim()));
        //            }
        //        }
        //    }

        //    var email = Email
        //        //发送人
        //        .From(emailAccount)
        //        //收件人
        //        .To(messageTo)
        //        //邮件标题
        //        .Subject(title)
        //        //邮件内容
        //        .Body(content, true);

        //    //附件（如果有的话）
        //    email = attachment == null ? email : email.Attach(new FluentEmail.Core.Models.Attachment { Data = attachment.Data, ContentType = attachment.ContentType, Filename = attachment.Filename });

        //    if (messageCC.Count > 0)
        //        email.CC(messageCC);

        //    //依据发送结果判断是否发送成功
        //    try
        //    {
        //        var result = email.Send();

        //        //或使用异步的方式发送
        //        //await email.SendAsync();

        //        if (result.Successful)
        //        {
        //            //发送成功逻辑
        //            return true;
        //        }
        //        else
        //        {
        //            //发送失败可以通过result.ErrorMessages查看失败原因
        //            return false;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}


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
                message.Dispose();
                message = null;
                GC.Collect();
                return true;
            }
            catch (System.Net.Mail.SmtpException ex)
            {
                message.Attachments.Clear();
                message.Dispose();
                message = null;
                GC.Collect();
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
        public static bool SendMail(string displayName, string emailAcount, string emailPassword, List<string> emailRecipientList, List<string> ccRecipientList, string title, string content, List<string> attachmentPathList, string smtpHost, int? smtpPort, bool EnableSsl = true)
        {
            MailMessage message = new MailMessage();
            MailAddress fromAddr = new MailAddress(emailAcount, displayName);
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
}
