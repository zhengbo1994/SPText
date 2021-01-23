using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF_CodeDB
{
   
    public partial class SysLogDetail
    {
        public int Id { get; set; }
     
        public string Introduction { get; set; }
        public string Detail { get; set; }
        public byte LogType { get; set; }
        public int CreatorId { get; set; }
        public int? LastModifierId { get; set; }
        public DateTime? LastModifyTime { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
