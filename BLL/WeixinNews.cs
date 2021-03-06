﻿using System;
using System.Collections.Generic;
using CSMP.DAL;
using CSMP.Model;

namespace CSMP.BLL
{
    public static class WeixinNewsBLL
    {
        private static readonly DAL.WeixinNewsDAL dal = new DAL.WeixinNewsDAL();

        #region Get
        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<WeixinNewsInfo> GetList(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            return dal.GetList(PageSize, CurPage, StrWhere, out Count);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<WeixinNewsInfo> GetList(string StrWhere)
        {
            return dal.GetList(StrWhere);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<WeixinNewsInfo> GetList()
        {
            return GetList(" 1=1 ");
        }


        /// <summary>
        /// 获取Info
        /// </summary>
        /// <param name="id">id</param>
        public static WeixinNewsInfo Get(int id)
        {
            return dal.Get(id);
        }

        public static List<WeixinNewsInfo> Get(string KeyWord, bool IsConst)
        {
            KeyWord = Tool.Function.ClearText(KeyWord);
            return dal.Get(KeyWord, IsConst);
        }

        #endregion

        #region Set
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="info">info</param>
        public static int Add(WeixinNewsInfo info)
        {
            return dal.Add(info);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="info">info</param>
        public static bool Edit(WeixinNewsInfo info)
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