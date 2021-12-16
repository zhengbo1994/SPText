using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace SPCoreText.Model
{
    /// <summary>
    /// 树状结构实体
    /// </summary>
    public abstract class TreeEntity : Entity
    {
        /// <summary>
        /// 节点语义ID
        /// </summary>
        [Description("节点语义ID")]
        public string CascadeId { get; set; }
        /// <summary>
        /// 功能模块名称
        /// </summary>
        [Description("功能模块名称")]
        public string Name { get; set; }

        /// <summary>
        /// 父节点流水号
        /// </summary>
        [Description("父节点流水号")]
        public string ParentId { get; set; }
        /// <summary>
        /// 父节点名称
        /// </summary>
        [Description("父节点名称")]
        public string ParentName { get; set; }

    }
}
