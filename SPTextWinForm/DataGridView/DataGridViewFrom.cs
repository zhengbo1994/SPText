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
    public partial class DataGridViewFrom : Form
    {
        public DataGridViewFrom()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetProduct();
        }
        private SqlDataAdapter adapter;//保存查询时的适配器
        public void GetProduct()
        {
            //ADO.NET操作数据库
            string sql = @"select ProductNo, ProductName, Price, ImageUrl, TypeId
                from Product";
            adapter = DBHelper.GetDataAdapter(sql);
            DataTable dt = new DataTable();
            adapter.Fill(dt);//数据填充
            this.dgvProducts.DataSource = dt;
            //产品编号 乱码
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            //把控件的数据源，转换为临时表
            DataTable dt = this.dgvProducts.DataSource as DataTable;
            SqlCommandBuilder scb = new SqlCommandBuilder(adapter);//sqlserver的字段关联
            adapter.Update(dt);//执行 添加、修改、删除
            MessageBox.Show("操作成功", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk);
        }

        private void dgvProducts_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            //固定按键，在系统中都是有枚举值的
            if (e.Button == MouseButtons.Right)//判断如果是鼠标右键
            {
                this.dgvProducts.ClearSelection();//清除选中
                this.dgvProducts.Rows[e.RowIndex].Selected = true;//手动选中
                //设置右键菜单在屏幕中的位置
                //this.contextMenuStrip1.Show(MousePosition.X, MousePosition.Y);
                //TreeView
            }
        }

        private void tsmiDelete_Click(object sender, EventArgs e)
        {
            //反悔，添加记录历史集合
            //获取选中需要删除的数据行
            DataGridViewRow row = this.dgvProducts.SelectedRows[0];//默认选中的第一行
            if (!row.IsNewRow)//判断如果不是新行，就删除
            {
                this.dgvProducts.Rows.Remove(row);
            }
        }

        private void 添加ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddProductForm apf = new AddProductForm();//this
            apf.UpdateData += Apf_UpdateData;//注册委托事件
            apf.Show();//线程阻塞,后面的代码暂停
        }

        private void Apf_UpdateData()
        {
            GetProduct();
        }

        private void DataGridViewFrom_Load(object sender, EventArgs e)
        {
            //初始化下拉框
            string sql = "select typeid,typename from ProductType";
            DataGridViewComboBoxColumn cbo = this.dgvProducts.Columns["TypeId"]
                as DataGridViewComboBoxColumn;//找控件
            cbo.ValueMember = "typeid";
            cbo.DisplayMember = "typename";
            cbo.DataSource = DBHelper.GetDataTable(sql);//绑定数据源
        }
        /// <summary>
        /// 自动编号（正序）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvProducts_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            //自动编号，与数据无关
            //Rectangle rectangle = new Rectangle(e.RowBounds.Location.X, e.RowBounds.Location.Y, dataGVScanShipment.RowHeadersWidth - 4, e.RowBounds.Height);
            //TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(), dataGVScanShipment.RowHeadersDefaultCellStyle.Font,
            //    rectangle, dataGVScanShipment.RowHeadersDefaultCellStyle.ForeColor, TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }
        /// <summary>
        /// 自动编号（倒序）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvProducts_Paint(object sender, PaintEventArgs e)
        {
            int rowcount = dgvProducts.RowCount;
            int showcount = dgvProducts.DisplayedRowCount(true);
            if (showcount == 0) return;
            System.Drawing.Rectangle currrct;
            int startNo = dgvProducts.FirstDisplayedCell.RowIndex;
            int ColNo = dgvProducts.FirstDisplayedCell.ColumnIndex;
            string stext = "";
            int nowy = 0;
            int hDelta = 0;
            for (int i = startNo; i < startNo + showcount; i++)
            {
                currrct = (System.Drawing.Rectangle)dgvProducts.GetCellDisplayRectangle(ColNo, i, true);
                nowy = currrct.Y + 2;
                stext = string.Format("{0, 3}", rowcount - i);
                if (hDelta == 0)
                    hDelta = (currrct.Height - dgvProducts.Font.Height) / 2;
                if (dgvProducts.Rows[i].Selected == true)
                    e.Graphics.DrawString(stext, dgvProducts.Font, new System.Drawing.SolidBrush(System.Drawing.Color.White), 10, nowy + hDelta);
                else
                    e.Graphics.DrawString(stext, dgvProducts.Font, new System.Drawing.SolidBrush(System.Drawing.Color.Black), 10, nowy + hDelta);
            }
        }
    }
}
