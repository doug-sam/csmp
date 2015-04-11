using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using DBUtility;
using CSMP.Model;

namespace CSMP.DAL
{
    public class ProvincesDAL
    {
        private const string ALL_PARM = "  ID,f_Name,f_IsClosed ";
        private const string FROM_TABLE = " from [sys_Provinces] ";
        private const string TABLE = " sys_Provinces ";
        private const string INSET = " (f_Name,f_IsClosed) values(@Name,@IsClosed)  ";
        private const string UPDATE = " f_Name=@Name,f_IsClosed=@IsClosed ";

        #region ReadyData
        private ProvincesInfo GetByDataReader(SqlDataReader rdr)
        {
            ProvincesInfo info = new ProvincesInfo();
            info.ID = Convert.ToInt32(rdr["ID"]);
            info.Name = rdr["f_Name"].ToString();
            info.IsClosed = Convert.ToBoolean(rdr["f_IsClosed"]);
            
            return info;
        }

        private SqlParameter[] GetParameter(ProvincesInfo info)
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter("@Name", info.Name),
            new SqlParameter("@IsClosed", info.IsClosed),
            
            };

            return parms;
        }
        
        #endregion


        #region Get
        /// <summary>
        /// 获取列表
        /// </summary>
        public List<ProvincesInfo> GetList(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            List<ProvincesInfo> list = new List<ProvincesInfo>();
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

        public List<ProvincesInfo> GetList(string StrWhere)
        {
            List<ProvincesInfo> list = new List<ProvincesInfo>();
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
        public ProvincesInfo Get(int id)
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
        public int Add(ProvincesInfo info)
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
        public bool Edit(ProvincesInfo info)
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

            strSQL.Append("delete ").Append(FROM_TABLE).Append(" where id = ").Append(id);

            return SqlHelper.ExecuteNonQueryByTran(SqlHelper.SqlconnString, CommandType.Text, strSQL.ToString(), null);
        }

        /// <summary>
        /// 其新其它表的名称语句
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Name"></param>
        /// <returns></returns>
        private static string UpdateNameSQL(int ID, string Name)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" UPDATE ").Append(WorkGroupDAL.TABLE).Append(" SET f_ProvinceName='{0}' WHERE f_ProvinceID={1} ");
            sb.Append(" UPDATE ").Append(StoresDAL.TABLE).Append(" SET f_ProvinceName='{0}' WHERE f_ProvinceID={1} ");
            sb.Append(" UPDATE ").Append(CallDAL.TABLE).Append(" SET f_ProvinceName='{0}' WHERE f_ProvinceID={1} ");
            

            return string.Format(sb.ToString(), Name, ID);
        }

        
        #endregion
    }
}