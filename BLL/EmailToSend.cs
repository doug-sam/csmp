using System;
using System.Collections.Generic;
using CSMP.DAL;
using CSMP.Model;
using Tool;
using System.Data;

namespace CSMP.BLL
{
    public static class EmailToSendBLL
    {
        private static readonly DAL.EmailToSendDAL dal = new DAL.EmailToSendDAL();

        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<EmailToSend> GetList(string StrWhere)
        {
            return dal.GetList(StrWhere);
        }
        /// <summary>
        /// 获取Info
        /// </summary>
        /// <param name="id">id</param>
        public static EmailToSend Get(int id)
        {
            return dal.Get(id);
        }
        #region Set
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="info">info</param>
        public static int Add(EmailToSend info)
        {
            return dal.Add(info);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="info">info</param>
        public static bool Edit(EmailToSend info)
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
