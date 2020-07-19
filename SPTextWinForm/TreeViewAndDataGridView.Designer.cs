namespace SPTextWinForm
{
    partial class TreeViewAndDataGridView
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
            this.dgvPrograms = new System.Windows.Forms.DataGridView();
            this.tvChannels = new System.Windows.Forms.TreeView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPrograms)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvPrograms
            // 
            this.dgvPrograms.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPrograms.Location = new System.Drawing.Point(270, 13);
            this.dgvPrograms.Name = "dgvPrograms";
            this.dgvPrograms.RowTemplate.Height = 23;
            this.dgvPrograms.Size = new System.Drawing.Size(518, 425);
            this.dgvPrograms.TabIndex = 3;
            // 
            // tvChannels
            // 
            this.tvChannels.Location = new System.Drawing.Point(12, 12);
            this.tvChannels.Name = "tvChannels";
            this.tvChannels.Size = new System.Drawing.Size(240, 426);
            this.tvChannels.TabIndex = 2;
            this.tvChannels.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvChannels_AfterSelect);
            // 
            // TreeViewAndDataGridView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dgvPrograms);
            this.Controls.Add(this.tvChannels);
            this.Name = "TreeViewAndDataGridView";
            this.Text = "TreeViewAndDataGridView";
            this.Load += new System.EventHandler(this.TreeViewAndDataGridView_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPrograms)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvPrograms;
        private System.Windows.Forms.TreeView tvChannels;
    }
}