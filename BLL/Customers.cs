using System;
using System.Collections.Generic;
using CSMP.DAL;
using CSMP.Model;
using Tool;
using System.Data;

namespace CSMP.BLL
{
    public static class CustomersBLL
    {
        private static readonly DAL.CustomersDAL dal = new DAL.CustomersDAL();

        #region Get
        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<CustomersInfo> GetList(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            return dal.GetList(PageSize, CurPage, StrWhere, out Count);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<CustomersInfo> GetList(string StrWhere)
        {
            return dal.GetList(StrWhere);
        }



        /// <summary>
        /// 根据登录帐户与客户关系找出客户
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public static List<CustomersInfo> GetList(UserInfo uinfo)
        {
            if (null == uinfo)
            {
                return new List<CustomersInfo>();
            }
            if (uinfo.Rule.Contains(SysEnum.Rule.管理员.ToString()))
            {
                return GetList();
            }
            return dal.GetList(uinfo.WorkGroupID);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<CustomersInfo> GetList()
        {
            return GetList(" 1=1 order by f_Name ");
        }


        /// <summary>
        /// 获取Info
        /// </summary>
        /// <param name="id">id</param>
        public static CustomersInfo Get(int id)
        {
            return dal.Get(id);
        }

        /// <summary>
        /// 检查数据库中名称是否重复
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool NameExit(string value)
        {
            return Get(value) != null;
        }

        /// <summary>
        /// 检查数据库中名称是否重复
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static CustomersInfo Get(string value)
        {
            value = value.Trim();
            return dal.Get(value);
        }
        /// <summary>
        /// 根据用户ID 找到对应的客户列表
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static DataTable GetCustomerByUserID(string userID)
        {
            return dal.GetCustomerByUserID(userID);
        
        }
        /// <summary>
        /// 根据openid 找到对应的客户列表，用于微信
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        public static DataTable GetCustomerByOpenID(string openId)
        {
            return dal.GetCustomerByOpenID(openId);

        }


        #endregion

        #region Set
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="info">info</param>
        public static int Add(CustomersInfo info)
        {
            return dal.Add(info);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="info">info</param>
        public static bool Edit(CustomersInfo info)
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