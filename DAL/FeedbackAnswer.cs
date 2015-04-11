using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using DBUtility;
using CSMP.Model;

namespace CSMP.DAL
{
    public class FeedbackAnswerDAL
    {
        private const string ALL_PARM = "  ID,f_PaperID,f_QuestionID,f_Answer,f_Answer2,f_RecorderID,f_RecorderName,f_FeedbackUserName,f_CallID,f_CallStepID,f_AddDate,f_QuestionName ";
        private const string FROM_TABLE = " from [sys_FeedbackAnswer] ";
        private const string TABLE = " sys_FeedbackAnswer ";
        private const string INSET = " (f_PaperID,f_QuestionID,f_Answer,f_Answer2,f_RecorderID,f_RecorderName,f_FeedbackUserName,f_CallID,f_CallStepID,f_AddDate,f_QuestionName) values(@PaperID,@QuestionID,@Answer,@Answer2,@RecorderID,@RecorderName,@FeedbackUserName,@CallID,@CallStepID,@AddDate,@QuestionName)  ";
        private const string UPDATE = " f_PaperID=@PaperID,f_QuestionID=@QuestionID,f_Answer=@Answer,f_Answer2=@Answer2,f_RecorderID=@RecorderID,f_RecorderName=@RecorderName,f_FeedbackUserName=@FeedbackUserName,f_CallID=@CallID,f_CallStepID=@CallStepID,f_AddDate=@AddDate,f_QuestionName=@QuestionName ";

        #region ReadyData
        private FeedbackAnswerInfo GetByDataReader(SqlDataReader rdr)
        {
            FeedbackAnswerInfo info = new FeedbackAnswerInfo();
            info.ID = Convert.ToInt32(rdr["ID"]);
            info.PaperID = Convert.ToInt32(rdr["f_PaperID"]);
            info.QuestionID = Convert.ToInt32(rdr["f_QuestionID"]);
            info.Answer = rdr["f_Answer"].ToString();
            info.Answer2 = Convert.ToInt32(rdr["f_Answer2"]);
            info.RecorderID = Convert.ToInt32(rdr["f_RecorderID"]);
            info.RecorderName = rdr["f_RecorderName"].ToString();
            info.FeedbackUserName = rdr["f_FeedbackUserName"].ToString();
            info.CallID = Convert.ToInt32(rdr["f_CallID"]);
            info.CallStepID = Convert.ToInt32(rdr["f_CallStepID"]);
            info.AddDate = Convert.ToDateTime(rdr["f_AddDate"]);
            info.QuestionName = rdr["f_QuestionName"].ToString();
            return info;
        }

        private SqlParameter[] GetParameter(FeedbackAnswerInfo info)
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter("@PaperID", info.PaperID),
            new SqlParameter("@QuestionID", info.QuestionID),
            new SqlParameter("@Answer", info.Answer),
            new SqlParameter("@Answer2", info.Answer2),
            new SqlParameter("@RecorderID", info.RecorderID),
            new SqlParameter("@RecorderName", info.RecorderName),
            new SqlParameter("@FeedbackUserName", info.FeedbackUserName),
            new SqlParameter("@CallID", info.CallID),
            new SqlParameter("@CallStepID", info.CallStepID),
            new SqlParameter("@AddDate", info.AddDate),
            new SqlParameter("@QuestionName", info.QuestionName),
            
            };

            return parms;
        }
        
        #endregion


        #region Get
        /// <summary>
        /// 获取列表
        /// </summary>
        public List<FeedbackAnswerInfo> GetList(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            List<FeedbackAnswerInfo> list = new List<FeedbackAnswerInfo>();
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

        public List<FeedbackAnswerInfo> GetList(string StrWhere)
        {
            List<FeedbackAnswerInfo> list = new List<FeedbackAnswerInfo>();
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
        public FeedbackAnswerInfo Get(int id)
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
        /// <param name="QuestionID"></param>
        /// <param name="CallID"></param>
        /// <returns></returns>
        public FeedbackAnswerInfo Get(int QuestionID,int CallID)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("select top 1 ").Append(ALL_PARM).Append(FROM_TABLE);
            strSQL.Append(" where f_QuestionID=").Append(QuestionID).Append(" AND f_CallID=").Append(CallID);

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
        public int Add(FeedbackAnswerInfo info)
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
        public bool Edit(FeedbackAnswerInfo info)
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
        public bool DeleteByCallID(int CallID)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("delete ").Append(FROM_TABLE).Append(" where f_CallID = ").Append(CallID);

            return SqlHelper.ExecuteNonQueryByTran(SqlHelper.SqlconnString, CommandType.Text, strSQL.ToString(), null);
        }

        
        #endregion
    }
}