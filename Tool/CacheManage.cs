using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Tool
{
    public static class CacheManage
    {
        /// <summary>
        /// 从缓存中读取
        /// </summary>
        /// <returns></returns>
        public static object GetSearch(string cacheKey)
        {
            object obj = HttpRuntime.Cache.Get(cacheKey);
            if (obj == null)
            {
                return null;
            }

            return HttpRuntime.Cache.Get(cacheKey);
        }

        /// <summary>
        /// 加入或者刷新缓存
        /// </summary>
        /// <param name="obj"></param>
        public static void InsertCache(string cacheKey, object obj)
        {
            HttpRuntime.Cache.Insert(cacheKey, obj);
        }

        /// <summary>
        /// 删除缓存
        /// </summary>
        public static void DeleteCache(string cacheKey)
        {
            HttpRuntime.Cache.Remove(cacheKey);
        }
    }
}
