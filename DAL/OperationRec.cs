using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using DBUtility;
using CSMP.Model;

namespace CSMP.DAL
{
    public class OperationRecDAL
    {
        private const string ALL_PARM = "  ID,f_CallID,f_UserID,f_UserName,f_LogType,f_Memo,f_FlagID,f_Details,f_AddDate ";
        private const string FROM_TABLE = " from [sys_OperationRec] ";
        private const string TABLE = " sys_OperationRec ";
        private const string INSET = " (f_CallID,f_UserID,f_UserName,f_LogType,f_Memo,f_FlagID,f_Details,f_AddDate) values(@CallID,@UserID,@UserName,@LogType,@Memo,@FlagID,@Details,@AddDate)  ";
        private const string UPDATE = " f_CallID=@CallID,f_UserID=@UserID,f_UserName=@UserName,f_LogType=@LogType,f_Memo=@Memo,f_FlagID=@FlagID,f_Details=@Details,f_AddDate=@AddDate ";

        #region ReadyData
        private OperationRecInfo GetByDataReader(SqlDataReader rdr)
        {
            OperationRecInfo info = new OperationRecInfo();
            info.ID = Convert.ToInt32(rdr["ID"]);
            info.CallID = Convert.ToInt32(rdr["f_CallID"]);
            info.UserID = Convert.ToInt32(rdr["f_UserID"]);
            info.UserName = rdr["f_UserName"].ToString();
            info.LogType = rdr["f_LogType"].ToString();
            info.Memo = rdr["f_Memo"].ToString();
            info.FlagID = Convert.ToInt32(rdr["f_FlagID"]);
            info.Details = rdr["f_Details"].ToString();
            info.AddDate = Convert.ToDateTime(rdr["f_AddDate"]);
            
            return info;
        }

        private SqlParameter[] GetParameter(OperationRecInfo info)
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter("@CallID", info.CallID),
            new SqlParameter("@UserID", info.UserID),
            new SqlParameter("@UserName", info.UserName),
            new SqlParameter("@LogType", info.LogType),
            new SqlParameter("@Memo", info.Memo),
            new SqlParameter("@FlagID", info.FlagID),
            new SqlParameter("@Details", info.Details),
            new SqlParameter("@AddDate", info.AddDate),
            
            };

            return parms;
        }
        
        #endregion


        #region Get
        /// <summary>
        /// 获取列表
        /// </summary>
        public List<OperationRecInfo> GetList(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            List<OperationRecInfo> list = new List<OperationRecInfo>();
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

        public List<OperationRecInfo> GetList(string StrWhere)
        {
            List<OperationRecInfo> list = new List<OperationRecInfo>();
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
        public OperationRecInfo Get(int id)
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
        public int Add(OperationRecInfo info)
        {
            StringBuilder strSQL;
            SqlParameter[] parms;
            AddSQL(info, out strSQL, out parms);

            if (SqlHelper.ExecuteNonQueryByTran(SqlHelper.SqlconnString, CommandType.Text, strSQL.ToString(), parms))
            {
                return Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.SqlconnString, CommandType.Text, "select max(id) from " + TABLE, null).ToString());
            }
            else
            {
                return 0;
            }
        }

        public void AddSQL(OperationRecInfo info, out StringBuilder strSQL, out SqlParameter[] parms)
        {
            strSQL = new StringBuilder();
            strSQL.Append("insert into ").Append(TABLE).Append(INSET);

            parms = GetParameter(info);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="info">info</param>
        public bool Edit(OperationRecInfo info)
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