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

public partial class page_Task_MissionEmail2_Default : System.Web.UI.Page
{
    private const string Dir = "/page/Task/MissionEmail2/";
    protected void Page_Load(object sender, EventArgs e)
    {



        //-------------------------------------------------------------------------------
        FlushMsg(string.Format("我于{0}开始打开执行", DateTime.Now));
        FlushMsg(string.Format("我的具体路径在{0}", System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase));
        List<CallInfo> listcall = GetListCall();
        List<BrandInfo> listbrand = BrandBLL.GetList();
        FlushMsg(string.Format("总共有 {0} 个call需要判断是否需要发警报滴", listcall.Count));

        int FlagOver = 0;
        foreach (CallInfo item in listcall)
        {
            BrandInfo infoBrand = listbrand.Find(b => b.ID == item.BrandID);
            if (null == infoBrand)
            {
                FlushMsg(string.Format("----单号:{0}其品牌为空，数据错误", item.No));
                continue;
            }
            if (item.SLADateEnd == DicInfo.DateZone || string.IsNullOrEmpty(infoBrand.SlaTimerTo))
            {
                FlagOver++;
                continue;
            }

            
            string SendResult = string.Empty;
            int EmailRecordCount = GetEmailRecCount(item);
            if (EmailRecordCount >= 2)//发够两次就不再发了
            {
                continue;
            }
            if (infoBrand.SlaTimer1 > 0 && DateTime.Now.AddMinutes(infoBrand.SlaTimer1) > item.SLADateEnd)
            {
                if (EmailRecordCount == 0)
                {
                    SendResult = SendEmailAndMarkRecord(item, infoBrand);
                    FlushMsg(string.Format("于{0}，单号{1},ID:{2};品牌：{3}发了第一个警报邮件{4}", DateTime.Now, item.No, item.ID, item.BrandName, SendResult));
                    continue;
                }
            }
            if (infoBrand.SlaTimer2 > 0 && DateTime.Now.AddMinutes(infoBrand.SlaTimer2) > item.SLADateEnd)
            {
                if (EmailRecordCount == 1)
                {
                    SendResult = SendEmailAndMarkRecord(item, infoBrand);
                    FlushMsg(string.Format("于{0}，单号{1},ID:{2};品牌：{3}发了第二个警报邮件{4}", DateTime.Now, item.No, item.ID, item.BrandName, SendResult));
                    continue;
                }
            }
            FlushMsg(string.Format("----完成执行单号为{0}；ID为{1}，的call遍历", item.No, item.ID));
        }

        FlushMsg(string.Format("----其中跳过了{0}个call。因其未配置超时项", FlagOver));
    }


    private static string SendEmailAndMarkRecord(CallInfo item, BrandInfo infoBrand)
    {
        Configuration cfg = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(Dir);
        AppSettingsSection appSetting = cfg.AppSettings;


        EmailInfo einfo = new EmailInfo();
        einfo.Attachment = new List<System.Net.Mail.Attachment>();
        einfo.Body = string.Format("{0}的{1}单号:({2})，将于{3}超时", item.BrandName, item.No, item.Details, item.SLADateEnd);
        einfo.CC = new List<System.Net.Mail.MailAddress>();
        einfo.FromEmailAddress = appSetting.Settings["发件箱地址"].Value; ;
        einfo.FromEmailDisplayName = appSetting.Settings["发件箱显示名"].Value;
        einfo.FromEmailHost = appSetting.Settings["发件箱主机信息"].Value;
        einfo.FromEmailPwd = appSetting.Settings["发件箱密码"].Value;
        einfo.FromPort = Function.ConverToInt(appSetting.Settings["发件箱端口"].Value, 25);
        einfo.MailAddress = new List<System.Net.Mail.MailAddress>();
        foreach (string itemTo in infoBrand.SlaTimerTo.Split(';'))
        {
            if (!string.IsNullOrEmpty(itemTo))
            {
                einfo.MailAddress.Add(new System.Net.Mail.MailAddress(itemTo));
            }
        }
        einfo.ReplayTo = new System.Net.Mail.MailAddress(einfo.FromEmailAddress);
        einfo.Subject = "单号{0}|品牌{1}|店铺{2}|将在{3}超时，还剩{4}分钟";
        einfo.Subject = string.Format(einfo.Subject, item.No, item.BrandName, item.StoreName, item.SLADateEnd.ToString("MM-dd HH:mm"), (int)(item.SLADateEnd - DateTime.Now).TotalMinutes);
        string Result = EmailBLL.Email_Send(einfo);
        if (string.IsNullOrEmpty(Result))//发成功了
        {
            SlaWarnRecordInfo warnInfo = SlaWarnRecordBLL.GetByCallID(item.ID);
            if (null == warnInfo)
            {
                warnInfo = new SlaWarnRecordInfo();
                warnInfo.CallID = item.ID;
                warnInfo.Detail = Result;
                warnInfo.WarnTime = 1;
                warnInfo.ID = SlaWarnRecordBLL.Add(warnInfo);
            }
            warnInfo.WarnTime++;
            SlaWarnRecordBLL.Edit(warnInfo);
        }
        return Result;
    }



    private static List<CallInfo> GetListCall()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(" 1=1 and f_StateMain in(");
        sb.Append((int)SysEnum.CallStateMain.未处理);
        sb.Append(",");
        sb.Append((int)SysEnum.CallStateMain.处理中);
        sb.Append(")");

        List<CallInfo> listcall = CallBLL.GetList(sb.ToString());
        return listcall;
    }

    private static int GetEmailRecCount(CallInfo info)
    {

        SlaWarnRecordInfo sinfo = SlaWarnRecordBLL.GetByCallID(info.ID);
        if (null == sinfo)
        {
            return 0;
        }
        return sinfo.WarnTime;
    }


    private void FlushMsg(string input)
    {
        Response.Write("<div>" + input + "<div>");
        Response.Flush();
    }

}