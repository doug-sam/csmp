using System;
using System.Collections.Generic;
using CSMP.DAL;
using CSMP.Model;
using System.Data;
using Tool;
using System.Text;
using System.Web;
using System.IO;

namespace CSMP.BLL
{
    public static class StatBLL
    {
        private const string TemplateDirectory = "~/file/Template/StatList.accdb";


        private static readonly DAL.StatDAL dal = new DAL.StatDAL();

        public static DataTable StatCity(DateTime DateBegin, DateTime DateEnd, int CustomerID, int BrandID)
        {
            UserInfo uinfo = null;
            if (CustomerID <= 0 && BrandID <= 0)
            {
                uinfo = UserBLL.GetCurrent();
            }
            return dal.StatCity(DateBegin, DateEnd, CustomerID, BrandID,uinfo);
        }
        public static DataTable StatCityClass(DateTime DateBegin, DateTime DateEnd)
        {
            return dal.StatCityClass(DateBegin, DateEnd);
        }
        public static DataTable StatCityHour(DateTime DateBegin, DateTime DateEnd, int CustomerID, int BrandID)
        {
            if (BrandID > 0)
            {
                return dal.StatCityHour(DateBegin, DateEnd, CustomerID, BrandID, 0);
            }
            UserInfo uinfo = UserBLL.GetCurrent();
            if (null == uinfo)
            {
                return new DataTable();
            }
            return dal.StatCityHour(DateBegin, DateEnd, CustomerID, BrandID, uinfo.Rule.Contains(SysEnum.Rule.管理员.ToString()) ? 0 : uinfo.WorkGroupID);
        }
        /// <summary>
        /// 指定月份内，每周各服务方式 数
        /// </summary>
        /// <param name="DateBegin"></param>
        /// <param name="DateEnd"></param>
        /// <returns></returns>
        public static DataTable StatWeekCountServerWay(DateTime DateBegin, DateTime DateEnd)
        {
            return dal.StatWeekCountServerWay(DateBegin, DateEnd);
        }
        /// <summary>
        /// 指定月份内，每周各类型报修 数
        /// </summary>
        /// <param name="DateBegin"></param>
        /// <param name="DateEnd"></param>
        /// <returns></returns>
        public static DataTable StatWeekCountClass(DateTime DateBegin, DateTime DateEnd)
        {
            return dal.StatWeekCountClass(DateBegin, DateEnd);
        }

        #region Stat

        /// <summary>
        /// 统计某天报修数
        /// </summary>
        /// <param name="Date">日期</param>
        /// <param name="project">是否大类故障为项目</param>
        /// <param name="BrandID">品牌ID，不需要统计侧传0</param>
        /// <param name="WorkGroupID">所在工作组，不限品牌的前提。控制品牌在指定工作组里的</param>
        /// <param name="Pending">是否只统计Pend的</param>
        /// <returns></returns>
        public static int StatCount1_Pending(DateTime Date, StatInfo.ProjectType project, int BrandID, int WorkGroupID, StatInfo.PendingType Pending)
        {
            if (BrandID > 0)
            {
                return dal.StatCount1_Pending(Date, project, BrandID, 0, Pending);
            }
            UserInfo uinfo = UserBLL.GetCurrent();
            if (null == uinfo)
            {
                return 0;
            }
            return dal.StatCount1_Pending(Date, project, 0, uinfo.Rule.Contains(SysEnum.Rule.管理员.ToString()) ? 0 : uinfo.WorkGroupID, Pending);
        }

        /// <summary>
        /// 超sla量
        /// </summary>
        /// <param name="DateBegin">从哪天开始</param>
        /// <param name="Date">结束日期，如果不指定开始日期，则只返回当日超sla量</param>
        /// <param name="BrandID">品牌ID，不需要统计侧传0</param>
        /// <param name="WorkGroupID">所在工作组，不限品牌的前提。控制品牌在指定工作组里的</param>
        /// <returns></returns>
        public static int StatCount2_OverSla(DateTime? DateBegin, DateTime Date, int BrandID, int WorkGroupID)
        {
            if (BrandID > 0)
            {

                return dal.StatCount2_OverSla(DateBegin, Date, BrandID, 0);
            }
            UserInfo uinfo = UserBLL.GetCurrent();
            if (null == uinfo)
            {
                return 0;
            }
            return dal.StatCount2_OverSla(DateBegin, Date, 0, uinfo.Rule.Contains(SysEnum.Rule.管理员.ToString()) ? 0 : uinfo.WorkGroupID);
        }

