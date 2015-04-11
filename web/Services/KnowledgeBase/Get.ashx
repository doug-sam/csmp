<%@ WebHandler Language="C#" Class="Get" %>

using System;
using System.Web;
using Tool;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using CSMP.BLL;
using CSMP.Model;
using System.Collections.Generic;
using System.Text;
using Tool;
using CSMP.BLL;
using CSMP.Model;

public class Get : IHttpHandler
{
    private enum ActionMethord { ListOfClass3 = 1, ListOfSch, GetOne }
    HttpContext ctx = null;
    public void ProcessRequest(HttpContext context)
    {
        ctx = context;
        int ActionID = Function.Request.GetRequestInt(context, "ActionID");
        string wd = Function.Request.GetRequestSrtring(context, "wd");
        wd = Function.ClearText(wd);
        if (!string.IsNullOrEmpty(wd))
        {
            ActionID = (int)ActionMethord.ListOfSch;
        }
        switch (ActionID)
        {
            case (int)ActionMethord.ListOfClass3:
                ActionMethord_ListOfClass3();
                break;
            case (int)ActionMethord.ListOfSch:
                ActionMethord_ListOfSch();
                break;
            case (int)ActionMethord.GetOne:
                ActionMethord_GetOne();
                break;
            default:
                break;
        }

        // context.Response.Write("Hello World" + ActionID);
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

    private void ActionMethord_ListOfClass3()
    {
        int Class3ID = Function.Request.GetRequestInt(ctx, "Class3ID");
        if (Class3ID <= 0)
        {
            return;
        }
        List<KnowledgeBaseInfo> list = KnowledgeBaseBLL.GetListByBrandID(Class3ID);
        Newtonsoft.Json.Converters.IsoDateTimeConverter timeConverter = new Newtonsoft.Json.Converters.IsoDateTimeConverter();
        timeConverter.DateTimeFormat = "yyyy'-'MM'-'dd' 'HH':'mm':'ss";
        string Result = Newtonsoft.Json.JsonConvert.SerializeObject(list, timeConverter);

        Result = JoinCallBack(Result);
        ctx.Response.Write(Result);
    }

    private void ActionMethord_GetOne()
    {
        int KnowledgeID = Function.Request.GetRequestInt(ctx, "KnowledgeID");
        KnowledgeBaseInfo info = KnowledgeBaseBLL.Get(KnowledgeID);
        if (null != info && info.ID > 0)
        {
            Newtonsoft.Json.Converters.IsoDateTimeConverter timeConverter = new Newtonsoft.Json.Converters.IsoDateTimeConverter();
            timeConverter.DateTimeFormat = "yyyy'-'MM'-'dd' 'HH':'mm':'ss";
            string Result = Newtonsoft.Json.JsonConvert.SerializeObject(info, timeConverter);

            ctx.Response.Write(JoinCallBack(Result));
        }
    }

    private void ActionMethord_ListOfSch()
    {
        string wd = Function.Request.GetRequestSrtring(ctx, "wd");
        wd = Function.ClearText(wd);
        if (string.IsNullOrEmpty(wd))
        {
            return;
        }
        StringBuilder SQL = new StringBuilder();
        SQL.Append(" 1=1 and  f_Title like '%").Append(wd).Append("%' or f_Labs like '%").Append(wd).Append("%' ");
        int Count = 0;
        List<KnowledgeBaseInfo> list = KnowledgeBaseBLL.GetList(100, 1, SQL.ToString(), out Count);
        Newtonsoft.Json.Converters.IsoDateTimeConverter timeConverter = new Newtonsoft.Json.Converters.IsoDateTimeConverter();
        timeConverter.DateTimeFormat = "yyyy'-'MM'-'dd' 'HH':'mm':'ss";
        string Result = Newtonsoft.Json.JsonConvert.SerializeObject(list, timeConverter);

        ctx.Response.Write(Function.Request.GetRequestSrtring(ctx, "callback") + "(" + Result + ");");
    }

    private string JoinCallBack(string PrintValue)
    {
        return Function.Request.GetRequestSrtring(ctx, "callback") + "(" + PrintValue + ");";
    }
}