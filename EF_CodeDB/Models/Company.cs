namespace EF_CodeDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("Company")]
    public partial class Company
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Company()
        {
            Users = new HashSet<User>();
        }

        public int Id { get; set; }

        [StringLength(500)]
        public string Name { get; set; }

        public DateTime CreateTime { get; set; }

        public int CreatorId { get; set; }

        public int? LastModifierId { get; set; }

        public DateTime? LastModifyTime { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<User> Users { get; set; }
        public string Description { get; set; }

        /// <summary> 
        /// 指定某个属性为另个实体的结合或者实体对象 
        /// 导航属性 
        /// 配置了导航属性以后，生成数据库默认会生成对应的主外键关系；
        /// </summary>
        public virtual ICollection<SysUser> SysUser { get; set; }
    }
}
