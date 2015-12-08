using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSMP.DAL;
using CSMP.Model;
using System.Data;

namespace CSMP.BLL
{
    public class MarqueeMessageReportBLL
    {
        private static readonly DAL.MarqueeMessageReportDAL dal = new DAL.MarqueeMessageReportDAL();
        /// <summary>
        /// 根据二线负责人ID查
        /// </summary>
        /// <param name="value"></param>
        /// <returns>无则返回null</returns>
        public static List<MarqueeMessageReport> GetByMaintaimUserID(int MaintaimUserID)
        {
            return dal.GetByMaintaimUserID(MaintaimUserID);
        }
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="CurPage"></param>
        /// <param name="StrWhere"></param>
        /// <param name="Count"></param>
        /// <returns></returns>
        public static List<MarqueeMessageReport> GetList(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            return dal.GetList(PageSize, CurPage, StrWhere, out Count);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<MarqueeMessageReport> GetList(string StrWhere)
        {
            return dal.GetList(StrWhere);
        }
        /// <summary>
        /// 统计APP使用情况
        /// </summary>
        /// <param name="strSQL"></param>
        /// <returns></returns>
        public static DataTable GetDailyReport(string strSQL)
        {
            return dal.GetDailyReport(strSQL);
        }
        /// <summary>
        /// 统计APP使用情况，使用SP
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="majorUserID"></param>
        /// <returns></returns>
        public static DataTable GetDailyReportBySP(string startTime,string endTime, string majorUserName,int provinceID,int workgroupID,int majorUserID)
        {
            return dal.GetDailyReportBySP( startTime,  endTime,  majorUserName, provinceID, workgroupID, majorUserID);
        }

        #region Set
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="info">info</param>
        public static int Add(MarqueeMessageReport info)
        {
            return dal.Add(info);
        }
        
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="info">info</param>
        public static bool Edit(MarqueeMessageReport info)
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
        /// <summary>
        /// 删除通过 callNo
        /// </summary>
        /// <param name="No"></param>
        /// <returns></returns>
        public static bool Delete(string No)
        {
            return dal.Delete(No);
        }

        #endregion
    }
}
