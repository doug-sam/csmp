using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using CSMP.BLL;
using CSMP.Model;
using Tool;

public partial class page_call_sln : _Call_Step
{
    public string CTIWSIP
    {
        get { return ConfigurationManager.AppSettings["CTIWSIP"].ToString(); }
    }
    public string CTIWSPort
    {
        get { return ConfigurationManager.AppSettings["CTIWSPort"].ToString(); }
    }
    public string CTIWSObAniName
    {
        get { return ConfigurationManager.AppSettings["CTIWSObAniName"].ToString(); }
    }
    public string CTIWSObDnisName
    {
        get { return ConfigurationManager.AppSettings["CTIWSObDnisName"].ToString(); }
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
        CallState1.CallID = info.ID;
        ViewState["info"] = info;
        return info;
    }
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
            CheckStatus(info);
            GetAniDnis4Outbound(info.StoreID);

        }
    }

    private void CheckStatus(CallInfo info)
    {
        if (info.StateMain != (int)SysEnum.CallStateMain.处理中 && info.StateMain != (int)SysEnum.CallStateMain.未处理)
        {
            Response.Write("数据有误，无法处理已完成的call");
            Response.End();
            return;
        }

        switch ((SysEnum.CallStateDetails)info.StateDetail)
        {
            case SysEnum.CallStateDetails.系统接单_未处理:
                DdlSln.Visible = false;
                BtnDeal.Visible = true;
                TxbDetail.Enabled = true;
                break;
            case SysEnum.CallStateDetails.电话支持:
                break;
            case SysEnum.CallStateDetails.升级到客户:
                break;
            case SysEnum.CallStateDetails.等待备件:
                Response.Redirect("slnDropIn1.aspx?ID=" + info.ID);
                break;
            case SysEnum.CallStateDetails.等待安排上门:
                Response.Redirect("slnDropIn1.aspx?ID=" + info.ID);
                break;
            case SysEnum.CallStateDetails.等待第三方响应:
                Response.Redirect("slnDropIn2.aspx?ID=" + info.ID);
                break;
            case SysEnum.CallStateDetails.等待工程师上门:
                Response.Redirect("slnDropIn2.aspx?ID=" + info.ID);
                break;
            case SysEnum.CallStateDetails.工程师接收服务单:
                Response.Redirect("slnDropIn2.aspx?ID=" + info.ID);
                break;
            case SysEnum.CallStateDetails.上门支持:
                Response.Redirect("slnDropIn4.aspx?ID=" + info.ID);
                break;
            case SysEnum.CallStateDetails.二线离场确认:
                break;
            case SysEnum.CallStateDetails.处理完成:
                Response.End();
                break;
            case SysEnum.CallStateDetails.已回访:
                Response.End();
                break;
            case SysEnum.CallStateDetails.到达门店处理:
                Response.Redirect("slnDropIn3.aspx?ID=" + info.ID);
                break;
            case SysEnum.CallStateDetails.等待厂商响应:
                Response.Redirect("slnDropIn2.aspx?ID=" + info.ID);
                break;
            case SysEnum.CallStateDetails.上门取消:
                break;
            case SysEnum.CallStateDetails.第三方预约上门:
                Response.Redirect("/page/CallStep/ThirdPartyLeave.aspx?ID=" + info.ID);
                break;
            case SysEnum.CallStateDetails.第三方预约取消:
                break;
            case SysEnum.CallStateDetails.第三方处理离场:
                break;
            default:
                break;
        }


        #region 用户检查
        NotMyCallCheck(info.ID);
        #endregion
    }

    protected void DdlSln_SelectedIndexChanged(object sender, EventArgs e)
    {
        int ID = Function.ConverToInt(DdlSln.SelectedValue);
        if (ID <= 0)
        {
            return;
        }
        switch (ID)
        {
            case 1:
                Response.Redirect("slnRemote.aspx?ID=" + GetInfo().ID);
                break;
            case 2:
                Response.Redirect("slnCustomer.aspx?ID=" + GetInfo().ID);
                break;
            case 3:
                Response.Redirect("slnDropIn.aspx?ID=" + GetInfo().ID);
                break;
            case 4:
                Response.Redirect("/page/CallStep/ThirdParty.aspx?ID=" + GetInfo().ID);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 开始处理
    /// </summary>
    protected void BtnDeal_Click(object sender, EventArgs e)
    {
        string callbackrecordid = Request["callbackrecordid"];
        int POS1 = callbackrecordid.IndexOf("#");
        if ((POS1 + 1) >= callbackrecordid.Length)
            POS1 = -1;
        int POS2 = callbackrecordid.IndexOf("#", POS1 + 1);

        if (POS1 != -1 && POS2 != -1 && POS2 > POS1)
        {
            callbackrecordid = callbackrecordid.Substring(POS1 + 1, POS2 - POS1 - 1);
            POS1 = callbackrecordid.IndexOf("=");
            if (POS1 != -1)
                callbackrecordid = callbackrecordid.Substring(POS1 + 1);
            else
                callbackrecordid = "";
        }
        else
            callbackrecordid = "";

        CallInfo cinfo = GetInfo();
        CheckStatus(cinfo);

        CallStepInfo sinfo = new CallStepInfo();
        sinfo.StepType = (int)SysEnum.StepType.开始处理;
        sinfo.MajorUserID = CurrentUserID;
        sinfo.MajorUserName = CurrentUserName;
        sinfo.AddDate = DateTime.Now;
        sinfo.CallID = cinfo.ID;
        sinfo.DateBegin = DateTime.Now;
        sinfo.DateEnd = sinfo.DateBegin;
        if (string.IsNullOrEmpty(callbackrecordid))
            sinfo.Details = TxbDetail.Text;
        else
            sinfo.Details = TxbDetail.Text + "  A$B$C" + callbackrecordid + "D$E$F";
        sinfo.StepIndex = CallStepBLL.GetMaxStepIndex(cinfo.ID) + 1;
        sinfo.IsSolved = false;
        sinfo.StepName = "由" + CurrentUserName + "进行负责处理";
        sinfo.UserID = CurrentUser.ID;
        sinfo.UserName = CurrentUser.Name;
        sinfo.SolutionID = 0;
        sinfo.SolutionName = string.Empty;

        cinfo.StateMain = (int)SysEnum.CallStateMain.处理中;
        cinfo.StateDetail = (int)SysEnum.CallStateDetails.开始处理;

        if (CallStepBLL.AddCallStep_UpdateCall(cinfo, sinfo))
        {
            string js = "top.ReloadLeft();alert('成功记录');location.href='";
            js += "sln.aspx?id=" + cinfo.ID;
            js += "';";
            ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "", js, true);
        }
        else
        {
            Function.AlertMsg("提交失败，重试不行的话联系管理员");
            return;
        }
    }

    /// <summary>
    /// 获取外呼用的主被叫号码
    /// </summary>
    /// <param name="StoreID"></param>
    protected void GetAniDnis4Outbound(int StoreID)
    {
        StoreInfo sinfo = StoresBLL.Get(StoreID);
        if (null == sinfo)
            return;
        txtCalledNO.Text = sinfo.Tel;
        if (HttpContext.Current.Request.Cookies != null && HttpContext.Current.Request.Cookies["Station"] != null)
        {
            txtStation.Text = HttpContext.Current.Request.Cookies["Station"].Value.ToString();
        }
    }
}

