using System;
using System.Collections.Generic;
using CSMP.DAL;
using CSMP.Model;

namespace CSMP.BLL
{
    public static class WorkGroupBrandBLL
    {
        private static readonly DAL.WorkGroupBrandDAL dal = new DAL.WorkGroupBrandDAL();

        #region Get
        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<WorkGroupBrandInfo> GetList(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            return dal.GetList(PageSize, CurPage, StrWhere, out Count);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<WorkGroupBrandInfo> GetList(string StrWhere)
        {
            return dal.GetList(StrWhere);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<WorkGroupBrandInfo> GetList()
        {
            return GetList(" 1=1 ");
        }


        /// <summary>
        /// 获取Info
        /// </summary>
        /// <param name="id">id</param>
        public static WorkGroupBrandInfo Get(int id)
        {
            return dal.Get(id);
        }

        /// <summary>
        /// 工作组跟客户是否有关系
        /// </summary>
        /// <param name="WorkGroupID"></param>
        /// <param name="BrandID"></param>
        /// <returns></returns>
        public static bool HasRelaction(int WorkGroupID, int BrandID)
        {
            return dal.HasRelaction(WorkGroupID, BrandID);
        }

        

        #endregion

        #region Set
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="info">info</param>
        public static int Add(WorkGroupBrandInfo info)
        {
            return dal.Add(info);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="info">info</param>
        public static bool Edit(WorkGroupBrandInfo info)
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

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="WorkGroupID">工作组</param>
        /// <param name="CustomerID">客户ID</param>
        /// <returns></returns>
        public static bool Delete(int WorkGroupID, int CustomerID)
        {
            return dal.Delete(WorkGroupID, CustomerID);
        }
        
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">Member id</param>
        public static bool DeleteByMID(int MID)
        {
            return dal.DeleteByMID(MID);
        }

        
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">Member id</param>
        public static bool DeleteByWorkGroupID(int WorkGroupID)
        { 
            return dal.DeleteByWorkGroupID(WorkGroupID);
        }
        #endregion
    }
}