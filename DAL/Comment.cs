using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using DBUtility;
using CSMP.Model;

namespace CSMP.DAL
{
    public class CommentDAL
    {
        private const string ALL_PARM = "  ID,f_DropInUserID,f_SupportUserID,f_CallStepID,f_CallID,f_IsDropInUserDoIt,f_ByMachine,f_AddDate,f_Score,f_Details,f_WorkGroupID,f_Score2,f_Score3 ";
        private const string FROM_TABLE = " from [sys_Comment] ";
        private const string TABLE = " sys_Comment ";
        private const string INSET = " (f_DropInUserID,f_SupportUserID,f_CallStepID,f_CallID,f_IsDropInUserDoIt,f_ByMachine,f_AddDate,f_Score,f_Details,f_WorkGroupID,f_Score2,f_Score3) values(@DropInUserID,@SupportUserID,@CallStepID,@CallID,@IsDropInUserDoIt,@ByMachine,@AddDate,@Score,@Details,@WorkGroupID,@Score2,@Score3)  ";
        private const string UPDATE = " f_DropInUserID=@DropInUserID,f_SupportUserID=@SupportUserID,f_CallStepID=@CallStepID,f_CallID=@CallID,f_IsDropInUserDoIt=@IsDropInUserDoIt,f_ByMachine=@ByMachine,f_AddDate=@AddDate,f_Score=@Score,f_Details=@Details,f_WorkGroupID=@WorkGroupID,f_Score2=@Score2,,f_Score3=@Score3 ";
        

        #region ReadyData
        private CommentInfo GetByDataReader(SqlDataReader rdr)
        {
            CommentInfo info = new CommentInfo();
            info.ID = Convert.ToInt32(rdr["ID"]);
            info.DropInUserID = Convert.ToInt32(rdr["f_DropInUserID"]);
            info.SupportUserID = Convert.ToInt32(rdr["f_SupportUserID"]);
            info.CallStepID = Convert.ToInt32(rdr["f_CallStepID"]);
            info.CallID = Convert.ToInt32(rdr["f_CallID"]);
            info.IsDropInUserDoIt = Convert.ToBoolean(rdr["f_IsDropInUserDoIt"]);
            info.ByMachine = rdr["f_ByMachine"].ToString();
            info.AddDate = Convert.ToDateTime(rdr["f_AddDate"]);
            info.Score = Convert.ToInt32(rdr["f_Score"]);
            info.Details = rdr["f_Details"].ToString();
            info.WorkGroupID = Convert.ToInt32(rdr["f_WorkGroupID"]);
            info.Score2 = Convert.ToInt32(rdr["f_Score2"]);
            info.Score3 = Convert.ToInt32(rdr["f_Score3"]);

            return info;
        }

        private SqlParameter[] GetParameter(CommentInfo info)
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter("@DropInUserID", info.DropInUserID),
            new SqlParameter("@SupportUserID", info.SupportUserID),
            new SqlParameter("@CallStepID", info.CallStepID),
            new SqlParameter("@CallID", info.CallID),
            new SqlParameter("@IsDropInUserDoIt", info.IsDropInUserDoIt),
            new SqlParameter("@ByMachine", info.ByMachine),
            new SqlParameter("@AddDate", info.AddDate),
            new SqlParameter("@Score", info.Score),
            new SqlParameter("@Details", info.Details),
            new SqlParameter("@WorkGroupID", info.WorkGroupID),
            new SqlParameter("@Score2", info.Score2),
            new SqlParameter("@Score3", info.Score3),
            
            };

            return parms;
        }
        
        #endregion


        #region Get
        /// <summary>
        /// 获取列表
        /// </summary>
        public List<CommentInfo> GetList(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            List<CommentInfo> list = new List<CommentInfo>();
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

        public List<CommentInfo> GetList(string StrWhere)
        {
            List<CommentInfo> list = new List<CommentInfo>();
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
        public CommentInfo Get(int id)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("select top 1 ").Append(ALL_PARM).Append(FROM_TABLE).Append(" where id = ").Append(id);

            using (SqlDataReader rdr = SqlHelper.ExecuteReader(SqlHelper.SqlconnString, CommandType.Text, strSQL.ToString(), null))
            {
                if (!rdr.Read()) return null;

                return GetByDataReader(rdr);
            }
        }


        public int GetCountScore(string StrWhere, int ScoreValue, CommentInfo.ScoreType ScType)
        {
            string SQL = "SELECT COUNT(1) " + FROM_TABLE + " where " + StrWhere;
            if (ScoreValue > 0)
            {
                SQL += " and ";
                switch (ScType)
                {
                    case CommentInfo.ScoreType.Total:
                          SQL += " f_Score=";
                      break;
                    case CommentInfo.ScoreType.Score2:
                            SQL += " f_Score2=";
                      break;
                    case CommentInfo.ScoreType.Score3:
                         SQL += " f_Score3=";
                       break;
                    default:
                          SQL += " 0=";
                      break;
                }
                SQL += ScoreValue;
            }
            return (int)SqlHelper.ExecuteScalar(SqlHelper.SqlconnString, CommandType.Text, SQL, null);
        }
        public Int64 GetSum(string StrWhere, int ScoreValue, CommentInfo.ScoreType ScType)
        {
            string ColumnName = string.Empty;
            switch (ScType)
            {
                case CommentInfo.ScoreType.Total:
                    ColumnName = "f_Score";
                    break;
                case CommentInfo.ScoreType.Score2:
                    ColumnName = "f_Score2";
                    break;
                case CommentInfo.ScoreType.Score3:
                    ColumnName = "f_Score3";
                    break;
                default:
                    ColumnName = "0";
                    break;
            }

            string SQL = "SELECT  SUM(CONVERT(bigint, " + ColumnName + ")) " + FROM_TABLE + " where " + StrWhere;
            if (ScoreValue > 0)
            {
                SQL += " and " + ColumnName + "=" + ScoreValue;
            }
            return (Int64)SqlHelper.ExecuteScalar(SqlHelper.SqlconnString, CommandType.Text, SQL, null);
        }

        #endregion


        #region set
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="info">info</param>
        public int Add(CommentInfo info)
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
        public bool Edit(CommentInfo info)
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