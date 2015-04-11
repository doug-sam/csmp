using System;
using System.Collections.Generic;
using CSMP.DAL;
using CSMP.Model;

namespace CSMP.BLL
{
    public static class Class1BLL
    {
        private static readonly DAL.Class1DAL dal = new DAL.Class1DAL();

        #region Get
        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<Class1Info> GetList(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            return dal.GetList(PageSize, CurPage, StrWhere, out Count);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<Class1Info> GetList(string StrWhere)
        {
            return dal.GetList(StrWhere);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<Class1Info> GetList(int CustomerID)
        {
            if (CustomerID < 1) return new List<Class1Info>();
            return dal.GetList(" 1=1 and f_CustomerID=" + CustomerID);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<Class1Info> GetList()
        {
            return GetList(" 1=1 ");
        }


        /// <summary>
        /// 获取Info
        /// </summary>
        /// <param name="id">id</param>
        public static Class1Info Get(int id)
        {
            return dal.Get(id);
        }


        /// <summary>
        /// 检查数据库中名称是否重复
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Class1Info Get(string value, int CustomerID)
        {
            return dal.Get(value, CustomerID);
        }
        #endregion

        #region Set
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="info">info</param>
        public static int Add(Class1Info info)
        {
            return dal.Add(info);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="info">info</param>
        public static bool Edit(Class1Info info)
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