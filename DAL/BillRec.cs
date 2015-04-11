using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using DBUtility;
using CSMP.Model;

namespace CSMP.DAL
{
    public class BillRecDAL
    {
        private const string ALL_PARM = "  ID,f_StoreID,f_CallID,f_CallStepID,f_CallNo,f_Url,f_AddDate,f_Pwd,f_Confirm,f_CreateBy,f_Flag ";
        private const string FROM_TABLE = " from [sys_BillRec] ";
        private const string TABLE = " sys_BillRec ";
        private const string INSET = " (f_StoreID,f_CallID,f_CallStepID,f_CallNo,f_Url,f_AddDate,f_Pwd,f_Confirm,f_CreateBy,f_Flag) values(@StoreID,@CallID,@CallStepID,@CallNo,@Url,@AddDate,@Pwd,@Confirm,@CreateBy,@Flag)  ";
        private const string UPDATE = " f_StoreID=@StoreID,f_CallID=@CallID,f_CallStepID=@CallStepID,f_CallNo=@CallNo,f_Url=@Url,f_AddDate=@AddDate,f_Pwd=@Pwd,f_Confirm=@Confirm,f_CreateBy=@CreateBy,f_Flag=@Flag ";

        #region ReadyData
        private BillRecInfo GetByDataReader(SqlDataReader rdr)
        {
            BillRecInfo info = new BillRecInfo();
            info.ID = Convert.ToInt32(rdr["ID"]);
            info.StoreID = Convert.ToInt32(rdr["f_StoreID"]);
            info.CallID = Convert.ToInt32(rdr["f_CallID"]);
            info.CallStepID = Convert.ToInt32(rdr["f_CallStepID"]);
            info.CallNo = rdr["f_CallNo"].ToString();
            info.Url = rdr["f_Url"].ToString();
            info.AddDate = Convert.ToDateTime(rdr["f_AddDate"]);
            info.Pwd = rdr["f_Pwd"].ToString();
            info.Confirm = Convert.ToBoolean(rdr["f_Confirm"]);
            info.CreateBy = rdr["f_CreateBy"].ToString();
            info.Flag = Convert.ToInt32(rdr["f_Flag"]);
            
            return info;
        }

        private SqlParameter[] GetParameter(BillRecInfo info)
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter("@StoreID", info.StoreID),
            new SqlParameter("@CallID", info.CallID),
            new SqlParameter("@CallStepID", info.CallStepID),
            new SqlParameter("@CallNo", info.CallNo),
            new SqlParameter("@Url", info.Url),
            new SqlParameter("@AddDate", info.AddDate),
            new SqlParameter("@Pwd", info.Pwd),
            new SqlParameter("@Confirm", info.Confirm),
            new SqlParameter("@CreateBy", info.CreateBy),
            new SqlParameter("@Flag", info.Flag),
            
            };

            return parms;
        }
        
        #endregion


        #region Get
        /// <summary>
        /// 获取列表
        /// </summary>
        public List<BillRecInfo> GetList(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            List<BillRecInfo> list = new List<BillRecInfo>();
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

        public List<BillRecInfo> GetList(string StrWhere)
        {
            List<BillRecInfo> list = new List<BillRecInfo>();
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
        public BillRecInfo Get(int id)
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
        /// <param name="id">id</param>
        public BillRecInfo Get(string CallNo,string Pwd)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("select top 1 ").Append(ALL_PARM).Append(FROM_TABLE);
            strSQL.Append(" where f_CallNo = ").Append(CallNo).Append(" and f_Pwd='").Append(Pwd).Append("'");

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
        public int Add(BillRecInfo info)
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
        public bool Edit(BillRecInfo info)
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
        public bool DeleteByCallID(int id)
        {
            StringBuilder strSQL = DeleteByCallIDsql(id.ToString());
            return SqlHelper.ExecuteNonQueryByTran(SqlHelper.SqlconnString, CommandType.Text, strSQL.ToString(), null);
        }

        public static StringBuilder DeleteByCallIDsql(string IDOrSelectIDSQL)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("delete ").Append(FROM_TABLE).Append(" where f_CallID in (").Append(IDOrSelectIDSQL).Append(") ");

            return strSQL;
        }


        
        #endregion
    }
}