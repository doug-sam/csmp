using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using DBUtility;
using CSMP.Model;

namespace CSMP.DAL
{
    public class PunchInDAL
    {
        private const string ALL_PARM = "  ID,f_AddByUserID,f_AddByUserName,f_UserID,f_UserName,f_GroupID,f_DateAdd,f_DateRegister,f_DateRegisterAbs,f_Device,f_Memo,f_IsStartWork,f_PositionAddress,f_PositionLog,f_PositionLat,f_DeviceDetail ";
        private const string FROM_TABLE = " from [sys_PunchIn] ";
        private const string TABLE = " sys_PunchIn ";
        private const string INSET = " (f_AddByUserID,f_AddByUserName,f_UserID,f_UserName,f_GroupID,f_DateAdd,f_DateRegister,f_DateRegisterAbs,f_Device,f_Memo,f_IsStartWork,f_PositionAddress,f_PositionLog,f_PositionLat,f_DeviceDetail) values(@AddByUserID,@AddByUserName,@UserID,@UserName,@GroupID,@DateAdd,@DateRegister,@DateRegisterAbs,@Device,@Memo,@IsStartWork,@PositionAddress,@PositionLog,@PositionLat,@DeviceDetail)  ";
        private const string UPDATE = " f_AddByUserID=@AddByUserID,f_AddByUserName=@AddByUserName,f_UserID=@UserID,f_UserName=@UserName,f_GroupID=@GroupID,f_DateAdd=@DateAdd,f_DateRegister=@DateRegister,f_DateRegisterAbs=@DateRegisterAbs,f_Device=@Device,f_Memo=@Memo,f_IsStartWork=@IsStartWork,f_PositionAddress=@PositionAddress,f_PositionLog=@PositionLog,f_PositionLat=@PositionLat,f_DeviceDetail=@DeviceDetail ";

        #region ReadyData
        private PunchInInfo GetByDataReader(SqlDataReader rdr)
        {
            PunchInInfo info = new PunchInInfo();
            info.ID = Convert.ToInt32(rdr["ID"]);
            info.AddByUserID = Convert.ToInt32(rdr["f_AddByUserID"]);
            info.AddByUserName = rdr["f_AddByUserName"].ToString();
            info.UserID = Convert.ToInt32(rdr["f_UserID"]);
            info.UserName = rdr["f_UserName"].ToString();
            info.GroupID = Convert.ToInt32(rdr["f_GroupID"]);
            info.DateAdd = Convert.ToDateTime(rdr["f_DateAdd"]);
            info.DateRegister = Convert.ToDateTime(rdr["f_DateRegister"]);
            info.DateRegisterAbs = Convert.ToDateTime(rdr["f_DateRegisterAbs"]);
            info.Device = rdr["f_Device"].ToString();
            info.Memo = rdr["f_Memo"].ToString();
            info.IsStartWork = Convert.ToInt32(rdr["f_IsStartWork"]);
            info.PositionAddress = rdr["f_PositionAddress"].ToString();
            info.PositionLog = Convert.ToDecimal(Convert.ToDecimal(rdr["f_PositionLog"]).ToString("G0"));
            info.PositionLat = Convert.ToDecimal(Convert.ToDecimal(rdr["f_PositionLat"]).ToString("G0"));
            info.DeviceDetail = rdr["f_DeviceDetail"].ToString();
            
            return info;
        }

        private SqlParameter[] GetParameter(PunchInInfo info)
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter("@AddByUserID", info.AddByUserID),
            new SqlParameter("@AddByUserName", info.AddByUserName),
            new SqlParameter("@UserID", info.UserID),
            new SqlParameter("@UserName", info.UserName),
            new SqlParameter("@GroupID", info.GroupID),
            new SqlParameter("@DateAdd", info.DateAdd),
            new SqlParameter("@DateRegister", info.DateRegister),
            new SqlParameter("@DateRegisterAbs", info.DateRegisterAbs),
            new SqlParameter("@Device", info.Device),
            new SqlParameter("@Memo", info.Memo),
            new SqlParameter("@IsStartWork", info.IsStartWork),
            new SqlParameter("@PositionAddress", info.PositionAddress),
            new SqlParameter("@PositionLog", info.PositionLog),
            new SqlParameter("@PositionLat", info.PositionLat),
            new SqlParameter("@DeviceDetail", info.DeviceDetail),
            
            };

            return parms;
        }
        
        #endregion


        #region Get
        /// <summary>
        /// 获取列表
        /// </summary>
        public List<PunchInInfo> GetList(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            List<PunchInInfo> list = new List<PunchInInfo>();
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

        public List<PunchInInfo> GetList(string StrWhere)
        {
            List<PunchInInfo> list = new List<PunchInInfo>();
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
        public PunchInInfo Get(int id)
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
        public int Add(PunchInInfo info)
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
        public bool Edit(PunchInInfo info)
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

        /// <summary>
        /// 获取Info
        /// </summary>
        /// <param name="id">id</param>
        public PunchInInfo GetLastPunch(int UserID)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("select top 1 ").Append(ALL_PARM).Append(FROM_TABLE);
            strSQL.Append(" where f_UserID = ").Append(UserID).Append(" order by id desc");

            using (SqlDataReader rdr = SqlHelper.ExecuteReader(SqlHelper.SqlconnString, CommandType.Text, strSQL.ToString(), null))
            {
                if (!rdr.Read()) return null;

                return GetByDataReader(rdr);
            }
        }

    }
}