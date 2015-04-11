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
using System.Net.Mail;
using EMPPLib;
using System.Threading;


public partial class page_call_slnDropIn1 : _Call_Sln1
{
    public page_call_slnDropIn1()
    {

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        //call状态规则，先判断可以跳转的，再判断不在指定状态的
        if (!IsPostBack)
        {

            TxbDate.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

            CallInfo info = GetInfo();
            if (null == info)
            {
                Function.AlertBack("数据有误。");
            }
            CallState1.CallID = info.ID;
            #region 状态跳转
            if (info.StateMain == (int)SysEnum.CallStateMain.已完成)
            {
                Function.AlertBack("数据有误，无法处理已完成的call");
                return;
            }
            if (info.StateDetail != (int)SysEnum.CallStateDetails.等待安排上门 && info.StateDetail != (int)SysEnum.CallStateDetails.等待备件)
            {
                Function.AlertBack("数据有误，call状态不符，请联系管理员");
                return;
            }
            CallStepInfo csinfo = CallStepBLL.GetLast(info.ID, SysEnum.StepType.上门准备);
            if (csinfo == null)
            {
                Function.AlertBack("此操作前一步应该是上门前的备件准备吧。请不要作弊！"); return;
            }
            if (info.StateDetail == (int)SysEnum.CallStateDetails.等待第三方响应 || info.StateDetail == (int)SysEnum.CallStateDetails.等待工程师上门)
            {
                Response.Redirect("slnDropIn2.aspx?ID=" + info.ID); return;
            }

            #endregion
            if (csinfo.StepName.Trim() == SysEnum.CallStateDetails.等待备件.ToString())
            {
                LabHardWare.Text = "<span style='color:#f60;'>安排上门前请确定‘备件详细及工作说明’中所提及备件已准备完毕。否则不要安排！！</span>";
            }
            else
            {
                LabHardWare.Text = "此前已确认无需备件或已准备好备件";
            }
            LabDetails.Text = csinfo.Details;

            #region 用户检查
            NotMyCallCheck(info.ID);
            #endregion



        }
    }


    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        if (!DdlMajorUserID.Visible || Function.ConverToInt(DdlMajorUserID.SelectedValue) <= 0)
        {
            Function.AlertMsg("请选择上门工程师"); return;
        }
        UserInfo uinfo = UserBLL.Get(Function.ConverToInt(DdlMajorUserID.SelectedValue));
        if (null == uinfo)
        {
            Function.AlertMsg("请选择上门工程师"); return;
        }
        if (TxbJobCode.Text.Trim().Length > 50)
        {
            Function.AlertMsg("请认真填写JobCode"); return;
        }
        if (Function.ConverToDateTime(TxbDate.Text.Trim()) == Tool.Function.ErrorDate)
        {
            Function.AlertMsg("预约上门时间有误"); return;
        }


        CallInfo cinfo = GetInfo();
        CallStepInfo sinfo = new CallStepInfo();
        sinfo.StepType = (int)SysEnum.StepType.上门安排;
        sinfo.AddDate = DateTime.Now;
        sinfo.CallID = cinfo.ID;
        sinfo.DateBegin = sinfo.DateEnd = Function.ConverToDateTime(TxbDate.Text.Trim());
        sinfo.Details = TxbJobCode.Text.Trim().ToLower();
        sinfo.IsSolved = false;
        sinfo.MajorUserID = uinfo.ID;
        sinfo.MajorUserName = uinfo.Name;
        sinfo.SolutionID = 0;
        sinfo.SolutionName = "";
        sinfo.StepIndex = CallStepBLL.GetMaxStepIndex(cinfo.ID) + 1;
        sinfo.UserID = CurrentUserID;
        sinfo.UserName = CurrentUserName;
        sinfo.IsSolved = false;
        if (DdlActualize.SelectedValue == "2")
        {
            sinfo.StepName = SysEnum.CallStateDetails.等待第三方响应.ToString();
            cinfo.StateDetail = (int)SysEnum.CallStateDetails.等待第三方响应;
        }
        else
        {
            sinfo.StepName = SysEnum.CallStateDetails.等待工程师上门.ToString();
            cinfo.StateDetail = (int)SysEnum.CallStateDetails.等待工程师上门;
        }

