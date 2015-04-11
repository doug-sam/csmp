<%@ WebHandler Language="C#" Class="Validate" %>

using System;
using System.Web;
using Tool;
using CSMP.BLL;
using CSMP.Model;


public class Validate : IHttpHandler
{
    HttpContext ctx = null;
    public void ProcessRequest(HttpContext context)
    {
        ctx = context;
        int ServerCurrentVersion = Function.ConverToInt(ProfileBLL.GetValue(ProfileInfo.UserKey.AppVersion, true), 0);
        int ClientVersion = Function.Request.GetRequestInt(ctx, "ClientVersion");
        if (ClientVersion<ServerCurrentVersion)
        {
            OutPut("false");
        }

    }

    private void OutPut(object Value)
    {
        object o = new  { 
            Text=Value
        };
        string PrintResult = Newtonsoft.Json.JsonConvert.SerializeObject(o);

        ctx.Response.Write(JoinCallBack(PrintResult));
    }

    private string JoinCallBack(string PrintValue)
    {
        return Function.Request.GetRequestSrtring(ctx, "callback") + "(" + PrintValue + ");";
    }

    
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}