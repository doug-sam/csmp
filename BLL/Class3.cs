using System;
using System.Collections.Generic;
using CSMP.DAL;
using CSMP.Model;

namespace CSMP.BLL
{
    public static class Class3BLL
    {
        private static readonly DAL.Class3DAL dal = new DAL.Class3DAL();

        #region Get
        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<Class3Info> GetList(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            return dal.GetList(PageSize, CurPage, StrWhere, out Count);
        }


        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<Class3Info> GetList(int Class2ID)
        {
            if (Class2ID < 1) return new List<Class3Info>();
            return dal.GetList(" 1=1 and f_Class2ID="+Class2ID);
        }



        /// <summary>
        /// 获取Info
        /// </summary>
        /// <param name="id">id</param>
        public static Class3Info Get(int id)
        {
            return dal.Get(id);
        }




        /// <summary>
        /// 获取Info
        /// </summary
        /// </summary>
        /// <param name="value"></param>
        /// <param name="Class2ID"></param>
        /// <returns></returns>
        public static Class3Info Get(string value, int Class2ID)
        { 
            return dal.Get(value, Class2ID);
        }

        #endregion

        #region Set
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="info">info</param>
        public static int Add(Class3Info info)
        {
            return dal.Add(info);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="info">info</param>
        public static bool Edit(Class3Info info)
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