using System;
using System.Collections.Generic;
using System.Text;
using CSMP.DAL;
using CSMP.Model;

namespace CSMP.BLL
{
    public static class BrandBLL
    {
        private static readonly DAL.BrandDAL dal = new DAL.BrandDAL();

        #region Get
        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<BrandInfo> GetList(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            return dal.GetList(PageSize, CurPage, StrWhere, out Count);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<BrandInfo> GetList(string StrWhere)
        {
            return dal.GetList(StrWhere);
        }


        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<BrandInfo> GetList(int CustomerID)
        {
            if (CustomerID <= 0)
            {
                return new List<BrandInfo>();
            }
            return dal.GetList(" 1=1 and f_CustomerID=" + CustomerID);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<BrandInfo> GetListByKnowledgeID(int KnowledgeID)
        {
            if (KnowledgeID <= 0)
            {
                return new List<BrandInfo>();
            }
            return dal.GetListByKnowledgeID(KnowledgeID);
        }



        /// <summary>
        /// 获取列表，指定客户下，根据工作组找出其品牌
        /// </summary>
        /// <param name="CustomerID">指定客户</param>
        /// <param name="WorkGroupID">指定工作组</param>
        /// <returns></returns>
        public static List<BrandInfo> GetList(int CustomerID,int WorkGroupID)
        {
            if (CustomerID <= 0)
            {
                return new List<BrandInfo>();
            }
            string strWhere = " 1=1 and f_CustomerID=" + CustomerID;
            strWhere +=string.Format(" and ID in(SELECT f_MID FROM sys_WorkGroupBrand WHERE f_WorkGroupID={0})  ",WorkGroupID);
            return dal.GetList(strWhere);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<BrandInfo> GetList()
        {
            return GetList(" 1=1 ");
        }


        /// <summary>
        /// 获取Info
        /// </summary>
        /// <param name="id">id</param>
        public static BrandInfo Get(int id)
        {
            return dal.Get(id);
        }
        /// <summary>
        /// 获取Info
        /// </summary>
        /// <param name="Name">Name</param>
        public static BrandInfo Get(string Name)
        {
            return dal.Get(Name);
        }

        /// <summary>
        /// 检查数据库中名称是否重复
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static BrandInfo Get(string value, int CustomerID)
        { 
            return dal.Get(value, CustomerID);
        }
        /// <summary>
        /// 检查数据库中名称是否重复
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool NameExit(string value)
        {
            BrandInfo info = dal.Get(value, 0);
            return info != null && info.ID > 0;
        }

        /// <summary>
        /// 根据工作组找出所有品牌
        /// </summary>
        /// <param name="WorkGroupID"></param>
        /// <returns></returns>
        public static List<BrandInfo> GetListByWorkGroup(int WorkGroupID)
        { 
            return dal.GetList(WorkGroupID);
        }


        #endregion

        #region Set
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="info">info</param>
        public static int Add(BrandInfo info)
        {
            return dal.Add(info);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="info">info</param>
        public static bool Edit(BrandInfo info)
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