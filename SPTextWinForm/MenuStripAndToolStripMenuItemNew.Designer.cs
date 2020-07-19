namespace SPTextWinForm
{
    partial class MenuStripAndToolStripMenuItemNew
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.学员管理ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.新增学员ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.班级管理ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.新增班级ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.学员管理ToolStripMenuItem,
            this.班级管理ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(666, 25);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 学员管理ToolStripMenuItem
            // 
            this.学员管理ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.新增学员ToolStripMenuItem});
            this.学员管理ToolStripMenuItem.Name = "学员管理ToolStripMenuItem";
            this.学员管理ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.学员管理ToolStripMenuItem.Text = "学员管理";
            // 
            // 新增学员ToolStripMenuItem
            // 
            this.新增学员ToolStripMenuItem.Name = "新增学员ToolStripMenuItem";
            this.新增学员ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.新增学员ToolStripMenuItem.Text = "新增学员";
            this.新增学员ToolStripMenuItem.Click += new System.EventHandler(this.新增学员ToolStripMenuItem_Click);
            // 
            // 班级管理ToolStripMenuItem
            // 
            this.班级管理ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.新增班级ToolStripMenuItem});
            this.班级管理ToolStripMenuItem.Name = "班级管理ToolStripMenuItem";
            this.班级管理ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.班级管理ToolStripMenuItem.Text = "班级管理";
            // 
            // 新增班级ToolStripMenuItem
            // 
            this.新增班级ToolStripMenuItem.Name = "新增班级ToolStripMenuItem";
            this.新增班级ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.新增班级ToolStripMenuItem.Text = "新增班级";
            // 
            // MenuStripAndToolStripMenuItemNew
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(666, 415);
            this.Controls.Add(this.menuStrip1);
            this.IsMdiContainer = true;
            this.Name = "MenuStripAndToolStripMenuItemNew";
            this.Text = "MenuStripAndToolStripMenuItemNew";
            this.Load += new System.EventHandler(this.MenuStripAndToolStripMenuItemNew_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 学员管理ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 新增学员ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 班级管理ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 新增班级ToolStripMenuItem;
    }
}