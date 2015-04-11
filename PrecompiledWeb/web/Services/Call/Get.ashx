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

public class Get : IHttpHandler
{
    private enum ActionMethord { Current = 1, History, GetOne, Sch }
    HttpContext ctx = null;
    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "application/x-javascript";
        ctx = context;

        int ActionID = Function.Request.GetRequestInt(context, "ActionID");

        switch (ActionID)
        {
            case (int)ActionMethord.Current:
                ActionMethord_CurrentORHistory();
                break;
            case (int)ActionMethord.History:
                ActionMethord_CurrentORHistory();
                break;
            case (int)ActionMethord.GetOne:
                ActionMethord_GetOne();
                break;
            case (int)ActionMethord.Sch:
                ActionMethord_Sch();
                break;
            default:
                break;
        }

    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

    private List<CallInfo> JoinDropInUser(List<CallInfo> list)
    {
        foreach (CallInfo item in list)
        {
            CallStepInfo sinfo = CallStepBLL.GetLast(item.ID);
            item.Details = string.Empty;
            if (null != sinfo)
            {
                item.Details = sinfo.MajorUserName;
            }
        }
        return list;
    }

    private void ActionMethord_Sch()
    {
        int DropInUserID = Function.Request.GetRequestInt(ctx, "DropInUserID");
        string CallNo = Function.ClearText(Function.Request.GetRequestSrtring(ctx, "CallNo"));
        UserInfo uinfo = UserBLL.Get(DropInUserID);
        if (null == uinfo)
        {
            return;
        }
        StringBuilder SQL = new StringBuilder();
        SQL.Append(" 1=1 ");
        SQL.Append(" AND f_WorkGroupID=" + uinfo.WorkGroupID);
        SQL.Append(" AND f_No like '%").Append(CallNo).Append("%' ");
        SQL.Append(" ORDER BY ID DESC");

        int Count = 0;
        List<CallInfo> list = CallBLL.GetList(50, 1, SQL.ToString(), out Count);
        list = JoinDropInUser(list);
        Newtonsoft.Json.Converters.IsoDateTimeConverter timeConverter = new Newtonsoft.Json.Converters.IsoDateTimeConverter();
        timeConverter.DateTimeFormat = "yyyy'-'MM'-'dd' 'HH':'mm':'ss";
        string Result = Newtonsoft.Json.JsonConvert.SerializeObject(list, timeConverter);

        ctx.Response.Write(Function.Request.GetRequestSrtring(ctx, "callback") + "(" + Result + ");");
    }

    private void ActionMethord_GetOne()
    {
        int CallID = Function.Request.GetRequestInt(ctx, "CallID");
        CallInfo info = CallBLL.Get(CallID);
        if (null != info && info.ID > 0)
        {
            Newtonsoft.Json.Converters.IsoDateTimeConverter timeConverter = new Newtonsoft.Json.Converters.IsoDateTimeConverter();
            timeConverter.DateTimeFormat = "yyyy'-'MM'-'dd' 'HH':'mm':'ss";
            string Result = Newtonsoft.Json.JsonConvert.SerializeObject(info, timeConverter);

            ctx.Response.Write(JoinCallBack(Result));
        }
    }

    private void ActionMethord_CurrentORHistory()
    {
        int DropInUserID = Function.Request.GetRequestInt(ctx, "DropInUserID");
        int CallState = Function.Request.GetRequestInt(ctx, "CallState");
        int CurPage = Function.GetRequestInt("PageIndex");
        bool IsCurrentUser = Function.Request.GetRequestInt(ctx, "IsCurrentUser") == 1;
        if (DropInUserID <= 0)
        {
            return;
        }
        if (CurPage <= 0)
        {
            CurPage = 1;
        }
        //string SQL = " ID in(SELECT MAX(f_CallID) FROM sys_CallStep where f_StepType={0} and f_MajorUserID ={1}  GROUP BY f_CallID   ) ";
        StringBuilder SQL = new StringBuilder();
        SQL.Append(" 1=1 ");
        if (!IsCurrentUser)
        {
            UserInfo uinfo = UserBLL.Get(DropInUserID);
            SQL.Append("AND f_WorkGroupID=").Append(null == uinfo ? 0 : uinfo.WorkGroupID);
        }
        SQL.Append(" AND ID not in(select f_CallID from sys_CallStep where  f_StepName ='").Append(SysEnum.CallStateDetails.等待第三方响应.ToString()).Append("')");
        SQL.Append(" AND ID in(select f_CallID from sys_CallStep where  f_StepType in( ");
        SQL.Append("                                                                    ").Append((int)SysEnum.StepType.上门安排);
        SQL.Append(",                                                                   ").Append((int)SysEnum.StepType.到达门店处理);
        SQL.Append("                                                                 )   ");
        if (IsCurrentUser)
        {
            SQL.Append(" and f_MajorUserID =").Append(DropInUserID);
        }

        SQL.Append("  ) and f_StateMain in(");
        if (CallState == (int)SysEnum.CallStateMain.处理中)
        {
            SQL.Append((int)SysEnum.CallStateMain.处理中);
        }
        else
        {
            SQL.Append((int)SysEnum.CallStateMain.已完成).Append(",");
            SQL.Append((int)SysEnum.CallStateMain.已关闭);
        }
        SQL.Append(" ) order by ID desc"); 
        
        int Count = 0;
        List<CallInfo> list = CallBLL.GetList(20, CurPage, SQL.ToString(), out Count);
        list = JoinDropInUser(list);
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