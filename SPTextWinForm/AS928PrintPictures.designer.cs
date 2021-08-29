
namespace ReportPrinting
{
    partial class AS928PrintPictures
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
            this.labMsg = new System.Windows.Forms.Label();
            this.txtNum = new System.Windows.Forms.TextBox();
            this.btnExport = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labMsg
            // 
            this.labMsg.AutoSize = true;
            this.labMsg.Location = new System.Drawing.Point(99, 103);
            this.labMsg.Name = "labMsg";
            this.labMsg.Size = new System.Drawing.Size(41, 12);
            this.labMsg.TabIndex = 7;
            this.labMsg.Text = "未执行";
            // 
            // txtNum
            // 
            this.txtNum.Location = new System.Drawing.Point(101, 27);
            this.txtNum.Name = "txtNum";
            this.txtNum.Size = new System.Drawing.Size(210, 21);
            this.txtNum.TabIndex = 6;
            this.txtNum.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNum_KeyPress);
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(101, 64);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(75, 23);
            this.btnExport.TabIndex = 5;
            this.btnExport.Text = "打印";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "跟单条码：";
            // 
            // AS928PrintPictures
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(339, 139);
            this.Controls.Add(this.labMsg);
            this.Controls.Add(this.txtNum);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "AS928PrintPictures";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AS928贴纸打印";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labMsg;
        private System.Windows.Forms.TextBox txtNum;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Label label1;
    }
}