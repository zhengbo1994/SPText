using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Consul;
using Microsoft.AspNetCore.Mvc;
using SPCoreApiText.Utiltiy;

namespace SPCoreText.Controllers
{
    public class TextController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ApiIndex()
        {
            return View();
        }

        public IActionResult Info()
        {
            List<Users> userList = new List<Users>();
            string resultUrl = null;

            #region 直接调用
            {
                //string url = "http://localhost:5726/api/users/get";
                //string result = WebApiHelperExtend.InvokeApi(url);
                //userList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Users>>(result);
                //resultUrl = url;
            }
            #endregion

            #region 这么多地址你怎么管理？1 2 3 要累死--可以自行选择

            #endregion

            #region 通过consul去发现这些服务地址
            {
                //using (ConsulClient client = new ConsulClient(c =>
                //{
                //    c.Address = new Uri("http://localhost:8500/");
                //    c.Datacenter = "dc1";
                //}))
                //{
                //    var dictionary = client.Agent.Services().Result.Response;
                //    string message = "";
                //    foreach (var keyValuePair in dictionary)
                //    {
                //        AgentService agentService = keyValuePair.Value;
                //        this._logger.LogWarning($"{agentService.Address}:{agentService.Port} {agentService.ID} {agentService.Service}");//找的是全部服务 全部实例  其实可以通过ServiceName筛选
                //        message += $"{agentService.Address}:{agentService.Port};";
                //    }
                //    //获取当前consul的全部服务
                //    base.ViewBag.Message = message;
                //}
            }
            #endregion

            #region 调用---负载均衡
            {
                //string url = "http://localhost:5726/api/users/get";
                //string url = "http://localhost:5727/api/users/get";
                //string url = "http://localhost:5728/api/users/get";
                string url = "http://ZhaoxiUserService/api/users/get";
                //consul解决使用服务名字 转换IP:Port----DNS

                Uri uri = new Uri(url);
                string groupName = uri.Host;
                using (ConsulClient client = new ConsulClient(c =>
                {
                    c.Address = new Uri("http://localhost:8500/");
                    c.Datacenter = "dc1";
                }))
                {
                    var dictionary = client.Agent.Services().Result.Response;
                    var list = dictionary.Where(k => k.Value.Service.Equals(groupName, StringComparison.OrdinalIgnoreCase));//获取consul上全部对应服务实例
                    KeyValuePair<string, AgentService> keyValuePair = new KeyValuePair<string, AgentService>();
                    //拿到3个地址，只需要从中选择---可以在这里做负载均衡--
                    //{
                    //    keyValuePair = list.First();//直接拿的第一个
                    //}
                    //{
                    //    var array = list.ToArray();
                    //    //随机策略---平均策略
                    //    keyValuePair = array[new Random(iSeed++).Next(0, array.Length)];
                    //}
                    //{
                    //    var array = list.ToArray();
                    //    //轮询策略---平均策略
                    //    keyValuePair = array[iSeed++ % array.Length];
                    //}
                    {
                        //权重---注册服务时指定权重，分配时获取权重并以此为依据
                        List<KeyValuePair<string, AgentService>> pairsList = new List<KeyValuePair<string, AgentService>>();
                        foreach (var pair in list)
                        {
                            int count = int.Parse(pair.Value.Tags?[0]);
                            for (int i = 0; i < count; i++)
                            {
                                pairsList.Add(pair);
                            }
                        }
                        keyValuePair = pairsList.ToArray()[new Random(iSeed++).Next(0, pairsList.Count())];
                    }
                    resultUrl = $"{uri.Scheme}://{keyValuePair.Value.Address}:{keyValuePair.Value.Port}{uri.PathAndQuery}";
                    string result = WebApiHelperExtend.InvokeApi(resultUrl, HttpMethod.Get);
                    userList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Users>>(result);
                }
            }
            #endregion
            base.ViewBag.Users = userList;
            base.ViewBag.Url = resultUrl;
            return View();
        }

        private static int iSeed = 0;//没考虑溢出问题
    }
}