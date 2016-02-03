using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using DBUtility;
using CSMP.Model;

namespace CSMP.DAL
{
    public class KnowledgeBaseDAL
    {
        private const string ALL_PARM = "  ID,f_Title,f_Content,f_AddByUserName,f_Enable,f_AddDate,f_ViewCount,f_GoodCount,f_Labs,f_KnowledgeType ";
        private const string FROM_TABLE = " from [sys_KnowledgeBase] ";
        private const string TABLE = " sys_KnowledgeBase ";
        private const string INSET = " (f_Title,f_Content,f_AddByUserName,f_Enable,f_AddDate,f_ViewCount,f_GoodCount,f_Labs,f_KnowledgeType) values(@Title,@Content,@AddByUserName,@Enable,@AddDate,@ViewCount,@GoodCount,@Labs,@KnowledgeType)  ";
        private const string UPDATE = " f_Title=@Title,f_Content=@Content,f_AddByUserName=@AddByUserName,f_Enable=@Enable,f_AddDate=@AddDate,f_ViewCount=@ViewCount,f_GoodCount=@GoodCount,f_Labs=@Labs,f_KnowledgeType=@KnowledgeType ";

        #region ReadyData
        private KnowledgeBaseInfo GetByDataReader(SqlDataReader rdr)
        {
            KnowledgeBaseInfo info = new KnowledgeBaseInfo();
            info.ID = Convert.ToInt32(rdr["ID"]);
            info.Title = rdr["f_Title"].ToString();
            info.Content = rdr["f_Content"].ToString();
            info.AddByUserName = rdr["f_AddByUserName"].ToString();
            info.Enable = Convert.ToBoolean(rdr["f_Enable"]);
            info.AddDate = Convert.ToDateTime(rdr["f_AddDate"]);
            info.ViewCount = Convert.ToInt32(rdr["f_ViewCount"]);
            info.GoodCount = Convert.ToInt32(rdr["f_GoodCount"]);
            info.Labs = rdr["f_Labs"].ToString();
            info.KnowledgeType = Convert.ToInt32(rdr["f_KnowledgeType"]);
            
            return info;
        }

        private SqlParameter[] GetParameter(KnowledgeBaseInfo info)
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter("@Title", info.Title),
            new SqlParameter("@Content", info.Content),
            new SqlParameter("@AddByUserName", info.AddByUserName),
            new SqlParameter("@Enable", info.Enable),
            new SqlParameter("@AddDate", info.AddDate),
            new SqlParameter("@ViewCount", info.ViewCount),
            new SqlParameter("@GoodCount", info.GoodCount),
            new SqlParameter("@Labs", info.Labs),
            new SqlParameter("@KnowledgeType", info.KnowledgeType),
            
            };

            return parms;
        }
        
        #endregion


        #region Get
        /// <summary>
        /// 获取列表
        /// </summary>
        public List<KnowledgeBaseInfo> GetList(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            List<KnowledgeBaseInfo> list = new List<KnowledgeBaseInfo>();
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

        public List<KnowledgeBaseInfo> GetList(string StrWhere)
        {
            List<KnowledgeBaseInfo> list = new List<KnowledgeBaseInfo>();
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
        /// 根据传来的客户ID和品牌ID显示列表
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="brandId"></param>
        /// <returns></returns>
        public DataTable GetListByBothId(string openId,string customerId, string brandId)
        {
            StoreProcedure sp = new StoreProcedure("sp_wx_queryknowledges");//类的对象
            Object[] paraValues = new object[4];//注意,这里是存储过程中全部的参数,一共有三个,还要注意顺序啊,返回值是第一个,那么赋值时第一个参数就为空
            paraValues[0] = customerId;//从第二个参数开始赋值
            paraValues[1] = brandId;
            paraValues[2] = openId;
            paraValues[3] = "";
            object[] output = new object[3];
            DataTable dt = new DataTable();
            dt = sp.ExecuteDataTable(out output, paraValues);
            return dt;
        }


        /// <summary>
        /// 根据小类故障查找
        /// </summary>
        /// <param name="Class3ID"></param>
        /// <returns></returns>
        public List<KnowledgeBaseInfo> GetListByBrandID(int BrandID)
        {
            List<KnowledgeBaseInfo> list = new List<KnowledgeBaseInfo>();
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("select ").Append(ALL_PARM).Append(FROM_TABLE).Append(" where ");
            strSQL.Append(" ID in( select f_KnowledgeID from sys_KnowkedgeBaseBrand where f_BrandID=").Append(BrandID).Append(") ");
            strSQL.Append(" order by f_ViewCount desc,id desc");
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
        public KnowledgeBaseInfo Get(int id)
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
        public int Add(KnowledgeBaseInfo info)
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
        public bool Edit(KnowledgeBaseInfo info)
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