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


public partial class page_call_slnDropIn2 : _Call_Step
{
    int[] RightState = new int[] {
        (int)SysEnum.CallStateDetails.等待工程师上门,
        (int)SysEnum.CallStateDetails.等待第三方响应,
        (int)SysEnum.CallStateDetails.等待厂商响应
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

            #region 状态跳转
            if (!RightState.Contains(info.StateDetail))
            {
                Function.AlertBack("数据状态有误");
                return;
            }
            #endregion

            CallStepInfo csinfo = CallStepBLL.GetLast(info.ID, SysEnum.StepType.上门安排);
            if (null == csinfo)
            {
                Function.AlertBack("没有上门安排记录，你作弊？！");
            }

            UserInfo uinfo = UserBLL.Get(csinfo.MajorUserID);
            if (null == uinfo)
            {
                Function.AlertBack("系统发生严重错误，帐户信息丢失");
            }
            if (info.StateDetail==(int)SysEnum.CallStateDetails.等待厂商响应)
            {
                DdlUser.Visible = false;
                TxbDetails.Text = "厂商到达门店处理。";
            }
            else
            {
                DdlUser.DataSource = UserBLL.GetList(uinfo.WorkGroupID, SysEnum.Rule.现场工程师.ToString());
                DdlUser.DataBind();
                DdlUser.SelectedValue = uinfo.ID.ToString();

            }

            TxbDate.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

            NotMyCallCheck(info.ID);
           // EnterCheck(info);

        }
    }

    public CallInfo GetInfo()
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

        CallStepInfo csinfo = CallStepBLL.GetLast(GetInfo().ID, SysEnum.StepType.上门安排);
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
        TxbDetails.Text=TxbDetails.Text.Trim();
        if (TxbDetails.Text.Length>500)
        {
            Function.AlertMsg("备注过长。不能超500字");
            return;
        }
        CallStepInfo sinfo = new CallStepInfo();
        sinfo.StepType = (int)SysEnum.StepType.到达门店处理;
        sinfo.UserID = CurrentUserID;
        sinfo.UserName = CurrentUserName;
        sinfo.AddDate = DateTime.Now;
        sinfo.CallID = cinfo.ID;
        sinfo.StepIndex = CallStepBLL.GetMaxStepIndex(cinfo.ID) + 1;
        sinfo.DateBegin = Function.ConverToDateTime(TxbDate.Text.Trim());
        sinfo.DateEnd = sinfo.DateBegin;
        sinfo.StepName = SysEnum.CallStateDetails.到达门店处理.ToString();
        sinfo.IsSolved = false;
        sinfo.SolutionID = 0;
        sinfo.SolutionName = "";
        sinfo.Details = TxbDetails.Text;
        if (cinfo.StateDetail == (int)SysEnum.CallStateDetails.等待厂商响应)
        {
            CallStepInfo LastStep = CallStepBLL.GetLast(cinfo.ID, SysEnum.StepType.上门安排);
            if (null==LastStep)
            {
                Function.AlertMsg("数据出错，请联系管理员"); return;
            }
            sinfo.MajorUserID = LastStep.MajorUserID;
            sinfo.MajorUserName = LastStep.MajorUserName;
        }
        else
        {
            UserInfo uinfo = UserBLL.Get(Function.ConverToInt(DdlUser.SelectedValue));
            if (null==uinfo)
            {
                Function.AlertMsg("数据发生严重错误。请联系管理员"); return;
            }
            sinfo.MajorUserID = uinfo.ID;
            sinfo.MajorUserName = uinfo.Name;
        }
        cinfo.StateDetail = (int)SysEnum.CallStateDetails.到达门店处理;

        

        #region 验证
        if (sinfo.DateBegin == Tool.Function.ErrorDate)
        {
            Function.AlertMsg("请认真填写抵达时间"); return;
        }
        #endregion
        

        UpdateData(cinfo, sinfo);
        BtnSubmit.Visible = false;


    }

    private void UpdateData(CallInfo cinfo, CallStepInfo sinfo)
    {
        if (CallStepBLL.AddCallStep_UpdateCall(cinfo, sinfo))
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "", "top.ReloadLeft();alert('成功记录');location.href='sln.aspx?id=" + cinfo.ID + "';", true);
            return;
        }
        else
        {
            Function.AlertMsg("处理出错。请联系管理员");
            return;
        }
    }


}
