using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;

namespace SPCoreApiText.Utiltiy
{
    /// <summary>
    /// 自己封装的注册类
    /// </summary>
    public static class ConsulHelper
    {
        public static void ConsulRegist(this IConfiguration configuration)
        {
            ConsulClient client = new ConsulClient(c =>
            {
                c.Address = new Uri("http://localhost:8500/");
                c.Datacenter = "dc1";
            });
            string ip = configuration["ip"];
            int port = int.Parse(configuration["port"]);//命令行参数必须传入
            int weight = string.IsNullOrWhiteSpace(configuration["weight"]) ? 1 : int.Parse(configuration["weight"]);//命令行参数必须传入
            client.Agent.ServiceRegister(new AgentServiceRegistration()
            {
                ID = "service" + Guid.NewGuid(),////服务编号，不能重复，用Guid最简单
                Name = "ConsulHelperService",//服务的名字
                Address = ip,//我的ip地址(可以被其他应用访问的地址，本地测试可以用127.0.0.1，机房环境中一定要写自己的内网ip地址)
                Port = port,//我的端口
                Tags = new string[] { weight.ToString() },//标签
                Check = new AgentServiceCheck()//配置心跳检查的
                {
                    Interval = TimeSpan.FromSeconds(12),//健康检查时间间隔，或者称为心跳间隔
                    HTTP = $"http://{ip}:{port}/Api/Health/Index",//健康检查地址
                    Timeout = TimeSpan.FromSeconds(5),
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5)////服务停止多久后反注册
                }
            });
            Console.WriteLine($"http://{ip}:{port}完成注册");
        }

        public static async Task UseConsul(this IApplicationBuilder app, ConsulConfigModel consulService, HealthConfigModel healthService)
        {

            string ip = healthService.IP;// configuration["ip"];
            int port = healthService.Port;// int.Parse(configuration["port"]);//命令行参数必须传入
            //int weight = string.IsNullOrWhiteSpace(configuration["weight"]) ? 1 : int.Parse(configuration["weight"]);//命令行参数必须传入

            using (ConsulClient client = new ConsulClient(c =>
            {
                c.Address = new Uri($"http://{consulService.IP}:{consulService.Port}/");
                c.Datacenter = "dc1";
            }))
            {
                await client.Agent.ServiceRegister(new AgentServiceRegistration()
                {
                    ID = "grpcService" + ip + ":" + port,//唯一的
                    Name = healthService.GroupName,//组名称-Group
                    Address = ip,
                    Port = port,
                    Tags = healthService.Tag,
                    Check = new AgentServiceCheck()
                    {
                        Interval = TimeSpan.FromSeconds(12),
                        HTTP = $"http://{ip}:{healthService.CheckPort}/Health",
                        Timeout = TimeSpan.FromSeconds(5),
                        DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5)
                    }
                });
                Console.WriteLine($"http://{ip}:{port}完成注册");

                //client.KV.Put(new KVPair("Eleven") { Value = Encoding.UTF8.GetBytes("This is Teacher") });
                //Console.WriteLine(client.KV.Get("Eleven"));
                //client.KV.Delete("Eleven");

                //client.KV.Delete("Eleven");
                //直接查看KV类里面的封装

                //client.ACL.

                //client.ExecuteLocked
            }
        }
    }
}
