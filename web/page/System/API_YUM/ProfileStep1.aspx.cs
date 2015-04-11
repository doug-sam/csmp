using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


using CSMP.Model;
using CSMP.BLL;
using Tool;

public partial class system_API_YUM_ProfileStep1 : _Sys_Profile
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            TxbContact.Text = ProfileBLL.GetValue(ProfileInfo.YUM_API.邮件模板一, true);
        }
    }

    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        ProfileInfo info = ProfileBLL.Get(ProfileInfo.YUM_API.邮件模板一);
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
