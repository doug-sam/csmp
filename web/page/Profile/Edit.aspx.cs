using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSMP.BLL;
using CSMP.Model;
using Tool;

public partial class page_Profile_Edit : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            TxbSysAddress.Text = ProfileBLL.GetValue(ProfileInfo.UserKey.系统地址, true);
            TxbRecordUrl.Text = ProfileBLL.GetValue(ProfileInfo.UserKey.电话服务器录音根地址, true);
            TxbSysAddressWWW.Text = ProfileBLL.GetValue(ProfileInfo.UserKey.系统外网地址, true);
            TxbAppVersion.Text = ProfileBLL.GetValue(ProfileInfo.UserKey.AppVersion, true);
            TxbGrobalGroup.Text = ProfileBLL.GetValue(ProfileInfo.UserKey.GrobalGroupID, true);
        }
    }
    protected void BtnSysAddress_Click(object sender, EventArgs e)
    {
        ProfileInfo info = ProfileBLL.Get(ProfileInfo.UserKey.系统地址);
        info.Value = TxbSysAddress.Text.Trim();
        if (ProfileBLL.Edit(info))
        {
            Function.AlertRefresh("修改成功");
        }
        else
        {
            Function.AlertBack("修改失败");
        }
    }
    protected void BtnRecordUrl_Click(object sender, EventArgs e)
    {
        ProfileInfo info = ProfileBLL.Get(ProfileInfo.UserKey.电话服务器录音根地址);
        info.Value = TxbRecordUrl.Text.Trim();
        if (ProfileBLL.Edit(info))
        {
            Function.AlertRefresh("修改成功");
        }
        else
        {
            Function.AlertBack("修改失败");
        }
    }
    protected void BtnSysAddressWWW_Click(object sender, EventArgs e)
    {
        ProfileInfo info = ProfileBLL.Get(ProfileInfo.UserKey.系统外网地址);
        info.Value = TxbSysAddressWWW.Text.Trim();
        if (ProfileBLL.Edit(info))
        {
            Function.AlertRefresh("修改成功");
        }
        else
        {
            Function.AlertBack("修改失败");
        }
    }
    protected void BtnAppVersion_Click(object sender, EventArgs e)
    {
        ProfileInfo info = ProfileBLL.Get(ProfileInfo.UserKey.AppVersion);
        info.Value = TxbAppVersion.Text.Trim();
        if (ProfileBLL.Edit(info))
        {
            Function.AlertRefresh("修改成功");
        }
        else
        {
            Function.AlertBack("修改失败");
        }
    }
    protected void BtnGrobalGroup_Click(object sender, EventArgs e)
    {
        ProfileInfo info = ProfileBLL.Get(ProfileInfo.UserKey.GrobalGroupID);
        info.Value = TxbGrobalGroup.Text.Trim();
        if (ProfileBLL.Edit(info))
        {
            Function.AlertRefresh("修改成功");
        }
        else
        {
            Function.AlertBack("修改失败");
        }

    }
}
