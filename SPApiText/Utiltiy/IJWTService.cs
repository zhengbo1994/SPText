﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SPCoreApiText.Utiltiy
{
    /// <summary>
    /// 简单封装个注入
    /// </summary>
    public interface IJWTService
    {
        /// <summary>
        /// 用户信息
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        string GetToken(CurrentUserModel userInfo);
        string GetToken(string UserName);
    }
}
