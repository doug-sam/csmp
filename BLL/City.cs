using System;
using System.Collections.Generic;
using CSMP.DAL;
using CSMP.Model;

namespace CSMP.BLL
{
    public static class CityBLL
    {
        private static readonly DAL.CityDAL dal = new DAL.CityDAL();

        #region Get
        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<CityInfo> GetList(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            return dal.GetList(PageSize, CurPage, StrWhere, out Count);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<CityInfo> GetList(int ProvinceID)
        {
            if (ProvinceID < 1) return new List<CityInfo>();
            return dal.GetList(" 1=1 and f_ProvinceID=" + ProvinceID);
        }


        /// <summary>
        /// 获取Info
        /// </summary>
        /// <param name="id">id</param>
        public static CityInfo Get(int id)
        {
            return dal.Get(id);
        }


        /// <summary>
        /// 获取Info
        /// </summary>
        /// <param name="id">id</param>
        public static CityInfo Get(string Name)
        {
            Name = Name.Trim();
            if (string.IsNullOrEmpty(Name) || Name.Length < 1)
            {
                return null;
            }
            return dal.Get(Name);
        }
        #endregion

        #region Set
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="info">info</param>
        public static int Add(CityInfo info)
        {
            return dal.Add(info);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="info">info</param>
        public static bool Edit(CityInfo info)
        {
            return dal.Edit(info);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">id</param>
        public static bool Delete(int id)
        {
           List<StoreInfo> list= StoresBLL.GetList(id, 0);
           if (list==null||list.Count==0)
           {
               return dal.Delete(id);
           }
           return false;
        }


        #endregion
    }
}