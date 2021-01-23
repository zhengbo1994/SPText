using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPTextWinForm.Public
{
    public class OrderAddress
    {
        public int Id { get; set; }
        /// <summary>
        /// 地址表Id
        /// </summary>
        public int ZoneItemsId { get; set; }
        /// <summary>
        /// 流水号
        /// </summary>
        public string Sequence_Num { get; set; }
        /// <summary>
        /// JD账户
        /// </summary>
        public string AccountName { get; set; }
        /// <summary>
        /// 学校名称
        /// </summary>
        public string PatientName { get; set; }
        /// <summary>
        /// 学校代码
        /// </summary>
        public string SchoolCode { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }
    }
}
