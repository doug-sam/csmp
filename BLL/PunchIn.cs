using System;
using System.Collections.Generic;
using System.Text;
using CSMP.DAL;
using CSMP.Model;
using Tool;

namespace CSMP.BLL
{
    public static class PunchInBLL
    {
        private static readonly DAL.PunchInDAL dal = new DAL.PunchInDAL();

        #region Get
        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<PunchInInfo> GetList(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            return dal.GetList(PageSize, CurPage, StrWhere, out Count);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<PunchInInfo> GetList(string StrWhere)
        {
            return dal.GetList(StrWhere);
        }



        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<PunchInInfo> GetList(DateTime DateBegin, DateTime DateEnd, int UserID)
        {
            if (UserID <= 0)
            {
                return new List<PunchInInfo>();
            }
            StringBuilder StrWhere = new StringBuilder();
            StrWhere.Append(" 1=1 ");
            StrWhere.Append(" and f_UserID=").Append(UserID);
            StrWhere.Append(" and DATEDIFF(DAY,f_DateAdd,'").Append(DateBegin).Append("')<=0");
            StrWhere.Append(" and DATEDIFF(DAY,f_DateAdd,'").Append(DateEnd).Append("')>=0");
            //StrWhere.Append(" order by f_DateAdd  DESC");


            return dal.GetList(StrWhere.ToString());
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<PunchInInfo> GetList()
        {
            return GetList(" 1=1 ");
        }


        /// <summary>
        /// 获取Info
        /// </summary>
        /// <param name="id">id</param>
        public static PunchInInfo Get(int id)
        {
            return dal.Get(id);
        }
        public static PunchInInfo GetLastPunch(int UserID)
        {
            return dal.GetLastPunch(UserID);
        }

        #endregion

        #region Set
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="info">info</param>
        public static int Add(PunchInInfo info)
        {
            return dal.Add(info);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="info">info</param>
        public static bool Edit(PunchInInfo info)
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


        public static List<PunchInViewInfo> ToPunchInView(List<PunchInInfo> list)
        {
            List<PunchInViewInfo> result = new List<PunchInViewInfo>();
            DateTime DateLastRec =Tool.Function.ErrorDate;
            PunchInViewInfo info = new PunchInViewInfo();
            foreach (PunchInInfo item in list)
            {
                if (item.DateAdd.Date != DateLastRec.Date)
                {
                    DateLastRec = item.DateAdd.Date;
                    info = new PunchInViewInfo();
                    info.DateAdd = item.DateAdd;
                    info.ItemInfo = new List<PunchInItemInfo>();
                    result.Add(info);
                }
                PunchInItemInfo iteminfo = new PunchInItemInfo();
                iteminfo.Detail = item.DateRegisterAbs.ToString("HH:mm") + (item.IsStartWork == 1 ? "上班" : "下班");
                iteminfo.ID = item.ID;
                iteminfo.IsStartWork = item.IsStartWork;
                iteminfo.Memo = item.Memo;
                iteminfo.PositionAddress = item.PositionAddress;
                iteminfo.IsStartWork = item.IsStartWork;
                info.ItemInfo.Add(iteminfo);
            }
            Reverser<PunchInViewInfo> reverser = new Reverser<PunchInViewInfo>(typeof(PunchInViewInfo), "DateAdd", ReverserInfo.Direction.DESC);
            result.Sort(reverser);
            return result;
        }

    }
}