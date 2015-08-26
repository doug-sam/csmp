using System.Linq;
using System.Text;
using System;
using System.Collections.Generic;
using CSMP.DAL;
using CSMP.Model;
using System.Data.SqlClient;

namespace CSMP.BLL
{
    public static class CommonBLL
    {
        private static readonly DAL.CommonDAL dal = new DAL.CommonDAL();

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="info">info</param>
        public static bool ExecuteNonQueryStr(string SQLStr)
        {
            return dal.ExecuteNonQueryStr(SQLStr);
        }
        /// <summary>
        /// 获取查询结果
        /// </summary>
        /// <param name="SQLStr"></param>
        /// <returns></returns>
        public static SqlDataReader ExecuteReaderStr(string SQLStr)
        {
            return dal.ExecuteReaderStr(SQLStr);
        }
    }
}
