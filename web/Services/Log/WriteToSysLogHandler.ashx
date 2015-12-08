<%@ WebHandler Language="C#" Class="WriteToSysLogHandler" %>

using System;
using System.Web;
using CSMP.BLL;
using CSMP.Model;
using Tool;

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
        Logger.GetLogger(this.GetType()).Info("开始处理log，点击外呼记录日志，日志详情：" + msg + "，操作人：" + linfo.UserName + "\r\n", null);
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