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
    public partial class CheckBoxAndRadioButton : Form
    {
        public CheckBoxAndRadioButton()
        {
            InitializeComponent();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            //直接获取this.chk1.Text
            //循环
            foreach (Control c in this.panel3.Controls)
            {
                //Control是所有控件的基类（父类）
                //使用as 类型转换不会引发异常，null
                CheckBox cb = c as CheckBox;//引用类型的(兼容性)转换
                if (cb.Checked)
                {
                    MessageBox.Show(cb.Text + "被选中啦");
                }

            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            //this.Close();
            Application.Exit();//关闭整个应用程序
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //自定义系统
            //如果你的界面需要从数据库数据做参考，来动态创建
            RadioButton rb1 = new RadioButton();
            rb1.Text = "测试按钮";
            this.Controls.Add(rb1);
        }
    }
}
