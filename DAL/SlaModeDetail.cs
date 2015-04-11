using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using DBUtility;
using CSMP.Model;

namespace CSMP.DAL
{
    public class SlaModeDetailDAL
    {
        private const string ALL_PARM = "  ID,f_SlaModeID,f_DayOfWeek,f_TimerStart,f_TimeEnd ";
        private const string FROM_TABLE = " from [sys_SlaModeDetail] ";
        private const string TABLE = " sys_SlaModeDetail ";
        private const string INSET = " (f_SlaModeID,f_DayOfWeek,f_TimerStart,f_TimeEnd) values(@SlaModeID,@DayOfWeek,@TimerStart,@TimeEnd)  ";
        private const string UPDATE = " f_SlaModeID=@SlaModeID,f_DayOfWeek=@DayOfWeek,f_TimerStart=@TimerStart,f_TimeEnd=@TimeEnd ";

        #region ReadyData
        private SlaModeDetailInfo GetByDataReader(SqlDataReader rdr)
        {
            SlaModeDetailInfo info = new SlaModeDetailInfo();
            info.ID = Convert.ToInt32(rdr["ID"]);
            info.SlaModeID = Convert.ToInt32(rdr["f_SlaModeID"]);
            info.DayOfWeek = rdr["f_DayOfWeek"].ToString();
            info.TimerStart = Convert.ToDateTime(rdr["f_TimerStart"]);
            info.TimeEnd = Convert.ToDateTime(rdr["f_TimeEnd"]);
            
            return info;
        }

        private SqlParameter[] GetParameter(SlaModeDetailInfo info)
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter("@SlaModeID", info.SlaModeID),
            new SqlParameter("@DayOfWeek", info.DayOfWeek),
            new SqlParameter("@TimerStart", info.TimerStart),
            new SqlParameter("@TimeEnd", info.TimeEnd),
            
            };

            return parms;
        }
        
        #endregion


        #region Get
        /// <summary>
        /// 获取列表
        /// </summary>
        public List<SlaModeDetailInfo> GetList(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            List<SlaModeDetailInfo> list = new List<SlaModeDetailInfo>();
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

        public List<SlaModeDetailInfo> GetList(string StrWhere)
        {
            List<SlaModeDetailInfo> list = new List<SlaModeDetailInfo>();
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
        public SlaModeDetailInfo Get(int id)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("select top 1 ").Append(ALL_PARM).Append(FROM_TABLE).Append(" where id = ").Append(id);

            using (SqlDataReader rdr = SqlHelper.ExecuteReader(SqlHelper.SqlconnString, CommandType.Text, strSQL.ToString(), null))
            {
                if (!rdr.Read()) return null;

                return GetByDataReader(rdr);
            }
        }

        /// <summary>
        /// 获取Info
        /// </summary>
        /// <param name="id">id</param>
        public SlaModeDetailInfo GetNext(int SlaModeID,int MoreThanID)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("select top 1 ").Append(ALL_PARM).Append(FROM_TABLE);
            strSQL.Append(" where f_SlaModeID = ").Append(SlaModeID);
            strSQL.Append(" and ID>").Append(MoreThanID).Append(" order by id asc ");

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
        public int Add(SlaModeDetailInfo info)
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
        public bool Edit(SlaModeDetailInfo info)
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