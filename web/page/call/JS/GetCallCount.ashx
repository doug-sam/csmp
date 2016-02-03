<%@ WebHandler Language="C#" Class="GetCallCount" %>

using System;
using System.Web;
using System.Collections.Generic;
using CSMP.BLL;
using CSMP.Model;
using System.Web.SessionState;
using Tool;


public class GetCallCount : IHttpHandler,IRequiresSessionState //就是这样显示的实现一下，不用实现什么方法
{
    
    public void ProcessRequest (HttpContext context) {
        UserInfo CurrentUser = UserBLL.GetCurrent();
        if (null==CurrentUser)
        {
            return;
        }
        List<string> list = new List<string>();
        list.Add(CallBLL.GetCountSln1(CurrentUser.WorkGroupID).ToString());
        list.Add( CSMP.BLL.CallBLL.GetCount((int)SysEnum.CallStateMain.未处理, CurrentUser).ToString());
        list.Add(CSMP.BLL.CallBLL.GetCount((int)SysEnum.CallStateMain.处理中, CurrentUser).ToString());
        list.Add(CSMP.BLL.CallBLL.GetCount((int)SysEnum.CallStateMain.已完成, CurrentUser).ToString());
        //list.Add(CSMP.BLL.CallBLL.GetCount((int)SysEnum.CallStateMain.已关闭, CurrentUser).ToString());
        #region 读缓存close数
        try
        {
            int closeCount = 0;
            List<LeftMenuData> cacheList = CacheManage.GetSearch("leftMenuKey") as List<LeftMenuData>;
            if (cacheList == null || cacheList.Count <= 0)
            {

                LeftMenuDataBLL.InsertLeftMenuDataCache();
                cacheList = CacheManage.GetSearch("leftMenuKey") as List<LeftMenuData>;
                Logger.GetLogger(this.GetType()).Info("获取关闭工单数时缓存数据为空，从数据库中同步一次数据到缓存中。\r\n", null);
            }
            int index = cacheList.FindIndex(s => s.UserID == CurrentUser.ID);
            if (cacheList[index].HaveGroupPower)
            {
                foreach (LeftMenuData item in cacheList)
                {
                    if (item.WorkGroupID == cacheList[index].WorkGroupID)
                    {
                        closeCount += item.Closed;
                    }
                }
            }
            else {
                closeCount = cacheList[index].Closed;
            }
            list.Add(closeCount.ToString());
            Logger.GetLogger(this.GetType()).Info("获取close数成功，操作人" + CurrentUser.Name + "，close总数为:" + closeCount + "。\r\n", null);
        }
        catch (Exception ex)
        {
            list.Add(CSMP.BLL.CallBLL.GetCount((int)SysEnum.CallStateMain.已关闭, CurrentUser).ToString());
            Logger.GetLogger(this.GetType()).Info("获取close数失败，失败原因：" + ex.Message + "\r\n", null);
        }
        
        #endregion
        
        
        List<CallInfo> Tracelist = CallBLL.GetListTraceByCurrentUser();
        if (null==Tracelist||Tracelist.Count==0)
        {
            list.Add("0");
        }
        else
        {
            list.Add(Tracelist.Count.ToString());    
        }
        list.Add(CSMP.BLL.CustomerRequestBLL.GetConut(CurrentUser.WorkGroupID).ToString());

        List<MarqueeMessage> messageList = MarqueeMessageBLL.GetByMaintaimUserID(CurrentUser.ID);
        if (messageList.Count > 0)
        {
            list.Add("有提醒" + messageList.Count+ "条");
        }
        else {
            list.Add("无提醒");
        }
        context.Response.Write(string.Join("|", list.ToArray()));
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}