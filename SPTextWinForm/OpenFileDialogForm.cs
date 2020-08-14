using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using static System.Net.Mime.MediaTypeNames;
using System.Drawing.Drawing2D;

namespace SPTextWinForm
{
    public partial class OpenFileDialogForm : Form
    {
        public OpenFileDialogForm()
        {
            InitializeComponent();
        }

        private void btnFile_Click(object sender, EventArgs e)
        {
        }

        #region 上传图片 并生成缩略图
        /// <summary>
        /// 上传图片生成缩略图
        /// </summary>
        /// <param name="originalImagePath">图片源路径</param>
        /// <param name="thumbnailPath">缩略图路径(物理路径)</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="mode">生成缩略图的方式</param>
        public static void MakeThumbnail(string originalImagePath, string thumbnailPath, int width, int height, string mode)
        {
            
        }
        #endregion
    }
}
