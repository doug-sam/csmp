using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using DBUtility;
using CSMP.Model;

namespace CSMP.DAL
{
    public class Class1DAL
    {
        private const string ALL_PARM = "  ID,f_Name,f_IsClosed,f_CustomerID ";
        private const string FROM_TABLE = " from [sys_Class1] ";
        private const string TABLE = " sys_Class1 ";
        private const string INSET = " (f_Name,f_IsClosed,f_CustomerID) values(@Name,@IsClosed,@CustomerID)  ";
        private const string UPDATE = " f_Name=@Name,f_IsClosed=@IsClosed,f_CustomerID=@CustomerID ";

        #region ReadyData
        private Class1Info GetByDataReader(SqlDataReader rdr)
        {
            Class1Info info = new Class1Info();
            info.ID = Convert.ToInt32(rdr["ID"]);
            info.Name = rdr["f_Name"].ToString();
            info.IsClosed = Convert.ToBoolean(rdr["f_IsClosed"]);
            info.CustomerID = Convert.ToInt32(rdr["f_CustomerID"]);

            return info;
        }

        private SqlParameter[] GetParameter(Class1Info info)
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter("@Name", info.Name),
            new SqlParameter("@IsClosed", info.IsClosed),
            new SqlParameter("@CustomerID", info.CustomerID),
            
            };

            return parms;
        }

        #endregion


        #region Get
        /// <summary>
        /// 获取列表
        /// </summary>
        public List<Class1Info> GetList(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            List<Class1Info> list = new List<Class1Info>();
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

        public List<Class1Info> GetList(string StrWhere)
        {
            List<Class1Info> list = new List<Class1Info>();
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
        public Class1Info Get(int id)
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
        /// 检查数据库中名称是否重复
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Class1Info Get(string value, int CustomerID)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("SELECT   ").Append(ALL_PARM).Append(FROM_TABLE).Append(" WHERE f_Name='").Append(value).Append("' ");
            strSQL.Append(" AND f_CustomerID=").Append(CustomerID);
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
        public int Add(Class1Info info)
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
        public bool Edit(Class1Info info)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("update ").Append(TABLE).Append(" set ").Append(UPDATE).Append(" where id = ").Append(info.ID);
            strSQL.Append(UpdateNameSQL(info.ID, info.Name));
            strSQL.Append(Class2DAL.SetEnableByC1(info));
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
            strSQL.Append(Class2DAL.DeleteByC1sql(id.ToString()));
            strSQL.Append("delete ").Append(FROM_TABLE).Append(" where id = ").Append(id);

            return SqlHelper.ExecuteNonQueryByTran(SqlHelper.SqlconnString, CommandType.Text, strSQL.ToString(), null);
        }

        public static StringBuilder DeleteByCustomersql(string IDOrSelectIDSQL)
        {
            StringBuilder sb_C1IDs = new StringBuilder();
            sb_C1IDs.Append("select ID").Append(FROM_TABLE).Append(" where f_CustomerID in (").Append(IDOrSelectIDSQL).Append(") ");
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append(Class2DAL.DeleteByC1sql(sb_C1IDs.ToString()));
            strSQL.Append("delete ").Append(FROM_TABLE).Append(" where id in (").Append(sb_C1IDs.ToString()).Append(" ) ");

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
            sb.Append(" UPDATE ").Append(Class2DAL.TABLE).Append(" SET f_Class1Name='{0}' WHERE f_Class1ID={1} ");
            sb.Append(" UPDATE ").Append(CallDAL.TABLE).Append(" SET f_ClassName1='{0}' WHERE f_Class1={1} ");
            sb.Append(" UPDATE ").Append(SolutionDAL.TABLE).Append(" SET f_Class1Name='{0}' WHERE f_Class1={1} ");
            
            return string.Format(sb.ToString(), Name, ID);
        }

        #endregion
    }
}