        public static int StatCount3(DateTime DateBegin, DateTime DateEnd, bool IsProject, int BrandID)
        {
            if (DateBegin > DateEnd)
            {
                return 0;
            }
            return dal.StatCount3(DateBegin, DateEnd, IsProject, BrandID);
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
        public static int StatCount4(DateTime DateBegin, DateTime DateEnd, int BrandID, int BeginHour, int EndHour)
        {
            if (DateBegin > DateEnd)
            {
                return 0;
            }
            if (BeginHour >= EndHour)
            {
                return 0;
            }
            return dal.StatCount4(DateBegin, DateEnd, BrandID, BeginHour, EndHour);
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
        public static int StatCount4(DateTime DateBegin, DateTime DateEnd, int BrandID)
        {
            return dal.StatCount4(DateBegin, DateEnd, BrandID);
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
        public static int StatCount5(DateTime DateBegin, DateTime DateEnd, StatInfo.ProjectType ProjectType, SysEnum.SolvedBy? sloveby, int UserID)
        {
            return dal.StatCount5(DateBegin, DateEnd, ProjectType, sloveby, UserID);
        }

        /// <summary>
        /// 统计数量
        /// </summary>
        /// <param name="DateBegin">开始日期</param>
        /// <param name="DateEnd">结束日期</param>
        /// <param name="IsProject"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public static int StatCount5(DateTime DateBegin, DateTime DateEnd, StatInfo.ProjectType ProjectType, int UserID)
        {
            return dal.StatCount5(DateBegin, DateEnd, ProjectType, UserID);
        }

        /// <summary>
        /// 指定时间内超sla量
        /// </summary>
        /// <param name="DateBegin">开始日期</param>
        /// <param name="DateEnd">结束日期</param>
        /// <param name="BrandID">品牌ID</param>
        /// <returns></returns>
        public static int StatCount6_OverSla(DateTime DateBegin, DateTime DateEnd, int BrandID)
        {
            return dal.StatCount6_OverSla(DateBegin, DateEnd, BrandID);
        }

        /// <summary>
        /// 每月份报修量
        /// </summary>
        /// <param name="DateBegin">开始日期</param>
        /// <param name="DateEnd">结束日期</param>
        /// <param name="CustomerID">客户ID</param>
        /// <param name="BrandID">品牌ID</param>
        /// <returns></returns>
        public static DataTable StatCount7_MonthCount(DateTime DateBegin, DateTime DateEnd, int CustomerID, int BrandID)
        {
            return dal.StatCount7_MonthCount(DateBegin, DateEnd, CustomerID, BrandID);
        }



        /// <summary>
        /// 前十个报修量最多的店铺
        /// </summary>
        /// <param name="DateBegin">开始日期</param>
        /// <param name="DateEnd">结束日期</param>
        /// <param name="CustomerID">客户ID</param>
        /// <param name="BrandID">品牌ID</param>
        /// <returns></returns>
        public static DataTable StatCount8_StoreTop(DateTime DateBegin, DateTime DateEnd, int CustomerID, int BrandID)
        {
            if (CustomerID <= 0)
            {
                return new DataTable();
            }
            return dal.StatCount8_StoreTop(DateBegin, DateEnd, CustomerID, BrandID);

        }



        /// <summary>
        /// 各品牌大类报修数
        /// </summary>
        /// <param name="DateBegin"></param>
        /// <param name="DateEnd"></param>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public static DataTable StatCount9_BrandClass1(DateTime DateBegin, DateTime DateEnd, int CustomerID, int BrandID)
        {
            if (DateEnd < DateBegin || CustomerID <= 0)
            {
                return new DataTable();
            }
            return dal.StatCount9_BrandClass1(DateBegin, DateEnd, CustomerID, BrandID);
        }


        /// <summary>
        /// 各品牌大类报修数
        /// </summary>
        /// <param name="DateBegin"></param>
        /// <param name="DateEnd"></param>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public static DataTable StatCount10_BrandSloveBy(DateTime DateBegin, DateTime DateEnd, int CustomerID, int BrandID)
        {
            if (DateEnd < DateBegin || CustomerID <= 0)
            {
                return new DataTable();
            }
            return dal.StatCount10_BrandSloveBy(DateBegin, DateEnd, CustomerID, BrandID);
        }


        /// <summary>
        /// 中类故障报修数
        /// </summary>
        /// <param name="DateBegin"></param>
        /// <param name="DateEnd"></param>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public static int StatCount11_BrandClass2(DateTime DateBegin, DateTime DateEnd, int CustomerID, int BrandID, int Class2ID)
        {
            if (DateEnd < DateBegin || CustomerID <= 0)
            {
                return 0;
            }
            return dal.StatCount11_BrandClass2(DateBegin, DateEnd, CustomerID, BrandID, Class2ID);
        }


        /// <summary>
        /// 未完成记录数
        /// </summary>
        /// <param name="DateBegin"></param>
        /// <param name="DateEnd"></param>
        /// <param name="CustomerID"></param>
        /// <param name="BrandID"></param>
        /// <returns></returns>
        public static int StatCount12_UnFinish(DateTime DateBegin, DateTime DateEnd, int CustomerID, int BrandID)
        {
            if (DateEnd < DateBegin || CustomerID <= 0)
            {
                return 0;
            }
            return dal.StatCount12_UnFinish(DateBegin, DateEnd, CustomerID, BrandID);
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
        public static int StatCount13_L2(DateTime DateBegin, DateTime DateEnd, int BeginHour, int EndHour, bool WithoutDropIn, int UserID, bool WithoutProject, bool WithoutAssign)
        {
            return dal.StatCount13_L2(DateBegin, DateEnd, BeginHour, EndHour, WithoutDropIn, UserID, WithoutProject, WithoutAssign);
        }

        /// <summary>
        /// 主用于一线统计
        /// </summary>
        /// <param name="Date"></param>
        /// <param name="OnlyDropIn"></param>
        /// <param name="UserID"></param>
        /// <param name="WithoutProject"></param>
        /// <returns></returns>
        public static int StatCount14_L1(DateTime Date, bool OnlyDropIn, int UserID, bool WithoutProject)
        {
            return dal.StatCount14_L1(Date, OnlyDropIn, UserID, WithoutProject);
        }
        /// <summary>
        /// 主用于一线统计
        /// </summary>
        /// <param name="Date"></param>
        /// <param name="WithoutDropIn"></param>
        /// <param name="UserID"></param>
        /// <param name="WithoutProject"></param>
        /// <returns></returns>
        public static int StatCount14_L1(DateTime Date, bool WithoutDropIn, int UserID, bool WithoutProject, int BeginHour, int EndHour)
        {
            return dal.StatCount14_L1(Date, WithoutDropIn, UserID, WithoutProject, BeginHour, EndHour);
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
        public static int StatCount15_L2(DateTime Date, int Hour, int CreateUserID, bool WithoutDropIn, bool WithoutProject)
        {
            return dal.StatCount15_L2(Date, Hour, CreateUserID, WithoutDropIn, WithoutProject);
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
        public static DataTable FeedBackRadio(int CustomerID, int BrandID, DateTime? DateBegin, DateTime? DateEnd, int QuestionID)
        {
            return dal.FeedBackRadio(CustomerID, BrandID, DateBegin, DateEnd, QuestionID);
        }

        public static DataTable FeedBackRadio(int BrandID, int PaperID, DateTime? DateBegin, DateTime? DateEnd)
        { 
            return dal.FeedBackRadio(BrandID, PaperID, DateBegin, DateEnd);
        }


        public static DataTable PendingDateBrand(DateTime CurrentDate, int BrandID, int WorkGroupID)
        {
            return dal.PendingDateBrand(CurrentDate, BrandID, WorkGroupID);
        }

        /// <summary>
        /// 导出数据
        /// </summary>
        /// <param name="table">导出具体数据</param>
        /// <param name="RowIndex">第几行</param>
        /// <param name="file">目标文件。绝对路径</param>
        /// <returns></returns>
        public static bool ExportAccess(DataTable dt, int RowIndex, string file)
        {

            string Path = file.Substring(0, file.Replace("\\", "/").LastIndexOf("/"));
            string FileName = file.Substring(file.Replace("\\", "/").LastIndexOf("/"));
            if (!Directory.Exists(Path))
            {
                Directory.CreateDirectory(Path);
            }
            if (!File.Exists(file))
            {
                File.Copy(HttpContext.Current.Server.MapPath(TemplateDirectory), file);
            }

            return dal.ExportExcel(dt, RowIndex, file);
        }

        public static DataTable GetExcel()
        {
            string TemplateDirectory = "~/file/Template/StatList.xlsx";

            return dal.GetExcel(HttpContext.Current.Server.MapPath(TemplateDirectory));
        }

        public static DataTable StatK(int CustomerID, int BrandID, int ProvinceID, int CityID, int Class1ID, int Class2ID, DateTime DateBegin, DateTime DateEnd)
        {
            if (CustomerID <= 0 || DateBegin == Tool.Function.ErrorDate || DateEnd == Tool.Function.ErrorDate)
            {
                return new DataTable();
            }
            return dal.StatK(CustomerID, BrandID, ProvinceID, CityID, Class1ID, Class2ID, DateBegin, DateEnd);
        }

        public static DataTable StatN(int CustomerID, int BrandID, bool OverSLA, DateTime DateBegin, DateTime DateEnd)
        {
            if (CustomerID <= 0)
            {
                return new DataTable();
            }
            return dal.StatN(CustomerID, BrandID, OverSLA, DateBegin, DateEnd);
        }
        /// <summary>
        /// 城市报修量top 10
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <param name="BrandID"></param>
        /// <param name="DateBegin"></param>
        /// <param name="DateEnd"></param>
        /// <returns></returns>
        public static DataTable StatO(int CustomerID, int BrandID, DateTime DateBegin, DateTime DateEnd)
        {
            if (CustomerID <= 0)
            {
                return new DataTable();
            }
            return dal.StatO(CustomerID, BrandID, DateBegin, DateEnd);
        }
    }

}