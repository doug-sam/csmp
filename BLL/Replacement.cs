using System;
using System.Collections.Generic;
using CSMP.DAL;
using CSMP.Model;

namespace CSMP.BLL
{
    public static class ReplacementBLL
    {
        private static readonly DAL.ReplacementDAL dal = new DAL.ReplacementDAL();

        #region Get
        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<ReplacementInfo> GetList(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            return dal.GetList(PageSize, CurPage, StrWhere, out Count);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<ReplacementInfo> GetList(string StrWhere)
        {
            return dal.GetList(StrWhere);
        }

        public static List<string> GetSerialNo(int CallID)
        {
            if (CallID<=0)
            {
                return new List<string>();
            }
            return dal.GetSerialNo(CallID);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<ReplacementInfo> GetList(int CallID)
        {
            if (CallID<=0)
            {
                return new List<ReplacementInfo>();
            }
            string StrWhere = string.Format(" 1=1 and f_CallID={0} order by f_DateAction ",CallID);
            return dal.GetList(StrWhere);
        }

        public static List<ReplacementInfo> GetList(int CallID, string SerialNo)
        {
            if (string.IsNullOrEmpty(SerialNo))
            {
                return new List<ReplacementInfo>();
            }
            return dal.GetList(CallID, SerialNo);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<ReplacementInfo> GetList()
        {
            return GetList(" 1=1 ");
        }


        /// <summary>
        /// 获取Info
        /// </summary>
        /// <param name="id">id</param>
        public static ReplacementInfo Get(int id)
        {
            return dal.Get(id);
        }

        #endregion

        #region Set
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="info">info</param>
        public static int Add(ReplacementInfo info)
        {
            return dal.Add(info);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="info">info</param>
        public static bool Edit(ReplacementInfo info)
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

        public static bool DeleteBySerialNo(string SerialNo)
        {
            if (string.IsNullOrEmpty(SerialNo))
            {
                return false;
            }
            return dal.DeleteBySerialNo(SerialNo);
        }

        #endregion
    }
}