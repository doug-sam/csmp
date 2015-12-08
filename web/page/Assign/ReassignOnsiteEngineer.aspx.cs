using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSMP.BLL;
using CSMP.Model;
using Tool;


public partial class page_Assign_ReassignOnsiteEngineer : _Call_Assign
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CallInfo info = GetInfo();
            if (null == info)
            {
                Function.AlertBack("数据有误。");
            }
            if (info.StateMain == (int)SysEnum.CallStateMain.已关闭)
            {
                tb_Assign.Visible = false;
            }
            //if ((info.StateDetail == (int)SysEnum.CallStateDetails.等待备件 || info.StateDetail == (int)SysEnum.CallStateDetails.等待安排上门) && info.AssignID != CurrentUser.WorkGroupID)
            //{
            //    HlAssignWorkGroup.Visible = true;
            
            //}
            CallStepInfo csinfo = CallStepBLL.GetLast(info.ID, SysEnum.StepType.上门安排);
            if (null == csinfo)
            {
                Function.AlertBack("没有上门安排记录，你作弊？！");
            }
            if (csinfo.StepName.Trim() != SysEnum.CallStateDetails.等待工程师上门.ToString())
            {
                Function.AlertBack("不符合重新指派工程师上门的条件！");
            }
            this.oldOnside.Text = csinfo.MajorUserName;
            this.TxbDate.Text = csinfo.DateBegin.ToString("yyyy-MM-dd HH:mm:ss");
            this.CallStepID.Value = csinfo.ID.ToString();
            BindUser(csinfo.MajorUserID);

            //list1.CallID = info.ID;

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
    /// <summary>
    /// 当选择的工程师有号码，显示可以发短信
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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

    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        int UserID = Function.ConverToInt(DdlMajorUserID.SelectedValue);
        if (UserID <= 0)
        {
            Function.AlertMsg("请选择负责工程师"); return;
        }
        UserInfo TargetUserInfo = UserBLL.Get(UserID);
        if (TargetUserInfo == null)
        {
            Function.AlertMsg("工程师不存在。请不要作弊"); return;
        }
        int callStepID = Function.ConverToInt(this.CallStepID.Value);
        CallStepInfo oldcsinfo = CallStepBLL.Get(callStepID);
           

        AssignInfo asinfo = new AssignInfo();
        AssignInfo asold = AssignBLL.GetChangeEngineerMax(oldcsinfo.CallID);
        asinfo.AddDate = DateTime.Now;
        asinfo.CallID = oldcsinfo.CallID;
        asinfo.UseID = UserID;
        asinfo.UserName = TargetUserInfo.Name;
        asinfo.CreatorID = CurrentUser.ID;
        asinfo.CreatorName = CurrentUser.Name;
        asinfo.WorkGroupID = TargetUserInfo.WorkGroupID;
        asinfo.CrossWorkGroup = false;
        asinfo.AssignType = 1;

        if (null == asold || asold.ID == 0)
        {
            asinfo.OldID = oldcsinfo.MajorUserID;
            asinfo.OldName = oldcsinfo.MajorUserName;
            asinfo.Step = 1;
        }
        else
        {
            asinfo.OldID = oldcsinfo.MajorUserID;
            asinfo.OldName = oldcsinfo.MajorUserName;
            asinfo.Step = asold.Step + 1;
        }

        if (asinfo.UseID == asinfo.OldID)
        {
            Function.AlertMsg("你是不是重复点击了？"); return;
        }
        CallStepInfo newcsinfo = new CallStepInfo();
        int newcsIndex = CallStepBLL.GetMaxStepIndex(oldcsinfo.CallID);
        newcsinfo.CallID = oldcsinfo.CallID;
        newcsinfo.MajorUserID = UserID;
        newcsinfo.MajorUserName = TargetUserInfo.Name;
        newcsinfo.StepIndex = newcsIndex+1;
        newcsinfo.UserID = CurrentUserID;
        newcsinfo.UserName = CurrentUserName;
        newcsinfo.AddDate = DateTime.Now.AddSeconds(1);
        newcsinfo.DateBegin = Function.ConverToDateTime(TxbDate.Text.Trim());
        newcsinfo.DateEnd = Function.ConverToDateTime(TxbDate.Text.Trim());
        newcsinfo.StepName = SysEnum.CallStateDetails.等待工程师上门.ToString();
        newcsinfo.StepType = (int)SysEnum.StepType.上门安排;
        newcsinfo.IsSolved = false;
        newcsinfo.SolutionID = 0;
        newcsinfo.SolutionName = "";
        newcsinfo.Details = "";
        

        if (CallStepBLL.Add(newcsinfo)>0)
        {
            AssignBLL.Add(asinfo);
            Function.AlertRefresh("现场工程师更换成功", "main");
            WriteLog("成功", newcsinfo.CallID);
        }
        else
        {
            Function.AlertMsg("更换失败，请联系管理员");
            WriteLog("失败", newcsinfo.CallID);
        }
    }

    protected void BindUser(int ID)
    {

        UserInfo uinfo = UserBLL.Get(ID);
        if (uinfo == null)
        {
            Function.AlertMsg("系统数据发生严重错误。找不到该单技术负责人");
            return;
        }
        List<UserInfo> list = UserBLL.GetList(uinfo.WorkGroupID, SysEnum.Rule.现场工程师.ToString());
        //List<UserInfo> list = UserBLL.GetList(72, SysEnum.Rule.现场工程师.ToString());


        DdlMajorUserID.DataSource = list;

        DdlMajorUserID.DataBind();
        DdlMajorUserID.Items.Insert(0, new ListItem("请选择", "0"));
        foreach (ListItem item in DdlMajorUserID.Items)
        {
            if (item.Value.Trim() == uinfo.ID.ToString())
            {
                DdlMajorUserID.Items.Remove(item); break;
            }
        }

    }
    protected void WriteLog(string status, int callID)
    {
        LogInfo linfo = new LogInfo();
        linfo.AddDate = DateTime.Now;
        linfo.Category = SysEnum.LogType.报修数据修改.ToString();
        linfo.Content = string.Format("更换现场工程师{0};报修信息：CallID={1}", status,callID);
        linfo.ErrorDate = DateTime.Now;
        linfo.SendEmail = false;
        linfo.Serious = 1;
        linfo.UserName = CurrentUserName;
        LogBLL.Add(linfo);
    
    }
}
