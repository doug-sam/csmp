using System;
using System.Collections.Generic;
using CSMP.DAL;
using CSMP.Model;
using System.Data;
using Tool;
using System.Text;
using System.Threading;

namespace CSMP.BLL
{
    public static class CallBLL
    {
        private static readonly DAL.CallDAL dal = new DAL.CallDAL();

        #region Get
        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<CallInfo> GetList(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            return dal.GetList(PageSize, CurPage, StrWhere, out Count);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<CallInfo> GetList(int PageSize, int CurPage, int WorkGroupID, out int Count)
        {
            return dal.GetList(PageSize, CurPage, WorkGroupID, out Count);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<CallInfo> GetList(string StrWhere)
        {
            return dal.GetList(StrWhere);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<CallInfo> GetList(int StoreID)
        {
            if (StoreID <= 0)
            {
                return null;
            }
            return GetList(" 1=1 and f_StoreID=" + StoreID + " order by ID desc ");
        }

        /// <summary>
        /// 查询现场工程师负责的call列表
        /// </summary>
        /// <param name="StateMain">2代表处理中，3代表完成</param>
        /// <param name="UserID">现场工程师的userid</param>
        /// <param name="WorkGroupID">工作组ID，暂不使用</param>
        /// <returns></returns>
        public  static List<CallInfo> GetMyCallsForOnsiteEngineer(int StateMain, int UserID, int WorkGroupID)
        {
            return dal.GetMyCallsForOnsiteEngineer(StateMain, UserID, WorkGroupID);
        }

        /// <summary>
        /// 查询现场工程师负责的call列表，APP查询我的工单专用，调用SP实现
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="returnValue"></param>
        /// <returns></returns>
        public static DataTable GetMyCallsForOnsiteEngineerBySp(string userName, out string returnValue)
        {
            return dal.GetMyCallsForOnsiteEngineerBySp(userName, out returnValue);
        }
        /// <summary>
        /// 查询现场工程师负责的历史call列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="UserID"></param>
        /// <param name="WorkGroupID"></param>
        /// <returns></returns>
        public static List<CallInfo> GetHistoryCallsForOnsiteEngineer(int pageIndex, int pageSize, int UserID, int WorkGroupID)
        {
            return dal.GetHistoryCallsForOnsiteEngineer( pageIndex,  pageSize,  UserID,  WorkGroupID);
        }

        /// <summary>
        /// 工单查询，APP专用
        /// </summary>
        /// <param name="StateMain"></param>
        /// <param name="StateDetail"></param>
        /// <param name="UserID"></param>
        /// <param name="WorkGroupID"></param>
        /// <param name="CustomeName"></param>
        /// <param name="BrandName"></param>
        /// <returns></returns>
        public static List<CallInfo> GetMyCallsForOnsiteEngineer(int StateMain, int StateDetail, int UserID, int WorkGroupID, string CustomeName, string BrandName)
        {
            //ZQL 20151013 为APP接口编写
            return dal.GetMyCallsForOnsiteEngineer( StateMain,  StateDetail,  UserID,  WorkGroupID,  CustomeName,  BrandName);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="state"></param>
        /// <param name="getGroup"></param>
        /// <param name="CustomeName"></param>
        /// <param name="BrandName"></param>
        /// <param name="returnValue"></param>
        /// <returns></returns>
        public static DataTable GetMyCallsForOnsiteEngineerBySp(string userName, string state, string getGroup, string CustomeName, string BrandName,string errorStartTime,string errorEndTime, out string returnValue)
        {
            //ZQL 20151013 为APP接口编写,调用存储过程实现
            return dal.GetMyCallsForOnsiteEngineerBySp(userName, state, getGroup, CustomeName, BrandName, errorStartTime, errorEndTime, out returnValue);
        }
        /// <summary>
        /// 获取当前用户所有负责的追call
        /// </summary>
        /// <param name="CallID"></param>
        /// <returns></returns>
        public static List<CallInfo> GetListTraceByCurrentUser()
        {
            UserInfo info = UserBLL.GetCurrent();
            StringBuilder strWhere = new StringBuilder();
            strWhere.Append(" f_StateMain in(").Append((int)SysEnum.CallStateMain.处理中).Append(",").Append((int)SysEnum.CallStateMain.未处理).Append(") ");
            strWhere.Append("   AND ID IN(");
            //strWhere.Append("       SELECT DISTINCT f_CallID FROM sys_CallStep WHERE f_StepType=").Append((int)SysEnum.StepType.店铺催促);
            strWhere.Append("       SELECT MAX(f_CallID) FROM sys_CallStep WHERE f_StepType=").Append((int)SysEnum.StepType.店铺催促).Append(" group by f_CallID");
            strWhere.Append("   )");

            if (GroupBLL.PowerCheck((int)PowerInfo.P1_Call.查看组内所有报修))
            {
                strWhere.Append(" AND f_BrandID IN(SELECT f_MID FROM sys_WorkGroupBrand WHERE f_WorkGroupID=").Append(info.WorkGroupID).Append(") ");
            }
            else
            {
                strWhere.Append(" AND f_MaintainUserID=").Append(info.ID);
            }

            return dal.GetList(strWhere.ToString());
        }
        //ZQL 2015.8.15 修改为加锁的
        //public static string GetCallNoNew()
        //{
        //    string No = DateTime.Now.ToString("yyyyMMddHHmmssfff"); ;
        //    while (string.IsNullOrEmpty(No) || null != CallBLL.Get(No))
        //    {
        //        Thread.Sleep(1);
        //        No = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        //    }
        //    return No;
        //}
        private static object syncLocker = new object();
        public static string GetCallNoNew()
        {
            
            lock (syncLocker)
            {
                Thread.Sleep(3);
                string No = DateTime.Now.ToString("yyyyMMddHHmmssfff");

                while (string.IsNullOrEmpty(No) || null != CallBLL.Get(No))
                {
                    Thread.Sleep(2);
                    No = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                }
                return No;
            }
        }



        /// <summary>
        /// 获取Info
        /// </summary>
        /// <param name="id">id</param>
        public static CallInfo Get(int id)
        {
            return dal.Get(id);
        }

        public static CallInfo Get(string No)
        {
            No = Tool.Function.ClearText(No);
            return dal.Get(No);
        }

        /// <summary>
        /// 获取call数
        /// </summary>
        /// <param name="StateMain">状态</param>
        /// <param name="UserID">帐户ID</param>
        /// <returns></returns>
        public static int GetCount(int StateMain, UserInfo uinfo)
        {
            if (uinfo == null)
            {
                return 0;
            }
            if (!GroupBLL.PowerCheck((int)PowerInfo.P1_Call.查看组内所有报修))
            {
                return dal.GetCount(StateMain, uinfo.ID, 0);
            }
            return dal.GetCount(StateMain, 0, uinfo.WorkGroupID);
        }

        /// <summary>
        /// 获取call数
        /// </summary>
        /// <param name="StateMain">状态</param>
        /// <param name="UserID">帐户ID</param>
        /// ZQL 20151211新增
        /// <returns></returns>
        public static int GetCountForLeftMenuData(bool havaPower,int StateMain, UserInfo uinfo)
        {
            if (uinfo == null)
            {
                return 0;
            }
            if (!havaPower)
            {
                return dal.GetCount(StateMain, uinfo.ID, 0);
            }
            return dal.GetCount(StateMain, 0, uinfo.WorkGroupID);
        }

        /// <summary>
        /// 根据工作组找出等待上门call数
        /// </summary>
        /// <param name="WorkGroup"></param>
        /// <returns></returns>
        public static int GetCountSln1(int WorkGroup)
        {
            return dal.GetCountSln1(WorkGroup);
        }

        ///// <summary>
        ///// 获取列表
        ///// </summary>
        //public static List<CallsInfo> GetListFull(int PageSize, int CurPage, string StrWhere, out int Count)
        //{
        //    return dal.GetListFull(PageSize, CurPage, StrWhere, out Count);
        //}
        /// <summary>
        /// 获取列表
        /// </summary>
        //public static List<CallsInfo> GetListFull(string StrWhere)
        //{
        //    int count = 0;
        //    return GetListFull(999999999, 1, StrWhere, out count);
        //}
        #endregion

        #region Set
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="info">info</param>
        public static int Add(CallInfo info)
        {
            int Result = dal.Add(info);
            if (Result > 0)
            {
                SolutionInfo sinfo = SolutionBLL.Get(info.SuggestSlnID);
                if (null != sinfo)
                {
                    sinfo.SuggestCount++;
                    SolutionBLL.Edit(sinfo);
                }
            }
            return Result;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="info">info</param>
        public static bool Edit(CallInfo info)
        {
            return dal.Edit(info);
        }


        /// <summary>
        /// 编辑报修数据，同时记录日志
        /// </summary>
        /// <param name="info">修改后的Call</param>
        /// <param name="Memo">其它记录信息，如 操作人名</param>
        /// <returns></returns>
        public static bool EditWithLog(CallInfo info, string Memo)
        {
            LogInfo linfo = new LogInfo();
            linfo.AddDate = DateTime.Now;
            linfo.Category = SysEnum.LogType.报修数据修改.ToString();
            CallInfo infoOldFlash = CallBLL.Get(info.ID);
            linfo.Content = Memo;
            linfo.Content += "\n old Data:\n<br/>___________________________\n<br/>";
            linfo.Content += infoOldFlash.ToString();
            linfo.Content += "\n new Data:\n<br/>___________________________\n<br/>";
            linfo.Content += info.ToString();
            linfo.ErrorDate = DateTime.Now;
            linfo.SendEmail = false;
            linfo.Serious = 1;
            linfo.UserName = info.No;
            if (LogBLL.Add(linfo) > 0)
            {
                return Edit(info);
            }
            return false;
        }









        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">id</param>
        public static bool Delete(int id)
        {
            CallInfo cinfo=CallBLL.Get(id);
            if (null==cinfo)
	{
		 return false;
	}
            UserInfo uinfo = UserBLL.GetCurrent();
            LogInfo infoLog = new LogInfo();
            infoLog.AddDate = DateTime.Now;
            infoLog.ErrorDate = DateTime.Now;
            infoLog.Category = Enum.GetName(typeof(SysEnum.LogType), SysEnum.LogType.删除数据);
            infoLog.Content = "用户:{0}<br/>      删除了ID为:{1}的call<br/> ，call具体信息如下："+cinfo.ToString();
            infoLog.Content = string.Format(infoLog.Content, uinfo.Name, id);
            infoLog.SendEmail = true;
            infoLog.Serious = 1;
            infoLog.UserName = uinfo.Name;
            if (LogBLL.Add(infoLog) <= 0)
            {
                return false;
            }

            return dal.Delete(id);
        }

        public static bool UpdateSLADateEnd(CallInfo info)
        {
            //if (info.StateMain != (int)SysEnum.CallStateMain.未处理 && info.StateDetail != (int)SysEnum.CallStateMain.处理中)
            //{
            //    return false;
            //}
            return dal.UpdateSLADateEnd(info.ID);
        }

        ///// <summary>
        ///// 编辑记录同事增加一条复制旧映像
        ///// </summary>
        ///// <param name="info"></param>
        ///// <param name="UserID"></param>
        ///// <returns></returns>
        //public static bool EditWithFlash(CallsInfo info, int UserID)
        //{
        //    return dal.EditWithFlash(info, UserID);
        //}
        #endregion

        /// <summary>
        /// 当前报修数据是否处于上门状态
        /// </summary>
        /// <param name="CallID"></param>
        /// <returns></returns>
        public static bool IsDropIn(int StateDetail)
        {
            List<int> DropInArr = new List<int>();
            DropInArr.Add((int)SysEnum.CallStateDetails.到达门店处理);
            DropInArr.Add((int)SysEnum.CallStateDetails.等待安排上门);
            DropInArr.Add((int)SysEnum.CallStateDetails.等待备件);
            DropInArr.Add((int)SysEnum.CallStateDetails.等待第三方响应);
            DropInArr.Add((int)SysEnum.CallStateDetails.等待工程师上门);
            DropInArr.Add((int)SysEnum.CallStateDetails.工程师接收服务单);
            DropInArr.Add((int)SysEnum.CallStateDetails.等待厂商响应);
            return DropInArr.Contains(StateDetail);
        }

        /// <summary>
        /// 当前报修数据是否处于上门状态
        /// </summary>
        /// <param name="CallID"></param>
        /// <returns></returns>
        public static bool IsDropIn(CallInfo info)
        {
            if (info == null)
            {
                return false;
            }
            return IsDropIn(info.StateDetail);
        }

        /// <summary>
        /// 根据模板得具出具短信发送内容
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string GetMessageContent(CallInfo info, DateTime DropInPreDate, string details, string DropInUserName)
        {
            StoreInfo sinfo = StoresBLL.Get(info.StoreID);
            if (null == sinfo)
            {
                sinfo = new StoreInfo();
            }
            UserInfo uinfo = UserBLL.Get(info.MaintainUserID);
            if (uinfo == null)
            {
                uinfo = new UserInfo();
            }
            WorkGroupInfo winfo = WorkGroupBLL.Get(uinfo.WorkGroupID);
            if (null == winfo)
            {
                winfo = new WorkGroupInfo();
            }
            string MessageContent = ProfileBLL.GetValue(ProfileInfo.API_Message.MsgTemplate1, true);
            List<string> ReplaceItem = new List<string>();
            MessageContent = MessageContent.Replace("(((((系统单号)))))", info.No);
            MessageContent = MessageContent.Replace("(((((客户)))))", info.CustomerName);
            MessageContent = MessageContent.Replace("(((((品牌)))))", info.BrandName);
            MessageContent = MessageContent.Replace("(((((店铺号)))))", info.StoreName);
            MessageContent = MessageContent.Replace("(((((店铺名)))))", info.StoreNo);
            MessageContent = MessageContent.Replace("(((((店铺电话)))))", sinfo.Tel);
            MessageContent = MessageContent.Replace("(((((预约上门时间)))))", DropInPreDate.ToString("yyyy-MM-dd HH:mm"));
            MessageContent = MessageContent.Replace("(((((备件详细及工作说明)))))", details);
            MessageContent = MessageContent.Replace("(((((二线工程师名)))))", info.MaintaimUserName);
            MessageContent = MessageContent.Replace("(((((二线工程师电话)))))", uinfo.Tel);
            MessageContent = MessageContent.Replace("(((((二线工程师邮箱)))))", uinfo.Email);
            MessageContent = MessageContent.Replace("(((((单号工作组)))))", winfo.Name);
            MessageContent = MessageContent.Replace("(((((上门工程师名)))))", DropInUserName);
            return MessageContent;

        }

        /// <summary>
        /// 当前权限组要显示的字段
        /// </summary>
        public static List<string> GetListItem(UserInfo CurrentUser)
        {
            return GroupBLL.GetListItem(CurrentUser.PowerGroupID);
        }
        /// <summary>
        /// 当前权限组要显示的字段
        /// </summary>
        public static  List<string> GetListItem2(UserInfo CurrentUser)
        {
            return GroupBLL.GetListItem2(CurrentUser.PowerGroupID);
        }

        public static bool EnableEdit(CallInfo info, UserInfo CurrentUser)
        {
            if (null == info)
            {
                return false;
            }
            if (CurrentUser.Code == DicInfo.Admin)
            {
                return true;
            }
            //2015.09.14 ZQL 修改，call状态为已完成时也可以修改call
            //if (info.StateMain == (int)SysEnum.CallStateMain.已完成 || info.StateMain == (int)SysEnum.CallStateMain.已关闭)
            //{
            //    return false;
            //}
            if (info.StateMain == (int)SysEnum.CallStateMain.已关闭)
            {
                return false;
            }
            if (!GroupBLL.PowerCheck((int)PowerInfo.P1_Call.编辑记录))
            {
                return false;
            }
            if (!WorkGroupBrandBLL.HasRelaction(CurrentUser.WorkGroupID, info.BrandID))
            {
                return false;
            }
            if (info.MaintainUserID == CurrentUser.ID || info.CreatorID == CurrentUser.ID)
            {
                return true;
            }
            if (!GroupBLL.PowerCheck((int)PowerInfo.P1_Call.处理组内所有报修))
            {
                return false;
            }
            return true;
        }


        /// <summary>
        /// 用户是否有权限处理该报修
        /// </summary>
        /// <param name="CallID"></param>
        public static bool EnableTrace(CallInfo info, UserInfo CurrentUser)
        {//这里的if判断顺序很重要的，不要上下动
            if (info.StateMain == (int)SysEnum.CallStateMain.已关闭 || info.StateMain == (int)SysEnum.CallStateMain.已完成)
            {
                return false;
            }
            if (CurrentUser.Code == DicInfo.Admin)
            {
                return true;
            }
            if (null == info)
            {
                return false;
            }
            if (!WorkGroupBrandBLL.HasRelaction(CurrentUser.WorkGroupID, info.BrandID))
            {
                return false;
            }
            if (info.MaintainUserID == CurrentUser.ID || info.CreatorID == CurrentUser.ID)
            {
                return true;
            }
            if (!GroupBLL.PowerCheck((int)PowerInfo.P1_Call.处理组内所有报修))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// call是否为异地工程师处理的状态
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static bool IsCrossWorkGroup(CallInfo info, UserInfo CurrentUser)
        {
            return CurrentUser.WorkGroupID == info.AssignID;
        }
        /// <summary>
        /// 插入汉堡王Task
        /// </summary>
        /// <param name="sqlStr">sql语句</param>
        /// <returns></returns>
        public static int AddBurgerKingTask(string sqlStr)
        {
            return dal.AddBurgerKingTask(sqlStr);
        }
    }
}