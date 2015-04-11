using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using DBUtility;
using CSMP.Model;

namespace CSMP.DAL
{
    public class CustomerRequestDAL
    {
        private const string ALL_PARM = "  ID,f_UserID,f_UserName,f_CallID,f_AddDate,f_ErrorReportDate,f_BrandID,f_StoreID,f_StoreNo,f_StoreName,f_ErrorReportUserID,f_ErrorReportUserName,f_Details,f_Enable ";
        private const string FROM_TABLE = " from [sys_CustomerRequest] ";
        private const string TABLE = " sys_CustomerRequest ";
        private const string INSET = " (f_UserID,f_UserName,f_CallID,f_AddDate,f_ErrorReportDate,f_BrandID,f_StoreID,f_StoreNo,f_StoreName,f_ErrorReportUserID,f_ErrorReportUserName,f_Details,f_Enable) values(@UserID,@UserName,@CallID,@AddDate,@ErrorReportDate,@BrandID,@StoreID,@StoreNo,@StoreName,@ErrorReportUserID,@ErrorReportUserName,@Details,@Enable)  ";
        private const string UPDATE = " f_UserID=@UserID,f_UserName=@UserName,f_CallID=@CallID,f_AddDate=@AddDate,f_ErrorReportDate=@ErrorReportDate,f_BrandID=@BrandID,f_StoreID=@StoreID,f_StoreNo=@StoreNo,f_StoreName=@StoreName,f_ErrorReportUserID=@ErrorReportUserID,f_ErrorReportUserName=@ErrorReportUserName,f_Details=@Details,f_Enable=@Enable ";

        #region ReadyData
        private CustomerRequestInfo GetByDataReader(SqlDataReader rdr)
        {
            CustomerRequestInfo info = new CustomerRequestInfo();
            info.ID = Convert.ToInt32(rdr["ID"]);
            info.UserID = Convert.ToInt32(rdr["f_UserID"]);
            info.UserName = rdr["f_UserName"].ToString();
            info.CallID = Convert.ToInt32(rdr["f_CallID"]);
            info.AddDate = Convert.ToDateTime(rdr["f_AddDate"]);
            info.ErrorReportDate = Convert.ToDateTime(rdr["f_ErrorReportDate"]);
            info.BrandID = Convert.ToInt32(rdr["f_BrandID"]);
            info.StoreID = Convert.ToInt32(rdr["f_StoreID"]);
            info.StoreNo = rdr["f_StoreNo"].ToString();
            info.StoreName = rdr["f_StoreName"].ToString();
            info.ErrorReportUserID = Convert.ToInt32(rdr["f_ErrorReportUserID"]);
            info.ErrorReportUserName = rdr["f_ErrorReportUserName"].ToString();
            info.Details = rdr["f_Details"].ToString();
            info.Enable = Convert.ToBoolean(rdr["f_Enable"]);

            return info;
        }

        private SqlParameter[] GetParameter(CustomerRequestInfo info)
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter("@UserID", info.UserID),
            new SqlParameter("@UserName", info.UserName),
            new SqlParameter("@CallID", info.CallID),
            new SqlParameter("@AddDate", info.AddDate),
            new SqlParameter("@ErrorReportDate", info.ErrorReportDate),
            new SqlParameter("@BrandID", info.BrandID),
            new SqlParameter("@StoreID", info.StoreID),
            new SqlParameter("@StoreNo", info.StoreNo),
            new SqlParameter("@StoreName", info.StoreName),
            new SqlParameter("@ErrorReportUserID", info.ErrorReportUserID),
            new SqlParameter("@ErrorReportUserName", info.ErrorReportUserName),
            new SqlParameter("@Details", info.Details),
            new SqlParameter("@Enable", info.Enable),
            
            };

            return parms;
        }

        #endregion


        #region Get
        /// <summary>
        /// 获取列表
        /// </summary>
        public List<CustomerRequestInfo> GetList(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            List<CustomerRequestInfo> list = new List<CustomerRequestInfo>();
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

        public List<CustomerRequestInfo> GetList(string StrWhere)
        {
            List<CustomerRequestInfo> list = new List<CustomerRequestInfo>();
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
        public CustomerRequestInfo Get(int id)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("select top 1 ").Append(ALL_PARM).Append(FROM_TABLE).Append(" where id = ").Append(id);

            using (SqlDataReader rdr = SqlHelper.ExecuteReader(SqlHelper.SqlconnString, CommandType.Text, strSQL.ToString(), null))
            {
                if (!rdr.Read()) return null;

                return GetByDataReader(rdr);
            }
        }


        public int GetConut(int WorkGroupID)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("select COUNT(1) ").Append(FROM_TABLE).Append(" where f_Enable=1 AND f_CallID=0 and f_BrandID in( ");
            strSQL.Append("     SELECT f_MID FROM sys_WorkGroupBrand WHERE f_WorkGroupID=").Append(WorkGroupID);
            strSQL.Append(")");

           object obj= SqlHelper.ExecuteScalar(SqlHelper.SqlconnString,CommandType.Text,strSQL.ToString(),null);
            if (obj!=null&&obj!=DBNull.Value)
	        {
		         return Convert.ToInt32(obj);
	        }
            return 0;
        }

        #endregion


        #region set
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="info">info</param>
        public int Add(CustomerRequestInfo info)
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
        public bool Edit(CustomerRequestInfo info)
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