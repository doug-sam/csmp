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
    public class AssignDAL
    {
        private const string ALL_PARM = "  ID,f_CallID,f_Step,f_UseID,f_UserName,f_AddDate,f_OldName,f_OldID,f_WorkGroupID,f_CreatorID,f_CreatorName,f_CrossWorkGroup,f_AssignType ";
        private const string FROM_TABLE = " from [sys_Assign] ";
        private const string TABLE = " sys_Assign ";
        private const string INSET = " (f_CallID,f_Step,f_UseID,f_UserName,f_AddDate,f_OldName,f_OldID,f_WorkGroupID,f_CreatorID,f_CreatorName,f_CrossWorkGroup,f_AssignType) values(@CallID,@Step,@UseID,@UserName,@AddDate,@OldName,@OldID,@WorkGroupID,@CreatorID,@CreatorName,@CrossWorkGroup,@AssignType)  ";
        private const string UPDATE = " f_CallID=@CallID,f_Step=@Step,f_UseID=@UseID,f_UserName=@UserName,f_AddDate=@AddDate,f_OldName=@OldName,f_OldID=@OldID,f_WorkGroupID=@WorkGroupID,f_CreatorID=@CreatorID,f_CreatorName=@CreatorName,f_CrossWorkGroup=@CrossWorkGroup,f_AssignType=@AssignType ";

        #region ReadyData
        private AssignInfo GetByDataReader(SqlDataReader rdr)
        {
            AssignInfo info = new AssignInfo();
            info.ID = Convert.ToInt32(rdr["ID"]);
            info.CallID = Convert.ToInt32(rdr["f_CallID"]);
            info.Step = Convert.ToInt32(rdr["f_Step"]);
            info.UseID = Convert.ToInt32(rdr["f_UseID"]);
            info.UserName = rdr["f_UserName"].ToString();
            info.AddDate = Convert.ToDateTime(rdr["f_AddDate"]);
            info.OldName = rdr["f_OldName"].ToString();
            info.OldID = Convert.ToInt32(rdr["f_OldID"]);
            info.WorkGroupID = Convert.ToInt32(rdr["f_WorkGroupID"]);
            info.CreatorID = Convert.ToInt32(rdr["f_CreatorID"]);
            info.CreatorName = rdr["f_CreatorName"].ToString();
            info.CrossWorkGroup = Convert.ToBoolean(rdr["f_CrossWorkGroup"]);
            info.AssignType = Convert.ToInt32(rdr["f_AssignType"]);
            return info;
        }

        private SqlParameter[] GetParameter(AssignInfo info)
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter("@CallID", info.CallID),
            new SqlParameter("@Step", info.Step),
            new SqlParameter("@UseID", info.UseID),
            new SqlParameter("@UserName", info.UserName),
            new SqlParameter("@AddDate", info.AddDate),
            new SqlParameter("@OldName", info.OldName),
            new SqlParameter("@OldID", info.OldID),
            new SqlParameter("@WorkGroupID", info.WorkGroupID),
            new SqlParameter("@CreatorID", info.CreatorID),
            new SqlParameter("@CreatorName", info.CreatorName),
            new SqlParameter("@CrossWorkGroup", info.CrossWorkGroup),
            new SqlParameter("@AssignType", info.AssignType),
            
            };

            return parms;
        }
        
        #endregion


        #region Get
        /// <summary>
        /// 获取列表
        /// </summary>
        public List<AssignInfo> GetList(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            List<AssignInfo> list = new List<AssignInfo>();
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

        public List<AssignInfo> GetList(string StrWhere)
        {
            List<AssignInfo> list = new List<AssignInfo>();
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
        public AssignInfo Get(int id)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("select top 1 ").Append(ALL_PARM).Append(FROM_TABLE).Append(" where id = ").Append(id);

            using (SqlDataReader rdr = SqlHelper.ExecuteReader(SqlHelper.SqlconnString, CommandType.Text, strSQL.ToString(), null))
            {
                if (!rdr.Read()) return null;

                return GetByDataReader(rdr);
            }
        }

        public AssignInfo GetMax(int CallID)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("select top 1 ").Append(ALL_PARM).Append(FROM_TABLE).Append(" where f_CallID=").Append(CallID).Append(" order By ID desc ");
            using (SqlDataReader rdr = SqlHelper.ExecuteReader(SqlHelper.SqlconnString, CommandType.Text, strSQL.ToString(), null))
            {
                if (!rdr.Read()) return null;

                return GetByDataReader(rdr);
            }
        }
        /// <summary>
        /// 获取做转派操作
        /// </summary>
        /// <param name="CallID"></param>
        /// <returns></returns>
        public AssignInfo GetZhuanPaiMax(int CallID)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("select top 1 ").Append(ALL_PARM).Append(FROM_TABLE).Append(" where f_CallID=").Append(CallID).Append("  AND f_AssignType=0 order By ID desc ");
            using (SqlDataReader rdr = SqlHelper.ExecuteReader(SqlHelper.SqlconnString, CommandType.Text, strSQL.ToString(), null))
            {
                if (!rdr.Read()) return null;

                return GetByDataReader(rdr);
            }
        }
        /// <summary>
        /// 获取更换现场工程师
        /// </summary>
        /// <param name="CallID"></param>
        /// <returns></returns>
        public AssignInfo GetChangeEngineerMax(int CallID)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("select top 1 ").Append(ALL_PARM).Append(FROM_TABLE).Append(" where f_CallID=").Append(CallID).Append("  AND f_AssignType=1 order By ID desc ");
            using (SqlDataReader rdr = SqlHelper.ExecuteReader(SqlHelper.SqlconnString, CommandType.Text, strSQL.ToString(), null))
            {
                if (!rdr.Read()) return null;

                return GetByDataReader(rdr);
            }
        }

        public List<AssignInfo> GetList(int CallID)
        {
            return GetList(" f_CallID=" + CallID + " order by id asc ");
        }

        public List<AssignInfo> GetList(int CallID,bool IsCrossWorkGroup)
        {
            string strWhere = " f_CallID=" + CallID ;
            if (IsCrossWorkGroup)
            {
                strWhere +=string.Format(" AND f_CrossWorkGroup=1 ");   
            }
            strWhere +=" order by id asc ";
            return GetList(strWhere);
        }

        #endregion


        #region set
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="info">info</param>
        public int Add(AssignInfo info)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("insert into ").Append(TABLE).Append(INSET);

            SqlParameter[] parms = GetParameter(info);

            if (SqlHelper.ExecuteNonQueryByTran(SqlHelper.SqlconnString, CommandType.Text, strSQL.ToString(), parms))
            {
                Logger.GetLogger(this.GetType()).Info("转派成功，callid=" + info.CallID+"，操作人："+info.CreatorName, null);
                return Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.SqlconnString, CommandType.Text, "select max(id) from " + TABLE, null).ToString());
            }
            else
            {
                Logger.GetLogger(this.GetType()).Info("转派失败，callid=" + info.CallID + "，操作人：" + info.CreatorName, null);
                return 0;
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="info">info</param>
        public bool Edit(AssignInfo info)
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
            StringBuilder strSQL = DeleteByCallIDsql(CallID.ToString());

            return SqlHelper.ExecuteNonQueryByTran(SqlHelper.SqlconnString, CommandType.Text, strSQL.ToString(), null);
        }

        public static StringBuilder DeleteByCallIDsql(string IDOrSelectIDSQL)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("delete ").Append(FROM_TABLE).Append(" where f_CallID in (").Append(IDOrSelectIDSQL).Append(") ");

            return strSQL;
        }

        
        #endregion

        /// <summary>
        /// 该call转派过的公司
        /// </summary>
        /// <param name="CallID"></param>
        /// <returns></returns>
        public int GetCrossWorkGroupID(int CallID)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("select MAX(f_WorkGroupID) ").Append(FROM_TABLE).Append(" where f_CallID = ").Append(CallID);
            strSQL.Append(" AND f_CrossWorkGroup=1 AND f_AssignType=0");
           object obj= SqlHelper.ExecuteScalar(CommandType.Text, strSQL.ToString(), null);
           if (obj==DBNull.Value)
           {
               return 0;
           }
           return Convert.ToInt16(obj);
        }
    }
}