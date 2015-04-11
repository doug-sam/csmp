using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using DBUtility;
using System.Text;

namespace CSMP.DAL
{
    public static class Function
    {
        public static readonly string SqlconnString = SqlHelper.SqlconnString;
        /// <summary>
        /// 获取分页SQL语句（来自网监系统）
        /// </summary>
        /// <param name="PageSize">每页记录条数</param>
        /// <param name="PageIndex">当前第几页</param>
        /// <param name="TableName">表名</param>
        /// <param name="StrWhere">查询条件。后可带order by</param>
        /// <param name="Count">返回参数。指明总页数</param>
        /// <returns>返回查询分页的sql语句</returns>
        public static string GetPageSQL(int PageSize, int PageIndex, string TableName, string StrWhere, out int Count)
        {
            return GetPageSQL(PageSize, PageIndex, TableName, StrWhere, "*", " ID ", out Count);
        }

        /// <summary>
        /// 支持连表查询询的分页语句
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="TableName">tableName1 join tableName2 on tableName1.columnA=tableName2.columnB</param>
        /// <param name="StrWhere"></param>
        /// <param name="SelectColumn">tableName1.columns and tableName2.columns</param>
        /// <param name="PrimaryKeyColumn">作为分页主键列</param>
        /// <param name="Count"></param>
        /// <returns></returns>
        public static string GetPageSQL(int PageSize, int PageIndex, string TableName, string StrWhere, string SelectColumn, string PrimaryKeyColumn, out int Count)
        {
            Count = 0;
            string CountWhere = StrWhere.ToLower().Contains(" order") ? StrWhere.Substring(0, StrWhere.ToLower().IndexOf(" order")) : StrWhere;//有聚合函数count要去掉order by
            string CountSQL = "select count(*) as MyCount from " + TableName + " where " + CountWhere;
            using (SqlDataReader rdr = SqlHelper.ExecuteReader(SqlconnString, CommandType.Text, CountSQL, null))
            {
                while (rdr.Read())
                {
                    Count = Convert.ToInt32(rdr["MyCount"].ToString());
                }
            }

            int MaxPage = 0;
            if (Count % PageSize == 0) MaxPage = Count / PageSize;
            else MaxPage = (Count / PageSize) + 1;
            if (PageIndex > MaxPage) PageIndex = MaxPage;
            if (PageIndex <= 1)
            {
                return "select top " + PageSize + " " + SelectColumn + " from " + TableName + " where " + StrWhere;
            }
            else
            {
                string minsql = "(select " + PrimaryKeyColumn + " from (select top " + ((PageIndex - 1) * PageSize) + " " + PrimaryKeyColumn + " from " + TableName + " where " + StrWhere + ") as temp)";
                return "select top " + PageSize + " " + SelectColumn + " from " + TableName + " where " + PrimaryKeyColumn + " not in " + minsql + " and " + StrWhere;
            }
        }



    #region 不实际的练习，因为排序影响ID无序排列时就无法分页了
        ///// <summary>
        ///// 高效分页sql拼接，默认SelectColumn为所有，PrimaryKeyColumn为ID
        ///// </summary>
        ///// <param name="PageSize">每页记录数</param>
        ///// <param name="PageIndex">当前页码数</param>
        ///// <param name="TableName">查询表名</param>
        ///// <param name="StrWhere">条件，可加order by</param>
        ///// <param name="Count">返回当前条件下记录数</param>
        ///// <returns>用于分页的sql字符串</returns>
        //private static string GetPageSQL(int PageSize, int PageIndex, string TableName, string StrWhere, out int Count)
        //{
        //    return GetPageSQL(PageSize, PageIndex, TableName, StrWhere, " * ", " ID ", out Count);
        //}

        ///// <summary>
        ///// 高效分页sql拼接
        ///// </summary>
        ///// <param name="PageSize">每页记录数</param>
        ///// <param name="PageIndex">当前页码数</param>
        ///// <param name="TableName">查询表名</param>
        ///// <param name="StrWhere">条件，可加order by </param>
        ///// <param name="SelectColumn">要查询出来的列，可用*</param>
        ///// <param name="PrimaryKeyColumn">用来分页的主键列</param>
        ///// <param name="Count">返回当前条件下记录数</param>
        ///// <returns>用于分页的sql字符串</returns>
        //private static string GetPageSQL(int PageSize, int PageIndex, string TableName, string StrWhere,string SelectColumn, string PrimaryKeyColumn, out int Count)
        //{
        //   PageIndex= PageIndex <= 0 ? 1 : PageIndex;
        //    Count = 0;
        //    string CountWhere = StrWhere.ToLower().Contains("order") ? StrWhere.Substring(0, StrWhere.ToLower().IndexOf("order")) : StrWhere;//有聚合函数count要去掉order by
        //    string CountSQL = "select count(1) as MyCount from " + TableName + " where " + CountWhere;
        //    Count=(int)SqlHelper.ExecuteScalar(CommandType.Text, CountSQL, null);

        //    int PreCount = PageSize * (PageIndex - 1);
        //    if (PreCount < 0) PreCount = 0;
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("SELECT TOP ").Append(PageSize).Append(" ").Append(SelectColumn).Append(" FROM ").Append(TableName);
        //    sb.Append("WHERE id > ");
        //    sb.Append("         (");
        //    sb.Append("         SELECT ISNULL(MAX(").Append(PrimaryKeyColumn).Append("),0) ").Append(" from ");
        //    sb.Append("             (");
        //    sb.Append("                 SELECT TOP ").Append(PreCount).Append(" ").Append(PrimaryKeyColumn).Append("  FROM ").Append(TableName);
        //    sb.Append("                 WHERE "+StrWhere);
        //    sb.Append("              ) A");
        //    sb.Append("          )");
        //    sb.Append(" and " + StrWhere);

        //    return sb.ToString();

        //    //SELECT TOP 页大小 *
        //    //FROM table1
        //    //WHERE id >
        //    //          (
        //    //          SELECT ISNULL(MAX(id),0) 
        //    //          FROM 
        //    //                (
        //    //                SELECT TOP 页大小*(页数-1) id FROM table1 where 1=1 ORDER BY id
        //    //                ) A
        //    //          )
        //    //where 1=1
        //    //ORDER BY id        
        //}
 
	#endregion    
    }
}
