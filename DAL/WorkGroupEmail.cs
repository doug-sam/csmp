using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using DBUtility;
using CSMP.Model;

namespace CSMP.DAL
{
    public class WorkGroupEmailDAL
    {
        private const string ALL_PARM = "  ID,f_Email,f_Name,f_GroupID,f_EmailGroupID,f_BrandID,f_OrderNo,f_Enable ";
        public const string FROM_TABLE = " from [sys_WorkGroupEmail] ";
        private const string TABLE = " sys_WorkGroupEmail ";
        private const string INSET = " (f_Email,f_Name,f_GroupID,f_EmailGroupID,f_BrandID,f_OrderNo,f_Enable) values(@Email,@Name,@GroupID,@EmailGroupID,@BrandID,@OrderNo,@Enable)  ";
        private const string UPDATE = " f_Email=@Email,f_Name=@Name,f_GroupID=@GroupID,f_EmailGroupID=@EmailGroupID,f_BrandID=@BrandID,f_OrderNo=@OrderNo,f_Enable=@Enable ";

        #region ReadyData
        private WorkGroupEmailInfo GetByDataReader(SqlDataReader rdr)
        {
            WorkGroupEmailInfo info = new WorkGroupEmailInfo();
            info.ID = Convert.ToInt32(rdr["ID"]);
            info.Email = rdr["f_Email"].ToString();
            info.Name = rdr["f_Name"].ToString();
            info.GroupID = Convert.ToInt32(rdr["f_GroupID"]);
            info.EmailGroupID = Convert.ToInt32(rdr["f_EmailGroupID"]);
            info.BrandID = Convert.ToInt32(rdr["f_BrandID"]);
            info.OrderNo = Convert.ToInt32(rdr["f_OrderNo"]);
            info.Enable = Convert.ToBoolean(rdr["f_Enable"]);
            
            return info;
        }

        private SqlParameter[] GetParameter(WorkGroupEmailInfo info)
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter("@Email", info.Email),
            new SqlParameter("@Name", info.Name),
            new SqlParameter("@GroupID", info.GroupID),
            new SqlParameter("@EmailGroupID", info.EmailGroupID),
            new SqlParameter("@BrandID", info.BrandID),
            new SqlParameter("@OrderNo", info.OrderNo),
            new SqlParameter("@Enable", info.Enable),
            
            };

            return parms;
        }
        
        #endregion


        #region Get
        /// <summary>
        /// 获取列表
        /// </summary>
        public List<WorkGroupEmailInfo> GetList(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            List<WorkGroupEmailInfo> list = new List<WorkGroupEmailInfo>();
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

        public List<WorkGroupEmailInfo> GetList(string StrWhere)
        {
            List<WorkGroupEmailInfo> list = new List<WorkGroupEmailInfo>();
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
        public WorkGroupEmailInfo Get(int id)
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
        public int Add(WorkGroupEmailInfo info)
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
        public bool Edit(WorkGroupEmailInfo info)
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