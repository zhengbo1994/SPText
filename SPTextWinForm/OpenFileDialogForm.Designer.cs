namespace SPTextWinForm
{
    partial class OpenFileDialogForm
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
            this.openFileDialogPath = new System.Windows.Forms.OpenFileDialog();
            this.labEndPath = new System.Windows.Forms.Label();
            this.labBeginPath = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // openFileDialogPath
            // 
            this.openFileDialogPath.FileName = "openFileDialogPath";
            // 
            // labEndPath
            // 
            this.labEndPath.AutoSize = true;
            this.labEndPath.Location = new System.Drawing.Point(137, 103);
            this.labEndPath.Name = "labEndPath";
            this.labEndPath.Size = new System.Drawing.Size(77, 12);
            this.labEndPath.TabIndex = 10;
            this.labEndPath.Text = "上传路径显示";
            // 
            // labBeginPath
            // 
            this.labBeginPath.AutoSize = true;
            this.labBeginPath.Location = new System.Drawing.Point(137, 73);
            this.labBeginPath.Name = "labBeginPath";
            this.labBeginPath.Size = new System.Drawing.Size(77, 12);
            this.labBeginPath.TabIndex = 9;
            this.labBeginPath.Text = "上传路径显示";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(139, 37);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "上传文件";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.btnFile_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(52, 103);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "转化后路径";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(52, 73);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "转化前文件";
            // 
            // OpenFileDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(487, 189);
            this.Controls.Add(this.labEndPath);
            this.Controls.Add(this.labBeginPath);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "OpenFileDialogForm";
            this.Text = "OpenFileDialogForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialogPath;
        private System.Windows.Forms.Label labEndPath;
        private System.Windows.Forms.Label labBeginPath;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}