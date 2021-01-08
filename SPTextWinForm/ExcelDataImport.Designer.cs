
namespace SPTextWinForm
{
    partial class ExcelDataImport
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
            this.rchtxtLog = new System.Windows.Forms.RichTextBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblHS = new System.Windows.Forms.Label();
            this.lblPJ = new System.Windows.Forms.Label();
            this.dataSource = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.targetSource = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // rchtxtLog
            // 
            this.rchtxtLog.Location = new System.Drawing.Point(12, 109);
            this.rchtxtLog.Name = "rchtxtLog";
            this.rchtxtLog.Size = new System.Drawing.Size(494, 301);
            this.rchtxtLog.TabIndex = 8;
            this.rchtxtLog.Text = "";
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(431, 80);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 9;
            this.btnStart.Text = "开始";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(12, 80);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 15;
            this.btnStop.Text = "停止";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Visible = false;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 16;
            this.label1.Text = "数据源";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 433);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(394, 23);
            this.progressBar1.TabIndex = 17;
            this.progressBar1.Visible = false;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(420, 438);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(83, 12);
            this.lblStatus.TabIndex = 18;
            this.lblStatus.Text = "200000/200000";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblStatus.Visible = false;
            // 
            // lblHS
            // 
            this.lblHS.AutoSize = true;
            this.lblHS.Location = new System.Drawing.Point(374, 476);
            this.lblHS.Name = "lblHS";
            this.lblHS.Size = new System.Drawing.Size(41, 12);
            this.lblHS.TabIndex = 19;
            this.lblHS.Text = "耗时：";
            this.lblHS.Visible = false;
            // 
            // lblPJ
            // 
            this.lblPJ.AutoSize = true;
            this.lblPJ.Location = new System.Drawing.Point(448, 476);
            this.lblPJ.Name = "lblPJ";
            this.lblPJ.Size = new System.Drawing.Size(41, 12);
            this.lblPJ.TabIndex = 20;
            this.lblPJ.Text = "平均：";
            this.lblPJ.Visible = false;
            // 
            // dataSource
            // 
            this.dataSource.Location = new System.Drawing.Point(54, 14);
            this.dataSource.Name = "dataSource";
            this.dataSource.ReadOnly = true;
            this.dataSource.Size = new System.Drawing.Size(449, 21);
            this.dataSource.TabIndex = 21;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 22;
            this.label2.Text = "目标";
            // 
            // targetSource
            // 
            this.targetSource.Location = new System.Drawing.Point(54, 49);
            this.targetSource.Name = "targetSource";
            this.targetSource.ReadOnly = true;
            this.targetSource.Size = new System.Drawing.Size(449, 21);
            this.targetSource.TabIndex = 23;
            // 
            // ExcelDataImport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(532, 500);
            this.Controls.Add(this.targetSource);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dataSource);
            this.Controls.Add(this.lblPJ);
            this.Controls.Add(this.lblHS);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.rchtxtLog);
            this.Name = "ExcelDataImport";
            this.Text = "ExcelDataImport";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox rchtxtLog;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblHS;
        private System.Windows.Forms.Label lblPJ;
        private System.Windows.Forms.TextBox dataSource;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox targetSource;
    }
}