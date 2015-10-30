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
    public class LeftMenuDataDAL
    {
        private const string ALL_PARM = " f_ToBeOnSite,f_ToBeDisposed,f_Disposing,f_Complete,f_Closed,f_StoreUrgency,f_StoreRequest,f_UserID,f_WorkGroupID,f_HaveGroupPower ";
        private const string FROM_TABLE = " from [sys_LeftMenuData] ";
        private const string TABLE = " sys_LeftMenuData ";
        private const string INSET = " (f_ToBeOnSite,f_ToBeDisposed,f_Disposing,f_Complete,f_Closed,f_StoreUrgency,f_StoreRequest,f_UserID,f_WorkGroupID,f_HaveGroupPower) values(@ToBeOnSite,@ToBeDisposed,@Disposing,@Complete,@Closed,@StoreUrgency,@StoreRequest,@UserID,@WorkGroupID,@HaveGroupPower)  ";
        private const string UPDATE = " f_ToBeOnSite=@ToBeOnSite,f_ToBeDisposed=@ToBeDisposed,f_Disposing=@Disposing,f_Complete=@Complete,f_Closed=@Closed,f_StoreUrgency=@StoreUrgency,f_StoreRequest=@StoreRequest,f_UserID=@UserID,f_WorkGroupID=@WorkGroupID,f_HaveGroupPower=@HaveGroupPower ";

        #region ReadyData
        private LeftMenuData GetByDataReader(SqlDataReader rdr)
        {
            LeftMenuData info = new LeftMenuData();

            info.ToBeOnSite = Convert.ToInt32(rdr["f_ToBeOnSite"]);
            info.ToBeDisposed = Convert.ToInt32(rdr["f_ToBeDisposed"]);
            info.Disposing = Convert.ToInt32(rdr["f_Disposing"]);
            info.Complete = Convert.ToInt32(rdr["f_Complete"]);
            info.Closed = Convert.ToInt32(rdr["f_Closed"]);
            info.StoreUrgency = Convert.ToInt32(rdr["f_StoreUrgency"]);
            info.StoreRequest = Convert.ToInt32(rdr["f_StoreRequest"]);
            info.UserID = Convert.ToInt32(rdr["f_UserID"]);
            info.WorkGroupID = Convert.ToInt32(rdr["f_WorkGroupID"]);
            info.HaveGroupPower = Convert.ToBoolean(rdr["f_HaveGroupPower"]);

            return info;
        }

        private List<LeftMenuData> GetByDataTable(DataTable dt)
        {
            List<LeftMenuData> dataList = new List<LeftMenuData>();
            foreach (DataRow dr in dt.Rows)
            {
                LeftMenuData info = new LeftMenuData();
                info.ToBeOnSite = Convert.ToInt32(dr["f_ToBeOnSite"].ToString());
                info.ToBeDisposed = Convert.ToInt32(dr["f_ToBeDisposed"].ToString());
                info.Disposing = Convert.ToInt32(dr["f_Disposing"].ToString());
                info.Complete = Convert.ToInt32(dr["f_Complete"].ToString());
                info.Closed = Convert.ToInt32(dr["f_Closed"].ToString());
                info.StoreUrgency = Convert.ToInt32(dr["f_StoreUrgency"].ToString());
                info.StoreRequest = Convert.ToInt32(dr["f_StoreRequest"].ToString());
                info.UserID = Convert.ToInt32(dr["f_UserID"].ToString());
                info.WorkGroupID = Convert.ToInt32(dr["f_WorkGroupID"].ToString());
                info.HaveGroupPower = Convert.ToBoolean(dr["f_HaveGroupPower"].ToString());

                dataList.Add(info);
            }


            return dataList;
        }

        private SqlParameter[] GetParameter(LeftMenuData info)
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter("@ToBeOnSite", info.ToBeOnSite),
            new SqlParameter("@ToBeDisposed", info.ToBeDisposed),
            new SqlParameter("@Disposing", info.Disposing),
            new SqlParameter("@Complete", info.Complete),
            new SqlParameter("@Closed", info.Closed),
            new SqlParameter("@StoreUrgency", info.StoreUrgency),
            new SqlParameter("@StoreRequest", info.StoreRequest),
            new SqlParameter("@UserID", info.UserID),
            new SqlParameter("@WorkGroupID", info.WorkGroupID),
            new SqlParameter("@HaveGroupPower", info.HaveGroupPower),
            
            };

            return parms;
        }

        #endregion

        #region Get


        public List<LeftMenuData> GetList(string StrWhere)
        {
            List<LeftMenuData> list = new List<LeftMenuData>();
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
        /// 使用存储过程获取所有列表
        /// </summary>
        /// <param name="StrWhere"></param>
        /// <returns></returns>
        public List<LeftMenuData> GetListBySP()
        {
            List<LeftMenuData> list = new List<LeftMenuData>();
            //StoreProcedure sp = new StoreProcedure("sp_wx_querycoworkers");
            StoreProcedure sp = new StoreProcedure("SP_GetLeftData");//类的对象
            //Object[] paraValues = new object[2];//注意,这里是存储过程中全部的参数,一共有三个,还要注意顺序啊,返回值是第一个,那么赋值时第一个参数就为空

            //paraValues[0] = "o8dWJt1hWuBHIJnJUZ46i6B1myuk";//从第二个参数开始赋值
            //paraValues[1] = "";//从第二个参数开始赋值

            list = GetByDataTable(sp.ExecuteDataTable(null));
            return list;
        }
        

        /// <summary>
        /// 获取Info
        /// </summary>
        /// <param name="id">id</param>
        public LeftMenuData Get(int UserID)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("select top 1 ").Append(ALL_PARM).Append(FROM_TABLE).Append(" where f_UserID = '").Append(UserID).Append("' order by f_UserID");

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
        public int Add(LeftMenuData info)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("insert into ").Append(TABLE).Append(INSET);

            SqlParameter[] parms = GetParameter(info);

            if (SqlHelper.ExecuteNonQueryByTran(SqlHelper.SqlconnString, CommandType.Text, strSQL.ToString(), parms))
            {
                return Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.SqlconnString, CommandType.Text, "select f_UserID from " + TABLE+" where f_UserID="+info.UserID, null).ToString());
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
        public bool Edit(LeftMenuData info)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("update ").Append(TABLE).Append(" set ").Append(UPDATE).Append(" where f_UserID = ").Append(info.UserID);
            SqlParameter[] parms = GetParameter(info);

            return SqlHelper.ExecuteNonQueryByTran(SqlHelper.SqlconnString, CommandType.Text, strSQL.ToString(), parms);
        }

        /// <summary>
        /// 删除Member
        /// </summary>
        /// <param name="id">Member id</param>
        public bool Delete(int UserID)
        {
            StringBuilder strSQL = new StringBuilder();

            strSQL.Append("delete ").Append(FROM_TABLE).Append(" where f_UserID = ").Append(UserID);

            return SqlHelper.ExecuteNonQueryByTran(SqlHelper.SqlconnString, CommandType.Text, strSQL.ToString(), null);
        }

        


        #endregion
    }
}
