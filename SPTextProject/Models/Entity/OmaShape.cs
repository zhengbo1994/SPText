using SPTextProject.Models.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SPTextProject.Models.Entity
{
    /// <summary>
    /// OMA图形实体类，即各工厂MDDB.dbo.OMA_SHAPE表
    /// </summary>
    [Table("OMA_SHAPE")]
    public class OmaShape : BaseEntity
    {
        [Key]
        [Column("LINE_ID")]
        public int LineId { get; set; }

        /// <summary>
        /// 获取订单号
        /// </summary>
        [Column("ORDER_NUMBER")]
        public string OrderNumber { get; set; } = null;

        /// <summary>
        /// 获取/设置 右眼DIA
        /// </summary>
        [Column("R_DIA")]
        public string DiaR { get; set; } = null;

        /// <summary>
        /// 获取/设置 右眼镜种
        /// </summary>
        [Column("R_LENS")]
        public string LensR { get; set; } = null;

        /// <summary>
        /// 获取/设置 右眼SPH
        /// </summary>
        [Column("R_SPH")]
        public int SphR { get; set; } = 0;

        /// <summary>
        /// 获取/设置 右眼CYL
        /// </summary>
        [Column("R_CYL")]
        public int CylR { get; set; } = 0;

        /// <summary>
        /// 获取/设置 右眼ADD
        /// </summary>
        [Column("R_ADD ")]
        public int AddR { get; set; } = 0;

        /// <summary>
        /// 获取/设置 右眼AXIS
        /// </summary>
        [Column("R_AXIS")]
        public int AxisR { get; set; } = 0;

        /// <summary>
        /// 获取/设置 右眼片数
        /// </summary>
        [Column("R_QTY")]
        public int QtyR { get; set; } = 0;

        /// <summary>
        /// 获取/设置 右眼ET
        /// </summary>
        [Column("R_ET")]
        public string EtR { get; set; } = null;

        /// <summary>
        /// 获取/设置 右眼CT
        /// </summary>
        [Column("R_CT")]
        public string CtR { get; set; } = null;

        /// <summary>
        /// 获取/设置 右眼A值
        /// </summary>
        [Column("R_A")]
        public string A_R { get; set; } = null;

        /// <summary>
        /// 获取/设置 右眼B值
        /// </summary>
        [Column("R_B")]
        public string B_R { get; set; } = null;

        /// <summary>
        /// 获取/设置 右眼远PD值
        /// </summary>
        [Column("R_FAR_PD")]
        public string FarPdR { get; set; } = "0";

        /// <summary>
        /// 获取/设置 右眼近PD值
        /// </summary>
        [Column("R_NEAR_PD")]
        public string NearPdR { get; set; } = "0";

        /// <summary>
        /// 获取/设置 右眼镜高
        /// </summary>
        [Column("R_H")]
        public string HeightR { get; set; } = "0";

        /// <summary>
        /// 获取/设置 右眼最小DIA
        /// </summary>
        [Column("R_MIN_DIA")]
        public string MinDiaR { get; set; } = null;

        /// <summary>
        /// 获取/设置 右眼棱镜度参数Prism1
        /// </summary>
        [Column("R_PRISM1")]
        public string Prism1R { get; set; } = string.Empty;

        /// <summary>
        /// 获取/设置 右眼棱镜度参数Basis1
        /// </summary>
        [Column("R_BASIS1")]
        public string Basis1R { get; set; } = string.Empty;

        /// <summary>
        /// 获取/设置 右眼棱镜度参数Prism2
        /// </summary>
        [Column("R_PRISM2")]
        public string Prism2R { get; set; } = string.Empty;

        /// <summary>
        /// 获取/设置 右眼棱镜度参数Basis2
        /// </summary>
        [Column("R_BASIS2")]
        public string Basis2R { get; set; } = string.Empty;

        /// <summary>
        /// 获取/设置 右眼图形数据
        /// </summary>
        [Column("R_SHAPE")]
        public string ShapeR { get; set; }

        /// <summary>
        /// 获取/设置 右眼图形数据点的总数
        /// </summary>
        [Column("R_TOTAL_POINT")]
        public int TotalPointR { get; set; }

        /// <summary>
        ///  获取/设置 左眼DIA
        /// </summary>
        [Column("L_DIA")]
        public string DiaL { get; set; } = null;

        /// <summary>
        /// 获取/设置 左眼镜种
        /// </summary>
        [Column("L_LENS ")]
        public string LensL { get; set; } = null;

        /// <summary>
        /// 获取/设置 左眼SPH
        /// </summary>
        [Column("L_SPH")]
        public int SphL { get; set; } = 0;

        /// <summary>
        /// 获取/设置 左眼CYL
        /// </summary>
        [Column("L_CYL")]
        public int CylL { get; set; } = 0;

        /// <summary>
        /// 获取/设置 左眼ADD
        /// </summary>
        [Column("L_ADD")]
        public int AddL { get; set; } = 0;

        /// <summary>
        /// 获取/设置 左眼AXIS
        /// </summary>
        [Column("L_AXIS")]

        public int AxisL { get; set; } = 0;

        /// <summary>
        /// 获取/设置 左眼片数
        /// </summary>
        [Column("L_QTY")]
        public int QtyL { get; set; } = 0;

        /// <summary>
        /// 获取/设置 左眼ET
        /// </summary>
        [Column("L_ET")]
        public string EtL { get; set; } = null;

        /// <summary>
        /// 获取/设置 左眼CT
        /// </summary>
        [Column("L_CT")]
        public string CtL { get; set; } = null;

        /// <summary>
        /// 获取/设置 左眼A值
        /// </summary>
        [Column("L_A")]
        public string A_L { get; set; } = null;

        /// <summary>
        /// 获取/设置 左眼B值
        /// </summary>
        [Column("L_B")]
        public string B_L { get; set; } = null;

        /// <summary>
        /// 获取/设置 左眼远PD值
        /// </summary>
        [Column("L_FAR_PD")]
        public string FarPdL { get; set; } = "0";

        /// <summary>
        /// 获取/设置 左眼近PD值
        /// </summary>
        [Column("L_NEAR_PD")]
        public string NearPdL { get; set; } = "0";

        /// <summary>
        /// 获取/设置 左眼镜高
        /// </summary>
        [Column("L_H")]
        public string HeightL { get; set; } = "0";

        /// <summary>
        /// 获取/设置 左眼最小DIA
        /// </summary>
        [Column("L_MIN_DIA")]
        public string MinDiaL { get; set; } = null;

        /// <summary>
        /// 获取/设置 左眼棱镜度参数Prism1
        /// </summary>
        [Column("L_PRISM1")]
        public string Prism1L { get; set; } = string.Empty;

        /// <summary>
        /// 获取/设置 左眼棱镜度参数Basis1
        /// </summary>
        [Column("L_BASIS1")]
        public string Basis1L { get; set; } = string.Empty;

        /// <summary>
        /// 获取/设置 左眼棱镜度参数Prism2
        /// </summary>
        [Column("L_PRISM2")]
        public string Prism2L { get; set; } = string.Empty;

        /// <summary>
        /// 获取/设置 左眼棱镜度参数Basis2
        /// </summary>
        [Column("L_BASIS2")]
        public string Basis2L { get; set; } = string.Empty;

        /// <summary>
        /// 获取/设置 左眼图形数据
        /// </summary>
        [Column("L_SHAPE")]
        public string ShapeL { get; set; }

        /// <summary>
        /// 获取/设置 左眼图形数据点的总数
        /// </summary>
        [Column("L_TOTAL_POINT")]
        public int TotalPointL { get; set; }

        /// <summary>
        /// 获取/设置 染色代号
        /// </summary>
        [Column("TINT_CODE")]
        public string TintCode { get; set; }

        /// <summary>
        /// 获取/设置 染色名称
        /// </summary>
        [Column("TINT_DESC")]
        public string TintDesc { get; set; }

        /// <summary>
        /// 获取/设置 加硬
        /// </summary>
        [Column("HARD")]
        public bool Hard { get; set; }

        /// <summary>
        /// 获取/设置 MC/焗色/加膜
        /// </summary>
        [Column("MC")]
        public string Coat { get; set; }

        /// <summary>
        /// 获取/设置 DBL
        /// </summary>
        [Column("DBL")]
        public string Dbl { get; set; }

        /// <summary>
        /// 获取/设置 FrameType
        /// </summary>
        [Column("FRAME_TYPE")]
        public string FrameType { get; set; }

        /// <summary>
        /// 获取/设置 Remark
        /// </summary>
        [Column("REMARK")]
        public string Remark { get; set; }

        /// <summary>
        /// 获取/设置 创建日期
        /// </summary>
        [Column("CREATION_DATE")]
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// 获取/设置 创建人
        /// </summary>
        [Column("CREATED_BY_COMPUTER")]
        public string CreatedByComputer { get; set; }

        /// <summary>
        /// 获取/设置 是否已导出
        /// </summary>
        [Column("EXPORTED")]
        public bool Exported { get; set; }

        /// <summary>
        /// 获取/设置 右眼GDEPTH
        /// </summary>
        [Column("R_GDEPTH")]
        public string GdepthR { get; set; }

        /// <summary>
        /// 获取/设置 右眼GWIDTH
        /// </summary>
        [Column("R_GWIDTH")]
        public string GwidthR { get; set; }

        /// <summary>
        /// 获取/设置 左眼GDEPTH
        /// </summary>
        [Column("L_GDEPTH")]
        public string GdepthL { get; set; }

        /// <summary>
        /// 获取/设置 左眼GWIDTH
        /// </summary>
        [Column("L_GWIDTH")]
        public string GwidthL { get; set; }
    }
}
