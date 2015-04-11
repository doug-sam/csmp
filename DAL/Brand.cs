using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using DBUtility;
using CSMP.Model;

namespace CSMP.DAL
{
    public class BrandDAL
    {
        private const string ALL_PARM = "  ID,f_Name,f_CustomerID,f_CustomerName,f_UserID,f_Contact,f_Phone,f_Email,f_IsClose,f_SlaModeID,f_SlaTimer1,f_SlaTimer2,f_SlaTimerTo ";
        private const string FROM_TABLE = " from [sys_Brand] ";
        public const string TABLE = " sys_Brand ";
        private const string INSET = " (f_Name,f_CustomerID,f_CustomerName,f_UserID,f_Contact,f_Phone,f_Email,f_IsClose,f_SlaModeID,f_SlaTimer1,f_SlaTimer2,f_SlaTimerTo) values(@Name,@CustomerID,@CustomerName,@UserID,@Contact,@Phone,@Email,@IsClose,@SlaModeID,@SlaTimer1,@SlaTimer2,@SlaTimerTo)  ";
        private const string UPDATE = " f_Name=@Name,f_CustomerID=@CustomerID,f_CustomerName=@CustomerName,f_UserID=@UserID,f_Contact=@Contact,f_Phone=@Phone,f_Email=@Email,f_IsClose=@IsClose,f_SlaModeID=@SlaModeID,f_SlaTimer1=@SlaTimer1,f_SlaTimer2=@SlaTimer2,f_SlaTimerTo=@SlaTimerTo ";

        #region ReadyData
        private BrandInfo GetByDataReader(SqlDataReader rdr)
        {
            BrandInfo info = new BrandInfo();
            info.ID = Convert.ToInt32(rdr["ID"]);
            info.Name = rdr["f_Name"].ToString();
            info.CustomerID = Convert.ToInt32(rdr["f_CustomerID"]);
            info.CustomerName = rdr["f_CustomerName"].ToString();
            info.UserID = Convert.ToInt32(rdr["f_UserID"]);
            info.Contact = rdr["f_Contact"].ToString();
            info.Phone = rdr["f_Phone"].ToString();
            info.Email = rdr["f_Email"].ToString();
            info.IsClose = Convert.ToBoolean(rdr["f_IsClose"]);
            info.SlaModeID = Convert.ToInt32(rdr["f_SlaModeID"]);
            info.SlaTimer1 = Convert.ToInt32(rdr["f_SlaTimer1"]);
            info.SlaTimer2 = Convert.ToInt32(rdr["f_SlaTimer2"]);
            info.SlaTimerTo = rdr["f_SlaTimerTo"].ToString();
            return info;
        }

        private SqlParameter[] GetParameter(BrandInfo info)
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter("@Name", info.Name),
            new SqlParameter("@CustomerID", info.CustomerID),
            new SqlParameter("@CustomerName", info.CustomerName),
            new SqlParameter("@UserID", info.UserID),
            new SqlParameter("@Contact", info.Contact),
            new SqlParameter("@Phone", info.Phone),
            new SqlParameter("@Email", info.Email),
            new SqlParameter("@IsClose", info.IsClose),
            new SqlParameter("@SlaModeID", info.SlaModeID),
            new SqlParameter("@SlaTimer1", info.SlaTimer1),
            new SqlParameter("@SlaTimer2", info.SlaTimer2),
            new SqlParameter("@SlaTimerTo", info.SlaTimerTo),
            
            };

