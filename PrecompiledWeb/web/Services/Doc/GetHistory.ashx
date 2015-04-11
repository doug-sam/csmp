<%@ WebHandler Language="C#" Class="GetHistory" %>

using System;
using System.Web;
using CSMP.BLL;
using CSMP.Model;
using Tool;
using System.Collections.Generic;

public class GetHistory : IHttpHandler {

    HttpContext ctx;
    public void ProcessRequest (HttpContext context) {
        ctx = context;
        int CallID = Function.Request.GetRequestInt(context,"CallID");
        if (CallID<=0)
        {
            return;
        }
       List<AttachmentInfo> list= AttachmentBLL.GetList(CallID,AttachmentInfo.EUserFor.Call);
       Newtonsoft.Json.Converters.IsoDateTimeConverter timeConverter = new Newtonsoft.Json.Converters.IsoDateTimeConverter();
       timeConverter.DateTimeFormat = "yyyy'-'MM'-'dd' 'HH':'mm':'ss";
       string Result = Newtonsoft.Json.JsonConvert.SerializeObject(list, timeConverter);

       context.Response.Write(JoinCallBack(Result));

    }
    private string JoinCallBack(string PrintValue)
    {
        return Function.Request.GetRequestSrtring(ctx, "callback") + "(" + PrintValue + ");";
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}