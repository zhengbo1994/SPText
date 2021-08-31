using SPTextProject.BLL.Entity;
using SPTextProject.Common;
using SPTextProject.Core;
using SPTextProject.IBLL;
using SPTextProject.IDAL;
using SPTextProject.Models;
using SPTextProject.Models.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Text.RegularExpressions.Regex;
using System.Linq;

namespace SPTextProject.BLL
{
    /// <summary>
    /// 对OMA图形数据的操作的继承类
    /// </summary>
    public class OmaShapeDataService : IOmaShapeDataService
    {
        private readonly IUnitWork<OutOrderContext> _out;
        private readonly IUnitWork<InOrderContext> _in;

        public OmaShapeDataService(IUnitWork<OutOrderContext> outUnitWork, IUnitWork<InOrderContext> inUnitWork)
        {
            _out = outUnitWork;
            _in = inUnitWork;
        }

        public string GetOmaFileUrlByBarcode(string barcode)
        {
            return _in.Query<OriginalOrderInfo>(SqlCache.GetOmaFileBYBarcodeSql, barcode).ToList().FirstOrDefault().OmaFileUrl;
        }

        public string GetOmaDataContent(string url)
        {
            string content = HttpCallHelper.Get(url).Result;

            return content;
        }

        public void SendOmaData(OutOrder order, string content)
        {
            //图形文件中的数据
            OmaFileSeparateInfo info = GetShape(content);

            //图形数据对象
            var omaShape = GetOmaShape(order, info);

            //将图形数据对象添加到表中
            _out.Add<OmaShape>(omaShape);
        }

