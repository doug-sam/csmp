<%@ WebHandler Language="C#" Class="sch" %>

using System;
using System.Web;
using Newtonsoft.Json;
using CSMP.BLL;
using CSMP.Model;
using Tool;
using System.Collections.Generic;


public class sch : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        UserInfo uinfo = UserBLL.GetCurrent();
        if (uinfo==null)
        {
            return;
        }
        string key = Function.GetRequestSrtring("q");
        if (string.IsNullOrEmpty(key))
        {
            return;
        }
        List<StoreInfo> list= StoresBLL.GetList(key,uinfo.Name==DicInfo.Admin?0:uinfo.WorkGroupID);
        if (list==null||list.Count==0)
        {
            return;
        }
        string json = JsonConvert.SerializeObject(list);
        context.Response.Write(json);
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}