using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace ReportPrinting.Model
{
    [Serializable]
    public class PrinterAssembleInfo
    {
        public PrinterAssembleInfo()
        {
            this.IsRun = false;
            this.IsRunView = IsRun ? "已开启" : "未开启";
            this.Status = "空";
            this.ControlView = "Run";
            this.Note = new Dictionary<DateTime, string>();
            this.isNoteFormShow = false;
        }
        public string Id { get; set; }

        //打印机化名
        public string PrinterAlias { get; set; }

        //选定的打印机名称
        public string PrinterName { get; set; }

        //选定的该打印机中盒子的名称
        public string PrinterBoxName { get; set; }

        //报表名
        public string ReportName { get; set; }

        //是否Run
        public bool IsRun { get; set; }

        // 开启/停止
        public string IsRunView { get; set; }

        //当前的状态 例如：13/20
        public string Status { get; set; }

        //开关Button的显示 Run/Stop/Wait
        public string ControlView { get; set; }

        //详细内容
        public Dictionary<DateTime, string> Note { get; set; }

        //详情日志窗口是否打开中
        public bool isNoteFormShow { get; set; }

        public PrinterAssembleInfo Clone()
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, this);
            stream.Position = 0;
            return formatter.Deserialize(stream) as PrinterAssembleInfo;
        }
    }

    public static class ModelExt
    {
        public static void AddNote(this PrinterAssembleInfo info, string str)
        {
            if (info != null)
            {
                info.Note.Add(DateTime.Now, str);
                Ext.Delay(1);//休息1毫秒
            }
        }
    }
}
