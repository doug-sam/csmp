using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSMP.BLL;
using CSMP.Model;
using Tool;

public partial class page_Task_MissionEmail1_Default : System.Web.UI.Page
{
    private const string Dir = "/page/Task/MissionEmail1/";
    protected void Page_Load(object sender, EventArgs e)
    {
        
        try
        {

            Configuration cfg = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(Dir);
            AppSettingsSection appSetting = cfg.AppSettings;

            EmailInfo info = new EmailInfo();
            info.Attachment = new List<System.Net.Mail.Attachment>();
            info.Body = DocHelper.Read(Dir + "Body.txt", true);
            info.CC = new List<System.Net.Mail.MailAddress>();
            foreach (string item in appSetting.Settings["抄送人列表"].Value.Split(';'))
            {
                if (string.IsNullOrEmpty(item)) continue;
                info.CC.Add(new System.Net.Mail.MailAddress(item));
            }
            info.FromEmailAddress = appSetting.Settings["发件箱地址"].Value;
            info.FromEmailDisplayName = appSetting.Settings["发件箱显示名"].Value;
            info.FromEmailHost = appSetting.Settings["发件箱主机信息"].Value;
            info.FromEmailPwd = appSetting.Settings["发件箱密码"].Value;
            info.FromPort = Function.ConverToInt(appSetting.Settings["发件箱端口"].Value, 25);
            info.MailAddress = new List<System.Net.Mail.MailAddress>();
            foreach (string item in appSetting.Settings["收件人列表"].Value.Split(';'))
            {
                if (string.IsNullOrEmpty(item)) continue;
                info.MailAddress.Add(new System.Net.Mail.MailAddress(item));
            }
            info.ReplayTo = new System.Net.Mail.MailAddress(info.FromEmailAddress);
            info.Subject = DocHelper.Read(Dir + "Subject.txt", true);

            info.Subject = info.Subject.Replace("(((((Date)))))", DateTime.Now.ToString("yyyyMMdd"));

            StringBuilder sbBody = GetTableRow(appSetting);
            info.Body = info.Body.Replace("(((((中间的行集合)))))", sbBody.ToString());

          string Result=  EmailBLL.Email_Send(info);
          if (string.IsNullOrEmpty(Result))
          {
              Response.Write("success");
              return;
          }
          Response.Write("failed,"+Result);
        }
        catch (Exception ex)
        {
            Response.Write("系统任务在执行MissionEmail1时出系统级别错误，具体错误为：<br/>"+ex.Message);
        }
    }

    private StringBuilder GetTableRow(AppSettingsSection appSetting)
    {
        StringBuilder sbBody = new StringBuilder();
        List<CallInfo> list = CallBLL.GetList(" 1=1 and DATEDIFF(day,f_ErrorDate,GETDATE())=0 and f_BrandID in(" + appSetting.Settings["触发邮件品牌ID列表"].Value.Trim(',') + ")");
        foreach (CallInfo item in list)
        {
            sbBody.Append("<tr>");

            sbBody.Append(GetTdBegin());
            sbBody.Append(item.CreateDate);//创建时间
            sbBody.Append(GetTdEnd());

            sbBody.Append(GetTdBegin());
            sbBody.Append(item.ErrorReportUser);//报修人
            sbBody.Append(GetTdEnd());

            sbBody.Append(GetTdBegin());
            sbBody.Append(item.CustomerName);//客户
            sbBody.Append(GetTdEnd());

            sbBody.Append(GetTdBegin());
            sbBody.Append(item.BrandName);//品牌
            sbBody.Append(GetTdEnd());

            sbBody.Append(GetTdBegin());
            sbBody.Append(item.ProvinceName);
            sbBody.Append(GetTdEnd());

            sbBody.Append(GetTdBegin());
            sbBody.Append(item.CityName);
            sbBody.Append(GetTdEnd());

            sbBody.Append(GetTdBegin());
            sbBody.Append(item.StoreName);
            sbBody.Append(GetTdEnd());

            sbBody.Append(GetTdBegin());
            sbBody.Append(item.StoreNo);
            sbBody.Append(GetTdEnd());

            sbBody.Append(GetTdBegin());
            sbBody.Append(item.ReportSourceName);//报修源
            sbBody.Append(GetTdEnd());

            sbBody.Append(GetTdBegin());
            sbBody.Append(item.ClassName1);
            sbBody.Append(GetTdEnd());

            sbBody.Append(GetTdBegin());
            sbBody.Append(item.ClassName2);
            sbBody.Append(GetTdEnd());

            sbBody.Append(GetTdBegin());
            sbBody.Append(item.ClassName3);
            sbBody.Append(GetTdEnd());

            sbBody.Append(GetTdBegin());
            sbBody.Append(item.Details);
            sbBody.Append(GetTdEnd());

            sbBody.Append(GetTdBegin());
            sbBody.Append(item.MaintaimUserName);//二线负责人
            sbBody.Append(GetTdEnd());

            sbBody.Append(GetTdBegin());
            sbBody.Append(Enum.Parse(typeof(SysEnum.CallStateMain), item.StateMain.ToString()).ToString());
            sbBody.Append(GetTdEnd());

            sbBody.Append(GetTdBegin());
            sbBody.Append(item.SloveBy);//二线最终的解决描述
            sbBody.Append(GetTdEnd());

            sbBody.Append("</tr>");
        }
        return sbBody;
    }

    private string GetTdBegin()
    {
        return "    <td class=\"td1_2\">";
    }
    private string GetTdEnd()
    {
        return "    </td>";
    }

}