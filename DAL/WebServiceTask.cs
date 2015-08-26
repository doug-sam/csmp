using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using DBUtility;
using CSMP.Model;

namespace CSMP.DAL
{
    public class WebServiceTaskDAL
    {
        private const string ALL_PARM = "  ID,f_CallNo,f_TaskUrl,f_IsDone,f_CustomerID,f_CustomerName,f_BrandID,f_BrandName,f_Remark ";
        private const string FROM_TABLE = " from [sys_WebServiceTask] ";
        private const string TABLE = " sys_WebServiceTask ";
        private const string INSET = " (f_CallNo,f_TaskUrl,f_IsDone,f_CustomerID,f_CustomerName,f_BrandID,f_BrandName,f_Remark) values(@CallNo,@TaskUrl,@IsDone,@CustomerID,@CustomerName,@BrandID,@BrandName,@Remark)  ";
        private const string UPDATE = " f_CallNo=@CallNo,f_TaskUrl=@TaskUrl,f_IsDone=@IsDone,f_CustomerID=@CustomerID,f_CustomerName=@CustomerName,f_BrandID=@BrandID,f_BrandName=@BrandName,f_Remark=@Remark ";

        #region ReadyData
        private WebServiceTaskInfo GetByDataReader(SqlDataReader rdr)
        {
            WebServiceTaskInfo info = new WebServiceTaskInfo();
            info.ID = Convert.ToInt32(rdr["ID"]);
            info.TaskUrl = rdr["f_TaskUrl"].ToString();
            info.CustomerID = Convert.ToInt32(rdr["f_CustomerID"]);
            info.CustomerName = rdr["f_CustomerName"].ToString();
            info.BrandID = Convert.ToInt32(rdr["f_BrandID"]);
            info.BrandName = rdr["f_BrandName"].ToString();
            info.Remark = rdr["f_Remark"].ToString();
            info.IsDone = Convert.ToBoolean(rdr["f_IsDone"]);
            info.CallNo = rdr["f_CallNo"].ToString();

            return info;
        }

        private SqlParameter[] GetParameter(WebServiceTaskInfo info)
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter("@TaskUrl", info.TaskUrl),
                new SqlParameter("@CustomerID", info.CustomerID),
                new SqlParameter("@CustomerName", info.CustomerName),
                new SqlParameter("@BrandID", info.BrandID),
                new SqlParameter("@BrandName", info.BrandName),
                new SqlParameter("@Remark", info.Remark),
                new SqlParameter("@IsDone", info.IsDone),
                new SqlParameter("@CallNo", info.CallNo),
            };

            return parms;
        }
        #endregion
        /// <summary>
        /// 根据查询条件 获取列表
        /// </summary>
        /// <param name="StrWhere"></param>
        /// <returns></returns>
        public List<WebServiceTaskInfo> GetList(string StrWhere)
        {
            List<WebServiceTaskInfo> list = new List<WebServiceTaskInfo>();
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("select ").Append(ALL_PARM).Append(FROM_TABLE);
            strSQL.Append(" where ").Append(" 1=1 AND f_IsDone=0 ").Append(StrWhere.Replace("1=1", " "));
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
        public WebServiceTaskInfo Get(int id)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("select top 1 ").Append(ALL_PARM).Append(FROM_TABLE).Append(" where id = ").Append(id);

            using (SqlDataReader rdr = SqlHelper.ExecuteReader(SqlHelper.SqlconnString, CommandType.Text, strSQL.ToString(), null))
            {
                if (!rdr.Read()) return null;

                return GetByDataReader(rdr);
            }
        }
        #region set
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="info">info</param>
        public int Add(WebServiceTaskInfo info)
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
        public bool Edit(WebServiceTaskInfo info)
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