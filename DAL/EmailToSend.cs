using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using DBUtility;
using CSMP.Model;

namespace CSMP.DAL
{
    public class EmailToSendDAL
    {
        private const string ALL_PARM = "  ID,f_CallNo,f_Subject,f_MailAddress,f_CC,f_ReplayTo,f_Attachment,f_FromEmailAddress,f_FromEmailDisplayName,f_FromEmailHost,f_FromEmailPwd,f_FromPort,f_Body,f_CustomerID,f_CustomerName,f_BrandID,f_BrandName ";
        private const string FROM_TABLE = " from [sys_EmailsToSend] ";
        private const string TABLE = " sys_EmailsToSend ";
        private const string INSET = " (f_CallNo,f_Subject,f_MailAddress,f_CC,f_ReplayTo,f_Attachment,f_FromEmailAddress,f_FromEmailDisplayName,f_FromEmailHost,f_FromEmailPwd,f_FromPort,f_Body,f_CustomerID,f_CustomerName,f_BrandID,f_BrandName) values(@CallNo,@Subject,@MailAddress,@CC,@ReplayTo,@Attachment,@FromEmailAddress,@FromEmailDisplayName,@FromEmailHost,@FromEmailPwd,@FromPort,@Body,@CustomerID,@CustomerName,@BrandID,@BrandName)  ";
        private const string UPDATE = " f_CallNo=@CallNo,f_Subject=@Subject,f_MailAddress=@MailAddress,f_CC=@CC,f_ReplayTo=@ReplayTo,f_Attachment=@Attachment,f_FromEmailAddress=@FromEmailAddress,f_FromEmailDisplayName=@FromEmailDisplayName,f_FromEmailHost=@FromEmailHost,f_FromEmailPwd=@FromEmailPwd,f_FromPort=@FromPort,f_Body=@Body,f_CustomerID=@CustomerID,f_CustomerName=@CustomerName,f_BrandID=@BrandID,f_BrandName=@BrandName ";

        #region ReadyData
        private EmailToSend GetByDataReader(SqlDataReader rdr)
        {
            EmailToSend info = new EmailToSend();
            info.ID = Convert.ToInt32(rdr["ID"]);
            info.CustomerID = Convert.ToInt32(rdr["f_CustomerID"]);
            info.CustomerName = rdr["f_CustomerName"].ToString();
            info.BrandID = Convert.ToInt32(rdr["f_BrandID"]);
            info.BrandName = rdr["f_BrandName"].ToString();
            info.CallNo = rdr["f_CallNo"].ToString();
            info.Subject = rdr["f_Subject"].ToString();
            info.MailAddress = rdr["f_MailAddress"].ToString();
            info.CC = rdr["f_CC"].ToString();
            info.ReplayTo = rdr["f_ReplayTo"].ToString();
            info.Attachment = rdr["f_Attachment"].ToString();
            info.FromEmailAddress = rdr["f_FromEmailAddress"].ToString();
            info.FromEmailDisplayName = rdr["f_FromEmailDisplayName"].ToString();
            info.FromEmailHost = rdr["f_FromEmailHost"].ToString();
            info.FromEmailPwd = rdr["f_FromEmailPwd"].ToString();
            info.FromPort = rdr["f_FromPort"].ToString().Trim();
            info.Body = rdr["f_Body"].ToString();

            return info;
        }

        private SqlParameter[] GetParameter(EmailToSend info)
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter("@CustomerID", info.CustomerID),
                new SqlParameter("@CustomerName", info.CustomerName),
                new SqlParameter("@BrandID", info.BrandID),
                new SqlParameter("@BrandName", info.BrandName),
                new SqlParameter("@CallNo", info.CallNo),
                new SqlParameter("@Subject", info.Subject),
                new SqlParameter("@MailAddress", info.MailAddress),
                new SqlParameter("@CC", info.CC),
                new SqlParameter("@ReplayTo", info.ReplayTo),
                new SqlParameter("@Attachment", info.Attachment),
                new SqlParameter("@FromEmailAddress", info.FromEmailAddress),
                new SqlParameter("@FromEmailDisplayName", info.FromEmailDisplayName),
                new SqlParameter("@FromEmailHost", info.FromEmailHost),
                new SqlParameter("@FromEmailPwd", info.FromEmailPwd),
                new SqlParameter("@FromPort", info.FromPort),
                new SqlParameter("@Body", info.Body),

            };

            return parms;
        }
        #endregion
        /// <summary>
        /// 根据查询条件 获取列表
        /// </summary>
        /// <param name="StrWhere"></param>
        /// <returns></returns>
        public List<EmailToSend> GetList(string StrWhere)
        {
            List<EmailToSend> list = new List<EmailToSend>();
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("select ").Append(ALL_PARM).Append(FROM_TABLE);
            strSQL.Append(" where ").Append(" 1=1 ").Append(StrWhere.Replace("1=1", " "));
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
        public EmailToSend Get(int id)
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
        public int Add(EmailToSend info)
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
        public bool Edit(EmailToSend info)
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
