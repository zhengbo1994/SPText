using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace SPTextWinForm
{
    public partial class TreeViewAndDataGridView : Form
    {
        public TreeViewAndDataGridView()
        {
            InitializeComponent();
        }

        private void tvChannels_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //判断深度等于根节点
            if (this.tvChannels.SelectedNode.Level == 0) return;
            string filePath = this.tvChannels.SelectedNode.Tag.ToString();
            List<ProgramModel> list = new List<ProgramModel>();

            XmlNode rootNode = GetXmlDocument(filePath);
            XmlNodeList nodeList = rootNode.SelectNodes("Program");
            foreach (XmlNode item in nodeList)
            {
                ProgramModel model = new ProgramModel();
                foreach (XmlNode item2 in item.ChildNodes)
                {
                    switch (item2.Name)
                    {
                        case "name":
                            model.Name = item2.InnerText;
                            break;
                        case "playTime":
                            model.PlayTime = item2.InnerText;
                            break;
                        case "path":
                            model.Path = item2.InnerText;
                            break;
                    }
                }

                list.Add(model);
            }

            this.dgvPrograms.DataSource = list;
        }

        private void TreeViewAndDataGridView_Load(object sender, EventArgs e)
        {
            BindTree();
            this.tvChannels.ExpandAll();//展开节点
            //this.tvChannels.CollapseAll();//收起节点
        }

        /// <summary>
        /// 获取xml信息绑定树控件
        /// </summary>
        private void BindTree()
        {
            //C:\Users\zhouzheng\source\repos\VIP_26\bin\Debug\Channels.xml
            //Application.StartupPath+"/Channels.xml";//获取当前项目的根目录绝对路径
            TreeNode rootNode = new TreeNode("所有电视台");//树控件节点
            this.tvChannels.Nodes.Add(rootNode);//this.tvChannels树

            XmlNode xmlNode = GetXmlDocument("Channels.xml");//获取到xml文件根节点
            XmlNodeList xmlList = xmlNode.SelectNodes("Channel");//获取子节点列表
            foreach (XmlNode item in xmlList)
            {
                string name = item.Attributes["name"].Value;//获取节点信息
                //string id = item.Attributes["id"].Value;//获取节点信息
                //item.ChildNodes，假如你有多个节点
                XmlNode pathNode = item.FirstChild;//获取频道节点下的第一个节点
                string path = pathNode.InnerText;//文件路径节点,取括号

                TreeNode channelNode = new TreeNode(name);//电视台节点
                channelNode.Tag = path;
                rootNode.Nodes.Add(channelNode);//把电视台节点，放到根节点
            }
            //添加到树
        }

        /// <summary>
        /// 通用方法：根据文件路径加载xml文档
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private XmlElement GetXmlDocument(string path)
        {
            //创建一个xml文件处理对象
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(path);//加载文件内容
                                   //获取xml根节点
            XmlElement xmlElement = xmlDocument.DocumentElement;
            return xmlElement;
        }
    }

    public class ProgramModel
    {
        public string Name { get; set; }
        public string PlayTime { get; set; }
        public string Path { get; set; }
    }
}
