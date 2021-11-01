using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SPCoreText.Model
{
    /// <summary>
    /// 用户基本信息表
    /// </summary>
    [Table("User_Verification")]
    public partial class User_Verification : Entity
    {
        [Key]
        public new int Id { get; set; }

        [Description("用户ID")]
        public string UserId { get; set; }
        [Description("用户Code")]
        public string UserCode { get; set; }
        [Description("验证码")]
        public string VerificationCode { get; set; }
        [Description("验证码发生时间")]
        public DateTime VerificationTime { get; set; }
        [Description("备用字段1")]
        public string Reserve1 { get; set; }
    }
}
