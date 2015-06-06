using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using CSMP.BLL;
using CSMP.Model;
using Tool;
using System.Threading;

namespace CSMPTask
{


    public partial class Form1 : Form
    {
        private delegate void ProxyClient();//代理
        public Form1()
        {
            InitializeComponent();
            //this.Visible = false;
            //this.Hide();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            TxbSetValueLine("");
            TxbSetValueLine(string.Format("我于{0}开始打开执行", DateTime.Now));
            TxbSetValueLine(string.Format("我的具体路径在{0}", System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase));
            List<CallInfo> listcall = GetListCall();
            List<BrandInfo> listbrand = BrandBLL.GetList();
            TxbSetValueLine(string.Format("总共有 {0} 个call需要判断是否需要发警报滴", listcall.Count));

            LogInfo info = new LogInfo();
            info.AddDate = DateTime.Now;
            info.Category = Enum.GetName(typeof(SysEnum.LogType), SysEnum.LogType.系统任务执行监视);
            info.Content = "CSMPTask执行" + string.Format("{0}总共有 {1} 个call需要判断是否需要发警报滴", DateTime.Now, listcall.Count);
            info.ErrorDate = DateTime.Now;
            info.SendEmail = false;
            info.UserName = DicInfo.Admin;
            info.Serious = 1;
            LogBLL.Add(info);

            foreach (CallInfo item in listcall)
            {
                if (item.SLADateEnd == DicInfo.DateZone)
                {
                    continue;
                }
                List<BrandInfo> listBrandtemp = listbrand.Where(b => b.ID == item.BrandID).ToList();
                if (listBrandtemp.Count == 0)
                {
                    continue;
                }
                BrandInfo infoBrand = listBrandtemp[0];
                if (string.IsNullOrEmpty(infoBrand.SlaTimerTo))
                {
                    continue;
                }
                int EmailRecordCount = GetEmailRecCount(item);
                if (infoBrand.SlaTimer1 > 0 && DateTime.Now.AddMinutes(infoBrand.SlaTimer1) > item.SLADateEnd)
                {
                    if (EmailRecordCount == 0)
                    {
                        SendEmail(item, infoBrand);
                        TxbSetValueLine(string.Format("于{0}，单号{1},ID:{2}发了第一个警报邮件",DateTime.Now,item.No,item.ID ));
                    }
                }
                if (infoBrand.SlaTimer2 > 0 && DateTime.Now.AddMinutes(infoBrand.SlaTimer2) > item.SLADateEnd)
                {
                    if (EmailRecordCount == 1)
                    {
                        SendEmail(item, infoBrand);
                        TxbSetValueLine(string.Format("于{0}，单号{1},ID:{2}发了第二个警报邮件", DateTime.Now, item.No, item.ID));
                    }
                }
                TxbSetValueLine(string.Format("----完成执行单号为{0}；ID为{1}，的call遍历",  item.No, item.ID));
            }
            
            Application.ExitThread();
            System.Environment.Exit(0);
            Application.Exit();
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
            string strSQLRecord = string.Format(" 1=1 and f_UserID=1 and f_CallID={0}  ", info.ID);
            return EmailRecordBLL.GetList(strSQLRecord).Count;
        }

        private static void SendEmail(CallInfo item, BrandInfo infoBrand)
        {
            EmailInfo einfo = new EmailInfo();
            einfo.Attachment = new List<System.Net.Mail.Attachment>();
            einfo.Body = string.Format("{0}的{1}单号:({2})，将于{3}超时", item.BrandName, item.No, item.Details, item.SLADateEnd);
            einfo.CC = new List<System.Net.Mail.MailAddress>();
            einfo.FromEmailAddress = ConfigHelper.GetAppendSettingValue("FromEmailAddress");
            einfo.FromEmailDisplayName = ConfigHelper.GetAppendSettingValue("FromEmailDisplayName");
            einfo.FromEmailHost = ConfigHelper.GetAppendSettingValue("FromEmailHost");
            einfo.FromEmailPwd = ConfigHelper.GetAppendSettingValue("FromEmailPwd");
            einfo.FromPort = Function.ConverToInt(ConfigHelper.GetAppendSettingValue("FromPort"), 25);
            einfo.MailAddress = new List<System.Net.Mail.MailAddress>();
            foreach (string itemTo in infoBrand.SlaTimerTo.Split(';'))
            {
                if (!string.IsNullOrEmpty(itemTo))
                {
                    einfo.MailAddress.Add(new System.Net.Mail.MailAddress(itemTo));
                }
            }
            einfo.ReplayTo = new System.Net.Mail.MailAddress(ConfigHelper.GetAppendSettingValue("ReplayTo"));
            einfo.Subject = "系统警报";
            string Result = EmailBLL.Email_Send(einfo);
            if (string.IsNullOrEmpty(Result))//发成功了
            {
                EmailRecordInfo recInfo = new EmailRecordInfo();
                recInfo.CallID = item.ID;
                recInfo.CallNo = item.No;
                recInfo.DateAdd = DateTime.Now;
                recInfo.ToUser = infoBrand.SlaTimerTo;
                recInfo.UserID = 1;
                recInfo.UserName = "admin";
                EmailRecordBLL.Add(recInfo);


            }
        }


        private delegate void setTextDelegate(String str,string IsAppend);

        /// <summary>
        /// 我的出现。只不过为了实现setTesxtDelegate
        /// </summary>
        /// <param name="value"></param>
        private void setText(String value,string IsAppend)
        {
            if (IsAppend=="1")
            {
                tb.Text =tb.Text+ value;
            }
            else
            {
                tb.Text = value;
            }
        }

        public void TxbAppendValue(string value)
        {
            if (tb.InvokeRequired)
            {
                setTextDelegate mydelegate = new setTextDelegate(setText);
                tb.Invoke(mydelegate, new String[] { value,"1" });
            }
            else
            {
                tb.Text = value;
            }
        }

        public void TxbSetValueLine(string value)
        {
            tb.Text = tb.Text + value + "\r\n";
            //TxbAppendValue(value + "\n");
        }
    }


}
