using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using DBUtility;
using CSMP.Model;
using Tool;
using Npgsql;



namespace CSMP.DAL
{
    public class RecordDAL
    {
        private const string ALL_PARM = "  dbid,recordname,filepath,startdate,enddate,unickid ";
        private const string FROM_TABLE = " from t_record ";
        private const string TABLE = " t_record ";

        #region ReadyData
        private RecordInfo GetByDataReader(NpgsqlDataReader rdr)
        {
            RecordInfo info = new RecordInfo();
            info.dbid =Tool.Function.ConverToInt(rdr["dbid"]);
            info.recordname = rdr["recordname"].ToString();
            info.filepath = rdr["filepath"].ToString();
            info.stardate = Tool.Function.ConverToDateTime(rdr["startdate"]);
            info.enddate = Tool.Function.ConverToDateTime(rdr["enddate"]);
            info.unickid = rdr["unickid"].ToString();
            
            return info;
        }

        private RecordInfo GetByDataReader(SqlDataReader rdr)
        {
            RecordInfo info = new RecordInfo();
            info.filepath = rdr["TS_FILE_URL"].ToString();
            return info;
        }
        
        #endregion


        #region Get

        /// <summary>
        /// 获取Info
        /// </summary>
        /// <param name="id">id</param>
        public RecordInfo Get(string unickid)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("select  ").Append(ALL_PARM).Append(FROM_TABLE).Append(" where unickid like '").Append(unickid).Append("%' order by dbid desc limit 1 ");
            using (NpgsqlDataReader rdr = PostgreHelper.ExecuteReader(PostgreHelper.PostgreConnectionString, CommandType.Text, strSQL.ToString(), null))
            {
                if (!rdr.Read()) return null;

                return GetByDataReader(rdr);
            }
        }

        /// <summary>
        /// 获取Info。新版。与新的呼叫中心系统匹配。
        /// </summary>
        /// <param name="uniqueid">uniqueid</param>
        public RecordInfo GetRecordInfo(string uniqueid)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("select TS_FILE_URL FROM TS_REC_LOG where TS_CALL_ID = '").Append(uniqueid).Append("'");
            using (SqlDataReader rdr = SqlHelper.ExecuteReader(SqlHelper.CCRecordSqlconnString, CommandType.Text, strSQL.ToString(), null))
            {
                if (!rdr.Read()) return null;

                return GetByDataReader(rdr);
            }
        }
        #endregion


    }
}