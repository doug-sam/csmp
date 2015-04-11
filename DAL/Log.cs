using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using DBUtility;
using CSMP.Model;

namespace CSMP.DAL
{
    public class LogDAL
    {
        private const string ALL_PARM = "  ID,f_Category,f_UserName,f_Content,f_AddDate,f_ErrorDate,f_Serious,f_SendEmail ";
        private const string FROM_TABLE = " from [sys_Log] ";
        private const string TABLE = " sys_Log ";
        private const string INSET = " (f_Category,f_UserName,f_Content,f_AddDate,f_ErrorDate,f_Serious,f_SendEmail) values(@Category,@UserName,@Content,@AddDate,@ErrorDate,@Serious,@SendEmail)  ";
        private const string UPDATE = " f_Category=@Category,f_UserName=@UserName,f_Content=@Content,f_AddDate=@AddDate,f_ErrorDate=@ErrorDate,f_Serious=@Serious,f_SendEmail=@SendEmail ";

        #region ReadyData
        private LogInfo GetByDataReader(SqlDataReader rdr)
        {
            LogInfo info = new LogInfo();
            info.ID = Convert.ToInt32(rdr["ID"]);
            info.Category = rdr["f_Category"].ToString();
            info.UserName = rdr["f_UserName"].ToString();
            info.Content = rdr["f_Content"].ToString();
            info.AddDate = Convert.ToDateTime(rdr["f_AddDate"]);
            info.ErrorDate = Convert.ToDateTime(rdr["f_ErrorDate"]);
            info.Serious = Convert.ToInt32(rdr["f_Serious"]);
            info.SendEmail = Convert.ToBoolean(rdr["f_SendEmail"]);
            
            return info;
        }

        private SqlParameter[] GetParameter(LogInfo info)
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter("@Category", info.Category),
            new SqlParameter("@UserName", info.UserName),
            new SqlParameter("@Content", info.Content),
            new SqlParameter("@AddDate", info.AddDate),
            new SqlParameter("@ErrorDate", info.ErrorDate),
            new SqlParameter("@Serious", info.Serious),
            new SqlParameter("@SendEmail", info.SendEmail),
            
            };

            return parms;
        }
        
        #endregion


        #region Get
        /// <summary>
        /// 获取列表
        /// </summary>
        public List<LogInfo> GetList(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            List<LogInfo> list = new List<LogInfo>();
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

        public List<LogInfo> GetList(string StrWhere)
        {
            List<LogInfo> list = new List<LogInfo>();
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
        public LogInfo Get(int id)
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
        public int Add(LogInfo info)
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
        public bool Edit(LogInfo info)
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