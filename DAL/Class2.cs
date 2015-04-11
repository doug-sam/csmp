using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using DBUtility;
using CSMP.Model;

namespace CSMP.DAL
{
    public class Class2DAL
    {
        private const string ALL_PARM = "  ID,f_Name,f_Class1ID,f_Class1Name,f_IsClosed ";
        private const string FROM_TABLE = " from [sys_Class2] ";
        public const string TABLE = " sys_Class2 ";
        private const string INSET = " (f_Name,f_Class1ID,f_Class1Name,f_IsClosed) values(@Name,@Class1ID,@Class1Name,@IsClosed)  ";
        private const string UPDATE = " f_Name=@Name,f_Class1ID=@Class1ID,f_Class1Name=@Class1Name,f_IsClosed=@IsClosed ";

        #region ReadyData
        private Class2Info GetByDataReader(SqlDataReader rdr)
        {
            Class2Info info = new Class2Info();
            info.ID = Convert.ToInt32(rdr["ID"]);
            info.Name = rdr["f_Name"].ToString();
            info.Class1ID = Convert.ToInt32(rdr["f_Class1ID"]);
            info.Class1Name = rdr["f_Class1Name"].ToString();
            info.IsClosed = Convert.ToBoolean(rdr["f_IsClosed"]);
            
            return info;
        }

        private SqlParameter[] GetParameter(Class2Info info)
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter("@Name", info.Name),
            new SqlParameter("@Class1ID", info.Class1ID),
            new SqlParameter("@Class1Name", info.Class1Name),
            new SqlParameter("@IsClosed", info.IsClosed),
            
            };

            return parms;
        }
        
        #endregion


        #region Get
        /// <summary>
        /// 获取列表
        /// </summary>
        public List<Class2Info> GetList(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            List<Class2Info> list = new List<Class2Info>();
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

        public List<Class2Info> GetList(string StrWhere)
        {
            List<Class2Info> list = new List<Class2Info>();
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
        public Class2Info Get(int id)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("select top 1 ").Append(ALL_PARM).Append(FROM_TABLE).Append(" where id = ").Append(id);

            using (SqlDataReader rdr = SqlHelper.ExecuteReader(SqlHelper.SqlconnString, CommandType.Text, strSQL.ToString(), null))
            {
                if (!rdr.Read()) return null;

                return GetByDataReader(rdr);
            }
        }

        public Class2Info Get(string value, int Class1ID)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("SELECT ").Append(ALL_PARM).Append(FROM_TABLE).Append(" WHERE f_Name='").Append(value).Append("' "); ;
            strSQL.Append(" AND f_Class1ID=").Append(Class1ID);
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
        public int Add(Class2Info info)
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
        public bool Edit(Class2Info info)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("update ").Append(TABLE).Append(" set ").Append(UPDATE).Append(" where id = ").Append(info.ID);
            strSQL.Append(UpdateNameSQL(info.ID, info.Name));

            SqlParameter[] parms = GetParameter(info);

            strSQL.Append(Class3DAL.SetEnableByC2(info.ID.ToString(),info.IsClosed));

            return SqlHelper.ExecuteNonQueryByTran(SqlHelper.SqlconnString, CommandType.Text, strSQL.ToString(), parms);
        }

        /// <summary>
        /// 删除Member
        /// </summary>
        /// <param name="id">Member id</param>
        public bool Delete(int id)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append(Class3DAL.DeleteByC2sql(id.ToString()));
            strSQL.Append("delete ").Append(FROM_TABLE).Append(" where id = ").Append(id);

            return SqlHelper.ExecuteNonQueryByTran(SqlHelper.SqlconnString, CommandType.Text, strSQL.ToString(), null);
        }

        public static StringBuilder DeleteByC1sql(string IDOrSelectIDSQL)
        {
            StringBuilder sb_C2IDs = new StringBuilder();
            sb_C2IDs.Append("select ID").Append(FROM_TABLE).Append(" where f_Class1ID in (").Append(IDOrSelectIDSQL).Append(") ");
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append(Class3DAL.DeleteByC2sql(sb_C2IDs.ToString()));
            strSQL.Append("delete ").Append(FROM_TABLE).Append(" where id in (").Append(sb_C2IDs.ToString()).Append(" ) ");

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
            sb.Append(" UPDATE ").Append(Class3DAL.TABLE).Append(" SET f_Class2Name='{0}' WHERE f_Class2ID={1} ");
            sb.Append(" UPDATE ").Append(CallDAL.TABLE).Append(" SET f_ClassName2='{0}' WHERE f_Class2={1} ");
            sb.Append(" UPDATE ").Append(SolutionDAL.TABLE).Append(" SET f_Class2Name='{0}' WHERE f_Class2={1} ");
            return string.Format(sb.ToString(), Name, ID);
        }

        /// <summary>
        /// 根据大类故障是否禁用，更新中类是否禁用
        /// </summary>
        /// <param name="c1info"></param>
        /// <returns></returns>
        public static string SetEnableByC1(Class1Info c1info)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append(" update ").Append(TABLE).Append(" set f_IsClosed=").Append(c1info.IsClosed ? 1 : 0);
            strSQL.Append(" where f_Class1ID=").Append(c1info.ID);

            string C2ListSQL = "select ID  " + Class2DAL.FROM_TABLE + "where f_Class1ID="+c1info.ID;
            strSQL.Append(Class3DAL.SetEnableByC2(C2ListSQL, c1info.IsClosed));

            return strSQL.ToString();
        }

        #endregion
    }
}