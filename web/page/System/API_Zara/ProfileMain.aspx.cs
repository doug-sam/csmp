using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


using CSMP.Model;
using CSMP.BLL;
using Tool;

public partial class system_API_Zara_ProfileMain : _Sys_Profile
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CbEnable.Checked = ProfileBLL.GetValue(ProfileInfo.API_ZARA.总开关, true).ToLower() == "true";
            TxbID.Text = ProfileBLL.GetValue(ProfileInfo.API_ZARA.ZARA客户ID, true);
        }
    }
    protected void BtnID_Click(object sender, EventArgs e)
    {
        ProfileInfo _ID = ProfileBLL.Get(ProfileInfo.API_ZARA.ZARA客户ID);
        _ID.Value = TxbID.Text.Trim('|').Replace(" ","");
        if (ProfileBLL.Edit(_ID))
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
        ProfileInfo YUM_Enable = ProfileBLL.Get(ProfileInfo.API_ZARA.总开关);
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
