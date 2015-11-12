using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using DBUtility;
using CSMP.Model;

namespace CSMP.DAL
{
    public class APPLoginLogoutLogDAL
    {
        private const string ALL_PARM = "  ID,f_openid,f_logintime,f_loginlocation,f_logouttime,f_logoutlocation,f_status ";
        private const string FROM_TABLE = " from [wx_loginlog] ";
        private const string TABLE = " wx_loginlog ";
        private const string INSET = " (f_openid,f_logintime,f_loginlocation,f_logouttime,f_logoutlocation,f_status) values(@OpenID,@LoginTime,@LoginLocation,@LogoutTime,@LogoutLocation,@Status)  ";
        private const string UPDATE = " f_openid=@OpenID,f_logintime=@LoginTime,f_loginlocation=@LoginLocation,f_logouttime=@LogoutTime,f_logoutlocation=@LogoutLocation,f_status=@Status ";

        #region ReadyData
        private APPLoginLogoutLog GetByDataReader(SqlDataReader rdr)
        {
            APPLoginLogoutLog info = new APPLoginLogoutLog();
            info.ID = Convert.ToInt32(rdr["ID"]);
            info.OpenID = rdr["f_openid"].ToString();
            info.LoginLocation = rdr["f_loginlocation"].ToString();
            if (!string.IsNullOrEmpty(rdr["f_logintime"].ToString()))
            {
                info.LoginTime=Convert.ToDateTime(rdr["f_logintime"]);
            }
            
            info.LogoutLocation = rdr["f_logoutlocation"].ToString();
            if(!string.IsNullOrEmpty(rdr["f_logouttime"].ToString()))
            {
                info.LogoutTime=Convert.ToDateTime(rdr["f_logouttime"]);
            }
            
            info.Status = Convert.ToInt32(rdr["f_status"]);

            return info;
        }

        private SqlParameter[] GetParameter(APPLoginLogoutLog info)
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter("@OpenID", info.OpenID),
            new SqlParameter("@LoginLocation", info.LoginLocation),
            new SqlParameter("@LoginTime", info.LoginTime),
            new SqlParameter("@LogoutLocation", info.LogoutLocation),
            new SqlParameter("@LogoutTime", info.LogoutTime),
            new SqlParameter("@Status", info.Status),
            
            };

            return parms;
        }

        #endregion


        #region Get
        public List<APPLoginLogoutLog> GetList(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            List<APPLoginLogoutLog> list = new List<APPLoginLogoutLog>();
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

        public List<APPLoginLogoutLog> GetList(string StrWhere)
        {
            List<APPLoginLogoutLog> list = new List<APPLoginLogoutLog>();
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("select ").Append(ALL_PARM).Append(FROM_TABLE);
            strSQL.Append(" where ").Append(" 1=1 ").Append(StrWhere.Replace("1=1", " "));
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
        public APPLoginLogoutLog Get(int id)
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
        public APPLoginLogoutLog Get(string OpenID)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("select top 1 ").Append(ALL_PARM).Append(FROM_TABLE).Append(" where f_openid = '").Append(OpenID).Append("' order by ID DESC");

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
        public int Add(APPLoginLogoutLog info)
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
        /// 使用SP打卡，牵出、签退
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="loginMode">打卡类型 1为签到，0为签退</param>
        /// <param name="loginTime">打卡时间</param>
        /// <param name="location">打卡地点</param>
        /// <returns></returns>
        public string AddBySP(string userID,string loginMode,string loginTime,string location)
        {
            StoreProcedure sp = new StoreProcedure("sp_APP_Loginlogout");//类的对象
            Object[] paraValues = new object[5];//注意,这里是存储过程中全部的参数,一共有三个,还要注意顺序啊,返回值是第一个,那么赋值时第一个参数就为空

            paraValues[0] = userID;//从第二个参数开始赋值
            paraValues[1] = loginMode;
            paraValues[2] = loginTime;
            paraValues[3] = location;
            paraValues[4] = "";
            object[] output;
            sp.ExecProcOutput(out  output, 2,paraValues);
            if (output != null)
            {
                return output[1].ToString();
            }
            else {
                return "";
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="info">info</param>
        public bool Edit(APPLoginLogoutLog info)
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
