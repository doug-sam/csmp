using System;
using System.Collections.Generic;
using CSMP.DAL;
using CSMP.Model;

namespace CSMP.BLL
{
    public static class CallCategoryBLL
    {
        private static readonly DAL.CallCategoryDAL dal = new DAL.CallCategoryDAL();

        #region Get
        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<CallCategoryInfo> GetList(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            return dal.GetList(PageSize, CurPage, StrWhere, out Count);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<CallCategoryInfo> GetList(string StrWhere)
        {
            return dal.GetList(StrWhere);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<CallCategoryInfo> GetList()
        {
            return GetList(" 1=1 ");
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<CallCategoryInfo> GetListEnable()
        {
            return GetList(" 1=1  and f_Enable=1 order by f_OrderID desc,ID asc");
        }


        /// <summary>
        /// 获取Info
        /// </summary>
        /// <param name="id">id</param>
        public static CallCategoryInfo Get(int id)
        {
            CallCategoryInfo info= dal.Get(id);
            if (null==info)
            {
                info = new CallCategoryInfo();
            }
            return info;
        }

        #endregion

        #region Set
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="info">info</param>
        public static int Add(CallCategoryInfo info)
        {
            return dal.Add(info);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="info">info</param>
        public static bool Edit(CallCategoryInfo info)
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