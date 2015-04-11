using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using DBUtility;
using CSMP.Model;

namespace CSMP.DAL
{
    public class WeixinNewsDAL
    {
        private const string ALL_PARM = "  ID,f_MsgType,f_Pic,f_Title,f_Content,f_Description,f_LinkURL,f_KeyWord,f_IsConst,f_OrderID ";
        private const string FROM_TABLE = " from [fy_WeixinNews] ";
        private const string TABLE = " fy_WeixinNews ";
        private const string INSET = " (f_MsgType,f_Pic,f_Title,f_Content,f_Description,f_LinkURL,f_KeyWord,f_IsConst,f_OrderID) values(@MsgType,@Pic,@Title,@Content,@Description,@LinkURL,@KeyWord,@IsConst,@OrderID)  ";
        private const string UPDATE = " f_MsgType=@MsgType,f_Pic=@Pic,f_Title=@Title,f_Content=@Content,f_Description=@Description,f_LinkURL=@LinkURL,f_KeyWord=@KeyWord,f_IsConst=@IsConst,f_OrderID=@OrderID ";

        #region ReadyData
        private WeixinNewsInfo GetByDataReader(SqlDataReader rdr)
        {
            WeixinNewsInfo info = new WeixinNewsInfo();
            info.ID = Convert.ToInt32(rdr["ID"]);
            info.MsgType = Convert.ToInt32(rdr["f_MsgType"]);
            info.Pic = rdr["f_Pic"].ToString();
            info.Title = rdr["f_Title"].ToString();
            info.Content = rdr["f_Content"].ToString();
            info.Description = rdr["f_Description"].ToString();
            info.LinkURL = rdr["f_LinkURL"].ToString();
            info.KeyWord = rdr["f_KeyWord"].ToString();
            info.IsConst = Convert.ToBoolean(rdr["f_IsConst"]);
            info.OrderID = Convert.ToInt32(rdr["f_OrderID"]);
            
            return info;
        }

        private SqlParameter[] GetParameter(WeixinNewsInfo info)
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter("@MsgType", info.MsgType),
            new SqlParameter("@Pic", info.Pic),
            new SqlParameter("@Title", info.Title),
            new SqlParameter("@Content", info.Content),
            new SqlParameter("@Description", info.Description),
            new SqlParameter("@LinkURL", info.LinkURL),
            new SqlParameter("@KeyWord", info.KeyWord),
            new SqlParameter("@IsConst", info.IsConst),
            new SqlParameter("@OrderID", info.OrderID),
            
            };

            return parms;
        }
        
        #endregion


        #region Get
        /// <summary>
        /// 获取列表
        /// </summary>
        public List<WeixinNewsInfo> GetList(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            List<WeixinNewsInfo> list = new List<WeixinNewsInfo>();
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

        public List<WeixinNewsInfo> GetList(string StrWhere)
        {
            List<WeixinNewsInfo> list = new List<WeixinNewsInfo>();
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
        public WeixinNewsInfo Get(int id)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("select top 1 ").Append(ALL_PARM).Append(FROM_TABLE).Append(" where id = ").Append(id);

            using (SqlDataReader rdr = SqlHelper.ExecuteReader(SqlHelper.SqlconnString, CommandType.Text, strSQL.ToString(), null))
            {
                if (!rdr.Read()) return null;

                return GetByDataReader(rdr);
            }
        }

        public List<WeixinNewsInfo> Get(string KeyWord,bool IsConst)
        {
            List<WeixinNewsInfo> list = new List<WeixinNewsInfo>();
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("select ").Append(ALL_PARM).Append(FROM_TABLE).Append(" where ");
            if (IsConst)
            {
                strSQL.Append(" f_KeyWord='").Append(KeyWord).Append("'");                
            }
            else
            {
                strSQL.Append(" f_KeyWord like '%").Append(KeyWord).Append("%'");
            }
            strSQL.Append(" and f_IsConst=").Append(IsConst ? 1 : 0);
            strSQL.Append(" ORDER BY f_OrderID DESC,ID DESC");

            using (SqlDataReader rdr = SqlHelper.ExecuteReader(SqlHelper.SqlconnString, CommandType.Text, strSQL.ToString(), null))
            {
                while (rdr.Read())
                {
                    list.Add(GetByDataReader(rdr));
                }
            }

            LogInfo info = new LogInfo();
            info.AddDate = DateTime.Now;
            info.Category = "sql record";
            info.Content = strSQL.ToString();
            info.ErrorDate = DateTime.Now;
            info.SendEmail = false;
            info.Serious = 1;
            info.UserName = "";
            new LogDAL().Add(info);
            

            return list;
        }
        
        #endregion


        #region set
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="info">info</param>
        public int Add(WeixinNewsInfo info)
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
        public bool Edit(WeixinNewsInfo info)
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