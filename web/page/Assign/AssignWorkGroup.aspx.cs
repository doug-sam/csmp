using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSMP.BLL;
using CSMP.Model;
using Tool;

public partial class page_Assign_AssignWorkGroup : _Call_Assign
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

            int GrobalWorkGroupID=Function.ConverToInt(ProfileBLL.GetValue(ProfileInfo.UserKey.GrobalGroupID),0);
            DdlUser.DataSource = UserBLL.GetList(GrobalWorkGroupID, SysEnum.Rule.二线.ToString());
            DdlUser.DataBind();
            DdlUser.Items.Insert(0, new ListItem("请选择", "0"));

            list1.CallID = info.ID;

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

    protected void BtnSubmit_Click(object sender, EventArgs e)
    {

        int UserID = Function.ConverToInt(DdlUser.SelectedValue);
        if (UserID <= 0)
        {
            Function.AlertMsg("请选择负责工程师"); return;
        }
        UserInfo TargetUserInfo = UserBLL.Get(UserID);
        if (TargetUserInfo == null)
        {
            Function.AlertMsg("工程师不存在。请不要作弊"); return;
        }
        CallInfo info = GetInfo();

        AssignInfo asinfo = new AssignInfo();
        AssignInfo asold = AssignBLL.GetMax(info.ID);
        asinfo.AddDate = DateTime.Now;
        asinfo.CallID = info.ID;
        asinfo.UseID = TargetUserInfo.ID;
        asinfo.UserName = TargetUserInfo.Name;
        asinfo.CreatorID = CurrentUser.ID;
        asinfo.CreatorName = CurrentUser.Name;
        asinfo.WorkGroupID = TargetUserInfo.WorkGroupID;

        if (null == asold || asold.ID == 0)
        {
            asinfo.OldID = info.MaintainUserID;
            asinfo.OldName = info.MaintaimUserName;
            asinfo.Step = 1;
        }
        else
        {
            asinfo.OldID = asold.UseID;
            asinfo.OldName = asold.UserName;
            asinfo.Step = asold.Step + 1;
        }
        if (TargetUserInfo.WorkGroupID != info.WorkGroupID)
        {
            asinfo.CrossWorkGroup = true;
            int CrossWorkGroupID = AssignBLL.GetCrossWorkGroupID(info.ID);
            if (CrossWorkGroupID == 0 && info.AssignID == 0)
            {
                info.AssignID = TargetUserInfo.WorkGroupID;
                info.AssignUserID = TargetUserInfo.ID;
                info.AssignUserName = TargetUserInfo.Name;

                info.MaintainUserID = TargetUserInfo.ID;
                info.MaintaimUserName = TargetUserInfo.Name;
            }
        }
        else
        {
            asinfo.CrossWorkGroup = false;
        }




        if (CallBLL.Edit(info))
        {
            Logger.GetLogger(this.GetType()).Info("跨组转派修改主表状态成功，callid=" + asinfo.CallID + "，操作人：" + info.CreatorName, null);
            AssignBLL.Add(asinfo);
            Function.AlertRefresh("转派成功", "main");
        }
        else
        {
            Function.AlertMsg("转派失败，请联系管理员");
        }
    }

    public string GetWorkGroupName(int WorkGroupID)
    {
        WorkGroupInfo winfo = WorkGroupBLL.Get(WorkGroupID);
        if (null == winfo)
        {
            return string.Empty;
        }
        return winfo.Name;
    }


    private void BindUser(int WorkGroupID)
    {
        if (WorkGroupID <= 0)
        {
            return;
        }
        DdlUser.DataSource = UserBLL.GetList(WorkGroupID, SysEnum.Rule.二线.ToString());
        DdlUser.DataBind();
        DdlUser.Items.Insert(0, new ListItem("请选择", "0"));
        foreach (ListItem item in DdlUser.Items)
        {
            if (item.Value.Trim() == GetInfo().MaintainUserID.ToString())
            {
                DdlUser.Items.Remove(item); break;
            }
        }
    }
}
