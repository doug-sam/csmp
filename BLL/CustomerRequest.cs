using System;
using System.Collections.Generic;
using CSMP.DAL;
using CSMP.Model;

namespace CSMP.BLL
{
    public static class CustomerRequestBLL
    {
        private static readonly DAL.CustomerRequestDAL dal = new DAL.CustomerRequestDAL();

        #region Get
        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<CustomerRequestInfo> GetList(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            return dal.GetList(PageSize, CurPage, StrWhere, out Count);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<CustomerRequestInfo> GetListEnableOrderByIDDesc(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            return dal.GetList(PageSize, CurPage, StrWhere + " and f_Enable=1 order by id desc ", out Count);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<CustomerRequestInfo> GetList(string StrWhere)
        {
            return dal.GetList(StrWhere);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<CustomerRequestInfo> GetList()
        {
            return GetList(" 1=1 ");
        }


        /// <summary>
        /// 获取Info
        /// </summary>
        /// <param name="id">id</param>
        public static CustomerRequestInfo Get(int id)
        {
            return dal.Get(id);
        }

        public static int GetConut(int WorkGroupID)
        {
            return dal.GetConut(WorkGroupID);
        }

        #endregion

        #region Set
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="info">info</param>
        public static int Add(CustomerRequestInfo info)
        {
            return dal.Add(info);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="info">info</param>
        public static bool Edit(CustomerRequestInfo info)
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