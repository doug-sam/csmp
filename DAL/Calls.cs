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
    public class CallDAL
    {
        private const string ALL_PARM = "  ID,f_No,f_CreatorID,f_CreatorName,f_CreateDate,f_CustomerID,f_ErrorReportUser,f_CustomerName,f_BrandID,f_BrandName,f_ProvinceID,f_ProvinceName,f_CityID,f_CityName,f_StoreID,f_StoreName,f_ReporterName,f_ReportSourceID,f_ReportSourceName,f_ReportSourceNo,f_Class1,f_Class2,f_Class3,f_ClassName1,f_ClassName2,f_ClassName3,f_PriorityID,f_PriorityName,f_Details,f_MaintainUserID,f_MaintaimUserName,f_ErrorDate,f_StateMain,f_StateDetail,f_SuggestSlnID,f_SuggestSlnName,f_SlnID,f_SlnName,f_SloveBy,f_SLA,f_StoreNo,f_SLAMinute,f_WorkGroupID,f_AssignUserID,f_AssignID,f_AssignUserName,f_IsClosed,f_CallNo2,f_CallNo3,f_VideoSrc,f_VideoID,f_FinishDate,f_IsSameCall,f_SLA2,f_ReplacementStatus,f_SLADateEnd,f_Category ";
        private const string FROM_TABLE = " from [sys_Calls] ";
        public const string TABLE = " sys_Calls ";
        private const string INSET = " (f_No,f_CreatorID,f_CreatorName,f_CreateDate,f_CustomerID,f_ErrorReportUser,f_CustomerName,f_BrandID,f_BrandName,f_ProvinceID,f_ProvinceName,f_CityID,f_CityName,f_StoreID,f_StoreName,f_ReporterName,f_ReportSourceID,f_ReportSourceName,f_ReportSourceNo,f_Class1,f_Class2,f_Class3,f_ClassName1,f_ClassName2,f_ClassName3,f_PriorityID,f_PriorityName,f_Details,f_MaintainUserID,f_MaintaimUserName,f_ErrorDate,f_StateMain,f_StateDetail,f_SuggestSlnID,f_SuggestSlnName,f_SlnID,f_SlnName,f_SloveBy,f_SLA,f_StoreNo,f_SLAMinute,f_WorkGroupID,f_AssignUserID,f_AssignID,f_AssignUserName,f_IsClosed,f_CallNo2,f_CallNo3,f_VideoSrc,f_VideoID,f_FinishDate,f_IsSameCall,f_SLA2,f_ReplacementStatus,f_SLADateEnd,f_Category) values(@No,@CreatorID,@CreatorName,@CreateDate,@CustomerID,@ErrorReportUser,@CustomerName,@BrandID,@BrandName,@ProvinceID,@ProvinceName,@CityID,@CityName,@StoreID,@StoreName,@ReporterName,@ReportSourceID,@ReportSourceName,@ReportSourceNo,@Class1,@Class2,@Class3,@ClassName1,@ClassName2,@ClassName3,@PriorityID,@PriorityName,@Details,@MaintainUserID,@MaintaimUserName,@ErrorDate,@StateMain,@StateDetail,@SuggestSlnID,@SuggestSlnName,@SlnID,@SlnName,@SloveBy,@SLA,@StoreNo,@SLAMinute,@WorkGroupID,@AssignUserID,@AssignID,@AssignUserName,@IsClosed,@CallNo2,@CallNo3,@VideoSrc,@VideoID,@FinishDate,@IsSameCall,@SLA2,@ReplacementStatus,@SLADateEnd,@Category)  ";
        private const string UPDATE = " f_No=@No,f_CreatorID=@CreatorID,f_CreatorName=@CreatorName,f_CreateDate=@CreateDate,f_CustomerID=@CustomerID,f_ErrorReportUser=@ErrorReportUser,f_CustomerName=@CustomerName,f_BrandID=@BrandID,f_BrandName=@BrandName,f_ProvinceID=@ProvinceID,f_ProvinceName=@ProvinceName,f_CityID=@CityID,f_CityName=@CityName,f_StoreID=@StoreID,f_StoreName=@StoreName,f_ReporterName=@ReporterName,f_ReportSourceID=@ReportSourceID,f_ReportSourceName=@ReportSourceName,f_ReportSourceNo=@ReportSourceNo,f_Class1=@Class1,f_Class2=@Class2,f_Class3=@Class3,f_ClassName1=@ClassName1,f_ClassName2=@ClassName2,f_ClassName3=@ClassName3,f_PriorityID=@PriorityID,f_PriorityName=@PriorityName,f_Details=@Details,f_MaintainUserID=@MaintainUserID,f_MaintaimUserName=@MaintaimUserName,f_ErrorDate=@ErrorDate,f_StateMain=@StateMain,f_StateDetail=@StateDetail,f_SuggestSlnID=@SuggestSlnID,f_SuggestSlnName=@SuggestSlnName,f_SlnID=@SlnID,f_SlnName=@SlnName,f_SloveBy=@SloveBy,f_SLA=@SLA,f_StoreNo=@StoreNo,f_SLAMinute=@SLAMinute,f_WorkGroupID=@WorkGroupID,f_AssignUserID=@AssignUserID,f_AssignID=@AssignID,f_AssignUserName=@AssignUserName,f_IsClosed=@IsClosed,f_CallNo2=@CallNo2,f_CallNo3=@CallNo3,f_VideoSrc=@VideoSrc,f_VideoID=@VideoID,f_FinishDate=@FinishDate,f_IsSameCall=@IsSameCall,f_SLA2=@SLA2,f_ReplacementStatus=@ReplacementStatus,f_SLADateEnd=@SLADateEnd,f_Category=@Category ";

        #region ReadyData
        private CallInfo GetByDataReader(SqlDataReader rdr)
        {
            CallInfo info = new CallInfo();
            info.ID = Convert.ToInt32(rdr["ID"]);
            info.No = rdr["f_No"].ToString();
            info.CreatorID = Convert.ToInt32(rdr["f_CreatorID"]);
            info.CreatorName = rdr["f_CreatorName"].ToString();
            info.CreateDate = Convert.ToDateTime(rdr["f_CreateDate"]);
            info.CustomerID = Convert.ToInt32(rdr["f_CustomerID"]);
            info.ErrorReportUser = rdr["f_ErrorReportUser"].ToString();
            info.CustomerName = rdr["f_CustomerName"].ToString();
            info.BrandID = Convert.ToInt32(rdr["f_BrandID"]);
            info.BrandName = rdr["f_BrandName"].ToString();
            info.ProvinceID = Convert.ToInt32(rdr["f_ProvinceID"]);
            info.ProvinceName = rdr["f_ProvinceName"].ToString();
            info.CityID = Convert.ToInt32(rdr["f_CityID"]);
            info.CityName = rdr["f_CityName"].ToString();
            info.StoreID = Convert.ToInt32(rdr["f_StoreID"]);
            info.StoreName = rdr["f_StoreName"].ToString();
            info.ReporterName = rdr["f_ReporterName"].ToString();
            info.ReportSourceID = Convert.ToInt32(rdr["f_ReportSourceID"]);
            info.ReportSourceName = rdr["f_ReportSourceName"].ToString();
            info.ReportSourceNo = rdr["f_ReportSourceNo"].ToString();
            info.Class1 = Convert.ToInt32(rdr["f_Class1"]);
            info.Class2 = Convert.ToInt32(rdr["f_Class2"]);
            info.Class3 = Convert.ToInt32(rdr["f_Class3"]);
            info.ClassName1 = rdr["f_ClassName1"].ToString();
            info.ClassName2 = rdr["f_ClassName2"].ToString();
            info.ClassName3 = rdr["f_ClassName3"].ToString();
            info.PriorityID = Convert.ToInt32(rdr["f_PriorityID"]);
            info.PriorityName = rdr["f_PriorityName"].ToString();
            info.Details = rdr["f_Details"].ToString();
            info.MaintainUserID = Convert.ToInt32(rdr["f_MaintainUserID"]);
            info.MaintaimUserName = rdr["f_MaintaimUserName"].ToString();
            info.ErrorDate = Convert.ToDateTime(rdr["f_ErrorDate"]);
            info.StateMain = Convert.ToInt32(rdr["f_StateMain"]);
            info.StateDetail = Convert.ToInt32(rdr["f_StateDetail"]);
            info.SuggestSlnID = Convert.ToInt32(rdr["f_SuggestSlnID"]);
            info.SuggestSlnName = rdr["f_SuggestSlnName"].ToString();
            info.SlnID = Convert.ToInt32(rdr["f_SlnID"]);
            info.SlnName = rdr["f_SlnName"].ToString();
            info.SloveBy = rdr["f_SloveBy"].ToString();
            info.SLA = Convert.ToInt32(rdr["f_SLA"]);
            info.StoreNo = rdr["f_StoreNo"].ToString();
            info.SLAMinute = Convert.ToInt32(rdr["f_SLAMinute"]);
            info.WorkGroupID = Convert.ToInt32(rdr["f_WorkGroupID"]);
            info.AssignUserID = Convert.ToInt32(rdr["f_AssignUserID"]);
            info.AssignID = Convert.ToInt32(rdr["f_AssignID"]);
            info.AssignUserName = rdr["f_AssignUserName"].ToString();
            info.IsClosed = Convert.ToBoolean(rdr["f_IsClosed"]);
            info.CallNo2 = rdr["f_CallNo2"].ToString();
            info.CallNo3 = rdr["f_CallNo3"].ToString();
            info.VideoSrc = rdr["f_VideoSrc"].ToString();
            info.VideoID = rdr["f_VideoID"].ToString();
            info.FinishDate = Convert.ToDateTime(rdr["f_FinishDate"]);
            info.IsSameCall = Convert.ToBoolean(rdr["f_IsSameCall"]);
            info.SLA2 = rdr["f_SLA2"].ToString();
            info.ReplacementStatus = Convert.ToInt32(rdr["f_ReplacementStatus"]);
            info.SLADateEnd =Convert.ToDateTime(rdr["f_SLADateEnd"]);
            info.Category = Convert.ToInt32(rdr["f_Category"]);
            return info;
        }

        private SqlParameter[] GetParameter(CallInfo info)
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter("@No", info.No),
            new SqlParameter("@CreatorID", info.CreatorID),
            new SqlParameter("@CreatorName", info.CreatorName),
            new SqlParameter("@CreateDate", info.CreateDate),
            new SqlParameter("@CustomerID", info.CustomerID),
            new SqlParameter("@ErrorReportUser", info.ErrorReportUser),
            new SqlParameter("@CustomerName", info.CustomerName),
            new SqlParameter("@BrandID", info.BrandID),
            new SqlParameter("@BrandName", info.BrandName),
            new SqlParameter("@ProvinceID", info.ProvinceID),
            new SqlParameter("@ProvinceName", info.ProvinceName),
            new SqlParameter("@CityID", info.CityID),
            new SqlParameter("@CityName", info.CityName),
            new SqlParameter("@StoreID", info.StoreID),
            new SqlParameter("@StoreName", info.StoreName),
            new SqlParameter("@ReporterName", info.ReporterName),
            new SqlParameter("@ReportSourceID", info.ReportSourceID),
            new SqlParameter("@ReportSourceName", info.ReportSourceName),
            new SqlParameter("@ReportSourceNo", info.ReportSourceNo),
            new SqlParameter("@Class1", info.Class1),
            new SqlParameter("@Class2", info.Class2),
            new SqlParameter("@Class3", info.Class3),
            new SqlParameter("@ClassName1", info.ClassName1),
            new SqlParameter("@ClassName2", info.ClassName2),
            new SqlParameter("@ClassName3", info.ClassName3),
            new SqlParameter("@PriorityID", info.PriorityID),
            new SqlParameter("@PriorityName", info.PriorityName),
            new SqlParameter("@Details", info.Details),
            new SqlParameter("@MaintainUserID", info.MaintainUserID),
            new SqlParameter("@MaintaimUserName", info.MaintaimUserName),
            new SqlParameter("@ErrorDate", info.ErrorDate),
            new SqlParameter("@StateMain", info.StateMain),
            new SqlParameter("@StateDetail", info.StateDetail),
            new SqlParameter("@SuggestSlnID", info.SuggestSlnID),
            new SqlParameter("@SuggestSlnName", info.SuggestSlnName),
            new SqlParameter("@SlnID", info.SlnID),
            new SqlParameter("@SlnName", info.SlnName),
            new SqlParameter("@SloveBy", info.SloveBy),
            new SqlParameter("@SLA", info.SLA),
            new SqlParameter("@StoreNo", info.StoreNo),
            new SqlParameter("@SLAMinute", info.SLAMinute),
            new SqlParameter("@WorkGroupID", info.WorkGroupID),
            new SqlParameter("@AssignUserID", info.AssignUserID),
            new SqlParameter("@AssignID", info.AssignID),
            new SqlParameter("@AssignUserName", info.AssignUserName),
            new SqlParameter("@IsClosed", info.IsClosed),
            new SqlParameter("@CallNo2", info.CallNo2),
            new SqlParameter("@CallNo3", info.CallNo3),
            new SqlParameter("@VideoSrc", info.VideoSrc),
            new SqlParameter("@VideoID", info.VideoID),
            new SqlParameter("@FinishDate", info.FinishDate),
            new SqlParameter("@IsSameCall", info.IsSameCall),
            new SqlParameter("@SLA2", info.SLA2),
            new SqlParameter("@ReplacementStatus", info.ReplacementStatus),
            new SqlParameter("@SLADateEnd", info.SLADateEnd),
            new SqlParameter("@Category", info.Category),
            
            };

            return parms;
        }


        //private List<CallInfo> GetByDataTable(DataTable dt)
        //{
        //    List<CallInfo> dataList = new List<CallInfo>();
        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        CallInfo info = new CallInfo();
        //        info.No = dr["f_No"].ToString();
        //        info.CustomerName = dr["f_CustomerName"].ToString();
        //        info.BrandName = dr["f_BrandName"].ToString();
        //        info.Details = dr["f_Details"].ToString();
        //        info.a = dr["f_Address"].ToString();
        //        info.StoreUrgency = Convert.ToInt32(dr["f_StoreUrgency"].ToString());
        //        info.StoreRequest = Convert.ToInt32(dr["f_StoreRequest"].ToString());
        //        info.UserID = Convert.ToInt32(dr["f_UserID"].ToString());
        //        info.WorkGroupID = Convert.ToInt32(dr["f_WorkGroupID"].ToString());
        //        info.HaveGroupPower = Convert.ToBoolean(dr["f_HaveGroupPower"].ToString());

        //        dataList.Add(info);
        //    }


        //    return dataList;
        //}
        #endregion


        #region Get
        /// <summary>
        /// 获取列表
        /// </summary>
        public List<CallInfo> GetList(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            List<CallInfo> list = new List<CallInfo>();
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

        public List<CallInfo> GetList(string StrWhere)
        {
            List<CallInfo> list = new List<CallInfo>();
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
        public CallInfo Get(int id)
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
        /// <param name="No">No</param>
        public CallInfo Get(string No)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("select top 1 ").Append(ALL_PARM).Append(FROM_TABLE).Append(" where f_No = '").Append(No).Append("'");

            using (SqlDataReader rdr = SqlHelper.ExecuteReader(SqlHelper.SqlconnString, CommandType.Text, strSQL.ToString(), null))
            {
                if (!rdr.Read()) return null;

                return GetByDataReader(rdr);
            }
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        public List<CallInfo> GetList(int PageSize, int CurPage, int WorkGroupID, out int Count)
        {
            return GetList(PageSize, CurPage, " 1=1 " + strWhereSln1sql(WorkGroupID), out Count);
        }


        /// <summary>
        /// 获取call数
        /// </summary>
        /// <param name="StateMain">状态</param>
        /// <param name="UserID">帐户ID</param>
        /// <returns></returns>
        public int GetCount(int StateMain, int UserID, int WorkGroupID)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("SELECT COUNT(1) ").Append(FROM_TABLE).Append(" WHERE 1=1 ");
            if (UserID > 0) strSQL.Append(" AND (f_MaintainUserID=").Append(UserID).Append(" OR f_CreatorID=").Append(UserID).Append(" ) ");
            if (WorkGroupID > 0)
            {
                strSQL.Append(" and f_BrandID IN(");
                strSQL.Append("                     SELECT sys_WorkGroupBrand.f_MID FROM sys_WorkGroupBrand WHERE sys_WorkGroupBrand.f_WorkGroupID=").Append(WorkGroupID);
                strSQL.Append("                 )");
            }
            if (StateMain > 0) strSQL.Append(" AND f_StateMain=").Append(StateMain);

            return Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.SqlconnString, CommandType.Text, strSQL.ToString(), null));
        }

        /// <summary>
        /// 根据工作组找出等待上门call数
        /// </summary>
        /// <param name="WorkGroup"></param>
        /// <returns></returns>
        public int GetCountSln1(int WorkGroup)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("SELECT COUNT(1) ").Append(FROM_TABLE);
            strSQL.Append(" WHERE 1=1 ");
            strSQL.Append(strWhereSln1sql(WorkGroup));

            return Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.SqlconnString, CommandType.Text, strSQL.ToString(), null));
        }

        private static string strWhereSln1sql(int WorkGroup)
        {
            StringBuilder strSQL = new StringBuilder();

            strSQL.Append(" and f_StateMain=").Append((int)SysEnum.CallStateMain.处理中);
            strSQL.Append(" and (f_StateDetail in(").Append((int)SysEnum.CallStateDetails.等待安排上门);
            strSQL.Append(" , ").Append((int)SysEnum.CallStateDetails.等待备件);
            strSQL.Append(") ) ");
            if (WorkGroup > 0) strSQL.Append(" AND f_BrandID in(select f_MID ").Append(WorkGroupBrandDAL.FROM_TABLE).Append(" where f_WorkGroupID=").Append(WorkGroup).Append(") ");
            return strSQL.ToString();
        }
        
        /// <summary>
        /// 查询现场工程师负责的call列表
        /// </summary>
        /// <param name="StateMain">2代表处理中，3代表完成</param>
        /// <param name="UserID">现场工程师的userid</param>
        /// <param name="WorkGroupID">工作组ID，暂不使用</param>
        /// <returns></returns>
        public List<CallInfo> GetMyCallsForOnsiteEngineer(int StateMain, int UserID, int WorkGroupID)
        {
            //ZQL 20151013 为APP接口编写
            StringBuilder strSQL = new StringBuilder();
            List<CallInfo> list = new List<CallInfo>();
            strSQL.Append("SELECT " + ALL_PARM + " FROM sys_Calls ");
            strSQL.Append(" inner join ");
            strSQL.Append(" (select f_MajorUserID,f_MajorUserName,CallStep.f_CallID  from sys_CallStep CallStep,(select f_CallID ,max(f_StepIndex) as maxid from sys_CallStep where f_StepType=3 and f_AddDate> DATEADD(MM,-12,GETDATE()) group by f_callid) MAXIndexCallStep where CallStep.f_callid=MAXIndexCallStep.f_CallID and CallStep.f_StepIndex=MAXIndexCallStep.maxid and f_MajorUserID =" + UserID + ") sysCallStep ");
            strSQL.Append(" on sys_Calls.ID = sysCallStep.f_CallID where f_StateMain = " + StateMain + " and f_CreateDate>DATEADD(MM,-6,GETDATE()) ORDER BY sys_Calls.f_CreateDate DESC ");
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
        /// 查询现场工程师负责的call列表，APP查询我的工单专用，调用SP实现
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="returnValue"></param>
        /// <returns></returns>
        public DataTable GetMyCallsForOnsiteEngineerBySp(string userName,out string returnValue )
        {
            StoreProcedure sp = new StoreProcedure("sp_APP_MyOrder");//类的对象
            Object[] paraValues = new object[2];//注意,这里是存储过程中全部的参数,一共有三个,还要注意顺序啊,返回值是第一个,那么赋值时第一个参数就为空
            paraValues[0] = userName;//从第二个参数开始赋值
            paraValues[1] = "";
            object[] output = new object[1];
            DataTable dt = new DataTable();
            dt = sp.ExecuteDataTable(out output, paraValues);
            returnValue = output[0].ToString();
            return dt;
        }


        /// <summary>
        /// 查询现场工程师负责的历史call列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="UserID"></param>
        /// <param name="WorkGroupID"></param>
        /// <returns></returns>
        public List<CallInfo> GetHistoryCallsForOnsiteEngineer(int pageIndex,int pageSize, int UserID, int WorkGroupID)
        {
            //ZQL 20151013 为APP接口编写
            int count = 100000;
            int pageCount = 0;
            
            StringBuilder strSQLCount = new StringBuilder();
            strSQLCount.Append("SELECT COUNT(1) FROM sys_Calls ");
            strSQLCount.Append(" inner join ");
            strSQLCount.Append(" (select f_MajorUserID,f_MajorUserName,CallStep.f_CallID  from sys_CallStep CallStep,(select f_CallID ,max(f_StepIndex) as maxid from sys_CallStep where f_StepType=3 and f_AddDate> DATEADD(MM,-12,GETDATE()) group by f_callid) MAXIndexCallStep where CallStep.f_callid=MAXIndexCallStep.f_CallID and CallStep.f_StepIndex=MAXIndexCallStep.maxid and f_MajorUserID =" + UserID + ") sysCallStep ");
            strSQLCount.Append(" on sys_Calls.ID = sysCallStep.f_CallID where (f_StateMain = 3 or f_StateMain = 4 ) AND f_SloveBy='上门' and f_CreateDate>DATEADD(MM,-6,GETDATE()) ");
            using (SqlDataReader rdr = SqlHelper.ExecuteReader(SqlHelper.SqlconnString, CommandType.Text, strSQLCount.ToString(), null))
            count =  Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.SqlconnString, CommandType.Text, strSQLCount.ToString(), null));
            pageCount = count / pageSize;
            int curPageSize = pageSize;
            if (count % pageSize>0)
            {
                pageCount += 1;
                curPageSize=count % pageSize;
            }
            if(pageCount==0)
            {
                return null;
            }
            int top1 = 0;
            int top2 = 0;
            if (pageIndex >= pageCount && curPageSize > 0)
            {
                top1 = curPageSize;
            }
            else {
                top1 = pageSize;
            }
            top2 = (pageIndex - 1) * pageSize + top1;
            StringBuilder strSQL = new StringBuilder();
            List<CallInfo> list = new List<CallInfo>();
            strSQL.Append("SELECT B.* FROM ( ");
            strSQL.Append("SELECT TOP " + top1 + " A.* FROM (");
            strSQL.Append("SELECT TOP " + top2 + ALL_PARM + "  FROM sys_Calls inner join ");
            strSQL.Append(" (select f_MajorUserID,f_MajorUserName,CallStep.f_CallID  from sys_CallStep CallStep,(select f_CallID ,max(f_StepIndex) as maxid from sys_CallStep where f_StepType=3 and f_AddDate> DATEADD(MM,-6,GETDATE()) group by f_callid) MAXIndexCallStep where CallStep.f_callid=MAXIndexCallStep.f_CallID and CallStep.f_StepIndex=MAXIndexCallStep.maxid and f_MajorUserID =" + UserID + ") sysCallStep ");
            strSQL.Append(" on sys_Calls.ID = sysCallStep.f_CallID where (f_StateMain = 3 or f_StateMain = 4 ) AND f_SloveBy='上门' and f_CreateDate>DATEADD(MM,-6,GETDATE()) ORDER BY sys_Calls.f_CreateDate DESC ");
            strSQL.Append(") A ORDER BY  A.f_CreateDate ASC");
            strSQL.Append(" ) B ORDER BY B.f_CreateDate DESC");

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
        /// 工单查询
        /// </summary>
        /// <param name="StateMain"></param>
        /// <param name="StateDetail"></param>
        /// <param name="UserID"></param>
        /// <param name="WorkGroupID"></param>
        /// <param name="CustomeName"></param>
        /// <param name="BrandName"></param>
        /// <returns></returns>
        public List<CallInfo> GetMyCallsForOnsiteEngineer(int StateMain, int StateDetail,int UserID, int WorkGroupID,string  CustomeName,string BrandName)
        {
            //ZQL 20151013 为APP接口编写
            StringBuilder strSQL = new StringBuilder();
            List<CallInfo> list = new List<CallInfo>();
            strSQL.Append("SELECT " + ALL_PARM + ",sysCallStep.UserWorkgroupID FROM sys_Calls ");
            strSQL.Append(" inner join ");
            strSQL.Append(" (select f_MajorUserID,f_MajorUserName,CallStep.f_CallID ,sys_User.f_WorkGroupID UserWorkgroupID from sys_CallStep CallStep,(select f_CallID ,max(f_StepIndex) as maxid from sys_CallStep where f_StepType=3 and f_AddDate> DATEADD(MM,-12,GETDATE()) group by f_callid) MAXIndexCallStep ,sys_user where CallStep.f_callid=MAXIndexCallStep.f_CallID and CallStep.f_StepIndex=MAXIndexCallStep.maxid and sys_user.ID=CallStep.f_MajorUserID ) sysCallStep ");
            strSQL.Append(" on sys_Calls.ID = sysCallStep.f_CallID where  f_CreateDate>DATEADD(MM,-6,GETDATE()) ");
            strSQL.Append(" and f_StateMain = " + StateMain );
            if (StateMain == 2)
            {
                if (StateDetail == 100)
                {
                    strSQL.Append(" and (f_StateDetail = " + (int)SysEnum.CallStateDetails.等待工程师上门+" or f_StateDetail ="+(int)SysEnum.CallStateDetails.等待第三方响应+") ");
                }
                else {
                    strSQL.Append(" and f_StateDetail = " + StateDetail);
                }
                
            }
            if (UserID >0)
            {
                strSQL.Append(" and f_MajorUserID  = " + UserID);
            }
            if (WorkGroupID > 0)
            {
                strSQL.Append(" and UserWorkgroupID  = " + WorkGroupID);
            }
            if (!string.IsNullOrEmpty(CustomeName))
            {
                strSQL.Append(" and f_CustomerName = '" + CustomeName+"' ");
            }
            if (!string.IsNullOrEmpty(BrandName))
            {
                strSQL.Append(" and f_BrandName = '" + BrandName + "' ");
            }

            strSQL.Append(" ORDER BY sys_Calls.f_CreateDate DESC ");
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
        /// 工单查询SP实现
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="state"></param>
        /// <param name="getGroup"></param>
        /// <param name="CustomeName"></param>
        /// <param name="BrandName"></param>
        /// <param name="returnValue"></param>
        /// <returns></returns>
        public DataTable GetMyCallsForOnsiteEngineerBySp(string userName, string state, string getGroup, string CustomeName, string BrandName, string errorStartTime, string errorEndTime, out string returnValue)
        {
            StoreProcedure sp = new StoreProcedure("sp_APP_OrderSearch");//类的对象
            Object[] paraValues = new object[8];//注意,这里是存储过程中全部的参数,一共有三个,还要注意顺序啊,返回值是第一个,那么赋值时第一个参数就为空

            paraValues[0] = userName;//从第二个参数开始赋值
            paraValues[1] = CustomeName;
            paraValues[2] = BrandName;
            paraValues[3] = state;
            paraValues[4] = errorStartTime;
            paraValues[5] = errorEndTime;
            paraValues[6] = getGroup;
            paraValues[7] = "";
            object[] output = new object[1];
            DataTable dt = new DataTable() ;
            dt=sp.ExecuteDataTable(out output, paraValues);
            returnValue = output[0].ToString();
            return dt;
        
        }

        #endregion


        #region set
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="info">info</param>
        public int Add(CallInfo info)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("insert into ").Append(TABLE).Append(INSET);

            SqlParameter[] parms = GetParameter(info);

            if (SqlHelper.ExecuteNonQueryByTran(SqlHelper.SqlconnString, CommandType.Text, strSQL.ToString(), parms))
            {
                return Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.SqlconnString, CommandType.Text, "select id from " + TABLE + " where f_No='" + info.No+ "';", null).ToString());
                //return Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.SqlconnString, CommandType.Text, "select max(id) from " + TABLE, null).ToString());
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
        public bool Edit(CallInfo info)
        {
            StringBuilder strSQL;
            SqlParameter[] parms;
            EditSQL(info, out strSQL, out parms);

            return SqlHelper.ExecuteNonQueryByTran(SqlHelper.SqlconnString, CommandType.Text, strSQL.ToString(), parms);
        }


        public void EditSQL(CallInfo info, out StringBuilder strSQL, out SqlParameter[] parms)
        {
            strSQL = new StringBuilder();
            strSQL.Append("update ").Append(TABLE).Append(" set ").Append(UPDATE).Append(" where id = ").Append(info.ID);

            parms = GetParameter(info);
        }


        /// <summary>
        /// 删除Member
        /// </summary>
        /// <param name="id">Member id</param>
        public bool Delete(int id)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append(" delete from sys_DropInMemo where f_StepID in(select ID from sys_CallStep where f_CallID=").Append(id).Append(")");
            strSQL.Append(" delete from sys_CallStep where f_CallID=").Append(id);
            strSQL.Append(" delete from sys_Assign where f_CallID=").Append(id);
            strSQL.Append(" delete ").Append(FROM_TABLE).Append(" where id = ").Append(id);

            return SqlHelper.ExecuteNonQueryByTran(SqlHelper.SqlconnString, CommandType.Text, strSQL.ToString(), null);
        }

        /// <summary>
        /// 利用存储过程更新f_SLADateEnd列
        /// </summary>
        /// <param name="id">CALL id</param>
        public bool UpdateSLADateEnd(int id)
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter("@id", id),
            };
            return SqlHelper.ExecuteNonQueryByTran(SqlHelper.SqlconnString, CommandType.StoredProcedure, "cp_Call_GetEndTime", parms);
        }

        
        #endregion

        public static StringBuilder DeleteByStoreIDsql(string IDOrSelectIDSQL)
        {
            StringBuilder sb_CallIDs = new StringBuilder();
            sb_CallIDs.Append("select ID").Append(FROM_TABLE).Append(" where f_StoreID in (").Append(IDOrSelectIDSQL).Append(") ");

            StringBuilder strSQL = new StringBuilder();
            strSQL.Append(CallStepDAL.DeleteByCallIDsql(sb_CallIDs.ToString())).Append(" ");
            strSQL.Append(BillRecDAL.DeleteByCallIDsql(sb_CallIDs.ToString())).Append(" ");
            strSQL.Append("delete ").Append(FROM_TABLE).Append(" where id in (").Append(sb_CallIDs.ToString()).Append(" ) ");
            return strSQL;
        }
        public static StringBuilder DeleteByClass3IDsql(string IDOrSelectIDSQL)
        {
            StringBuilder sb_CallIDs = new StringBuilder();
            sb_CallIDs.Append("select ID").Append(FROM_TABLE).Append(" where f_Class3 in (").Append(IDOrSelectIDSQL).Append(") ");

            StringBuilder strSQL = new StringBuilder();
            strSQL.Append(CallStepDAL.DeleteByCallIDsql(sb_CallIDs.ToString()));
            strSQL.Append(BillRecDAL.DeleteByCallIDsql(sb_CallIDs.ToString())).Append(" ");
            strSQL.Append("delete ").Append(FROM_TABLE).Append(" where id in (").Append(sb_CallIDs.ToString()).Append(" ) ");
            return strSQL;
        }
        /// <summary>
        /// 插入汉堡王Task
        /// </summary>
        /// <param name="sqlStr">sql语句</param>
        /// <returns></returns>
        public int AddBurgerKingTask(string sqlStr)
        { 
            int  records = Convert.ToInt32(SqlHelper.ExecuteNonQuery(CommandType.Text, sqlStr, null));
            return records;
        }

    }
}