using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SPTextWinForm
{
    public partial class ProgressBarFrom : Form
    {
        private delegate void SetPos(int ipos, string vinfo, int max);//代理
        public ProgressBarFrom()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread fThread = new Thread(new ParameterizedThreadStart(o => this.SleepT(100)));
            fThread.Start();
        }



        //定义新的线程执行函数
        private void SleepT(int onum)
        {
            int num = int.Parse(onum.ToString());
            int numMax = num + 1;
            for (int i = 0; i <= numMax; i++)   //滚动条最大值为500
            {
                if (i < numMax)
                {
                    System.Threading.Thread.Sleep(10);
                    SetTextMsg(i, i.ToString() + "\r\n", num);
                }
                else
                {
                    MessageBox.Show("读取完毕");
                }

            }
        }

        //进度条值更新函数（参数必须跟声明的代理参数一样）
        private void SetTextMsg(int ipos, string vinfo, int max)
        {
            if (this.InvokeRequired)   //InvokeRequired属性为真时，说明一个创建它以以外的线程(即SleepT)想访问它
            {
                SetPos setpos = new SetPos(SetTextMsg);
                this.Invoke(setpos, new object[] { ipos, vinfo, max });//SleepT线程调用本控件Form1中的方法
            }
            else
            {
                this.richTextBox1.Text = ipos.ToString() + $"/{max}";
                this.progressBar1.Value = Convert.ToInt32(ipos);
            }
        }
    }
}
