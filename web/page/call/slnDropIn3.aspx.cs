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


public partial class page_call_slnDropIn3 : _Call_Step
{
    int[] RightState = new int[] {
        (int)SysEnum.CallStateDetails.到达门店处理
    };


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CallInfo info = GetInfo();
            if (null == info)
            {
                Function.AlertMsg("数据有误。"); return;
            }
            CallState1.CallID = info.ID;

            #region 状态跳转
            if (!RightState.Contains(info.StateDetail))
            {
                Function.AlertBack("数据状态有误");
                return;
            }
            #endregion

            CallStepInfo csinfo = CallStepBLL.GetLast(info.ID, SysEnum.StepType.到达门店处理);
            if (null == csinfo)
            {
                Function.AlertBack("没有到达门店记录，你作弊？！");
            }
            UserInfo uinfo = UserBLL.Get(csinfo.MajorUserID);
            if (null == uinfo)
            {
                Function.AlertBack("系统发生严重错误，帐户信息丢失");
            }
            LabUser.Text = csinfo.MajorUserName;
            LabDateBegin.Text = csinfo.DateBegin.ToString("yyyy-MM-dd HH:mm");

            TxbDateEnd.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

            #region 用户检查
            NotMyCallCheck(info.ID);
            #endregion

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

    public int GetLastStepID()
    {
        int StepID = Function.GetRequestInt("StepID");

        CallStepInfo csinfo = CallStepBLL.GetLast(GetInfo().ID, SysEnum.StepType.到达门店处理);
        return csinfo == null ? 0 : csinfo.ID;
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
        CallStepInfo csinfo = CallStepBLL.GetLast(cinfo.ID, SysEnum.StepType.到达门店处理);

        // UserInfo uinfo = UserBLL.Get(Function.ConverToInt(DdlUser.SelectedValue));
        sinfo.StepType = (int)SysEnum.StepType.上门详细;
        sinfo.MajorUserID = csinfo.MajorUserID;
        sinfo.MajorUserName = csinfo.MajorUserName;
        sinfo.UserID = CurrentUserID;
        sinfo.UserName = CurrentUserName;
        sinfo.AddDate = DateTime.Now;
        sinfo.CallID = cinfo.ID;
        sinfo.StepIndex = CallStepBLL.GetMaxStepIndex(cinfo.ID) + 1;
        sinfo.DateBegin = sinfo.DateEnd = Function.ConverToDateTime(TxbDateEnd.Text.Trim());
        sinfo.Details = TxbDetails.Text.Trim();
        sinfo.StepName = SysEnum.CallStateDetails.上门支持.ToString();
        sinfo.IsSolved = false;
        #region 验证
        if (sinfo.DateBegin == Function.ErrorDate || sinfo.DateEnd == Function.ErrorDate)
        {
            Function.AlertMsg("请认真填写抵达和离场时间"); return;
        }
        if (sinfo.DateBegin > sinfo.DateEnd)
        {
            Function.AlertMsg("抵达现场时间比离开时间晚。你填错了吧？"); return;
        }
        if (sinfo.Details.Length > 500)
        {
            Function.AlertMsg("过程备注太长。不应超500字"); return;
        }
        #endregion
        sinfo.SolutionID = 0;
        sinfo.SolutionName = "";
        cinfo.StateMain = (int)SysEnum.CallStateMain.处理中;
        cinfo.StateDetail = (int)SysEnum.CallStateDetails.上门支持;


        UpdateData(cinfo, sinfo);
        BtnSubmit.Visible = false;


    }

    private void UpdateData(CallInfo cinfo, CallStepInfo sinfo)
    {
        if (CallStepBLL.AddCallStep_UpdateCall(cinfo, sinfo))
        {
            string js = "top.ReloadLeft();alert('成功记录');location.href='";
            js += "sln.aspx?id=" + cinfo.ID ;
            js += "';";
            ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "", js, true);
            return;
        }
        else
        {
            Function.AlertMsg("处理出错。请联系管理员");
            return;
        }
    }

}
