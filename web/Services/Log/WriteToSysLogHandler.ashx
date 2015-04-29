<%@ WebHandler Language="C#" Class="WriteToSysLogHandler" %>

using System;
using System.Web;
using CSMP.BLL;
using CSMP.Model;

public class WriteToSysLogHandler : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        string category = context.Request.Params["category"].ToString().Trim();
        string msg = context.Request.Params["msg"].ToString().Trim();
        string result = string.Empty;
        result = "1";
        LogInfo linfo = new LogInfo();
        linfo.AddDate = DateTime.Now;
        linfo.Category = category;
        linfo.Content = msg;
        linfo.ErrorDate = DateTime.Now;
        linfo.SendEmail = false;
        linfo.Serious = 0;
        linfo.UserName = UserBLL.GetCurrentEmployeeName();
        LogBLL.Add(linfo);        
        context.Response.ContentType = "text/plain";
        context.Response.Write(result);
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}