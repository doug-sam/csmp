using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using DBUtility;
using CSMP.Model;

namespace CSMP.DAL
{
    public class StoresDAL
    {
        private const string ALL_PARM = "  ID,f_No,f_Name,f_BrandID,f_ProvinceID,f_CityID,f_Address,f_Tel,f_IsClosed,f_BrandName,f_ProvinceName,f_CityName,f_CustomerID,f_CustomerName,f_Email,f_AddDate ";
        public const string FROM_TABLE = " from [sys_Stores] ";
        public const string TABLE = " sys_Stores ";
        private const string INSET = " (f_No,f_Name,f_BrandID,f_ProvinceID,f_CityID,f_Address,f_Tel,f_IsClosed,f_BrandName,f_ProvinceName,f_CityName,f_CustomerID,f_CustomerName,f_Email,f_AddDate) values(@No,@Name,@BrandID,@ProvinceID,@CityID,@Address,@Tel,@IsClosed,@BrandName,@ProvinceName,@CityName,@CustomerID,@CustomerName,@Email,@AddDate)  ";
        private const string UPDATE = " f_No=@No,f_Name=@Name,f_BrandID=@BrandID,f_ProvinceID=@ProvinceID,f_CityID=@CityID,f_Address=@Address,f_Tel=@Tel,f_IsClosed=@IsClosed,f_BrandName=@BrandName,f_ProvinceName=@ProvinceName,f_CityName=@CityName,f_CustomerID=@CustomerID,f_CustomerName=@CustomerName,f_Email=@Email,f_AddDate=@AddDate ";

        #region ReadyData
        private StoreInfo GetByDataReader(SqlDataReader rdr)
        {
            StoreInfo info = new StoreInfo();
            info.ID = Convert.ToInt32(rdr["ID"]);
            info.No = rdr["f_No"].ToString();
            info.Name = rdr["f_Name"].ToString();
            info.BrandID = Convert.ToInt32(rdr["f_BrandID"]);
            info.ProvinceID = Convert.ToInt32(rdr["f_ProvinceID"]);
            info.CityID = Convert.ToInt32(rdr["f_CityID"]);
            info.Address = rdr["f_Address"].ToString();
            info.Tel = rdr["f_Tel"].ToString();
            info.IsClosed = Convert.ToBoolean(rdr["f_IsClosed"]);
            info.BrandName = rdr["f_BrandName"].ToString();
            info.ProvinceName = rdr["f_ProvinceName"].ToString();
            info.CityName = rdr["f_CityName"].ToString();
            info.CustomerID = Convert.ToInt32(rdr["f_CustomerID"]);
            info.CustomerName = rdr["f_CustomerName"].ToString();
            info.Email = rdr["f_Email"].ToString();
            info.AddDate = Convert.ToDateTime(rdr["f_AddDate"]);

            return info;
        }

        private SqlParameter[] GetParameter(StoreInfo info)
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter("@No", info.No),
            new SqlParameter("@Name", info.Name),
            new SqlParameter("@BrandID", info.BrandID),
            new SqlParameter("@ProvinceID", info.ProvinceID),
            new SqlParameter("@CityID", info.CityID),
            new SqlParameter("@Address", info.Address),
            new SqlParameter("@Tel", info.Tel),
            new SqlParameter("@IsClosed", info.IsClosed),
            new SqlParameter("@BrandName", info.BrandName),
            new SqlParameter("@ProvinceName", info.ProvinceName),
            new SqlParameter("@CityName", info.CityName),
            new SqlParameter("@CustomerID", info.CustomerID),
            new SqlParameter("@CustomerName", info.CustomerName),
            new SqlParameter("@Email", info.Email),
            new SqlParameter("@AddDate", info.AddDate),
            
            };

