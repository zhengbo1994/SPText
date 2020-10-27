using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SPTextWinForm
{
    public partial class CodeGenerator : Form
    {
        public CodeGenerator()
        {
            InitializeComponent();
        }

        private DataTable SqlCon(string sqlStr)
        {
            string conStr = txtConStr.Text.Trim();
            using (SqlConnection con = new SqlConnection(conStr))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(sqlStr, con))
                {
                    DataSet ds = new DataSet();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.FillSchema(ds,SchemaType.Source);
                    sda.Fill(ds);
                    DataTable dt = ds.Tables[0];
                    return dt;
                }
            }
        }
        DataTable tables = null;

        private void btnConnection_Click(object sender, EventArgs e)
        {
            //获取所有的表名
            tables = SqlCon("SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE ='BASE TABLE'");
            //增加全选
            DataRow dr = tables.NewRow();
            dr["TABLE_NAME"] = "全选";
            tables.Rows.InsertAt(dr, 0);
            //绑定下拉菜单
            cbxTables.DataSource = tables;
            cbxTables.DisplayMember = "TABLE_NAME";
            cbxTables.ValueMember = "TABLE_NAME";
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            string filePath = AppDomain.CurrentDomain.BaseDirectory + "solutionPath.txt";
            if (File.Exists(filePath))
            {
                fbd.SelectedPath = File.ReadAllText(filePath);
            }
            else
            {
                File.Create(filePath).Close();
            }
            DialogResult dialogResult = fbd.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                string createPath = fbd.SelectedPath;
                File.WriteAllText(filePath, createPath);
                string sulotionName = txtSolution.Text.Trim();
                CreateCode((dt, tableName) =>
                {
                    try
                    {
                        File.WriteAllText($@"{createPath}\{sulotionName}.Model\{tableName}Model.cs", CreateModel(dt, tableName));
                        File.WriteAllText($@"{createPath}\{sulotionName}.DAL\{tableName}DAL.cs", CreateDAL(dt, tableName));
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("项目尚未创建或者选择目录错误");
                    }
                });
            }
        }
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            CreateCode((dt, tableName) =>
            {
                CreateModel(dt, tableName);
                CreateDAL(dt, tableName);
            });
        }

        private void CreateCode(Action<DataTable, string> createAction)
        {
            txtBLL.Text = null;
            txtDAL.Text = null;
            txtModel.Text = null;
            string tableName = cbxTables.SelectedValue.ToString();
            if (tableName != "全选")
            {
                DataTable dt = SqlCon($"SELECT * FROM [{tableName}]");
                createAction(dt, tableName);
            }
            else
            {
                for (int i = 0; i < tables.Rows.Count; i++)
                {
                    DataRow dr = tables.Rows[i];
                    if (dr["TABLE_NAME"].ToString() != "全选")
                    {
                        DataTable dt = SqlCon($"SELECT * FROM [{dr["TABLE_NAME"].ToString()}]");
                        createAction(dt, dr["TABLE_NAME"].ToString());
                    }
                }

            }
        }
        private string CreateModel(DataTable dt, string tableName)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("using System;");
            sb.Append($"namespace {txtSolution.Text.Trim()}.Model").AppendLine();
            sb.Append("{").AppendLine();
            sb.Append($"            public class {tableName}").AppendLine();
            sb.Append("            {").AppendLine();
            foreach (DataColumn dc in dt.Columns)
            {
                sb.Append($"                public {GetDbType(dc)} {dc.ColumnName}").Append("{ get; set; }").AppendLine();
            }
            sb.Append("            }").AppendLine();
            sb.Append("}").AppendLine();
            txtModel.AppendText(sb.ToString());
            return sb.ToString();
        }


        private string CreateDAL(DataTable dt, string tableName)
        {
            string[] colsNotAutoKey = GetCols(dt).Where(m => m != txtAutoAdd.Text.Trim()).ToArray();
            string[] cols = GetCols(dt);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"using {txtSolution.Text.Trim()}.Model;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using System.Configuration;");
            sb.AppendLine("using System.Data;");
            sb.AppendLine("using System.Data.SqlClient;");
            sb.Append($"namespace {txtSolution.Text.Trim()}.DAL").AppendLine();
            sb.Append("{").AppendLine();
            sb.AppendLine($"        public class {tableName}DAL");
            sb.AppendLine("        {");
            sb.AppendLine("            AceClass.SqlHelper sh = new AceClass.SqlHelper(ConfigurationManager.ConnectionStrings[\"ConStr\"].ToString());");
            //添加数据
            sb.AppendLine($"            public void InsertRecord({tableName} md)");
            sb.AppendLine("            {");
            sb.AppendLine($"                sh.ExecuteNonQuery(@\"INSERT INTO {tableName}");
            sb.AppendLine($"        ({string.Join(",", colsNotAutoKey)}) VALUES");
            sb.AppendLine($"        ({string.Join(",", colsNotAutoKey.Select(m => "@" + m))})\" ");
            for (int i = 0; i < colsNotAutoKey.Length; i++)
            {
                sb.AppendLine($"                    ,new SqlParameter(\"@{colsNotAutoKey[i]}\", md.{colsNotAutoKey[i]})");
            }
            sb.AppendLine("            );}");
            //ToModel
            sb.AppendLine($"        private {tableName} ToModel(DataRow dr)");
            sb.AppendLine("        {");
            sb.AppendLine($"            {tableName} md = new {tableName}();");
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                sb.AppendLine($"            md.{dt.Columns[i].ColumnName} = ({GetDbType(dt.Columns[i])})dr[\"{dt.Columns[i].ColumnName}\"];");
            }

            sb.AppendLine("            return md;");
            sb.AppendLine("        }");

            sb.AppendLine("        }");

            sb.Append("}").AppendLine();
            txtDAL.AppendText(sb.ToString());
            return sb.ToString();
        }
        private void CreateBLL()
        {

        }

        private string GetDbType(DataColumn dc)
        {
            if (dc.AllowDBNull && dc.DataType.IsValueType)
            {
                return dc.DataType + "?";
            }
            else
            {
                return dc.DataType.ToString();
            }
        }
        private string[] GetCols(DataTable dt)
        {
            List<string> cols = new List<string>();
            foreach (DataColumn dc in dt.Columns)
            {
                cols.Add(dc.ColumnName);
            }
            return cols.ToArray();
        }
    }
}
