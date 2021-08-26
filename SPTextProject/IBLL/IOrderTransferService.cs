using System;
using System.Collections.Generic;
using System.Text;

namespace SPTextProject.IBLL
{
    public interface IOrderTransferService
    {
        /// <summary>
        /// 派发订单到各个工厂
        /// </summary>
        void Transfer();
    }
}
