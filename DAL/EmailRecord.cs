using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using DBUtility;
using CSMP.Model;

namespace CSMP.DAL
{
    public class EmailRecordDAL
    {
        private const string ALL_PARM = "  ID,f_UserID,f_UserName,f_CallNo,f_CallID,f_DateAdd,f_ToUser ";
        private const string FROM_TABLE = " from [sys_EmailRecord] ";
        private const string TABLE = " sys_EmailRecord ";
        private const string INSET = " (f_UserID,f_UserName,f_CallNo,f_CallID,f_DateAdd,f_ToUser) values(@UserID,@UserName,@CallNo,@CallID,@DateAdd,@ToUser)  ";
        private const string UPDATE = " f_UserID=@UserID,f_UserName=@UserName,f_CallNo=@CallNo,f_CallID=@CallID,f_DateAdd=@DateAdd,f_ToUser=@ToUser ";

        #region ReadyData
        private EmailRecordInfo GetByDataReader(SqlDataReader rdr)
        {
            EmailRecordInfo info = new EmailRecordInfo();
            info.ID = Convert.ToInt32(rdr["ID"]);
            info.UserID = Convert.ToInt32(rdr["f_UserID"]);
            info.UserName = rdr["f_UserName"].ToString();
            info.CallNo = rdr["f_CallNo"].ToString();
            info.CallID = Convert.ToInt32(rdr["f_CallID"]);
            info.DateAdd = Convert.ToDateTime(rdr["f_DateAdd"]);
            info.ToUser = rdr["f_ToUser"].ToString();

            return info;
        }

        private SqlParameter[] GetParameter(EmailRecordInfo info)
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter("@UserID", info.UserID),
            new SqlParameter("@UserName", info.UserName),
            new SqlParameter("@CallNo", info.CallNo),
            new SqlParameter("@CallID", info.CallID),
            new SqlParameter("@DateAdd", info.DateAdd),
            new SqlParameter("@ToUser", info.ToUser),
            
            };

            return parms;
        }

        #endregion


        #region Get
        /// <summary>
        /// 获取列表
        /// </summary>
        public List<EmailRecordInfo> GetList(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            List<EmailRecordInfo> list = new List<EmailRecordInfo>();
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

        public List<EmailRecordInfo> GetList(string StrWhere)
        {
            List<EmailRecordInfo> list = new List<EmailRecordInfo>();
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
        public EmailRecordInfo Get(int id)
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
        public int Add(EmailRecordInfo info)
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
        public bool Edit(EmailRecordInfo info)
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
        /// <summary>
        /// 删除
        /// </summary>
        public bool Delete(int UserID, int CallID, DateTime DateStart, DateTime DateEnd)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("delete ").Append(FROM_TABLE).Append(" where 1=1 ");
            if (UserID > 0)
            {
                strSQL.Append(" and f_UserID=").Append(UserID);
            }
            if (CallID > 0)
            {
                strSQL.Append(" and f_CallID=").Append(CallID);
            }
            if (DateStart > DicInfo.DateZone)
            {
                strSQL.Append(" and DATEDIFF(day,f_DateAdd,'").Append(DateStart).Append("')<=0 ");
            }
            if (DateEnd > DicInfo.DateZone)
            {
                strSQL.Append(" and DATEDIFF(day,f_DateAdd,'").Append(DateStart).Append("')>=0 ");
            }
            return SqlHelper.ExecuteNonQueryByTran(SqlHelper.SqlconnString, CommandType.Text, strSQL.ToString(), null);
        }


        #endregion
    }
}