using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using DBUtility;
using CSMP.Model;

namespace CSMP.DAL
{
    public class Class3DAL
    {
        private const string ALL_PARM = "  ID,f_Name,f_Class2ID,f_Class2Name,f_PriorityID,f_PriorityName,f_SLA,f_IsClosed ";
        private const string FROM_TABLE = " from [sys_Class3] ";
        public const string TABLE = " sys_Class3 ";
        private const string INSET = " (f_Name,f_Class2ID,f_Class2Name,f_PriorityID,f_PriorityName,f_SLA,f_IsClosed) values(@Name,@Class2ID,@Class2Name,@PriorityID,@PriorityName,@SLA,@IsClosed)  ";
        private const string UPDATE = " f_Name=@Name,f_Class2ID=@Class2ID,f_Class2Name=@Class2Name,f_PriorityID=@PriorityID,f_PriorityName=@PriorityName,f_SLA=@SLA,f_IsClosed=@IsClosed ";

        #region ReadyData
        private Class3Info GetByDataReader(SqlDataReader rdr)
        {
            Class3Info info = new Class3Info();
            info.ID = Convert.ToInt32(rdr["ID"]);
            info.Name = rdr["f_Name"].ToString();
            info.Class2ID = Convert.ToInt32(rdr["f_Class2ID"]);
            info.Class2Name = rdr["f_Class2Name"].ToString();
            info.PriorityID = Convert.ToInt32(rdr["f_PriorityID"]);
            info.PriorityName = rdr["f_PriorityName"].ToString();
            info.SLA = Convert.ToInt32(rdr["f_SLA"]);
            info.IsClosed = Convert.ToBoolean(rdr["f_IsClosed"]);
            
            return info;
        }

        private SqlParameter[] GetParameter(Class3Info info)
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter("@Name", info.Name),
            new SqlParameter("@Class2ID", info.Class2ID),
            new SqlParameter("@Class2Name", info.Class2Name),
            new SqlParameter("@PriorityID", info.PriorityID),
            new SqlParameter("@PriorityName", info.PriorityName),
            new SqlParameter("@SLA", info.SLA),
            new SqlParameter("@IsClosed", info.IsClosed),
            
            };

            return parms;
        }
        
        #endregion


        #region Get
        /// <summary>
        /// 获取列表
        /// </summary>
        public List<Class3Info> GetList(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            List<Class3Info> list = new List<Class3Info>();
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

        public List<Class3Info> GetList(string StrWhere)
        {
            List<Class3Info> list = new List<Class3Info>();
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
        public Class3Info Get(int id)
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
        /// </summary
        /// </summary>
        /// <param name="value"></param>
        /// <param name="Class2ID"></param>
        /// <returns></returns>
        public Class3Info Get(string value, int Class2ID)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("SELECT ").Append(ALL_PARM).Append(FROM_TABLE).Append(" WHERE f_Name='").Append(value).Append("' ");
            strSQL.Append(" AND f_Class2ID=").Append(Class2ID);
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
        public int Add(Class3Info info)
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
        public bool Edit(Class3Info info)
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
            strSQL.Append(CallDAL.DeleteByClass3IDsql(id.ToString()));
            return SqlHelper.ExecuteNonQueryByTran(SqlHelper.SqlconnString, CommandType.Text, strSQL.ToString(), null);
        }

        public static StringBuilder DeleteByC2sql(string IDOrSelectIDSQL)
        {
            StringBuilder sb_C3IDs = new StringBuilder();
            sb_C3IDs.Append("select ID").Append(FROM_TABLE).Append(" where f_Class2ID in (").Append(IDOrSelectIDSQL).Append(") ");
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append(SolutionDAL.DeleteByC3sql(sb_C3IDs.ToString()));
            strSQL.Append(CallDAL.DeleteByClass3IDsql(sb_C3IDs.ToString()));
            strSQL.Append("delete ").Append(FROM_TABLE).Append(" where id in (").Append(sb_C3IDs.ToString()).Append(" ) ");

            return strSQL;
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
            sb.Append(" UPDATE ").Append(CallDAL.TABLE).Append(" SET f_ClassName3='{0}' WHERE f_Class3={1} ");
            sb.Append(" UPDATE ").Append(SolutionDAL.TABLE).Append(" SET f_Class3Name='{0}' WHERE f_Class3={1} ");
            return string.Format(sb.ToString(), Name, ID);
        }


        /// <summary>
        /// 根据中类故障是否禁用，更新小类是否禁用
        /// </summary>
        /// <param name="Class2ID"></param>
        /// <param name="Enable"></param>
        /// <returns></returns>
        public static string SetEnableByC2(string Class2IDList,bool IsClosed)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append(" update ").Append(TABLE).Append(" set f_IsClosed=").Append(IsClosed ? 1 : 0);
            strSQL.Append(" where f_Class2ID in(").Append(Class2IDList).Append(") ");
            return strSQL.ToString();

        }


        #endregion
    }
}