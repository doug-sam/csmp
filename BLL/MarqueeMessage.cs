using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSMP.DAL;
using CSMP.Model;

namespace CSMP.BLL
{
    public static class MarqueeMessageBLL
    {
        private static readonly DAL.MarqueeMessageDAL dal = new DAL.MarqueeMessageDAL();
        /// <summary>
        /// 根据二线负责人ID查
        /// </summary>
        /// <param name="value"></param>
        /// <returns>无则返回null</returns>
        public static List<MarqueeMessage> GetByMaintaimUserID(int MaintaimUserID)
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
        public static List<MarqueeMessage> GetList(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            return dal.GetList(PageSize, CurPage, StrWhere, out Count);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<MarqueeMessage> GetList(string StrWhere)
        {
            return dal.GetList(StrWhere);
        }
        /// <summary>
        /// 查询总记录数
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static int CountAll()
        {
            return dal.CountAll();
        }

        #region Set
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="info">info</param>
        public static int Add(MarqueeMessage info)
        {
            return dal.Add(info);
        }
        /// <summary>
        /// 通过SP插入或更新一条记录
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string AddBySP(MarqueeMessage info)
        {
            return dal.AddBySP(info);
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="info">info</param>
        public static bool Edit(MarqueeMessage info)
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
