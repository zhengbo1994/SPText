namespace SPTextWinForm
{
    partial class ListView
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ListView));
            this.lvFriends = new System.Windows.Forms.ListView();
            this.chUserNo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chNickName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chAge = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.大图标ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.小图标ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.详情ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.列表ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.旧样式ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imgSmall = new System.Windows.Forms.ImageList(this.components);
            this.imgLarge = new System.Windows.Forms.ImageList(this.components);
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // lvFriends
            // 
            this.lvFriends.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chUserNo,
            this.chNickName,
            this.chAge});
            this.lvFriends.ContextMenuStrip = this.contextMenuStrip1;
            this.lvFriends.HideSelection = false;
            this.lvFriends.LargeImageList = this.imgLarge;
            this.lvFriends.Location = new System.Drawing.Point(53, 46);
            this.lvFriends.Name = "lvFriends";
            this.lvFriends.Size = new System.Drawing.Size(397, 320);
            this.lvFriends.SmallImageList = this.imgSmall;
            this.lvFriends.TabIndex = 1;
            this.lvFriends.UseCompatibleStateImageBehavior = false;
            this.lvFriends.View = System.Windows.Forms.View.Details;
            // 
            // chUserNo
            // 
            this.chUserNo.Text = "编号";
            // 
            // chNickName
            // 
            this.chNickName.Text = "昵称";
            // 
            // chAge
            // 
            this.chAge.Text = "年龄";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.大图标ToolStripMenuItem,
            this.小图标ToolStripMenuItem,
            this.详情ToolStripMenuItem,
            this.列表ToolStripMenuItem,
            this.旧样式ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(113, 114);
            // 
            // 大图标ToolStripMenuItem
            // 
            this.大图标ToolStripMenuItem.Name = "大图标ToolStripMenuItem";
            this.大图标ToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.大图标ToolStripMenuItem.Tag = "1";
            this.大图标ToolStripMenuItem.Text = "大图标";
            this.大图标ToolStripMenuItem.Click += new System.EventHandler(this.大图标ToolStripMenuItem_Click);
            // 
            // 小图标ToolStripMenuItem
            // 
            this.小图标ToolStripMenuItem.Name = "小图标ToolStripMenuItem";
            this.小图标ToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.小图标ToolStripMenuItem.Tag = "2";
            this.小图标ToolStripMenuItem.Text = "小图标";
            this.小图标ToolStripMenuItem.Click += new System.EventHandler(this.大图标ToolStripMenuItem_Click);
            // 
            // 详情ToolStripMenuItem
            // 
            this.详情ToolStripMenuItem.Name = "详情ToolStripMenuItem";
            this.详情ToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.详情ToolStripMenuItem.Tag = "3";
            this.详情ToolStripMenuItem.Text = "详情";
            this.详情ToolStripMenuItem.Click += new System.EventHandler(this.大图标ToolStripMenuItem_Click);
            // 
            // 列表ToolStripMenuItem
            // 
            this.列表ToolStripMenuItem.Name = "列表ToolStripMenuItem";
            this.列表ToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.列表ToolStripMenuItem.Tag = "4";
            this.列表ToolStripMenuItem.Text = "列表";
            this.列表ToolStripMenuItem.Click += new System.EventHandler(this.大图标ToolStripMenuItem_Click);
            // 
            // 旧样式ToolStripMenuItem
            // 
            this.旧样式ToolStripMenuItem.Name = "旧样式ToolStripMenuItem";
            this.旧样式ToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.旧样式ToolStripMenuItem.Tag = "5";
            this.旧样式ToolStripMenuItem.Text = "旧样式";
            this.旧样式ToolStripMenuItem.Click += new System.EventHandler(this.大图标ToolStripMenuItem_Click);
            // 
            // imgSmall
            // 
            this.imgSmall.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgSmall.ImageStream")));
            this.imgSmall.TransparentColor = System.Drawing.Color.Transparent;
            this.imgSmall.Images.SetKeyName(0, "1-1.bmp");
            this.imgSmall.Images.SetKeyName(1, "2-1.bmp");
            this.imgSmall.Images.SetKeyName(2, "3-1.bmp");
            this.imgSmall.Images.SetKeyName(3, "4-1.bmp");
            this.imgSmall.Images.SetKeyName(4, "5-1.bmp");
            this.imgSmall.Images.SetKeyName(5, "6-1.bmp");
            this.imgSmall.Images.SetKeyName(6, "7-1.bmp");
            this.imgSmall.Images.SetKeyName(7, "8-1.bmp");
            this.imgSmall.Images.SetKeyName(8, "9-1.bmp");
            this.imgSmall.Images.SetKeyName(9, "10-1.bmp");
            // 
            // imgLarge
            // 
            this.imgLarge.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgLarge.ImageStream")));
            this.imgLarge.TransparentColor = System.Drawing.Color.Transparent;
            this.imgLarge.Images.SetKeyName(0, "1.bmp");
            this.imgLarge.Images.SetKeyName(1, "2.bmp");
            this.imgLarge.Images.SetKeyName(2, "3.bmp");
            this.imgLarge.Images.SetKeyName(3, "4.bmp");
            this.imgLarge.Images.SetKeyName(4, "5.bmp");
            this.imgLarge.Images.SetKeyName(5, "6.bmp");
            this.imgLarge.Images.SetKeyName(6, "7.bmp");
            this.imgLarge.Images.SetKeyName(7, "8.bmp");
            this.imgLarge.Images.SetKeyName(8, "9.bmp");
            this.imgLarge.Images.SetKeyName(9, "10.bmp");
            // 
            // ListView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(588, 432);
            this.Controls.Add(this.lvFriends);
            this.Name = "ListView";
            this.Text = "好友窗口";
            this.Load += new System.EventHandler(this.ListView_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lvFriends;
        private System.Windows.Forms.ColumnHeader chUserNo;
        private System.Windows.Forms.ColumnHeader chNickName;
        private System.Windows.Forms.ColumnHeader chAge;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 大图标ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 小图标ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 详情ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 列表ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 旧样式ToolStripMenuItem;
        private System.Windows.Forms.ImageList imgLarge;
        private System.Windows.Forms.ImageList imgSmall;
        private System.Windows.Forms.BindingSource bindingSource1;
    }
}