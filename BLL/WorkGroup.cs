using System;
using System.Collections.Generic;
using CSMP.DAL;
using CSMP.Model;
using Tool;

namespace CSMP.BLL
{
    public static class WorkGroupBLL
    {
        private static readonly DAL.WorkGroupDAL dal = new DAL.WorkGroupDAL();
        
        #region Get
        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<WorkGroupInfo> GetList(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            return dal.GetList(PageSize, CurPage, StrWhere, out Count);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<WorkGroupInfo> GetList(int Province)
        {
            return dal.GetList(Province);
        }


        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<WorkGroupInfo> GetList()
        {
            return dal.GetList(0);
        }


        /// <summary>
        /// 获取Info
        /// </summary>
        /// <param name="id">id</param>
        public static WorkGroupInfo Get(int id)
        {
            return dal.Get(id);
        }
        /// <summary>
        /// 根据关系找出组
        /// </summary>
        /// <param name="MID">关系id</param>
        /// <param name="TrueIn_FalseOut">true为所有有关系，false为所有无关系的记录</param>
        /// <returns></returns>
        public static List<WorkGroupInfo> GetList(int MID, bool TrueIn_FalseOut)
        {
            return dal.GetList(MID, TrueIn_FalseOut);
        }


        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<WorkGroupInfo> GetList(int Province, SysEnum.WorkGroupType WorkGroupType)
        {
            return dal.GetList(Province, WorkGroupType);
        }

        public static string GetWorkGroupName(int WorkGroupID)
        {
            WorkGroupInfo winfo = WorkGroupBLL.Get(WorkGroupID);
            if (null == winfo)
            {
                return string.Empty;
            }
            return winfo.Name;
        }


        #endregion

        #region Set
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="info">info</param>
        public static int Add(WorkGroupInfo info)
        {
            return dal.Add(info);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="info">info</param>
        public static bool Edit(WorkGroupInfo info)
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