using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SPMVCText.Controllers
{
    public class TextController : Controller
    {
        // GET: Text
        public ActionResult Index()
        {
            return View();
        }




        /// <summary>
        /// 下载文件
        /// </summary>
        [HttpGet]
        public void DownloadFile()
        {
            var request = HttpContext.Request;
            NameValueCollection nvCollection = request.Params;
            string fileName = nvCollection.GetValues("fileName")[0];
            string filePath = Path.Combine(HttpContext.Server.MapPath("~/App_Data/"), fileName);
            if (System.IO.File.Exists(filePath))
            {
                HttpResponseBase response = HttpContext.Response;
                response.Clear();
                response.ClearHeaders();
                response.ClearContent();
                response.Buffer = true;
                response.AddHeader("content-disposition", string.Format("attachment; FileName={0}", fileName));
                response.Charset = "GB2312";
                response.ContentEncoding = Encoding.GetEncoding("GB2312");
                response.ContentType = MimeMapping.GetMimeMapping(fileName);
                response.WriteFile(filePath);
                response.Flush();
                response.Close();
            }
        }



        /// <summary>
        /// 上传文件
        /// </summary>
        [HttpPost]
        public string UploadFile()
        {
            string result = string.Empty;
            try
            {
                string uploadPath = HttpContext.Server.MapPath("~/App_Data/");
                HttpRequest request = System.Web.HttpContext.Current.Request;
                HttpFileCollection fileCollection = request.Files;
                // 判断是否有文件
                if (fileCollection.Count > 0)
                {
                    // 获取文件
                    HttpPostedFile httpPostedFile = fileCollection[0];
                    string fileExtension = Path.GetExtension(httpPostedFile.FileName);// 文件扩展名
                    string fileName = Guid.NewGuid().ToString() + fileExtension;// 名称
                    string filePath = uploadPath + httpPostedFile.FileName;// 上传路径
                                                                           // 如果目录不存在则要先创建
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }
                    // 保存新的文件
                    while (System.IO.File.Exists(filePath))
                    {
                        fileName = Guid.NewGuid().ToString() + fileExtension;
                        filePath = uploadPath + fileName;
                    }
                    httpPostedFile.SaveAs(filePath);
                    result = "上传成功";
                }
            }
            catch (Exception)
            {
                result = "上传失败";
            }
            return result;
        }


        /// <summary>
        /// 上传文件
        /// </summary>
        public ActionResult Upload(HttpPostedFileBase file)
        {
            //获取文件名
            var fileName = file.FileName;

            string UpData= HttpContext.Server.MapPath("~/UpData/");
            //判断文件夹是否存在
            if (!Directory.Exists(UpData))
            {
                //不存在创建
                Directory.CreateDirectory(UpData);
            }
            //指定路径
            var filePath = Server.MapPath(string.Format("~/{0}", "UpData"));
            //保存
            file.SaveAs(Path.Combine(filePath, fileName));
            return Json("上传成功");
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        public FileStreamResult Download(string fileName)
        {
            //获取文件路径
            string filePath = Server.MapPath(string.Format("~/{0}/{1}", "App_Data", fileName));
            //通过流读取
            FileStream fs = new FileStream(filePath, FileMode.Open);
            return File(fs, "text/plain", fileName);
        }

    }
}