using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using DBUtility;
using CSMP.Model;

namespace CSMP.DAL
{
    public class MarqueeMessageReportDAL
    {
        private const string ALL_PARM = "  ID,f_No,f_MaintainUserID,f_MaintainUserName,f_MajorUserID,f_MajorUserName,f_ActionTime,f_Content ";
        public const string FROM_TABLE = " from [sys_MarqueeMessageReport] ";
        public const string TABLE = " sys_MarqueeMessageReport ";
        private const string INSET = " (f_No,f_MaintainUserID,f_MaintainUserName,f_MajorUserID,f_MajorUserName,f_ActionTime,f_Content) values (@No,@MaintainUserID,@MaintainUserName,@MajorUserID,@MajorUserName,@ActionTime,@Content)  ";
        private const string UPDATE = " f_No=@No,f_MaintainUserID=@MaintainUserID,f_MaintainUserName=@MaintainUserName,f_MajorUserID=@MajorUserID,f_MajorUserName=@MajorUserName,f_ActionTime=@ActionTime,f_Content=@Content ";

        #region ReadyData
        private MarqueeMessageReport GetByDataReader(SqlDataReader rdr)
        {
            MarqueeMessageReport info = new MarqueeMessageReport();
            info.ID = Convert.ToInt32(rdr["ID"]);
            info.No = rdr["f_No"].ToString();
            info.MaintainUserID = Convert.ToInt32(rdr["f_MaintainUserID"]);
            info.MaintainUserName = rdr["f_MaintainUserName"].ToString();
            info.MajorUserID = Convert.ToInt32(rdr["f_MajorUserID"]);
            info.MajorUserName = rdr["f_MajorUserName"].ToString();
            info.ActionTime =Convert.ToDateTime(rdr["f_ActionTime"]);
            info.Content = rdr["f_Content"].ToString();

            return info;
        }

        private SqlParameter[] GetParameter(MarqueeMessageReport info)
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter("@No", info.No),
                new SqlParameter("@MaintainUserID", info.MaintainUserID),
            new SqlParameter("@MaintainUserName", info.MaintainUserName),
            new SqlParameter("@MajorUserID", info.MajorUserID),
            new SqlParameter("@MajorUserName", info.MajorUserName),
            new SqlParameter("@ActionTime", info.ActionTime),
            new SqlParameter("@Content", info.Content),
                      
            };

            return parms;
        }
        #endregion
        /// <summary>
        /// 获取列表
        /// </summary>
        public List<MarqueeMessageReport> GetList(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            List<MarqueeMessageReport> list = new List<MarqueeMessageReport>();
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
        /// <summary>
        /// 统计APP使用情况
        /// </summary>
        /// <param name="strSQL"></param>
        /// <returns></returns>
        public DataTable GetDailyReport(string strSQL)
        {
            return SqlHelper.ExecuteReader(strSQL, "0");
        }
        /// <summary>
        /// 统计APP使用情况，使用SP
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="majorUserID"></param>
        /// <returns></returns>
        public DataTable GetDailyReportBySP(string startTime,string endTime, string majorUserName,int provinceID,int workgroupID,int majorUserID)
        {
            StoreProcedure sp = new StoreProcedure("sp_APP_MarqueeMessageDailyReport");//类的对象
            Object[] paraValues = new object[7];
            paraValues[0] = startTime;
            paraValues[1] = endTime;
            paraValues[2] = majorUserName;
            paraValues[3] = provinceID;
            paraValues[4] = workgroupID;
            paraValues[5] = majorUserID;
            paraValues[6] = "";
            object[] output = new object[1];
            DataTable dt = new DataTable();
            dt = sp.ExecuteDataTable(out output, paraValues);
            
            return dt;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="StrWhere"></param>
        /// <returns></returns>
        public List<MarqueeMessageReport> GetList(string StrWhere)
        {
            List<MarqueeMessageReport> list = new List<MarqueeMessageReport>();
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("select ").Append(ALL_PARM).Append(FROM_TABLE);
            strSQL.Append(" where ").Append(" 1=1 ").Append(StrWhere);
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
        /// 根据二线负责人ID
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public List<MarqueeMessageReport> GetByMaintaimUserID(int MaintaimUserID)
        {
            string sqlWhere = " AND f_MaintainUserID =" + MaintaimUserID;
            return GetList(sqlWhere);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="info">info</param>
        public int Add(MarqueeMessageReport info)
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
        public bool Edit(MarqueeMessageReport info)
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
        /// <param name="id"></param>
        public bool Delete(string No)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("delete ").Append(FROM_TABLE).Append(" where f_No = '").Append(No).Append("'");

            return SqlHelper.ExecuteNonQueryByTran(SqlHelper.SqlconnString, CommandType.Text, strSQL.ToString(), null);
        }
    }
}
