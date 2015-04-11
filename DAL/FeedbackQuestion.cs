using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using DBUtility;
using CSMP.Model;

namespace CSMP.DAL
{
    public class FeedbackQuestionDAL
    {
        private const string ALL_PARM = "  ID,f_PaperID,f_Name,f_Memo,f_Type,f_OrderNumber,f_Enable ";
        private const string FROM_TABLE = " from [sys_FeedbackQuestion] ";
        private const string TABLE = " sys_FeedbackQuestion ";
        private const string INSET = " (f_PaperID,f_Name,f_Memo,f_Type,f_OrderNumber,f_Enable) values(@PaperID,@Name,@Memo,@Type,@OrderNumber,@Enable)  ";
        private const string UPDATE = " f_PaperID=@PaperID,f_Name=@Name,f_Memo=@Memo,f_Type=@Type,f_OrderNumber=@OrderNumber,f_Enable=@Enable ";

        #region ReadyData
        private FeedbackQuestionInfo GetByDataReader(SqlDataReader rdr)
        {
            FeedbackQuestionInfo info = new FeedbackQuestionInfo();
            info.ID = Convert.ToInt32(rdr["ID"]);
            info.PaperID = Convert.ToInt32(rdr["f_PaperID"]);
            info.Name = rdr["f_Name"].ToString();
            info.Memo = rdr["f_Memo"].ToString();
            info.Type = Convert.ToInt32(rdr["f_Type"]);
            info.OrderNumber = Convert.ToInt32(rdr["f_OrderNumber"]);
            info.Enable = Convert.ToBoolean(rdr["f_Enable"]);
            
            return info;
        }

        private SqlParameter[] GetParameter(FeedbackQuestionInfo info)
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter("@PaperID", info.PaperID),
            new SqlParameter("@Name", info.Name),
            new SqlParameter("@Memo", info.Memo),
            new SqlParameter("@Type", info.Type),
            new SqlParameter("@OrderNumber", info.OrderNumber),
            new SqlParameter("@Enable", info.Enable),
            
            };

            return parms;
        }
        
        #endregion


        #region Get
        /// <summary>
        /// 获取列表
        /// </summary>
        public List<FeedbackQuestionInfo> GetList(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            List<FeedbackQuestionInfo> list = new List<FeedbackQuestionInfo>();
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

        public List<FeedbackQuestionInfo> GetList(string StrWhere)
        {
            List<FeedbackQuestionInfo> list = new List<FeedbackQuestionInfo>();
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
        public FeedbackQuestionInfo Get(int id)
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
        public int Add(FeedbackQuestionInfo info)
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
        public bool Edit(FeedbackQuestionInfo info)
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