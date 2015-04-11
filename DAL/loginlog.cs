using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using DBUtility;
using CSMP.Model;

namespace CSMP.DAL
{
    public class loginlogDAL
    {
        private const string ALL_PARM = "  ID,f_openid,f_logintime,f_loginlocation,f_logouttime,f_logoutlocation,f_status ";
        private const string FROM_TABLE = " from [wx_loginlog] ";
        private const string TABLE = " wx_loginlog ";
        private const string INSET = " (f_openid,f_logintime,f_loginlocation,f_logouttime,f_logoutlocation,f_status) values(@openid,@logintime,@loginlocation,@logouttime,@logoutlocation,@status)  ";
        private const string UPDATE = " f_openid=@openid,f_logintime=@logintime,f_loginlocation=@loginlocation,f_logouttime=@logouttime,f_logoutlocation=@logoutlocation,f_status=@status ";

        #region ReadyData
        private loginlogInfo GetByDataReader(SqlDataReader rdr)
        {
            loginlogInfo info = new loginlogInfo();
            info.ID = Convert.ToInt32(rdr["ID"]);
            info.openid = rdr["f_openid"].ToString();
            info.logintime = Convert.ToDateTime(rdr["f_logintime"]);
            info.loginlocation = rdr["f_loginlocation"].ToString();
            info.logouttime = Convert.ToDateTime(rdr["f_logouttime"]);
            info.logoutlocation = rdr["f_logoutlocation"].ToString();
            info.status = Convert.ToInt32(rdr["f_status"]);
            
            return info;
        }

        private SqlParameter[] GetParameter(loginlogInfo info)
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter("@openid", info.openid),
            new SqlParameter("@logintime", info.logintime),
            new SqlParameter("@loginlocation", info.loginlocation),
            new SqlParameter("@logouttime", info.logouttime),
            new SqlParameter("@logoutlocation", info.logoutlocation),
            new SqlParameter("@status", info.status),
            
            };

            return parms;
        }
        
        #endregion


        #region Get
        /// <summary>
        /// 获取列表
        /// </summary>
        public List<loginlogInfo> GetList(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            List<loginlogInfo> list = new List<loginlogInfo>();
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

        public List<loginlogInfo> GetList(string StrWhere)
        {
            List<loginlogInfo> list = new List<loginlogInfo>();
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
        public loginlogInfo Get(int id)
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
        public int Add(loginlogInfo info)
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
        public bool Edit(loginlogInfo info)
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