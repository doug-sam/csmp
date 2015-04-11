<%@ WebHandler Language="C#" Class="Get" %>

using System;
using System.Web;
using Tool;
using CSMP.Model;
using CSMP.BLL;

public class Get : IHttpHandler {
    private enum ActionMethord { GetInfo = 1, GetList = 2 }
    HttpContext ctx = null;

    public void ProcessRequest (HttpContext context) {
        ctx = context;
        int ActionID = Function.Request.GetRequestInt(context, "ActionID");

        switch (ActionID)
        {
            case (int)ActionMethord.GetInfo:
                GetInfo();
                break;
            case (int)ActionMethord.GetList:
                GetList();
                break;
            default:
                break;
        }

    }

    private void GetInfo()
    {
        int ID = Function.Request.GetRequestInt(ctx, "StoreID");
        if (ID <= 0) return;
        StoreInfo info = StoresBLL.Get(ID);
        if (null == info) return;
        Newtonsoft.Json.Converters.IsoDateTimeConverter timeConverter = new Newtonsoft.Json.Converters.IsoDateTimeConverter();
        timeConverter.DateTimeFormat = "yyyy'-'MM'-'dd' 'HH':'mm':'ss";
        string Result = Newtonsoft.Json.JsonConvert.SerializeObject(info, timeConverter);
        ctx.Response.Write(Result);
    }

    private void GetList()
    { 
        
    }
    
    public bool IsReusable {
        get {
            return false;
        }
    }

}