using System;
using System.Collections.Generic;
using CSMP.DAL;
using CSMP.Model;

namespace CSMP.BLL
{
    public static class BKStoreInfoBLL
    {
        private static readonly DAL.BKStoreInfoDAL dal = new DAL.BKStoreInfoDAL();
        /// <summary>
        /// 根据店铺号查找
        /// </summary>
        /// <param name="value"></param>
        /// <returns>无则返回null</returns>
        public static BKStoreInfo GetByStoreNo(string LocalCode)
        {
            return dal.GetByStoreNo(LocalCode);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<BKStoreInfo> GetList(string StrWhere)
        {
            return dal.GetList(StrWhere);
        }

        

        #region Set
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="info">info</param>
        public static int Add(BKStoreInfo info)
        {
            return dal.Add(info);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="info">info</param>
        public static bool Edit(BKStoreInfo info)
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
