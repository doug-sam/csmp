﻿using System;
using System.Collections.Generic;
using CSMP.DAL;
using CSMP.Model;

namespace CSMP.BLL
{
    public static class FeedbackChooseBLL
    {
        private static readonly DAL.FeedbackChooseDAL dal = new DAL.FeedbackChooseDAL();

        #region Get
        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<FeedbackChooseInfo> GetList(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            return dal.GetList(PageSize, CurPage, StrWhere, out Count);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<FeedbackChooseInfo> GetList(string StrWhere)
        {
            return dal.GetList(StrWhere);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<FeedbackChooseInfo> GetList(int QuestionID)
        {
            if (QuestionID <= 0) return null;
            return dal.GetList(" 1=1  and f_QuestionID=" + QuestionID);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<FeedbackChooseInfo> GetList()
        {
            return GetList(" 1=1 ");
        }


        /// <summary>
        /// 获取Info
        /// </summary>
        /// <param name="id">id</param>
        public static FeedbackChooseInfo Get(int id)
        {
            return dal.Get(id);
        }

        #endregion

        #region Set
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="info">info</param>
        public static int Add(FeedbackChooseInfo info)
        {
            return dal.Add(info);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="info">info</param>
        public static bool Edit(FeedbackChooseInfo info)
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