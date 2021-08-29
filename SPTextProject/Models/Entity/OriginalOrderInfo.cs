using Microsoft.EntityFrameworkCore;
using SPTextProject.Models.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SPTextProject.Models.Entity
{
    [Keyless]
    [Table("hkows_order")]
    public class OriginalOrderInfo : BaseEntity
    {
        /// <summary>
        /// OMA数据文件的URL地址
        /// </summary>
        [Column("oma_file")]
        [Key]
        public string OmaFileUrl { get; set; }

        ///// <summary>
        ///// 主键ID
        ///// </summary>
        //[Column("id")]
        //public int Id { get; set; }

        ///// <summary>
        ///// 客户代号
        ///// </summary>
        //[Column("customer_code")]
        //public string CustomerCode { get; set; }

        ///// <summary>
        ///// 店铺代号
        ///// </summary>
        //[Column("shop_code")]
        //public string ShopCode { get; set; }

        ///// <summary>
        ///// 登记号
        ///// </summary>
        //[Column("deposit_number")]
        //public string DepositNumber { get; set; }

        ///// <summary>
        ///// 工单号
        ///// </summary>
        //[Column("wo_number")]
        //public string WoNumber { get; set; }

        ///// <summary>
        ///// 账单
        ///// </summary>
        //public string BillTo { get; set; }
    }
}
