using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Reflection;
using System.Web;
using System.Web.Caching;


namespace SPText
{
    public sealed class UserCache
    {
        private readonly Dictionary<string, object> cacheDictionary = new Dictionary<string, object>();
        private readonly object lockObj = new object();

        /// <summary>
        /// 索引器
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>缓存对象</returns>
        public object this[string key]
        {
            get
            {
                lock (lockObj)
                {
                    return cacheDictionary.ContainsKey(key) ? cacheDictionary[key] : null;
                }
            }
            set
            {
                lock (lockObj)
                {
                    if (cacheDictionary.ContainsKey(key))
                    {
                        cacheDictionary[key] = value;
                    }
                    else
                    {
                        cacheDictionary.Add(key, value);
                    }
                }
            }
        }

        public void Remove(string key)
        {
            lock (lockObj)
            {
                if (cacheDictionary.ContainsKey(key))
                {
                    cacheDictionary.Remove(key);
                }
            }
        }

        public void Clear()
        {
            lock (lockObj)
            {
                cacheDictionary.Clear();
            }
        }
    }
    /// <summary>
    /// 缓存操作类
    /// </summary>
    public class WebCache
    {
        #region 私有变量

        private const string UserIdentifyKey = "CacheUserIdentifyKey";

        #endregion

        #region 公共方法

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public static object GetCache(string key)
        {
            return GetUserCache()[key];
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static bool SetCache(string key, object value)
        {
            try
            {
                var userCache = GetUserCache();
                userCache[key] = value;
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 清空缓存
        /// </summary>
        /// <returns></returns>
        public static bool ClearCache()
        {
            try
            {
                // 只清除缓存内容
                // GetUserCache().Clear();

                // 直接从Cache里移除
                var identify = GetUserIdentify();
                HttpContext.Current.Cache.Remove(identify);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public static bool RemoveCache(string key)
        {
            try
            {
                GetUserCache().Remove(key);
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region 私有方法

        private static string GetUserIdentify()
        {
            if (HttpContext.Current.Session[UserIdentifyKey] != null)
                return HttpContext.Current.Session[UserIdentifyKey].ToString();
            var identify = Guid.NewGuid().ToString();
            HttpContext.Current.Session[UserIdentifyKey] = identify;
            return identify;
        }

        private static UserCache GetUserCache()
        {
            var identify = GetUserIdentify();
            if (HttpContext.Current.Cache.Get(identify) == null)
            {
                HttpContext.Current.Cache.Insert(identify, new UserCache(), null, Cache.NoAbsoluteExpiration,
                    new TimeSpan(0, 20, 0), CacheItemPriority.High, CacheRemovedCallback);
            }
            return HttpContext.Current.Cache.Get(identify) as UserCache; // as是一种强制类型转化的方式
        }

        /// <summary>
        /// 缓存被移除时触发
        /// </summary>
        /// <param name="key">被移除的缓存的key</param>
        /// <param name="value">被移除的缓存的值</param>
        /// <param name="reason">移除原因</param>
        private static void CacheRemovedCallback(string key, object value, CacheItemRemovedReason reason)
        {
            // 缓存被移除时执行的操作
            // 如果是手动移除，则不处理
            //if (reason == CacheItemRemovedReason.Removed)
            //    return;

            // 此处访问页面会报错，暂时注释掉
            // ShowNotification(MessageType.Warning, "警告", "由于您太久没操作页面已过期，请重新登录！", true);
        }

        #endregion
    }
}
