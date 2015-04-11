using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using DBUtility;
using CSMP.Model;

namespace CSMP.DAL
{
    public class WorkGroupBrandDAL
    {
        private const string ALL_PARM = "  ID,f_MID,f_MName,f_WorkGroupID,f_WorkGroupName ";
        public const string FROM_TABLE = " from [sys_WorkGroupBrand] ";
        public const string TABLE = " sys_WorkGroupBrand ";
        private const string INSET = " (f_MID,f_MName,f_WorkGroupID,f_WorkGroupName) values(@MID,@MName,@WorkGroupID,@WorkGroupName)  ";
        private const string UPDATE = " f_MID=@MID,f_MName=@MName,f_WorkGroupID=@WorkGroupID,f_WorkGroupName=@WorkGroupName ";

        #region ReadyData
        private WorkGroupBrandInfo GetByDataReader(SqlDataReader rdr)
        {
            WorkGroupBrandInfo info = new WorkGroupBrandInfo();
            info.ID = Convert.ToInt32(rdr["ID"]);
            info.MID = Convert.ToInt32(rdr["f_MID"]);
            info.MName = rdr["f_MName"].ToString();
            info.WorkGroupID = Convert.ToInt32(rdr["f_WorkGroupID"]);
            info.WorkGroupName = rdr["f_WorkGroupName"].ToString();
            
            return info;
        }

        private SqlParameter[] GetParameter(WorkGroupBrandInfo info)
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter("@MID", info.MID),
            new SqlParameter("@MName", info.MName),
            new SqlParameter("@WorkGroupID", info.WorkGroupID),
            new SqlParameter("@WorkGroupName", info.WorkGroupName),
            
            };

            return parms;
        }
        
        #endregion

        #region Get
        /// <summary>
        /// 获取列表
        /// </summary>
        public List<WorkGroupBrandInfo> GetList(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            List<WorkGroupBrandInfo> list = new List<WorkGroupBrandInfo>();
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

        public List<WorkGroupBrandInfo> GetList(string StrWhere)
        {
            List<WorkGroupBrandInfo> list = new List<WorkGroupBrandInfo>();
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
        public WorkGroupBrandInfo Get(int id)
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
        /// 工作组跟客户是否有关系
        /// </summary>
        /// <param name="WorkGroupID"></param>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public bool HasRelaction(int WorkGroupID,int BrandID)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("select COUNT(1) ").Append(FROM_TABLE);
            strSQL.Append(" where f_MID=").Append(BrandID).Append(" and f_WorkGroupID=").Append(WorkGroupID);
            object obj = SqlHelper.ExecuteScalar(SqlHelper.SqlconnString, CommandType.Text, strSQL.ToString(), null);
            return Convert.ToInt16(obj)>0;

        }

        #endregion


        #region set
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="info">info</param>
        public int Add(WorkGroupBrandInfo info)
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
        public bool Edit(WorkGroupBrandInfo info)
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
        /// 删除
        /// </summary>
        /// <param name="WorkGroupID">工作组</param>
        /// <param name="CustomerID">客户ID</param>
        /// <returns></returns>
        public bool Delete(int WorkGroupID,int CustomerID)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("delete ").Append(FROM_TABLE).Append(" where f_WorkGroupID = ").Append(WorkGroupID);
            strSQL.Append(" AND f_MID in(select ID from sys_Brand where sys_Brand.f_CustomerID=").Append(CustomerID).Append(") ");

            return SqlHelper.ExecuteNonQueryByTran(SqlHelper.SqlconnString, CommandType.Text, strSQL.ToString(), null);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">Member id</param>
        public bool DeleteByMID(int MID)
        {
            StringBuilder strSQL = DeleteByMIDsql(MID.ToString());

            return SqlHelper.ExecuteNonQueryByTran(SqlHelper.SqlconnString, CommandType.Text, strSQL.ToString(), null);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">Member id</param>
        public bool DeleteByWorkGroupID(int WorkGroupID)
        {
            StringBuilder strSQL = DeleteByWorkGroupIDsql(WorkGroupID.ToString());

            return SqlHelper.ExecuteNonQueryByTran(SqlHelper.SqlconnString, CommandType.Text, strSQL.ToString(), null);
        }





        public static StringBuilder DeleteByMIDsql(string IDOrSelectIDSQL)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("delete ").Append(FROM_TABLE).Append(" where f_MID in (").Append(IDOrSelectIDSQL).Append(") ");
            return strSQL;
        }

        public static StringBuilder DeleteByWorkGroupIDsql(string IDOrSelectIDSQL)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("delete ").Append(FROM_TABLE).Append(" where f_WorkGroupID in (").Append(IDOrSelectIDSQL).Append(") ");
            return strSQL;
        }

        
        #endregion
    }
}