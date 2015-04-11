using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using DBUtility;
using CSMP.Model;

namespace CSMP.DAL
{
    public class ManufacturerDAL
    {
        private const string ALL_PARM = "  ID,f_Name,f_PassWord,f_Tel,f_ProvinceID,f_CityID,f_WorkGroupID,f_Type,f_Enable ";
        private const string FROM_TABLE = " from [sys_Manufacturer] ";
        private const string TABLE = " sys_Manufacturer ";
        private const string INSET = " (f_Name,f_PassWord,f_Tel,f_ProvinceID,f_CityID,f_WorkGroupID,f_Type,f_Enable) values(@Name,@PassWord,@Tel,@ProvinceID,@CityID,@WorkGroupID,@Type,@Enable)  ";
        private const string UPDATE = " f_Name=@Name,f_PassWord=@PassWord,f_Tel=@Tel,f_ProvinceID=@ProvinceID,f_CityID=@CityID,f_WorkGroupID=@WorkGroupID,f_Type=@Type,f_Enable=@Enable ";

        #region ReadyData
        private ManufacturerInfo GetByDataReader(SqlDataReader rdr)
        {
            ManufacturerInfo info = new ManufacturerInfo();
            info.ID = Convert.ToInt32(rdr["ID"]);
            info.Name = rdr["f_Name"].ToString();
            info.PassWord = rdr["f_PassWord"].ToString();
            info.Tel = rdr["f_Tel"].ToString();
            info.ProvinceID = Convert.ToInt32(rdr["f_ProvinceID"]);
            info.CityID = Convert.ToInt32(rdr["f_CityID"]);
            info.WorkGroupID = Convert.ToInt32(rdr["f_WorkGroupID"]);
            info.Type = rdr["f_Type"].ToString();
            info.Enable = Convert.ToBoolean(rdr["f_Enable"]);
            
            return info;
        }

        private SqlParameter[] GetParameter(ManufacturerInfo info)
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter("@Name", info.Name),
            new SqlParameter("@PassWord", info.PassWord),
            new SqlParameter("@Tel", info.Tel),
            new SqlParameter("@ProvinceID", info.ProvinceID),
            new SqlParameter("@CityID", info.CityID),
            new SqlParameter("@WorkGroupID", info.WorkGroupID),
            new SqlParameter("@Type", info.Type),
            new SqlParameter("@Enable", info.Enable),
            
            };

            return parms;
        }
        
        #endregion


        #region Get
        /// <summary>
        /// 获取列表
        /// </summary>
        public List<ManufacturerInfo> GetList(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            List<ManufacturerInfo> list = new List<ManufacturerInfo>();
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

        public List<ManufacturerInfo> GetList(string StrWhere)
        {
            List<ManufacturerInfo> list = new List<ManufacturerInfo>();
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
        public ManufacturerInfo Get(int id)
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
        public int Add(ManufacturerInfo info)
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
        public bool Edit(ManufacturerInfo info)
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