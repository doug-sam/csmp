using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using DBUtility;
using CSMP.Model;

namespace CSMP.DAL
{
    public class TrunkNO
    {
        private const string ALL_PARM = "  ID,f_PhysicalNo,f_VirtualNo,f_Description ";
        private const string FROM_TABLE = " from [sys_TrunkNO] ";
        private const string TABLE = " sys_TrunkNO ";
        private const string INSET = " (f_PhysicalNo,f_VirtualNo,f_Description) values(@PhysicalNo,@VirtualNo,@Description)  ";
        private const string UPDATE = " f_PhysicalNo=@PhysicalNo,f_VirtualNo=@VirtualNo,f_Description=@Description ";

        #region ReadyData
        private TrunkNOInfo GetByDataReader(SqlDataReader rdr)
        {
            TrunkNOInfo info = new TrunkNOInfo();
            info.ID = Convert.ToInt32(rdr["ID"]);
            info.PhysicalNo = rdr["f_PhysicalNo"].ToString();
            info.VirtualNo =  rdr["f_VirtualNo"].ToString();
            info.Description = rdr["f_Description"].ToString();

            return info;
        }

        private SqlParameter[] GetParameter(TrunkNOInfo info)
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter("@PhysicalNo", info.PhysicalNo),
            new SqlParameter("@VirtualNo", info.VirtualNo),
            new SqlParameter("@Description", info.Description),
            
            };

            return parms;
        }

        #endregion


        #region Get
        /// <summary>
        /// 获取列表
        /// </summary>
        public List<TrunkNOInfo> GetList(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            List<TrunkNOInfo> list = new List<TrunkNOInfo>();
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

        
        /// <summary>
        /// 获取Info
        /// </summary>
        /// <param name="id">id</param>
        public TrunkNOInfo Get(int id)
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
        /// 更具物理号码获取Info
        /// </summary>
        /// <param name="id">id</param>
        public TrunkNOInfo Get(string PhysicalNo)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("select top 1 ").Append(ALL_PARM).Append(FROM_TABLE).Append(" where f_PhysicalNo = '").Append(PhysicalNo).Append("' ");

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
        public int Add(TrunkNOInfo info)
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
        public bool Edit(TrunkNOInfo info)
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
