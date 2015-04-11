using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


using CSMP.Model;
using CSMP.BLL;
using Tool;

public partial class system_EmailSetting : _Sys_Profile
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            TxbAddress.Text = ProfileBLL.GetValue(ProfileInfo.UserKey.发件邮箱地址, true);
            TxbHost.Text = ProfileBLL.GetValue(ProfileInfo.UserKey.发件邮件主机, true);
            TxbPort.Text = ProfileBLL.GetValue(ProfileInfo.UserKey.发件邮箱端口, true);
            TxbPwd.Text = ProfileBLL.GetValue(ProfileInfo.UserKey.发件邮箱密码, true);
            TxbDisplayName.Text = ProfileBLL.GetValue(ProfileInfo.UserKey.发件人显示名, true);
            TxbReplayName.Text = ProfileBLL.GetValue(ProfileInfo.UserKey.回复时显示名, true);
            TxbReplayAddress.Text = ProfileBLL.GetValue(ProfileInfo.UserKey.回复地址, true);
           
            

        }
    }
    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        ProfileInfo Email_Address = ProfileBLL.Get(ProfileInfo.UserKey.发件邮箱地址);
        ProfileInfo Email_Host = ProfileBLL.Get(ProfileInfo.UserKey.发件邮件主机);
        ProfileInfo Email_Port = ProfileBLL.Get(ProfileInfo.UserKey.发件邮箱端口);
        ProfileInfo Email_Pwd = ProfileBLL.Get(ProfileInfo.UserKey.发件邮箱密码);
        ProfileInfo Email_DisplayName = ProfileBLL.Get(ProfileInfo.UserKey.发件人显示名);
        ProfileInfo Email_ReplyName = ProfileBLL.Get(ProfileInfo.UserKey.回复时显示名);
        ProfileInfo Email_ReplyAddress = ProfileBLL.Get(ProfileInfo.UserKey.回复地址);


        Email_Address.Value = TxbAddress.Text.Trim();
        Email_Host.Value = TxbHost.Text.Trim();
        Email_Port.Value = TxbPort.Text.Trim();
        Email_Pwd.Value = TxbPwd.Text.Trim();
        Email_DisplayName.Value = TxbDisplayName.Text.Trim();
        Email_ReplyName.Value = TxbReplayName.Text.Trim();
        Email_ReplyAddress.Value = TxbReplayAddress.Text.Trim();


        ProfileBLL.Edit(Email_Address);
        ProfileBLL.Edit(Email_Host);
        ProfileBLL.Edit(Email_Port);
        ProfileBLL.Edit(Email_Pwd);
        ProfileBLL.Edit(Email_DisplayName);
        ProfileBLL.Edit(Email_ReplyName);
        ProfileBLL.Edit(Email_ReplyAddress);
        Function.Refresh();

    }
}
