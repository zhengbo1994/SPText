using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SPTextWinForm.DataGridView
{
    public partial class DataGridViewPageFrom : Form
    {
        public DataGridViewPageFrom()
        {
            InitializeComponent();
        }


        public event EventPagingHandler EventPaging;
        public delegate void EventPagingHandler(EventArgs e);

        #region 公开属性
        private int _pageSize = 50;
        /// <summary>
        /// 每页显示记录数(默认50)
        /// </summary>
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                if (value > 0)
                {
                    _pageSize = value;
                }
                else
                {
                    _pageSize = 50;
                }
                this.comboPageSize.Text = _pageSize.ToString();
            }
        }
        private int _currentPage = 1;
        /// <summary>
        /// 当前页
        /// </summary>
        public int CurrentPage
        {
            get
            {
                return _currentPage;
            }
            set
            {
                if (value > 0)
                {
                    _currentPage = value;
                }
                else
                {
                    _currentPage = 1;
                }

            }
        }
        private int _totalCount = 0;
        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalCount
        {
            get
            {
                return _totalCount;
            }
            set
            {
                if (value >= 0)
                {
                    _totalCount = value;
                }
                else
                {
                    _totalCount = 0;
                }
                this.lblTotalCount.Text = this._totalCount.ToString();
                CalculatePageCount();
                this.lblRecordRegion.Text = GetRecordRegion();
            }
        }

        private int _pageCount = 0;
        /// <summary>
        /// 页数
        /// </summary>
        public int PageCount
        {
            get
            {
                return _pageCount;
            }
            set
            {
                if (value >= 0)
                {
                    _pageCount = value;
                }
                else
                {
                    _pageCount = 0;
                }
                this.lblPageCount.Text = _pageCount + "";
            }
        }
        #endregion

        #region  分页事件
        private void btnFirst_Click(object sender, EventArgs e)
        {
            this.CurrentPage = 1;
            this.Bind();
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            this.CurrentPage -= 1;
            this.Bind();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            this.CurrentPage += 1;
            this.Bind();
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            this.CurrentPage = this.PageCount;
            this.Bind();
        }

        private void gvOperateLogList_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            //自动编号，与数据无关
            Rectangle rectangle = new Rectangle(e.RowBounds.Location.X, e.RowBounds.Location.Y, gvOperateLogList.RowHeadersWidth - 4, e.RowBounds.Height);
            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(), gvOperateLogList.RowHeadersDefaultCellStyle.Font,
                rectangle, gvOperateLogList.RowHeadersDefaultCellStyle.ForeColor, TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }

        private void gvOperateLogList_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {

        }

        private void gvOperateLogList_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {

        }

        /// <summary>
        ///  改变每页条数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.PageSize = Convert.ToInt32(comboPageSize.Text);
            this.PageCount = ((this.TotalCount / this.PageSize) + (this.TotalCount % this.PageSize > 0 ? 1 : 0));
            if (this.CurrentPage > this.PageCount)
            {
                this.CurrentPage = this.PageCount;
            }
            this.Bind();
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuery_Click(object sender, EventArgs e)
        {
            //GvDataBind();
            this.Bind();
        }

        /// <summary>
        /// 页码输入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                this.CurrentPage = currentPage;
                this.PageSize = Convert.ToInt32(comboPageSize.Text);
                this.PageCount = ((this.TotalCount / this.PageSize) + (this.TotalCount % this.PageSize > 0 ? 1 : 0));
                this.Bind();
            }
        }
        #endregion

        #region  私有方法调用
        /// <summary>
        /// 获取显示记录区间（格式如：1-50）
        /// </summary>
        /// <returns></returns>
        private string GetRecordRegion()
        {
            if (this.PageCount == 1) //只有一页
            {
                return "1-" + this.TotalCount.ToString();
            }
            else  //有多页
            {
                if (this.CurrentPage == 1) //当前显示为第一页
                {
                    return "1-" + this.PageSize;
                }
                else if (this.CurrentPage == this.PageCount) //当前显示为最后一页
                {
                    return ((this.CurrentPage - 1) * this.PageSize + 1) + "-" + this.TotalCount;
                }
                else //中间页
                {
                    return ((this.CurrentPage - 1) * this.PageSize + 1) + "-" + this.CurrentPage * this.PageSize;
                }
            }
        }

        /// <summary>
        /// 数据绑定
        /// </summary>
        public void Bind()
        {
            GvDataBind();
            if (this.EventPaging != null)
            {
                this.EventPaging(new EventArgs());
            }
            if (this.CurrentPage > this.PageCount)
            {
                this.CurrentPage = this.PageCount;
            }
            this.txtBoxCurPage.Text = this.CurrentPage + "";
            this.lblTotalCount.Text = this.TotalCount + "";
            this.lblPageCount.Text = ((this.TotalCount / this.PageSize) + (this.TotalCount % this.PageSize > 0 ? 1 : 0)) + "";
            this.lblRecordRegion.Text = GetRecordRegion();
            if (this.CurrentPage == 1)
            {
                this.btnFirst.Enabled = false;
                this.btnPrev.Enabled = false;
            }
            else
            {
                this.btnFirst.Enabled = true;
                this.btnPrev.Enabled = true;
            }
            if (this.CurrentPage == this.PageCount)
            {
                this.btnNext.Enabled = false;
                this.btnLast.Enabled = false;
            }
            else
            {
                this.btnNext.Enabled = true;
                this.btnLast.Enabled = true;
            }
            if (this.TotalCount == 0)
            {
                this.btnFirst.Enabled = false;
                this.btnPrev.Enabled = false;
                this.btnNext.Enabled = false;
                this.btnLast.Enabled = false;
            }

        }

        /// <summary>
        /// 计算页数
        /// </summary>
        private void CalculatePageCount()
        {
            if (this.TotalCount > 0)
            {
                this.PageCount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(this.TotalCount) / Convert.ToDouble(this.PageSize)));
            }
            else
            {
                this.PageCount = 0;
            }
        }

        /// <summary>
        /// gvOperateLogList 数据邦定
        /// </summary>
        private void GvDataBind()
        {
            PagingCondition paging = new PagingCondition()
            {
                startIndex = this.CurrentPage,
                pageSize = this.PageSize,
                needPaging = true,
                sortField = "收貨日期",
                fieldorder = orderBy.DESC,
            };

            System.Windows.Forms.DataGridView dataGridView = this.gvOperateLogList;//数据源
            DataTable dt = GetByCondition(paging);
            this.TotalCount = Convert.ToInt32(dt.TableName);
            dataGridView.DataSource = dt;
            dataGridView.Columns.Clear();
            var dict = GetGvColumnsDict();
            this.DisplayColList(dataGridView, dict);
        }

        /// <summary>
        /// gv显示列设置
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetGvColumnsDict()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("流水號", "操作类型");
            dict.Add("傳出區", "操作对象");
            dict.Add("流水號HK", "操作内容");
            dict.Add("擬出貨", "操作人员");
            return dict;
        }

        /// <summary>
        /// 替换列表
        /// </summary>
        /// <param name="dgv">类表名称</param>
        /// <param name="dic">数据</param>
        /// <param name="isRM">是否显示序列号</param>
        public void DisplayColList(System.Windows.Forms.DataGridView dgv, Dictionary<string, string> dic)//, bool isRM
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
            dgv.RowPostPaint += new DataGridViewRowPostPaintEventHandler(gvOperateLogList_RowPostPaint);
            dgv.CellPainting += new DataGridViewCellPaintingEventHandler(gvOperateLogList_CellPainting);//列头样式
            dgv.CellFormatting += new DataGridViewCellFormattingEventHandler(gvOperateLogList_CellFormatting);//选中行样式
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
            //dataBinding();数据绑定集中方式
        }


        /// <summary>
        /// 获取条件查询数据
        /// </summary>
        /// <param name="paging"></param>
        /// <param name="conditon"></param>
        /// <returns></returns>
        private DataTable GetByCondition(PagingCondition paging)
        {
            string strSql = @"SELECT * FROM (SELECT ROW_NUMBER()OVER(ORDER BY 收貨日期 DESC) AS row_Index,* FROM [MDDB].[dbo].[YJJDK]) AS t ";
            string strSqlCount = @"SELECT count(*) FROM (SELECT ROW_NUMBER()OVER(ORDER BY 收貨日期 DESC) AS row_Index,* FROM [MDDB].[dbo].[YJJDK]) AS t ";
            string strWhere = @" WHERE 1=1  ";

            if (paging != null)
            {
                if (paging.needPaging)
                    strWhere += string.Format("AND t.row_Index BETWEEN {0} AND {1}", (paging.startIndex - 1) * paging.pageSize + 1, paging.startIndex * paging.pageSize);
            }
            if (!string.IsNullOrEmpty(paging.sortField))
            {
                strSql += strWhere + string.Format(" order by {0} {1}", paging.sortField, paging.fieldorder);
            }


            DataTable dt = DBHelper.GetDataSet(strSql).Tables[0];
            dt.TableName = DBHelper.ExecuteScalar(strSqlCount).ToString();
            return dt;
        }

        /// <summary>
        /// 数据绑定的集中方式（供参考）
        /// </summary>
        /// <param name="a"></param>
        /// <param name="dgv"></param>
        /// <param name="dt"></param>
        /// <param name="orderrecord"></param>
        /// <param name="orderRecordList"></param>
        private void dataBinding(int a, System.Windows.Forms.DataGridView dgv, DataTable dt, OrdersConverterDeliveryRecord orderrecord, List<OrdersConverterDeliveryRecord> orderRecordList)
        {
            if (a == 4)//倒序进行添加
            {
                dgv.Rows.Clear();
                foreach (DataRow row in dt.Rows)
                {
                    string purchaseOrderNumber = row["PurchaseOrderNumber"] == null ? "" : row["PurchaseOrderNumber"].ToString();
                    string commodityNumber = row["CommodityNumber"] == null ? "" : row["CommodityNumber"].ToString();
                    string commodityName = row["CommodityName"] == null ? "" : row["CommodityName"].ToString();
                    string deliveryQuantity = row["DeliveryQuantity"] == null ? "" : row["DeliveryQuantity"].ToString();
                    string outboundOrderNumber = row["OutboundOrderNumber"] == null ? "" : row["OutboundOrderNumber"].ToString();
                    string logisticsNumber = row["LogisticsNumber"] == null ? "" : row["LogisticsNumber"].ToString();
                    string creationTime = row["CreationTime"] == null ? "" : row["CreationTime"].ToString();

                    orderrecord = new OrdersConverterDeliveryRecord()
                    {
                        PurchaseOrderNumber = purchaseOrderNumber,
                        CommodityNumber = commodityNumber,
                        CommodityName = commodityName,
                        DeliveryQuantity = deliveryQuantity == "" ? 0 : Convert.ToInt32(deliveryQuantity),
                        OutboundOrderNumber = outboundOrderNumber,
                        LogisticsNumber = logisticsNumber,
                        PackageNumber = "1",
                        CreationTime = Convert.ToDateTime(creationTime),
                    };
                    {
                        dgv.Rows.Insert(0);
                        dgv.Rows[0].Cells["PurchaseOrderNumber"].Value = orderrecord.PurchaseOrderNumber;
                        dgv.Rows[0].Cells["CommodityNumber"].Value = orderrecord.CommodityNumber;
                        dgv.Rows[0].Cells["Brand"].Value = orderrecord.Brand;
                        dgv.Rows[0].Cells["CommodityName"].Value = orderrecord.CommodityName;
                        dgv.Rows[0].Cells["DeliveryQuantity"].Value = orderrecord.DeliveryQuantity;
                        dgv.Rows[0].Cells["GiftQuantity"].Value = orderrecord.GiftQuantity;
                        dgv.Rows[0].Cells["InvoiceNumber"].Value = orderrecord.InvoiceNumber;
                        dgv.Rows[0].Cells["OutboundOrderNumber"].Value = orderrecord.OutboundOrderNumber;
                        dgv.Rows[0].Cells["LogisticsCompany"].Value = orderrecord.LogisticsCompany;
                        dgv.Rows[0].Cells["LogisticsNumber"].Value = orderrecord.LogisticsNumber;
                        dgv.Rows[0].Cells["PackageNumber"].Value = orderrecord.PackageNumber;
                        dgv.Rows[0].Cells["CreationTime"].Value = orderrecord.CreationTime;
                    }
                    orderRecordList.Add(orderrecord);
                }
            }
            else if (a == 2)//正序进行添加
            {
                dgv.Rows.Clear();
                int m = 0;
                foreach (DataRow row in dt.Rows)
                {
                    string purchaseOrderNumber = row["PurchaseOrderNumber"] == null ? "" : row["PurchaseOrderNumber"].ToString();
                    string commodityNumber = row["CommodityNumber"] == null ? "" : row["CommodityNumber"].ToString();
                    string commodityName = row["CommodityName"] == null ? "" : row["CommodityName"].ToString();
                    string deliveryQuantity = row["DeliveryQuantity"] == null ? "" : row["DeliveryQuantity"].ToString();
                    string outboundOrderNumber = row["OutboundOrderNumber"] == null ? "" : row["OutboundOrderNumber"].ToString();
                    string logisticsNumber = row["LogisticsNumber"] == null ? "" : row["LogisticsNumber"].ToString();
                    string creationTime = row["CreationTime"] == null ? "" : row["CreationTime"].ToString();

                    orderrecord = new OrdersConverterDeliveryRecord()
                    {
                        PurchaseOrderNumber = purchaseOrderNumber,
                        CommodityNumber = commodityNumber,
                        CommodityName = commodityName,
                        DeliveryQuantity = deliveryQuantity == "" ? 0 : Convert.ToInt32(deliveryQuantity),
                        OutboundOrderNumber = outboundOrderNumber,
                        LogisticsNumber = logisticsNumber,
                        PackageNumber = "1",
                        CreationTime = Convert.ToDateTime(creationTime),
                    };
                    {
                        dgv.Rows.Add();
                        dgv.Rows[m].Cells["PurchaseOrderNumber"].Value = orderrecord.PurchaseOrderNumber;
                        dgv.Rows[m].Cells["CommodityNumber"].Value = orderrecord.CommodityNumber;
                        dgv.Rows[m].Cells["Brand"].Value = orderrecord.Brand;
                        dgv.Rows[m].Cells["CommodityName"].Value = orderrecord.CommodityName;
                        dgv.Rows[m].Cells["DeliveryQuantity"].Value = orderrecord.DeliveryQuantity;
                        dgv.Rows[m].Cells["GiftQuantity"].Value = orderrecord.GiftQuantity;
                        dgv.Rows[m].Cells["InvoiceNumber"].Value = orderrecord.InvoiceNumber;
                        dgv.Rows[m].Cells["OutboundOrderNumber"].Value = orderrecord.OutboundOrderNumber;
                        dgv.Rows[m].Cells["LogisticsCompany"].Value = orderrecord.LogisticsCompany;
                        dgv.Rows[m].Cells["LogisticsNumber"].Value = orderrecord.LogisticsNumber;
                        dgv.Rows[m].Cells["PackageNumber"].Value = orderrecord.PackageNumber;
                        dgv.Rows[m].Cells["CreationTime"].Value = orderrecord.CreationTime;
                        m++;
                    }
                    orderRecordList.Add(orderrecord);
                }
            }
            else if (a == 3)//正序
            {
                dgv.AutoGenerateColumns = false;
                dgv.DataSource = dt;
                dgv.Columns.Clear();
                Dictionary<string, string> dict = new Dictionary<string, string>();
                dict.Add("CreationTime", "创建时间");
                dict.Add("PurchaseOrderNumber", "采购单号");
                dict.Add("CommodityNumber", "商品编号");
                dict.Add("Brand", "品牌");
                dict.Add("CommodityName", "商品名称");
                dict.Add("DeliveryQuantity", "发货数量");
                dict.Add("GiftQuantity", "赠品数量");
                dict.Add("InvoiceNumber", "发票号");
                dict.Add("OutboundOrderNumber", "出库单号");
                dict.Add("LogisticsCompany", "物流公司");
                dict.Add("LogisticsNumber", "物流单号");
                dict.Add("PackageNumber", "包裹数量");
                foreach (KeyValuePair<string, string> cl in dict)
                {
                    dgv.AutoGenerateColumns = false;
                    DataGridViewTextBoxColumn obj = new DataGridViewTextBoxColumn();
                    obj.DataPropertyName = cl.Key;
                    obj.HeaderText = cl.Value;
                    obj.Name = cl.Key;
                    obj.Width = 100;
                    obj.Resizable = DataGridViewTriState.True;
                    dgv.Columns.AddRange(new DataGridViewColumn[] { obj });
                }
            }
            else if (a == 4)//正序
            {
                dgv.AutoGenerateColumns = false;
                #region  倒序排序
                DataView dv = dt.DefaultView;
                dv.Sort = "CreationTime Desc";
                DataTable dt2 = dv.ToTable();
                #endregion
                dgv.DataSource = dt;
                dgv.Columns[0].HeaderText = "创建时间";
                dgv.Columns[0].DataPropertyName = "CreationTime";
                dgv.Columns[1].HeaderText = "采购单号";
                dgv.Columns[1].DataPropertyName = "PurchaseOrderNumber";
                dgv.Columns[2].HeaderText = "商品编号";
                dgv.Columns[2].DataPropertyName = "CommodityNumber";
                dgv.Columns[3].HeaderText = "品牌";
                dgv.Columns[3].DataPropertyName = "Brand";
                dgv.Columns[4].HeaderText = "商品名称";
                dgv.Columns[4].DataPropertyName = "CommodityName";
                dgv.Columns[5].HeaderText = "发货数量";
                dgv.Columns[5].DataPropertyName = "DeliveryQuantity";
                dgv.Columns[6].HeaderText = "赠品数量";
                dgv.Columns[6].DataPropertyName = "GiftQuantity";
                dgv.Columns[7].HeaderText = "发票号";
                dgv.Columns[7].DataPropertyName = "InvoiceNumber";
                dgv.Columns[8].HeaderText = "出库单号";
                dgv.Columns[8].DataPropertyName = "OutboundOrderNumber";
                dgv.Columns[9].HeaderText = "物流公司";
                dgv.Columns[9].DataPropertyName = "LogisticsCompany";
                dgv.Columns[10].HeaderText = "物流单号";
                dgv.Columns[10].DataPropertyName = "LogisticsNumber";
                dgv.Columns[11].HeaderText = "包裹数量";
                dgv.Columns[11].DataPropertyName = "PackageNumber";
            }
        }
        #endregion

        #region  辅助类
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
        public class OrdersConverterDeliveryRecord
        {
            public int Id { get; set; }
            /// <summary>
            /// 采购单号
            /// </summary>
            public string PurchaseOrderNumber { get; set; }
            /// <summary>
            /// 商品编号
            /// </summary>
            public string CommodityNumber { get; set; }
            /// <summary>
            /// 品牌
            /// </summary>
            public string Brand { get; set; }
            /// <summary>
            /// 商品名称
            /// </summary>
            public string CommodityName { get; set; }
            /// <summary>
            /// 发货数量
            /// </summary>
            public int DeliveryQuantity { get; set; }
            /// <summary>
            /// 赠品数量
            /// </summary>
            public string GiftQuantity { get; set; }
            /// <summary>
            /// 发票号
            /// </summary>
            public string InvoiceNumber { get; set; }
            /// <summary>
            /// 出库单号（流水号）
            /// </summary>
            public string OutboundOrderNumber { get; set; }
            /// <summary>
            /// 物流公司
            /// </summary>
            public string LogisticsCompany { get; set; }
            /// <summary>
            /// 物流单号
            /// </summary>
            public string LogisticsNumber { get; set; }
            /// <summary>
            /// 包裹数量
            /// </summary>
            public string PackageNumber { get; set; }
            /// <summary>
            /// 创建时间
            /// </summary>
            public DateTime CreationTime { get; set; }
            /// <summary>
            /// 创建人
            /// </summary>
            public string CreationId { get; set; }
        }
        #endregion
    }

}


