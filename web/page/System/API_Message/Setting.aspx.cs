using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


using CSMP.Model;
using CSMP.BLL;
using Tool;

public partial class system_API_Message_Setting : _Sys_Profile
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            TxbHost.Text = ProfileBLL.GetValue(ProfileInfo.API_Message.Host, true);
            TxbPort.Text = ProfileBLL.GetValue(ProfileInfo.API_Message.Port, true);
            TxbPwd.Text = ProfileBLL.GetValue(ProfileInfo.API_Message.password, true);
            TxbAccountId.Text = ProfileBLL.GetValue(ProfileInfo.API_Message.accountId, true);
            TxbServiceId.Text = ProfileBLL.GetValue(ProfileInfo.API_Message.serviceId, true);
            CbEnable.Checked = ProfileBLL.GetValue(ProfileInfo.API_Message.总开关, true).ToLower() == "true";

            TxbTempLate1.Text = ProfileBLL.GetValue(ProfileInfo.API_Message.MsgTemplate1, true);

            List<string> ReplaceItem = new List<string>();
            ReplaceItem.Add("系统单号");
            ReplaceItem.Add("客户");
            ReplaceItem.Add("品牌");
            ReplaceItem.Add("店铺号");
            ReplaceItem.Add("店铺名");
            ReplaceItem.Add("店铺电话");
            ReplaceItem.Add("预约上门时间");
            ReplaceItem.Add("备件详细及工作说明");
            ReplaceItem.Add("二线工程师名");
            ReplaceItem.Add("二线工程师电话");
            ReplaceItem.Add("二线工程师邮箱");
            ReplaceItem.Add("单号工作组");
            ReplaceItem.Add("上门工程师名");
            Repeater1.DataSource = ReplaceItem;
            Repeater1.DataBind();
        }
    }
    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        ProfileInfo pHost = ProfileBLL.Get(ProfileInfo.API_Message.Host);
        ProfileInfo pPort = ProfileBLL.Get(ProfileInfo.API_Message.Port);
        ProfileInfo pPwd = ProfileBLL.Get(ProfileInfo.API_Message.password);
        ProfileInfo pAccountId = ProfileBLL.Get(ProfileInfo.API_Message.accountId);
        ProfileInfo pServiceId = ProfileBLL.Get(ProfileInfo.API_Message.serviceId);
        ProfileInfo pEnable = ProfileBLL.Get(ProfileInfo.API_Message.总开关);

        pHost.Value = TxbHost.Text.Trim();
        pPort.Value = TxbPort.Text.Trim();
        pPwd.Value = TxbPwd.Text.Trim();
        pAccountId.Value = TxbAccountId.Text.Trim();
        pServiceId.Value = TxbServiceId.Text.Trim();
        pEnable.Value = CbEnable.Checked ? "true" : "false";


        ProfileBLL.Edit(pHost);
        ProfileBLL.Edit(pPort);
        ProfileBLL.Edit(pPwd);
        ProfileBLL.Edit(pAccountId);
        ProfileBLL.Edit(pServiceId);
        ProfileBLL.Edit(pEnable);
        Function.Refresh();

    }
    protected void BtnTempLate1_Click(object sender, EventArgs e)
    {
        ProfileInfo TempLate1 = ProfileBLL.Get(ProfileInfo.API_Message.MsgTemplate1);

        TempLate1.Value = TxbTempLate1.Text.Trim();
        ProfileBLL.Edit(TempLate1);
        Function.Refresh();
    }


}
