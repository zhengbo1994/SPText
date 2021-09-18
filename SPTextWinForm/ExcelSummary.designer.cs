
namespace ExcelSummarySheet
{
    partial class ExcelSummary
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.labzipCount = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.texzipFileOutPath = new System.Windows.Forms.TextBox();
            this.texzipFileInPath = new System.Windows.Forms.TextBox();
            this.butzipImport = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.labxlsCount = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.texxlsFileOutPath = new System.Windows.Forms.TextBox();
            this.texxlsFileInPath = new System.Windows.Forms.TextBox();
            this.butxlsImport = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(21, 21);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(462, 186);
            this.tabControl1.TabIndex = 8;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.labzipCount);
            this.tabPage1.Controls.Add(this.label7);
            this.tabPage1.Controls.Add(this.texzipFileOutPath);
            this.tabPage1.Controls.Add(this.texzipFileInPath);
            this.tabPage1.Controls.Add(this.butzipImport);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(454, 160);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "按zip导入";
            // 
            // labzipCount
            // 
            this.labzipCount.AutoSize = true;
            this.labzipCount.Location = new System.Drawing.Point(229, 39);
            this.labzipCount.Name = "labzipCount";
            this.labzipCount.Size = new System.Drawing.Size(11, 12);
            this.labzipCount.TabIndex = 21;
            this.labzipCount.Text = "0";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(121, 39);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(101, 12);
            this.label7.TabIndex = 20;
            this.label7.Text = "选择文件数量为：";
            // 
            // texzipFileOutPath
            // 
            this.texzipFileOutPath.Location = new System.Drawing.Point(123, 112);
            this.texzipFileOutPath.Name = "texzipFileOutPath";
            this.texzipFileOutPath.ReadOnly = true;
            this.texzipFileOutPath.Size = new System.Drawing.Size(291, 21);
            this.texzipFileOutPath.TabIndex = 12;
            // 
            // texzipFileInPath
            // 
            this.texzipFileInPath.Location = new System.Drawing.Point(123, 73);
            this.texzipFileInPath.Name = "texzipFileInPath";
            this.texzipFileInPath.ReadOnly = true;
            this.texzipFileInPath.Size = new System.Drawing.Size(291, 21);
            this.texzipFileInPath.TabIndex = 11;
            // 
            // butzipImport
            // 
            this.butzipImport.Location = new System.Drawing.Point(26, 34);
            this.butzipImport.Name = "butzipImport";
            this.butzipImport.Size = new System.Drawing.Size(76, 23);
            this.butzipImport.TabIndex = 10;
            this.butzipImport.Text = "导入";
            this.butzipImport.UseVisualStyleBackColor = true;
            this.butzipImport.Click += new System.EventHandler(this.butzipImport_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 115);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 12);
            this.label2.TabIndex = 9;
            this.label2.Text = "文件导出路径：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 76);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 8;
            this.label1.Text = "文件导入路径：";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.labxlsCount);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.texxlsFileOutPath);
            this.tabPage2.Controls.Add(this.texxlsFileInPath);
            this.tabPage2.Controls.Add(this.butxlsImport);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(454, 160);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "按xls导入";
            // 
            // labxlsCount
            // 
            this.labxlsCount.AutoSize = true;
            this.labxlsCount.Location = new System.Drawing.Point(228, 39);
            this.labxlsCount.Name = "labxlsCount";
            this.labxlsCount.Size = new System.Drawing.Size(11, 12);
            this.labxlsCount.TabIndex = 19;
            this.labxlsCount.Text = "0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(120, 39);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(101, 12);
            this.label5.TabIndex = 18;
            this.label5.Text = "选择文件数量为：";
            // 
            // texxlsFileOutPath
            // 
            this.texxlsFileOutPath.Location = new System.Drawing.Point(122, 112);
            this.texxlsFileOutPath.Name = "texxlsFileOutPath";
            this.texxlsFileOutPath.ReadOnly = true;
            this.texxlsFileOutPath.Size = new System.Drawing.Size(291, 21);
            this.texxlsFileOutPath.TabIndex = 17;
            // 
            // texxlsFileInPath
            // 
            this.texxlsFileInPath.Location = new System.Drawing.Point(122, 73);
            this.texxlsFileInPath.Name = "texxlsFileInPath";
            this.texxlsFileInPath.ReadOnly = true;
            this.texxlsFileInPath.Size = new System.Drawing.Size(291, 21);
            this.texxlsFileInPath.TabIndex = 16;
            // 
            // butxlsImport
            // 
            this.butxlsImport.Location = new System.Drawing.Point(25, 34);
            this.butxlsImport.Name = "butxlsImport";
            this.butxlsImport.Size = new System.Drawing.Size(76, 23);
            this.butxlsImport.TabIndex = 15;
            this.butxlsImport.Text = "导入";
            this.butxlsImport.UseVisualStyleBackColor = true;
            this.butxlsImport.Click += new System.EventHandler(this.butxlsImport_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 115);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 12);
            this.label3.TabIndex = 14;
            this.label3.Text = "文件导出路径：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(23, 76);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 12);
            this.label4.TabIndex = 13;
            this.label4.Text = "文件导入路径：";
            // 
            // ExcelSummary
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(515, 244);
            this.Controls.Add(this.tabControl1);
            this.Name = "ExcelSummary";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SMD excel整合工具_V2.0";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TextBox texzipFileOutPath;
        private System.Windows.Forms.TextBox texzipFileInPath;
        private System.Windows.Forms.Button butzipImport;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox texxlsFileOutPath;
        private System.Windows.Forms.TextBox texxlsFileInPath;
        private System.Windows.Forms.Button butxlsImport;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label labzipCount;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label labxlsCount;
        private System.Windows.Forms.Label label5;
    }
}