        UpdateData(cinfo, sinfo);
        BtnSubmit.Visible = false;

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
    private void UpdateData(CallInfo cinfo, CallStepInfo sinfo)
    {
        if (CallStepActionBLL.ActionYUM.Enable())
        {
            if (CallStepActionBLL.ActionInfo.IsYUM(cinfo))
            {
                if (string.IsNullOrEmpty(cinfo.CallNo3))
                {
                    Function.AlertMsg("系统没有对方扩展单号记录。请先填写");
                    return;
                }
            }
        }
        if (CallStepBLL.AddCallStep_UpdateCall(cinfo, sinfo))
        {
            string APImsg = CallStepActionBLL.StepReadyDropIn(cinfo, sinfo);
            if (APImsg != null)
            {
                if (APImsg == string.Empty)
                {
                    APImsg += "邮件发送成功";
                }
                else
                {
                    APImsg = "邮件发送失败" + APImsg;
                }
            }
            if (APImsg==null)
            {
                APImsg = string.Empty;
            }
            APImsg = APImsg.Replace("\"", "").Replace("'", "");
            string Alertmsg = "alert('成功记录。" + APImsg+ "');location.href='";
            if (GroupBLL.PowerCheck((int)PowerInfo.P1_Call.跟进处理))
            {
                Alertmsg += "sln.aspx?id=" + cinfo.ID + "';";
            }
            else
            {
                Alertmsg += "list.aspx?state=2';";
            }
              ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "", Alertmsg, true);
            return;
        }
        else
        {
            Function.AlertMsg("处理出错。请联系管理员");
            return;
        }
    }

    protected void DdlActualize_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DdlActualize.SelectedValue == "1")//当前工程师上门
        {
            CallInfo cinfo = GetInfo();
            UserInfo uinfo = UserBLL.Get(cinfo.MaintainUserID);
            if (uinfo == null)
            {
                Function.AlertMsg("系统数据发生严重错误。找不到该单技术负责人");
                return;
            }
            List<UserInfo> list = UserBLL.GetList(uinfo.WorkGroupID, SysEnum.Rule.现场工程师.ToString());
            foreach (UserInfo item in list)
            {
                item.Name = Hz2Py.JoinFirstPy(item.Name);
            }

            DdlMajorUserID.DataSource = list;
            DdlMajorUserID.DataBind();
            DdlMajorUserID.Items.Insert(0, new ListItem("请选择", "0"));
            DdlMajorUserID.Visible = true;
            DdlProvince.Visible = DdlWorkGroup.Visible = false;
        }
        if (DdlActualize.SelectedValue == "2")
        {
            DdlProvince.Visible = true;
            DdlProvince.DataSource = ProvincesBLL.GetList();
            DdlProvince.DataBind();
            DdlProvince.Items.Insert(0, new ListItem("请选择", "0"));
            DdlMajorUserID.Visible = DdlWorkGroup.Visible = false;
        }

    }
    protected void DdlProvince_SelectedIndexChanged(object sender, EventArgs e)
    {
        int ID = Function.ConverToInt(DdlProvince.SelectedValue);
        if (ID > 0)
        {
            DdlWorkGroup.DataSource = WorkGroupBLL.GetList(ID);
            DdlWorkGroup.DataBind();
            DdlWorkGroup.Items.Insert(0, new ListItem("请选择", "0"));
            DdlWorkGroup.Visible = true;
        }
    }
    protected void DdlWorkGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        int ID = Function.ConverToInt(DdlWorkGroup.SelectedValue);
        if (ID > 0)
        {
            CallInfo cinfo = GetInfo();
            UserInfo uinfo = UserBLL.Get(cinfo.MaintainUserID);
            if (uinfo == null)
            {
                Function.AlertMsg("系统数据发生严重错误。找不到该单技术负责人");
                return;
            }
            List<UserInfo> list = UserBLL.GetList(ID, SysEnum.Rule.现场工程师.ToString());
            foreach (UserInfo item in list)
            {
                item.Name = Hz2Py.JoinFirstPy(item.Name);
            }
            DdlMajorUserID.DataSource = list;
            DdlMajorUserID.DataBind();
            DdlMajorUserID.Items.Insert(0, new ListItem("请选择", "0"));
            DdlMajorUserID.Visible = true;
        }
    }

    protected void DdlMajorUserID_SelectedIndexChanged(object sender, EventArgs e)
    {
        int ID = Function.ConverToInt(DdlMajorUserID.SelectedValue);
        if (ID <= 0)
        {
            LabTel.Text = "";
            PanelSendMsg.Visible = false;
        }
        else
        {
            UserInfo uinfo = UserBLL.Get(ID);
            if (null != uinfo)
            {
                LabTel.Text = uinfo.Tel;
                PanelSendMsg.Visible = ProfileBLL.GetValue(ProfileInfo.API_Message.总开关).ToLower() == "true"; ;
            }
        }
    }


}
