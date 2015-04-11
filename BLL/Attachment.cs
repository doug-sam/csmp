using System;
using System.Collections.Generic;
using CSMP.DAL;
using CSMP.Model;
using System.Configuration;
using System.IO;

namespace CSMP.BLL
{
    public static class AttachmentBLL
    {
        private static readonly DAL.AttachmentDAL dal = new DAL.AttachmentDAL();

        /// <summary>
        /// 获得存储路径
        /// </summary>
        /// <returns></returns>
        public static string GetAttachmentPath()
        {
            return ConfigurationSettings.AppSettings["AttachmentPath"];
        }
        /// <summary>
        /// 获得存储路径
        /// </summary>
        /// <returns></returns>
        public static string GetAttachmentPathTemp()
        {
            return ConfigurationSettings.AppSettings["AttachmentPathTemp"];
        }

        #region Get
        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<AttachmentInfo> GetList(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            return dal.GetList(PageSize, CurPage, StrWhere, out Count);
        }

        public static List<AttachmentInfo> GetList(int CallID,AttachmentInfo.EUserFor eUserFor)
        {
            string StrWhere = string.Format(" 1=1 and  f_CallID={0} and f_UseFor='{1}'",CallID,eUserFor.ToString());
            return GetList(StrWhere);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<AttachmentInfo> GetList(string StrWhere)
        {
            return dal.GetList(StrWhere);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<AttachmentInfo> GetList()
        {
            return GetList(" 1=1 ");
        }


        /// <summary>
        /// 获取Info
        /// </summary>
        /// <param name="id">id</param>
        public static AttachmentInfo Get(int id)
        {
            return dal.Get(id);
        }

        /// <summary>
        /// 文件保存时所保存的路径
        /// </summary>
        /// <returns></returns>
        public static string GetOrCreateFilePath()
        {
            string FilePath = string.Empty;
            FilePath = AttachmentBLL.GetAttachmentPath();
            FilePath += DateTime.Now.Year + "\\";
            FilePath += DateTime.Now.Month + "\\";
            FilePath += DateTime.Now.Day + "\\";

            if (!System.IO.Directory.Exists(FilePath))
            {
                System.IO.Directory.CreateDirectory(FilePath);
            }
            return FilePath;
        }

        #endregion

        #region Set
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="info">info</param>
        public static int Add(AttachmentInfo info)
        {
            return dal.Add(info);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="info">info</param>
        public static bool Edit(AttachmentInfo info)
        {
            return dal.Edit(info);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">id</param>
        public static bool Delete(int id)
        {
            AttachmentInfo info = AttachmentBLL.Get(id);
            if (null==info)
            {
                return true;
            }
            string FileDirecory=info.FilePath+info.Title+info.Ext;
            if (File.Exists(FileDirecory))
            {
                try
                {
                    File.Delete(FileDirecory);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return dal.Delete(id);
        }


        #endregion
    }
}