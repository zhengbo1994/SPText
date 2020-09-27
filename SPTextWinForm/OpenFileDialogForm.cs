using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using static System.Net.Mime.MediaTypeNames;
using System.Drawing.Drawing2D;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.HSSF.UserModel;
using SPText.Common;

namespace SPTextWinForm
{
    public partial class OpenFileDialogForm : Form
    {
        public OpenFileDialogForm()
        {
            InitializeComponent();
        }

        private void btnFile_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Multiselect = false;
                ofd.DefaultExt = "xls";
                ofd.Filter = "Excel 文件|*.xls";
                ofd.FileName = "";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    this.labBeginPath.Text = ofd.FileName;
                    FileInfo fileInfo = new FileInfo(ofd.FileName);
                    Stream fs = fileInfo.Open(FileMode.Open);
                    fs.Position = 0;
                    DataSet ds = new DataSet();
                    ds = NOPIHelper.RenderDataSetFromExcel(fs, 0);
                    {
                        SaveFileDialog saveDialog = new SaveFileDialog();
                        saveDialog.DefaultExt = "xls";
                        saveDialog.Filter = "Excel 文件|*.xls";
                        saveDialog.FileName = ofd.SafeFileName.Split('.')[0] + "_Convert";
                        if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            this.labEndPath.Text = saveDialog.FileName;
                            #region NOPI导出
                            HSSFWorkbook wk;
                            wk = new HSSFWorkbook();   //把xls文件中的数据写入wk中

                            #region  【后8列】对样式进行设置，不能超过4000个样式
                            //字体样式调整
                            ICellStyle cellStyle = wk.CreateCellStyle();
                            cellStyle.VerticalAlignment = VerticalAlignment.Center;//垂直对齐
                            cellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;//水平对齐    
                            cellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin; //下边框
                            cellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;//上边框
                            cellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;//左边框
                            cellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;//右边框
                            //cellStyle.WrapText = true;//换行

                            HSSFFont font = (HSSFFont)wk.CreateFont();
                            font.FontHeightInPoints = 11;//字體尺寸
                            font.FontName = "新細明體";//字體尺寸
                            #endregion

                            for (int m = 0; m < ds.Tables.Count; m++)
                            {
                                ISheet sheets = wk.CreateSheet(ds.Tables[m].TableName);
                                DataTable dt = ds.Tables[m];
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    if (i <= 8)
                                    {
                                        IRow row = sheets.CreateRow(i);
                                        string strCell_Row2 = "";
                                        for (int j = 0; j < dt.Columns.Count; j++)
                                        {
                                            try
                                            {
                                                ICell cell = row.CreateCell(j);//模板的多少行数据
                                                cell.CellStyle.SetFont(font);
                                                string strValue = dt.Rows[i][j].ToString();
                                                if (j == 1)
                                                {
                                                    strCell_Row2 = strValue;
                                                    strValue = "";
                                                    cell.SetCellValue(strCell_Row2);
                                                }
                                                else if (j == 5)
                                                {
                                                    if (i == 7)
                                                    {
                                                        sheets.AddMergedRegion(new CellRangeAddress(i, i, 0, 5 + 5));
                                                    }
                                                    else
                                                    {
                                                        if (i == 0 || i == 6)
                                                        {

                                                        }
                                                        else
                                                        {
                                                            sheets.AddMergedRegion(new CellRangeAddress(i, i, 5, 5 + 5));
                                                        }
                                                        //sheets.AddMergedRegion(new CellRangeAddress(i, i, 5, 5 + 5));
                                                    }

                                                    //sheets.AddMergedRegion(new CellRangeAddress(i, i, 8, 8 + 7));
                                                    cell.SetCellValue(strCell_Row2);
                                                }
                                                else
                                                {
                                                    //特殊处理：1.加载到数据的最后一列 2.数据的长度小于I（8）列
                                                    if (dt.Columns.Count - 1 == j && dt.Columns.Count <= 5 && j < 5)
                                                    {
                                                        {//没有到达I行，填充最后一行
                                                            if (i == 7)
                                                                sheets.AddMergedRegion(new CellRangeAddress(i, i, 0, 0 + 4));
                                                            else
                                                                sheets.AddMergedRegion(new CellRangeAddress(i, i, 0, 0 + 4));
                                                            //sheets.AddMergedRegion(new CellRangeAddress(i, i, 0, 0 + 7));
                                                            cell.SetCellValue(strValue);
                                                        }
                                                        {//没有到达I行，进行创建之后再进行填充
                                                            ICell cell8 = row.CreateCell(5);//模板的多少行数据
                                                            if (i == 7)
                                                            {
                                                                sheets.AddMergedRegion(new CellRangeAddress(i, i, 0, 5 + 5));
                                                                cell8.SetCellValue(strCell_Row2);
                                                            }
                                                            else
                                                            {
                                                                if (i == 0 || i == 6)
                                                                {

                                                                }
                                                                else
                                                                {
                                                                    sheets.AddMergedRegion(new CellRangeAddress(i, i, 5, 5 + 5));
                                                                }
                                                            }

                                                            //sheets.AddMergedRegion(new CellRangeAddress(i, i, 8, 8 + 7));
                                                            cell8.SetCellValue(strCell_Row2);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (i == 7)
                                                        {
                                                            sheets.AddMergedRegion(new CellRangeAddress(i, i, 0, 5 + 5));
                                                        }
                                                        else
                                                        {
                                                            sheets.AddMergedRegion(new CellRangeAddress(i, i, 0, 0 + 4));
                                                        }

                                                        cell.SetCellValue(strValue);
                                                    }

                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                throw ex;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        try
                                        {
                                            IRow row = sheets.CreateRow(i);

                                            for (int j = 0; j < dt.Columns.Count; j++)
                                            {
                                                if (j >= 1)
                                                {
                                                    sheets.SetColumnWidth(j, 6 * 256);
                                                }

                                                try
                                                {
                                                    ICell cell = row.CreateCell(j);//模板的多少行数据
                                                    cellStyle.SetFont(font);
                                                    cell.CellStyle = cellStyle;
                                                    string strValue = dt.Rows[i][j].ToString();
                                                    cell.SetCellValue(strValue);
                                                }
                                                catch (Exception ex)
                                                {
                                                    throw ex;
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {

                                            throw ex;
                                        }
                                    }
                                }
                                sheets.HorizontallyCenter = true;//水平居中
                                sheets.VerticallyCenter = true;//垂直居中
                                sheets.ForceFormulaRecalculation = true;//力公式重新计算
                            }

                            using (FileStream fs0 = System.IO.File.OpenWrite(saveDialog.FileName))
                            {
                                wk.Write(fs0);
                                MessageBox.Show("导出成功", "提示", MessageBoxButtons.OK);
                            }
                            #endregion
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region 上传图片 并生成缩略图
        /// <summary>
        /// 上传图片生成缩略图
        /// </summary>
        /// <param name="originalImagePath">图片源路径</param>
        /// <param name="thumbnailPath">缩略图路径(物理路径)</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="mode">生成缩略图的方式</param>
        public static void MakeThumbnail(string originalImagePath, string thumbnailPath, int width, int height, string mode)
        {
            
        }
        #endregion
    }
}
