using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSMP.BLL;
using CSMP.Model;
using Tool;
using System.Collections;


public partial class page_call_slnDropInCancel : _Call_Step
{
    int[] RightState = new int[] {
        (int)SysEnum.CallStateDetails.等待备件,
        (int)SysEnum.CallStateDetails.等待安排上门,
        (int)SysEnum.CallStateDetails.等待第三方响应,
        (int)SysEnum.CallStateDetails.等待工程师上门,
        (int)SysEnum.CallStateDetails.第三方预约上门
    };

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CallInfo info = GetInfo();
            if (null == info)
            {
                Function.AlertBack("数据有误。");
            }
            CallState1.CallID = info.ID;
            if (info.StateMain == (int)SysEnum.CallStateMain.已完成)
            {
                Function.AlertBack("数据有误，无法处理已完成的call");
                return;
            }
            #region 状态跳转
            if (!RightState.Contains(info.StateDetail))
            {
                Function.AlertBack("数据状态有误");
                return;
            }
            #endregion


            #region 用户检查
            NotMyCallCheck(info.ID);
            #endregion

        }
    }

    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        CallInfo cinfo = GetInfo();
        if (!RightState.Contains(cinfo.StateDetail))
        {
            Function.AlertMsg("数据失效，请刷新");
            return;
        }

        CallStepInfo sinfo = new CallStepInfo();
        sinfo.StepType = (int)GetStepType();
        sinfo.MajorUserID = CurrentUserID;
        sinfo.MajorUserName = CurrentUserName;
        sinfo.SolutionID = 0;
        sinfo.SolutionName = "";
        sinfo.UserID = CurrentUserID;
        sinfo.UserName = CurrentUserName;
        sinfo.AddDate = DateTime.Now;
        sinfo.CallID = cinfo.ID;
        sinfo.StepIndex = CallStepBLL.GetMaxStepIndex(cinfo.ID) + 1;
        sinfo.DateEnd = sinfo.DateBegin = DateTime.Now;
        sinfo.Details = TxbDetails.Text.Trim();

        sinfo.IsSolved = false;
        sinfo.StepName = GetStateDetails().ToString();
        cinfo.StateDetail = (int)GetStateDetails();
        cinfo.StateMain = (int)SysEnum.CallStateMain.处理中;
        UpdateData(cinfo, sinfo);
        BtnSubmit.Visible = false;

    }

    private SysEnum.StepType GetStepType()
    {
        CallInfo info = GetInfo();
        switch (info.StateDetail)
        {
            case (int)SysEnum.CallStateDetails.第三方预约上门:
                return SysEnum.StepType.第三方预约上门;
            default:
                return SysEnum.StepType.取消上门;
        }
    }
    private SysEnum.CallStateDetails GetStateDetails()
    {
        CallInfo info = GetInfo();
        switch (info.StateDetail)
        {
            case (int)SysEnum.CallStateDetails.第三方预约上门:
                return SysEnum.CallStateDetails.第三方预约取消;
            default:
                return SysEnum.CallStateDetails.上门取消;
        }
    }


    private CallInfo GetInfo()
    {
        if (ViewState["info"] != null)
        {
            return (CallInfo)ViewState["info"];
        }
        int ID = Function.GetRequestInt("id");
        if (ID <= 0)
        {
            return null;
        }
        CallInfo info = CallBLL.Get(ID);
        if (null == info)
        {
            return null;
        }
        ViewState["info"] = info;
        return info;
    }
    private void UpdateData(CallInfo cinfo, CallStepInfo sinfo)
    {
        if (CallStepBLL.AddCallStep_UpdateCall(cinfo, sinfo))
        {
            // ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "", "alert('成功记录');location.href='list.aspx?state=2';", true);
            ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "", "alert('成功记录');location.href='sln.aspx?id=" + cinfo.ID + "';", true);
            return;
        }
        else
        {
            Function.AlertMsg("处理出错。请联系管理员");
            return;
        }
    }

}
