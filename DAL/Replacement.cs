using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using DBUtility;
using CSMP.Model;

namespace CSMP.DAL
{
    public class ReplacementDAL
    {
        private const string ALL_PARM = "  ID,f_CallID,f_StateID,f_StateName,f_MaintainUserID,f_MaintainUserName,f_RecordUserID,f_RecordUserName,f_DateAction,f_DateAdd,f_Detail,f_RpBrand,f_RpMode,f_RpSerialNo ";
        private const string FROM_TABLE = " from [sys_Replacement] ";
        private const string TABLE = " sys_Replacement ";
        private const string INSET = " (f_CallID,f_StateID,f_StateName,f_MaintainUserID,f_MaintainUserName,f_RecordUserID,f_RecordUserName,f_DateAction,f_DateAdd,f_Detail,f_RpBrand,f_RpMode,f_RpSerialNo) values(@CallID,@StateID,@StateName,@MaintainUserID,@MaintainUserName,@RecordUserID,@RecordUserName,@DateAction,@DateAdd,@Detail,@RpBrand,@RpMode,@RpSerialNo)  ";
        private const string UPDATE = " f_CallID=@CallID,f_StateID=@StateID,f_StateName=@StateName,f_MaintainUserID=@MaintainUserID,f_MaintainUserName=@MaintainUserName,f_RecordUserID=@RecordUserID,f_RecordUserName=@RecordUserName,f_DateAction=@DateAction,f_DateAdd=@DateAdd,f_Detail=@Detail,f_RpBrand=@RpBrand,f_RpMode=@RpMode,f_RpSerialNo=@RpSerialNo ";

        #region ReadyData
        private ReplacementInfo GetByDataReader(SqlDataReader rdr)
        {
            ReplacementInfo info = new ReplacementInfo();
            info.ID = Convert.ToInt32(rdr["ID"]);
            info.CallID = Convert.ToInt32(rdr["f_CallID"]);
            info.StateID = Convert.ToInt32(rdr["f_StateID"]);
            info.StateName = rdr["f_StateName"].ToString();
            info.MaintainUserID = Convert.ToInt32(rdr["f_MaintainUserID"]);
            info.MaintainUserName = rdr["f_MaintainUserName"].ToString();
            info.RecordUserID = Convert.ToInt32(rdr["f_RecordUserID"]);
            info.RecordUserName = rdr["f_RecordUserName"].ToString();
            info.DateAction = Convert.ToDateTime(rdr["f_DateAction"]);
            info.DateAdd = Convert.ToDateTime(rdr["f_DateAdd"]);
            info.Detail = rdr["f_Detail"].ToString();
            info.RpBrand = rdr["f_RpBrand"].ToString();
            info.RpMode = rdr["f_RpMode"].ToString();
            info.RpSerialNo = rdr["f_RpSerialNo"].ToString();
            
            return info;
        }

        private SqlParameter[] GetParameter(ReplacementInfo info)
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter("@CallID", info.CallID),
            new SqlParameter("@StateID", info.StateID),
            new SqlParameter("@StateName", info.StateName),
            new SqlParameter("@MaintainUserID", info.MaintainUserID),
            new SqlParameter("@MaintainUserName", info.MaintainUserName),
            new SqlParameter("@RecordUserID", info.RecordUserID),
            new SqlParameter("@RecordUserName", info.RecordUserName),
            new SqlParameter("@DateAction", info.DateAction),
            new SqlParameter("@DateAdd", info.DateAdd),
            new SqlParameter("@Detail", info.Detail),
            new SqlParameter("@RpBrand", info.RpBrand),
            new SqlParameter("@RpMode", info.RpMode),
            new SqlParameter("@RpSerialNo", info.RpSerialNo),
            
            };

            return parms;
        }
        
        #endregion


        #region Get
        /// <summary>
        /// 获取列表
        /// </summary>
        public List<ReplacementInfo> GetList(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            List<ReplacementInfo> list = new List<ReplacementInfo>();
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

        public List<ReplacementInfo> GetList(string StrWhere)
        {
            List<ReplacementInfo> list = new List<ReplacementInfo>();
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

        public List<ReplacementInfo> GetList(int CallID,string SerialNo)
        {
            List<ReplacementInfo> list = new List<ReplacementInfo>();
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("select ").Append(ALL_PARM).Append(FROM_TABLE).Append(" where f_CallID=@CallID and f_RpSerialNo=@RpSerialNo");
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter("@CallID", CallID),
                new SqlParameter("@RpSerialNo", SerialNo),
            };

            using (SqlDataReader rdr = SqlHelper.ExecuteReader(SqlHelper.SqlconnString, CommandType.Text, strSQL.ToString(), parms))
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
        public ReplacementInfo Get(int id)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("select top 1 ").Append(ALL_PARM).Append(FROM_TABLE).Append(" where id = ").Append(id);

            using (SqlDataReader rdr = SqlHelper.ExecuteReader(SqlHelper.SqlconnString, CommandType.Text, strSQL.ToString(), null))
            {
                if (!rdr.Read()) return null;

                return GetByDataReader(rdr);
            }
        }

        public List<string> GetSerialNo(int CallID)
        {
            List<string> list = new List<string>();
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("select f_RpSerialNo ").Append(FROM_TABLE).Append(" where f_CallID=").Append(CallID);
            strSQL.Append(" group by f_RpSerialNo ");
            using (SqlDataReader rdr = SqlHelper.ExecuteReader(SqlHelper.SqlconnString, CommandType.Text, strSQL.ToString(), null))
            {
                while (rdr.Read())
                {
                    list.Add(rdr["f_RpSerialNo"].ToString());
                }
            }
            return list;

        }

        #endregion


        #region set
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="info">info</param>
        public int Add(ReplacementInfo info)
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
        public bool Edit(ReplacementInfo info)
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
        /// 删除Member
        /// </summary>
        /// <param name="id">Member id</param>
        public bool DeleteBySerialNo(string SerialNo)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("delete ").Append(FROM_TABLE).Append(" where f_RpSerialNo =@RpSerialNo ");
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter("@RpSerialNo", SerialNo),
            };
            return SqlHelper.ExecuteNonQueryByTran(SqlHelper.SqlconnString, CommandType.Text, strSQL.ToString(), parms);
        }

        
        #endregion
    }
}