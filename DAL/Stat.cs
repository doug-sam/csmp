using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using DBUtility;
using CSMP.Model;
using Tool;
using System.Data.OleDb;

namespace CSMP.DAL
{
    public class StatDAL
    {
        public const string FROM_TABLE = " from [sys_Calls] ";
        public const string TABLE = " sys_Calls ";
        #region Stat
        public DataTable StatCity(DateTime DateBegin, DateTime DateEnd, int CustomerID, int BrandID, UserInfo uinfo)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT top 10 COUNT(1) AS F_COUNT,f_CityID,f_CityName as CityName FROM sys_Calls ");
            sb.Append("             WHERE DATEDIFF(DAY,f_ErrorDate,@v_DateBegin)<=0 ");
            sb.Append("                 AND DATEDIFF(DAY,f_ErrorDate,@v_DateEnd)>=0 ");
            if (CustomerID > 0)
            {
                sb.Append("             AND sys_Calls.f_CustomerID=").Append(CustomerID);
            }
            if (BrandID > 0)
            {
                sb.Append("             AND sys_Calls.f_BrandID=").Append(BrandID);
            }
            if (null != uinfo)
            {
                sb.Append(" and f_BrandID in(");
                sb.Append("             select f_MID from sys_WorkGroupBrand where f_WorkGroupID=").Append(uinfo.WorkGroupID);
                sb.Append("                 ) ");
            }
            sb.Append("         GROUP BY f_CityName,f_CityID");
            sb.Append("         ORDER BY F_COUNT DESC ");
            SqlParameter[] paraList = new SqlParameter[]{
                new SqlParameter("@v_DateBegin",DateBegin),
                new SqlParameter("@v_DateEnd",DateEnd),
            };
            return SqlHelper.ExecuteDataTable(SqlHelper.SqlconnString, CommandType.Text, sb.ToString(), paraList);

        }
        public DataTable StatCityClass(DateTime DateBegin, DateTime DateEnd)
        {
            SqlParameter[] paraList = new SqlParameter[]{
                new SqlParameter("@v_DateBegin",DateBegin),
                new SqlParameter("@v_DateEnd",DateEnd),
            };
            return SqlHelper.ExecuteDataTable(SqlHelper.SqlconnString, CommandType.StoredProcedure, "cp_Stat_CityClass", paraList);
        }
        public DataTable StatCityHour(DateTime DateBegin, DateTime DateEnd, int CustomerID, int BrandID, int WorkGroupID)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT DATEPART(HOUR, f_CreateDate) AS F_HOUR ,COUNT(1) AS F_COUNT from sys_Calls ");
            sb.Append("     WHERE DATEDIFF(DAY,f_CreateDate,@v_DateBegin)<=0");
            sb.Append("           AND DATEDIFF(DAY,f_CreateDate,@v_DateEnd)>=0");
            if (CustomerID > 0)
            {
                sb.Append("             AND sys_Calls.f_CustomerID=").Append(CustomerID);
            }
            if (BrandID > 0)
            {
                sb.Append("             AND sys_Calls.f_BrandID=").Append(BrandID);
            }
            else
            {
                sb.Append("             AND sys_Calls.f_BrandID IN(SELECT sys_WorkGroupBrand.f_MID FROM sys_WorkGroupBrand WHERE f_WorkGroupID=");
                sb.Append(WorkGroupID);
                sb.Append(")");
            }
            sb.Append(" GROUP BY DATEPART(HOUR, f_CreateDate) ");
            sb.Append(" ORDER BY F_HOUR ");


