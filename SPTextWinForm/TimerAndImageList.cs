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
    public partial class TimerAndImageList : Form
    {
        public TimerAndImageList()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimerAndImageList_Load(object sender, EventArgs e)
        {

        }

        int i = 0;//初始值
        /// <summary>
        /// 开始启动之后，每隔xx事件执行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            //集合或数组  count length
            if (i < this.imgUserList.Images.Count)
            {
                this.pictureBox1.Image = this.imgUserList.Images[i];//第三个图片
                i++;
            }
            else
            {
                i = 0;//重新初始化
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            this.timer1.Stop();//停止 false
        }

        private void btnStart_Click_1(object sender, EventArgs e)
        {
            this.timer1.Start(); //启动
            // this.Enabled = true; false
        }
    }
}
