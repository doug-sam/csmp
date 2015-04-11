using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using DBUtility;
using CSMP.Model;

namespace CSMP.DAL
{
    public class JobcodeDAL
    {
        private const string ALL_PARM = "  ID,f_WorkGroupID,f_CodeNo,f_Money,f_TimeAction,f_TimeArrive ";
        private const string FROM_TABLE = " from [sys_Jobcode] ";
        private const string TABLE = " sys_Jobcode ";
        private const string INSET = " (f_WorkGroupID,f_CodeNo,f_Money,f_TimeAction,f_TimeArrive) values(@WorkGroupID,@CodeNo,@Money,@TimeAction,@TimeArrive)  ";
        private const string UPDATE = " f_WorkGroupID=@WorkGroupID,f_CodeNo=@CodeNo,f_Money=@Money,f_TimeAction=@TimeAction,f_TimeArrive=@TimeArrive ";

        #region ReadyData
        private JobcodeInfo GetByDataReader(SqlDataReader rdr)
        {
            JobcodeInfo info = new JobcodeInfo();
            info.ID = Convert.ToInt32(rdr["ID"]);
            info.WorkGroupID = Convert.ToInt32(rdr["f_WorkGroupID"]);
            info.CodeNo = rdr["f_CodeNo"].ToString().Trim();
            info.Money = Convert.ToDecimal(Convert.ToDecimal(rdr["f_Money"]).ToString("G0"));
            info.TimeAction = rdr["f_TimeAction"].ToString();
            info.TimeArrive = rdr["f_TimeArrive"].ToString();
            
            return info;
        }

        private SqlParameter[] GetParameter(JobcodeInfo info)
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter("@WorkGroupID", info.WorkGroupID),
            new SqlParameter("@CodeNo", info.CodeNo),
            new SqlParameter("@Money", info.Money),
            new SqlParameter("@TimeAction", info.TimeAction),
            new SqlParameter("@TimeArrive", info.TimeArrive),
            
            };

            return parms;
        }
        
        #endregion


        #region Get
        /// <summary>
        /// 获取列表
        /// </summary>
        public List<JobcodeInfo> GetList(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            List<JobcodeInfo> list = new List<JobcodeInfo>();
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

        public List<JobcodeInfo> GetList(string StrWhere)
        {
            List<JobcodeInfo> list = new List<JobcodeInfo>();
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
        public JobcodeInfo Get(int id)
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
        /// <param name="CodeNo">CodeNo</param>
        public JobcodeInfo Get(string CodeNo,int WorkGroupID)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("select top 1 ").Append(ALL_PARM).Append(FROM_TABLE).Append(" where f_CodeNo =@CodeNo and  f_WorkGroupID=@WorkGroupID");
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter("@CodeNo", CodeNo),
                new SqlParameter("@WorkGroupID", WorkGroupID),
            };
            using (SqlDataReader rdr = SqlHelper.ExecuteReader(SqlHelper.SqlconnString, CommandType.Text, strSQL.ToString(), parms))
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
        public int Add(JobcodeInfo info)
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
        public bool Edit(JobcodeInfo info)
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