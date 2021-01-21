using SPTextCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SPTextWinForm.Public
{
    public class InitialConfiguration
    {
        #region  加载数据库和发送邮件配置信息
        /// <summary>
        /// 装载数据库信息
        /// </summary>
        public DatabaseSetSettingInfo GetDatabaseSetSettingInfo(string settingPath)
        {
            string str = "/set/customers/customer[@customerName='NA319']";
            string dataSource = XmlHelper.Read(settingPath, str + "/databaseSet/dataSource");
            string database = XmlHelper.Read(settingPath, str + "/databaseSet/database");
            string user = XmlHelper.Read(settingPath, str + "/databaseSet/user");
            string pwd = XmlHelper.Read(settingPath, str + "/databaseSet/pwd");

            DatabaseSetSettingInfo databaseSetSettingInfo = new DatabaseSetSettingInfo()
            {
                dbHost = dataSource,
                dbName = database,
                dbUser = user,
                dbPwd = pwd
            };
            return databaseSetSettingInfo;
        }

        /// <summary>
        /// 装载邮件信息
        /// </summary>
        public SendEmailSettingInfo GetSendEmailSettingInfo(string settingPath)
        {
            string str = "/set/customers/customer[@customerName='NA319']";
            string smtpHost = XmlHelper.Read(settingPath, str + "/sendEmailSets/sendEmailSet/smtpHost");
            string smtpPort = XmlHelper.Read(settingPath, str + "/sendEmailSets/sendEmailSet/smtpPort");
            string emailAccount = XmlHelper.Read(settingPath, str + "/sendEmailSets/sendEmailSet/emailAccount");
            string emailPassword = XmlHelper.Read(settingPath, str + "/sendEmailSets/sendEmailSet/emailPassword");
            string emailRecipients = XmlHelper.Read(settingPath, str + "/sendEmailSets/sendEmailSet/emailRecipients");
            string ccRecipients = XmlHelper.Read(settingPath, str + "/sendEmailSets/sendEmailSet/ccRecipients");
            string lastEmailTime = XmlHelper.Read(settingPath, str + "/sendEmailSets/sendEmailSet/lastEmailTime");
            string sendEmailCount = XmlHelper.Read(settingPath, str + "/sendEmailSets/sendEmailSet/sendEmailCount");
            List<string> strEmailRecipientList = new List<string>();
            List<string> emailRecipientList = emailRecipients.Split(';').ToList();
            foreach (string emailRecipient in emailRecipientList)
            {
                if (!string.IsNullOrEmpty(emailRecipient))
                {
                    strEmailRecipientList.Add(emailRecipient);
                }
            }

            List<string> strCcRecipientsList = new List<string>();
            List<string> ccRecipientsList = ccRecipients.Split(';').ToList();
            foreach (string emailRecipient in ccRecipientsList)
            {
                if (!string.IsNullOrEmpty(emailRecipient))
                {
                    strCcRecipientsList.Add(emailRecipient);
                }
            }


            SendEmailSettingInfo sendEmailSettingInfo = new SendEmailSettingInfo()
            {
                SmtpHost = smtpHost,
                SmtpPort = int.Parse(smtpPort),
                EmailAccount = emailAccount,
                EmailPassword = emailPassword,
                EmailRecipientList = strEmailRecipientList,//电子邮件收件人列表
                CcRecipientList = strCcRecipientsList,//抄送收件人列表
                LastEmailTime = Convert.ToDateTime(lastEmailTime),
            };
            return sendEmailSettingInfo;
        }


        public List<SendEmailSettingInfo> GetSendEmailSettingInfoList(string settingPath)
        {
            List<SendEmailSettingInfo> sendEmailSettingInfoList = new List<SendEmailSettingInfo>();
            XmlHelper xmlHelper = new XmlHelper(settingPath);
            string str = "/set/customers/customer";
            XmlNodeList xmlNodedata = xmlHelper.ReadAllChild(str);

            for (int i = 0; i < xmlNodedata.Count; i++)
            {
                var xmlData = xmlNodedata[i];
                if (xmlData.Name == "sendEmailSets")
                {
                    for (int j = 0; j < xmlData.ChildNodes.Count; j++)
                    {
                        XmlNode sendEmailSet = xmlData.ChildNodes[j];
                        SendEmailSettingInfo sendEmailSettingInfo = new SendEmailSettingInfo();
                        foreach (XmlNode item in sendEmailSet)
                        {
                            var subXmlNode = item.InnerText;
                            switch (item.Name)
                            {
                                case "smtpHost":
                                    sendEmailSettingInfo.SmtpHost = subXmlNode;
                                    break;
                                case "smtpPort":
                                    sendEmailSettingInfo.SmtpPort = int.Parse(subXmlNode);
                                    break;
                                case "emailAccount":
                                    sendEmailSettingInfo.EmailAccount = subXmlNode;
                                    break;
                                case "emailPassword":
                                    sendEmailSettingInfo.EmailPassword = subXmlNode;
                                    break;
                                case "emailRecipients":
                                    sendEmailSettingInfo.EmailRecipientList = subXmlNode.Replace("；", ";").Split(';').ToList();
                                    break;
                                case "ccRecipients":
                                    sendEmailSettingInfo.CcRecipientList = subXmlNode.Replace("；", ";").Split(';').ToList();
                                    break;
                                case "lastEmailTime":
                                    sendEmailSettingInfo.LastEmailTime = DateTime.Parse(subXmlNode);
                                    break;
                                    //default:
                                    //    throw new Exception("出现错误");
                            }
                        }
                        sendEmailSettingInfoList.Add(sendEmailSettingInfo);
                    }
                }
            }
            return sendEmailSettingInfoList;
        }



        public string GetLanguageChoice(string settingPath)
        {
            string str = "/set/customers/customer[@customerName='NA319']";
            string languageChoice = XmlHelper.Read(settingPath, str + "/languageChoice");
            return languageChoice;
        }
        #endregion
    }
}
