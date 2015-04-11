using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using DBUtility;
using CSMP.Model;

namespace CSMP.DAL
{
    public class YumRecordDAL
    {
        private const string ALL_PARM = "  ID,f_CallID,f_CallNo,f_SendDate,f_IsSuccess,f_Step,f_Flag ";
        private const string FROM_TABLE = " from [sys_YumRecord] ";
        private const string TABLE = " sys_YumRecord ";
        private const string INSET = " (f_CallID,f_CallNo,f_SendDate,f_IsSuccess,f_Step,f_Flag) values(@CallID,@CallNo,@SendDate,@IsSuccess,@Step,@Flag)  ";
        private const string UPDATE = " f_CallID=@CallID,f_CallNo=@CallNo,f_SendDate=@SendDate,f_IsSuccess=@IsSuccess,f_Step=@Step,f_Flag=@Flag ";

        #region ReadyData
        private YumRecordInfo GetByDataReader(SqlDataReader rdr)
        {
            YumRecordInfo info = new YumRecordInfo();
            info.ID = Convert.ToInt32(rdr["ID"]);
            info.CallID = Convert.ToInt32(rdr["f_CallID"]);
            info.CallNo = Convert.ToInt32(rdr["f_CallNo"]);
            info.SendDate = Convert.ToDateTime(rdr["f_SendDate"]);
            info.IsSuccess = Convert.ToBoolean(rdr["f_IsSuccess"]);
            info.Step = rdr["f_Step"].ToString();
            info.Flag = rdr["f_Flag"].ToString();
            
            return info;
        }

        private SqlParameter[] GetParameter(YumRecordInfo info)
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter("@CallID", info.CallID),
            new SqlParameter("@CallNo", info.CallNo),
            new SqlParameter("@SendDate", info.SendDate),
            new SqlParameter("@IsSuccess", info.IsSuccess),
            new SqlParameter("@Step", info.Step),
            new SqlParameter("@Flag", info.Flag),
            
            };

            return parms;
        }
        
        #endregion


        #region Get
        /// <summary>
        /// 获取列表
        /// </summary>
        public List<YumRecordInfo> GetList(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            List<YumRecordInfo> list = new List<YumRecordInfo>();
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

        public List<YumRecordInfo> GetList(string StrWhere)
        {
            List<YumRecordInfo> list = new List<YumRecordInfo>();
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
        public YumRecordInfo Get(int id)
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
        public YumRecordInfo Get(int CallID, string StepAction)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("select top 1 ").Append(ALL_PARM).Append(FROM_TABLE).Append(" where f_CallID = ").Append(CallID);
            strSQL.Append(" and f_Step='").Append(StepAction).Append("'");

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
        public int Add(YumRecordInfo info)
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
        public bool Edit(YumRecordInfo info)
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