
namespace SPTextWinForm
{
    partial class JDOrderSplitFlow
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.radioNum = new System.Windows.Forms.RadioButton();
            this.radioCRN = new System.Windows.Forms.RadioButton();
            this.CRN_TextBox = new System.Windows.Forms.TextBox();
            this.dgvGoodsTable = new System.Windows.Forms.DataGridView();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Sequence_Num = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PatientName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SchoolCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ZoneItemsId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CreationTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtAddGoodsByNum = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnRecoverData = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnExportExcel = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnClearWhiteOrder = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.EditEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmtRenewLastData = new System.Windows.Forms.ToolStripMenuItem();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGoodsTable)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.radioNum);
            this.panel2.Controls.Add(this.radioCRN);
            this.panel2.Controls.Add(this.CRN_TextBox);
            this.panel2.Location = new System.Drawing.Point(323, 30);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(341, 33);
            this.panel2.TabIndex = 23;
            // 
            // radioNum
            // 
            this.radioNum.AutoSize = true;
            this.radioNum.Location = new System.Drawing.Point(74, 10);
            this.radioNum.Name = "radioNum";
            this.radioNum.Size = new System.Drawing.Size(83, 16);
            this.radioNum.TabIndex = 1;
            this.radioNum.Text = "流水号删除";
            this.radioNum.UseVisualStyleBackColor = true;
            // 
            // radioCRN
            // 
            this.radioCRN.AutoSize = true;
            this.radioCRN.Checked = true;
            this.radioCRN.Location = new System.Drawing.Point(3, 10);
            this.radioCRN.Name = "radioCRN";
            this.radioCRN.Size = new System.Drawing.Size(71, 16);
            this.radioCRN.TabIndex = 0;
            this.radioCRN.TabStop = true;
            this.radioCRN.Text = "分箱删除";
            this.radioCRN.UseVisualStyleBackColor = true;
            // 
            // CRN_TextBox
            // 
            this.CRN_TextBox.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.CRN_TextBox.Location = new System.Drawing.Point(163, 7);
            this.CRN_TextBox.Name = "CRN_TextBox";
            this.CRN_TextBox.Size = new System.Drawing.Size(161, 21);
            this.CRN_TextBox.TabIndex = 18;
            // 
            // dgvGoodsTable
            // 
            this.dgvGoodsTable.AllowUserToAddRows = false;
            this.dgvGoodsTable.AllowUserToDeleteRows = false;
            this.dgvGoodsTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvGoodsTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Id,
            this.Sequence_Num,
            this.PatientName,
            this.SchoolCode,
            this.ZoneItemsId,
            this.CreationTime});
            this.dgvGoodsTable.Location = new System.Drawing.Point(10, 82);
            this.dgvGoodsTable.Name = "dgvGoodsTable";
            this.dgvGoodsTable.RowHeadersVisible = false;
            this.dgvGoodsTable.RowTemplate.Height = 23;
            this.dgvGoodsTable.Size = new System.Drawing.Size(655, 350);
            this.dgvGoodsTable.TabIndex = 22;
            // 
            // Id
            // 
            this.Id.HeaderText = "ID";
            this.Id.Name = "Id";
            this.Id.ReadOnly = true;
            this.Id.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Id.Width = 50;
            // 
            // Sequence_Num
            // 
            this.Sequence_Num.HeaderText = "流水号";
            this.Sequence_Num.Name = "Sequence_Num";
            this.Sequence_Num.ReadOnly = true;
            this.Sequence_Num.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Sequence_Num.Width = 80;
            // 
            // PatientName
            // 
            this.PatientName.HeaderText = "患者名称";
            this.PatientName.Name = "PatientName";
            this.PatientName.Width = 230;
            // 
            // SchoolCode
            // 
            this.SchoolCode.HeaderText = "学校编码";
            this.SchoolCode.Name = "SchoolCode";
            this.SchoolCode.ReadOnly = true;
            this.SchoolCode.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.SchoolCode.Width = 90;
            // 
            // ZoneItemsId
            // 
            this.ZoneItemsId.HeaderText = "分箱编号";
            this.ZoneItemsId.Name = "ZoneItemsId";
            this.ZoneItemsId.Width = 80;
            // 
            // CreationTime
            // 
            this.CreationTime.HeaderText = "创建时间";
            this.CreationTime.Name = "CreationTime";
            this.CreationTime.Width = 130;
            // 
            // txtAddGoodsByNum
            // 
            this.txtAddGoodsByNum.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.txtAddGoodsByNum.Location = new System.Drawing.Point(58, 37);
            this.txtAddGoodsByNum.Name = "txtAddGoodsByNum";
            this.txtAddGoodsByNum.Size = new System.Drawing.Size(160, 21);
            this.txtAddGoodsByNum.TabIndex = 21;
            this.txtAddGoodsByNum.TextChanged += new System.EventHandler(this.txtAddGoodsByNum_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 20;
            this.label1.Text = "流水号";
            // 
            // btnRecoverData
            // 
            this.btnRecoverData.Location = new System.Drawing.Point(278, 450);
            this.btnRecoverData.Name = "btnRecoverData";
            this.btnRecoverData.Size = new System.Drawing.Size(94, 23);
            this.btnRecoverData.TabIndex = 28;
            this.btnRecoverData.Text = "恢复上次数据";
            this.btnRecoverData.UseVisualStyleBackColor = true;
            this.btnRecoverData.Click += new System.EventHandler(this.btnRecoverData_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(181, 450);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 27;
            this.btnDelete.Text = "删除分箱";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnExportExcel
            // 
            this.btnExportExcel.Location = new System.Drawing.Point(393, 450);
            this.btnExportExcel.Name = "btnExportExcel";
            this.btnExportExcel.Size = new System.Drawing.Size(75, 23);
            this.btnExportExcel.TabIndex = 26;
            this.btnExportExcel.Text = "导出Excel";
            this.btnExportExcel.UseVisualStyleBackColor = true;
            this.btnExportExcel.Click += new System.EventHandler(this.btnExportExcel_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(590, 450);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 25;
            this.btnClear.Text = "清空重做";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnClearWhiteOrder
            // 
            this.btnClearWhiteOrder.Location = new System.Drawing.Point(491, 450);
            this.btnClearWhiteOrder.Name = "btnClearWhiteOrder";
            this.btnClearWhiteOrder.Size = new System.Drawing.Size(80, 23);
            this.btnClearWhiteOrder.TabIndex = 24;
            this.btnClearWhiteOrder.Text = "清空列表";
            this.btnClearWhiteOrder.UseVisualStyleBackColor = true;
            this.btnClearWhiteOrder.Click += new System.EventHandler(this.btnClearWhiteOrder_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.EditEToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(681, 25);
            this.menuStrip1.TabIndex = 29;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // EditEToolStripMenuItem
            // 
            this.EditEToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmtRenewLastData});
            this.EditEToolStripMenuItem.Name = "EditEToolStripMenuItem";
            this.EditEToolStripMenuItem.Size = new System.Drawing.Size(59, 21);
            this.EditEToolStripMenuItem.Text = "编辑(&E)";
            // 
            // tsmtRenewLastData
            // 
            this.tsmtRenewLastData.Name = "tsmtRenewLastData";
            this.tsmtRenewLastData.Size = new System.Drawing.Size(180, 22);
            this.tsmtRenewLastData.Text = "恢复上次修改";
            this.tsmtRenewLastData.Click += new System.EventHandler(this.tsmtRenewLastData_Click);
            // 
            // JDOrderSplitFlow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(681, 488);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.btnRecoverData);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnExportExcel);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnClearWhiteOrder);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.dgvGoodsTable);
            this.Controls.Add(this.txtAddGoodsByNum);
            this.Controls.Add(this.label1);
            this.Name = "JDOrderSplitFlow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "JDOrderSplitFlow";
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGoodsTable)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RadioButton radioNum;
        private System.Windows.Forms.RadioButton radioCRN;
        private System.Windows.Forms.TextBox CRN_TextBox;
        private System.Windows.Forms.DataGridView dgvGoodsTable;
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn Sequence_Num;
        private System.Windows.Forms.DataGridViewTextBoxColumn PatientName;
        private System.Windows.Forms.DataGridViewTextBoxColumn SchoolCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn ZoneItemsId;
        private System.Windows.Forms.DataGridViewTextBoxColumn CreationTime;
        private System.Windows.Forms.TextBox txtAddGoodsByNum;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnRecoverData;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnExportExcel;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnClearWhiteOrder;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem EditEToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsmtRenewLastData;
    }
}