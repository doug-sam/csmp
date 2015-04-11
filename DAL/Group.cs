using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using DBUtility;
using CSMP.Model;

namespace CSMP.DAL
{
    public class GroupDAL
    {
        private const string ALL_PARM = "  ID,f_Name,f_LeaderID,f_LeaderName,f_Enable,f_PowerList,f_CityID,f_CityName,f_Rule,f_ItemList,f_ItemList2 ";
        private const string FROM_TABLE = " from [sys_Group] ";
        private const string TABLE = " sys_Group ";
        private const string INSET = " (f_Name,f_LeaderID,f_LeaderName,f_Enable,f_PowerList,f_CityID,f_CityName,f_Rule,f_ItemList,f_ItemList2) values(@Name,@LeaderID,@LeaderName,@Enable,@PowerList,@CityID,@CityName,@Rule,@ItemList,@ItemList2)  ";
        private const string UPDATE = " f_Name=@Name,f_LeaderID=@LeaderID,f_LeaderName=@LeaderName,f_Enable=@Enable,f_PowerList=@PowerList,f_CityID=@CityID,f_CityName=@CityName,f_Rule=@Rule,f_ItemList=@ItemList,f_ItemList2=@ItemList2 ";

        #region ReadyData
        private GroupInfo GetByDataReader(SqlDataReader rdr)
        {
            GroupInfo info = new GroupInfo();
            info.ID = Convert.ToInt32(rdr["ID"]);
            info.Name = rdr["f_Name"].ToString();
            info.LeaderID = Convert.ToInt32(rdr["f_LeaderID"]);
            info.LeaderName = rdr["f_LeaderName"].ToString();
            info.Enable = Convert.ToBoolean(rdr["f_Enable"]);
            info.PowerList = rdr["f_PowerList"].ToString();
            info.CityID = Convert.ToInt32(rdr["f_CityID"]);
            info.CityName = rdr["f_CityName"].ToString();
            info.Rule = new List<string>();
            string Rule = rdr["f_Rule"].ToString().Trim().Trim(',');
            foreach (string item in Rule.Split(','))
            {
                if (string.IsNullOrEmpty(item))
                {
                    continue;
                }
                info.Rule.Add(item);
            }
            info.ItemList = rdr["f_ItemList"].ToString();
            info.ItemList2 = rdr["f_ItemList2"].ToString();
            return info;
        }

        private SqlParameter[] GetParameter(GroupInfo info)
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter("@Name", info.Name),
            new SqlParameter("@LeaderID", info.LeaderID),
            new SqlParameter("@LeaderName", info.LeaderName),
            new SqlParameter("@Enable", info.Enable),
            new SqlParameter("@PowerList", info.PowerList),
            new SqlParameter("@CityID", info.CityID),
            new SqlParameter("@CityName", info.CityName),
            new SqlParameter("@Rule",","+string.Join(",",info.Rule.ToArray()).Trim(',')+","),
            new SqlParameter("@ItemList", info.ItemList),
            new SqlParameter("@ItemList2", info.ItemList2),
            
            };

            return parms;
        }
        
        #endregion


        #region Get
        /// <summary>
        /// 获取列表
        /// </summary>
        public List<GroupInfo> GetList(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            List<GroupInfo> list = new List<GroupInfo>();
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

        public List<GroupInfo> GetList(string StrWhere)
        {
            List<GroupInfo> list = new List<GroupInfo>();
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
        public GroupInfo Get(int id)
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
        public int Add(GroupInfo info)
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
        public bool Edit(GroupInfo info)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("update ").Append(TABLE).Append(" set ").Append(UPDATE).Append(" where id = ").Append(info.ID);
            strSQL.Append(" UPDATE sys_User set f_Rule=@Rule ");
            strSQL.Append(" where f_PowerGroupID= ").Append(info.ID);

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