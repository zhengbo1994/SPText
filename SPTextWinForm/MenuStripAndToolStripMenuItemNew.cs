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
    public partial class MenuStripAndToolStripMenuItemNew : Form
    {
        public MenuStripAndToolStripMenuItemNew()
        {
            InitializeComponent();
        }

        private void MenuStripAndToolStripMenuItemNew_Load(object sender, EventArgs e)
        {
        }

        private void 新增学员ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddStudentForm asf = new AddStudentForm();
            asf.MdiParent = this;//将创建的窗体 的父窗体设置为this
            asf.Show();//显示
            //asf.ShowDialog();//不允许的
        }
    }
}
