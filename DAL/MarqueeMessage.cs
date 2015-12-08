using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using DBUtility;
using CSMP.Model;

namespace CSMP.DAL
{
    public class MarqueeMessageDAL
    {
        private const string ALL_PARM = "  ID,f_No,f_MaintainUserID,f_MaintainUserName,f_MajorUserID,f_MajorUserName,f_Content ";
        public const string FROM_TABLE = " from [sys_MarqueeMessage] ";
        public const string TABLE = " sys_MarqueeMessage ";
        private const string INSET = " (f_No,f_MaintainUserID,f_MaintainUserName,f_MajorUserID,f_MajorUserName,f_Content) values (@No,@MaintainUserID,@MaintainUserName,@MajorUserID,@MajorUserName,@Content)  ";
        private const string UPDATE = " f_No=@No,f_MaintainUserID=@MaintainUserID,f_MaintainUserName=@MaintainUserName,f_MajorUserID=@MajorUserID,f_MajorUserName=@MajorUserName,f_Content=@Content ";

        #region ReadyData
        private MarqueeMessage GetByDataReader(SqlDataReader rdr)
        {
            MarqueeMessage info = new MarqueeMessage();
            info.ID = Convert.ToInt32(rdr["ID"]);
            info.No = rdr["f_No"].ToString();
            info.MaintainUserID = Convert.ToInt32(rdr["f_MaintainUserID"]);
            info.MaintainUserName = rdr["f_MaintainUserName"].ToString();
            info.MajorUserID = Convert.ToInt32(rdr["f_MajorUserID"]);
            info.MajorUserName = rdr["f_MajorUserName"].ToString();
            info.Content = rdr["f_Content"].ToString();
           
            return info;
        }

        private SqlParameter[] GetParameter(MarqueeMessage info)
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter("@No", info.No),
                new SqlParameter("@MaintainUserID", info.MaintainUserID),
            new SqlParameter("@MaintainUserName", info.MaintainUserName),
            new SqlParameter("@MajorUserID", info.MajorUserID),
            new SqlParameter("@MajorUserName", info.MajorUserName),
            new SqlParameter("@Content", info.Content),
                      
            };

            return parms;
        }
        #endregion
        /// <summary>
        /// 获取列表
        /// </summary>
        public List<MarqueeMessage> GetList(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            List<MarqueeMessage> list = new List<MarqueeMessage>();
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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="StrWhere"></param>
        /// <returns></returns>
        public List<MarqueeMessage> GetList(string StrWhere)
        {
            List<MarqueeMessage> list = new List<MarqueeMessage>();
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("select ").Append(ALL_PARM).Append(FROM_TABLE);
            strSQL.Append(" where ").Append(" 1=1 ").Append(StrWhere);
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
        /// 根据二线负责人ID
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public List<MarqueeMessage> GetByMaintaimUserID(int MaintaimUserID)
        {
            string sqlWhere = " AND f_MaintainUserID =" + MaintaimUserID + " AND f_IsRead =0";
            return GetList( sqlWhere);
        }
        /// <summary>
        /// 查询总记录数
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public int CountAll()
        {
            string CountSQL = "select count(*) as MyCount from " + TABLE;
            return Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.SqlconnString, CommandType.Text, CountSQL, null).ToString());
        }


        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="info">info</param>
        public int Add(MarqueeMessage info)
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
        /// 通过SP插入或更新一条记录
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public string AddBySP(MarqueeMessage info)
        {
            StoreProcedure sp = new StoreProcedure("sp_APP_UpdateMarqueeMessage");//类的对象
            Object[] paraValues = new object[8];//注意,这里是存储过程中全部的参数,一共有三个,还要注意顺序啊,返回值是第一个,那么赋值时第一个参数就为空

            paraValues[0] = info.No;
            paraValues[1] = info.MaintainUserID;//从第二个参数开始赋值
            paraValues[2] = info.MaintainUserName;
            paraValues[3] = info.MajorUserID;
            paraValues[4] = info.MajorUserName;
            paraValues[5] = info.Content;
            paraValues[6] = info.IsRead;
            paraValues[7] = "";
            object[] output;
            sp.ExecProcOutput(out  output, 2, paraValues);
            if (output != null)
            {
                return output[1].ToString();
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="info">info</param>
        public bool Edit(MarqueeMessage info)
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

        /// <summary>
        /// 删除Member
        /// </summary>
        /// <param name="id"></param>
        public bool Delete(string No)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("delete ").Append(FROM_TABLE).Append(" where f_No = '").Append(No).Append("'");

            return SqlHelper.ExecuteNonQueryByTran(SqlHelper.SqlconnString, CommandType.Text, strSQL.ToString(), null);
        }
    }
}
