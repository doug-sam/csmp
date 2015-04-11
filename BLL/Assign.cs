using System;
using System.Collections.Generic;
using CSMP.DAL;
using CSMP.Model;

namespace CSMP.BLL
{
    public static class AssignBLL
    {
        private static readonly DAL.AssignDAL dal = new DAL.AssignDAL();

        #region Get
        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<AssignInfo> GetList(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            return dal.GetList(PageSize, CurPage, StrWhere, out Count);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<AssignInfo> GetList(string StrWhere)
        {
            return dal.GetList(StrWhere);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<AssignInfo> GetList()
        {
            return GetList(" 1=1 ");
        }


        /// <summary>
        /// 获取Info
        /// </summary>
        /// <param name="id">id</param>
        public static AssignInfo Get(int id)
        {
            return dal.Get(id);
        }

        public static AssignInfo GetMax(int CallID)
        {
            return dal.GetMax(CallID);
        }

        public static List<AssignInfo> GetList(int CallID)
        {
            return dal.GetList(CallID);
        }

        #endregion

        #region Set
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="info">info</param>
        public static int Add(AssignInfo info)
        {
            return dal.Add(info);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="info">info</param>
        public static bool Edit(AssignInfo info)
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

        /// <summary>
        /// 该call转派过的公司
        /// </summary>
        /// <param name="CallID"></param>
        /// <returns></returns>
        public static int GetCrossWorkGroupID(int CallID)
        {
            return dal.GetCrossWorkGroupID(CallID);
        }
    }
}