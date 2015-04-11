using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using DBUtility;
using CSMP.Model;

namespace CSMP.DAL
{
    public class TaskDAL
    {
        private const string ALL_PARM = "  ID,f_Name,f_IntervalTime,f_ExcuteTime,f_ExcuteTimeLast,f_CycleMode,f_URL,f_Enable ";
        private const string FROM_TABLE = " from [sys_Task] ";
        private const string TABLE = " sys_Task ";
        private const string INSET = " (f_Name,f_IntervalTime,f_ExcuteTime,f_ExcuteTimeLast,f_CycleMode,f_URL,f_Enable) values(@Name,@IntervalTime,@ExcuteTime,@ExcuteTimeLast,@CycleMode,@URL,@Enable)  ";
        private const string UPDATE = " f_Name=@Name,f_IntervalTime=@IntervalTime,f_ExcuteTime=@ExcuteTime,f_ExcuteTimeLast=@ExcuteTimeLast,f_CycleMode=@CycleMode,f_URL=@URL,f_Enable=@Enable ";

        #region ReadyData
        private TaskInfo GetByDataReader(SqlDataReader rdr)
        {
            TaskInfo info = new TaskInfo();
            info.ID = Convert.ToInt32(rdr["ID"]);
            info.Name = rdr["f_Name"].ToString();
            info.IntervalTime = Convert.ToDecimal(Convert.ToDecimal(rdr["f_IntervalTime"]).ToString("G0"));
            info.ExcuteTime = Convert.ToDateTime(rdr["f_ExcuteTime"]);
            info.ExcuteTimeLast = Convert.ToDateTime(rdr["f_ExcuteTimeLast"]);
            info.CycleMode = rdr["f_CycleMode"].ToString().Trim();
            info.URL = rdr["f_URL"].ToString();
            info.Enable = Convert.ToBoolean(rdr["f_Enable"]);
            
            return info;
        }

        private SqlParameter[] GetParameter(TaskInfo info)
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter("@Name", info.Name),
            new SqlParameter("@IntervalTime", info.IntervalTime),
            new SqlParameter("@ExcuteTime", info.ExcuteTime),
            new SqlParameter("@ExcuteTimeLast", info.ExcuteTimeLast),
            new SqlParameter("@CycleMode", info.CycleMode),
            new SqlParameter("@URL", info.URL),
            new SqlParameter("@Enable", info.Enable),
            
            };

            return parms;
        }
        
        #endregion


        #region Get
        /// <summary>
        /// 获取列表
        /// </summary>
        public List<TaskInfo> GetList(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            List<TaskInfo> list = new List<TaskInfo>();
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

        public List<TaskInfo> GetList(string StrWhere)
        {
            List<TaskInfo> list = new List<TaskInfo>();
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
        public TaskInfo Get(int id)
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
        public int Add(TaskInfo info)
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
        public bool Edit(TaskInfo info)
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