using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using DBUtility;
using CSMP.Model;

namespace CSMP.DAL
{
    public class SpanTimeDAL
    {
        private const string ALL_PARM = "  ID,f_CallID,f_CallNo,f_AddDate,f_UserIDStart,f_UserNameStart,f_DateBegin,f_DateEnd,f_Hours,f_StartupBy,f_Reason,f_Memo,f_TotalMinutes,f_UserIDStop,f_UserNameStop ";
        private const string FROM_TABLE = " from [sys_SpanTime] ";
        private const string TABLE = " sys_SpanTime ";
        private const string INSET = " (f_CallID,f_CallNo,f_AddDate,f_UserIDStart,f_UserNameStart,f_DateBegin,f_DateEnd,f_Hours,f_StartupBy,f_Reason,f_Memo,f_TotalMinutes,f_UserIDStop,f_UserNameStop) values(@CallID,@CallNo,@AddDate,@UserIDStart,@UserNameStart,@DateBegin,@DateEnd,@Hours,@StartupBy,@Reason,@Memo,@TotalMinutes,@UserIDStop,@UserNameStop)  ";
        private const string UPDATE = " f_CallID=@CallID,f_CallNo=@CallNo,f_AddDate=@AddDate,f_UserIDStart=@UserIDStart,f_UserNameStart=@UserNameStart,f_DateBegin=@DateBegin,f_DateEnd=@DateEnd,f_Hours=@Hours,f_StartupBy=@StartupBy,f_Reason=@Reason,f_Memo=@Memo,f_TotalMinutes=@TotalMinutes,f_UserIDStop=@UserIDStop,f_UserNameStop=@UserNameStop ";

        #region ReadyData
        private SpanTimeInfo GetByDataReader(SqlDataReader rdr)
        {
            SpanTimeInfo info = new SpanTimeInfo();
            info.ID = Convert.ToInt32(rdr["ID"]);
            info.CallID = Convert.ToInt32(rdr["f_CallID"]);
            info.CallNo = rdr["f_CallNo"].ToString();
            info.AddDate = Convert.ToDateTime(rdr["f_AddDate"]);
            info.UserIDStart = Convert.ToInt32(rdr["f_UserIDStart"]);
            info.UserNameStart = rdr["f_UserNameStart"].ToString();
            info.DateBegin = Convert.ToDateTime(rdr["f_DateBegin"]);
            info.DateEnd = Convert.ToDateTime(rdr["f_DateEnd"]);
            info.Hours = Convert.ToDecimal(Convert.ToDecimal(rdr["f_Hours"]).ToString("G0"));
            info.StartupBy = Convert.ToInt32(rdr["f_StartupBy"]);
            info.Reason = rdr["f_Reason"].ToString();
            info.Memo = rdr["f_Memo"].ToString();
            info.TotalMinutes = Convert.ToInt32(rdr["f_TotalMinutes"]);
            info.UserIDStop = Convert.ToInt32(rdr["f_UserIDStop"]);
            info.UserNameStop = rdr["f_UserNameStop"].ToString();
            
            return info;
        }

        private SqlParameter[] GetParameter(SpanTimeInfo info)
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter("@CallID", info.CallID),
            new SqlParameter("@CallNo", info.CallNo),
            new SqlParameter("@AddDate", info.AddDate),
            new SqlParameter("@UserIDStart", info.UserIDStart),
            new SqlParameter("@UserNameStart", info.UserNameStart),
            new SqlParameter("@DateBegin", info.DateBegin),
            new SqlParameter("@DateEnd", info.DateEnd),
            new SqlParameter("@Hours", info.Hours),
            new SqlParameter("@StartupBy", info.StartupBy),
            new SqlParameter("@Reason", info.Reason),
            new SqlParameter("@Memo", info.Memo),
            new SqlParameter("@TotalMinutes", info.TotalMinutes),
            new SqlParameter("@UserIDStop", info.UserIDStop),
            new SqlParameter("@UserNameStop", info.UserNameStop),
            
            };

            return parms;
        }
        
        #endregion


        #region Get
        /// <summary>
        /// 获取列表
        /// </summary>
        public List<SpanTimeInfo> GetList(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            List<SpanTimeInfo> list = new List<SpanTimeInfo>();
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

        public List<SpanTimeInfo> GetList(string StrWhere)
        {
            List<SpanTimeInfo> list = new List<SpanTimeInfo>();
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
        public List<SpanTimeInfo> GetList(int CallID)
        {
            List<SpanTimeInfo> list = new List<SpanTimeInfo>();
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("select ").Append(ALL_PARM).Append(FROM_TABLE).Append(" where f_CallID=").Append(CallID);
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
        public SpanTimeInfo Get(int id)
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
        public int Add(SpanTimeInfo info)
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
        public bool Edit(SpanTimeInfo info)
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