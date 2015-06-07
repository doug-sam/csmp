using System;
using System.Collections.Generic;
using CSMP.DAL;
using CSMP.Model;

namespace CSMP.BLL
{
    public static class TrunkNO
    {
        private static readonly DAL.TrunkNO dal = new DAL.TrunkNO();

        #region Get
        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<TrunkNOInfo> GetList(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            return dal.GetList(PageSize, CurPage, StrWhere, out Count);
        }

       


        /// <summary>
        /// 获取Info
        /// </summary>
        /// <param name="id">id</param>
        public static TrunkNOInfo Get(int id)
        {
            return dal.Get(id);
        }


        /// <summary>
        /// 获取Info
        /// </summary>
        /// <param name="id">id</param>
        public static TrunkNOInfo Get(string PhysicalNo)
        {
            PhysicalNo = PhysicalNo.Trim();
            if (string.IsNullOrEmpty(PhysicalNo) || PhysicalNo.Length < 1)
            {
                return null;
            }
            return dal.Get(PhysicalNo);
        }
        #endregion

        #region Set
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="info">info</param>
        public static int Add(TrunkNOInfo info)
        {
            return dal.Add(info);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="info">info</param>
        public static bool Edit(TrunkNOInfo info)
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
