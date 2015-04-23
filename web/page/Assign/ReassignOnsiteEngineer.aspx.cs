﻿using System;
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
        CallStepInfo csinfo = CallStepBLL.Get(callStepID);
        csinfo.MajorUserID = UserID;
        csinfo.MajorUserName = TargetUserInfo.Name;
        csinfo.AddDate = DateTime.Now;
        csinfo.DateBegin = Function.ConverToDateTime(TxbDate.Text.Trim());
        csinfo.DateEnd = Function.ConverToDateTime(TxbDate.Text.Trim());

        if (CallStepBLL.Edit(csinfo))
        {
            
            Function.AlertRefresh("现场工程师更换成功", "main");
        }
        else
        {
            Function.AlertMsg("更换失败，请联系管理员");
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
}