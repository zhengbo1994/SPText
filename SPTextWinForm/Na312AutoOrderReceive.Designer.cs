namespace SPTextWinForm
{
    partial class Na312AutoOrderReceive
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
            this.rtb_logs = new System.Windows.Forms.RichTextBox();
            this.btn_run = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // rtb_logs
            // 
            this.rtb_logs.BackColor = System.Drawing.Color.Gainsboro;
            this.rtb_logs.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtb_logs.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.rtb_logs.Location = new System.Drawing.Point(0, 79);
            this.rtb_logs.Name = "rtb_logs";
            this.rtb_logs.ReadOnly = true;
            this.rtb_logs.Size = new System.Drawing.Size(567, 309);
            this.rtb_logs.TabIndex = 5;
            this.rtb_logs.Text = "";
            // 
            // btn_run
            // 
            this.btn_run.AllowDrop = true;
            this.btn_run.BackColor = System.Drawing.Color.Green;
            this.btn_run.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_run.Location = new System.Drawing.Point(23, 26);
            this.btn_run.Name = "btn_run";
            this.btn_run.Size = new System.Drawing.Size(154, 36);
            this.btn_run.TabIndex = 4;
            this.btn_run.Text = "开始";
            this.btn_run.UseVisualStyleBackColor = false;
            this.btn_run.Click += new System.EventHandler(this.btn_run_Click);
            // 
            // Na312AutoOrderReceive
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(567, 388);
            this.Controls.Add(this.rtb_logs);
            this.Controls.Add(this.btn_run);
            this.Name = "Na312AutoOrderReceive";
            this.Text = "Na312自动接单";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtb_logs;
        private System.Windows.Forms.Button btn_run;
    }
}