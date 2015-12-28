using System;
using System.Collections.Generic;
using CSMP.DAL;
using CSMP.Model;
using System.Text;
using Tool;
namespace CSMP.BLL
{
    public static class  LeftMenuDataBLL
    {
        private static readonly DAL.LeftMenuDataDAL dal = new DAL.LeftMenuDataDAL();
        #region Get

        public static List<LeftMenuData> GetList(string StrWhere)
        {
            return dal.GetList(StrWhere);
        }
        /// <summary>
        /// 调用SP 获取列表
        /// </summary>
        /// <param name="StrWhere"></param>
        /// <returns></returns>
        public static List<LeftMenuData> GetListBySP()
        {
            return dal.GetListBySP();
        }

        /// <summary>
        /// 获取Info
        /// </summary>
        /// <param name="id">id</param>
        public static LeftMenuData Get(int UserID)
        {
            return dal.Get(UserID);
        }

        #endregion


        #region set
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="info">info</param>
        public static int Add(LeftMenuData info)
        {
            return dal.Add(info);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="info">info</param>
        public static bool Edit(LeftMenuData info)
        {
            return dal.Edit(info);
        }

        /// <summary>
        /// 删除Member
        /// </summary>
        /// <param name="id">Member id</param>
        public static bool Delete(int UserID)
        {
            return dal.Delete(UserID);
        }

        /// <summary>
        /// 获取当前用户所有负责的追call
        /// </summary>
        /// <param name="CallID"></param>
        /// <returns></returns>
        public static List<CallInfo> GetListTraceByCurrentUser(bool havePower,UserInfo info)
        {
            StringBuilder strWhere = new StringBuilder();
            strWhere.Append(" f_StateMain in(").Append((int)SysEnum.CallStateMain.处理中).Append(",").Append((int)SysEnum.CallStateMain.未处理).Append(") ");
            strWhere.Append("   AND ID IN(");
            //strWhere.Append("       SELECT DISTINCT f_CallID FROM sys_CallStep WHERE f_StepType=").Append((int)SysEnum.StepType.店铺催促);
            strWhere.Append("       SELECT MAX(f_CallID) FROM sys_CallStep WHERE f_StepType=").Append((int)SysEnum.StepType.店铺催促).Append(" group by f_CallID");
            strWhere.Append("   )");

            if (havePower)
            {
                strWhere.Append(" AND f_BrandID IN(SELECT f_MID FROM sys_WorkGroupBrand WHERE f_WorkGroupID=").Append(info.WorkGroupID).Append(") ");
            }
            else
            {
                strWhere.Append(" AND f_MaintainUserID=").Append(info.ID);
            }

            return CallBLL.GetList(strWhere.ToString());
        }

        /// <summary>
        /// 根据 UserID 从缓存中获取该用户对应的数据
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static LeftMenuData GetLeftMenuCacheByName(int userID)
        {

            List<LeftMenuData> list;
            object obj = CacheManage.GetSearch("leftMenuKey");
            if (obj == null)
            {
                InsertLeftMenuDataCache();
                list = CacheManage.GetSearch("leftMenuKey") as List<LeftMenuData>;
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].UserID == userID)
                    {
                        return list[i];

                    }
                }
                return null;
            }
            else
            {
                list = obj as List<LeftMenuData>;
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].UserID == userID)
                    {
                        return list[i];

                    }
                }

                return null;

            }

        }

        /// <summary>
        /// 更新LeftMenu在数据库中 对应的数据，并将最新的数据从数据库中取出写入缓存中
        /// </summary>
        /// <returns></returns>
        public static bool InsertLeftMenuDataCache()
        {
            try {
                List<LeftMenuData> dataList = new List<LeftMenuData>();
                dataList = LeftMenuDataBLL.GetListBySP();
                CacheManage.InsertCache("leftMenuKey", dataList);
                return true;
            }
            catch(Exception ex) {
                return false;
            }
        }

        
        #endregion
    }
}
