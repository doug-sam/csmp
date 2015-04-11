using System;
using System.Collections.Generic;
using CSMP.DAL;
using CSMP.Model;
using System.Text;
using System.Linq;

namespace CSMP.BLL
{
    public static class SlaModeDetailBLL
    {
        private static List<SlaModeDetailInfo> listDetail = new List<SlaModeDetailInfo>();



        private static readonly DAL.SlaModeDetailDAL dal = new DAL.SlaModeDetailDAL();

        #region Get
        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<SlaModeDetailInfo> GetList(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            return dal.GetList(PageSize, CurPage, StrWhere, out Count);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<SlaModeDetailInfo> GetList(string StrWhere)
        {
            return dal.GetList(StrWhere);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<SlaModeDetailInfo> GetList(DayOfWeek dayOfWeek, CallInfo info)
        {


            StringBuilder sb = new StringBuilder();
            sb.Append(" 1=1 ");
            sb.Append(" and f_DayOfWeek='").Append(dayOfWeek.ToString()).Append("'");
            sb.Append(" and [f_SlaModeID]=(");
            sb.Append("                     select f_SlaModeID from sys_Brand where ID=").Append(info.BrandID);
            sb.Append(" )");

            return GetList(sb.ToString());
        }


        /// <summary>
        /// 获取Info
        /// </summary>
        /// <param name="id">id</param>
        public static SlaModeDetailInfo Get(int id)
        {
            return dal.Get(id);
        }

        /// <summary>
        /// 获取Info
        /// </summary>
        /// <param name="id">id</param>
        public static SlaModeDetailInfo GetNext(int SlaModeID, int MoreThanID)
        {
            return dal.GetNext(SlaModeID, MoreThanID);
        }

        public static List<SlaModeDetailInfo> GetListByCache(DayOfWeek dayOfWeek, CallInfo info)
        {
            List<SlaModeDetailInfo> listOfAll = GetListDetail();
           return listOfAll.Where(d => d.DayOfWeek == dayOfWeek.ToString()).ToList();
        }

        public static List<SlaModeDetailInfo> GetListDetail()
        {
            if (null == listDetail && listDetail.Count <= 0)
            {
                listDetail = GetList(" 1=1 ");
            }
            return listDetail;
        }
        public static void CleanListDetailCache()
        {
            listDetail = new List<SlaModeDetailInfo>();
            listDetail.Clear();
        }

        #endregion

        #region Set
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="info">info</param>
        public static int Add(SlaModeDetailInfo info)
        {
            return dal.Add(info);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="info">info</param>
        public static bool Edit(SlaModeDetailInfo info)
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


        ///// <summary>
        ///// 已用存储过程实现
        ///// </summary>
        ///// <param name="info"></param>
        ///// <param name="TiredCalcoulator"></param>
        ///// <returns></returns>
        //public static DateTime GetEndTime(CallInfo info, out int TiredCalcoulator)
        //{
        //    info.SLAMinute = info.SLA*60;
        //    DateTime EndTime = info.ErrorDate;
        //    int SlaMinuteCalcoulator = 0;//WorkTime累加器
        //    TiredCalcoulator = 0;//疲劳累加器，过4320分钟都没算出来，就返回错误时间
        //    while (SlaMinuteCalcoulator < info.SLAMinute)
        //    {//TODO:这里可以优化，先直接以小时累加，超出范围的情况下再以分钟累加

        //        if (IsWorkTime(info, EndTime))
        //        {
        //            SlaMinuteCalcoulator++;
        //        }
        //        EndTime = EndTime.AddMinutes(1);
        //        TiredCalcoulator++;
        //        if (TiredCalcoulator >= 4320)
        //        {
        //            return Tool.Function.ErrorDate;
        //        }
        //    }
        //    return EndTime;
        //}


        ///// <summary>
        ///// 判断这endtime值是否属于工作时间///已用存储过程实现
        ///// 已用存储过程实现
        ///// </summary>
        ///// <param name="info"></param>
        ///// <param name="EndTime"></param>
        ///// <returns></returns>
        //private static bool IsWorkTime(CallInfo info, DateTime EndTime)
        //{
        //    //List<CallSuppendInfo> listSuppend = CallSuppendBLL.GetList(info.ID);
        //    //foreach (CallSuppendInfo item in listSuppend)
        //    //{
        //    //    if (EndTime >= item.DateStart && EndTime <= item.DateEnd)
        //    //    {
        //    //        return false;
        //    //    }
        //    //}
        //    List<SlaModeDetailInfo> listWorkTime = SlaModeDetailBLL.GetList(EndTime.DayOfWeek, info);
        //    foreach (SlaModeDetailInfo item in listWorkTime)
        //    {
        //        if (EndTime.TimeOfDay >= item.TimerStart.TimeOfDay && EndTime.TimeOfDay <= item.TimeEnd.TimeOfDay)
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}



    }
}