<%@ WebHandler Language="C#" Class="initData" %>

using System;
using System.Web;
using CSMP.Model;
using CSMP.BLL;
using Tool;
using System.Collections.Generic;
using System.Web.SessionState;

public class initData : IHttpHandler,IRequiresSessionState //就是这样显示的实现一下，不用实现什么方法
{
    
    public void ProcessRequest (HttpContext context) {
        string ActionName = context.Request["Action"];
        if (null != context.Session[ActionName])
        {
            List<WorkGroupEmailInfo> list = (List<WorkGroupEmailInfo>)context.Session[ActionName];
            WriteEmail(list, context);
        }
        if (null != context.Session["EmailGroup"+ActionName])
        {
            List<EmailGroupInfo> list = (List<EmailGroupInfo>)context.Session["EmailGroup" + ActionName];
            foreach (EmailGroupInfo item in list)
            {
                List<WorkGroupEmailInfo> listEmail = WorkGroupEmailBLL.GetListByEmailGroup(item.ID);
                WriteEmail(listEmail, context);
            }
        }
        
    }

    private void WriteEmail(List<WorkGroupEmailInfo> list, HttpContext context)
    {
        foreach (WorkGroupEmailInfo item in list)
        {
            string EmailAddress = string.Format("{0}<{1}>;", item.Name, item.Email);
            context.Response.Write(EmailAddress);
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}