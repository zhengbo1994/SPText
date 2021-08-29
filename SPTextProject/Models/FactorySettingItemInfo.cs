using System;
using System.Collections.Generic;
using System.Text;

namespace SPTextProject.Models
{
    public class FactorySettingItemInfo
    {
        public FactorySettingItemInfo(string name, string connStr, string condition, string[] emailRecipientList, string[] emailCCRecipientList, int order, string customerCode)
        {
            Name = name;
            ConnStr = connStr;
            Condition = condition;
            EmailRecipientList = emailRecipientList;
            EmailCCRecipientList = emailCCRecipientList;
            Order = order;
            CustomerCode = customerCode;
        }

        /// <summary>
        /// 工厂代号
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 工厂数据库连接字符串
        /// </summary>
        public string ConnStr { get; set; }

        /// <summary>
        /// 工厂导出订单的查询条件
        /// </summary>
        public string Condition { get; set; }

        /// <summary>
        /// 邮件收件人列表
        /// </summary>
        public string[] EmailRecipientList { get; set; }

        /// <summary>
        /// 邮件抄送人列表
        /// </summary>
        public string[] EmailCCRecipientList { get; set; }

        /// <summary>
        /// 执行次序
        /// </summary>
        public int Order { get; set; }
        /// <summary>
        /// 客户代码
        /// </summary>
        public string CustomerCode { get; set; }
    }
}
