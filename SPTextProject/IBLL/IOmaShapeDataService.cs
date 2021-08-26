using SPTextProject.Models.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SPTextProject.IBLL
{
    /// <summary>
    /// 对OMA图形数据的操作接口
    /// </summary>
    public interface IOmaShapeDataService
    {
        public void Execute(OutOrder outOrder)
        {
            var fileUrl = GetOmaFileUrlByBarcode(outOrder.Barcode);
            if (string.IsNullOrEmpty(fileUrl))
            {
                return;
            }
            SendOmaData(outOrder, GetOmaDataContent(fileUrl));
        }

        /// <summary>
        /// 通过跟单条码获取OMA文件的URL地址
        /// </summary>
        /// <param name="barcode">跟单条码</param>
        /// <returns>URL地址</returns>
        string GetOmaFileUrlByBarcode(string barcode);

        /// <summary>
        /// 获取OMA图形数据内容
        /// </summary>
        /// <param name="url">OMA图形数据URL地址</param>
        /// <returns>OMA图形数据文内容</returns>
        string GetOmaDataContent(string url);

        /// <summary>
        /// 发送OMA图形数据到表中
        /// </summary>
        /// <param name="order">工厂订单yjjdk实体</param>
        /// <param name="content">读取到的OMA图形数据文内容</param>
        void SendOmaData(OutOrder order, string content);
    }
}
