using SPTextCommon;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace SPTextWinForm
{
    public partial class ExcelDataImport : Form
    {
        public ExcelDataImport()
        {
            InitializeComponent();
        }

        //开始
        private void btnStart_Click(object sender, EventArgs e)
        {
            if (this.dataSource.Text.Trim().Length == 0)
            {
                MessageBox.Show("请设置要导入的文件");
                return;
            }
            var tasks = new List<Task<DataTable>>();

            if (dataSource.Text.Trim().Length > 0)
            {
                tasks.Add(
                    Task.Factory.StartNew(() =>
                    {
                        WriteLog("开始提取数据");
                        var dt = GetDataSource();
                        WriteLog("数据有" + dt.Rows.Count + "条");
                        return dt;
                    }));
            }

            var th = new Thread(new ParameterizedThreadStart(obj =>
            {
                Task.WaitAll(tasks.ToArray());
                this.BeginInvoke((MethodInvoker)(() =>
                {
                    //WriteLog("所有数据提取完毕，一共有" + total + "条");
                    //progressBar1.Maximum = total;
                    progressBar1.Visible = true;
                    lblStatus.Visible = true;
                }));
                var current = (int)obj;
                try
                {
                    var watch = new System.Diagnostics.Stopwatch();
                    watch.Start();
                }
                catch (Exception ex)
                {
                    WriteLog("数据导入发生错误:" + ex.Message);
                }

            }));
            th.IsBackground = true;
            th.Start(0);
        }

        //结束
        private void btnStop_Click(object sender, EventArgs e)
        {

        }





        //文件打开
        private void OpenFileDialogShow()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            ofd.DefaultExt = "xls";
            ofd.Filter = "Excel 文件|*.xls";
            //ofd.FileName = "";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                this.dataSource.Text = ofd.FileName;
            }
        }

        //文件保存
        private void SaveFileDialogShow()
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.DefaultExt = "xls";
            saveDialog.Filter = "Excel 文件|*.xls";
            //saveDialog.FileName = saveDialog.SafeFileName.Split('.')[0] + "_Convert";
            if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.dataSource.Text = saveDialog.FileName;
            }
        }


        private void WriteLog(string s)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)(() =>
                {
                    rchtxtLog.AppendText(string.Format("{0} {1}\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), s));
                }));
            }
            else
            {
                rchtxtLog.AppendText(string.Format("{0} {1}\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), s));
            }
        }


        private DataTable GetDataSource()
        {
            DataTable dt = new DataTable();
            var db = new DBHelperExcel();
            DataSet ds = db.GetDataSet(this.dataSource.Text);
            if (ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
            }
            return dt;
        }
    }
}

