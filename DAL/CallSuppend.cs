using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using DBUtility;
using CSMP.Model;

namespace CSMP.DAL
{
    public class CallSuppendDAL
    {
        private const string ALL_PARM = "  ID,f_CallID,f_DateStart,f_DateEnd,f_Reason,f_UserID,f_UserName,f_AddDate ";
        private const string FROM_TABLE = " from [sys_CallSuppend] ";
        private const string TABLE = " sys_CallSuppend ";
        private const string INSET = " (f_CallID,f_DateStart,f_DateEnd,f_Reason,f_UserID,f_UserName,f_AddDate) values(@CallID,@DateStart,@DateEnd,@Reason,@UserID,@UserName,@AddDate)  ";
        private const string UPDATE = " f_CallID=@CallID,f_DateStart=@DateStart,f_DateEnd=@DateEnd,f_Reason=@Reason,f_UserID=@UserID,f_UserName=@UserName,f_AddDate=@AddDate ";

        #region ReadyData
        private CallSuppendInfo GetByDataReader(SqlDataReader rdr)
        {
            CallSuppendInfo info = new CallSuppendInfo();
            info.ID = Convert.ToInt32(rdr["ID"]);
            info.CallID = Convert.ToInt32(rdr["f_CallID"]);
            info.DateStart = Convert.ToDateTime(rdr["f_DateStart"]);
            info.DateEnd = Convert.ToDateTime(rdr["f_DateEnd"]);
            info.Reason = rdr["f_Reason"].ToString();
            info.UserID = Convert.ToInt32(rdr["f_UserID"]);
            info.UserName = rdr["f_UserName"].ToString();
            info.AddDate = Convert.ToDateTime(rdr["f_AddDate"]);
            
            return info;
        }

        private SqlParameter[] GetParameter(CallSuppendInfo info)
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter("@CallID", info.CallID),
            new SqlParameter("@DateStart", info.DateStart),
            new SqlParameter("@DateEnd", info.DateEnd),
            new SqlParameter("@Reason", info.Reason),
            new SqlParameter("@UserID", info.UserID),
            new SqlParameter("@UserName", info.UserName),
            new SqlParameter("@AddDate", info.AddDate),
            
            };

            return parms;
        }
        
        #endregion


        #region Get
        /// <summary>
        /// 获取列表
        /// </summary>
        public List<CallSuppendInfo> GetList(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            List<CallSuppendInfo> list = new List<CallSuppendInfo>();
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

        public List<CallSuppendInfo> GetList(string StrWhere)
        {
            List<CallSuppendInfo> list = new List<CallSuppendInfo>();
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
        public CallSuppendInfo Get(int id)
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
        public int Add(CallSuppendInfo info)
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
        public bool Edit(CallSuppendInfo info)
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