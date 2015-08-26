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
    public class CommonDAL
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="info">info</param>
        public bool ExecuteNonQueryStr(string SQLStr)
        {
            return SqlHelper.ExecuteNonQuery(CommandType.Text, SQLStr, null)>0;
        }
        /// <summary>
        /// 得到查询结果
        /// </summary>
        /// <param name="SQLStr"></param>
        /// <returns></returns>
        public SqlDataReader ExecuteReaderStr(string SQLStr)
        {

            return SqlHelper.ExecuteReader(SqlHelper.SqlconnString, CommandType.Text, SQLStr, null);
        }
    }
}
