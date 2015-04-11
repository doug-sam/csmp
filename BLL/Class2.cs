using System;
using System.Collections.Generic;
using CSMP.DAL;
using CSMP.Model;

namespace CSMP.BLL
{
    public static class Class2BLL
    {
        private static readonly DAL.Class2DAL dal = new DAL.Class2DAL();

        #region Get
        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<Class2Info> GetList(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            return dal.GetList(PageSize, CurPage, StrWhere, out Count);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<Class2Info> GetList(int Class1ID)
        {
            if (Class1ID < 1) return new List<Class2Info>();
            return dal.GetList(" 1=1 and f_Class1ID="+Class1ID);
        }



        /// <summary>
        /// 获取Info
        /// </summary>
        /// <param name="id">id</param>
        public static Class2Info Get(int id)
        {
            return dal.Get(id);
        }



        public static Class2Info Get(string value, int Class1ID)
        {
            value = value.Trim();
            return dal.Get(value, Class1ID);
        }
        #endregion

        #region Set
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="info">info</param>
        public static int Add(Class2Info info)
        {
            return dal.Add(info);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="info">info</param>
        public static bool Edit(Class2Info info)
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