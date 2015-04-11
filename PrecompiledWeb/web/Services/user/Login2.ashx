<%@ WebHandler Language="C#" Class="Login2" %>

using System;
using System.Web;
using CSMP.BLL;
using CSMP.Model;
using Tool;
using System.Web.SessionState;


public class Login2 : IHttpHandler,IRequiresSessionState 
{
    private void PrintJsonp(string iResult, UserInfo info)
    {
        if (null==info)
        {
            info = new UserInfo();
            info.ID = 0;
            info.Name = string.Empty;
            info.Rule = new System.Collections.Generic.List<string>();
        }
        object o =new { Result=iResult,
                        UserID=info.ID,
                        UserName=info.Name,
                        Rule=string.Join(",",info.Rule.ToArray())
                   };
        string PrintResult = Newtonsoft.Json.JsonConvert.SerializeObject(o);
        ctx.Response.Write(JoinCallBack(PrintResult));
    }

    private string JoinCallBack(string PrintValue)
    {
        return Function.Request.GetRequestSrtring(ctx, "callback") + "(" + PrintValue + ");";
    }

    HttpContext ctx;
    public void ProcessRequest(HttpContext context)
    {
        ctx = context;
        string UserName = Function.ClearText(Function.Request.GetRequestSrtring(context, "_UserName")).Trim();
        string Pwd = Function.Request.GetRequestSrtring(context, "_Pwd").Trim();
        if (UserName.Length > 50 || string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Pwd))
        {
            PrintJsonp("信息有误", null);
            return;
        }
        SysEnum.LoginState stateofLogin = UserBLL.Login(UserName, Pwd, 0);
        if (stateofLogin == SysEnum.LoginState.登录成功)
        {
            PrintJsonp(SysEnum.LoginState.登录成功.ToString(),  UserBLL.Get(UserName));

        }
        else
        {
            PrintJsonp(stateofLogin.ToString(), null);
        }
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}