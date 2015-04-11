<%@ WebHandler Language="C#" Class="Get" %>

using System;
using System.Web;
using Tool;
using CSMP.BLL;
using CSMP.Model;
using System.Collections.Generic;

public class Get : IHttpHandler
{
    HttpContext ctx = null;

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "application/x-javascript";
        ctx = context;
        int CallID = Function.Request.GetRequestInt(ctx, "CallID");
        if (CallID <= 0)
        {
            return;
        }
        CallInfo info = CallBLL.Get(CallID);
        if (null == info || info.ID <= 0)
        {
            return;
        }
        List<CallStepInfo> list = CallStepBLL.GetListJoin(info);
        Newtonsoft.Json.Converters.IsoDateTimeConverter timeConverter = new Newtonsoft.Json.Converters.IsoDateTimeConverter();
        timeConverter.DateTimeFormat = "yyyy'-'MM'-'dd' 'HH':'mm':'ss";
        string Result = Newtonsoft.Json.JsonConvert.SerializeObject(list, timeConverter);
        ctx.Response.Write(JoinCallBack(Result));

    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

    private string JoinCallBack(string PrintValue)
    {
        return Function.Request.GetRequestSrtring(ctx, "callback") + "(" + PrintValue + ");";
    }


}