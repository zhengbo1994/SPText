﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SPText.Common.Crawler
{
    /// <summary>
    /// http://tool.sufeinet.com/HttpHelper.aspx
    /// </summary>
    public partial class HttpHelper
    {
        /// <summary>
        /// 根据url下载内容  之前是GB2312
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string DownloadUrl(string url)
        {
            return DownloadHtml(url, Encoding.UTF8);
        }

        //HttpClient--WebApi

        /// <summary>
        /// 下载html
        /// http://tool.sufeinet.com/HttpHelper.aspx
        /// HttpWebRequest功能比较丰富，WebClient使用比较简单
        /// WebRequest
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string DownloadHtml(string url, Encoding encode)
        {
            string html = string.Empty;
            try
            {

                //可以使用httpWebRequest  也可以使用HttpClent
                //https可以下载--

                //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback((object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors) =>
                //{
                //    return true; //总是接受  
                //});
                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;//模拟请求
                request.Timeout = 30 * 1000;//设置30s的超时
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.106 Safari/537.36";
                //request.UserAgent = "User - Agent:Mozilla / 5.0(iPhone; CPU iPhone OS 7_1_2 like Mac OS X) App leWebKit/ 537.51.2(KHTML, like Gecko) Version / 7.0 Mobile / 11D257 Safari / 9537.53";

                request.ContentType = "text/html; charset=utf-8";// "text/html;charset=gbk";// 

                //request.Host = "search.yhd.com"; 
                //request.Headers.Add("Cookie", @"newUserFlag=1; guid=YFT7C9E6TMFU93FKFVEN7TEA5HTCF5DQ26HZ; gray=959782; cid=av9kKvNkAPJ10JGqM_rB_vDhKxKM62PfyjkB4kdFgFY5y5VO; abtest=31; _ga=GA1.2.334889819.1425524072; grouponAreaId=37; provinceId=20; search_showFreeShipping=1; rURL=http%3A%2F%2Fsearch.yhd.com%2Fc0-0%2Fkiphone%2F20%2F%3Ftp%3D1.1.12.0.73.Ko3mjRR-11-FH7eo; aut=5GTM45VFJZ3RCTU21MHT4YCG1QTYXERWBBUFS4; ac=57265177%40qq.com; msessionid=H5ACCUBNPHMJY3HCK4DRF5VD5VA9MYQW; gc=84358431%2C102362736%2C20001585%2C73387122; tma=40580330.95741028.1425524063040.1430288358914.1430790348439.9; tmd=23.40580330.95741028.1425524063040.; search_browse_history=998435%2C1092925%2C32116683%2C1013204%2C6486125%2C38022757%2C36224528%2C24281304%2C22691497%2C26029325; detail_yhdareas=""; cart_cookie_uuid=b64b04b6-fca7-423b-b2d1-ff091d17e5e5; gla=20.237_0_0; JSESSIONID=14F1F4D714C4EE1DD9E11D11DDCD8EBA; wide_screen=1; linkPosition=search");

                //request.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                //request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch");
                //request.Headers.Add("Referer", "http://list.yhd.com/c0-0/b/a-s1-v0-p1-price-d0-f0-m1-rt0-pid-mid0-kiphone/");

                //Encoding enc = Encoding.GetEncoding("GB2312"); // 如果是乱码就改成 utf-8 / GB2312

                //如何自动读取cookie
                request.CookieContainer = new CookieContainer();//1 给请求准备个container
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)//发起请求
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        //logger.Warn(string.Format("抓取{0}地址返回失败,response.StatusCode为{1}", url, response.StatusCode));
                    }
                    else
                    {
                        try
                        {
                            //string sessionValue = response.Cookies["ASP.NET_SessionId"].Value;//2 读取cookie
                            StreamReader sr = new StreamReader(response.GetResponseStream(), encode);
                            html = sr.ReadToEnd();//读取数据
                            sr.Close();
                        }
                        catch (Exception ex)
                        {
                            //logger.Error(string.Format($"DownloadHtml抓取{url}失败"), ex);
                            html = null;
                        }
                    }
                }
            }
            catch (System.Net.WebException ex)
            {
                if (ex.Message.Equals("远程服务器返回错误: (306)。"))
                {
                    //logger.Error("远程服务器返回错误: (306)。", ex);
                    html = null;
                }
            }
            catch (Exception ex)
            {
                //logger.Error(string.Format("DownloadHtml抓取{0}出现异常", url), ex);
                html = null;
            }
            return html;
        }


        public static string DownloadJsonData(string url, Encoding encode)
        {
            string html = string.Empty;
            try
            {
                HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;//模拟请求
                request.Timeout = 30 * 1000;//设置30s的超时
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";

                request.ContentType = "application/json;charset=utf-8";
                request.Referer = "https://ke.qq.com/admin/index.html";
                request.Accept = "*/*";


                request.Headers.Add("authority", "ke.qq.com");
                request.Headers.Add("method", "GET");
                request.Headers.Add("path", "/cgi-agency/agency/manager/get_course_list?aid=80207&count=10&page=1&pay_type=2&only_owner=1&cname_key=&course_state=0&signup_state=0&sort_method=0&is_ascending=0&bkn=449651946&t=0.0029");

                request.Headers.Add("scheme", "https");
                request.Headers.Add("accept-encodin", "gzip, deflate, br");
                request.Headers.Add("accept-language", "zh-CN,zh;q=0.9");
                request.Headers.Add("sec-fetch-mode", "cors");
                request.Headers.Add("sec-fetch-site", "same-origin");

                request.CookieContainer = new CookieContainer();//1 给请求准备个container
                request.Headers.Add("cookie", @"pgv_pvi=5087136768; RK=WXbobkaNsr; ptcz=447bb81ae5a54414d650b3476dced39f0cb80683ac471ec83f6cd827e4fdf5b7; pgv_pvid=3854687776; ts_uid=2094536335; localInterest=[2002]; iswebp=1; pac_uid=1_2751435708; ied_qq=o2751435708; isHideDealTips=1; ts_refer=pay.qq.com/enterprise/separate.shtml; tvfe_boss_uuid=86ed223020e11b4d; o_cookie=2751435708; ke_login_type=1; luin=o2751435708; course_origin=[{'cid':297038,'ext':{'pagelocation':'list, 1.3'}}]; tdw_data_testid=; tdw_data_flowid=; tdw_auin_data=-; tdw_first_visited=1; pgv_info=ssid=s6817397715; Hm_lvt_0c196c536f609d373a16d246a117fd44=1575856560,1575893514,1575896171,1575951173; index_new_key={'index_interest_cate_id':2002}; _qpsvr_localtk=0.05340837932628206; pgv_si=s9919754240; miniapp_qrcode_id=0115055f128549d785b0b1593b6084e4; uin=o2751435708; skey=@iGqJqPF6r; ptisp=cnc; lskey=000100005c4c2b2b6499fad1306a5536e639f4503825b33a2f4ee7f6c8ad4b5d43621199c8c99163c07c78c0; p_uin=o2751435708; pt4_token=SXdux*M4NNFzFRs6r59i-wKIVO4xk1AofSqAo8pvGg8_; p_skey=B4sUwtIxpxoGHY7OrQue0wyl2KkTlW4-Jr0MPc7Vf88_; p_luin=o2751435708; p_lskey=000400007af2a05b862ff9c18913d8cb474fb1c0c9f957de9f12b024a4372a5544cd95ecca989efba4edd06a; _pathcode=0.8704610796361238; tdw_data_sessionid=157595933804520618739236; ts_last=ke.qq.com/course/list; Hm_lpvt_0c196c536f609d373a16d246a117fd44=1575959482; tdw_data_new_2={'auin':' - ','sourcetype':'tuin','sourcefrom':'a3ff93bc','ver9':2751435708,'uin':2751435708,'visitor_id':'8882822910493176','url_page':'index','url_module':'searchbar','url_position':''}; tdw_data={'ver4':'4','ver5':'','ver6':'','refer':'','from_channel':'','path':'aBar - 0.8704610796361238','auin':' - ','uin':2751435708,'real_uin':2751435708}");

                //如何自动读取cookie 
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)//发起请求
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        //logger.Warn(string.Format("抓取{0}地址返回失败,response.StatusCode为{1}", url, response.StatusCode));
                    }
                    else
                    {
                        try
                        {
                            //string sessionValue = response.Cookies["ASP.NET_SessionId"].Value;//2 读取cookie
                            StreamReader sr = new StreamReader(response.GetResponseStream(), encode);
                            html = sr.ReadToEnd();//读取数据
                            sr.Close();
                        }
                        catch (Exception ex)
                        {
                            //logger.Error(string.Format($"DownloadHtml抓取{url}失败"), ex);
                            html = null;
                        }
                    }
                }
            }
            catch (System.Net.WebException ex)
            {
                if (ex.Message.Equals("远程服务器返回错误: (306)。"))
                {
                    //logger.Error("远程服务器返回错误: (306)。", ex);
                    html = null;
                }
            }
            catch (Exception ex)
            {
                //logger.Error(string.Format("DownloadHtml抓取{0}出现异常", url), ex);
                html = null;
            }
            return html;
        }

    }
}