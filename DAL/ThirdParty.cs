using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using DBUtility;
using CSMP.Model;

namespace CSMP.DAL
{
    public class ThirdPartyDAL
    {
        private const string ALL_PARM = "  ID,f_WorkGroupID,f_Name,f_Contact,f_OrderNo,f_Enable ";
        private const string FROM_TABLE = " from [sys_ThirdParty] ";
        private const string TABLE = " sys_ThirdParty ";
        private const string INSET = " (f_WorkGroupID,f_Name,f_Contact,f_OrderNo,f_Enable) values(@WorkGroupID,@Name,@Contact,@OrderNo,@Enable)  ";
        private const string UPDATE = " f_WorkGroupID=@WorkGroupID,f_Name=@Name,f_Contact=@Contact,f_OrderNo=@OrderNo,f_Enable=@Enable ";

        #region ReadyData
        private ThirdPartyInfo GetByDataReader(SqlDataReader rdr)
        {
            ThirdPartyInfo info = new ThirdPartyInfo();
            info.ID = Convert.ToInt32(rdr["ID"]);
            info.WorkGroupID = Convert.ToInt32(rdr["f_WorkGroupID"]);
            info.Name = rdr["f_Name"].ToString();
            info.Contact = rdr["f_Contact"].ToString();
            info.OrderNo = Convert.ToInt32(rdr["f_OrderNo"]);
            info.Enable = Convert.ToBoolean(rdr["f_Enable"]);
            
            return info;
        }

        private SqlParameter[] GetParameter(ThirdPartyInfo info)
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter("@WorkGroupID", info.WorkGroupID),
            new SqlParameter("@Name", info.Name),
            new SqlParameter("@Contact", info.Contact),
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
        public List<ThirdPartyInfo> GetList(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            List<ThirdPartyInfo> list = new List<ThirdPartyInfo>();
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

        public List<ThirdPartyInfo> GetList(string StrWhere)
        {
            List<ThirdPartyInfo> list = new List<ThirdPartyInfo>();
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
        public ThirdPartyInfo Get(int id)
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
        public int Add(ThirdPartyInfo info)
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
        public bool Edit(ThirdPartyInfo info)
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