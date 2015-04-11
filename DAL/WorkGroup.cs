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
    public class WorkGroupDAL
    {
        private const string ALL_PARM = "  ID,f_Name,f_ProvinceID,f_ProvinceName,f_CityID,f_CityName,f_Enable,f_Type ";
        private const string FROM_TABLE = " from [sys_WorkGroup] ";
        public const string TABLE = " sys_WorkGroup ";
        private const string INSET = " (f_Name,f_ProvinceID,f_ProvinceName,f_CityID,f_CityName,f_Enable,f_Type) values(@Name,@ProvinceID,@ProvinceName,@CityID,@CityName,@Enable,@Type)  ";
        private const string UPDATE = " f_Name=@Name,f_ProvinceID=@ProvinceID,f_ProvinceName=@ProvinceName,f_CityID=@CityID,f_CityName=@CityName,f_Enable=@Enable,f_Type=@Type ";

        #region ReadyData
        private WorkGroupInfo GetByDataReader(SqlDataReader rdr)
        {
            WorkGroupInfo info = new WorkGroupInfo();
            info.ID = Convert.ToInt32(rdr["ID"]);
            info.Name = rdr["f_Name"].ToString();
            info.ProvinceID = Convert.ToInt32(rdr["f_ProvinceID"]);
            info.ProvinceName = rdr["f_ProvinceName"].ToString();
            info.CityID = Convert.ToInt32(rdr["f_CityID"]);
            info.CityName = rdr["f_CityName"].ToString();
            info.Enable = Convert.ToBoolean(rdr["f_Enable"]);
            info.Type = Convert.ToInt32(rdr["f_Type"]);
            
            return info;
        }

        private SqlParameter[] GetParameter(WorkGroupInfo info)
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter("@Name", info.Name),
            new SqlParameter("@ProvinceID", info.ProvinceID),
            new SqlParameter("@ProvinceName", info.ProvinceName),
            new SqlParameter("@CityID", info.CityID),
            new SqlParameter("@CityName", info.CityName),
            new SqlParameter("@Enable", info.Enable),
            new SqlParameter("@Type", info.Type),
            
            };

            return parms;
        }
        
        #endregion


        #region Get
        /// <summary>
        /// 获取列表
        /// </summary>
        public List<WorkGroupInfo> GetList(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            List<WorkGroupInfo> list = new List<WorkGroupInfo>();
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

        public List<WorkGroupInfo> GetList(int Province)
        {
            List<WorkGroupInfo> list = new List<WorkGroupInfo>();
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("select ").Append(ALL_PARM).Append(FROM_TABLE);
            if (Province > 0) strSQL.Append(" where f_ProvinceID=").Append(Province);
            using (SqlDataReader rdr = SqlHelper.ExecuteReader(SqlHelper.SqlconnString, CommandType.Text, strSQL.ToString(), null))
            {
                while (rdr.Read())
                {
                    list.Add(GetByDataReader(rdr));
                }
            }
            return list;
        }
        public List<WorkGroupInfo> GetList(int Province,SysEnum.WorkGroupType WorkGroupType)
        {
            List<WorkGroupInfo> list = new List<WorkGroupInfo>();
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("select ").Append(ALL_PARM).Append(FROM_TABLE);
            if (Province > 0) strSQL.Append(" where f_ProvinceID=").Append(Province);
            strSQL.Append(" and f_Type=").Append((int)WorkGroupType);
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
        public WorkGroupInfo Get(int id)
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
        /// 根据关系找出组
        /// </summary>
        /// <param name="MID">关系id</param>
        /// <param name="TrueIn_FalseOut">true为所有有关系，false为所有无关系的记录</param>
        /// <returns></returns>
        public List<WorkGroupInfo> GetList(int MID, bool TrueIn_FalseOut)
        {
            List<WorkGroupInfo> list = new List<WorkGroupInfo>();
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("select ").Append(ALL_PARM).Append(FROM_TABLE).Append(" where ID");
            strSQL.Append(TrueIn_FalseOut ? " IN " : " NOT IN ");
            strSQL.Append("(select f_WorkGroupID from sys_WorkGroupBrand  WHERE f_MID=").Append(MID).Append(")");
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
        public int Add(WorkGroupInfo info)
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
        public bool Edit(WorkGroupInfo info)
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