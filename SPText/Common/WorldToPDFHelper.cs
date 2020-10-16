using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Aspose.Words;
using System.IO;
using Microsoft.Office.Interop.Word;

namespace SPText.Common
{
    public class WorldToPDFHelper
    {
        public bool WordToPDF2(string sourcePath)
        {
            bool result = false;
            Application application = new Application();
            Microsoft.Office.Interop.Word.Document document = null;

            try
            {
                application.Visible = false;

                document = application.Documents.Open(sourcePath);

                string PDFPath = sourcePath.Replace(".doc", ".pdf");//pdf存放位置

                if (!File.Exists(PDFPath))//存在PDF，不需要继续转换
                {
                    document.ExportAsFixedFormat(PDFPath, WdExportFormat.wdExportFormatPDF);
                }
                result = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                result = false;
            }
            finally
            {
                document.Close();
            }
            return result;
        }
        public bool WordToPDF1(string sourcePath)
        {
            try
            {
                Aspose.Words.Document doc = new Aspose.Words.Document(sourcePath);
                string targetPath = sourcePath.ToUpper().Replace(".DOCX", ".PDF");
                doc.Save(targetPath, SaveFormat.Pdf);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            return true;
        }

    }
}

