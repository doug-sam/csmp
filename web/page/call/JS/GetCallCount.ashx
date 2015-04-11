<%@ WebHandler Language="C#" Class="GetCallCount" %>

using System;
using System.Web;
using System.Collections.Generic;
using CSMP.BLL;
using CSMP.Model;
using System.Web.SessionState;


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
        list.Add(CSMP.BLL.CallBLL.GetCount((int)SysEnum.CallStateMain.已关闭, CurrentUser).ToString());
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
        context.Response.Write(string.Join("|", list.ToArray()));
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}