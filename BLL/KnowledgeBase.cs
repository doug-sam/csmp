using System;
using System.Collections.Generic;
using CSMP.DAL;
using CSMP.Model;
using System.Data;

namespace CSMP.BLL
{
    public static class KnowledgeBaseBLL
    {
        private static readonly DAL.KnowledgeBaseDAL dal = new DAL.KnowledgeBaseDAL();

        #region Get
        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<KnowledgeBaseInfo> GetList(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            return dal.GetList(PageSize, CurPage, StrWhere, out Count);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<KnowledgeBaseInfo> GetList(string StrWhere)
        {
            return dal.GetList(StrWhere);
        }

        /// <summary>
        /// 根据小类故障查找
        /// </summary>
        /// <param name="Class3ID"></param>
        /// <returns></returns>
        public static List<KnowledgeBaseInfo> GetListByBrandID(int BrandID)
        {
            return dal.GetListByBrandID(BrandID);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<KnowledgeBaseInfo> GetList()
        {
            return GetList(" 1=1 ");
        }


        /// <summary>
        /// 获取Info
        /// </summary>
        /// <param name="id">id</param>
        public static KnowledgeBaseInfo Get(int id)
        {
            return dal.Get(id);
        }
        /// <summary>
        /// 根据传来的客户ID和品牌ID显示列表
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="brandId"></param>
        /// <returns></returns>
        public static DataTable GetListByBothId(string openId, string customerId, string brandId)
        {
            return dal.GetListByBothId(openId,customerId, brandId);

        }

        #endregion

        #region Set
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="info">info</param>
        public static int Add(KnowledgeBaseInfo info)
        {
            return dal.Add(info);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="info">info</param>
        public static bool Edit(KnowledgeBaseInfo info)
        {
            return dal.Edit(info);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">id</param>
        public static bool Delete(int id)
        {
            List<AttachmentInfo> list = AttachmentBLL.GetList(id, AttachmentInfo.EUserFor.KnowledgeBase);
            if (null!=list&&list.Count>0)
            {
                foreach (AttachmentInfo item in list)
                {
                    AttachmentBLL.Delete(item.ID);
                }
            }
            return dal.Delete(id);
        }


        #endregion
    }
}