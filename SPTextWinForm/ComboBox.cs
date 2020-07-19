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
    public partial class ComboBox : Form
    {
        public ComboBox()
        {
            InitializeComponent();
        }

        private void btnReader_Click(object sender, EventArgs e)
        {


            //数据保存都是编号
            //用户表中，存放都是城市编号
            //MessageBox.Show(this.cboCity.SelectedValue.ToString());
            if (this.cboCity.SelectedValue.ToString() == "0")
            {
                MessageBox.Show("请选择需要购票的城市!");
            }
        }

        /// <summary>
        /// 查询城市数据
        /// </summary>
        /// <returns></returns>
        private List<City> GetList()
        {
            List<City> list = new List<City>()
            {
                new City(){ CityNo=1, CityName="北京" },
                 new City(){ CityNo=2, CityName="武汉" },
                  new City(){ CityNo=3, CityName="上海" }
            };
            return list;
        }

        private void cboCity_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        /// <summary>
        /// 城市模型类
        /// </summary>
        public class City
        {
            /// <summary>
            /// 编号
            /// </summary>
            public int CityNo { get; set; }
            /// <summary>
            /// 名称
            /// </summary>
            public string CityName { get; set; }
        }

        /// <summary>
        /// 窗体打开加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form3_Load_1(object sender, EventArgs e)
        {
            #region 手动添加信息
            //index 从0开始
            //MessageBox.Show(this.cboCity.Items[1].ToString());
            //获取当前选中的值
            //MessageBox.Show(this.cboCity.Text);
            //for (int i = 0; i < 10; i++)
            //{
            //    this.cboCity.Items.Add(i);
            //}
            //手动循环添加
            #endregion

            #region 数据源绑定方式
            //绑定数据源方式，3个属性需要赋值
            List<City> list = GetList();
            list.Insert(0, new City() { CityName = "--请选择--", CityNo = 0 });

            this.cboCity.ValueMember = "CityNo";//value隐藏值
            this.cboCity.DisplayMember = "CityName";//Display显示
            this.cboCity.DataSource = list;//数据源 DataTable List<City>
                                           ////手动添加
                                           //this.cboCity.Items.Add("重庆");

            #endregion
        }
    }
}
