using SPTextWinForm.Public;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SPTextWinForm
{
    public partial class JDOrderSplitFlow : Form
    {
        List<OrderAddress> AlloAList = new List<OrderAddress>();

        public static string startupPath = AppDomain.CurrentDomain.BaseDirectory;
        public static readonly string settingPath = AppDomain.CurrentDomain.BaseDirectory + "config\\set.xml";
        DatabaseSetSettingInfo db = null;
        private string strLanguageChoice = null;
        private JDScanOrder jdScanOrder = new JDScanOrder();


        public JDOrderSplitFlow()
        {
            InitialConfiguration initialConfiguration = new InitialConfiguration();
            db = initialConfiguration.GetDatabaseSetSettingInfo(settingPath);
            strLanguageChoice = initialConfiguration.GetLanguageChoice(settingPath);
            InitializeComponent();
        }

        /// <summary>
        /// 流水号输入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtAddGoodsByNum_TextChanged(object sender, EventArgs e)
        {
            if (txtAddGoodsByNum.Text.Trim().Length >= 12)
            {
                string num = txtAddGoodsByNum.Text.Trim().ToUpper();
                if (AlloAList.Count > 0)
                {
                    for (int i = 0; i < AlloAList.Count; i++)
                    {
                        if (num == AlloAList[i].Sequence_Num)
                        {
                            jdScanOrder.VoicePlay("重复扫入", strLanguageChoice);
                            MessageBox.Show("不能重复添加!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            txtAddGoodsByNum.Text = "";
                            return;
                        }
                    }
                }

                bool b = jdScanOrder.InspectSequenceNum(num, db);
                if (b)
                {
                    jdScanOrder.SearchAndCreate(num, db, this.dgvGoodsTable, strLanguageChoice, ref AlloAList);
                    txtAddGoodsByNum.Text = "";
                    txtAddGoodsByNum.Focus();
                }
                else
                {
                    MessageBox.Show(string.Format("{0}该流水号有错误，请检查", num));
                    txtAddGoodsByNum.Focus();
                    return;
                }
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (AlloAList.Count == 0)
            {
                MessageBox.Show("没有数据，无法删除");
                return;
            }

            string crn = this.CRN_TextBox.Text;
            if (string.IsNullOrEmpty(crn))
            {
                if (this.radioCRN.Checked)
                {
                    MessageBox.Show("请输入分箱编号！");
                    return;
                }
                else
                {
                    MessageBox.Show("请输入流水号！");
                    return;
                }
            }
            else
            {
                if (this.radioCRN.Checked)
                {
                    if (jdScanOrder.IsNumeric(crn))
                    {
                        jdScanOrder.DeleteButton(crn, true, db, this.dgvGoodsTable, ref AlloAList);

                    }
                    else
                    {
                        MessageBox.Show("你输入的不是数字类型，请检查！");
                        return;
                    }
                }
                else
                {
                    jdScanOrder.DeleteButton(crn, false, db, this.dgvGoodsTable, ref AlloAList);
                }
            }
        }

        /// <summary>
        /// 恢复上次
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRecoverData_Click(object sender, EventArgs e)
        {
            try
            {
                jdScanOrder.RenewLastData(db, dgvGoodsTable, ref AlloAList);
                MessageBox.Show("订单已恢复上次修改记录！");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("确定要导出？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (DialogResult.Yes == dialogResult)
            {
                if (AlloAList.Count == 0)
                {
                    MessageBox.Show("表格没有数据，请插入数据再导出");
                    return;
                }

                string filePath = string.Empty;
                string excelEnclosureFile = Path.Combine(startupPath, "ExcelEnclosure");//模板文件
                if (!Directory.Exists(excelEnclosureFile))
                {
                    Directory.CreateDirectory(excelEnclosureFile);
                }

                filePath = Path.Combine(excelEnclosureFile, "SplitFlow_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
                List<OrderAddress> oAList = AlloAList.OrderBy(p => p.ZoneItemsId).OrderByDescending(p => p.Sequence_Num).ToList();
                jdScanOrder.Excelexport(oAList, filePath);
                MessageBox.Show("Excel生成成功");
            }
        }
        /// <summary>
        /// 清空列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClearWhiteOrder_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("确定要清空列表？", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (DialogResult.Yes == dialogResult)
            {
                if (dgvGoodsTable.Rows.Count != 0)
                {
                    dgvGoodsTable.Rows.Clear();
                    AlloAList.Clear();
                    MessageBox.Show("已清空");
                }
                else
                {
                    MessageBox.Show("没有数据可供清空");
                }
            }
        }
        /// <summary>
        /// 清空重做
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("确定要清空重做吗？", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
            if (DialogResult.Yes == dialogResult)
            {
                if (dgvGoodsTable.Rows.Count != 0)
                {
                    dgvGoodsTable.Rows.Clear();
                    AlloAList.Clear();
                    string sqlDelete = "TRUNCATE TABLE [ARO].[dbo].[NA319_OrderSplitFlow]";
                    SqlHelper sqlHelper = new SqlHelper(db.dbHost, db.dbName, db.dbUser, db.dbPwd);
                    int iValue = sqlHelper.ExecuteNonQuery(sqlDelete, new SqlParameter[] { });
                    MessageBox.Show("已清空");
                }
                else
                {
                    MessageBox.Show("没有数据可供清空");
                }
            }
        }

        /// <summary>
        /// 恢复上次数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmtRenewLastData_Click(object sender, EventArgs e)
        {
            try
            {
                jdScanOrder.RenewLastData(db, dgvGoodsTable, ref AlloAList);
                MessageBox.Show("订单已恢复上次修改记录！");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
