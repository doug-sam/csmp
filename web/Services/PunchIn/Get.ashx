<%@ WebHandler Language="C#" Class="Get" %>

using System;
using System.Web;
using Tool;
using CSMP.BLL;
using CSMP.Model;
using System.Collections.Generic;


public class Get : IHttpHandler {
    private enum PunchInType { GetToWork=1,LeaveWork=2,Noral=3}
    private enum ActionMethord { Add = 1, GetList = 2,GetLast }
    HttpContext ctx = null;
    public void ProcessRequest (HttpContext context) {
        ctx = context;
        int ActionID = Function.Request.GetRequestInt(context, "ActionID");

        switch (ActionID)
        {
            case (int)ActionMethord.Add:
                ActionMethord_Add();
                break;
            case (int)ActionMethord.GetList:
                ActionMethord_GetList();
                break;
            case (int)ActionMethord.GetLast:
                ActionMethord_GetLast();
                break;
            default:
                break;
        }
    }

    private void ActionMethord_Add()
    {
        int UserID = Function.Request.GetRequestInt(ctx, "UserID");
        if (UserID <= 0)
        {
            PrintJsonp(0, "登录名有误");
            return;
        }
        UserInfo info = UserBLL.Get(UserID);
        if (null == info)
        {
            PrintJsonp(0, "登录名有误");
            return;
        }
        if (info.Name != Function.Request.GetRequestSrtring(ctx, "UserName").Trim())
        {
            PrintJsonp(0, "登录名有误");
            return;
        }
        PunchInInfo pinfo = new PunchInInfo();
        pinfo.AddByUserID = info.ID;
        pinfo.AddByUserName = info.Name;
        pinfo.UserID = info.ID;
        pinfo.UserName = info.Name;
        pinfo.GroupID = info.WorkGroupID;
        pinfo.DateAdd = DateTime.Now;
        pinfo.DateRegister = pinfo.DateAdd;
        pinfo.DateRegisterAbs = pinfo.DateAdd;
        pinfo.PositionLat = Function.ConverToDecimal(Function.Request.GetRequestSrtring(ctx, "lat"));
        pinfo.PositionLog = Function.ConverToDecimal(Function.Request.GetRequestSrtring(ctx, "log"));
        pinfo.PositionAddress = Function.Request.GetRequestSrtring(ctx, "PositionAddress");
        pinfo.DeviceDetail = Function.Request.GetRequestSrtring(ctx, "DeviceDetail");
        pinfo.Memo = Function.Request.GetRequestSrtring(ctx, "Memo");
        pinfo.Device = Function.Request.GetRequestSrtring(ctx, "Device").Trim();
        if (string.IsNullOrEmpty(pinfo.Device) || string.IsNullOrEmpty(pinfo.PositionAddress) || pinfo.PositionLat <= 0 || pinfo.PositionLog<=0)
        {
            PrintJsonp(0, "参数有误");
        }
        pinfo.Memo = Function.Request.GetRequestSrtring(ctx, "Memo").Trim();
        pinfo.IsStartWork = Function.Request.GetRequestInt(ctx, "IsStartWork");
        if (pinfo.IsStartWork < 1 || pinfo.IsStartWork > 3)
        {
            PrintJsonp(0, "非法操作");
            return;
        }

        PunchInInfo infoLast = PunchInBLL.GetLastPunch(info.ID);
        if (null!=infoLast&&infoLast.IsStartWork==pinfo.IsStartWork)
        {
            PrintJsonp(0, "非法操作，跟上次打卡操作相同，请退出后重试");
            return;
        }
        

        string PunchinWhat = string.Empty;
        switch (pinfo.IsStartWork)
        {
            case (int)PunchInType.GetToWork:
                PunchinWhat = "上班打卡";
                break;
            case (int)PunchInType.LeaveWork:
                PunchinWhat = "收工打卡";
                break;
            case (int)PunchInType.Noral:
                PunchinWhat = "其它打卡";
                break;
            default:
                return;
                break;
        }

        if (PunchInBLL.Add(pinfo) > 0)
        {
            PrintJsonp(1, PunchinWhat + "成功");
            return;
        }
        else
        {
            PrintJsonp(0, PunchinWhat + "失败，请联系管理员");
        }

    }

    private void ActionMethord_GetList()
    {
        int UserID = Function.Request.GetRequestInt(ctx, "UserID");
        if (UserID <= 0)
        {
            PrintJsonp(0, "登录名有误");
            return;
        }
        UserInfo info = UserBLL.Get(UserID);
        if (null == info)
        {
            PrintJsonp(0, "登录名有误");
            return;
        }
        if (info.Name != Function.Request.GetRequestSrtring(ctx, "UserName").Trim())
        {
            PrintJsonp(0, "登录名有误");
            return;
        }

        List<PunchInInfo> list = PunchInBLL.GetList(DateTime.Now.AddDays(-30), DateTime.Now, info.ID);
        List<PunchInViewInfo> listview = PunchInBLL.ToPunchInView(list);
        Newtonsoft.Json.Converters.IsoDateTimeConverter timeConverter = new Newtonsoft.Json.Converters.IsoDateTimeConverter();
        timeConverter.DateTimeFormat = "yyyy'-'MM'-'dd' 'HH':'mm':'ss";
        string Result = Newtonsoft.Json.JsonConvert.SerializeObject(listview, timeConverter);

        Result = JoinCallBack(Result);
        ctx.Response.Write(Result);

    }

    
    private void ActionMethord_GetLast()
    {
        PunchInInfo pinfo = new PunchInInfo();
        int UserID = Function.Request.GetRequestInt(ctx, "UserID");
        if (UserID <= 0)
        {
            pinfo.ID = -1;
            pinfo.Memo = "登录名有误1";
            PrintInfo(pinfo);
            return;
        }
        UserInfo info = UserBLL.Get(UserID);
        if (null == info)
        {
            pinfo.ID = -1;
            pinfo.Memo = "登录名有误2";
            PrintInfo(pinfo);
            return;
        }
        if (info.Name != Function.Request.GetRequestSrtring(ctx, "UserName").Trim())
        {
            pinfo.ID = -1;
            pinfo.Memo = "登录名有误3";
            PrintInfo(pinfo);
            return;
        }
       pinfo= PunchInBLL.GetLastPunch(info.ID);
       if (null == pinfo || pinfo.ID <= 0)
       {
           pinfo = new PunchInInfo();
       }
       PrintInfo(pinfo);

    }

    private void PrintInfo(PunchInInfo pinfo)
    {
        Newtonsoft.Json.Converters.IsoDateTimeConverter timeConverter = new Newtonsoft.Json.Converters.IsoDateTimeConverter();
        timeConverter.DateTimeFormat = "yyyy'-'MM'-'dd' 'HH':'mm':'ss";
        string Result = Newtonsoft.Json.JsonConvert.SerializeObject(pinfo, timeConverter);

        ctx.Response.Write(JoinCallBack(Result));
    }


    private void PrintJsonp(int iResult,string iMsg)
    {
        object o = new
        {
            Result = iResult,
            Msg = iMsg,
        };
        string PrintResult = Newtonsoft.Json.JsonConvert.SerializeObject(o);
        ctx.Response.Write(JoinCallBack(PrintResult));
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