            return parms;
        }
        
        #endregion


        #region Get
        /// <summary>
        /// 获取列表
        /// </summary>
        public List<StoreInfo> GetList(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            List<StoreInfo> list = new List<StoreInfo>();
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

        public List<StoreInfo> GetList(string StrWhere)
        {
            List<StoreInfo> list = new List<StoreInfo>();
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("select ").Append(ALL_PARM).Append(FROM_TABLE);
            strSQL.Append(" where ").Append(" 1=1 AND f_IsClosed=0 ").Append(StrWhere.Replace("1=1"," "));
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
        /// 获取列表
        /// </summary>
        public List<StoreInfo> GetList(string Key, int WorkGroup)
        {
            List<StoreInfo> list = new List<StoreInfo>();
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("select sys_Stores.* ").Append(FROM_TABLE);
            strSQL.Append(" WHERE 1=1 ");
            if (WorkGroup > 0)
            {
                strSQL.Append(" and f_BrandID in( ");
                strSQL.Append("     select f_MID from sys_WorkGroupBrand where f_WorkGroupID=").Append(WorkGroup);
                strSQL.Append(" )");
            }
            if (!string.IsNullOrEmpty(Key))
            {
                strSQL.Append(" AND sys_Stores.f_No LIKE '%").Append(Key).Append("%'");
            }
            strSQL.Append(" AND sys_Stores.f_IsClosed=0 ");
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
        /// 获取列表
        /// </summary>
        public List<StoreInfo> GetList(int CityID, int WorkGroup)
        {
            List<StoreInfo> list = new List<StoreInfo>();
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("select sys_Stores.* ").Append(FROM_TABLE);
            strSQL.Append(" WHERE 1=1 ");
            if (WorkGroup > 0)
            {
                strSQL.Append(" and f_BrandID in( ");
                strSQL.Append("     select f_MID from sys_WorkGroupBrand where f_WorkGroupID=").Append(WorkGroup);
                strSQL.Append(" )");
            }

            strSQL.Append(" AND sys_Stores.f_CityID=").Append(CityID).Append(" ");
            strSQL.Append(" AND sys_Stores.f_IsClosed=0 ");
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
        public StoreInfo Get(int id)
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
        public StoreInfo Get(string Tel)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("select top 1 ").Append(ALL_PARM).Append(FROM_TABLE).Append(" where f_Tel like '%").Append(Tel).Append("%' ");

            using (SqlDataReader rdr = SqlHelper.ExecuteReader(SqlHelper.SqlconnString, CommandType.Text, strSQL.ToString(), null))
            {
                if (!rdr.Read()) return null;

                return GetByDataReader(rdr);
            }
        }
       /// <summary>
        /// 获取Info
       /// </summary>
       /// <param name="Tel">电话号码</param>
       /// <returns></returns>
        public StoreInfo GetByCallNO(string Tel)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("select top 1 ").Append(ALL_PARM).Append(FROM_TABLE).Append(" where f_Tel like '%").Append(Tel).Append("%' AND f_IsClose=0");

            using (SqlDataReader rdr = SqlHelper.ExecuteReader(SqlHelper.SqlconnString, CommandType.Text, strSQL.ToString(), null))
            {
                if (!rdr.Read()) return null;

                return GetByDataReader(rdr);
            }
        }

        /// <summary>
        /// 根据店铺号查找
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public StoreInfo GetByStoreNo(string StoreNo)
        {
            StringBuilder strSQL = new StringBuilder();
            SqlParameter[] parms = new SqlParameter[] { new SqlParameter("@StoreNo", StoreNo)};

            strSQL.Append("select top 1 ").Append(ALL_PARM).Append(FROM_TABLE).Append(" WHERE f_NO=@StoreNo ");
            using (SqlDataReader rdr = SqlHelper.ExecuteReader(SqlHelper.SqlconnString, CommandType.Text, strSQL.ToString(), parms))
            {
                if (!rdr.Read()) return null;

                return GetByDataReader(rdr);
            }
        }

        /// <summary>
        /// 检查数据库中电话是否重复
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TelExit(string value)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("SELECT COUNT(1)  ").Append(FROM_TABLE).Append(" WHERE f_Tel=@tel ");
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter("@tel", value)
            };
            object o = SqlHelper.ExecuteScalar(CommandType.Text, strSQL.ToString(), parms);
            return Convert.ToInt16(o) > 0;
        }

        #endregion


        #region set
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="info">info</param>
        public int Add(StoreInfo info)
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
        public bool Edit(StoreInfo info)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("update ").Append(TABLE).Append(" set ").Append(UPDATE).Append(" where id = ").Append(info.ID);
            strSQL.Append(UpdateNameSQL(info.ID,info.Name,info.No));
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
            strSQL.Append(CallDAL.DeleteByStoreIDsql(id.ToString()));
            strSQL.Append("delete ").Append(FROM_TABLE).Append(" where id = ").Append(id);

            return SqlHelper.ExecuteNonQueryByTran(SqlHelper.SqlconnString, CommandType.Text, strSQL.ToString(), null);
        }


        public static StringBuilder DeleteByBrandIDsql(string IDOrSelectIDSQL)
        {
            StringBuilder sbStoreIDs = new StringBuilder();
            sbStoreIDs.Append("select ID").Append(FROM_TABLE).Append(" where f_BrandID in(").Append(IDOrSelectIDSQL).Append(") ");

            StringBuilder strSQL = new StringBuilder();
            strSQL.Append(CallDAL.DeleteByStoreIDsql(sbStoreIDs.ToString()));
            strSQL.Append("delete ").Append(FROM_TABLE).Append(" where id in (").Append(sbStoreIDs.ToString()).Append(" ) ");
            return strSQL;
        }

        /// <summary>
        /// 其新其它表的名称语句
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Name"></param>
        /// <returns></returns>
        private static string UpdateNameSQL(int ID, string Name, string StoreNo)
        {
            StringBuilder sb = new StringBuilder();
            if (ID<=0)
            {
                return string.Empty;
            }
            sb.Append(" UPDATE ").Append(CallDAL.TABLE).Append(" SET f_StoreName='{0}',f_StoreNo='{1}' WHERE f_StoreID={2} ");

            return string.Format(sb.ToString(), Name,StoreNo, ID);
        }


        
        #endregion
    }
}