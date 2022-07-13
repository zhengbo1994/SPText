using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace SPText.Common.Crawler
{
    public class CourseSearch
    {
        private TencentCategoryEntity category = null;

        public CourseSearch()
        {

        }

        public CourseSearch(TencentCategoryEntity _category)
        {
            category = _category;
        }

        public void Crawler()
        {
            try
            {
                if (string.IsNullOrEmpty(category.Url))
                {
                    //warnRepository.SaveWarn(category, string.Format("Url为空,Name={0} Level={1} Url={2}", category.Name, category.CategoryLevel, category.Url));
                    return;
                }
                {
                    #region 分页获取  
                    //ImageHelper.DeleteDir(Constant.ImagePath);
                    GetPageCourseData();
                    #endregion

                    #region 获取某一页的数据
                    //this.Show(category.Url);
                    #endregion
                }
            }
            catch (Exception ex)
            {
                //warnRepository.SaveWarn(category, string.Format("出现异常,Name={0} Level={1} Url={2}", category.Name, category.CategoryLevel, category.Url));
            }
        }

        static int count = 0;

        //这个爬虫定制的套路如果能理解；刷个1
        public void Show(string url)
        {
            string strHtml = HttpHelper.DownloadUrl(url);
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(strHtml);
            string liPath = "/html/body/section[1]/div/div[@class='market-bd market-bd-6 course-list course-card-list-multi-wrap js-course-list']/ul/li";
            HtmlNodeCollection liNodes = document.DocumentNode.SelectNodes(liPath);
            foreach (var node in liNodes)
            {
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("************************************************");
                HtmlDocument lidocument = new HtmlDocument();
                lidocument.LoadHtml(node.OuterHtml);
                string aPath = "//*/a[1]";
                HtmlNode classANode = lidocument.DocumentNode.SelectSingleNode(aPath);
                string aHref = classANode.Attributes["href"].Value;

                Console.WriteLine($"课程Url:{aHref}");

                string Id = classANode.Attributes["data-id"].Value;

                Console.WriteLine($"课程Id:{Id}");

                string imgPath = "//*/a[1]/img";
                HtmlNode imgNode = lidocument.DocumentNode.SelectSingleNode(imgPath);
                string imgUrl = imgNode.Attributes["src"].Value;

                Console.WriteLine($"ImageUrl:{imgUrl}");

                string namePaths = "//*/h4/a[1]";
                HtmlNode nameNode = lidocument.DocumentNode.SelectSingleNode(namePaths);
                string name = nameNode.InnerText;
                Console.WriteLine(name);

                Console.WriteLine($"课程名称:{name}");
                // courseEntity.Price = new Random().Next(100, 10000);  //关于腾讯课堂上的课程价格抓取 这是一个进阶内容  通过普通方式搞不了（他有一个自己的算法） 

                count = count + 1;
            }
        }

        public void ShowPageData(string url)
        {
            string strHtml = HttpHelper.DownloadUrl(url);
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(strHtml);
            string pagePath = "/html/body/section[1]/div/div[5]/a[@class='page-btn']";
            HtmlNodeCollection pageNodes = document.DocumentNode.SelectNodes(pagePath);
            int maxPage = pageNodes.Select(p => int.Parse(p.InnerText)).Max();
            for (int page = 1; page <= maxPage; page++)
            {
                string pageUrl = $"{url}&page={page}";
                Show(pageUrl);
            }
            Console.WriteLine($"一共抓取数据{count}条");
        }

        #region 分页抓取 
        private void GetPageCourseData()
        {
            //1. 确定总页数
            //2. 分别抓取每一页的数据
            //3. 分析  过滤  清洗
            //4. 入库 

            category.Url = $"https://ke.qq.com{category.Url}";

            string strHtml = HttpHelper.DownloadUrl(category.Url);
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(strHtml);
            //Xpath
            string pagePath = "/html/body/section[1]/div/div[@class='sort-page']/a[@class='page-btn']";
            HtmlNodeCollection pageNodes = document.DocumentNode.SelectNodes(pagePath);

            int pageCount = 1;
            if (pageNodes != null)
            {
                pageCount = pageNodes.Select(a => int.Parse(a.InnerText)).Max();
            }
            List<CourseEntity> courseList = new List<CourseEntity>();

            for (int pageIndex = 1; pageIndex <= pageCount; pageIndex++)
            {
                Console.WriteLine($"******************************当前是第{pageIndex}页数据************************************");
                string pageIndexUrl = $"{category.Url}&page={pageIndex}";
                List<CourseEntity> courseEntities = GetPageIndeData(pageIndexUrl);
                courseList.AddRange(courseEntities);
            }
            //courseRepository.SaveList(courseList);


        }

        private List<CourseEntity> GetPageIndeData(string url)
        {
            //获取li标签里面的数据 
            // 先获取所有的Li 
            //  然后循环获取li中的有效数据
            string strHtml = HttpHelper.DownloadUrl(url);
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(strHtml);
            string liPath = "/html/body/section[1]/div/div[@class='market-bd market-bd-6 course-list course-card-list-multi-wrap js-course-list']/ul/li";
            HtmlNodeCollection liNodes = document.DocumentNode.SelectNodes(liPath);
  

            List<CourseEntity> courseEntities = new List<CourseEntity>();
            if (liNodes == null)
            {
                return courseEntities;
            }
            foreach (var node in liNodes)
            {
                CourseEntity courseEntity = GetLiData(node);
                courseEntities.Add(courseEntity);
            }
            return courseEntities;
        }

        /// <summary>
        /// 当我们把这些数据获取到以后，那就应该保存起来
        /// </summary>
        /// <param name="node"></param>
        private CourseEntity GetLiData(HtmlNode node)
        {
            CourseEntity courseEntity = new CourseEntity();
            //从这里开始 
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(node.OuterHtml);
            string aPath = "//*/a[1]";
            HtmlNode classANode = document.DocumentNode.SelectSingleNode(aPath);
            string aHref = classANode.Attributes["href"].Value;
            courseEntity.Url = aHref;

            Console.WriteLine($"课程Url:{aHref}");

            string Id = classANode.Attributes["data-id"].Value;

            Console.WriteLine($"课程Id:{Id}");

            courseEntity.CourseId = long.Parse(Id);

            string imgPath = "//*/a[1]/img";
            HtmlNode imgNode = document.DocumentNode.SelectSingleNode(imgPath);
            string imgUrl = imgNode.Attributes["src"].Value;
            courseEntity.ImageUrl = imgUrl;

            Console.WriteLine($"ImageUrl:{imgUrl}");

            string namePaths = "//*/h4/a[1]";
            HtmlNode nameNode = document.DocumentNode.SelectSingleNode(namePaths);
            string name = nameNode.InnerText;

            courseEntity.Title = name;

            Console.WriteLine($"课程名称:{name}");

            courseEntity.Price = new Random().Next(100, 10000);  //关于腾讯课堂上的课程价格抓取 这是一个进阶内容  通过普通方式搞不了（他有一个自己的算法） 
            return courseEntity;

        }
        #endregion

        #region 获取Ajax 请求数据
        /// <summary>
        /// 1.匹配页面和请求URL
        /// 2.获取请求的数据
        /// 3.解析数据：根据json格式数据建立实体  HashTable
        /// 
        /// </summary>
        /// <returns></returns>
        public void GetAjaxRequest()
        {
            string url = "https://ke.qq.com/cgi-bin/get_cat_info?bkn=449651946&r=0.36532379182727115";
            var ajaxData = HttpHelper.DownloadHtml(url, Encoding.UTF8);

            Hashtable hashtable = JsonConvert.DeserializeObject<Hashtable>(ajaxData);
            string result = hashtable["result"].ToString();
            Hashtable hashResult = JsonConvert.DeserializeObject<Hashtable>(result);

            Dictionary<string, string> dicResult = JsonConvert.DeserializeObject<Dictionary<string, string>>(result);

            string catInfo = hashResult["catInfo"].ToString();

            //同学们建议很多，在这儿就不全部尝试了
            //dynamic dynamicCatInfo = JsonConvert.DeserializeObject<dynamic>(catInfo);

            //Hashtable hashcatInfo = JsonConvert.DeserializeObject<Hashtable>(catInfo);

            //foreach (var hashItem in hashcatInfo)
            //{
            //    JsonConvert.DeserializeObject<Hashtable>(hashItem["1001"].ToString());
            //}

            //Hashtable cat1001 = JsonConvert.DeserializeObject<Hashtable>(hashcatInfo["1001"].ToString());
            //Console.WriteLine($"类别为：{cat1001["n"]}");

            //Hashtable cat1002 = JsonConvert.DeserializeObject<Hashtable>(hashcatInfo["1002"].ToString());
            //Console.WriteLine($"类别为：{cat1002["n"]}");

            //Hashtable cat1003 = JsonConvert.DeserializeObject<Hashtable>(hashcatInfo["1003"].ToString());
            //Console.WriteLine($"类别为：{cat1003["n"]}");

            //Hashtable cat1004 = JsonConvert.DeserializeObject<Hashtable>(hashcatInfo["1004"].ToString());
            //Console.WriteLine($"类别为：{cat1004["n"]}");

            //Hashtable cat1005 = JsonConvert.DeserializeObject<Hashtable>(hashcatInfo["1005"].ToString());
            //Console.WriteLine($"类别为：{cat1005["n"]}");

            //Hashtable cat1006 = JsonConvert.DeserializeObject<Hashtable>(hashcatInfo["1006"].ToString());
            //Console.WriteLine($"类别为：{cat1006["n"]}");




        }
        #endregion 
    }


    public class CourseEntity
    {
        public int Id { get; set; }
        public long CourseId { get; set; }
        public int CategoryId { get; set; }  //类别Id
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
    }
}
