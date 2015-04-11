using System;
using System.Collections.Generic;
using CSMP.DAL;
using CSMP.Model;

namespace CSMP.BLL
{
    public static class SlaWarnRecordBLL
    {
        private static readonly DAL.SlaWarnRecordDAL dal = new DAL.SlaWarnRecordDAL();

        #region Get
        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<SlaWarnRecordInfo> GetList(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            return dal.GetList(PageSize, CurPage, StrWhere, out Count);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<SlaWarnRecordInfo> GetList(string StrWhere)
        {
            return dal.GetList(StrWhere);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<SlaWarnRecordInfo> GetList()
        {
            return GetList(" 1=1 ");
        }

        public static SlaWarnRecordInfo GetByCallID(int CallID)
        { 
            return dal.GetByCallID(CallID);
        }



        /// <summary>
        /// 获取Info
        /// </summary>
        /// <param name="id">id</param>
        public static SlaWarnRecordInfo Get(int id)
        {
            return dal.Get(id);
        }

        #endregion

        #region Set
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="info">info</param>
        public static int Add(SlaWarnRecordInfo info)
        {
            return dal.Add(info);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="info">info</param>
        public static bool Edit(SlaWarnRecordInfo info)
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