using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


using CSMP.Model;
using CSMP.BLL;
using Tool;

public partial class system_API_YUM_ProfileMain : _Sys_Profile
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CbEnable.Checked = ProfileBLL.GetValue(ProfileInfo.YUM_API.总开关, true).ToLower() == "true";
            TxbID.Text = ProfileBLL.GetValue(ProfileInfo.YUM_API.YUM客户ID, true);
        }
    }
    protected void BtnID_Click(object sender, EventArgs e)
    {
        if (Function.ConverToInt(TxbID.Text.Trim())<=0)
        {
            Function.AlertBack("请认真填写YUM客户ID");
        }
        ProfileInfo YUM_ID = ProfileBLL.Get(ProfileInfo.YUM_API.YUM客户ID);
        YUM_ID.Value = TxbID.Text.Trim();
        if (ProfileBLL.Edit(YUM_ID))
        {
            Function.AlertRefresh("提交成功");
            return;
        }
        else
        {
            Function.AlertBack("失败");
            return;
        }

    }
    protected void BtnEnable_Click(object sender, EventArgs e)
    {
        ProfileInfo YUM_Enable = ProfileBLL.Get(ProfileInfo.YUM_API.总开关);
        YUM_Enable.Value = CbEnable.Checked.ToString();
        if (ProfileBLL.Edit(YUM_Enable))
        {
            Function.AlertRefresh("提交成功");
            return;
        }
        else
        {
            Function.AlertBack("失败");
            return;
        }
    }
}
