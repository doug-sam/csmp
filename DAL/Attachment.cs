using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using DBUtility;
using CSMP.Model;

namespace CSMP.DAL
{
    public class AttachmentDAL
    {
        private const string ALL_PARM = "  ID,f_UserID,f_UserName,f_CallID,f_CallStepID,f_DirID,f_Title,f_Ext,f_ContentType,f_FileSize,f_FilePath,f_Memo,f_Addtime,f_UseFor ";
        private const string FROM_TABLE = " from [sys_Attachment] ";
        private const string TABLE = " sys_Attachment ";
        private const string INSET = " (f_UserID,f_UserName,f_CallID,f_CallStepID,f_DirID,f_Title,f_Ext,f_ContentType,f_FileSize,f_FilePath,f_Memo,f_Addtime,f_UseFor) values(@UserID,@UserName,@CallID,@CallStepID,@DirID,@Title,@Ext,@ContentType,@FileSize,@FilePath,@Memo,@Addtime,@UseFor)  ";
        private const string UPDATE = " f_UserID=@UserID,f_UserName=@UserName,f_CallID=@CallID,f_CallStepID=@CallStepID,f_DirID=@DirID,f_Title=@Title,f_Ext=@Ext,f_ContentType=@ContentType,f_FileSize=@FileSize,f_FilePath=@FilePath,f_Memo=@Memo,f_Addtime=@Addtime,f_UseFor=@UseFor ";

        #region ReadyData
        private AttachmentInfo GetByDataReader(SqlDataReader rdr)
        {
            AttachmentInfo info = new AttachmentInfo();
            info.ID = Convert.ToInt32(rdr["ID"]);
            info.UserID = Convert.ToInt32(rdr["f_UserID"]);
            info.UserName = rdr["f_UserName"].ToString();
            info.CallID = Convert.ToInt32(rdr["f_CallID"]);
            info.CallStepID = Convert.ToInt32(rdr["f_CallStepID"]);
            info.DirID = Convert.ToInt32(rdr["f_DirID"]);
            info.Title = rdr["f_Title"].ToString();
            info.Ext = rdr["f_Ext"].ToString();
            info.ContentType = rdr["f_ContentType"].ToString();
            info.FileSize = Convert.ToInt32(rdr["f_FileSize"]);
            info.FilePath = rdr["f_FilePath"].ToString();
            info.Memo = rdr["f_Memo"].ToString();
            info.Addtime = Convert.ToDateTime(rdr["f_Addtime"]);
            info.UseFor = rdr["f_UseFor"].ToString().Trim();
            
            return info;
        }

        private SqlParameter[] GetParameter(AttachmentInfo info)
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter("@UserID", info.UserID),
            new SqlParameter("@UserName", info.UserName),
            new SqlParameter("@CallID", info.CallID),
            new SqlParameter("@CallStepID", info.CallStepID),
            new SqlParameter("@DirID", info.DirID),
            new SqlParameter("@Title", info.Title),
            new SqlParameter("@Ext", info.Ext),
            new SqlParameter("@ContentType", info.ContentType),
            new SqlParameter("@FileSize", info.FileSize),
            new SqlParameter("@FilePath", info.FilePath),
            new SqlParameter("@Memo", info.Memo),
            new SqlParameter("@Addtime", info.Addtime),
            new SqlParameter("@UseFor", info.UseFor),
            
            };

            return parms;
        }
        
        #endregion


        #region Get
        /// <summary>
        /// 获取列表
        /// </summary>
        public List<AttachmentInfo> GetList(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            List<AttachmentInfo> list = new List<AttachmentInfo>();
            string strSQL = Function.GetPageSQL(PageSize, CurPage, TABLE, StrWhere, out Count);
            using (SqlDataReader rdr = SqlHelper.ExecuteReader(SqlHelper.SqlconnString, CommandType.Text, strSQL, null))
            {
                while (rdr.Read())
                {
                    list.Add(GetByDataReader(rdr));
                }
            }
            return list;
        }

        public List<AttachmentInfo> GetList(string StrWhere)
        {
            List<AttachmentInfo> list = new List<AttachmentInfo>();
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("select ").Append(ALL_PARM).Append(FROM_TABLE).Append(" where ").Append(StrWhere);
            using (SqlDataReader rdr = SqlHelper.ExecuteReader(SqlHelper.SqlconnString, CommandType.Text, strSQL.ToString(), null))
            {
                while (rdr.Read())
                {
                    list.Add(GetByDataReader(rdr));
                }
            }
            return list;
        }
        /// <summary>
        /// 获取Info
        /// </summary>
        /// <param name="id">id</param>
        public AttachmentInfo Get(int id)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("select top 1 ").Append(ALL_PARM).Append(FROM_TABLE).Append(" where id = ").Append(id);

            using (SqlDataReader rdr = SqlHelper.ExecuteReader(SqlHelper.SqlconnString, CommandType.Text, strSQL.ToString(), null))
            {
                if (!rdr.Read()) return null;

                return GetByDataReader(rdr);
            }
        }
        
        #endregion


        #region set
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="info">info</param>
        public int Add(AttachmentInfo info)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("insert into ").Append(TABLE).Append(INSET);

            SqlParameter[] parms = GetParameter(info);

            if (SqlHelper.ExecuteNonQueryByTran(SqlHelper.SqlconnString, CommandType.Text, strSQL.ToString(), parms))
            {
                return Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.SqlconnString, CommandType.Text, "select max(id) from " + TABLE, null).ToString());
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="info">info</param>
        public bool Edit(AttachmentInfo info)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("update ").Append(TABLE).Append(" set ").Append(UPDATE).Append(" where id = ").Append(info.ID);

            SqlParameter[] parms = GetParameter(info);

            return SqlHelper.ExecuteNonQueryByTran(SqlHelper.SqlconnString, CommandType.Text, strSQL.ToString(), parms);
        }

        /// <summary>
        /// 删除Member
        /// </summary>
        /// <param name="id">Member id</param>
        public bool Delete(int id)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("delete ").Append(FROM_TABLE).Append(" where id = ").Append(id);

            return SqlHelper.ExecuteNonQueryByTran(SqlHelper.SqlconnString, CommandType.Text, strSQL.ToString(), null);
        }

        
        #endregion
    }
}