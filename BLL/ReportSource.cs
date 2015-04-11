using System;
using System.Collections.Generic;
using CSMP.DAL;
using CSMP.Model;
using System.Data;

namespace CSMP.BLL
{
    public static class ReportSourceBLL
    {
        private static readonly DAL.ReportSourceDAL dal = new DAL.ReportSourceDAL();

        #region Get
        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<ReportSourceInfo> GetList(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            return dal.GetList(PageSize, CurPage, StrWhere, out Count);
        }


        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<ReportSourceInfo> GetList()
        {
            return dal.GetList(" 1=1 ");
        }

        public static DataTable GetReport(string strSQL)
        {
            ReportSourceDAL reportDAL = new ReportSourceDAL();
            return reportDAL.GetReport(strSQL);
        }

        /// <summary>
        /// 获取Info
        /// </summary>
        /// <param name="id">id</param>
        public static ReportSourceInfo Get(int id)
        {
            return dal.Get(id);
        }

        #endregion

        #region Set
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="info">info</param>
        public static int Add(ReportSourceInfo info)
        {
            return dal.Add(info);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="info">info</param>
        public static bool Edit(ReportSourceInfo info)
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