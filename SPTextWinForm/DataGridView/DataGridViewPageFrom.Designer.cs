namespace SPTextWinForm.DataGridView
{
    partial class DataGridViewPageFrom
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.gvOperateLogList = new System.Windows.Forms.DataGridView();
            this.paging1 = new System.Windows.Forms.Panel();
            this.btnLast = new System.Windows.Forms.Button();
            this.btnPrev = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnFirst = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.lblTotalCount = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lblPageCount = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblRecordRegion = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtBoxCurPage = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboPageSize = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnQuery = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gvOperateLogList)).BeginInit();
            this.paging1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gvOperateLogList
            // 
            this.gvOperateLogList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvOperateLogList.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gvOperateLogList.Location = new System.Drawing.Point(0, 106);
            this.gvOperateLogList.Name = "gvOperateLogList";
            this.gvOperateLogList.RowTemplate.Height = 23;
            this.gvOperateLogList.Size = new System.Drawing.Size(800, 311);
            this.gvOperateLogList.TabIndex = 0;
            this.gvOperateLogList.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.gvOperateLogList_CellFormatting);
            this.gvOperateLogList.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.gvOperateLogList_CellPainting);
            this.gvOperateLogList.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.gvOperateLogList_RowPostPaint);
            // 
            // paging1
            // 
            this.paging1.Controls.Add(this.btnLast);
            this.paging1.Controls.Add(this.btnPrev);
            this.paging1.Controls.Add(this.btnNext);
            this.paging1.Controls.Add(this.btnFirst);
            this.paging1.Controls.Add(this.label10);
            this.paging1.Controls.Add(this.lblTotalCount);
            this.paging1.Controls.Add(this.label8);
            this.paging1.Controls.Add(this.lblPageCount);
            this.paging1.Controls.Add(this.label6);
            this.paging1.Controls.Add(this.lblRecordRegion);
            this.paging1.Controls.Add(this.label4);
            this.paging1.Controls.Add(this.label3);
            this.paging1.Controls.Add(this.txtBoxCurPage);
            this.paging1.Controls.Add(this.label2);
            this.paging1.Controls.Add(this.comboPageSize);
            this.paging1.Controls.Add(this.label1);
            this.paging1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.paging1.Location = new System.Drawing.Point(0, 417);
            this.paging1.Name = "paging1";
            this.paging1.Size = new System.Drawing.Size(800, 33);
            this.paging1.TabIndex = 1;
            // 
            // btnLast
            // 
            this.btnLast.Location = new System.Drawing.Point(394, 5);
            this.btnLast.Name = "btnLast";
            this.btnLast.Size = new System.Drawing.Size(42, 23);
            this.btnLast.TabIndex = 18;
            this.btnLast.Text = "未页";
            this.btnLast.UseVisualStyleBackColor = true;
            this.btnLast.Click += new System.EventHandler(this.btnLast_Click);
            // 
            // btnPrev
            // 
            this.btnPrev.Location = new System.Drawing.Point(191, 4);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(54, 23);
            this.btnPrev.TabIndex = 16;
            this.btnPrev.Text = "上一页";
            this.btnPrev.UseVisualStyleBackColor = true;
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(336, 6);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(54, 23);
            this.btnNext.TabIndex = 17;
            this.btnNext.Text = "下一页";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnFirst
            // 
            this.btnFirst.Location = new System.Drawing.Point(147, 4);
            this.btnFirst.Name = "btnFirst";
            this.btnFirst.Size = new System.Drawing.Size(42, 23);
            this.btnFirst.TabIndex = 3;
            this.btnFirst.Text = "首页";
            this.btnFirst.UseVisualStyleBackColor = true;
            this.btnFirst.Click += new System.EventHandler(this.btnFirst_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(780, 14);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(17, 12);
            this.label10.TabIndex = 15;
            this.label10.Text = "条";
            // 
            // lblTotalCount
            // 
            this.lblTotalCount.AutoSize = true;
            this.lblTotalCount.Location = new System.Drawing.Point(740, 14);
            this.lblTotalCount.Name = "lblTotalCount";
            this.lblTotalCount.Size = new System.Drawing.Size(11, 12);
            this.lblTotalCount.TabIndex = 14;
            this.lblTotalCount.Text = "0";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(689, 13);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 12);
            this.label8.TabIndex = 13;
            this.label8.Text = "页，总计";
            // 
            // lblPageCount
            // 
            this.lblPageCount.AutoSize = true;
            this.lblPageCount.Location = new System.Drawing.Point(649, 12);
            this.lblPageCount.Name = "lblPageCount";
            this.lblPageCount.Size = new System.Drawing.Size(11, 12);
            this.lblPageCount.TabIndex = 12;
            this.lblPageCount.Text = "0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(609, 12);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 12);
            this.label6.TabIndex = 11;
            this.label6.Text = "条，共";
            // 
            // lblRecordRegion
            // 
            this.lblRecordRegion.AutoSize = true;
            this.lblRecordRegion.Location = new System.Drawing.Point(536, 13);
            this.lblRecordRegion.Name = "lblRecordRegion";
            this.lblRecordRegion.Size = new System.Drawing.Size(23, 12);
            this.lblRecordRegion.TabIndex = 10;
            this.lblRecordRegion.Text = "0-0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(508, 12);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 9;
            this.label4.Text = "显示";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(316, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "页";
            // 
            // txtBoxCurPage
            // 
            this.txtBoxCurPage.Location = new System.Drawing.Point(265, 6);
            this.txtBoxCurPage.Name = "txtBoxCurPage";
            this.txtBoxCurPage.Size = new System.Drawing.Size(47, 21);
            this.txtBoxCurPage.TabIndex = 5;
            this.txtBoxCurPage.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtBoxCurPage_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(247, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "第";
            // 
            // comboPageSize
            // 
            this.comboPageSize.FormattingEnabled = true;
            this.comboPageSize.Items.AddRange(new object[] {
            "10",
            "20",
            "50",
            "100",
            "1000",
            "10000"});
            this.comboPageSize.Location = new System.Drawing.Point(76, 6);
            this.comboPageSize.Name = "comboPageSize";
            this.comboPageSize.Size = new System.Drawing.Size(66, 20);
            this.comboPageSize.TabIndex = 1;
            this.comboPageSize.Text = "50";
            this.comboPageSize.SelectedIndexChanged += new System.EventHandler(this.comboPageSize_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "每页条数：";
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(696, 66);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(75, 23);
            this.btnQuery.TabIndex = 2;
            this.btnQuery.Text = "查询";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // DataGridViewPageFrom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnQuery);
            this.Controls.Add(this.gvOperateLogList);
            this.Controls.Add(this.paging1);
            this.Name = "DataGridViewPageFrom";
            this.Text = "数据分页";
            ((System.ComponentModel.ISupportInitialize)(this.gvOperateLogList)).EndInit();
            this.paging1.ResumeLayout(false);
            this.paging1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView gvOperateLogList;
        private System.Windows.Forms.Panel paging1;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label lblTotalCount;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblPageCount;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblRecordRegion;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtBoxCurPage;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboPageSize;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnQuery;
        private System.Windows.Forms.Button btnLast;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnFirst;
    }
}