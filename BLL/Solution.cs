using System;
using System.Collections.Generic;
using CSMP.DAL;
using CSMP.Model;

namespace CSMP.BLL
{
    public static class SolutionBLL
    {
        private static readonly DAL.SolutionDAL dal = new DAL.SolutionDAL();

        #region Get
        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<SolutionInfo> GetList(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            return dal.GetList(PageSize, CurPage, StrWhere, out Count);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<SolutionInfo> GetList(string StrWhere)
        {
            return dal.GetList(StrWhere);
        }

        /// <summary>
        /// 根据中类找到指定行数的记录，并从高到低排序
        /// </summary>
        /// <param name="Class2ID">中类故障ID</param>
        /// <param name="TopCount">需要获取行数，0则不限</param>
        /// <returns></returns>
        public static List<SolutionInfo> GetList(int Class3ID, int TopCount)
        {
            return dal.GetList(Class3ID, TopCount);
        }
        /// <summary>
        /// 根据中类找到10行记录，并从高到低排序
        /// </summary>
        /// <param name="Class2ID">中类故障ID</param>
        /// <param name="TopCount">需要获取行数，0则不限</param>
        /// <returns></returns>
        public static List<SolutionInfo> GetList(int Class3ID)
        {
            List<SolutionInfo> list = GetList(Class3ID, 10);
            int Count = 0;
            foreach (SolutionInfo item in list)
            {
                Count += item.SolveCount ;
            }
            decimal precent = 0;
            foreach (SolutionInfo item in list)
            {
                if (Count==0)
                {
                    precent =0;
                }
                else
                {
                    precent = Math.Round((((decimal)item.SolveCount) / (decimal)Count) * 100, 2);
                }
                item.Name += string.Format("[解决({0}次)解决率({1}%)]", item.SolveCount, precent);
            }
            return list;
        }

        /// <summary>
        /// 获取Info
        /// </summary>
        /// <param name="id">id</param>
        public static SolutionInfo Get(int id)
        {
            return dal.Get(id);
        }

        /// <summary>
        /// 获取Info
        /// </summary>
        /// <param name="id">id</param>
        public static SolutionInfo Get(string Name, int Class3ID)
        {
            return dal.Get(Name, Class3ID);
        }

        #endregion

        #region Set
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="info">info</param>
        public static int Add(SolutionInfo info)
        {
            return dal.Add(info);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="info">info</param>
        public static bool Edit(SolutionInfo info)
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