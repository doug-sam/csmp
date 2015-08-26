using System;
using System.Collections.Generic;
using CSMP.DAL;
using CSMP.Model;
using Tool;
using System.Data;

namespace CSMP.BLL
{
    public static class WebServiceTaskBLL
    {
        private static readonly DAL.WebServiceTaskDAL dal = new DAL.WebServiceTaskDAL();

        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<WebServiceTaskInfo> GetList(string StrWhere)
        {
            return dal.GetList(StrWhere);
        }
        /// <summary>
        /// 获取Info
        /// </summary>
        /// <param name="id">id</param>
        public static WebServiceTaskInfo Get(int id)
        {
            return dal.Get(id);
        }
        #region Set
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="info">info</param>
        public static int Add(WebServiceTaskInfo info)
        {
            return dal.Add(info);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="info">info</param>
        public static bool Edit(WebServiceTaskInfo info)
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
