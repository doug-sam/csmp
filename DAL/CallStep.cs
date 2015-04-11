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
    public class CallStepDAL
    {
        private const string ALL_PARM = "  ID,f_CallID,f_AddDate,f_DateBegin,f_DateEnd,f_UserID,f_UserName,f_StepName,f_StepIndex,f_SolutionID,f_SolutionName,f_Details,f_MajorUserID,f_MajorUserName,f_IsSolved,f_StepType ";
        public const string FROM_TABLE = " from [sys_CallStep] ";
        public const string TABLE = " sys_CallStep ";
        private const string INSET = " (f_CallID,f_AddDate,f_DateBegin,f_DateEnd,f_UserID,f_UserName,f_StepName,f_StepIndex,f_SolutionID,f_SolutionName,f_Details,f_MajorUserID,f_MajorUserName,f_IsSolved,f_StepType) values(@CallID,@AddDate,@DateBegin,@DateEnd,@UserID,@UserName,@StepName,@StepIndex,@SolutionID,@SolutionName,@Details,@MajorUserID,@MajorUserName,@IsSolved,@StepType)  ";
        private const string UPDATE = " f_CallID=@CallID,f_AddDate=@AddDate,f_DateBegin=@DateBegin,f_DateEnd=@DateEnd,f_UserID=@UserID,f_UserName=@UserName,f_StepName=@StepName,f_StepIndex=@StepIndex,f_SolutionID=@SolutionID,f_SolutionName=@SolutionName,f_Details=@Details,f_MajorUserID=@MajorUserID,f_MajorUserName=@MajorUserName,f_IsSolved=@IsSolved,f_StepType=@StepType ";

        #region ReadyData
        private CallStepInfo GetByDataReader(SqlDataReader rdr)
        {
            CallStepInfo info = new CallStepInfo();
            info.ID = Convert.ToInt32(rdr["ID"]);
            info.CallID = Convert.ToInt32(rdr["f_CallID"]);
            info.AddDate = Convert.ToDateTime(rdr["f_AddDate"]);
            info.DateBegin = Convert.ToDateTime(rdr["f_DateBegin"]);
            info.DateEnd = Convert.ToDateTime(rdr["f_DateEnd"]);
            info.UserID = Convert.ToInt32(rdr["f_UserID"]);
            info.UserName = rdr["f_UserName"].ToString();
            info.StepName = rdr["f_StepName"].ToString();
            info.StepIndex = Convert.ToInt32(rdr["f_StepIndex"]);
            info.SolutionID = Convert.ToInt32(rdr["f_SolutionID"]);
            info.SolutionName = rdr["f_SolutionName"].ToString();
            info.Details = rdr["f_Details"].ToString();
            info.MajorUserID = Convert.ToInt32(rdr["f_MajorUserID"]);
            info.MajorUserName = rdr["f_MajorUserName"].ToString();
            info.IsSolved = Convert.ToBoolean(rdr["f_IsSolved"].ToString());
            info.StepType = Convert.ToInt32(rdr["f_StepType"]);
            return info;
        }

        private SqlParameter[] GetParameter(CallStepInfo info)
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter("@CallID", info.CallID),
            new SqlParameter("@AddDate", info.AddDate),
            new SqlParameter("@DateBegin", info.DateBegin),
            new SqlParameter("@DateEnd", info.DateEnd),
            new SqlParameter("@UserID", info.UserID),
            new SqlParameter("@UserName", info.UserName),
            new SqlParameter("@StepName", info.StepName),
            new SqlParameter("@StepIndex", info.StepIndex),
            new SqlParameter("@SolutionID", info.SolutionID),
            new SqlParameter("@SolutionName", info.SolutionName),
            new SqlParameter("@Details", info.Details),
            new SqlParameter("@MajorUserID", info.MajorUserID),
            new SqlParameter("@MajorUserName", info.MajorUserName),
            new SqlParameter("@IsSolved", info.IsSolved),
            new SqlParameter("@StepType", info.StepType),
            
            };

            return parms;
        }

        #endregion


        #region Get
        /// <summary>
        /// 获取列表
        /// </summary>
        public List<CallStepInfo> GetList(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            List<CallStepInfo> list = new List<CallStepInfo>();
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

        public List<CallStepInfo> GetList(string StrWhere)
        {
            List<CallStepInfo> list = new List<CallStepInfo>();
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
        public CallStepInfo Get(int id)
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
        public CallStepInfo GetLast(int CallID, SysEnum.StepType? StepType)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("select top 1 ").Append(ALL_PARM).Append(FROM_TABLE).Append(" where f_CallID = ").Append(CallID);
            if (null != StepType) strSQL.Append(" and f_StepType=").Append((int)StepType);
            strSQL.Append(" order by id desc");

            using (SqlDataReader rdr = SqlHelper.ExecuteReader(SqlHelper.SqlconnString, CommandType.Text, strSQL.ToString(), null))
            {
                if (!rdr.Read()) return null;

                return GetByDataReader(rdr);
            }
        }

        /// <summary>
        /// 根据CallID获取记录
        /// </summary>
        /// <param name="CallID"></param>
        /// <returns></returns>
        public List<CallStepInfo> GetList(int CallID)
        {
            List<CallStepInfo> list = new List<CallStepInfo>();
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("select ").Append(ALL_PARM).Append(FROM_TABLE).Append(" where f_CallID=").Append(CallID).Append(" ORDER BY ID ASC");
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
        /// 根据CallID获取所在上门详细步的记录
        /// </summary>
        /// <param name="CallID"></param>
        /// <returns></returns>
        public List<CallStepInfo> GetListSln1(int CallID)
        {
            List<CallStepInfo> list = new List<CallStepInfo>();
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("select ").Append(ALL_PARM).Append(FROM_TABLE);
            strSQL.Append(" where f_CallID=").Append(CallID).Append(" and f_StepType=").Append((int)SysEnum.StepType.上门安排).Append(" ORDER BY ID ASC");
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
        /// 取得当前call共有多少步。没有则返回0
        /// </summary>
        /// <param name="CallID"></param>
        /// <returns></returns>
        public int GetMaxStepIndex(int CallID)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("select MAX(f_StepIndex) ").Append(FROM_TABLE).Append(" where f_CallID = ").Append(CallID);

            object obj = SqlHelper.ExecuteScalar(CommandType.Text, strSQL.ToString(), null);
            if (obj == DBNull.Value)
            {
                return 0;
            }
            return Convert.ToInt16(obj);
        }

        #endregion


        #region set
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="info">info</param>
        public int Add(CallStepInfo info)
        {
            StringBuilder strSQL;
            SqlParameter[] parms;
            AddSQL(info, out strSQL, out parms);

            if (SqlHelper.ExecuteNonQueryByTran(SqlHelper.SqlconnString, CommandType.Text, strSQL.ToString(), parms))
            {
                return Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.SqlconnString, CommandType.Text, "select max(id) from " + TABLE, null).ToString());
            }
            else
            {
                return 0;
            }
        }

        private void AddSQL(CallStepInfo info, out StringBuilder strSQL, out SqlParameter[] parms)
        {
            strSQL = new StringBuilder();
            strSQL.Append("insert into ").Append(TABLE).Append(INSET);

            parms = GetParameter(info);
        }

        /// <summary>
        /// 添加CallStepInfo，同时更新CallInfo
        /// </summary>
        /// <param name="cinfo"></param>
        /// <param name="sinfo"></param>
        /// <returns></returns>
        public bool AddCallStep_UpdateCall(CallInfo cinfo, CallStepInfo sinfo)
        {
            StringBuilder strSQL;
            SqlParameter[] parms;
            AddSQL(sinfo, out strSQL, out parms);
            SqlConnection con = new SqlConnection(SqlHelper.SqlconnString);
            con.Open();
            using (SqlTransaction tr = con.BeginTransaction())
            {
                #region CallStep
                int Result = SqlHelper.ExecuteNonQuery(tr, CommandType.Text, strSQL.ToString(), parms);
                if (Result <= 0)
                {
                    tr.Rollback(); con.Close(); return false;
                }
                #endregion


                #region Call
                strSQL = new StringBuilder();
                parms = new SqlParameter[] { };
                new CallDAL().EditSQL(cinfo, out strSQL, out parms);
                Result = SqlHelper.ExecuteNonQuery(tr, CommandType.Text, strSQL.ToString(), parms);
                if (Result <= 0)
                {
                    tr.Rollback(); con.Close(); return false;
                }
                #endregion
                tr.Commit();
                con.Close();
                return true;
            }
        }

        /// <summary>
        /// 删除操作步骤并更新call
        /// </summary>
        /// <param name="cinfo">要更新的call</param>
        /// <param name="sinfo">要保留的步骤，大于此步的将删除</param>
        /// <param name="opinfo">日志</param>
        /// <returns></returns>
        public bool DeleteCallStep_UpdateCall(CallInfo cinfo, CallStepInfo sinfo, OperationRecInfo opinfo)
        {
            StringBuilder strSQL = new StringBuilder();
            SqlParameter[] parms = null;
            StringBuilder sbCallStepSQL = new StringBuilder();
            sbCallStepSQL.Append(FROM_TABLE).Append(" WHERE f_CallID=").Append(cinfo.ID);
            sbCallStepSQL.Append(" AND ID>").Append(sinfo.ID);
            sbCallStepSQL.Append(" AND f_StepIndex>").Append(sinfo.StepIndex);
            SqlConnection con = new SqlConnection(SqlHelper.SqlconnString);
            con.Open();
            using (SqlTransaction tr = con.BeginTransaction())
            {
                try
                {

                    #region 上门备注
                    strSQL = DropInMemoDAL.DeleteByCallStepIDsql(" SELECT ID " + sbCallStepSQL);
                    SqlHelper.ExecuteNonQuery(tr, CommandType.Text, strSQL.ToString(), null); 
                    #endregion

                    #region CallStep

                    strSQL = new StringBuilder();
                    strSQL.Append(" DELETE ").Append(sbCallStepSQL);
                    int Result = SqlHelper.ExecuteNonQuery(tr, CommandType.Text, strSQL.ToString(), parms);
                    if (Result <= 0)
                    {
                        tr.Rollback(); con.Close(); return false;
                    }
                    #endregion

                    #region OperationRe日志
                    strSQL = new StringBuilder();
                    parms = new SqlParameter[] { };
                    new OperationRecDAL().AddSQL(opinfo, out strSQL, out parms);
                    Result = SqlHelper.ExecuteNonQuery(tr, CommandType.Text, strSQL.ToString(), parms);
                    if (Result <= 0)
                    {
                        tr.Rollback(); con.Close(); return false;
                    } 
                    #endregion


                    #region Call
                    strSQL = new StringBuilder();
                    parms = new SqlParameter[] { };
                    new CallDAL().EditSQL(cinfo, out strSQL, out parms);
                    Result = SqlHelper.ExecuteNonQuery(tr, CommandType.Text, strSQL.ToString(), parms);
                    if (Result <= 0)
                    {
                        tr.Rollback(); con.Close(); return false;
                    }
                    #endregion
                    tr.Commit();
                    con.Close();
                    return true;

                }
                catch (Exception)
                {
                    tr.Rollback(); con.Close(); return false;
                }
            }
        }


        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="info">info</param>
        public bool Edit(CallStepInfo info)
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