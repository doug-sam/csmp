using System;
using System.Collections.Generic;
using CSMP.DAL;
using CSMP.Model;

namespace CSMP.BLL
{
    public static class WorkGroupEmailBLL
    {
        private static readonly DAL.WorkGroupEmailDAL dal = new DAL.WorkGroupEmailDAL();

        #region Get
        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<WorkGroupEmailInfo> GetList(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            return dal.GetList(PageSize, CurPage, StrWhere, out Count);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<WorkGroupEmailInfo> GetList(int WorkGroupID)
        {
            string StrWhere = " f_GroupID=" + WorkGroupID + " and f_Enable=1 order by f_OrderNo desc,f_Name ";
            return dal.GetList(StrWhere);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<WorkGroupEmailInfo> GetListByEmailGroup(int EmailGroupID)
        {
            string StrWhere = " [f_EmailGroupID]=" + EmailGroupID + " and f_Enable=1 order by f_OrderNo desc,f_Name desc";
            return dal.GetList(StrWhere);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<WorkGroupEmailInfo> GetList(string StrWhere)
        {
            return dal.GetList(StrWhere);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<WorkGroupEmailInfo> GetList()
        {
            return GetList(" 1=1 ");
        }


        /// <summary>
        /// 获取Info
        /// </summary>
        /// <param name="id">id</param>
        public static WorkGroupEmailInfo Get(int id)
        {
            return dal.Get(id);
        }

        #endregion

        #region Set
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="info">info</param>
        public static int Add(WorkGroupEmailInfo info)
        {
            return dal.Add(info);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="info">info</param>
        public static bool Edit(WorkGroupEmailInfo info)
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