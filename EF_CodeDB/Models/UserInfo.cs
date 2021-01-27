using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EF_CodeDB
{
    public partial class UserInfo
    {
        [Key]
        public int UserId { get; set; }
        public string UserName { get; set; }

        public byte Status { get; set; }
        public int UserAge { get; set; }
        public string Description { get; set; }

        public string Description01 { get; set; }

        public string Remark { get; set; }

        public virtual SysUser SysUser { get; set; }
    }
}
