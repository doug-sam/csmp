using System;
using System.Collections.Generic;
using CSMP.DAL;
using CSMP.Model;
namespace CSMP.BLL
{
    public static class APPLoginLogoutLogBLL
    {
        private static readonly DAL.APPLoginLogoutLogDAL dal = new DAL.APPLoginLogoutLogDAL();

        #region Get

        public static List<APPLoginLogoutLog> GetList(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            return dal.GetList( PageSize,  CurPage,  StrWhere, out  Count);
        }

        public static List<APPLoginLogoutLog> GetList(string StrWhere)
        {
            return dal.GetList(StrWhere);
        }
        /// <summary>
        /// 获取Info
        /// </summary>
        /// <param name="id">id</param>
        public static APPLoginLogoutLog Get(int id)
        {
            return dal.Get(id);
        }

        /// <summary>
        /// 获取Info
        /// </summary>
        /// <param name="id">id</param>
        public static APPLoginLogoutLog Get(string OpenID)
        {
            return dal.Get(OpenID);
        }

        #endregion


        #region set
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="info">info</param>
        public static int Add(APPLoginLogoutLog info)
        {
            return dal.Add(info);
        }

        /// <summary>
        /// 是用SP打卡，牵出、签退
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="loginMode">打卡类型 1为签到，0为签退</param>
        /// <param name="loginTime">打卡时间</param>
        /// <param name="location">打卡地点</param>
        /// <returns></returns>
        public static string AddBySP(string userID,string loginMode,string loginTime,string location)
        {
            return dal.AddBySP( userID, loginMode, loginTime, location);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="info">info</param>
        public static bool Edit(APPLoginLogoutLog info)
        {
            return dal.Edit(info);
        }

        /// <summary>
        /// 删除Member
        /// </summary>
        /// <param name="id">Member id</param>
        public static bool Delete(int id)
        {
            return dal.Delete(id);
        }


        #endregion
    }
}
