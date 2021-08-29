using System;
using System.Collections.Generic;
using System.Text;

namespace SPTextProject.BLL.Entity
{
    public class OmaFileSeparateInfo
    {
        public string CLIENT { get; set; }
        public string ACCN { get; set; }
        public string DO { get; set; }
        public string LNAM { get; set; }
        public string SPH { get; set; }
        public string CYL { get; set; }
        public string AX { get; set; }
        public string ADD { get; set; }
        public string SBOCIN { get; set; }
        public string ACOAT { get; set; }
        public string PTOK { get; set; }
        public string _PROCESS { get; set; }
        public string _SOCIETE { get; set; }
        public string _REFERENCE { get; set; }
        public string _RUCHER { get; set; }
        public string _NUMCLI { get; set; }
        public string _FNCLIENT { get; set; }
        public string _CITY { get; set; }
        public string _COMMENT { get; set; }
        public string _SUPPL { get; set; }
        public string _PORTEUR { get; set; }
        public string CRIB { get; set; }
        public string MBASE { get; set; }
        public string TINT { get; set; }
        public string _CTO { get; set; }

        public string _RECTYPE { get; set; }
        public string IPD { get; set; }
        public string OCHT { get; set; }
        public string BRGSIZ { get; set; }
        public string HBOX { get; set; }
        public string VBOX { get; set; }
        public string _REFRE { get; set; }
        public string _DOC { get; set; }
        public string FCRV { get; set; }
        public string ZTILT { get; set; }
        public string _DATEORDER { get; set; }

        public int TotalPointR { get; set; }
        public string SHAPER { get; set; }
        public int TotalPointL { get; set; }
        public string SHAPEL { get; set; }

        public string CIRC { get; set; }
        public string _LCOAT { get; set; }
        public string BSIZ { get; set; }
        public string LMATTYPE { get; set; }
        public string FPD { get; set; }
        public string MPD { get; set; }
        public string FBFCIN { get; set; }
        public string FBFCUP { get; set; }
        public string _FBFCANG { get; set; }
        public string HBOX2 { get; set; }
        public string _MATRIX { get; set; }
        public string _LOTNUMBER { get; set; }
        public string _MONT { get; set; }

        /// <summary>
        /// 棱镜的字段
        /// </summary>
        public string PRVM { get; set; }
        public string PRVA { get; set; }
        public string _PRVM1 { get; set; }
        public string _PRVA1 { get; set; }
        public string _PRVM2 { get; set; }
        public string _PRVA2 { get; set; }

        /// <summary>
        /// 比桥开孔的资料
        /// </summary>
        public string DRILLE { get; set; }

        /// <summary>
        /// Frame type
        /// </summary>
        public string FTYP { get; set; }
        /// <summary>
        /// Edge type
        /// </summary>
        public string ETYP { get; set; }

        /// <summary>
        /// Bevel
        /// </summary>
        public string FPINB { get; set; }
        public string PINB { get; set; }
        public string BEVP { get; set; }
        public string BEVM { get; set; }

        /// <summary>
        /// 抛光
        /// </summary>
        public string POLISH { get; set; }

        /// <summary>
        /// Groove depth
        /// </summary>
        public string GDEPTH { get; set; }

        /// <summary>
        /// Groove width
        /// </summary>
        public string GWIDTH { get; set; }

        /// <summary>
        /// Distance between lenses 
        /// </summary>
        public string DBL { get; set; }

        /// <summary>
        /// MINEDG 裁型厚度
        /// </summary>
        public string MINEDG { get; set; }
    }
}
