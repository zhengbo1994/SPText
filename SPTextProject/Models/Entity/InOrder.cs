using SPTextProject.Models.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SPTextProject.Models.Entity
{
    [Table("yjjdk_pending_hkows")]
    public class InOrder : BaseEntity
    {
        /// <summary>
        /// 获取/设置 ID
        /// </summary>
        [Column("id")]
        [Key]
        public long Id { get; set; }

        /// <summary>
        /// 获取/设置 流水号
        /// </summary>
        [Column("流水號")]
        public string SequenceNumber { get; set; }

        /// <summary>
        /// 获取/设置 传出区
        /// </summary>
        [Column("傳出區")]
        public string FromZone { get; set; }

        /// <summary>
        /// 获取/设置 流水号HK
        /// </summary>
        [Column("流水號HK")]
        public string SequenceNumberHk { get; set; }

        /// <summary>
        /// 获取/设置 拟出货日期
        /// </summary>
        [Column("擬出貨")]
        public string DeliveryDate { get; set; }

        /// <summary>
        /// 获取/设置 拟出货时段
        /// </summary>
        [Column("擬出時段")]
        public string TimeOut { get; set; }

        /// <summary>
        /// 获取/设置 客户单号
        /// </summary>
        [Column("客戶單號")]
        public string CustomerOrderNumber { get; set; }

        [Column("PD")]
        public string PD { get; set; }

        [Column("鏡種類R")]
        public string LensCodeR { get; set; }

        [Column("附加內容R")]
        public string LensAddR { get; set; }

        [Column("SPHR")]
        public int? SphR { get; set; }

        [Column("CylR")]
        public int? CylR { get; set; }

        [Column("AXISR")]
        public int? AxisR { get; set; }

        [Column("ADDR")]
        public int? AddR { get; set; }

        [Column("片數R")]
        public int? QtyR { get; set; }

        [Column("鏡種類L")]
        public string LensCodeL { get; set; }

        [Column("附加內容L")]
        public string LensAddL { get; set; }

        [Column("SPHL")]
        public int? SphL { get; set; }

        [Column("CYLL ")]
        public int? CylL { get; set; }

        [Column("AXISL")]
        public int? AxisL { get; set; }

        [Column("ADDL")]
        public int? AddL { get; set; }

        [Column("片數L")]
        public int? QtyL { get; set; }

        [Column("焗色")]
        public string Coat { get; set; }

        /// <summary>
        /// 获取/设置 茶色
        /// </summary>
        [Column("茶色")]
        public string Brown { get; set; }

        /// <summary>
        /// 获取/设置 水银
        /// </summary>
        [Column("水銀")]
        public string Mir { get; set; }

        /// <summary>
        /// 获取/设置 染色代号
        /// </summary>
        [Column("染色代號")]
        public string TintCode { get; set; }

        /// <summary>
        /// 获取/设置 染色名称
        /// </summary>
        [Column("染色名稱")]
        public string TintName { get; set; }

        /// <summary>
        /// 获取/设置 UV
        /// </summary>
        [Column("UV")]
        public bool? UV { get; set; }

        /// <summary>
        /// 获取/设置 抛光
        /// </summary>
        [Column("拋光")]
        public bool? Polish { get; set; }

        /// <summary>
        /// 获取/设置 彩边
        /// </summary>
        [Column("彩邊")]
        public string ColorEdge { get; set; }

        /// <summary>
        /// 获取/设置 Hard
        /// </summary>
        [Column("加硬")]
        public bool? Hard { get; set; }

        /// <summary>
        /// 获取/设置 EdgeCode 车边代号
        /// </summary>
        [Column("車邊代號")]
        public string EdgeCode { get; set; }

        /// <summary>
        /// 获取/设置 SupeCode 开坑代号
        /// </summary>
        [Column("開坑代號")]
        public string SupeCode { get; set; }

        /// <summary>
        /// 获取/设置 SpecialCode 批花代号
        /// </summary>
        [Column("批花代號")]
        public string SpecialCode { get; set; }

        /// <summary>
        /// 获取/设置 输入日期
        /// </summary>
        [Column("輸入日期")]
        public DateTime? OrderDate { get; set; }

        /// <summary>
        /// 获取/设置 收货日期
        /// </summary>
        [Column("收貨日期")]
        public DateTime? PutDate { get; set; }

        /// <summary>
        /// 获取/设置 出货日期
        /// </summary>
        [Column("出貨日期")]
        public DateTime? OrderDispatch { get; set; }

        /// <summary>
        /// 获取/设置 已扣账
        /// </summary>
        [Column("已扣帳")]
        public bool? PutGood { get; set; }

        /// <summary>
        /// 获取/设置 取消单
        /// </summary>
        [Column("取消單")]
        public bool? Cancelled { get; set; }

        /// <summary>
        /// 获取/设置 中心R
        /// </summary>
        [Column("中心R")]
        public string CPR { get; set; }

        /// <summary>
        /// 获取/设置 中心L
        /// </summary>
        [Column("中心L")]
        public string CPL { get; set; }

        /// <summary>
        /// 获取/设置 刀边R
        /// </summary>
        [Column("刀邊R")]
        public bool CutR { get; set; }

        /// <summary>
        /// 获取/设置 刀边L
        /// </summary>
        [Column("刀邊L")]
        public bool CutL { get; set; }

        /// <summary>
        /// 获取/设置 面弯R
        /// </summary>
        [Column("面灣R")]
        public string BaseR { get; set; }

        /// <summary>
        /// 获取/设置 面弯L
        /// </summary>
        [Column("面灣L")]
        public string BaseL { get; set; }

        /// <summary>
        /// 获取/设置 镜架模式
        /// </summary>
        [Column("鏡架模式")]
        public string FrameMode { get; set; }

        /// <summary>
        /// 获取/设置 移中心R 
        /// </summary>
        [Column("移中心R")]
        public string DeCenterR { get; set; }

        /// <summary>
        /// 获取/设置 移中心L
        /// </summary>
        [Column("移中心L")]
        public string DeCenterL { get; set; }

        /// <summary>
        /// 获取/设置 棱镜度R
        /// </summary>
        [Column("稜鏡度R")]
        public string PrismR { get; set; }

        /// <summary>
        /// 获取/设置 棱镜度L
        /// </summary>
        [Column("稜鏡度L")]
        public string PrismL { get; set; }

        /// <summary>
        /// 获取/设置 是否车房
        /// </summary>
        [Column("車房")]
        public bool RX { get; set; }

        /// <summary>
        /// 获取/设置 客户名称
        /// </summary>
        [Column("客戶名稱")]
        public string CustomerName { get; set; }

        /// <summary>
        /// 获取/设置 客户代号
        /// </summary>
        [Column("客戶代號")]
        public string CustomerCode { get; set; }

        /// <summary>
        /// 获取/设置 玻璃/胶片 G/P
        /// </summary>
        [Column("玻璃膠片")]
        public string GorP { get; set; }

        /// <summary>
        /// 获取/设置 跟单条码
        /// </summary>
        [Column("跟單條碼")]
        public string Barcode { get; set; }

        [Column("特別注明")]
        public string Remark { get; set; }

        /// <summary>
        /// 获取/设置 是否已列印
        /// </summary>
        [Column("已列印")]
        public bool? Printed { get; set; }


        /// <summary>
        /// 获取/设置 是否已处理
        /// </summary>
        [Column("已處理")]
        public bool? Processed { get; set; }

        /// <summary>
        /// 获取/设置 FrameCode
        /// </summary>
        [Column("FRAME_CODE")]
        public string FrameCode { get; set; }

        /// <summary>
        /// 获取/设置 镜架数量
        /// </summary>
        [Column("FRAME_QTY")]
        public int? FrameQty { get; set; }

        /// <summary>
        /// 获取/设置 上次更新时间
        /// </summary>
        [Column("LAST_UPDATE_DATE")]
        public DateTime? LastUpdateDate { get; set; }

        /// <summary>
        /// 获取/设置 上次更新人
        /// </summary>
        [Column("LAST_UPDATED_BY_NAME")]
        public string LastUpdatedByName { get; set; }

        /// <summary>
        /// 获取/设置 Attribute1
        /// </summary>
        [Column("ATTRIBUTE1")]
        public string Attribute1 { get; set; }

        /// <summary>
        /// 获取/设置 Attribute2
        /// </summary>
        [Column("ATTRIBUTE2")]
        public string Attribute2 { get; set; }

        /// <summary>
        /// 获取/设置 Attribute3
        /// </summary>
        [Column("ATTRIBUTE3")]
        public string Attribute3 { get; set; }

        /// <summary>
        /// 获取/设置 Attribute4
        /// </summary>
        [Column("ATTRIBUTE4")]
        public string Attribute4 { get; set; }

        /// <summary>
        /// 获取/设置 Attribute5
        /// </summary>
        [Column("ATTRIBUTE5")]
        public string Attribute5 { get; set; }

        /// <summary>
        /// 获取/设置 镜高
        /// </summary>
        [Column("oc_height")]
        public string OcHeight { get; set; }

        /// <summary>
        /// 获取/设置 是否失效
        /// </summary>
        [Column("expired")]
        public bool? Expired { get; set; }

        /// <summary>
        /// 获取/设置 是否裁型
        /// </summary>
        [Column("edged")]
        public bool? Edged { get; set; }

        /// <summary>
        /// 获取/设置 是否同步到工厂
        /// </summary>
        [Column("isSyncFactory")]
        public bool IsSyncFactory { get; set; }
    }
}
