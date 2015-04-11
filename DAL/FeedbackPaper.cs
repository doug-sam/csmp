using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using DBUtility;
using CSMP.Model;

namespace CSMP.DAL
{
    public class FeedbackPaperDAL
    {
        private const string ALL_PARM = "  ID,f_Name,f_OrderNumber,f_Enable,f_Memo ";
        private const string FROM_TABLE = " from [sys_FeedbackPaper] ";
        private const string TABLE = " sys_FeedbackPaper ";
        private const string INSET = " (f_Name,f_OrderNumber,f_Enable,f_Memo) values(@Name,@OrderNumber,@Enable,@Memo)  ";
        private const string UPDATE = " f_Name=@Name,f_OrderNumber=@OrderNumber,f_Enable=@Enable,f_Memo=@Memo ";

        #region ReadyData
        private FeedbackPaperInfo GetByDataReader(SqlDataReader rdr)
        {
            FeedbackPaperInfo info = new FeedbackPaperInfo();
            info.ID = Convert.ToInt32(rdr["ID"]);
            info.Name = rdr["f_Name"].ToString();
            info.OrderNumber = Convert.ToInt32(rdr["f_OrderNumber"]);
            info.Enable = Convert.ToBoolean(rdr["f_Enable"]);
            info.Memo = rdr["f_Memo"].ToString();
            
            return info;
        }

        private SqlParameter[] GetParameter(FeedbackPaperInfo info)
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter("@Name", info.Name),
            new SqlParameter("@OrderNumber", info.OrderNumber),
            new SqlParameter("@Enable", info.Enable),
            new SqlParameter("@Memo", info.Memo),
            
            };

            return parms;
        }
        
        #endregion


        #region Get
        /// <summary>
        /// 获取列表
        /// </summary>
        public List<FeedbackPaperInfo> GetList(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            List<FeedbackPaperInfo> list = new List<FeedbackPaperInfo>();
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

        public List<FeedbackPaperInfo> GetList(string StrWhere)
        {
            List<FeedbackPaperInfo> list = new List<FeedbackPaperInfo>();
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
        public FeedbackPaperInfo Get(int id)
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
        public int Add(FeedbackPaperInfo info)
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
        public bool Edit(FeedbackPaperInfo info)
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