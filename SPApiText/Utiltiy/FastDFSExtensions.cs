﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using FastDFS.Client;
using System.Web;
using System.IO;

namespace SPCoreApiText.Utiltiy
{
    /// <summary>
    /// 依赖注入的扩展类
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        public static IFastDFSBuilder AddFastDFS(this IServiceCollection services)
        {
            var configuration = services.BuildServiceProvider()
                .GetRequiredService<IConfiguration>();
            // 添加FastDFS的json文件解析
            services.Configure<FastDFSOptions>(configuration.GetSection("FastDFSOptions"));
            return new FastDFSBuilder(services, configuration);
        }

        public static IFastDFSBuilder AddFastDFS(this IServiceCollection services, IConfiguration configuration)
        {
            return new FastDFSBuilder(services, configuration);
        }
    }


    public interface IFastDFSBuilder
    {
        IServiceCollection Services { get; }

        IConfiguration Configuration { get; }
    }


    public class FastDFSBuilder : IFastDFSBuilder
    {
        public IServiceCollection Services { get; }
        public IConfiguration Configuration { get; }

        public FastDFSBuilder(IServiceCollection services, IConfiguration configurationRoot)
        {
            Configuration = configurationRoot;
            Services = services;
            // 单例方式注入到IOC
            Services.TryAddSingleton<FastDFSProvider>();
        }
    }


    /// <summary>
    /// FastDFS操作提供类
    /// </summary>
    public class FastDFSProvider
    {
        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="objectByte">上传对象对应的字节数组</param>
        /// <param name="objectName">对象名</param>
        /// <param name="groupName">分组</param>
        /// <returns></returns>
        public async Task<string> UploadObjectByteAsync(Stream objectStream, string fileName)
        {
            // 获取要上传的storage-server节点
            var storageNode = await FastDFSClient.GetStorageNodeAsync();
            var filePath = await FastDFSClient.UploadFileAsync(storageNode, objectStream, Path.GetExtension(fileName));
            return storageNode.GroupName + "/" + filePath;
        }

        /// <summary>
        /// 文件下载
        /// </summary>
        /// <param name="fileName">下载文件的名称</param>
        /// <returns>返回下载文件对应的字节数组</returns>
        public async Task<byte[]> DownloadObjectByteAsync(string fileName)
        {
            var storageNode = await FastDFSClient.GetStorageNodeAsync();
            byte[] fileContent = await FastDFSClient.DownloadFileAsync(storageNode, fileName);

            return fileContent;
        }

        /// <summary>
        /// 查看文件属性
        /// </summary>
        /// <param name="fileName">查看的文件的名称</param>
        /// <returns>返回查询文件的文件信息</returns>
        public async Task<FDFSFileInfo> ViewObjectByteAsync(string fileName)
        {
            var storageNode = await FastDFSClient.GetStorageNodeAsync();
            var fileInfo = await FastDFSClient.GetFileInfoAsync(storageNode, fileName);

            return fileInfo;
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="fileName">上传的文件的名称</param>
        /// <returns>返回查询文件的文件信息</returns>
        public async Task<bool> DeleteObjectByteAsync(string fileName)
        {
            var storageNode = await FastDFSClient.GetStorageNodeAsync();
            try
            {
                await FastDFSClient.RemoveFileAsync(storageNode.GroupName, fileName);
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }

        }

        /// <summary>
        /// 解码前端进行Html加码的方法
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public string HtmlDecode(string filename)
        {
            return HttpUtility.UrlDecode(filename);
        }
    }


    public class FastDFSOptions
    {
        public List<FastDFSAddress> AddressList { get; set; }
    }

    public class FastDFSAddress
    {
        public string Host { get; set; }
        public int Port { get; set; }
    }
}
