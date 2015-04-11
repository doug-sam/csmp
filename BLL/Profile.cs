using System;
using System.Collections.Generic;
using CSMP.DAL;
using CSMP.Model;

namespace CSMP.BLL
{
    public static class ProfileBLL
    {
        private static readonly DAL.ProfileDAL dal = new DAL.ProfileDAL();

        #region Get
        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<ProfileInfo> GetList(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            return dal.GetList(PageSize, CurPage, StrWhere, out Count);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<ProfileInfo> GetList(string StrWhere)
        {
            return dal.GetList(StrWhere);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<ProfileInfo> GetList()
        {
            return GetList(" 1=1 ");
        }


        /// <summary>
        /// 获取Info
        /// </summary>
        /// <param name="id">id</param>
        public static ProfileInfo Get(int id)
        {
            return dal.Get(id);
        }

        #endregion

        #region Set
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="info">info</param>
        public static int Add(ProfileInfo info)
        {
            return dal.Add(info);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="info">info</param>
        public static bool Edit(ProfileInfo info)
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
        
        /// <summary>
        /// 获取ProfileInfo
        /// </summary>
        /// <param name="id">id</param>
        public static ProfileInfo Get(string Keys, bool WithCreate)
        {
            ProfileInfo info = dal.Get(Keys);
            if (null == info && WithCreate)
            {
                info = new ProfileInfo();
                info.GroupID = info.Value = string.Empty;
                info.Key = Keys;
                int ID = dal.Add(info);
                return dal.Get(info.ID);
            }
            return info;
        }

        /// <summary>
        /// 获取ProfileInfo的Content
        /// </summary>
        /// <param name="id">id</param>
        public static string GetValue(string keys, bool WithCreate)
        {
            ProfileInfo info = Get(keys, WithCreate);
            return info == null ? string.Empty : info.Value;
        }

        /// <summary>
        /// 获取ProfileInfo
        /// </summary>
        /// <param name="id">id</param>
        public static string GetValue(string keys)
        {
            return GetValue(keys, false);
        }
        public static ProfileInfo Get(string Keys)
        {
            return Get(Keys, false);
        }
    }
}