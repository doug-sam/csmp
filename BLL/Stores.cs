using System;
using System.Collections.Generic;
using CSMP.DAL;
using CSMP.Model;

namespace CSMP.BLL
{
    public static class StoresBLL
    {
        private static readonly DAL.StoresDAL dal = new DAL.StoresDAL();

        #region Get
        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<StoreInfo> GetList(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            return dal.GetList(PageSize, CurPage, StrWhere, out Count);
        }

        public static string GetName(int StoreID)
        {
            if (StoreID <= 0)
            {
                return string.Empty;
            }
            StoreInfo sinfo = StoresBLL.Get(StoreID);
            if (sinfo == null)
            {
                return string.Empty;
            }
            return sinfo.Name;
        }



        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<StoreInfo> GetList(string Key, int WorkGroup)
        {
            if (string.IsNullOrEmpty(Key))
            {
                return new List<StoreInfo>();
            }
            return dal.GetList(Key, WorkGroup);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<StoreInfo> GetListByCityWorkGroup(int CityID, int WorkGroup)
        {
            if (CityID<=0)
            {
                return new List<StoreInfo>();
            }
            return dal.GetList(CityID, WorkGroup);
        }


        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<StoreInfo> GetList(int CityID, int BrandID)
        {
            string strWhere = " 1=1 ";
            if (CityID>0)
            {
                strWhere += string.Format("and f_CityID={0} ", CityID);
            }
            if (BrandID>0)
            {
                strWhere += string.Format("and f_BrandID={0} ", BrandID);
            }

            return dal.GetList(strWhere);
        }


        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<StoreInfo> GetList(string StrWhere)
        {
            return dal.GetList(StrWhere);
        }


        /// <summary>
        /// 获取Info
        /// </summary>
        /// <param name="id">id</param>
        public static StoreInfo Get(int id)
        {
            return dal.Get(id);
        }


        /// <summary>
        /// 获取Info
        /// </summary>
        /// <param name="id">id</param>
        public static StoreInfo Get(string Tel)
        {
            Tel = Tel.Trim();
            if (string.IsNullOrEmpty(Tel)||Tel.Length<3)
            {
                return null;
            }
            return dal.Get(Tel);
        }
        /// <summary>
        /// 获取Info
        /// ZQL 2015.5.16
        /// </summary>
        /// <param name="id">id</param>
        public static StoreInfo GetByCallNO(string Tel)
        {
            Tel = Tel.Trim();
            if (string.IsNullOrEmpty(Tel) || Tel.Length < 3)
            {
                return null;
            }
            return dal.GetByCallNO(Tel);
        }

        /// <summary>
        /// 根据店铺号查找
        /// </summary>
        /// <param name="value"></param>
        /// <returns>无则返回null</returns>
        public static StoreInfo GetByStoreNo(string StoreNo)
        {
            return dal.GetByStoreNo(StoreNo);
        }

        /// <summary>
        /// 检查数据库中电话是否重复
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool TelExit(string value)
        { 
            return dal.TelExit(value);
        }
        #endregion

        #region Set
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="info">info</param>
        public static int Add(StoreInfo info)
        {
            return dal.Add(info);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="info">info</param>
        public static bool Edit(StoreInfo info)
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