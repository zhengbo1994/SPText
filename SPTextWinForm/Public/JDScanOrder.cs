using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SPTextWinForm.Public
{
    public class JDScanOrder
    {
        /// <summary>
        /// 语音播报
        /// </summary>
        /// <param name="orderAddress"></param>
        public void VoicePlay(string strVoicePlay, string strLanguageChoice)
        {
            SpeechSynthesizer speech = new System.Speech.Synthesis.SpeechSynthesizer();
            var voices = speech.GetInstalledVoices();
            if (voices.Any(p => p.VoiceInfo.Culture.Name.Trim() == strLanguageChoice.Trim()))
            {
                InstalledVoice installedVoice = voices.FirstOrDefault(p => p.VoiceInfo.Culture.Name == strLanguageChoice);
                string languageName = installedVoice.VoiceInfo.Name;
                speech.SelectVoice(languageName.Trim());
                speech.Rate = 0;
                speech.Volume = 100;

                string str = string.Format("{0}", strVoicePlay);
                speech.SpeakAsync(str); //语音方法调用
            }
            else
            {
                MessageBox.Show("未安装语音包，无法播放语音！");
                var voicesList = string.Join(",", voices.Select(p => p.VoiceInfo.Culture.Name).ToList());
                MessageBox.Show("本机安装的语言包有：" + voicesList.Substring(0, voicesList.Length));
            }
        }

        /// <summary>
        /// 检查是否在YJJDK里面有数据，若无数据，抛出错误
        /// </summary>
        /// <param name="num"></param>
        public bool InspectSequenceNum(string num, DatabaseSetSettingInfo db)
        {
            SqlHelper sqlHelper = new SqlHelper(db.dbHost, db.dbName, db.dbUser, db.dbPwd);

            string sql = "SELECT COUNT(1) FROM MDDB.dbo.YJJDK a JOIN ERP.BOM.BOM_PROCESS_LIST b ON a.流水號=b.SEQUENCE_NUM WHERE a.流水號=@Sequence_Num";
            System.Data.SqlClient.SqlParameter[] sp = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@Sequence_Num",num)
            };
            object o = sqlHelper.ExecuteScalar(sql, sp);
            if (o.ToString() != "0")
            {
                return true;
            }
            return false;
        }

        public void SearchAndCreate(string num, DatabaseSetSettingInfo db, System.Windows.Forms.DataGridView dgvGoodsTable, string strLanguageChoice, ref List<OrderAddress> alloAList)
        {
            string sql = "select * from [ARO].[dbo].[NA319_OrderSplitFlow] where Sequence_Num=@Sequence_Num";
            SqlParameter[] sqlParameters = new SqlParameter[] {
                new SqlParameter("@Sequence_Num",num)
            };
            SqlParameter sp = new SqlParameter("@Sequence_Num", num);
            SqlHelper sqlHelper = new SqlHelper(db.dbHost, db.dbName, db.dbUser, db.dbPwd);
            DataSet ds = sqlHelper.Query(sql, sqlParameters);
            DataTable dt = ds.Tables[0];
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    IntoCrn(num, true, true, db, dgvGoodsTable, strLanguageChoice, ref alloAList);
                }
                else
                {
                    IntoCrn(num, false, true, db, dgvGoodsTable, strLanguageChoice, ref alloAList);
                }
            }
        }


        /// <summary>
        /// 数据组装
        /// </summary>
        /// <param name="num">流水号</param>
        /// <param name="dataFlag">数据是否插入数据库【dataFlag：true---数据库存在】</param>
        /// <param name="flagVoicePlay">播放声音</param>
        private void IntoCrn(string num, bool dataFlag, bool flagVoicePlay, DatabaseSetSettingInfo db, System.Windows.Forms.DataGridView dgvGoodsTable, string strLanguageChoice, ref List<OrderAddress> alloAList)
        {
            int zoneItemsId = 0;
            string strAccountName = "";
            SqlHelper sqlHelper = new SqlHelper(db.dbHost, db.dbName, db.dbUser, db.dbPwd);
            {
                //查询NA319的订单数据
                WP_ORDERS order = this.GetWPOrder(sqlHelper, num);
                if (order == null)
                {
                    MessageBox.Show(string.Format("流水号为：{0}，未找到数据", num));
                    return;
                }
                strAccountName = order.ACCOUNT_ID;
                zoneItemsId = this.GetAddressZoneIdByOrder(sqlHelper, order);
                if (zoneItemsId == 0 && order.PATIENT_NAME.Replace("，", ",").Split(',')[0].Equals("Parker"))  //海外部需求，特殊处理（先把特殊类型存入数据库为ID=1，再判断学校代码有没有特殊标识）
                {
                    zoneItemsId = 0;
                }
                else if (zoneItemsId == 0)
                {
                    zoneItemsId = this.InsertAddressZone(sqlHelper, order);
                }
            }

            string sqlZONEITEMS = "SELECT * FROM [ARO].[dbo].[NA319_ADDRESS_ZONE_ITEMS] WHERE Id=@Id";

            SqlParameter[] sqlParameters = new SqlParameter[] {
                new SqlParameter("@Id",zoneItemsId)
            };
            DataSet ds = sqlHelper.Query(sqlZONEITEMS, sqlParameters);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                var data = ds.Tables[0].Rows;

                OrderAddress orderAddress = new OrderAddress()
                {
                    Id = 0,
                    AccountName = strAccountName,
                    ZoneItemsId = zoneItemsId,
                    Sequence_Num = num,
                    PatientName = data[0]["PatientName"].ToString(),
                    SchoolCode = data[0]["SchoolCode"].ToString(),
                    CreationTime = DateTime.Now
                };

                alloAList.Add(orderAddress);

                //插入至表格
                InsertDataGridView(orderAddress, dgvGoodsTable);

                //播放声音
                if (flagVoicePlay)
                {
                    string languageValue = orderAddress.ZoneItemsId.ToString().PadLeft(5, '0');
                    string languageVoice = "";

                    foreach (var item in languageValue)
                    {
                        languageVoice += item + " ";
                    }

                    languageVoice = languageVoice + "箱";

                    VoicePlay(languageVoice, strLanguageChoice);
                    //VoicePlay(orderAddress);
                }

                //插入至临时数据库
                if (dataFlag == false)
                {
                    AddNA319OrderSplitFlow(sqlHelper, orderAddress);
                }
            }
        }



        /// <summary>
        /// 根据跟单条码获取NA319订单数据
        /// </summary>
        /// <param name="barCode">跟单条码</param>
        /// <returns></returns>
        public WP_ORDERS GetWPOrder(SqlHelper sqlHelper, string Sequence_Num)
        {
            //string sql = "SELECT * FROM MDDB.dbo.WP_ORDERS WHERE BARCODE=@barCode";
            string sql = "SELECT * FROM MDDB.dbo.WP_ORDERS WHERE BARCODE IN(SELECT 跟單條碼 FROM [MDDB].[dbo].[YJJDK] WHERE 流水號=@Sequence_Num)";
            System.Data.SqlClient.SqlParameter[] sp = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@Sequence_Num",Sequence_Num)
            };
            DataSet ds = sqlHelper.Query(sql, sp);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                var list = DataHelper<WP_ORDERS>.DataModel(ds.Tables[0]);
                return list[0];
            }
            return null;
        }

        private int GetAddressZoneIdByOrder(SqlHelper sqlHelper, WP_ORDERS order)
        {
            int indexSchoolCode = order.PATIENT_NAME.Replace("，", ",").IndexOf(",");
            string strSchoolCode = string.Empty;
            string sql = string.Empty;
            if (indexSchoolCode > 0)
            {
                strSchoolCode = order.PATIENT_NAME.Substring(0, indexSchoolCode);
                sql = @"SELECT Id FROM [ARO].[dbo].[NA319_ADDRESS_ZONE_ITEMS] WHERE SchoolCode=@SchoolCode;";
            }
            else
            {
                strSchoolCode = order.PATIENT_NAME;
                sql = @"SELECT Id FROM [ARO].[dbo].[NA319_ADDRESS_ZONE_ITEMS] WHERE PatientName=@SchoolCode;";
            }


            System.Data.SqlClient.SqlParameter[] sqlParameters = new System.Data.SqlClient.SqlParameter[] {
                new System.Data.SqlClient.SqlParameter("@SchoolCode",strSchoolCode)
            };
            object o = sqlHelper.ExecuteScalar(sql, sqlParameters);
            int id;
            if (o != null && int.TryParse(o.ToString(), out id))
            {
                return id;
            }
            return 0;
        }

        private int InsertAddressZone(SqlHelper sqlHelper, WP_ORDERS order)
        {
            int indexSchoolCode = order.PATIENT_NAME.Replace("，", ",").IndexOf(",");
            string strSchoolCode = string.Empty;
            if (indexSchoolCode > 0)
            {
                strSchoolCode = order.PATIENT_NAME.Substring(0, indexSchoolCode);
            }
            else
            {
                strSchoolCode = order.PATIENT_NAME;
            }
            string sql = @"INSERT INTO [ARO].[dbo].[NA319_ADDRESS_ZONE_ITEMS]([PatientName],[SchoolCode]) VALUES(@PatientName,@SchoolCode);SELECT @@IDENTITY;";
            System.Data.SqlClient.SqlParameter[] sqlParameters = new System.Data.SqlClient.SqlParameter[] {
                new System.Data.SqlClient.SqlParameter("@PatientName",order.PATIENT_NAME),
                new System.Data.SqlClient.SqlParameter("@SchoolCode",strSchoolCode)
            };
            object o = sqlHelper.ExecuteScalar(sql, sqlParameters);
            return Convert.ToInt32(o.ToString());
        }

        /// <summary>
        /// 将数据插入至显示的表中
        /// </summary>
        /// <param name="orderAddress"></param>
        private void InsertDataGridView(OrderAddress orderAddress, System.Windows.Forms.DataGridView dgvGoodsTable)
        {
            dgvGoodsTable.Rows.Insert(0);
            dgvGoodsTable.Rows[0].Cells["Id"].Value = dgvGoodsTable.Rows.Count;
            dgvGoodsTable.Rows[0].Cells["Sequence_Num"].Value = orderAddress.Sequence_Num;
            dgvGoodsTable.Rows[0].Cells["PatientName"].Value = orderAddress.PatientName;
            dgvGoodsTable.Rows[0].Cells["SchoolCode"].Value = orderAddress.SchoolCode;
            dgvGoodsTable.Rows[0].Cells["CreationTime"].Value = orderAddress.CreationTime.ToString("yyyy-MM-dd HH:mm:ss");
            dgvGoodsTable.Rows[0].Cells["ZoneItemsId"].Value = orderAddress.ZoneItemsId.ToString().PadLeft(5, '0');
        }

        /// <summary>
        /// 插入数据库数据
        /// </summary>
        /// <param name="sqlHelper"></param>
        /// <param name="orderAddress"></param>
        private void AddNA319OrderSplitFlow(SqlHelper sqlHelper, OrderAddress orderAddress)
        {
            string sqlSplitFlow = "INSERT INTO [ARO].[dbo].[NA319_OrderSplitFlow]([AccountName],[ZoneItemsId],[Sequence_Num],[PatientName],[SchoolCode],[CreationTime]) VALUES(@AccountName,@ZoneItemsId,@Sequence_Num,@PatientName,@SchoolCode,@CreationTime)";
            SqlParameter[] sqlSplitFlowParameters = new SqlParameter[] {
                        new SqlParameter("@AccountName",orderAddress.AccountName),
                        new SqlParameter("@ZoneItemsId",orderAddress.ZoneItemsId),
                        new SqlParameter("@Sequence_Num",orderAddress.Sequence_Num),
                        new SqlParameter("@PatientName",orderAddress.PatientName),
                        new SqlParameter("@SchoolCode",orderAddress.SchoolCode),
                        new SqlParameter("@CreationTime",orderAddress.CreationTime)
                    };
            int i = sqlHelper.ExecuteNonQuery(sqlSplitFlow, sqlSplitFlowParameters);
            if (i == 0)
            {
                Console.WriteLine("插入失败");
            }
            else
            {
                Console.WriteLine("插入成功");
            }
        }

        /// 判断是否是数字
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public bool IsNumeric(string number)
        {
            try
            {
                int.Parse(number);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void DeleteButton(string crn, bool flagCRN, DatabaseSetSettingInfo db, System.Windows.Forms.DataGridView dgvGoodsTable, ref List<OrderAddress> alloAList)
        {
            string sqlDelete = "";

            if (flagCRN)
            {
                int intcrn = Convert.ToInt32(crn);
                sqlDelete = string.Format("DELETE FROM [ARO].[dbo].[NA319_OrderSplitFlow] WHERE 1=1 AND ZoneItemsId='{0}'", intcrn);
            }
            else
            {
                sqlDelete = string.Format("DELETE FROM [ARO].[dbo].[NA319_OrderSplitFlow] WHERE 1=1 AND Sequence_Num='{0}'", crn);
            }


            SqlHelper sqlHelper = new SqlHelper(db.dbHost, db.dbName, db.dbUser, db.dbPwd);
            int iVlue = sqlHelper.ExecuteNonQuery(sqlDelete, new SqlParameter[] { });
            if (iVlue == 0)
            {
                MessageBox.Show("未找到你要删除的数据！");
                return;
            }
            else
            {
                dgvGoodsTable.Rows.Clear();
                alloAList.Clear();
                bool flag = RenewLastData(db, dgvGoodsTable, ref alloAList);

                if (flag && iVlue > 0)
                {
                    MessageBox.Show("删除成功！");
                    return;
                }
            }
        }

        public bool RenewLastData(DatabaseSetSettingInfo db, System.Windows.Forms.DataGridView dgvGoodsTable, ref List<OrderAddress> alloAList)
        {
            string sqlSelectAll = "SELECT * FROM [ARO].[dbo].[NA319_OrderSplitFlow] WHERE 1=1 ORDER BY ZoneItemsId DESC,Sequence_Num ASC";
            SqlHelper sqlHelper = new SqlHelper(db.dbHost, db.dbName, db.dbUser, db.dbPwd);
            DataSet ds = sqlHelper.Query(sqlSelectAll, new SqlParameter[] { });
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                List<OrderAddress> orderList = DataHelper<OrderAddress>.DataModel(ds.Tables[0]).ToList();
                int i = 0;
                foreach (var item in orderList)
                {
                    if (alloAList.Any(p => p.Sequence_Num == item.Sequence_Num))
                    {
                        continue;
                    }

                    OrderAddress orderAddress = new OrderAddress()
                    {
                        Id = item.Id,
                        AccountName = item.AccountName,
                        ZoneItemsId = item.ZoneItemsId,
                        Sequence_Num = item.Sequence_Num,
                        PatientName = item.PatientName,
                        SchoolCode = item.SchoolCode,
                        CreationTime = item.CreationTime
                    };
                    alloAList.Add(orderAddress);
                    {
                        InsertDataGridView(orderAddress, dgvGoodsTable);
                        dgvGoodsTable.ResumeLayout();
                        dgvGoodsTable.Update();
                    }

                    Thread.Sleep(1);

                }

                {//绑定的方式，但不能一条条插入
                    //dgvGoodsTable.DataSource = ds.Tables[0];
                    //dgvGoodsTable.Columns[0].HeaderText = "Id";
                    //dgvGoodsTable.Columns[0].DataPropertyName = "Id";
                    //dgvGoodsTable.Columns[1].HeaderText = "流水号";
                    //dgvGoodsTable.Columns[1].DataPropertyName = "Sequence_Num";
                    //dgvGoodsTable.Columns[2].HeaderText = "患者名称";
                    //dgvGoodsTable.Columns[2].DataPropertyName = "PatientName";
                    //dgvGoodsTable.Columns[3].HeaderText = "学校编码";
                    //dgvGoodsTable.Columns[3].DataPropertyName = "SchoolCode";
                    //dgvGoodsTable.Columns[4].HeaderText = "分箱编号";
                    //dgvGoodsTable.Columns[4].DataPropertyName = "ZoneItemsId";
                    //dgvGoodsTable.Columns[5].HeaderText = "创建时间";
                    //dgvGoodsTable.Columns[5].DataPropertyName = "CreationTime";

                    //dgvGoodsTable.DataSource = new BindingList<OrderAddress>(alloAList.ToList());
                    //dgvGoodsTable.AutoGenerateColumns = false;


                }
            }
            return true;
        }

        public void Excelexport(List<OrderAddress> list, string filePath)
        {
            try
            {
                IWorkbook workbook = new HSSFWorkbook();
                int dtRowsCount = list.Count;
                int SheetCount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(dtRowsCount) / 65536));
                int SheetNum = 1;
                int tempIndex = 1; //标示 
                ISheet sheet = workbook.CreateSheet("sheet" + SheetNum);
                for (int i = 0; i < dtRowsCount; i++)
                {
                    if (i == 0 || tempIndex == 1)
                    {
                        IRow row0 = sheet.CreateRow(0);
                        row0.CreateCell(0).SetCellValue("流水号");
                        row0.CreateCell(1).SetCellValue("客户学校代码");
                        row0.CreateCell(2).SetCellValue("明达分区编码");
                    }

                    HSSFRow row = (HSSFRow)sheet.CreateRow(tempIndex);
                    var j = tempIndex + (SheetNum - 1) * tempIndex;
                    row.CreateCell(0).SetCellValue(list[i].Sequence_Num);
                    row.CreateCell(1).SetCellValue(list[i].SchoolCode);
                    row.CreateCell(2).SetCellValue(list[i].ZoneItemsId.ToString().PadLeft(5, '0'));
                    if (tempIndex == 65535)
                    {
                        SheetNum++;
                        sheet = workbook.CreateSheet("sheet" + SheetNum);
                        tempIndex = 0;
                    }
                    tempIndex++;
                }

                using (FileStream fs = System.IO.File.OpenWrite(filePath))
                {
                    workbook.Write(fs);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw ex;
            }
        }




        /// <summary>
        /// 将dataGridView数据转成DataTable
        /// </summary>
        /// <param name="dgv"></param>
        /// <returns></returns>
        public DataTable GetDgvToTable(System.Windows.Forms.DataGridView dgv)
        {
            {
                //如已绑定过数据源：
                DataTable dt = (dgv.DataSource as DataTable);
            }

            {
                DataTable dt = new DataTable();

                // 列强制转换
                for (int count = 0; count < dgv.Columns.Count; count++)
                {
                    DataColumn dc = new DataColumn(dgv.Columns[count].Name.ToString());
                    dt.Columns.Add(dc);
                }

                // 循环行
                for (int count = 0; count < dgv.Rows.Count; count++)
                {
                    DataRow dr = dt.NewRow();
                    for (int countsub = 0; countsub < dgv.Columns.Count; countsub++)
                    {
                        dr[countsub] = Convert.ToString(dgv.Rows[count].Cells[countsub].Value);
                    }
                    dt.Rows.Add(dr);
                }
                return dt;
            }
        }
    }
}
