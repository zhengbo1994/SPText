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
    public partial class ListView : Form
    {
        public ListView()
        {
            InitializeComponent();
        }

        private void ListView_Load(object sender, EventArgs e)
        {
            List<UserInfo> list = GetList();
            foreach (UserInfo userInfo in list)
            {
                ListViewItem item = new ListViewItem();//创建数据项(条)
                item.Text = userInfo.UserNo.ToString();
                //item.SubItems.Add(userInfo.NickName)
                item.SubItems.AddRange(new string[] { userInfo.NickName, userInfo.Age.ToString() });
                item.ImageIndex = userInfo.ImageIndex;
                this.lvFriends.Items.Add(item);//添加到控件
            }
        }

        private List<UserInfo> GetList()
        {
            List<UserInfo> list = new List<UserInfo>()
            {
                new UserInfo(){ Age=19, ImageIndex=0, NickName="张三", UserNo=1001 },
                 new UserInfo(){ Age=29, ImageIndex=1, NickName="李四", UserNo=1001 },
                  new UserInfo(){ Age=32, ImageIndex=2, NickName="王五", UserNo=1001 }
            };
            return list;
        }

        private void 大图标ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tsmi = sender as ToolStripMenuItem;
            switch (tsmi.Tag.ToString())
            {
                case "1":
                    this.lvFriends.View = View.LargeIcon;
                    break;
                case "2":
                    this.lvFriends.View = View.SmallIcon;
                    break;
                case "3":
                    this.lvFriends.View = View.Details;
                    break;
                case "4":
                    this.lvFriends.View = View.List;
                    break;
                case "5":
                    this.lvFriends.View = View.Tile;
                    break;
            }
        }
    }

    /// <summary>
    /// 用户信息类
    /// </summary>
    public class UserInfo
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int UserNo { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        public int Age { get; set; }
        /// <summary>
        /// 图片
        /// </summary>
        public int ImageIndex { get; set; }
    }
}
