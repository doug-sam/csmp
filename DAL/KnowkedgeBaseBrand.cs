using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using DBUtility;
using CSMP.Model;

namespace CSMP.DAL
{
    public class KnowkedgeBaseBrandDAL
    {            
        private const string ALL_PARM = "  ID,f_Class3ID,f_KnowledgeID ";
        private const string FROM_TABLE = " from [sys_KnowkedgeBaseBrand] ";
        private const string TABLE = " sys_KnowkedgeBaseBrand ";
        private const string INSET = " (f_BrandID,f_KnowledgeID) values(@BrandID,@KnowledgeID)  ";
        private const string UPDATE = " f_BrandID=@BrandID,f_KnowledgeID=@KnowledgeID ";

        #region ReadyData
        private KnowkedgeBaseBrandInfo GetByDataReader(SqlDataReader rdr)
        {
            KnowkedgeBaseBrandInfo info = new KnowkedgeBaseBrandInfo();
            info.ID = Convert.ToInt32(rdr["ID"]);
            info.BrandID = Convert.ToInt32(rdr["f_BrandID"]);
            info.KnowledgeID = Convert.ToInt32(rdr["f_KnowledgeID"]);
            
            return info;
        }

        private SqlParameter[] GetParameter(KnowkedgeBaseBrandInfo info)
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter("@BrandID", info.BrandID),
            new SqlParameter("@KnowledgeID", info.KnowledgeID),
            
            };

            return parms;
        }
        
        #endregion


        #region Get
        /// <summary>
        /// 获取列表
        /// </summary>
        public List<KnowkedgeBaseBrandInfo> GetList(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            List<KnowkedgeBaseBrandInfo> list = new List<KnowkedgeBaseBrandInfo>();
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

        public List<KnowkedgeBaseBrandInfo> GetList(string StrWhere)
        {
            List<KnowkedgeBaseBrandInfo> list = new List<KnowkedgeBaseBrandInfo>();
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
        public KnowkedgeBaseBrandInfo Get(int id)
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
        public int Add(KnowkedgeBaseBrandInfo info)
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
        public bool Edit(KnowkedgeBaseBrandInfo info)
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
            strSQL.Append(DeleteByKnowledgeIDsql(id.ToString()));
            return SqlHelper.ExecuteNonQueryByTran(SqlHelper.SqlconnString, CommandType.Text, strSQL.ToString(), null);
        }

        /// <summary>
        /// 根据知识库id删除
        /// </summary>
        /// <param name="KnowledgeID"></param>
        /// <returns></returns>
        public bool DeleteByKnowledgeID(int KnowledgeID)
        {
            StringBuilder strSQL = DeleteByKnowledgeIDsql(KnowledgeID.ToString());
            return SqlHelper.ExecuteNonQueryByTran(SqlHelper.SqlconnString, CommandType.Text, strSQL.ToString(), null);
        }

        public static StringBuilder DeleteByKnowledgeIDsql(string IDOrSelectIDSQL)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append(" delete ").Append(FROM_TABLE).Append(" where f_KnowledgeID in (").Append(IDOrSelectIDSQL.ToString()).Append(" ) ");

            return strSQL;
        }

        
        #endregion
    }
}