            SqlParameter[] paraList = new SqlParameter[]{
                new SqlParameter("@v_DateBegin",DateBegin),
                new SqlParameter("@v_DateEnd",DateEnd),
            };
            return SqlHelper.ExecuteDataTable(SqlHelper.SqlconnString, CommandType.Text, sb.ToString(), paraList);
        }
        /// <summary>
        /// 指定月份内，每周各服务方式 数
        /// </summary>
        /// <param name="DateBegin"></param>
        /// <param name="DateEnd"></param>
        /// <returns></returns>
        public DataTable StatWeekCountServerWay(DateTime DateBegin, DateTime DateEnd)
        {
            SqlParameter[] paraList = new SqlParameter[]{
                new SqlParameter("@v_DateBegin",DateBegin),
                new SqlParameter("@v_DateEnd",DateEnd),
            };
            return SqlHelper.ExecuteDataTable(SqlHelper.SqlconnString, CommandType.StoredProcedure, "cp_Stat_WeekCountServerWay", paraList);
        }
        /// <summary>
        /// 指定月份内，每周各类型报修 数
        /// </summary>
        /// <param name="DateBegin"></param>
        /// <param name="DateEnd"></param>
        /// <returns></returns>
        public DataTable StatWeekCountClass(DateTime DateBegin, DateTime DateEnd)
        {
            SqlParameter[] paraList = new SqlParameter[]{
                new SqlParameter("@v_DateBegin",DateBegin),
                new SqlParameter("@v_DateEnd",DateEnd),
            };
            return SqlHelper.ExecuteDataTable(SqlHelper.SqlconnString, CommandType.StoredProcedure, "cp_Stat_WeekCountClass", paraList);
        }



        /// <summary>
        /// 统计某天报修数
        /// </summary>
        /// <param name="Date">日期</param>
        /// <param name="project">是否大类故障为项目</param>
        /// <param name="BrandID">品牌ID，不需要统计侧传0</param>
        /// <param name="WorkGroupID">所在工作组，不限品牌的前提。控制品牌在指定工作组里的</param>
        /// <param name="Pending">是否只统计Pend的</param>
        /// <returns></returns>
        public int StatCount1_Pending(DateTime Date, StatInfo.ProjectType project, int BrandID, int WorkGroupID, StatInfo.PendingType Pending)
        {
            string strSQL = string.Format("select COUNT(1) from sys_Calls where DATEDIFF(DAY,f_ErrorDate,'{0}')=0", Date);
            switch (project)
            {
                case StatInfo.ProjectType.IsProject:
                    strSQL += string.Format(" and f_ClassName1='{0}' ", StatInfo.ProjectName);
                    break;
                case StatInfo.ProjectType.NotProject:
                    strSQL += string.Format(" and f_ClassName1<>'{0}' ", StatInfo.ProjectName);
                    break;
                default:
                    break;
            }
            switch (Pending)
            {
                case StatInfo.PendingType.Yes:
                    strSQL += " AND f_SloveBy='' ";
                    break;
                case StatInfo.PendingType.No:
                    strSQL += " AND f_SloveBy<>'' ";
                    break;
                default:
                    break;
            }
            if (BrandID > 0)
            {
                strSQL += "  and f_BrandID=" + BrandID;
            }
            if (WorkGroupID > 0)
            {
                strSQL += " and  f_BrandID IN(SELECT sys_WorkGroupBrand.f_MID FROM sys_WorkGroupBrand WHERE f_WorkGroupID=" + WorkGroupID + ")";
            }

            return Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.SqlconnString, CommandType.Text, strSQL.ToString(), null));
        }

        /// <summary>
        /// 某天非项目已完成数
        /// </summary>
        /// <param name="Date">日期</param>
        /// <param name="IsProject">是否大类故障为项目</param>
        /// <param name="BrandID">品牌ID，不需要统计侧传0</param>
        /// <param name="WorkGroupID">所在工作组，不限品牌的前提。控制品牌在指定工作组里的</param>
        /// <returns></returns>
        public int StatCount2_OverSla(DateTime? DateBegin, DateTime Date, int BrandID, int WorkGroupID)
        {
            string strSQL = "select COUNT(1) from sys_Calls where 1=1 ";
            if (null == DateBegin)
            {
                strSQL += string.Format(" AND  DATEDIFF(DAY,f_ErrorDate,'{0}')=0", Date);
                strSQL += string.Format(" AND DATEADD(HOUR,F_SLA,f_ErrorDate)>DATEADD(DAY,1,'{0}') ", Date.ToString("yyyy-MM-dd"));
            }
            else
            {
                strSQL += string.Format(" AND  DATEDIFF(DAY,f_ErrorDate,'{0}')<=0 AND  DATEDIFF(DAY,f_ErrorDate,'{1}')>=0", DateBegin, Date);
                strSQL += " AND DATEDIFF(HOUR,f_ErrorDate,Getdate())>f_SLA  ";
            }

            strSQL += string.Format(" and f_ClassName1<>'{0}' ", StatInfo.ProjectName);
            strSQL += " AND f_SloveBy='' ";

            if (BrandID > 0)
            {
                strSQL += "  and f_BrandID=" + BrandID;
            }
            if (WorkGroupID > 0)
            {
                strSQL += " and  f_BrandID IN(SELECT sys_WorkGroupBrand.f_MID FROM sys_WorkGroupBrand WHERE f_WorkGroupID=" + WorkGroupID + ")";
            }

            return Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.SqlconnString, CommandType.Text, strSQL.ToString(), null));
        }

        /// <summary>
        /// 统计数量
        /// </summary>
        /// <param name="DateBegin">开始日期</param>
        /// <param name="DateEnd">结束日期</param>
        /// <param name="IsProject">是否为项目</param>
        /// <param name="BrandID">品牌ID，不限可输入0</param>
        /// <returns></returns>
        public int StatCount3(DateTime DateBegin, DateTime DateEnd, bool IsProject, int BrandID)
        {
            StringBuilder STATCOUNT = new StringBuilder();
            STATCOUNT.Append("select COUNT(1) from sys_Calls where 1=1 ");
            STATCOUNT.Append(" AND DATEDIFF(DAY,f_ErrorDate,'").Append(DateBegin).Append("')<=0");
            STATCOUNT.Append(" AND DATEDIFF(DAY,f_ErrorDate,'").Append(DateEnd).Append("')>=0");
            if (BrandID > 0)
            {
                STATCOUNT.Append(" and f_BrandID=").Append(BrandID);
            }
            STATCOUNT.Append(" and f_ClassName1").Append(IsProject ? "=" : "<>").Append("'").Append(StatInfo.ProjectName).Append("'");

            return Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.SqlconnString, CommandType.Text, STATCOUNT.ToString(), null));

        }

        /// <summary>
        /// 统计数量————指定时间品牌内，在某时段完成的数量
        /// </summary>
        /// <param name="DateBegin">开始日期</param>
        /// <param name="DateEnd">结束日期</param>
        /// <param name="BrandID">品牌ID，不限输入0</param>
        /// <param name="BeginHour">大于几时小</param>
        /// <param name="EndHour">小于等于几时小</param>
        /// <returns></returns>
        public int StatCount4(DateTime DateBegin, DateTime DateEnd, int BrandID, int BeginHour, int EndHour)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("select COUNT(distinct sys_Calls.ID)").Append(FROM_TABLE);
            strSQL.Append(" left join sys_CallStep on	sys_CallStep .f_CallID=sys_Calls.ID ");
            strSQL.Append(" where sys_CallStep.f_IsSolved=1 ");
            strSQL.Append(" AND DATEDIFF(DAY,f_ErrorDate,'").Append(DateBegin).Append("')<=0");
            strSQL.Append(" AND DATEDIFF(DAY,f_ErrorDate,'").Append(DateEnd).Append("')>=0");
            strSQL.Append(" AND f_StateMain in(").Append((int)SysEnum.CallStateMain.已完成).Append(",").Append((int)SysEnum.CallStateMain.已关闭).Append(")  ");

            strSQL.Append(" and f_ClassName1<>'").Append(StatInfo.ProjectName).Append("'  ");
            strSQL.Append(" AND DATEDIFF(HOUR,f_ErrorDate,sys_CallStep.f_AddDate)>").Append(BeginHour);
            strSQL.Append(" AND DATEDIFF(HOUR,f_ErrorDate,sys_CallStep.f_AddDate)<=").Append(EndHour);
            if (BrandID > 0)
            {
                strSQL.Append(" and  f_BrandID=").Append(BrandID);
            }
            return Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.SqlconnString, CommandType.Text, strSQL.ToString(), null));
        }

        /// <summary>
        /// 统计数量————当日内完成
        /// </summary>
        /// <param name="DateBegin">开始日期</param>
        /// <param name="DateEnd">结束日期</param>
        /// <param name="BrandID">品牌ID，不限输入0</param>
        /// <param name="BeginHour">大于几时小</param>
        /// <param name="EndHour">小于等于几时小</param>
        /// <returns></returns>
        public int StatCount4(DateTime DateBegin, DateTime DateEnd, int BrandID)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("select COUNT(distinct sys_Calls.ID)").Append(FROM_TABLE);
            strSQL.Append(" left join sys_CallStep on	sys_CallStep .f_CallID=sys_Calls.ID ");
            strSQL.Append(" where sys_CallStep.f_IsSolved=1 ");
            strSQL.Append(" AND DATEDIFF(DAY,f_ErrorDate,'").Append(DateBegin).Append("')<=0");
            strSQL.Append(" AND DATEDIFF(DAY,f_ErrorDate,'").Append(DateEnd).Append("')>=0");
            strSQL.Append(" AND f_StateMain in(").Append((int)SysEnum.CallStateMain.已完成).Append(",").Append((int)SysEnum.CallStateMain.已关闭).Append(")  ");
            if (BrandID > 0)
            {
                strSQL.Append(" and  f_BrandID=").Append(BrandID);
            }
            strSQL.Append(" and f_ClassName1<>'").Append(StatInfo.ProjectName).Append("'  ");
            strSQL.Append(" AND DATEDIFF(day,f_ErrorDate,sys_CallStep.f_AddDate)=0");
            return Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.SqlconnString, CommandType.Text, strSQL.ToString(), null));
        }





        /// <summary>
        /// 统计数量
        /// </summary>
        /// <param name="DateBegin"></param>
        /// <param name="DateEnd"></param>
        /// <param name="ProjectType"></param>
        /// <param name="sloveby"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public int StatCount5(DateTime DateBegin, DateTime DateEnd, StatInfo.ProjectType ProjectType, SysEnum.SolvedBy? sloveby, int UserID)
        {
            StringBuilder STATCOUNT = new StringBuilder();
            STATCOUNT.Append("select COUNT(1) from sys_Calls where 1=1 ");

            STATCOUNT.Append(" AND DATEDIFF(DAY,f_ErrorDate,'").Append(DateBegin).Append("')<=0");
            STATCOUNT.Append(" AND DATEDIFF(DAY,f_ErrorDate,'").Append(DateEnd).Append("')>=0");
            if (null != sloveby)
            {
                STATCOUNT.Append(" AND f_SloveBy='").Append(sloveby.ToString()).Append("'");
            }
            if (UserID > 0)
            {
                STATCOUNT.Append(" AND f_MaintainUserID=").Append(UserID);
            }
            switch (ProjectType)
            {
                case StatInfo.ProjectType.IsProject:
                    STATCOUNT.Append(" and f_ClassName1='").Append(StatInfo.ProjectName).Append("'");
                    break;
                case StatInfo.ProjectType.NotProject:
                    STATCOUNT.Append(" and f_ClassName1<>'").Append(StatInfo.ProjectName).Append("'");
                    break;
                default:
                    break;
            }
            return Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.SqlconnString, CommandType.Text, STATCOUNT.ToString(), null));

        }

        /// <summary>
        /// 统计数量 指定时间内完成量
        /// </summary>
        /// <param name="DateBegin">开始日期</param>
        /// <param name="DateEnd">结束日期</param>
        /// <param name="IsProject"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public int StatCount5(DateTime DateBegin, DateTime DateEnd, StatInfo.ProjectType ProjectType, int UserID)
        {
            StringBuilder STATCOUNT = new StringBuilder();
            STATCOUNT.Append("select COUNT(1) from sys_Calls where 1=1 ");

            STATCOUNT.Append(" AND DATEDIFF(DAY,f_ErrorDate,'").Append(DateBegin).Append("')<=0");
            STATCOUNT.Append(" AND DATEDIFF(DAY,f_ErrorDate,'").Append(DateEnd).Append("')>=0");
            STATCOUNT.Append(" AND f_StateMain in(").Append((int)SysEnum.CallStateMain.已完成).Append(",").Append((int)SysEnum.CallStateMain.已关闭).Append(")  ");
            if (UserID > 0)
            {
                STATCOUNT.Append(" AND f_MaintainUserID=").Append(UserID);
            }
            switch (ProjectType)
            {
                case StatInfo.ProjectType.IsProject:
                    STATCOUNT.Append(" and f_ClassName1='").Append(StatInfo.ProjectName).Append("'");
                    break;
                case StatInfo.ProjectType.NotProject:
                    STATCOUNT.Append(" and f_ClassName1<>'").Append(StatInfo.ProjectName).Append("'");
                    break;
            }


            return Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.SqlconnString, CommandType.Text, STATCOUNT.ToString(), null));

        }


        /// <summary>
        /// 指定时间内超sla量
        /// </summary>
        /// <param name="DateBegin">开始日期</param>
        /// <param name="DateEnd">结束日期</param>
        /// <param name="BrandID">品牌ID</param>
        /// <returns></returns>
        public int StatCount6_OverSla(DateTime DateBegin, DateTime DateEnd, int BrandID)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("SELECT COUNT(1) FROM sys_Calls,sys_CallExt ");
            strSQL.Append(" WHERE sys_Calls.ID=sys_CallExt.f_CallID ");
            strSQL.Append(" AND DATEDIFF(HOUR,f_AddDate,f_FinishDate)>f_SLA ");
            strSQL.Append(" AND DATEDIFF(DAY,f_ErrorDate,'").Append(DateBegin).Append("')<=0");
            strSQL.Append(" AND DATEDIFF(DAY,f_ErrorDate,'").Append(DateEnd).Append("')>=0");
            if (BrandID > 0)
            {
                strSQL.Append(" and  f_BrandID=").Append(BrandID);
            }
            return Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.SqlconnString, CommandType.Text, strSQL.ToString(), null));
        }

        /// <summary>
        /// 每月份报修量
        /// </summary>
        /// <param name="DateBegin">开始日期</param>
        /// <param name="DateEnd">结束日期</param>
        /// <param name="CustomerID">客户ID</param>
        /// <param name="BrandID">品牌ID</param>
        /// <returns></returns>
        public DataTable StatCount7_MonthCount(DateTime DateBegin, DateTime DateEnd, int CustomerID, int BrandID)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("SELECT datepart(year,f_ErrorDate)*100 +'-'+datepart(month,f_ErrorDate) AS s_Month");
            //strSQL.Append(" CONVERT(nvarchar,YEAR(f_ErrorDate)) ");
            //strSQL.Append(" +'-'+  ");
            //strSQL.Append(" CONVERT(nvarchar,MONTH(f_ErrorDate)) AS s_Month  ");
            strSQL.Append(",COUNT(1) s_Count  ");
            strSQL.Append(FROM_TABLE);
            strSQL.Append("WHERE 1=1 AND DATEDIFF(DAY,f_ErrorDate,'").Append(DateBegin).Append("')<=0");
            strSQL.Append(" AND DATEDIFF(DAY,f_ErrorDate,'").Append(DateEnd).Append("')>=0");
            if (CustomerID > 0)
            {
                strSQL.Append(" and  f_CustomerID=").Append(CustomerID);
            }
            if (BrandID > 0)
            {
                strSQL.Append(" and  f_BrandID=").Append(BrandID);
            }
            strSQL.Append(" GROUP BY datepart(year,f_ErrorDate)*100 +'-'+datepart(month,f_ErrorDate)  ");
            strSQL.Append(" ORDER BY s_Month ");
            return SqlHelper.ExecuteDataTable(SqlHelper.SqlconnString, CommandType.Text, strSQL.ToString(), null);
        }

        /// <summary>
        /// 前十个报修量最多的店铺
        /// </summary>
        /// <param name="DateBegin">开始日期</param>
        /// <param name="DateEnd">结束日期</param>
        /// <param name="CustomerID">客户ID,不需限制可输入0</param>
        /// <param name="BrandID">品牌ID,不需限制可输入0</param>
        /// <returns></returns>
        public DataTable StatCount8_StoreTop(DateTime DateBegin, DateTime DateEnd, int CustomerID, int BrandID)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("select top 10 ");
            strSQL.Append(" sum(1) as StatCount ");
            strSQL.Append(",sys_Stores.f_Name as StatColumn  ");
            strSQL.Append(" from sys_Calls,sys_Stores ");
            strSQL.Append(" WHERE  sys_Calls.f_StoreID=sys_Stores.ID ");
            strSQL.Append(" AND DATEDIFF(DAY,f_ErrorDate,'").Append(DateBegin).Append("')<=0");
            strSQL.Append(" AND DATEDIFF(DAY,f_ErrorDate,'").Append(DateEnd).Append("')>=0");
            if (CustomerID > 0)
            {
                strSQL.Append(" and  sys_Calls.f_CustomerID=").Append(CustomerID);
            }
            if (BrandID > 0)
            {
                strSQL.Append(" and  sys_Calls.f_BrandID=").Append(BrandID);
            }
            strSQL.Append(" group by sys_Stores.f_Name  ");
            strSQL.Append(" order by StatCount desc  ");

            return SqlHelper.ExecuteDataTable(SqlHelper.SqlconnString, CommandType.Text, strSQL.ToString(), null);
        }


        /// <summary>
        /// 各品牌大类报修数
        /// </summary>
        /// <param name="DateBegin"></param>
        /// <param name="DateEnd"></param>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public DataTable StatCount9_BrandClass1(DateTime DateBegin, DateTime DateEnd, int CustomerID, int BrandID)
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter("@v_DateBegin", DateBegin),
                new SqlParameter("@v_DateEnd", DateEnd),
            };
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("if   exists(select   *   from   tempdb..sysobjects   where   id=object_id( 'tempdb..#temp ')) ");
            strSQL.Append("  drop table #temp ");

            strSQL.Append("select sys_Calls.f_BrandName,sys_Calls.f_ClassName1 ,COUNT(1) AS F_COUNT  into #temp ");
            strSQL.Append("  from sys_Calls");
            strSQL.Append("  WHERE DATEDIFF(DAY,f_CreateDate,@v_DateBegin)<=0 ");
            strSQL.Append("     AND DATEDIFF(DAY,f_CreateDate,@v_DateEnd)>=0 ");
            if (CustomerID > 0) strSQL.Append("  AND f_CustomerID=").Append(CustomerID);
            if (BrandID > 0) strSQL.Append("  AND f_BrandID=").Append(BrandID);
            strSQL.Append("  group by sys_Calls.f_BrandName,sys_Calls.f_ClassName1 ");
            strSQL.Append("  order by sys_Calls.f_BrandName ");

            strSQL.Append("  declare @sql varchar(8000) ");
            strSQL.Append("  set @sql = 'select f_BrandName as ' + '品牌名' ");
            strSQL.Append("  select @sql = @sql + ' , max(case f_ClassName1 when ''' + f_ClassName1 + ''' then F_COUNT else 0 end) [' + f_ClassName1 + ']'");
            strSQL.Append("  from (select distinct f_ClassName1 from #temp) as a");
            strSQL.Append("  set @sql = @sql + ' from #temp group by f_BrandName'");
            strSQL.Append("  exec(@sql) ");
            return SqlHelper.ExecuteDataTable(SqlHelper.SqlconnString, CommandType.Text, strSQL.ToString(), parms);
        }


        /// <summary>
        /// 各品牌大类报修数
        /// </summary>
        /// <param name="DateBegin"></param>
        /// <param name="DateEnd"></param>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public DataTable StatCount10_BrandSloveBy(DateTime DateBegin, DateTime DateEnd, int CustomerID, int BrandID)
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter("@v_DateBegin", DateBegin),
                new SqlParameter("@v_DateEnd", DateEnd),
            };
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("if   exists(select   *   from   tempdb..sysobjects   where   id=object_id( 'tempdb..#temp ')) ");
            strSQL.Append("  drop table #temp ");

            strSQL.Append("select sys_Calls.f_BrandName,sys_Calls.f_SloveBy ,COUNT(1) AS F_COUNT  into #temp  ");
            strSQL.Append("  from sys_Calls");
            strSQL.Append("  WHERE ");
            strSQL.Append("     f_StateMain in(3,4) ");
            strSQL.Append("     AND DATEDIFF(DAY,f_CreateDate,@v_DateBegin)<=0 ");
            strSQL.Append("     AND DATEDIFF(DAY,f_CreateDate,@v_DateEnd)>=0 ");
            if (CustomerID > 0) strSQL.Append("  AND f_CustomerID=").Append(CustomerID);
            if (BrandID > 0) strSQL.Append("  AND f_BrandID=").Append(BrandID);
            strSQL.Append("  group by sys_Calls.f_BrandName,sys_Calls.f_SloveBy ");
            strSQL.Append("  order by sys_Calls.f_BrandName ");

            strSQL.Append("  declare @sql varchar(8000) ");
            strSQL.Append("  set @sql = 'select f_BrandName as ' + '品牌名' ");
            strSQL.Append("  select @sql = @sql + ' , max(case f_SloveBy when ''' + f_SloveBy + ''' then F_COUNT else 0 end) [' + f_SloveBy + ']' ");
            strSQL.Append("  from (select distinct f_SloveBy from #temp) as a ");
            strSQL.Append("  set @sql = @sql + ' from #temp group by f_BrandName' ");
            strSQL.Append("  EXEC( @sql ) ");
            return SqlHelper.ExecuteDataTable(SqlHelper.SqlconnString, CommandType.Text, strSQL.ToString(), parms);

        }

        /// <summary>
        /// 中类故障报修数
        /// </summary>
        /// <param name="DateBegin"></param>
        /// <param name="DateEnd"></param>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public int StatCount11_BrandClass2(DateTime DateBegin, DateTime DateEnd, int CustomerID, int BrandID, int Class2ID)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("SELECT COUNT(1) ").Append(FROM_TABLE);
            strSQL.Append("WHERE 1=1 AND DATEDIFF(DAY,f_ErrorDate,'").Append(DateBegin).Append("')<=0");
            strSQL.Append(" AND DATEDIFF(DAY,f_ErrorDate,'").Append(DateEnd).Append("')>=0");
            if (CustomerID > 0)
            {
                strSQL.Append(" and  f_CustomerID=").Append(CustomerID);
            }
            if (BrandID > 0)
            {
                strSQL.Append(" and  f_BrandID=").Append(BrandID);
            }
            if (Class2ID > 0)
            {
                strSQL.Append(" and  f_Class2=").Append(Class2ID);
            }
            object obj = SqlHelper.ExecuteScalar(SqlHelper.SqlconnString, CommandType.Text, strSQL.ToString(), null);
            return Tool.Function.ConverToInt(obj, 0);
        }


        /// <summary>
        /// 未完成记录数
        /// </summary>
        /// <param name="DateBegin"></param>
        /// <param name="DateEnd"></param>
        /// <param name="CustomerID"></param>
        /// <param name="BrandID"></param>
        /// <returns></returns>
        public int StatCount12_UnFinish(DateTime DateBegin, DateTime DateEnd, int CustomerID, int BrandID)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("SELECT COUNT(1) ").Append(FROM_TABLE);
            strSQL.Append("WHERE 1=1 AND DATEDIFF(DAY,f_ErrorDate,'").Append(DateBegin).Append("')<=0");
            strSQL.Append(" AND DATEDIFF(DAY,f_ErrorDate,'").Append(DateEnd).Append("')>=0");
            if (CustomerID > 0)
            {
                strSQL.Append(" and  f_CustomerID=").Append(CustomerID);
            }
            if (BrandID > 0)
            {
                strSQL.Append(" and  f_BrandID=").Append(BrandID);
            }
            strSQL.Append(" and  f_SloveBy =''");
            object obj = SqlHelper.ExecuteScalar(SqlHelper.SqlconnString, CommandType.Text, strSQL.ToString(), null);
            return Tool.Function.ConverToInt(obj, 0);
        }
        #endregion


        /// <summary>
        /// 主用于二线统计
        /// </summary>
        /// <param name="DateBegin"></param>
        /// <param name="DateEnd"></param>
        /// <param name="BeginHour"></param>
        /// <param name="EndHour"></param>
        /// <param name="WithoutDropIn"></param>
        /// <param name="UserID"></param>
        /// <param name="WithoutProject">去掉项目的</param>
        /// <param name="WithoutAssign">去掉有转派记录的</param>
        /// <returns></returns>
        public int StatCount13_L2(DateTime DateBegin, DateTime DateEnd, int BeginHour, int EndHour, bool WithoutDropIn, int UserID, bool WithoutProject, bool WithoutAssign)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("SELECT COUNT(1) FROM view_CallFull ");
            strSQL.Append("WHERE 1=1 ");
            if (WithoutAssign) strSQL.Append(" AND ID NOT IN(SELECT f_CallID FROM sys_Assign) ");                                 //没有转派记录
            strSQL.Append(" AND f_SloveBy<>'' ");                                                              //已完成
            strSQL.Append(" AND DATEDIFF(DAY,f_ErrorDate,'").Append(DateBegin).Append("')<=0");                 //指定日期
            strSQL.Append(" AND DATEDIFF(DAY,f_ErrorDate,'").Append(DateEnd).Append("')>=0");                   //指定日期
            if (BeginHour > 0) strSQL.Append(" AND DATEDIFF(MINUTE,f_ErrorDate,f_finishdate)>").Append(BeginHour * 60);   //指定时间内完成
            if (EndHour > 0) strSQL.Append(" AND DATEDIFF(MINUTE,f_ErrorDate,f_finishdate)<=").Append(EndHour * 60);   //指定时间内完成
            if (WithoutDropIn)
                strSQL.Append(" AND f_SloveBy <>'").Append(SysEnum.SolvedBy.上门.ToString()).Append("' ");      //非上门
            if (UserID > 0)
                strSQL.Append(" AND f_MaintainUserID=").Append(UserID);                                         //指定人
            if (WithoutProject)
                strSQL.Append(" AND f_ClassName1<>'").Append(StatInfo.ProjectName).Append("'");                 //非项目


            object obj = SqlHelper.ExecuteScalar(SqlHelper.SqlconnString, CommandType.Text, strSQL.ToString(), null);
            return Tool.Function.ConverToInt(obj, 0);
        }

        /// <summary>
        /// 主用于一线统计
        /// </summary>
        /// <param name="Date"></param>
        /// <param name="OnlyDropIn"></param>
        /// <param name="UserID"></param>
        /// <param name="WithoutProject"></param>
        /// <returns></returns>
        public int StatCount14_L1(DateTime Date, bool OnlyDropIn, int UserID, bool WithoutProject)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("SELECT COUNT(1) FROM view_CallFull ");
            strSQL.Append("WHERE 1=1 ");
            //strSQL.Append(" AND f_SloveBy<>'' ");                                                              //已完成
            strSQL.Append(" AND DATEDIFF(DAY,f_ErrorDate,'").Append(Date).Append("')=0");                 //指定日期
            if (OnlyDropIn)
                strSQL.Append(" AND f_SloveBy='").Append(SysEnum.SolvedBy.上门.ToString()).Append("' ");      //上门
            if (UserID > 0)
                strSQL.Append(" AND f_CreatorID=").Append(UserID);                                         //指定人
            if (WithoutProject)
                strSQL.Append(" AND f_ClassName1<>'").Append(StatInfo.ProjectName).Append("'");                 //非项目


            object obj = SqlHelper.ExecuteScalar(SqlHelper.SqlconnString, CommandType.Text, strSQL.ToString(), null);
            return Tool.Function.ConverToInt(obj, 0);
        }
        /// <summary>
        /// 主用于一线统计
        /// </summary>
        /// <param name="Date"></param>
        /// <param name="WithoutDropIn"></param>
        /// <param name="UserID"></param>
        /// <param name="WithoutProject"></param>
        /// <returns></returns>
        public int StatCount14_L1(DateTime Date, bool WithoutDropIn, int UserID, bool WithoutProject, int BeginHour, int EndHour)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("SELECT COUNT(1) FROM view_CallFull ");
            strSQL.Append(" JOIN view_CallStepClose ON view_CallStepClose.f_CallID=view_CallFull.ID ");
            strSQL.Append("WHERE 1=1 ");
            strSQL.Append(" AND f_SloveBy<>'' ");                                                              //已完成
            strSQL.Append(" AND DATEDIFF(DAY,f_FinishDate,'").Append(Date).Append("')=0");                 //指定日期
            if (BeginHour > 0)
                strSQL.Append(" AND DATEPART(hour,f_FinishDate)>").Append(BeginHour);
            if (EndHour > 0)
                strSQL.Append(" AND DATEPART(hour,f_FinishDate)<=").Append(EndHour);
            if (WithoutDropIn)
                strSQL.Append(" AND f_SloveBy<>'").Append(SysEnum.SolvedBy.上门.ToString()).Append("' ");      //上门
            if (UserID > 0)
                strSQL.Append(" AND view_CallStepClose.f_MajorUserID=").Append(UserID);                                         //指定人
            if (WithoutProject)
                strSQL.Append(" AND f_ClassName1<>'").Append(StatInfo.ProjectName).Append("'");                 //非项目


            object obj = SqlHelper.ExecuteScalar(SqlHelper.SqlconnString, CommandType.Text, strSQL.ToString(), null);
            return Tool.Function.ConverToInt(obj, 0);
        }

        /// <summary>
        /// 一线统计表中的二线关单情况
        /// </summary>
        /// <param name="Date"></param>
        /// <param name="Hour"></param>
        /// <param name="CreateUserID"></param>
        /// <param name="WithoutDropIn"></param>
        /// <param name="WithoutProject"></param>
        /// <returns></returns>
        public int StatCount15_L2(DateTime Date, int Hour, int CreateUserID, bool WithoutDropIn, bool WithoutProject)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("SELECT COUNT(1) FROM view_CallFull,sys_CallStep ");
            strSQL.Append("WHERE sys_CallStep.f_IsSolved=1 and sys_CallStep.f_CallID=view_CallFull.ID ");
            strSQL.Append(" AND f_SloveBy<>'' ");                                                              //已完成
            if (CreateUserID > 0) strSQL.Append(" AND f_CreatorID=").Append(CreateUserID);
            if (Date != Tool.Function.ErrorDate)
            {
                strSQL.Append(" AND DATEDIFF(DAY,f_CreateDate,'").Append(Date).Append("')=0");
                strSQL.Append(" AND DATEDIFF(DAY,f_CreateDate,f_AddDate)=0");
            }
            if (Hour > 0)
                strSQL.Append(" AND DATEPART(hour,sys_CallStep.f_AddDate)<=").Append(Hour);
            if (CreateUserID > 0)
                strSQL.Append(" AND f_CreatorID=").Append(CreateUserID);
            if (WithoutDropIn)
                strSQL.Append(" AND f_SloveBy<>'").Append(SysEnum.SolvedBy.上门.ToString()).Append("' ");      //上门
            if (WithoutProject)
                strSQL.Append(" AND f_ClassName1<>'").Append(StatInfo.ProjectName).Append("'");                 //非项目


            object obj = SqlHelper.ExecuteScalar(SqlHelper.SqlconnString, CommandType.Text, strSQL.ToString(), null);
            return Tool.Function.ConverToInt(obj, 0);
        }


        public DataTable FeedBackRadio(int BrandID, int PaperID, DateTime? DateBegin, DateTime? DateEnd)
        {

            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter("@v_DateBegin", DateBegin),
                new SqlParameter("@v_DateEnd", DateEnd),
                new SqlParameter("@v_BrandID", BrandID),
            };
            return SqlHelper.ExecuteDataTable(SqlHelper.SqlconnString, CommandType.StoredProcedure, "cp_Stat_FeedBack", parms);
        }


        /// <summary>
        /// 单选题回访统计
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <param name="BrandID"></param>
        /// <param name="DateBegin"></param>
        /// <param name="DateEnd"></param>
        /// <param name="QuestionID"></param>
        /// <returns></returns>
        public DataTable FeedBackRadio(int CustomerID, int BrandID, DateTime? DateBegin, DateTime? DateEnd, int QuestionID)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("SELECT f_Name,sys_FeedbackChoose.ID,count(1) f_Count FROM sys_FeedbackChoose ");
            strSQL.Append(" LEFT JOIN sys_FeedbackAnswer ON sys_FeedbackChoose.ID=sys_FeedbackAnswer.f_Answer2 ");
            strSQL.Append(" LEFT JOIN sys_Calls ON sys_Calls.ID=sys_FeedbackAnswer.f_CallID ");
            strSQL.Append("     WHERE f_Answer2<>0 AND sys_FeedbackAnswer.f_QuestionID=sys_FeedbackChoose.f_QuestionID ");
            strSQL.Append(" AND sys_FeedbackChoose.f_QuestionID=").Append(QuestionID);
            if (CustomerID > 0)
            {
                strSQL.Append(" AND sys_Calls.f_CustomerID=").Append(CustomerID);
            }
            if (BrandID > 0)
            {
                strSQL.Append(" AND sys_Calls.f_BrandID=").Append(BrandID);
            }
            if (DateBegin != null)
            {
                strSQL.Append(" AND DATEDIFF(DAY,'").Append(DateBegin).Append("',sys_FeedbackAnswer.f_AddDate)>=0");
            }

            if (DateEnd != null)
            {
                strSQL.Append(" AND DATEDIFF(DAY,'").Append(DateEnd).Append("',sys_FeedbackAnswer.f_AddDate)<=0");
            }
            strSQL.Append(" GROUP BY f_Name,sys_FeedbackChoose.ID ");

            return SqlHelper.ExecuteDataTable(SqlHelper.SqlconnString, CommandType.Text, strSQL.ToString(), null);

        }

        public DataTable PendingDateBrand(DateTime CurrentDate, int BrandID, int WorkGroupID)
        {
            SqlParameter[] parms = new SqlParameter[] {
                    new SqlParameter("@BrandID", BrandID),
                    new SqlParameter("@WorkGroupID", WorkGroupID),
                    new SqlParameter("@CurrentDate", CurrentDate)
                };

            return SqlHelper.ExecuteDataTable(SqlHelper.SqlconnString, CommandType.StoredProcedure, "Pending_DateBrand", parms);
        }

        /// <summary>
        /// 无用了
        /// </summary>
        /// <param name="table"></param>
        /// <param name="OleDbConnectString"></param>
        /// <returns></returns>
        private bool CreateTemplateTable( string OleDbConnectString)
        {
            string SQL = "create Table [上门详细]([系统单号] text ,";
            for (int i = 1; i < 16; i++)
            {
                SQL += "[第" + i + "次上门工程师] text ,";
                SQL += "[第" + i + "次上门工程师所在工作组] text ,";
                SQL += "[第" + i + "次到达门店时间] text ,";
                SQL += "[第" + i + "次离开门店时间] text ,";
            }
            SQL = SQL.Trim().Substring(0, SQL.Length - 1);
            SQL += ")";
            //try
            //{
            OleDbHelper.ExecuteNonQuery(OleDbConnectString, CommandType.Text, SQL, null);
            return true;
            //}
            //catch (Exception)
            //{
            //    throw;
            //    return false;
            //}

        }


        public bool ExportExcel(DataTable dt, int RowIndex, string MapTemplateDirectory)
        {
            string OleDbConnectString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + MapTemplateDirectory;

            #region table1
            StringBuilder SQL = new StringBuilder();
            SQL.Append(" INSERT INTO [ReportDetail] ( ");
            foreach (DataColumn item in dt.Columns)
            {
                SQL.Append("[").Append(item.ColumnName.Trim()).Append("],");
            }
            SQL = SQL.Remove(SQL.Length - 1, 1);
            SQL = SQL.Append(" ) VALUES( ");
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                SQL.Append(" @MyParmeter").Append(i).Append(",");
            }
            SQL = SQL.Remove(SQL.Length - 1, 1);
            SQL = SQL.Append(" ) ");
            List<OleDbParameter> parms = new List<OleDbParameter>();
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                OleDbParameter par = new OleDbParameter("@MyParmeter" + i, OleDbType.LongVarChar);
                par.Value = dt.Rows[RowIndex][i].ToString();
                parms.Add(par);
            }

            #endregion

            try
            {
                OleDbHelper.ExecuteNonQuery(OleDbConnectString, CommandType.Text, SQL.ToString(), parms.ToArray());
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
                return false;
            }

        }



        public DataTable GetExcel(string MapTemplateDirectory)
        {
            string OleDbConnectString = "Provider=Microsoft.Ace.OleDb.12.0;data source=" + MapTemplateDirectory + ";Extended Properties='Excel 12.0; HDR=Yes;IMEX=2;'";

            return OleDbHelper.ExecuteDataTable(OleDbConnectString, CommandType.Text, "select * from statlist ", null);
        }

        public DataTable StatK(int CustomerID, int BrandID, int ProvinceID, int CityID, int Class1ID, int Class2ID, DateTime DateBegin, DateTime DateEnd)
        {
            string ClassName = Class1ID <= 0 ? "f_Class1" : Class2ID <= 0 ? "f_Class2" : "f_Class3";
            StringBuilder SQL = new StringBuilder();
            SQL.Append(" select SUM(1) as StatCount,");
            SQL.Append(ClassName).Append(" as StatClass FROM ");
            SQL.Append(CallDAL.TABLE).Append(" WHERE 1=1 ");
            SQL.Append(" AND DATEDIFF(DAY,'").Append(DateBegin).Append("',f_ErrorDate)>=0");
            SQL.Append(" AND DATEDIFF(DAY,'").Append(DateEnd).Append("',f_ErrorDate)<=0");
            SQL.Append(" AND f_CustomerID=").Append(CustomerID);
            if (BrandID > 0) SQL.Append(" AND f_BrandID=").Append(BrandID);
            if (ProvinceID > 0) SQL.Append(" AND f_ProvinceID=").Append(ProvinceID);
            if (CityID > 0) SQL.Append(" AND f_CityID=").Append(CityID);
            if (Class2ID > 0)
            {
                SQL.Append(" AND f_Class2=").Append(Class2ID);
            }
            else if (Class1ID > 0)
            {
                SQL.Append(" AND f_Class1=").Append(Class1ID);
            }

            SQL.Append(" group by ");
            SQL.Append(ClassName);
            SQL.Append(" ORDER BY StatCount DESC ");

            return SqlHelper.ExecuteDataTable(SqlHelper.SqlconnString, CommandType.Text, SQL.ToString(), null);

        }

        public DataTable StatN(int CustomerID, int BrandID, bool OverSLA, DateTime DateBegin, DateTime DateEnd)
        {
            StringBuilder SQL = new StringBuilder();
            SQL.Append(" select top 100 sum(1) as StatCount,f_Name as StatClass,sys_Class1.ID FROM");
            SQL.Append(CallDAL.TABLE).Append(" , sys_Class1 ");
            SQL.Append(" where sys_Calls.f_Class1=sys_Class1.ID ");
            SQL.Append(" AND f_StateMain in(").Append((int)SysEnum.CallStateMain.未处理).Append(",").Append((int)SysEnum.CallStateMain.处理中).Append(")");
            SQL.Append(" AND DATEDIFF(DAY,'").Append(DateBegin).Append("',f_ErrorDate)>=0");
            SQL.Append(" AND DATEDIFF(DAY,'").Append(DateEnd).Append("',f_ErrorDate)<=0");
            SQL.Append(" AND sys_Calls.f_CustomerID=").Append(CustomerID);
            if (BrandID > 0) SQL.Append(" AND sys_Calls.f_BrandID=").Append(BrandID);
            SQL.Append(" AND DATEDIFF(hour,f_ErrorDate,GETDATE())").Append(OverSLA ? ">" : "<").Append(" sys_Calls.f_SLA ");
            SQL.Append(" group by f_Name,sys_Class1.ID order by StatCount desc");

            return SqlHelper.ExecuteDataTable(SqlHelper.SqlconnString, CommandType.Text, SQL.ToString(), null);

        }

        /// <summary>
        /// 城市报修量top 10
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <param name="BrandID"></param>
        /// <param name="DateBegin"></param>
        /// <param name="DateEnd"></param>
        /// <returns></returns>
        public DataTable StatO(int CustomerID, int BrandID, DateTime DateBegin, DateTime DateEnd)
        {
            StringBuilder SQL = new StringBuilder();
            SQL.Append(" select top 10 SUM(1) as StatCount,f_CityName as StatColumn ");
            SQL.Append(" from [sys_Calls],sys_City ");
            SQL.Append(" WHERE sys_Calls.f_CityID=sys_City.ID ");
            SQL.Append(" AND DATEDIFF(DAY,'").Append(DateBegin).Append("',f_ErrorDate)>=0");
            SQL.Append(" AND DATEDIFF(DAY,'").Append(DateEnd).Append("',f_ErrorDate)<=0");
            SQL.Append(" AND sys_Calls.f_CustomerID=").Append(CustomerID);
            if (BrandID > 0) SQL.Append(" AND sys_Calls.f_BrandID=").Append(BrandID);
            SQL.Append(" GROUP BY f_CityName ORDER BY StatCount DESC");

            return SqlHelper.ExecuteDataTable(SqlHelper.SqlconnString, CommandType.Text, SQL.ToString(), null);

        }

    }
}

