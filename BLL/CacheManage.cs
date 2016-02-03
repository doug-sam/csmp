using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;
using CSMP.Model;
using CSMP.DAL;

namespace CSMP.BLL
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
            // HttpRuntime.Cache.Insert(cacheKey, obj);

            //HttpRuntime.Cache.Insert(cacheKey, obj, null, DateTime.Now.AddMonths(2), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, insertIntoDB);
            HttpRuntime.Cache.Insert(cacheKey, obj, null, DateTime.Now.AddMonths(2), System.Web.Caching.Cache.NoSlidingExpiration);
        }

        //public static void insertIntoDB(string key, object value, CacheItemRemovedReason reason)
        //{
        //    //List<LeftMenuData> dataList = CacheManage.GetSearch("leftMenuKey") as List<LeftMenuData>;
        //    List<LeftMenuData> dataList = value as List<LeftMenuData>;
        //    foreach (LeftMenuData data in dataList) {

        //        LeftMenuDataBLL.Edit(data);
        //    }
        //}

        /// <summary>
        /// 删除缓存
        /// </summary>
        public static void DeleteCache(string cacheKey)
        {
            HttpRuntime.Cache.Remove(cacheKey);
        }
    }
}
