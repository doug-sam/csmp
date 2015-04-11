using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using DBUtility;
using CSMP.Model;
using Tool;

namespace CSMP.DAL
{
    public class DropInMemoDAL
    {
        private const string ALL_PARM = "  ID,f_StepID,f_AddDate,f_Details,f_UserID,f_UserName,f_OrderNumber,f_Enable,f_MemoDate,f_TypeName ";
        private const string FROM_TABLE = " from [sys_DropInMemo] ";
        private const string TABLE = " sys_DropInMemo ";
        private const string INSET = " (f_StepID,f_AddDate,f_Details,f_UserID,f_UserName,f_OrderNumber,f_Enable,f_MemoDate,f_TypeName) values(@StepID,@AddDate,@Details,@UserID,@UserName,@OrderNumber,@Enable,@MemoDate,@TypeName)  ";
        private const string UPDATE = " f_StepID=@StepID,f_AddDate=@AddDate,f_Details=@Details,f_UserID=@UserID,f_UserName=@UserName,f_OrderNumber=@OrderNumber,f_Enable=@Enable,f_MemoDate=@MemoDate,f_TypeName=@TypeName ";

        #region ReadyData
        private DropInMemoInfo GetByDataReader(SqlDataReader rdr)
        {
            DropInMemoInfo info = new DropInMemoInfo();
            info.ID = Convert.ToInt32(rdr["ID"]);
            info.StepID = Convert.ToInt32(rdr["f_StepID"]);
            info.AddDate = Convert.ToDateTime(rdr["f_AddDate"]);
            info.Details = rdr["f_Details"].ToString();
            info.UserID = Convert.ToInt32(rdr["f_UserID"]);
            info.UserName = rdr["f_UserName"].ToString();
            info.OrderNumber = Convert.ToInt32(rdr["f_OrderNumber"]);
            info.Enable = Convert.ToBoolean(rdr["f_Enable"]);
            info.MemoDate = Convert.ToDateTime(rdr["f_MemoDate"]);
            info.TypeName = rdr["f_TypeName"].ToString();
            return info;
        }

        private SqlParameter[] GetParameter(DropInMemoInfo info)
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter("@StepID", info.StepID),
            new SqlParameter("@AddDate", info.AddDate),
            new SqlParameter("@Details", info.Details),
            new SqlParameter("@UserID", info.UserID),
            new SqlParameter("@UserName", info.UserName),
            new SqlParameter("@OrderNumber", info.OrderNumber),
            new SqlParameter("@Enable", info.Enable),
            new SqlParameter("@MemoDate", info.MemoDate),
            new SqlParameter("@TypeName", info.TypeName),
            
            };

            return parms;
        }
        
        #endregion


        #region Get
        /// <summary>
        /// 获取列表
        /// </summary>
        public List<DropInMemoInfo> GetList(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            List<DropInMemoInfo> list = new List<DropInMemoInfo>();
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

        public List<DropInMemoInfo> GetList(int CallStepID)
        {
            List<DropInMemoInfo> list = new List<DropInMemoInfo>();
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("select ").Append(ALL_PARM).Append(FROM_TABLE).Append(" where f_StepID=").Append(CallStepID).Append(" ORDER BY f_AddDate ASC");
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
        /// 根据callID 获取所有上门备注，并按ID排序
        /// </summary>
        /// <param name="CallID">CallID</param>
        /// <returns></returns>
        public List<DropInMemoInfo> GetListOrderByID(int CallID)
        {
            List<DropInMemoInfo> list = new List<DropInMemoInfo>();
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("select ").Append(ALL_PARM).Append(FROM_TABLE).Append(" where ");
            strSQL.Append("      f_StepID IN(SELECT ID ").Append(CallStepDAL.FROM_TABLE).Append(" WHERE f_CallID=").Append(CallID);
            strSQL.Append("                  ) ");
            //strSQL.Append("                                                                 AND f_StepType=").Append((int)SysEnum.StepType.到达门店处理).Append(") ");
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
        public DropInMemoInfo Get(int id)
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
        public int Add(DropInMemoInfo info)
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
        public bool Edit(DropInMemoInfo info)
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
        public bool DeleteByCallStep(int CallStepID)
        {
            StringBuilder strSQL = DeleteByCallStepIDsql(CallStepID.ToString());

            return SqlHelper.ExecuteNonQueryByTran(SqlHelper.SqlconnString, CommandType.Text, strSQL.ToString(), null);
        }

        public static StringBuilder DeleteByCallStepIDsql(string IDOrSelectIDSQL)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("delete ").Append(FROM_TABLE).Append(" where f_StepID in (").Append(IDOrSelectIDSQL).Append(") ");

            return strSQL;
        }

        
        #endregion
    }
}