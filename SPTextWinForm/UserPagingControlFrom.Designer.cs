namespace SPTextWinForm
{
    partial class UserPagingControlFrom
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.panelPage = new System.Windows.Forms.Panel();
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
            this.panelPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelPage
            // 
            this.panelPage.Controls.Add(this.btnLast);
            this.panelPage.Controls.Add(this.btnPrev);
            this.panelPage.Controls.Add(this.btnNext);
            this.panelPage.Controls.Add(this.btnFirst);
            this.panelPage.Controls.Add(this.label10);
            this.panelPage.Controls.Add(this.lblTotalCount);
            this.panelPage.Controls.Add(this.label8);
            this.panelPage.Controls.Add(this.lblPageCount);
            this.panelPage.Controls.Add(this.label6);
            this.panelPage.Controls.Add(this.lblRecordRegion);
            this.panelPage.Controls.Add(this.label4);
            this.panelPage.Controls.Add(this.label3);
            this.panelPage.Controls.Add(this.txtBoxCurPage);
            this.panelPage.Controls.Add(this.label2);
            this.panelPage.Controls.Add(this.comboPageSize);
            this.panelPage.Controls.Add(this.label1);
            this.panelPage.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelPage.Location = new System.Drawing.Point(0, 170);
            this.panelPage.Name = "panelPage";
            this.panelPage.Size = new System.Drawing.Size(818, 33);
            this.panelPage.TabIndex = 2;
            // 
            // btnLast
            // 
            this.btnLast.Location = new System.Drawing.Point(394, 5);
            this.btnLast.Name = "btnLast";
            this.btnLast.Size = new System.Drawing.Size(39, 23);
            this.btnLast.TabIndex = 18;
            this.btnLast.Text = "未页";
            this.btnLast.UseVisualStyleBackColor = true;
            this.btnLast.Click += new System.EventHandler(this.btnLast_Click);
            // 
            // btnPrev
            // 
            this.btnPrev.Location = new System.Drawing.Point(191, 4);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(50, 23);
            this.btnPrev.TabIndex = 16;
            this.btnPrev.Text = "上一页";
            this.btnPrev.UseVisualStyleBackColor = true;
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(339, 6);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(51, 23);
            this.btnNext.TabIndex = 17;
            this.btnNext.Text = "下一页";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnFirst
            // 
            this.btnFirst.Location = new System.Drawing.Point(147, 4);
            this.btnFirst.Name = "btnFirst";
            this.btnFirst.Size = new System.Drawing.Size(38, 23);
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
            // UserPagingControlFrom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelPage);
            this.Name = "UserPagingControlFrom";
            this.Size = new System.Drawing.Size(818, 203);
            this.panelPage.ResumeLayout(false);
            this.panelPage.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelPage;
        private System.Windows.Forms.Button btnLast;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnFirst;
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
    }
}