        /// <summary>
        /// 获取图形数据对象
        /// </summary>
        /// <param name="order"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        private OmaShape GetOmaShape(OutOrder order, OmaFileSeparateInfo info)
        {
            OmaShape omaShapeData = new OmaShape();

            omaShapeData.OrderNumber = order.CustomerOrderNumber;

            //PD
            MappingPd(order, omaShapeData);

            //H 镜高
            MappingHeight(order, omaShapeData);

            //Prism 棱镜度
            MappingPrism(order, omaShapeData);

            omaShapeData.DiaR = Match(Match(order.Remark, "(R⊕[0-9]+)").Value, "[0-9]+").Value;
            omaShapeData.MinDiaR = omaShapeData.DiaR;
            omaShapeData.DiaL = Match(Match(order.Remark, "(L⊕[0-9]+)").Value, "[0-9]+").Value;
            omaShapeData.MinDiaL = omaShapeData.DiaL;

            omaShapeData.LensR = order.LensCodeR;
            omaShapeData.SphR = order.SphR.GetValueOrDefault();
            omaShapeData.CylR = order.CylR.GetValueOrDefault();
            omaShapeData.AddR = order.AddR.GetValueOrDefault();
            omaShapeData.AxisR = order.AxisR.GetValueOrDefault();
            omaShapeData.QtyR = order.QtyR.GetValueOrDefault();
            omaShapeData.Prism1R = string.Empty;
            omaShapeData.Basis1R = string.Empty;
            omaShapeData.Prism2R = string.Empty;
            omaShapeData.Basis2R = string.Empty;
            omaShapeData.ShapeR = info.SHAPER;
            omaShapeData.TotalPointR = info.TotalPointR;

            omaShapeData.LensL = order.LensCodeL;
            omaShapeData.SphL = order.SphL.GetValueOrDefault();
            omaShapeData.CylL = order.CylL.GetValueOrDefault();
            omaShapeData.AddL = order.AddL.GetValueOrDefault();
            omaShapeData.AxisL = order.AxisL.GetValueOrDefault();
            omaShapeData.QtyL = order.QtyL.GetValueOrDefault();
            omaShapeData.ShapeL = info.SHAPEL;
            omaShapeData.TotalPointL = info.TotalPointL;

            omaShapeData.TintCode = string.Empty;
            omaShapeData.TintDesc = string.Empty;
            omaShapeData.Hard = order.Hard.GetValueOrDefault();
            omaShapeData.Coat = order.Coat;
            omaShapeData.Dbl = float.Parse(info.DBL).ToString("#0.00");
            omaShapeData.FrameType = string.Empty;
            omaShapeData.Remark = string.Empty;
            omaShapeData.CreationDate = DateTime.Now;
            //omaShapeData.CreatedByComputer = "NA353_AF101_ORDER";
            omaShapeData.CreatedByComputer = "Ordering Management";
            omaShapeData.Exported = false;

            return omaShapeData;

            static void MappingPd(OutOrder order, OmaShape omaShapeData)
            {
                if (IsMatch(order.Remark, "PD=[0-9]+.[0-9]+/[0-9]+.[0-9]+"))
                {
                    var matchPd = Match(Match(order.Remark, "PD=[0-9]+.[0-9]+/[0-9]+.[0-9]+").Value, "[0-9]+.[0-9]+");
                    omaShapeData.FarPdR = matchPd.Value;
                    omaShapeData.NearPdR = omaShapeData.FarPdR;
                    omaShapeData.FarPdL = matchPd.NextMatch().Value;
                    omaShapeData.NearPdL = omaShapeData.FarPdL;
                }
                else if (IsMatch(order.Remark, "RPD=[0-9]+.[0-9]+"))
                {
                    omaShapeData.FarPdR = Match(Match(order.Remark, "RPD=[0-9]+.[0-9]+").Value, "[0-9]+.[0-9]+").Value;
                    omaShapeData.NearPdR = omaShapeData.FarPdR;
                }
                else if (IsMatch(order.Remark, "LPD=[0-9]+.[0-9]+"))
                {
                    omaShapeData.FarPdL = Match(Match(order.Remark, "LPD=[0-9]+.[0-9]+").Value, "[0-9]+.[0-9]+").Value;
                    omaShapeData.NearPdL = omaShapeData.FarPdL;
                }
            }

            static void MappingHeight(OutOrder order, OmaShape omaShapeData)
            {
                if (IsMatch(order.Remark, "H=[0-9]+/[0-9]+"))
                {
                    var matchH = Match(Match(order.Remark, "H=[0-9]+/[0-9]+").Value, "[0-9]+");
                    omaShapeData.HeightR = matchH.Value;
                    omaShapeData.HeightL = matchH.NextMatch().Value;
                }
                else if (IsMatch(order.Remark, "RH=[0-9]+"))
                {
                    omaShapeData.HeightR = Match(order.Remark, "RH=[0-9]+").Value.Replace("RH=", "");
                }
                else if (IsMatch(order.Remark, "LH=[0-9]+"))
                {
                    omaShapeData.HeightL = Match(order.Remark, "LH=[0-9]+").Value.Replace("LH=", "");
                }
                else if (IsMatch(order.Remark, "H=[0-9]+"))
                {
                    omaShapeData.HeightR = Match(order.Remark, "H=[0-9]+").Value.Replace("H=", "");
                    omaShapeData.HeightL = omaShapeData.HeightR;
                }
            }

            static void MappingPrism(OutOrder order, OmaShape omaShapeData)
            {
                omaShapeData.Prism1R = order.PrismR.Replace("U", "UP").Replace("D", "DOWM").Replace("O", "OUT").Replace("I", "IN");
                omaShapeData.Prism2R = order.PrismR.ToString().Replace("U", "UP").Replace("D", "DOWM").Replace("O", "OUT").Replace("I", "IN");
                omaShapeData.Prism1L = order.PrismL.ToString().Replace("U", "UP").Replace("D", "DOWM").Replace("O", "OUT").Replace("I", "IN");
                omaShapeData.Prism2L = order.PrismL.ToString().Replace("U", "UP").Replace("D", "DOWM").Replace("O", "OUT").Replace("I", "IN");
            }
        }

