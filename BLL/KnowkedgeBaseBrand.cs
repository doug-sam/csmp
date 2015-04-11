using System;
using System.Collections.Generic;
using CSMP.DAL;
using CSMP.Model;

namespace CSMP.BLL
{
    public static class KnowkedgeBaseBrandBLL
    {
        private static readonly DAL.KnowkedgeBaseBrandDAL dal = new DAL.KnowkedgeBaseBrandDAL();

        #region Get
        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<KnowkedgeBaseBrandInfo> GetList(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            return dal.GetList(PageSize, CurPage, StrWhere, out Count);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<KnowkedgeBaseBrandInfo> GetList(string StrWhere)
        {
            return dal.GetList(StrWhere);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<KnowkedgeBaseBrandInfo> GetList()
        {
            return GetList(" 1=1 ");
        }


        /// <summary>
        /// 获取Info
        /// </summary>
        /// <param name="id">id</param>
        public static KnowkedgeBaseBrandInfo Get(int id)
        {
            return dal.Get(id);
        }

        #endregion

        #region Set
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="info">info</param>
        public static int Add(KnowkedgeBaseBrandInfo info)
        {
            return dal.Add(info);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="info">info</param>
        public static bool Edit(KnowkedgeBaseBrandInfo info)
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
        /// 根据知识库id删除
        /// </summary>
        /// <param name="KnowledgeID"></param>
        /// <returns></returns>
        public static bool DeleteByKnowledgeID(int KnowledgeID)
        { 
            return dal. DeleteByKnowledgeID(KnowledgeID);
        }

        #endregion
    }
}