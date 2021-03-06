﻿using System;
using System.Collections.Generic;
using CSMP.DAL;
using CSMP.Model;

namespace CSMP.BLL
{
    public static class BKStoreInfoBLL
    {
        private static readonly DAL.BKStoreInfoDAL dal = new DAL.BKStoreInfoDAL();
        /// <summary>
        /// 根据店铺号查找
        /// </summary>
        /// <param name="value"></param>
        /// <returns>无则返回null</returns>
        public static BKStoreInfo GetByStoreNo(string LocalCode)
        {
            return dal.GetByStoreNo(LocalCode);
        }
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="CurPage"></param>
        /// <param name="StrWhere"></param>
        /// <param name="Count"></param>
        /// <returns></returns>
        public static List<BKStoreInfo> GetList(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            return dal.GetList(PageSize, CurPage, StrWhere, out Count);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<BKStoreInfo> GetList(string StrWhere)
        {
            return dal.GetList(StrWhere);
        }
        /// <summary>
        /// 查询总记录数
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static int CountAll()
        {
            return dal.CountAll();
        }

        #region Set
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="info">info</param>
        public static int Add(BKStoreInfo info)
        {
            return dal.Add(info);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="info">info</param>
        public static bool Edit(BKStoreInfo info)
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
