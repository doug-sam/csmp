using System;
using System.Collections.Generic;
using CSMP.DAL;
using CSMP.Model;

namespace CSMP.BLL
{
    public static class EmailRecordBLL
    {
        private static readonly DAL.EmailRecordDAL dal = new DAL.EmailRecordDAL();

        #region Get
        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<EmailRecordInfo> GetList(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            return dal.GetList(PageSize, CurPage, StrWhere, out Count);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<EmailRecordInfo> GetList(string StrWhere)
        {
            return dal.GetList(StrWhere);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<EmailRecordInfo> GetList()
        {
            return GetList(" 1=1 ");
        }


        /// <summary>
        /// 获取Info
        /// </summary>
        /// <param name="id">id</param>
        public static EmailRecordInfo Get(int id)
        {
            return dal.Get(id);
        }

        #endregion

        #region Set
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="info">info</param>
        public static int Add(EmailRecordInfo info)
        {
            return dal.Add(info);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="info">info</param>
        public static bool Edit(EmailRecordInfo info)
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
        public static bool Delete(int UserID, int CallID, DateTime DateStart, DateTime DateEnd)
        {
            if (DateStart<=DicInfo.DateZone)
            {
                return false;
            }
            if (DateEnd<=DicInfo.DateZone)
            {
                return false;
            }
            if (DateEnd<DateStart)
            {
                return false;
            }
            return dal.Delete(UserID, CallID, DateStart, DateEnd);
        }


        #endregion
    }
}