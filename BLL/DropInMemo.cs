using System;
using System.Collections.Generic;
using CSMP.DAL;
using CSMP.Model;

namespace CSMP.BLL
{
    public static class DropInMemoBLL
    {
        private static readonly DAL.DropInMemoDAL dal = new DAL.DropInMemoDAL();

        #region Get
        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<DropInMemoInfo> GetList(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            return dal.GetList(PageSize, CurPage, StrWhere, out Count);
        }

                /// <summary>
        /// 根据callID 获取所有上门备注，并按ID排序
        /// </summary>
        /// <param name="CallID">CallID</param>
        /// <returns></returns>
        public static List<DropInMemoInfo> GetListOrderByID(int CallID)
        { 
            return dal.GetListOrderByID(CallID);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<DropInMemoInfo> GetList(int CallStepID)
        {
            return dal.GetList(CallStepID);
        }


        /// <summary>
        /// 获取Info
        /// </summary>
        /// <param name="id">id</param>
        public static DropInMemoInfo Get(int id)
        {
            return dal.Get(id);
        }

        #endregion

        #region Set
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="info">info</param>
        public static int Add(DropInMemoInfo info)
        {
            return dal.Add(info);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="info">info</param>
        public static bool Edit(DropInMemoInfo info)
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