            return parms;
        }

        #endregion


        #region Get
        /// <summary>
        /// 获取列表
        /// </summary>
        public List<BrandInfo> GetList(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            List<BrandInfo> list = new List<BrandInfo>();
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

        public List<BrandInfo> GetList(string StrWhere)
        {
            List<BrandInfo> list = new List<BrandInfo>();
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("select ").Append(ALL_PARM).Append(FROM_TABLE);
            strSQL.Append(" where ").Append(" 1=1 AND f_IsClose=0 ").Append(StrWhere.Replace("1=1", " "));
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
        public BrandInfo Get(int id)
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
        /// <param name="Name">Name</param>
        public BrandInfo Get(string Name)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("select top 1 ").Append(ALL_PARM).Append(FROM_TABLE).Append(" where f_Name = '").Append(Name).Append("' ");

            using (SqlDataReader rdr = SqlHelper.ExecuteReader(SqlHelper.SqlconnString, CommandType.Text, strSQL.ToString(), null))
            {
                if (!rdr.Read()) return null;

                return GetByDataReader(rdr);
            }
        }

        /// <summary>
        /// 检查数据库中名称是否重复
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public BrandInfo Get(string value, int CustomerID)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("SELECT  top 1  ").Append(ALL_PARM).Append(FROM_TABLE).Append(" WHERE f_Name='").Append(value).Append("' ");
            if (CustomerID > 0) strSQL.Append(" AND f_CustomerID=").Append(CustomerID);
            using (SqlDataReader rdr = SqlHelper.ExecuteReader(SqlHelper.SqlconnString, CommandType.Text, strSQL.ToString(), null))
            {
                if (!rdr.Read()) return null;

                return GetByDataReader(rdr);
            }
        }

        public List<BrandInfo> GetList(int WorkGroupID)
        {
            List<BrandInfo> list = new List<BrandInfo>();
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("SELECT sys_Brand.* FROM ").Append(TABLE).Append(",sys_WorkGroupBrand ");
            strSQL.Append("WHERE sys_Brand.ID=sys_WorkGroupBrand.f_MID AND sys_WorkGroupBrand.f_WorkGroupID=").Append(WorkGroupID);
            strSQL.Append(" AND sys_Brand.f_IsClose=0 ");
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
        /// 根据知识库id找到品牌
        /// </summary>
        /// <param name="KnowledgeID"></param>
        /// <returns></returns>
        public List<BrandInfo> GetListByKnowledgeID(int KnowledgeID)
        {
            List<BrandInfo> list = new List<BrandInfo>();
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("select ").Append(ALL_PARM).Append(FROM_TABLE);
            strSQL.Append(" where ").Append("  ID in( ");
            strSQL.Append("                             select f_BrandID from sys_KnowkedgeBaseBrand where f_KnowledgeID=").Append(KnowledgeID);
            strSQL.Append("                         )");
            using (SqlDataReader rdr = SqlHelper.ExecuteReader(SqlHelper.SqlconnString, CommandType.Text, strSQL.ToString(), null))
            {
                while (rdr.Read())
                {
                    list.Add(GetByDataReader(rdr));
                }
            }
            return list;
        }


        #endregion


        #region set
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="info">info</param>
        public int Add(BrandInfo info)
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
        public bool Edit(BrandInfo info)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("update ").Append(TABLE).Append(" set ").Append(UPDATE).Append(" where id = ").Append(info.ID);
            strSQL.Append(UpdateNameSQL(info));
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
            strSQL.Append(StoresDAL.DeleteByBrandIDsql(id.ToString()));
            strSQL.Append("delete ").Append(FROM_TABLE).Append(" where id = ").Append(id);


            return SqlHelper.ExecuteNonQueryByTran(SqlHelper.SqlconnString, CommandType.Text, strSQL.ToString(), null);
        }

        public static StringBuilder DeleteByCustomerIDsql(string IDOrSelectIDSQL)
        {
            StringBuilder sbBrandIDs = new StringBuilder();
            sbBrandIDs.Append("select ID").Append(FROM_TABLE).Append(" where f_CustomerID in(").Append(IDOrSelectIDSQL).Append(") ");

            StringBuilder strSQL = new StringBuilder();
            strSQL.Append(WorkGroupBrandDAL.DeleteByMIDsql(sbBrandIDs.ToString()));

            strSQL.Append(StoresDAL.DeleteByBrandIDsql(sbBrandIDs.ToString()));
            strSQL.Append("delete ").Append(FROM_TABLE).Append(" where id in (").Append(sbBrandIDs.ToString()).Append(" ) ");
            return strSQL;
        }

        /// <summary>
        /// 其新其它表的
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private static string UpdateNameSQL(BrandInfo info)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" UPDATE ").Append(StoresDAL.TABLE).Append(" SET f_BrandName='{0}',f_CustomerID={2},f_CustomerName='{3}' WHERE f_BrandID={1} ");
            sb.Append(" UPDATE ").Append(CallDAL.TABLE).Append(" SET f_BrandName='{0}',f_CustomerID={2},f_CustomerName='{3}' WHERE f_BrandID={1} ");
            sb.Append(" UPDATE ").Append(WorkGroupBrandDAL.TABLE).Append(" SET f_MName='{0}' WHERE f_MID={1} ");

            return string.Format(sb.ToString(), info.Name, info.ID, info.CustomerID, info.CustomerName);
        }


        #endregion
    }
}