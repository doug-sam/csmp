using System;
using System.Collections.Generic;
using CSMP.DAL;
using CSMP.Model;

namespace CSMP.BLL
{
    public static class JobcodeBLL
    {
        private static readonly DAL.JobcodeDAL dal = new DAL.JobcodeDAL();

        #region Get
        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<JobcodeInfo> GetList(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            return dal.GetList(PageSize, CurPage, StrWhere, out Count);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<JobcodeInfo> GetList(string StrWhere)
        {
            return dal.GetList(StrWhere);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<JobcodeInfo> GetList(string wd,int WorkGroupID)
        {
            wd = Tool.Function.ClearText(wd);
           string StrWhere =string.Format(" 1=1  and f_WorkGroupID={0} and  f_CodeNo like '%{1}%' ",WorkGroupID,wd);
            int count=0;
            return dal.GetList(20,1,StrWhere,out count);

        }


        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<JobcodeInfo> GetList()
        {
            return GetList(" 1=1 ");
        }


        /// <summary>
        /// 获取Info
        /// </summary>
        /// <param name="id">id</param>
        public static JobcodeInfo Get(int id)
        {
            return dal.Get(id);
        }
        public static JobcodeInfo Get(string CodeNo, int WorkGroupID)
        { 
            return dal.Get(CodeNo,WorkGroupID);
        }
        #endregion

        #region Set
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="info">info</param>
        public static int Add(JobcodeInfo info)
        {
            return dal.Add(info);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="info">info</param>
        public static bool Edit(JobcodeInfo info)
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