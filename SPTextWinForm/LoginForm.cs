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
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }


        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                //获取到界面信息
                int qq = int.Parse(this.txtQQ.Text.Trim());//去两边空格
                string password = this.txtPassword.Text.Trim();
                if (qq == 1 && password == "123")//查询数据库
                {
                    //if (this.chkPwd.Checked)
                    //{
                    //    //IO记住密码
                    //}
                    //MessageBox.Show("登录成功!");
                    CheckBoxAndRadioButton mainForm = new CheckBoxAndRadioButton();
                    mainForm.Show();//普通显示
                                    //mainForm.ShowDialog();//模式窗体
                                    //后面的代码不会执行,当你关闭模式窗体之后才会执行
                                    // this.Hide();//隐藏 方法 this.Show()//显示
                                    //this.Visible = false;//隐藏 属性 true显示
                }
                else
                {
                    LogHelper.DeBug($"登录失败!");
                    MessageBox.Show("登录失败!");
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error($"删除错误!{ex.StackTrace}", new Exception());
                throw;
            }

        }

        /// <summary>
        /// 关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            //删除(关闭)给一个反悔的机会
            DialogResult result = MessageBox.Show("确定要关闭应用程序?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Close();//关闭(当前窗体)应用程序
                //Application.Exit();//关闭整个应用程序
            }

        }
    }
}
