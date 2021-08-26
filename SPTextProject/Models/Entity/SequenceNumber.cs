using SPTextProject.Models.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SPTextProject.Models.Entity
{
    [Keyless]
    public class SequenceNumber : BaseEntity
    {
        [Column("流水號")]
        [Key]
        public string Sequence { get; set; }
    }
}
