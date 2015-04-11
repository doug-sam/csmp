using System;
using System.Collections.Generic;
using CSMP.DAL;
using CSMP.Model;
using System.Web;
using System.Linq;

namespace CSMP.BLL
{
    public static class GroupBLL
    {
        private static readonly DAL.GroupDAL dal = new DAL.GroupDAL();

        #region Get
        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<GroupInfo> GetList(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            return dal.GetList(PageSize, CurPage, StrWhere, out Count);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<GroupInfo> GetList(string StrWhere)
        {
            return dal.GetList(StrWhere);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<GroupInfo> GetList()
        {
            return GetList(" 1=1 ");
        }


        /// <summary>
        /// 获取Info
        /// </summary>
        /// <param name="id">id</param>
        public static GroupInfo Get(int id)
        {
            return dal.Get(id);
        }

        #endregion

        #region Set
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="info">info</param>
        public static int Add(GroupInfo info)
        {
            return dal.Add(info);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="info">info</param>
        public static bool Edit(GroupInfo info)
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

        public static void EnterCheck(bool HavePower)
        {
            if (!HavePower)
            {
                EnterCheck(-1);
            }
        }
        public static void EnterCheck(int ID)
        {
            if (!PowerCheck(ID))
            {
                NoPowerRedirect();
            }
        }

        public static void NoPowerRedirect()
        {
            string PrePage = HttpContext.Current.Server.UrlEncode(HttpContext.Current.Request.Url.ToString());
            HttpContext.Current.Response.Write("<script>top.location.href='/page/sys/PowerError.html?PrePage=" + PrePage + "';</script>");
            HttpContext.Current.Response.End();
        }
        private const string Comma = ",";
        public static bool PowerCheck(int ID)
        {
            if (HttpContext.Current.Session["Power"] == null)
            {
                // Function.AlertMsg("没有session[power]");
                UserInfo info = UserBLL.GetCurrent();
                if (null == info)
                {
                    HttpContext.Current.Response.Redirect("/Logout.aspx");
                    return false;
                }
                GroupInfo Ginfo;
                if (info.Code == "admin")
                {
                    Ginfo = new GroupInfo();
                    Ginfo.Name = "admin";
                    Ginfo.PowerList = "admin";
                    Ginfo.ID = -1;
                }
                else
                {
                    Ginfo = GroupBLL.Get(info.PowerGroupID);
                }
                HttpContext.Current.Session["Power"] = Ginfo;
            }
            if (HttpContext.Current.Session["Power"] == null)
            {
                return false;
            }
            GroupInfo CurrentGroupInfo = ((GroupInfo)HttpContext.Current.Session["Power"]);
            if (CurrentGroupInfo.ID == -1 && CurrentGroupInfo.Name == "admin")
            {
                return true;
            }
            string Powers = Comma + CurrentGroupInfo.PowerList.Trim(',') + Comma;
            bool b = Powers.Contains(Comma + ID + Comma);
            return b;
        }

        public static List<string> GetListItem(GroupInfo info)
        {
            if (System.Web.HttpContext.Current.Application[info.ID.ToString()] == null)
            {
                if (null == info)
                {
                    return new List<string>();
                }
                System.Web.HttpContext.Current.Application[info.ID.ToString()] = info.ItemList;
            }
            return (List<string>)System.Web.HttpContext.Current.Application[info.ID.ToString()].ToString().Split(',').ToList();
        }

        public static List<string> GetListItem2(GroupInfo info)
        {
            if (System.Web.HttpContext.Current.Application["item2" + info.ID.ToString()] == null)
            {
                if (null == info)
                {
                    return new List<string>();
                }
                System.Web.HttpContext.Current.Application["item2"+info.ID.ToString()] = info.ItemList2;
            }
            return System.Web.HttpContext.Current.Application["item2" + info.ID.ToString()].ToString().Split(',').ToList();
        }

        public static List<string> GetListItem(int GroupID)
        {
            if (GroupID <= 0)
            {
                return new List<string>();
            }
            return GetListItem(Get(GroupID));
        }
        public static List<string> GetListItem2(int GroupID)
        {
            if (GroupID <= 0)
            {
                return new List<string>();
            }
            return GetListItem2(Get(GroupID));
        }
    }
}