using System;
using System.Collections.Generic;
using CSMP.DAL;
using CSMP.Model;

namespace CSMP.BLL
{
    public static class FeedbackQuestionBLL
    {
        private static readonly DAL.FeedbackQuestionDAL dal = new DAL.FeedbackQuestionDAL();

        #region Get
        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<FeedbackQuestionInfo> GetList(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            return dal.GetList(PageSize, CurPage, StrWhere, out Count);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<FeedbackQuestionInfo> GetList(string StrWhere)
        {
            return dal.GetList(StrWhere);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<FeedbackQuestionInfo> GetList(int PaperID, CSMP.Model.SysEnum.QuestionType QuestionType)
        {
            return dal.GetList(string.Format(" f_PaperID={0} and f_Type={1} ORDER BY f_OrderNumber ", PaperID, (int)QuestionType));
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<FeedbackQuestionInfo> GetList(int PaperID)
        {
            return dal.GetList(string.Format(" f_PaperID={0} ORDER BY f_OrderNumber ", PaperID));
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<FeedbackQuestionInfo> GetList()
        {
            return GetList(" 1=1 ");
        }


        /// <summary>
        /// 获取Info
        /// </summary>
        /// <param name="id">id</param>
        public static FeedbackQuestionInfo Get(int id)
        {
            return dal.Get(id);
        }

        #endregion

        #region Set
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="info">info</param>
        public static int Add(FeedbackQuestionInfo info)
        {
            return dal.Add(info);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="info">info</param>
        public static bool Edit(FeedbackQuestionInfo info)
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