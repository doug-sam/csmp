using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using DBUtility;
using CSMP.Model;

namespace CSMP.DAL
{
    public class SolutionDAL
    {
        private const string ALL_PARM = "  ID,f_Class3,f_Class2,f_Class1,f_Class3Name,f_Class2Name,f_Class1Name,f_EnableFlag,f_SuggestCount,f_SolveCount,f_Name,f_EnableBy ";
        private const string FROM_TABLE = " from [sys_Solution] ";
        public const string TABLE = " sys_Solution ";
        private const string INSET = " (f_Class3,f_Class2,f_Class1,f_Class3Name,f_Class2Name,f_Class1Name,f_EnableFlag,f_SuggestCount,f_SolveCount,f_Name,f_EnableBy) values(@Class3,@Class2,@Class1,@Class3Name,@Class2Name,@Class1Name,@EnableFlag,@SuggestCount,@SolveCount,@Name,@EnableBy)  ";
        private const string UPDATE = " f_Class3=@Class3,f_Class2=@Class2,f_Class1=@Class1,f_Class3Name=@Class3Name,f_Class2Name=@Class2Name,f_Class1Name=@Class1Name,f_EnableFlag=@EnableFlag,f_SuggestCount=@SuggestCount,f_SolveCount=@SolveCount,f_Name=@Name,f_EnableBy=@EnableBy ";

        #region ReadyData
        private SolutionInfo GetByDataReader(SqlDataReader rdr)
        {
            SolutionInfo info = new SolutionInfo();
            info.ID = Convert.ToInt32(rdr["ID"]);
            info.Class3 = Convert.ToInt32(rdr["f_Class3"]);
            info.Class2 = Convert.ToInt32(rdr["f_Class2"]);
            info.Class1 = Convert.ToInt32(rdr["f_Class1"]);
            info.Class3Name = rdr["f_Class3Name"].ToString();
            info.Class2Name = rdr["f_Class2Name"].ToString();
            info.Class1Name = rdr["f_Class1Name"].ToString();
            info.EnableFlag = Convert.ToBoolean(rdr["f_EnableFlag"]);
            info.SuggestCount = Convert.ToInt32(rdr["f_SuggestCount"]);
            info.SolveCount = Convert.ToInt32(rdr["f_SolveCount"]);
            info.Name = rdr["f_Name"].ToString();
            info.EnableBy = rdr["f_EnableBy"].ToString();
            
            return info;
        }

        private SqlParameter[] GetParameter(SolutionInfo info)
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter("@Class3", info.Class3),
            new SqlParameter("@Class2", info.Class2),
            new SqlParameter("@Class1", info.Class1),
            new SqlParameter("@Class3Name", info.Class3Name),
            new SqlParameter("@Class2Name", info.Class2Name),
            new SqlParameter("@Class1Name", info.Class1Name),
            new SqlParameter("@EnableFlag", info.EnableFlag),
            new SqlParameter("@SuggestCount", info.SuggestCount),
            new SqlParameter("@SolveCount", info.SolveCount),
            new SqlParameter("@Name", info.Name),
            new SqlParameter("@EnableBy", info.EnableBy),
            
            };

            return parms;
        }
        
        #endregion


        #region Get
        /// <summary>
        /// 获取列表
        /// </summary>
        public List<SolutionInfo> GetList(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            List<SolutionInfo> list = new List<SolutionInfo>();
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

        public List<SolutionInfo> GetList(string StrWhere)
        {
            List<SolutionInfo> list = new List<SolutionInfo>();
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
        /// 根据中类找到指定行数的记录，并从高到低排序
        /// </summary>
        /// <param name="Class2ID">中类故障ID</param>
        /// <param name="TopCount">需要获取行数，0则不限</param>
        /// <returns></returns>
        public List<SolutionInfo> GetList(int Class3ID,int TopCount)
        {
            List<SolutionInfo> list = new List<SolutionInfo>();
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("select ");
            if (TopCount>0)
            {
                strSQL.Append(" Top ").Append(TopCount);
            }
            strSQL.Append(ALL_PARM).Append(FROM_TABLE).Append(" where f_Class3=").Append(Class3ID);
            strSQL.Append(" and f_EnableFlag=1 ");
            strSQL.Append(" ORDER BY f_SolveCount DESC, f_SuggestCount DESC ");
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
        public SolutionInfo Get(int id)
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
        public SolutionInfo Get(string Name,int Class3ID)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("select top 1 ").Append(ALL_PARM).Append(FROM_TABLE);
            strSQL.Append(" where f_Class3 = ").Append(Class3ID).Append(" and f_Name='").Append(Name).Append("' ");

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
        public int Add(SolutionInfo info)
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
        public bool Edit(SolutionInfo info)
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

        public static StringBuilder DeleteByC3sql(string IDOrSelectIDSQL)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("delete ").Append(FROM_TABLE).Append(" where f_Class3 in (").Append(IDOrSelectIDSQL).Append(") ");

            return strSQL;
        }
        #endregion
    }
}