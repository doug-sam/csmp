using System;
using System.Collections.Generic;
using CSMP.DAL;
using CSMP.Model;

namespace CSMP.BLL
{
    public static class CommentBLL
    {
        private static readonly DAL.CommentDAL dal = new DAL.CommentDAL();

        #region Get
        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<CommentInfo> GetList(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            return dal.GetList(PageSize, CurPage, StrWhere, out Count);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<CommentInfo> GetList(string StrWhere)
        {
            return dal.GetList(StrWhere);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<CommentInfo> GetList(string StrWhere, int Score)
        {
            if (Score > 0)
            {
                StrWhere += " and f_Score=" + Score;
            }
            return dal.GetList(StrWhere);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<CommentInfo> GetList()
        {
            return GetList(" 1=1 ");
        }


        /// <summary>
        /// 获取Info
        /// </summary>
        /// <param name="id">id</param>
        public static CommentInfo Get(int id)
        {
            return dal.Get(id);
        }

        public static int GetCountScore(string StrWhere, int ScoreValue, CommentInfo.ScoreType ScType)
        {
            return dal.GetCountScore(StrWhere, ScoreValue, ScType);
        }
        public static Int64 GetSum(string StrWhere, int ScoreValue, CommentInfo.ScoreType ScType)
        {
            return dal.GetSum(StrWhere, ScoreValue, ScType);
        }


        #endregion

        #region Set
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="info">info</param>
        public static int Add(CommentInfo info)
        {
            return dal.Add(info);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="info">info</param>
        public static bool Edit(CommentInfo info)
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