        /// <summary>
        /// 获取原始图形文件中的数据
        /// </summary>
        /// <param name="omaStr"></param>
        /// <returns></returns>
        private OmaFileSeparateInfo GetShape(string omaStr)
        {
            var info = new OmaFileSeparateInfo();
            string[] omaStrArr = omaStr.Replace("\n", "").Split('\r');
            StringBuilder sbShapeR = new StringBuilder();
            StringBuilder sbShapeL = new StringBuilder();

            int shapeRStart = 0, shapeLStart = 0;
            for (int i = 0; i < omaStrArr.Length; i++)
            {
                //if (IsMatch(omaStrArr[i], "TRCFMT=1;[0-9]+;E;R;F"))
                if (IsMatch(omaStrArr[i], "TRCFMT=1;[0-9]+;[E,U,C];R;[F,P,D]"))
                {
                    shapeRStart = i;
                    info.TotalPointR = int.Parse(omaStrArr[i].Split(';')[1]);
                }
                //else if (IsMatch(omaStrArr[i], "TRCFMT=1;[0-9]+;E;L;F"))
                else if (IsMatch(omaStrArr[i], "TRCFMT=1;[0-9]+;[E,U,C];L;[F,P,D]"))
                {
                    shapeLStart = i;
                    info.TotalPointL = int.Parse(omaStrArr[i].Split(';')[1]);
                }
                else if (i > shapeRStart && shapeRStart != 0 && shapeLStart == 0 && omaStrArr[i].StartsWith("R="))
                {
                    sbShapeR.Append(omaStrArr[i].Replace("R=", "").Replace(";", ""));
                }
                else if (i > shapeLStart && shapeLStart != 0 && omaStrArr[i].StartsWith("R="))
                {
                    sbShapeL.Append(omaStrArr[i].Replace("R=", "").Replace(";", ""));
                }
            }

            info.SHAPER = sbShapeR.Length > 0 ? sbShapeR.ToString() : string.Empty;
            info.SHAPEL = sbShapeL.Length > 0 ? sbShapeL.ToString() : string.Empty;

            info.CLIENT = omaStrArr.Where(o => o.Contains("CLIENT="))?.FirstOrDefault()?.Replace("CLIENT=", "").Trim() ?? "";
            info.ACCN = omaStrArr.Where(o => o.Contains("ACCN="))?.FirstOrDefault()?.Replace("ACCN=", "").Trim() ?? "";
            info.DO = omaStrArr.Where(o => o.Contains("DO="))?.FirstOrDefault()?.Replace("DO=", "").Trim() ?? "";
            info.LNAM = omaStrArr.Where(o => o.Contains("LNAM="))?.FirstOrDefault()?.Replace("LNAM=", "").Trim() ?? "";
            info._CTO = omaStrArr.Where(o => o.Contains("_CTO="))?.FirstOrDefault()?.Replace("_CTO=", "").Trim() ?? "";
            info._RECTYPE = omaStrArr.Where(o => o.Contains("_RECTYPE="))?.FirstOrDefault()?.Replace("_RECTYPE=", "").Trim() ?? "";
            info.IPD = omaStrArr.Where(o => o.Contains("IPD="))?.FirstOrDefault()?.Replace("IPD=", "").Trim() ?? "";
            info.OCHT = omaStrArr.Where(o => o.Contains("OCHT="))?.FirstOrDefault()?.Replace("OCHT=", "").Trim() ?? "";
            info.BRGSIZ = omaStrArr.Where(o => o.Contains("BRGSIZ="))?.FirstOrDefault()?.Replace("BRGSIZ=", "").Trim() ?? "";
            info.HBOX = omaStrArr.Where(o => o.Contains("HBOX="))?.FirstOrDefault()?.Replace("HBOX=", "").Trim() ?? "";
            info.VBOX = omaStrArr.Where(o => o.Contains("VBOX="))?.FirstOrDefault()?.Replace("VBOX=", "").Trim() ?? "";
            info._REFRE = omaStrArr.Where(o => o.Contains("_REFRE="))?.FirstOrDefault()?.Replace("_REFRE=", "").Trim() ?? "";
            info._DOC = omaStrArr.Where(o => o.Contains("_DOC="))?.FirstOrDefault()?.Replace("_DOC=", "").Trim() ?? "";
            info.FCRV = omaStrArr.Where(o => o.Contains("FCRV="))?.FirstOrDefault()?.Replace("FCRV=", "").Trim() ?? "";
            info.ZTILT = omaStrArr.Where(o => o.Contains("ZTILT="))?.FirstOrDefault()?.Replace("ZTILT=", "").Trim() ?? "";
            info.SPH = omaStrArr.Where(o => o.Contains("SPH="))?.FirstOrDefault()?.Replace("SPH=", "").Trim() ?? "";
            info.CYL = omaStrArr.Where(o => o.Contains("CYL="))?.FirstOrDefault()?.Replace("CYL=", "").Trim() ?? "";
            info.AX = omaStrArr.Where(o => o.Contains("AX="))?.FirstOrDefault()?.Replace("AX=", "").Trim() ?? "";
            info.ADD = omaStrArr.Where(o => o.Contains("ADD="))?.FirstOrDefault()?.Replace("ADD=", "").Trim() ?? "";
            info.SBOCIN = omaStrArr.Where(o => o.Contains("SBOCIN="))?.FirstOrDefault()?.Replace("SBOCIN=", "").Trim() ?? "";
            info.ACOAT = omaStrArr.Where(o => o.Contains("ACOAT="))?.FirstOrDefault()?.Replace("ACOAT=", "").Trim() ?? "";
            info.PTOK = omaStrArr.Where(o => o.Contains("PTOK="))?.FirstOrDefault()?.Replace("PTOK=", "").Trim() ?? "";
            info.TINT = omaStrArr.Where(o => o.Contains("TINT="))?.FirstOrDefault()?.Replace("TINT=", "").Trim() ?? "";
            info._PROCESS = omaStrArr.Where(o => o.Contains("_PROCESS="))?.FirstOrDefault()?.Replace("_PROCESS=", "").Trim() ?? "";
            info._DATEORDER = omaStrArr.Where(o => o.Contains("_DATEORDER="))?.FirstOrDefault()?.Replace("_DATEORDER=", "").Trim() ?? "";
            info._SOCIETE = omaStrArr.Where(o => o.Contains("_PROCESS="))?.FirstOrDefault()?.Replace("_PROCESS=", "").Trim() ?? "";
            info._REFERENCE = omaStrArr.Where(o => o.Contains("_REFERENCE="))?.FirstOrDefault()?.Replace("_REFERENCE=", "").Trim() ?? "";
            info._RUCHER = omaStrArr.Where(o => o.Contains("_RUCHER="))?.FirstOrDefault()?.Replace("_RUCHER=", "").Trim() ?? "";
            info._NUMCLI = omaStrArr.Where(o => o.Contains("_NUMCLI="))?.FirstOrDefault()?.Replace("_NUMCLI=", "").Trim() ?? "";
            info._FNCLIENT = omaStrArr.Where(o => o.Contains("_FNovacelLIENT="))?.FirstOrDefault()?.Replace("_FNovacelLIENT=", "").Trim() ?? "";
            info._CITY = omaStrArr.Where(o => o.Contains("_CITY="))?.FirstOrDefault()?.Replace("_CITY=", "").Trim() ?? "";
            info._COMMENT = omaStrArr.Where(o => o.Contains("_COMMENT="))?.FirstOrDefault()?.Replace("_COMMENT=", "").Trim() ?? "";
            info._SUPPL = omaStrArr.Where(o => o.Contains("_SUPPL="))?.FirstOrDefault()?.Replace("_SUPPL=", "").Trim() ?? "";
            info._PORTEUR = omaStrArr.Where(o => o.Contains("_PORTEUR="))?.FirstOrDefault()?.Replace("_PORTEUR=", "").Trim() ?? "";
            info._MATRIX = omaStrArr.Where(o => o.Contains("_MATRIX="))?.FirstOrDefault()?.Replace("_MATRIX=", "").Trim() ?? "";
            info._LOTNUMBER = omaStrArr.Where(o => o.Contains("_LOTNUMBER="))?.FirstOrDefault()?.Replace("_LOTNUMBER=", "").Trim() ?? "";
            info._MONT = omaStrArr.Where(o => o.Contains("_MONT="))?.FirstOrDefault()?.Replace("_MONT=", "").Trim() ?? "";
            info.MBASE = omaStrArr.Where(o => o.Contains("MBASE="))?.FirstOrDefault()?.Replace("MBASE=", "").Trim() ?? "";

            //比桥的开孔数据
            info.DRILLE = string.Join("\n", omaStrArr.Where(o => o.Contains("DRILLE=") || o.Contains("DIRLL="))?.ToArray());

            //斜角
            info.FPINB = omaStrArr.Where(o => o.Contains("FPINB="))?.FirstOrDefault()?.Replace("FPINB=", "").Trim() ?? "";
            info.PINB = omaStrArr.Where(o => o.Contains("PINB="))?.FirstOrDefault()?.Replace("PINB=", "").Trim() ?? "";
            info.BEVP = omaStrArr.Where(o => o.Contains("BEVP="))?.FirstOrDefault()?.Replace("BEVP=", "").Trim() ?? "";
            info.BEVM = omaStrArr.Where(o => o.Contains("BEVM="))?.FirstOrDefault()?.Replace("BEVM=", "").Trim() ?? "";

            //开槽的深度和宽度
            info.GDEPTH = omaStrArr.Where(o => o.Contains("GDEPTH="))?.FirstOrDefault()?.Replace("GDEPTH=", "").Trim() ?? "";
            info.GWIDTH = omaStrArr.Where(o => o.Contains("GWIDTH="))?.FirstOrDefault()?.Replace("GWIDTH=", "").Trim() ?? "";

            //可能有
            info.CIRC = omaStrArr.Where(o => o.Contains("CIRC="))?.FirstOrDefault()?.Replace("CIRC=", "").Trim() ?? "";
            info.DBL = omaStrArr.Where(o => o.Contains("DBL="))?.FirstOrDefault()?.Replace("DBL=", "").Trim() ?? "0";

            //车边方式
            info.ETYP = omaStrArr.Where(o => o.Contains("ETYP="))?.FirstOrDefault()?.Replace("ETYP=", "").Trim() ?? "";
            info.FTYP = omaStrArr.Where(o => o.Contains("FTYP="))?.FirstOrDefault()?.Replace("FTYP=", "").Trim() ?? "";

            info._LCOAT = omaStrArr.Where(o => o.Contains("_LCOAT="))?.FirstOrDefault()?.Replace("_LCOAT=", "").Trim() ?? "";
            info.BSIZ = omaStrArr.Where(o => o.Contains("BSIZ="))?.FirstOrDefault()?.Replace("BSIZ=", "").Trim() ?? "";
            info.LMATTYPE = omaStrArr.Where(o => o.Contains("LMATTYPE="))?.FirstOrDefault()?.Replace("LMATTYPE=", "").Trim() ?? "";
            info.POLISH = omaStrArr.Where(o => o.Contains("POLISH="))?.FirstOrDefault()?.Replace("POLISH=", "").Trim() ?? "";
            info.FPD = omaStrArr.Where(o => o.Contains("FPD="))?.FirstOrDefault()?.Replace("FPD=", "").Trim() ?? "";
            info.MPD = omaStrArr.Where(o => o.Contains("MPD="))?.FirstOrDefault()?.Replace("MPD=", "").Trim() ?? "0";
            info.FBFCIN = omaStrArr.Where(o => o.Contains("FBFCIN="))?.FirstOrDefault()?.Replace("FBFCIN=", "").Trim() ?? "";
            info.FBFCUP = omaStrArr.Where(o => o.Contains("FBFCUP="))?.FirstOrDefault()?.Replace("FBFCUP=", "").Trim() ?? "";
            info._FBFCANG = omaStrArr.Where(o => o.Contains("_FBFCANG="))?.FirstOrDefault()?.Replace("_FBFCANG=", "").Trim() ?? "";

            //可能有棱镜
            info.PRVA = omaStrArr.Where(o => o.Contains("PRVA="))?.FirstOrDefault()?.Replace("PRVA=", "").Trim() ?? "";
            info.PRVM = omaStrArr.Where(o => o.Contains("PRVM="))?.FirstOrDefault()?.Replace("PRVM=", "").Trim() ?? "";
            info._PRVA1 = omaStrArr.Where(o => o.Contains("_PRVA1="))?.FirstOrDefault()?.Replace("_PRVA1=", "").Trim() ?? "";
            info._PRVA2 = omaStrArr.Where(o => o.Contains("_PRVA2="))?.FirstOrDefault()?.Replace("_PRVA2=", "").Trim() ?? "";
            info._PRVM1 = omaStrArr.Where(o => o.Contains("_PRVM1="))?.FirstOrDefault()?.Replace("_PRVM1=", "").Trim() ?? "";
            info._PRVM2 = omaStrArr.Where(o => o.Contains("_PRVM2="))?.FirstOrDefault()?.Replace("_PRVM2=", "").Trim() ?? "";

            //HBOX2
            var hboxArr = omaStrArr.Where(o => o.Contains("HBOX="))?.ToArray();
            if (hboxArr.Any())
            {
                if (hboxArr.Length == 2)
                    info.HBOX2 = hboxArr[1].Replace("HBOX=", "").Trim();
                else
                    info.HBOX2 = hboxArr[0].Replace("HBOX=", "").Trim();
            }
            info.MINEDG = omaStrArr.Where(o => o.Contains("MINEDG="))?.FirstOrDefault()?.Replace("MINEDG=", "").Trim() ?? "";
            if (!string.IsNullOrEmpty(info.MINEDG))
            {
                if (info.MINEDG.Split(';')[0][0] == '.')
                {
                    info.MINEDG = "0" + info.MINEDG.Split(';')[0] + ";" + info.MINEDG.Split(';')[1];
                }
                if (info.MINEDG.Split(';')[1][0] == '.')
                {
                    info.MINEDG = info.MINEDG.Split(';')[0] + ";0" + info.MINEDG.Split(';')[1];
                }
            }

            return info;
        }
    }
}
