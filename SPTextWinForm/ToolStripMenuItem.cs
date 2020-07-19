using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SPTextWinForm
{
    public partial class ToolStripMenuItem : Form
    {
        public ToolStripMenuItem()
        {
            InitializeComponent();
        }

        private void tsmiExport_Click(object sender, EventArgs e)
        {
            //保存文件对象
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Excel|*.xls|Excel|*.xlsx";
            DialogResult result = sfd.ShowDialog();
            if (result == DialogResult.OK)
            {
                //获取临时表信息
                DataTable dt = this.dgvDatas.DataSource as DataTable;
                //多次调用导出Excel
                ExcelHelper.TableToExcel(dt, sfd.FileName);
            }
        }

        private void tsmiImoprt_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            //ofd.Multiselect = true;//允许选中多个文件
            ofd.Filter = "Excel|*.xls|Excel|*.xlsx";
            DialogResult result = ofd.ShowDialog();//显示选择文件的模式窗体
            if (result == DialogResult.OK)
            {
                this.dgvDatas.DataSource = ExcelHelper.ExcelToTable(ofd.FileName);//临时表和集合
            }
        }
    }
}
