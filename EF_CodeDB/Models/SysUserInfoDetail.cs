﻿using System;
using System.Collections.Generic;

namespace EF_CodeDB
{
    public partial class SysUserInfoDetail
    {
        public int Id { get; set; }
        public int SysUserInfoDetailId { get; set; }
        public string Description { get; set; }
        public int? SysUserInfoId { get; set; }
         
    }
}
