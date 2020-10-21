namespace SPTextWinForm
{
    partial class CodeGenerator
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
            this.btnCreate = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSolution = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtAutoAdd = new System.Windows.Forms.TextBox();
            this.txtBLL = new System.Windows.Forms.TextBox();
            this.txtDAL = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.txtModel = new System.Windows.Forms.TextBox();
            this.cbxTables = new System.Windows.Forms.ComboBox();
            this.btnConnection = new System.Windows.Forms.Button();
            this.txtConStr = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnCreate
            // 
            this.btnCreate.Location = new System.Drawing.Point(668, 50);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(75, 23);
            this.btnCreate.TabIndex = 23;
            this.btnCreate.Text = "生成";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(305, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 22;
            this.label2.Text = "命名空间";
            // 
            // txtSolution
            // 
            this.txtSolution.Location = new System.Drawing.Point(364, 50);
            this.txtSolution.Name = "txtSolution";
            this.txtSolution.Size = new System.Drawing.Size(217, 21);
            this.txtSolution.TabIndex = 21;
            this.txtSolution.Text = "SPTextWinForm";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(139, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 20;
            this.label1.Text = "自增列";
            // 
            // txtAutoAdd
            // 
            this.txtAutoAdd.Location = new System.Drawing.Point(186, 49);
            this.txtAutoAdd.Name = "txtAutoAdd";
            this.txtAutoAdd.Size = new System.Drawing.Size(100, 21);
            this.txtAutoAdd.TabIndex = 19;
            this.txtAutoAdd.Text = "Id";
            // 
            // txtBLL
            // 
            this.txtBLL.Location = new System.Drawing.Point(549, 76);
            this.txtBLL.Multiline = true;
            this.txtBLL.Name = "txtBLL";
            this.txtBLL.Size = new System.Drawing.Size(265, 320);
            this.txtBLL.TabIndex = 18;
            // 
            // txtDAL
            // 
            this.txtDAL.Location = new System.Drawing.Point(278, 76);
            this.txtDAL.Multiline = true;
            this.txtDAL.Name = "txtDAL";
            this.txtDAL.Size = new System.Drawing.Size(265, 320);
            this.txtDAL.TabIndex = 17;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(587, 50);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 16;
            this.btnBrowse.Text = "预览";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // txtModel
            // 
            this.txtModel.Location = new System.Drawing.Point(7, 76);
            this.txtModel.Multiline = true;
            this.txtModel.Name = "txtModel";
            this.txtModel.Size = new System.Drawing.Size(265, 320);
            this.txtModel.TabIndex = 15;
            // 
            // cbxTables
            // 
            this.cbxTables.FormattingEnabled = true;
            this.cbxTables.Location = new System.Drawing.Point(7, 50);
            this.cbxTables.Name = "cbxTables";
            this.cbxTables.Size = new System.Drawing.Size(122, 20);
            this.cbxTables.TabIndex = 14;
            // 
            // btnConnection
            // 
            this.btnConnection.Location = new System.Drawing.Point(587, 21);
            this.btnConnection.Name = "btnConnection";
            this.btnConnection.Size = new System.Drawing.Size(75, 23);
            this.btnConnection.TabIndex = 13;
            this.btnConnection.Text = "连接";
            this.btnConnection.UseVisualStyleBackColor = true;
            this.btnConnection.Click += new System.EventHandler(this.btnConnection_Click);
            // 
            // txtConStr
            // 
            this.txtConStr.Location = new System.Drawing.Point(7, 23);
            this.txtConStr.Name = "txtConStr";
            this.txtConStr.Size = new System.Drawing.Size(495, 21);
            this.txtConStr.TabIndex = 12;
            this.txtConStr.Text = "Data Source=.;Initial Catalog=Customers;User Id=sa;Password=123456;";
            // 
            // CodeGenerator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(827, 412);
            this.Controls.Add(this.btnCreate);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtSolution);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtAutoAdd);
            this.Controls.Add(this.txtBLL);
            this.Controls.Add(this.txtDAL);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.txtModel);
            this.Controls.Add(this.cbxTables);
            this.Controls.Add(this.btnConnection);
            this.Controls.Add(this.txtConStr);
            this.Name = "CodeGenerator";
            this.Text = "代码生成器";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSolution;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtAutoAdd;
        private System.Windows.Forms.TextBox txtBLL;
        private System.Windows.Forms.TextBox txtDAL;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox txtModel;
        private System.Windows.Forms.ComboBox cbxTables;
        private System.Windows.Forms.Button btnConnection;
        private System.Windows.Forms.TextBox txtConStr;
    }
}