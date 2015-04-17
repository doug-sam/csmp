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
    public class UserDAL
    {
        private const string ALL_PARM = "  ID,f_Name,f_Code,f_PassWord,f_Email,f_Tel,f_Phone,f_Sex,f_CityID,f_CreateDate,f_LastDate,f_Enable,f_WorkGroupID,f_PowerGroupID,f_Rule ";
        private const string FROM_TABLE = " from [sys_User] ";
        private const string TABLE = " sys_User ";
        private const string INSET = " (f_Name,f_Code,f_PassWord,f_Email,f_Tel,f_Phone,f_Sex,f_CityID,f_CreateDate,f_LastDate,f_Enable,f_WorkGroupID,f_PowerGroupID,f_Rule) values(@Name,@Code,@PassWord,@Email,@Tel,@Phone,@Sex,@CityID,@CreateDate,@LastDate,@Enable,@WorkGroupID,@PowerGroupID,@Rule)  ";
        private const string UPDATE = " f_Name=@Name,f_Code=@Code,f_PassWord=@PassWord,f_Email=@Email,f_Tel=@Tel,f_Phone=@Phone,f_Sex=@Sex,f_CityID=@CityID,f_CreateDate=@CreateDate,f_LastDate=@LastDate,f_Enable=@Enable,f_WorkGroupID=@WorkGroupID,f_PowerGroupID=@PowerGroupID,f_Rule=@Rule ";

        #region ReadyData
        private UserInfo GetByDataReader(SqlDataReader rdr)
        {
            UserInfo info = new UserInfo();
            info.ID = Convert.ToInt32(rdr["ID"]);
            info.Name = rdr["f_Name"].ToString();
            info.Code = rdr["f_Code"].ToString();
            info.PassWord = rdr["f_PassWord"].ToString();
            info.Email = rdr["f_Email"].ToString();
            info.Tel = rdr["f_Tel"].ToString();
            info.Phone = rdr["f_Phone"].ToString();
            info.Sex = Convert.ToBoolean(rdr["f_Sex"]);
            info.CityID = Convert.ToInt32(rdr["f_CityID"]);
            info.CreateDate = Convert.ToDateTime(rdr["f_CreateDate"]);
            info.LastDate = Convert.ToDateTime(rdr["f_LastDate"]);
            info.Enable = Convert.ToBoolean(rdr["f_Enable"]);
            info.WorkGroupID = Convert.ToInt32(rdr["f_WorkGroupID"]);
            info.PowerGroupID = Convert.ToInt32(rdr["f_PowerGroupID"]);
            info.Rule = new List<string>();
            string Rule= rdr["f_Rule"].ToString().Trim().Trim(',');
            foreach (string item in Rule.Split(','))
            {
                if (string.IsNullOrEmpty(item))
                {
                    continue;   
                }
                info.Rule.Add(item);
            }
            return info;
        }

        private SqlParameter[] GetParameter(UserInfo info)
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter("@Name", info.Name),
            new SqlParameter("@Code", info.Code),
            new SqlParameter("@PassWord", info.PassWord),
            new SqlParameter("@Email", info.Email),
            new SqlParameter("@Tel", info.Tel),
            new SqlParameter("@Phone", info.Phone),
            new SqlParameter("@Sex", info.Sex),
            new SqlParameter("@CityID", info.CityID),
            new SqlParameter("@CreateDate", info.CreateDate),
            new SqlParameter("@LastDate", info.LastDate),
            new SqlParameter("@Enable", info.Enable),
            new SqlParameter("@WorkGroupID", info.WorkGroupID),
            new SqlParameter("@PowerGroupID", info.PowerGroupID),
            new SqlParameter("@Rule",","+string.Join(",",info.Rule.ToArray()).Trim(',')+","),
            };

            return parms;
        }

        #endregion



        #region Get
        /// <summary>
        /// 获取列表
        /// </summary>
        public List<UserInfo> GetList(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            List<UserInfo> list = new List<UserInfo>();
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

        public List<UserInfo> GetList(string StrWhere)
        {
            List<UserInfo> list = new List<UserInfo>();
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("select ").Append(ALL_PARM).Append(FROM_TABLE);
            strSQL.Append(" where ").Append(" 1=1 AND f_Enable=1 ").Append(StrWhere.Replace("1=1"," "));

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
        /// 跟指定品牌有关的角色
        /// </summary>
        /// <param name="Rule"></param>
        /// <param name="BrandID"></param>
        /// <returns></returns>
        public List<UserInfo> GetListByBrand(string Rule, int BrandID)
        {
            List<UserInfo> list = new List<UserInfo>();
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("select ").Append(ALL_PARM).Append(FROM_TABLE).Append(" where 1=1 ");
            if (!string.IsNullOrEmpty(Rule))
            {
                strSQL.Append(" and f_Rule like '%").Append(Rule).Append("%' ");
            }
            if (BrandID > 0)
            {
                strSQL.Append(" and f_WorkGroupID in( ");
                strSQL.Append("                         SELECT DISTINCT f_WorkGroupID ").Append(WorkGroupBrandDAL.FROM_TABLE);
                strSQL.Append("                             WHERE f_MID=").Append(BrandID);
                strSQL.Append("                      ) ");

            }
            strSQL.Append(" AND f_Enable=1 ");
            strSQL.Append(" order by f_WorkGroupID ");
            using (SqlDataReader rdr = SqlHelper.ExecuteReader(SqlHelper.SqlconnString, CommandType.Text, strSQL.ToString(), null))
            {
                while (rdr.Read())
                {
                    list.Add(GetByDataReader(rdr));
                }
            }
            return list;
        }

        public List<UserInfo> GetList(string Rule, int WorkGroupID)
        {
            List<UserInfo> list = new List<UserInfo>();
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("select ").Append(ALL_PARM).Append(FROM_TABLE).Append(" where 1=1 ");
            if (!string.IsNullOrEmpty(Rule))
            {
                strSQL.Append(" and f_Rule like '%").Append(Rule).Append("%' ");
            }
            if (WorkGroupID > 0)
            {
                strSQL.Append(" and f_WorkGroupID=").Append(WorkGroupID).Append("  ");
            }
            strSQL.Append(" AND f_Enable=1 order by f_Name ASC ");
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
        public UserInfo Get(int id)
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
        public UserInfo Get(string NameOrEmail)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("select top 1 ").Append(ALL_PARM).Append(FROM_TABLE).Append(" where f_Code = '").Append(NameOrEmail).Append("' or f_Email='").Append(NameOrEmail).Append("'");

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
        public int Add(UserInfo info)
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
        public bool Edit(UserInfo info)
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