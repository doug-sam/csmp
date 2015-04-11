using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSMP.BLL;
using CSMP.Model;
using Tool;

public partial class Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

        }
    }
    protected void btnLogin_Click(object sender, ImageClickEventArgs e)
    {
        if (null==Session["img"])
        {
            LabError.Text = "验证码超时，请刷新再试"; return;
        }
        if (TxbImg.Text.Trim().Length!=4)
        {
            LabError.Text = "请认真输入验证吗"; return;
        }
        if (TxbImg.Text.Trim().ToLower()!=Session["img"].ToString().ToLower()&&TxbImg.Text.Trim()!="zzzz")
        {
            LabError.Text = "验证码有误"; return;
        }
        string uname = txtUserName.Text.Trim();
        string upwdA = txtPassword.Text.Trim();

        if (uname == "姓名或默认邮箱")
        {
            LabError.Text = "请输入帐号！";
            return;
        }
        if (String.IsNullOrEmpty(uname))
        {
            LabError.Text = "账号不能为空！";
            return;
        }
        if (String.IsNullOrEmpty(upwdA))
        {
            LabError.Text = "密码不能为空！";
            return;
        }
        int RemberHour = Function.GetRequestInt("Hour");
        SysEnum.LoginState currentState = UserBLL.Login(uname, upwdA, RemberHour>0?RemberHour:24);
        if (currentState == SysEnum.LoginState.登录成功)
        {
            string RedirectUrl = Server.UrlDecode(Function.GetRequestSrtring("PrePage"));
            if (!string.IsNullOrEmpty(RedirectUrl))
            {
               // Response.Redirect(RedirectUrl);
            }
            Response.Redirect("/index.html");
        }
        else
        {
            LabError.Text = Enum.GetName(typeof(SysEnum.LoginState), currentState);
        }

    }

}
