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
    public partial class TabControlAndToolStripMenuItem : Form
    {
        public TabControlAndToolStripMenuItem()
        {
            InitializeComponent();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //窗体变成正常大小
            this.WindowState = FormWindowState.Normal;//还原
            this.Activate();//激活窗体
            this.ShowInTaskbar = true;//隐藏工具栏图标
            this.notifyIcon1.Visible = false;//托盘显示
        }

        private void 关闭ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();//关闭应用程序
        }

        private void TabControlAndToolStripMenuItem_Load(object sender, EventArgs e)
        {
            //Utility.AddWater("d:/1.png", "d:/2.png", "美女");//加水印
        }

        private void TabControlAndToolStripMenuItem_SizeChanged(object sender, EventArgs e)
        {
            //如果窗体变小
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.ShowInTaskbar = false;//隐藏工具栏图标
                this.notifyIcon1.Visible = true;//托盘显示
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {

        }
    }
}
