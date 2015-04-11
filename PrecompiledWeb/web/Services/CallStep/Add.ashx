<%@ WebHandler Language="C#" Class="Add" %>

using System;
using System.Web;
using Tool;
using CSMP.BLL;
using CSMP.Model;


public class Add : IHttpHandler
{
    private enum ActionMethord { UserArrive = 1, UserLeave = 2 }
    HttpContext ctx = null;
    public void ProcessRequest(HttpContext context)
    {
        ctx = context;
        int ActionID = Function.Request.GetRequestInt(context, "ActionID");

        switch (ActionID)
        {
            case (int)ActionMethord.UserArrive:
                UserArrive();
                break;
            case (int)ActionMethord.UserLeave:
                UserLeave();
                break;
            default:
                break;
        }
    }

    private void OutPut(object Value)
    {
        ctx.Response.Write(Value.ToString());
    }

    private void UserLeave()
    {
        //bool IsSloved = Function.Request.GetRequestInt(ctx, "IsSloved") == 1;
        int CallID = Function.Request.GetRequestInt(ctx, "CallID");
        string Details = Function.Request.GetRequestSrtring(ctx, "Details");
        int UserID = Function.Request.GetRequestInt(ctx, "UserID");
        string UserName = Function.Request.GetRequestSrtring(ctx, "UserName");
        int Score = Function.Request.GetRequestInt(ctx, "Score");
        //int SlnID = Function.Request.GetRequestInt(ctx, "SlnID");
        //SolutionInfo slnInfo = null;
        CallInfo cinfo = CallBLL.Get(CallID);
        if ((int)SysEnum.CallStateDetails.到达门店处理 != cinfo.StateDetail)
        {
            OutPut("数据有误！"); return;
        }
        UserInfo PostUser = UserBLL.Get(UserID);
        if (null == PostUser || PostUser.Name != UserName)
        {
            OutPut("Fuck you,don't miss my data！"); return;
        }
        if (Details.Length > 500)
        {
            OutPut("过程备注太长。不应超500字"); return;
        }

        CallStepInfo sinfo = new CallStepInfo();
        CallStepInfo csinfo = CallStepBLL.GetLast(cinfo.ID, SysEnum.StepType.到达门店处理);

        // UserInfo uinfo = UserBLL.Get(Function.ConverToInt(DdlUser.SelectedValue));
        sinfo.StepType = (int)SysEnum.StepType.上门详细;
        sinfo.MajorUserID = PostUser.ID;
        sinfo.MajorUserName = PostUser.Name;
        sinfo.UserID = PostUser.ID;
        sinfo.UserName = PostUser.Name;
        sinfo.AddDate = DateTime.Now;
        sinfo.CallID = cinfo.ID;
        sinfo.StepIndex = CallStepBLL.GetMaxStepIndex(cinfo.ID) + 1;
        sinfo.DateBegin = sinfo.DateEnd = DateTime.Now;
        sinfo.Details = Details;
        sinfo.StepName = SysEnum.CallStateDetails.上门支持.ToString();
        sinfo.IsSolved = false;

        sinfo.SolutionID = 0;
        sinfo.SolutionName = "";
        cinfo.StateMain = (int)SysEnum.CallStateMain.处理中;
        cinfo.StateDetail = (int)SysEnum.CallStateDetails.上门支持;


        if (CallStepBLL.AddCallStep_UpdateCall(cinfo, sinfo))
        {
            Comment_Add(cinfo, PostUser, Score);
            OutPut(1);
            return;
        }
        OutPut(0);


    }
    private void UserArrive()
    {
        //(int)SysEnum.CallStateDetails.等待工程师上门,
        //(int)SysEnum.CallStateDetails.等待第三方响应,
        //(int)SysEnum.CallStateDetails.等待厂商响应
        int PostUserID = Function.Request.GetRequestInt(ctx, "CurrentUserID");
        string PostUserName = Function.Request.GetRequestSrtring(ctx, "CurrentUserName");
        int CallID = Function.Request.GetRequestInt(ctx, "CallID");
        string Details = Function.Request.GetRequestSrtring(ctx, "Details");
        CallInfo infoCall = CallBLL.Get(CallID);
        if (null == infoCall)
        {
            OutPut(0); return;
        }
        if (infoCall.StateDetail != (int)SysEnum.CallStateDetails.等待工程师上门 && infoCall.StateDetail != (int)SysEnum.CallStateDetails.等待第三方响应 && infoCall.StateDetail != (int)SysEnum.CallStateDetails.等待厂商响应)
        {
            OutPut(0); return;
        }
        UserInfo CurrentUserInfo = UserBLL.Get(PostUserID);
        if (null == CurrentUserInfo || PostUserName != CurrentUserInfo.Name)
        {
            OutPut(0); return;
        }

        CallInfo cinfo = CallBLL.Get(CallID);
        if (null == cinfo || cinfo.ID <= 0)
        {
            OutPut(0); return;
        }


        CallStepInfo sinfo = new CallStepInfo();
        sinfo.StepType = (int)SysEnum.StepType.到达门店处理;
        sinfo.UserID = CurrentUserInfo.ID;
        sinfo.UserName = CurrentUserInfo.Name;
        sinfo.AddDate = DateTime.Now;
        sinfo.CallID = cinfo.ID;
        sinfo.StepIndex = CallStepBLL.GetMaxStepIndex(cinfo.ID) + 1;
        sinfo.DateBegin = sinfo.AddDate;
        sinfo.DateEnd = sinfo.AddDate;
        sinfo.StepName = SysEnum.CallStateDetails.到达门店处理.ToString();
        sinfo.IsSolved = false;
        sinfo.SolutionID = 0;
        sinfo.SolutionName = "";
        sinfo.Details = "工程师到达门店（通过手机终端）；工程师定位：" + Details;

        sinfo.MajorUserID = CurrentUserInfo.ID;
        sinfo.MajorUserName = CurrentUserInfo.Name;
        cinfo.StateDetail = (int)SysEnum.CallStateDetails.到达门店处理;

        if (CallStepBLL.AddCallStep_UpdateCall(cinfo, sinfo))
        {
            OutPut(1);
            return;
        }
        OutPut(0);


    }

    //添加评论
    private bool Comment_Add(CallInfo cinfo,UserInfo CurrentUserInfo,int Score)
    {
        CallStepInfo stepinfo = CallStepBLL.GetLast(cinfo.ID, SysEnum.StepType.上门详细);
        CommentInfo info = new CommentInfo();
        info.AddDate = DateTime.Now;
        info.ByMachine = CommentInfo.MachineType.android.ToString();
        info.CallID = cinfo.ID;
        info.CallStepID = stepinfo.ID;
        info.Details = string.Format("现场工程{0}对二线人员{1}评分值:{2}", cinfo.MaintaimUserName, stepinfo.MajorUserName, Score);
        info.DropInUserID = CurrentUserInfo.ID;
        info.IsDropInUserDoIt = true;
        info.Score = Score;
        info.SupportUserID = cinfo.MaintainUserID;
        info.WorkGroupID = cinfo.WorkGroupID;
        info.ID = CommentBLL.Add(info);
        return info.ID > 0;
    }

    
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}