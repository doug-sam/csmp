using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSMP.BLL;
using CSMP.Model;
using Tool;

public partial class page_User_Pwd : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        if (TxbNew1.Text.Trim()!=TxbNew2.Text.Trim())
        {
            Function.AlertMsg("新密码两次输入不匹配"); return;
        }
        if (TxbNew1.Text.Trim().Length<5||TxbNew1.Text.Trim().Length>50)
        {
            Function.AlertMsg("密码应在6到50个字符间"); return;
        }
        if (Function.ConverToInt(TxbNew1.Text.Trim())>0)
        {
            Function.AlertMsg("密码不可以是纯数字"); return;
        }
        if (CurrentUser==null)
        {
            Function.AlertMsg("登录信息丢失，请重新登录"); return;
        }
        if (Md5Helper.Md5(TxbOld.Text.Trim())!=CurrentUser.PassWord)
        {
            Function.AlertMsg("原始密码有误"); return;
        }
        CurrentUser.PassWord = Md5Helper.Md5(TxbNew1.Text.Trim());
        if (UserBLL.Edit(CurrentUser))
        {
            Function.AlertMsg("密码修改成功。下次登录将使用新密码"); return;
        }
        else
        {
            Function.AlertMsg("修改失败，请重试或联系管理员");
        }
    }
}
