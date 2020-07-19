using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SPTextWinForm.DataGridView
{
    public partial class AddProductForm : Form
    {
        public AddProductForm()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            //获取数据
            string typeid = this.cboProductType.SelectedValue.ToString();//获取typeid
            string productName = this.txtProductName.Text.Trim();
            string price = this.txtPrice.Text;
            string imageUrl = this.txtImageUrl.Text.Trim();
            string description = this.txtDescription.Text.Trim();
            if (typeid == "0")
            {
                MessageBox.Show("请选择正确的类型!");
                return;
            }
            string sql = @"insert into Product(ProductName, Price, ImageUrl, Description, TypeId)
                        values(@ProductName, @Price, @ImageUrl,@Description, @TypeId)";
            SqlParameter[] paras =
                {
                 new SqlParameter("@ProductName",productName),
                 new SqlParameter("@Price",price),
                 new SqlParameter("@ImageUrl",imageUrl),
                 new SqlParameter("@TypeId",typeid),
                 new SqlParameter("@Description",description)
            };
            int result = DBHelper.ExecuteNonQuery(sql, paras);
            if (result > 0)
            {
                //form2.GetProduct();//刷新
                if (UpdateData != null)
                    UpdateData();

                MessageBox.Show("添加成功！");
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {

        }

        //event是委托的实例,委托事件
        public event Action UpdateData;//定义一个数据更新的委托事件

        private void AddProductForm_Load(object sender, EventArgs e)
        {
            //初始化下拉框
            string sql = "select typeid,typename from ProductType";
            DataTable dt = DBHelper.GetDataTable(sql);
            //临时表添加新行
            DataRow row = dt.NewRow();//内存中数据，没有结构
            row["typeid"] = 0;
            row["typename"] = "--请选择--";
            dt.Rows.InsertAt(row, 0);//添加到末尾

            this.cboProductType.ValueMember = "typeid";
            this.cboProductType.DisplayMember = "typename";
            this.cboProductType.DataSource = dt;//绑定数据源
        }
    }
}
