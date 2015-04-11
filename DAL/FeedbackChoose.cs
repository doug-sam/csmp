using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using DBUtility;
using CSMP.Model;

namespace CSMP.DAL
{
    public class FeedbackChooseDAL
    {
        private const string ALL_PARM = "  ID,f_Name,f_QuestionID,f_AddDate,f_IsDefault,f_OrderNumber,f_Enable ";
        private const string FROM_TABLE = " from [sys_FeedbackChoose] ";
        private const string TABLE = " sys_FeedbackChoose ";
        private const string INSET = " (f_Name,f_QuestionID,f_AddDate,f_IsDefault,f_OrderNumber,f_Enable) values(@Name,@QuestionID,@AddDate,@IsDefault,@OrderNumber,@Enable)  ";
        private const string UPDATE = " f_Name=@Name,f_QuestionID=@QuestionID,f_AddDate=@AddDate,f_IsDefault=@IsDefault,f_OrderNumber=@OrderNumber,f_Enable=@Enable ";

        #region ReadyData
        private FeedbackChooseInfo GetByDataReader(SqlDataReader rdr)
        {
            FeedbackChooseInfo info = new FeedbackChooseInfo();
            info.ID = Convert.ToInt32(rdr["ID"]);
            info.Name = rdr["f_Name"].ToString();
            info.QuestionID = Convert.ToInt32(rdr["f_QuestionID"]);
            info.AddDate = Convert.ToDateTime(rdr["f_AddDate"]);
            info.IsDefault = Convert.ToBoolean(rdr["f_IsDefault"]);
            info.OrderNumber = Convert.ToInt32(rdr["f_OrderNumber"]);
            info.Enable = Convert.ToBoolean(rdr["f_Enable"]);
            
            return info;
        }

        private SqlParameter[] GetParameter(FeedbackChooseInfo info)
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter("@Name", info.Name),
            new SqlParameter("@QuestionID", info.QuestionID),
            new SqlParameter("@AddDate", info.AddDate),
            new SqlParameter("@IsDefault", info.IsDefault),
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
        public List<FeedbackChooseInfo> GetList(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            List<FeedbackChooseInfo> list = new List<FeedbackChooseInfo>();
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

        public List<FeedbackChooseInfo> GetList(string StrWhere)
        {
            List<FeedbackChooseInfo> list = new List<FeedbackChooseInfo>();
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
        public FeedbackChooseInfo Get(int id)
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
        public int Add(FeedbackChooseInfo info)
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
        public bool Edit(FeedbackChooseInfo info)
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