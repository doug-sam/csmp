using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


using CSMP.Model;
using CSMP.BLL;
using Tool;
using System.Net.Mail;

public partial class system_SendEmail : _Sys_Profile
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            

        }
    }
    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        LabError.Text = string.Empty;
        
        EmailInfo einfo = new EmailInfo();
        einfo.Attachment = null;
        einfo.Body = TxbContent.Text.Trim();
        
        einfo.Subject = TxbSubject.Text.Trim();
        einfo.MailAddress = new List<MailAddress>();
        MailAddress maddress = new MailAddress(TxbToAddress.Text.Trim(),"显示名");
        einfo.MailAddress.Add(maddress);
        string result = EmailBLL.Email_Send(einfo);
        if (string.IsNullOrEmpty(result))
        {
            Function.AlertRefresh("邮件发送成功");
        }
        else
        {
            LabError.Text = "发送失败！！！！<br/>具体信息如下<br/>"+result;
        }

        

    }
}
