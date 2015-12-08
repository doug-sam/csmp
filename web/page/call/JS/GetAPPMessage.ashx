<%@ WebHandler Language="C#" Class="GetAPPMessage" %>

using System;
using System.Web;
using System.Collections.Generic;
using CSMP.BLL;
using CSMP.Model;
using System.Web.SessionState;
public class GetAPPMessage : IHttpHandler, IRequiresSessionState
{
    
    public void ProcessRequest (HttpContext context) {
        UserInfo CurrentUser = UserBLL.GetCurrent();
        
        if (null == CurrentUser)
        {
            return;
        }
        List<string> list = new List<string>();
        List<MarqueeMessage> messageList = MarqueeMessageBLL.GetByMaintaimUserID(CurrentUser.ID);
        if (messageList.Count > 0)
        {
            foreach (MarqueeMessage message in messageList)
            {
                list.Add(string.Format("现场工程师 {0} {1} 单号 {2}",message.MajorUserName,message.Content,message.No));
            }
        }
        else {
            return;
        }
                
        //context.Response.ContentType = "text/plain";
        context.Response.Write(string.Join("|", list.ToArray()));
        //context.Response.Write(list[i]);
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}