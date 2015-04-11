using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSMP.BLL;
using CSMP.Model;
using Tool;

public partial class page_Profile_TempLate : _Sys_Profile
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            TxbContact.Text = ProfileBLL.GetValue(ProfileInfo.UserKey.邮件模板, true);
        }
    }
    protected void BtnCallRecCount_Click(object sender, EventArgs e)
    {
        ProfileInfo info = ProfileBLL.Get(ProfileInfo.UserKey.邮件模板);
        info.Value = TxbContact.Text.Trim();
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
