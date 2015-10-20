using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using DBUtility;
using CSMP.Model;


namespace CSMP.DAL
{
    /// <summary>
    /// 汉堡王转呈系统店铺信息数据同步专用DAL
    /// </summary>
    public class BKStoreInfoDAL
    {
        private const string ALL_PARM = "  ID,f_GlobalCode,f_LocalCode,f_Region,f_City,f_StoreType,f_Name,f_Address,f_OpenDate,f_CloseDate,f_Tel,f_FAX,f_Email,f_OM,f_OC,f_ServerIP,f_LANGateway,f_TSI,f_BroadbandUsername,f_BroadbandPWD,f_Teamviewer,f_MenuBread,f_PriceTier,f_Status,f_Remark,f_Longitude,f_Latitude ";
        public const string FROM_TABLE = " from [sys_BKStoreInfo] ";
        public const string TABLE = " sys_BKStoreInfo ";
        private const string INSET = " (f_GlobalCode,f_LocalCode,f_Region,f_City,f_StoreType,f_Name,f_Address,f_OpenDate,f_CloseDate,f_Tel,f_FAX,f_Email,f_OM,f_OC,f_ServerIP,f_LANGateway,f_TSI,f_BroadbandUsername,f_BroadbandPWD,f_Teamviewer,f_MenuBread,f_PriceTier,f_Status,f_Remark,f_Longitude,f_Latitude) values(@GlobalCode,@LocalCode,@Region,@City,@StoreType,@Name,@Address,@OpenDate,@CloseDate,@Tel,@FAX,@Email,@OM,@OC,@ServerIP,@LANGateway,@TSI,@BroadbandUsername,@BroadbandPWD,@Teamviewer,@MenuBread,@PriceTier,@Status,@Remark,@Longitude,@Latitude)  ";
        private const string UPDATE = " f_GlobalCode=@GlobalCode,f_LocalCode=@LocalCode,f_Region=@Region,f_City=@City,f_StoreType=@StoreType,f_Name=@Name,f_Tel=@Tel,f_FAX=@FAX,f_Email=@Email,f_OM=@OM,f_OC=@OC,f_ServerIP=@ServerIP,f_LANGateway=@LANGateway,f_TSI=@TSI,f_BroadbandUsername=@BroadbandUsername,f_BroadbandPWD=@BroadbandPWD  ,f_Teamviewer=@Teamviewer,f_MenuBread=@MenuBread,f_PriceTier=@PriceTier,f_Status=@Status,f_Remark=@Remark,f_Longitude=@Longitude,f_Latitude=@Latitude ";

        #region ReadyData
        private BKStoreInfo GetByDataReader(SqlDataReader rdr)
        {
            BKStoreInfo info = new BKStoreInfo();
            info.ID = Convert.ToInt32(rdr["ID"]);
            info.GlobalCode = rdr["f_GlobalCode"].ToString();
            info.LocalCode = rdr["f_LocalCode"].ToString();
            info.Region = rdr["f_Region"].ToString();
            info.City = rdr["f_City"].ToString();
            info.StoreType = rdr["f_StoreType"].ToString();
            info.Name = rdr["f_Name"].ToString();
            info.Address = rdr["f_Address"].ToString();
            info.OpenDate = Convert.ToDateTime(rdr["f_OpenDate"]);
            info.CloseDate = Convert.ToDateTime(rdr["f_CloseDate"]);
            info.Tel = rdr["f_Tel"].ToString();
            info.FAX = rdr["f_FAX"].ToString();
            info.Email = rdr["f_Email"].ToString();
            info.OM = rdr["f_OM"].ToString();
            info.OC = rdr["f_OC"].ToString();
            info.ServerIP = rdr["f_ServerIP"].ToString();
            info.LANGateway = rdr["f_LANGateway"].ToString();

            info.TSI = rdr["f_TSI"].ToString();
            info.BroadbandUsername = rdr["f_BroadbandUsername"].ToString();
            info.BroadbandPWD = rdr["f_BroadbandPWD"].ToString();
            info.Teamviewer = rdr["f_Teamviewer"].ToString();
            info.MenuBread = rdr["f_MenuBread"].ToString();
            info.PriceTier = rdr["f_PriceTier"].ToString();
            info.Status = rdr["f_Status"].ToString();
            info.Remark = rdr["f_Remark"].ToString();
            info.Longitude = rdr["f_Longitude"].ToString();
            info.Latitude = rdr["f_Latitude"].ToString();
            return info;
        }

        private SqlParameter[] GetParameter(BKStoreInfo info)
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter("@GlobalCode", info.GlobalCode),
            new SqlParameter("@LocalCode", info.LocalCode),
            new SqlParameter("@Region", info.Region),
            new SqlParameter("@City", info.City),
            new SqlParameter("@StoreType", info.StoreType),
            new SqlParameter("@Name", info.Name),
            new SqlParameter("@Address", info.Address),
            new SqlParameter("@OpenDate", info.OpenDate),
            new SqlParameter("@CloseDate", info.CloseDate),
            new SqlParameter("@Tel", info.Tel),
            new SqlParameter("@FAX", info.FAX),
            new SqlParameter("@Email", info.Email),
            new SqlParameter("@OM", info.OM),
            new SqlParameter("@OC", info.OC),
            new SqlParameter("@ServerIP", info.ServerIP),
            new SqlParameter("@LANGateway", info.LANGateway),

            new SqlParameter("@TSI", info.TSI),
            new SqlParameter("@BroadbandUsername", info.BroadbandUsername),
            new SqlParameter("@BroadbandPWD", info.BroadbandPWD),
            new SqlParameter("@Teamviewer", info.Teamviewer),
            new SqlParameter("@MenuBread", info.MenuBread),
            new SqlParameter("@PriceTier", info.PriceTier),
            new SqlParameter("@Status", info.Status),
            new SqlParameter("@Remark", info.Remark),
            new SqlParameter("@Longitude", info.Longitude),
            new SqlParameter("@Latitude", info.Latitude),
            
            };

            return parms;
        }
        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="StrWhere"></param>
        /// <returns></returns>
        public List<BKStoreInfo> GetList(string StrWhere)
        {
            List<BKStoreInfo> list = new List<BKStoreInfo>();
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
        /// 根据店铺号查找
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public BKStoreInfo GetByStoreNo(string LocalCode)
        {
            StringBuilder strSQL = new StringBuilder();
            SqlParameter[] parms = new SqlParameter[] { new SqlParameter("@LocalCode", LocalCode) };

            strSQL.Append("select top 1 ").Append(ALL_PARM).Append(FROM_TABLE).Append(" WHERE f_LocalCode=@LocalCode ");
            using (SqlDataReader rdr = SqlHelper.ExecuteReader(SqlHelper.SqlconnString, CommandType.Text, strSQL.ToString(), parms))
            {
                if (!rdr.Read()) return null;

                return GetByDataReader(rdr);
            }
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="info">info</param>
        public int Add(BKStoreInfo info)
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
        public bool Edit(BKStoreInfo info)
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
    }
}
