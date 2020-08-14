using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SPTextWinForm
{
    public partial class UserPagingControlFrom : UserControl
    {
        #region  初始化
        private PagingCondition page = new PagingCondition();
        private System.Windows.Forms.DataGridView dataGridView;
        private PagingCondition paging;
        private string sqlStr= @"SELECT * FROM (SELECT ROW_NUMBER()OVER(ORDER BY 收貨日期 DESC) AS row_Index,* FROM [MDDB].[dbo].[YJJDK]) AS t  WHERE 1=1  ";
        private string sqlCountStr= @"SELECT count(*) FROM (SELECT ROW_NUMBER()OVER(ORDER BY 收貨日期 DESC) AS row_Index,* FROM [MDDB].[dbo].[YJJDK]) AS t  WHERE 1=1  ";
        public UserPagingControlFrom(System.Windows.Forms.DataGridView _dataGridView, PagingCondition _paging, string _sql, string _sqlCount)
        {
            if (paging == null)
            {
                paging = new PagingCondition()
                {
                    startIndex = 0,
                    pageSize = 0,
                    needPaging = false,
                    sortField = "",
                    currentPage = 0,
                    pageCount = 0,
                    totalCount = 0,
                    fieldorder = orderBy.ASC,
                };
            }
            else
            {
                this.paging = _paging;
            }
            this.sqlStr = _sql;
            this.sqlCountStr = _sqlCount;
            this.dataGridView = _dataGridView;

            InitializeComponent();
        }

        public UserPagingControlFrom()
        {
        }
        #endregion

        #region  事件
        private void btnFirst_Click(object sender, EventArgs e)
        {
            page.currentPage = 1;
            this.Bind();
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            page.currentPage -= 1;
            this.Bind();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            page.currentPage += 1;
            this.Bind();
        }


        private void btnLast_Click(object sender, EventArgs e)
        {
            page.currentPage = page.pageCount;
            this.Bind();
        }

        private void comboPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            page.pageSize = Convert.ToInt32(comboPageSize.Text);
            page.pageCount = ((page.totalCount / page.pageSize) + (page.totalCount % page.pageSize > 0 ? 1 : 0));
            if (page.currentPage > page.pageCount)
            {
                page.currentPage = page.pageCount;
            }
            this.Bind();
        }

        private void txtBoxCurPage_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                string curPage = this.txtBoxCurPage.Text;
                int currentPage = 0;
                if (string.IsNullOrEmpty(curPage))
                {
                    MessageBox.Show("请输入当前页！", "提示");
                    return;
                }

                try
                {
                    currentPage = int.Parse(curPage);
                }
                catch (Exception)
                {
                    MessageBox.Show("输入的不是数字！", "提示");
                    return;
                }
                page.currentPage = currentPage;
                page.pageSize = Convert.ToInt32(comboPageSize.Text);
                page.pageCount = ((page.totalCount / page.pageSize) + (page.totalCount % page.pageSize > 0 ? 1 : 0));
                this.Bind();
            }
        }
        #endregion

        #region  私有方法

        /// <summary>
        /// 数据绑定
        /// </summary>
        private void Bind()
        {
            DataBind();
            if (page.currentPage > page.pageCount)
            {
                page.currentPage = page.pageCount;
            }
            this.txtBoxCurPage.Text = page.currentPage + "";
            this.lblTotalCount.Text = page.totalCount + "";
            this.lblPageCount.Text = ((page.totalCount / page.pageSize) + (page.totalCount % page.pageSize > 0 ? 1 : 0)) + "";
            this.lblRecordRegion.Text = GetRecordRegion();
            if (page.currentPage == 1)
            {
                this.btnFirst.Enabled = false;
                this.btnPrev.Enabled = false;
            }
            else
            {
                this.btnFirst.Enabled = true;
                this.btnPrev.Enabled = true;
            }
            if (page.currentPage == page.pageCount)
            {
                this.btnNext.Enabled = false;
                this.btnLast.Enabled = false;
            }
            else
            {
                this.btnNext.Enabled = true;
                this.btnLast.Enabled = true;
            }
            if (page.totalCount == 0)
            {
                this.btnFirst.Enabled = false;
                this.btnPrev.Enabled = false;
                this.btnNext.Enabled = false;
                this.btnLast.Enabled = false;
            }

        }

        /// <summary>
        /// gvOperateLogList 数据邦定
        /// </summary>
        private void DataBind()
        {
            System.Windows.Forms.DataGridView dataGridView = this.dataGridView;//数据源
            DataTable dt = GetByCondition(paging);
            page.totalCount = Convert.ToInt32(dt.TableName);
            dataGridView.DataSource = dt;
            dataGridView.Columns.Clear();
            var dict = GetGvColumnsDict();
            this.DisplayColList(dataGridView, dict);
        }

        /// <summary>
        /// 替换列表
        /// </summary>
        /// <param name="dgv">类表名称</param>
        /// <param name="dic">数据</param>
        /// <param name="isRM">是否显示序列号</param>
        private void DisplayColList(System.Windows.Forms.DataGridView dgv, Dictionary<string, string> dic)//, bool isRM
        {
            var _dgv = dgv;
            dgv.RowsDefaultCellStyle.BackColor = Color.FromArgb(255, 255, 255);//第一行
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(231, 232, 239);//第二行
            dgv.GridColor = Color.FromArgb(207, 208, 216);//
            dgv.RowTemplate.Height = 25;//列宽
            dgv.AllowUserToAddRows = false;//无空行
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleVertical;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.AllowUserToOrderColumns = true;
            foreach (KeyValuePair<string, string> cl in dic)
            {
                dgv.AutoGenerateColumns = false;
                DataGridViewTextBoxColumn obj = new DataGridViewTextBoxColumn();
                obj.DataPropertyName = cl.Key;
                obj.HeaderText = cl.Value;
                obj.Name = cl.Key;
                obj.Width = 100;
                //obj.DefaultCellStyle.Padding.All = 10;
                obj.Resizable = DataGridViewTriState.True;
                dgv.Columns.AddRange(new DataGridViewColumn[] { obj });
            }
            //dgv.Rows.Insert(0);
            //dgv.Rows[0].Cells["PurchaseOrderNumber"].Value = orderrecord.PurchaseOrderNumber;
        }

        /// <summary>
        /// gv显示列设置
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetGvColumnsDict()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("流水號", "操作类型");
            dict.Add("傳出區", "操作对象");
            dict.Add("流水號HK", "操作内容");
            dict.Add("擬出貨", "操作人员");
            return dict;
        }
        /// <summary>
        /// 获取条件查询数据
        /// </summary>
        /// <param name="paging"></param>
        /// <param name="conditon"></param>
        /// <returns></returns>
        private DataTable GetByCondition(PagingCondition paging)
        {
            string strSql = this.sqlStr;
            string strSqlCount =this.sqlCountStr;
            string strAndWhere = @" ";

            if (paging != null)
            {
                if (paging.needPaging)
                    strAndWhere += string.Format("AND t.row_Index BETWEEN {0} AND {1}", (paging.startIndex - 1) * paging.pageSize + 1, paging.startIndex * paging.pageSize);
            }
            if (!string.IsNullOrEmpty(paging.sortField))
            {
                strSql += strAndWhere + string.Format(" order by {0} {1}", paging.sortField, paging.fieldorder);
            }


            DataTable dt = DBHelper.GetDataSet(strSql).Tables[0];
            dt.TableName = DBHelper.ExecuteScalar(strSqlCount).ToString();
            return dt;
        }

        /// <summary>
        /// 获取显示记录区间（格式如：1-50）
        /// </summary>
        /// <returns></returns>
        private string GetRecordRegion()
        {
            if (page.pageCount == 1) //只有一页
            {
                return "1-" + page.totalCount.ToString();
            }
            else  //有多页
            {
                if (page.currentPage == 1) //当前显示为第一页
                {
                    return "1-" + page.pageSize;
                }
                else if (page.currentPage == page.pageCount) //当前显示为最后一页
                {
                    return ((page.currentPage - 1) * page.pageSize + 1) + "-" + page.totalCount;
                }
                else //中间页
                {
                    return ((page.currentPage - 1) * page.pageSize + 1) + "-" + page.currentPage * page.pageSize;
                }
            }
        }
        #endregion

        #region  分页信息
        /// <summary>
        /// 分页
        /// </summary>
        public class PagingCondition
        {
            /// <summary>
            /// 开始索引
            /// </summary>
            public int startIndex { get; set; }
            /// <summary>
            /// 页码大小
            /// </summary>
            public int pageSize { get; set; }
            /// <summary>
            /// 当前页
            /// </summary>
            public int currentPage { get; set; }
            /// <summary>
            /// 总数
            /// </summary>
            public int totalCount { get; set; }
            /// <summary>
            /// 页码总数
            /// </summary>
            public int pageCount { get; set; }
            /// <summary>
            /// 启动分页
            /// </summary>
            public bool needPaging { get; set; }
            /// <summary>
            /// 排序字段
            /// </summary>
            public string sortField { get; set; }
            /// <summary>
            /// 字段升序、降序
            /// </summary>
            public orderBy fieldorder { get; set; }
        }
        public enum orderBy
        {
            ASC,
            DESC
        }
        #endregion
    }
}
