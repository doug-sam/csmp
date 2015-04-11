using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using DBUtility;
using CSMP.Model;

namespace CSMP.DAL
{
    public class CustomersDAL
    {
        private const string ALL_PARM = "  ID,f_Name,f_Contact,f_Phone,f_Email,f_CityID,f_IsClosed ";
        private const string FROM_TABLE = " from [sys_Customers] ";
        private const string TABLE = " sys_Customers ";
        private const string INSET = " (f_Name,f_Contact,f_Phone,f_Email,f_CityID,f_IsClosed) values(@Name,@Contact,@Phone,@Email,@CityID,@IsClosed)  ";
        private const string UPDATE = " f_Name=@Name,f_Contact=@Contact,f_Phone=@Phone,f_Email=@Email,f_CityID=@CityID,f_IsClosed=@IsClosed ";

        #region ReadyData
        private CustomersInfo GetByDataReader(SqlDataReader rdr)
        {
            CustomersInfo info = new CustomersInfo();
            info.ID = Convert.ToInt32(rdr["ID"]);
            info.Name = rdr["f_Name"].ToString();
            info.Contact = rdr["f_Contact"].ToString();
            info.Phone = rdr["f_Phone"].ToString();
            info.Email = rdr["f_Email"].ToString();
            info.CityID = Convert.ToInt32(rdr["f_CityID"]);
            info.IsClosed = Convert.ToBoolean(rdr["f_IsClosed"]);
            
            return info;
        }

        private SqlParameter[] GetParameter(CustomersInfo info)
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter("@Name", info.Name),
            new SqlParameter("@Contact", info.Contact),
            new SqlParameter("@Phone", info.Phone),
            new SqlParameter("@Email", info.Email),
            new SqlParameter("@CityID", info.CityID),
            new SqlParameter("@IsClosed", info.IsClosed),
            
            };

            return parms;
        }
        
        #endregion


        #region Get
        /// <summary>
        /// 获取列表
        /// </summary>
        public List<CustomersInfo> GetList(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            List<CustomersInfo> list = new List<CustomersInfo>();
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

        public List<CustomersInfo> GetList(string StrWhere)
        {
            List<CustomersInfo> list = new List<CustomersInfo>();
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
        /// 获取Info
        /// </summary>
        /// <param name="id">id</param>
        public CustomersInfo Get(int id)
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
        /// 根据登录帐户与客户关系找出客户
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public List<CustomersInfo> GetList(int WorkGroupID)
        {
            List<CustomersInfo> list = new List<CustomersInfo>();
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("SELECT [sys_Customers].*  ").Append(FROM_TABLE);
            strSQL.Append(" where ID in( ");
            strSQL.Append("             SELECT f_CustomerID FROM sys_Brand,sys_WorkGroupBrand WHERE sys_Brand.ID=sys_WorkGroupBrand.f_MID ");
            strSQL.Append("                 AND sys_WorkGroupBrand.f_WorkGroupID=").Append(WorkGroupID);
            strSQL.Append("              ) ");  
            strSQL.Append(" AND f_IsClosed=0 ");
            strSQL.Append(" order by sys_Customers.f_Name ");
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
        /// 检查数据库中名称是否重复
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public CustomersInfo Get(string value)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("SELECT ").Append(ALL_PARM).Append(FROM_TABLE).Append(" WHERE f_Name='").Append(value).Append("' ");
            using (SqlDataReader rdr = SqlHelper.ExecuteReader(SqlHelper.SqlconnString, CommandType.Text, strSQL.ToString(), null))
            {
                if (!rdr.Read()) return null;

                return GetByDataReader(rdr);
            }
        }
        /// <summary>
        /// 根据用户ID 找到对应的客户列表
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public DataTable GetCustomerByUserID(string userID)
        {
            string strSQL = "SELECT DISTINCT f_CustomerName Name,f_CustomerID ID From sys_Customers, sys_User, sys_WorkGroupBrand,sys_Brand WHERE sys_User.ID = " + userID + " AND sys_User.f_WorkGroupID = sys_WorkGroupBrand.f_WorkGroupID AND sys_WorkGroupBrand.f_MID = sys_Brand.ID";
            return SqlHelper.ExecuteReader(strSQL, "0");
        }

        #endregion


        #region set
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="info">info</param>
        public int Add(CustomersInfo info)
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
        public bool Edit(CustomersInfo info)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("update ").Append(TABLE).Append(" set ").Append(UPDATE).Append(" where id = ").Append(info.ID);
            strSQL.Append(UpdateNameSQL(info.ID, info.Name));
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
            strSQL.Append(BrandDAL.DeleteByCustomerIDsql(id.ToString()));
            strSQL.Append(Class1DAL.DeleteByCustomersql(id.ToString()));
            strSQL.Append("delete ").Append(FROM_TABLE).Append(" where id = ").Append(id);

            return SqlHelper.ExecuteNonQueryByTran(SqlHelper.SqlconnString, CommandType.Text, strSQL.ToString(), null);
        }

        /// <summary>
        /// 其新其它表的名称语句
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Name"></param>
        /// <returns></returns>
        private static string UpdateNameSQL(int ID,string Name) 
        {
            StringBuilder sb=new StringBuilder();
            sb.Append(" UPDATE ").Append(BrandDAL.TABLE).Append(" SET f_CustomerName='{0}' WHERE f_CustomerID={1} ");
            sb.Append(" UPDATE ").Append(StoresDAL.TABLE).Append(" SET f_CustomerName='{0}' WHERE f_CustomerID={1} ");
            sb.Append(" UPDATE ").Append(CallDAL.TABLE).Append(" SET f_CustomerName='{0}' WHERE f_CustomerID={1} ");
            return string.Format(sb.ToString(), Name, ID);
        }
        
        #endregion
    }
}