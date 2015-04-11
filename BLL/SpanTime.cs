using System;
using System.Collections.Generic;
using CSMP.DAL;
using CSMP.Model;

namespace CSMP.BLL
{
    public static class SpanTimeBLL
    {
        private static readonly DAL.SpanTimeDAL dal = new DAL.SpanTimeDAL();

        #region Get
        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<SpanTimeInfo> GetList(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            return dal.GetList(PageSize, CurPage, StrWhere, out Count);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<SpanTimeInfo> GetList(string StrWhere)
        {
            return dal.GetList(StrWhere);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<SpanTimeInfo> GetList()
        {
            return GetList(" 1=1 ");
        }

        public static List<SpanTimeInfo> GetList(int CallID)
        { 
            return dal.GetList(CallID);
        }

        /// <summary>
        /// 获取Info
        /// </summary>
        /// <param name="id">id</param>
        public static SpanTimeInfo Get(int id)
        {
            return dal.Get(id);
        }


        /// <summary>
        /// 判断当前call是不是在暂停状态
        /// </summary>
        /// <param name="CallID"></param>
        /// <returns></returns>
        public static bool IsInStop(int CallID)
        {
            return GetStopInfo(CallID) != null;

        }
        /// <summary>
        /// 获取当前暂停的数据，无则返回null
        /// </summary>
        /// <param name="CallID"></param>
        /// <returns></returns>
        public static SpanTimeInfo GetStopInfo(int CallID)
        {
            List<SpanTimeInfo> list = GetList(CallID);
            if (list != null && list.Count > 0)
            {
                foreach (SpanTimeInfo item in list)
                {
                    if (DateTime.Now > item.DateBegin && DateTime.Now < item.DateEnd)
                    {
                        return item;
                    }
                }
            }
            return null;
        }

        #endregion

        #region Set
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="info">info</param>
        public static int Add(SpanTimeInfo info)
        {
            return dal.Add(info);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="info">info</param>
        public static bool Edit(SpanTimeInfo info)
        {
            return dal.Edit(info);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">id</param>
        public static bool Delete(int id)
        {
            return dal.Delete(id);
        }


        #endregion
    }
}