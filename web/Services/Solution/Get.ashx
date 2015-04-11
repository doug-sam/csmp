<%@ WebHandler Language="C#" Class="Get" %>

using System;
using System.Web;
using Tool;
using CSMP.BLL;
using CSMP.Model;
using System.Collections.Generic;


public class Get : IHttpHandler {
    private enum ActionMethord { GetByClass3 = 1, History = 2 }
    HttpContext ctx = null;
    public void ProcessRequest (HttpContext context) {
        ctx = context;
        int ActionID = Function.Request.GetRequestInt(context, "ActionID");

        switch (ActionID)
        {
            case (int)ActionMethord.GetByClass3:
                ActionMethord_GetByClass3();
                break;
            case (int)ActionMethord.History:
                
                break;
            default:
                break;
        }
    }

    private void ActionMethord_GetByClass3()
    {
        int Class3ID = Function.Request.GetRequestInt(ctx, "Class3ID");
        if (Class3ID<=0)
        {
            return;
        }
        List<SolutionInfo> list = SolutionBLL.GetList(Class3ID);
        Newtonsoft.Json.Converters.IsoDateTimeConverter timeConverter = new Newtonsoft.Json.Converters.IsoDateTimeConverter();
        timeConverter.DateTimeFormat = "yyyy'-'MM'-'dd' 'HH':'mm':'ss";
        string Result = Newtonsoft.Json.JsonConvert.SerializeObject(list, timeConverter);

        ctx.Response.Write(Result);
    }
    
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}