using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Net;
using System.Web;
using System.IO;
using System.Data.SqlClient;
using System.Net.Mail;

using Newtonsoft.Json.Linq;
using Tool;
using CSMP.Model;
using CSMP.BLL;
using System.Threading;

namespace CSMPTimerTask
{
    partial class CSMPTimerService : ServiceBase
    {
        //System.Timers.Timer taskTimer;  //计时器
        public CSMPTimerService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            // TODO: 在此处添加代码以启动服务。
            //taskTimer = new System.Timers.Timer();
            //string taskTimeStr = ConfigHelper.GetAppendSettingValue("TaskTime");
            //int taskTime = 3;
            //try {
            //    taskTime = Convert.ToInt32(taskTimeStr);
            //    //Logger.GetLogger(this.GetType()).Info("定时时间为" + taskTime, null);
            //}
            //catch {
            //    taskTime = 3;
            //}
            Logger.GetLogger(this.GetType()).Info("Windows定时调用接口任务服务启动\r\n", null);
            ThreadPool.QueueUserWorkItem(new WaitCallback(DoTimerTask));
            //taskTimer.Interval = taskTime * 60 * 1000;  //设置计时器事件间隔执行时间,毫秒数1s=1000ms
            //taskTimer.Elapsed += new System.Timers.ElapsedEventHandler(taskTimer_Elapsed);
            //taskTimer.Enabled = true;
        }

        protected override void OnStop()
        {
            // TODO: 在此处添加代码以执行停止服务所需的关闭操作。
            //this.taskTimer.Enabled = false;
            Logger.GetLogger(this.GetType()).Info("Windows定时调用接口任务服务停止\r\n", null);
        }
        /// <summary>
        /// 拼装邮件信息
        /// </summary>
        /// <param name="item"></param>
        /// <param name="infoBrand"></param>
        private static void EmailInfo(WebServiceTaskInfo item, string errorMessage)
        {
            EmailInfo einfo = new EmailInfo();
            einfo.Attachment = new List<System.Net.Mail.Attachment>();
            einfo.Body = string.Format("客户:{0}，品牌:{1}，单号:{2}，调用时间:{3} 调用接口出现错误<br/>参数内容:{4}<br/>错误信息:{5}", item.CustomerName, item.BrandName, item.CallNo,DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), item.TaskUrl, errorMessage);
            einfo.CC = new List<System.Net.Mail.MailAddress>();
            einfo.FromEmailAddress = ConfigHelper.GetAppendSettingValue("FromEmailAddress");
            einfo.FromEmailDisplayName = ConfigHelper.GetAppendSettingValue("FromEmailDisplayName");
            einfo.FromEmailHost = ConfigHelper.GetAppendSettingValue("FromEmailHost");
            einfo.FromEmailPwd = ConfigHelper.GetAppendSettingValue("FromEmailPwd");
            einfo.FromPort = Function.ConverToInt(ConfigHelper.GetAppendSettingValue("FromPort"), 25);
            einfo.MailAddress = new List<System.Net.Mail.MailAddress>();
            string mailTo = ConfigHelper.GetAppendSettingValue("BKErrorEmailTo");
            string mailCCto = ConfigHelper.GetAppendSettingValue("BKErrorEmailCCTo");
            foreach (string itemTo in mailTo.Split(';'))
            {
                if (!string.IsNullOrEmpty(itemTo))
                {
                    einfo.MailAddress.Add(new System.Net.Mail.MailAddress(itemTo));
                }
            }
            foreach (string itemCCTo in mailCCto.Split(';'))
            {
                if (!string.IsNullOrEmpty(itemCCTo))
                {
                    einfo.CC.Add(new System.Net.Mail.MailAddress(itemCCTo));
                }
            }
            einfo.ReplayTo = new System.Net.Mail.MailAddress(ConfigHelper.GetAppendSettingValue("ReplayTo"));
            einfo.Subject = "客户:"+item.CustomerName+",品牌:"+item.BrandName+"调用接口错误告警邮件";
            //由于是第一次发送，第三个参数emailToSendID=0
            SendEmail(einfo,item,0);
            
        }
        /// <summary>
        /// 发送邮件，发送失败的将插入到数据库
        /// </summary>
        /// <param name="einfo"></param>
        /// <param name="taskInfo"></param>
        /// <param name="emailToSendID"></param>
        public static void SendEmail(EmailInfo einfo, WebServiceTaskInfo taskInfo,int emailToSendID)
        {
            Encoding Encod = Encoding.GetEncoding(936);
            if (einfo.FromPort <= 0)
            {
                einfo.FromPort = Tool.Function.ConverToInt(ProfileBLL.GetValue(ProfileInfo.UserKey.发件邮箱端口));
                einfo.FromEmailAddress = ProfileBLL.GetValue(ProfileInfo.UserKey.发件邮箱地址);
                einfo.FromEmailPwd = ProfileBLL.GetValue(ProfileInfo.UserKey.发件邮箱密码);
                einfo.FromEmailHost = ProfileBLL.GetValue(ProfileInfo.UserKey.发件邮件主机);
                einfo.FromEmailDisplayName = ProfileBLL.GetValue(ProfileInfo.UserKey.发件人显示名);
            }
            if (null == einfo.ReplayTo)
            {
                einfo.ReplayTo = new MailAddress(ProfileBLL.GetValue(ProfileInfo.UserKey.回复地址), ProfileBLL.GetValue(ProfileInfo.UserKey.回复时显示名), Encod);
            }
            SmtpClient smtp = new SmtpClient();                         //实例化一个SmtpClient
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;           //将smtp的出站方式设为 Network
            smtp.EnableSsl = false;                                     //smtp服务器是否启用SSL加密
            smtp.Host = einfo.FromEmailHost;                            //指定 smtp 服务器地址// "smtp.163.com";   
            smtp.Port = einfo.FromPort;                                 //指定 smtp 服务器的端口，默认是25，如果采用默认端口，可省去
            if (smtp.Port <= 0)
            {
                smtp.Port = 25;
            }
            //如果你的SMTP服务器不需要身份认证，则使用下面的方式，不过，目前基本没有不需要认证的了
            smtp.UseDefaultCredentials = true;//如果需要认证，则用下面的方式
            smtp.Credentials = new NetworkCredential(einfo.FromEmailAddress, einfo.FromEmailPwd);
            MailMessage mm = new MailMessage(); //实例化一个邮件类
            mm.Priority = MailPriority.High; //邮件的优先级，分为 Low, Normal, High，通常用 Normal即可
            mm.From = new MailAddress(einfo.FromEmailAddress, einfo.FromEmailDisplayName, Encod);
            //收件方看到的邮件来源；//第一个参数是发信人邮件地址
            //第二参数是发信人显示的名称
            mm.SubjectEncoding = Encod;
            mm.IsBodyHtml = true;
            mm.BodyEncoding = Encod;
            mm.Subject = einfo.Subject;
            //邮件标题
            mm.Body = einfo.Body;
            #region 收件人，抄送人，附件
            //邮件的接收者，支持群发，多个地址之间用 半角逗号 分开//当然也可以用全地址添加
            if (einfo.MailAddress != null && einfo.MailAddress.Count > 0)
            {
                foreach (MailAddress item in einfo.MailAddress)
                {
                    mm.To.Add(new MailAddress(item.Address, item.DisplayName, Encod));
                }
            }
            else
            {
                if (emailToSendID > 0)
                {
                    Logger.GetLogger("CSMPTimerService.SendEmail").Info("重新发送邮件失败，邮件ID：" + emailToSendID + "，失败原因：收件地址为空" + "，邮件内容：" + einfo.Body + "\r\n", null);
                }
                else {
                    Logger.GetLogger("CSMPTimerService.SendEmail").Info("Windows定时调用接口任务,任务ID：" + taskInfo.ID + "邮件发送失败，原因：收件地址为空。\r\n", null);
                }
                return;
            }

            //抄送
            if (einfo.CC != null && einfo.CC.Count > 0)
            {
                foreach (MailAddress item in einfo.CC)
                {
                    mm.CC.Add(new MailAddress(item.Address, item.DisplayName, Encod));
                }
            }


            if (einfo.Attachment != null && einfo.Attachment.Count > 0)
            {
                foreach (Attachment item in einfo.Attachment)
                {
                    mm.Attachments.Add(item);
                }
            }

            #endregion

            try
            {
                smtp.Send(mm);
                //如果ID大于0说明是发送数据库中的邮件信息，发送成功后需要删除信息
                if (emailToSendID > 0)
                {
                    if (EmailToSendBLL.Delete(emailToSendID))
                    {
                        Logger.GetLogger("CSMPTimerService.SendEmail").Info("重新发送邮件成功,删除数据库中邮件信息成功，邮件ID：" + emailToSendID + "邮件内容：" + einfo.Body + "\r\n", null);
                    }
                    else
                    {
                        Logger.GetLogger("CSMPTimerService.SendEmail").Info("重新发送邮件成功，删除数据库中邮件信息失败，邮件ID：" + emailToSendID + "邮件内容：" + einfo.Body+"\r\n", null);
                    }
                }
                else {
                    Logger.GetLogger("CSMPTimerService.SendEmail").Info("Windows定时调用接口任务,任务ID：" + taskInfo.ID + "邮件发送成功。\r\n", null);
                }
            }
            catch (Exception ex)
            {
                //如果ID等于0说明是第一次发送邮件，发送失败需要将信息写入数据库
                if (emailToSendID == 0)
                {
                    Logger.GetLogger("CSMPTimerService.SendEmail").Info("Windows定时调用接口任务,任务ID：" + taskInfo.ID + "邮件发送失败，原因：" + ex.Message + "\r\n", null);
                    EmailToSend emailToSend = new EmailToSend();

                    emailToSend.CustomerID = taskInfo.CustomerID;
                    emailToSend.CustomerName = taskInfo.CustomerName;
                    emailToSend.BrandID = taskInfo.BrandID;
                    emailToSend.BrandName = taskInfo.BrandName;
                    emailToSend.CallNo = taskInfo.CallNo;
                    emailToSend.Subject = einfo.Subject;
                    string mailAddress = string.Empty;
                    string ccAddress = string.Empty;

                    if (einfo.MailAddress.Count > 0)
                    {
                        for (int i = 0; i < einfo.MailAddress.Count; i++)
                        {
                            if (i == einfo.MailAddress.Count - 1)
                            {
                                mailAddress += einfo.MailAddress[i];
                            }
                            else
                            {
                                mailAddress += einfo.MailAddress[i] + ";";
                            }
                        }
                    }
                    if (einfo.CC.Count > 0)
                    {
                        for (int i = 0; i < einfo.CC.Count; i++)
                        {
                            if (i == einfo.CC.Count - 1)
                            {
                                ccAddress += einfo.CC[i];
                            }
                            else
                            {
                                ccAddress += einfo.CC[i] + ";";
                            }
                        }
                    }
                    emailToSend.MailAddress = mailAddress;
                    emailToSend.CC = ccAddress;
                    emailToSend.ReplayTo = einfo.ReplayTo.ToString();
                    emailToSend.Attachment = string.Empty;
                    emailToSend.FromEmailAddress = einfo.FromEmailAddress;
                    emailToSend.FromEmailDisplayName = einfo.FromEmailDisplayName;
                    emailToSend.FromEmailHost = einfo.FromEmailHost;
                    emailToSend.FromEmailPwd = einfo.FromEmailPwd;
                    emailToSend.FromPort = einfo.FromPort.ToString();
                    emailToSend.Body = einfo.Body;
                    if (EmailToSendBLL.Add(emailToSend) > 0)
                    {
                        Logger.GetLogger("CSMPTimerService.SendEmail").Info("Windows定时调用接口任务,任务ID：" + taskInfo.ID + "邮件发送失败，信息记录到数据库成功。\r\n", null);
                    }
                    else
                    {
                        Logger.GetLogger("CSMPTimerService.SendEmail").Info("Windows定时调用接口任务,任务ID：" + taskInfo.ID + "邮件发送失败，信息记录到数据库失败。\r\n", null);
                    }
                }
                else {
                    Logger.GetLogger("CSMPTimerService.SendEmail").Info("重新发送邮件失败，邮件ID：" + emailToSendID + "邮件内容：" + einfo.Body + "，失败原因：" + ex.Message + "\r\n", null);
                }

                
            }
        }
       

        //private void taskTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        private void DoTimerTask(Object stateInfo)
        {
            string taskTimeStr = ConfigHelper.GetAppendSettingValue("TaskTime");
            int taskTime = 3;
            try
            {
                taskTime = Convert.ToInt32(taskTimeStr);
                //Logger.GetLogger(this.GetType()).Info("定时时间为" + taskTime, null);
            }
            catch
            {
                taskTime = 3;
            }
            //毫秒数
            int taskTimems = taskTime *60 * 1000; //毫秒数
            while (true)
            {
                try
                {
                    //执行SQL语句或其他操作
                    Logger.GetLogger(this.GetType()).Info("Windows定时调用接口任务开始。\r\n", null);
                    List<WebServiceTaskInfo> taskList = new List<WebServiceTaskInfo>();
                    List<EmailToSend> emailToSendList = new List<EmailToSend>();
                    string SQLWhere = string.Empty;
                    taskList = WebServiceTaskBLL.GetList(SQLWhere);
                    Logger.GetLogger(this.GetType()).Info("Windows定时调用接口任务开始。需调用接口个数：" + taskList.Count + "\r\n", null);
                    emailToSendList = EmailToSendBLL.GetList(SQLWhere);
                    Logger.GetLogger(this.GetType()).Info("Windows定时调用接口任务开始。需重发邮件个数：" + emailToSendList.Count + "\r\n", null);
                    #region 先查看有没有需要重复发送邮件的
                    if (emailToSendList.Count > 0)
                    {
                        for (int j = 0; j < emailToSendList.Count; j++)
                        {
                            EmailInfo resendEinfo = new EmailInfo();
                            resendEinfo.Attachment = new List<System.Net.Mail.Attachment>();
                            resendEinfo.Body = emailToSendList[j].Body;
                            resendEinfo.CC = new List<System.Net.Mail.MailAddress>();
                            resendEinfo.FromEmailAddress = emailToSendList[j].FromEmailAddress;
                            resendEinfo.FromEmailDisplayName = emailToSendList[j].FromEmailDisplayName;
                            resendEinfo.FromEmailHost = emailToSendList[j].FromEmailHost;
                            resendEinfo.FromEmailPwd = emailToSendList[j].FromEmailPwd;
                            resendEinfo.FromPort = Function.ConverToInt(emailToSendList[j].FromPort, 25);
                            resendEinfo.MailAddress = new List<System.Net.Mail.MailAddress>();
                            string resendMailTo = emailToSendList[j].MailAddress;
                            string resendMCCto = emailToSendList[j].CC;
                            foreach (string itemTo in resendMailTo.Split(';'))
                            {
                                if (!string.IsNullOrEmpty(itemTo))
                                {
                                    resendEinfo.MailAddress.Add(new System.Net.Mail.MailAddress(itemTo));
                                }
                            }
                            foreach (string itemCCTo in resendMCCto.Split(';'))
                            {
                                if (!string.IsNullOrEmpty(itemCCTo))
                                {
                                    resendEinfo.CC.Add(new System.Net.Mail.MailAddress(itemCCTo));
                                }
                            }
                            resendEinfo.ReplayTo = new System.Net.Mail.MailAddress(emailToSendList[j].ReplayTo);
                            resendEinfo.Subject = emailToSendList[j].Subject;
                            //由于是第一次发送，第三个参数emailToSendID=0
                            SendEmail(resendEinfo, null, emailToSendList[j].ID);
                        }
                    }
                    #endregion
                    //string sqlListStr = "select * from sys_WebServiceTask where f_IsDone = 0 order by id asc;";
                    //using (SqlDataReader rdr = SqlHelper.ExecuteReader(SqlHelper.SqlconnString, CommandType.Text, sqlListStr, null))
                    if (taskList.Count > 0)
                    {
                        taskList = taskList.OrderBy(WebServiceTaskInfo => WebServiceTaskInfo.ID).ToList();
                        for (int i = 0; i < taskList.Count; i++)
                        {
                            int id = taskList[i].ID;
                            string paramStr = taskList[i].TaskUrl;
                            Logger.GetLogger(this.GetType()).Info("Windows定时调用接口任务,任务ID：" + id + "，参数：" + paramStr + "，CustomerName:" + taskList[i].CustomerName + ",BrandName:" + taskList[i].BrandName + "\r\n", null);
                            System.Text.Encoding encode = System.Text.Encoding.GetEncoding("GB2312");
                            string content = HttpUtility.UrlEncode(paramStr, encode);

                            string url = ConfigHelper.GetAppendSettingValue("BKWebServiceURL");
                            //string url = "http://helpdesk.bkchina.cn/siweb/ws_hesheng.ashx?";
                            url = url + "para={" + content + "}";
                            string targeturl = url.Trim().ToString();
                            Logger.GetLogger(this.GetType()).Info("Windows定时调用接口任务，任务ID=" + id + "，URL：" + targeturl + "\r\n", null);
                            HttpWebRequest hr = null;
                            try
                            {
                                //Logger.GetLogger(this.GetType()).Info("Windows定时调用接口任务执行失败，任务ID=" + id + "，httpcreate开始", null);
                                hr = (HttpWebRequest)WebRequest.Create(targeturl);
                                //Logger.GetLogger(this.GetType()).Info("Windows定时调用接口任务执行失败，任务ID=" + id + "，httpcreate成功", null);
                            }
                            catch (Exception ex)
                            {
                                Logger.GetLogger(this.GetType()).Info("Windows定时调用接口任务执行失败，任务ID=" + id + "，错误原因：创建链接失败，异常：" + ex.Message + "\r\n", null);
                                continue;
                            }
                            hr.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1)";
                            hr.Method = "GET";
                            hr.Timeout = 30 * 60 * 1000;
                            StreamReader ser = null;
                            //Logger.GetLogger(this.GetType()).Info("Windows定时调用接口任务执行失败，任务ID=" + id + "，错误原因：开始stream", null);
                            try
                            {

                                WebResponse hs = hr.GetResponse();
                                Stream sr = hs.GetResponseStream();
                                ser = new StreamReader(sr, System.Text.Encoding.UTF8);
                            }
                            catch (Exception ex)
                            {
                                Logger.GetLogger(this.GetType()).Info("Windows定时调用接口任务执行失败，任务ID=" + id + "，错误原因：发送请求响应错误，异常：" + ex.Message + "\r\n", null);
                                if (ex.Message.Contains("远程服务器返回错误: (500)"))
                                {
                                    if (WebServiceTaskBLL.Delete(id))
                                    {
                                        Logger.GetLogger(this.GetType()).Info("Windows定时调用接口任务执行失败，任务ID=" + id + "，错误原因：发送请求响应错误，异常：" + ex.Message + "删除数据库中的信息成功。\r\n", null);
                                    }
                                    else
                                    {
                                        Logger.GetLogger(this.GetType()).Info("Windows定时调用接口任务执行失败，任务ID=" + id + "，错误原因：发送请求响应错误，异常：" + ex.Message + "删除数据库中的信息失败。\r\n", null);
                                    }
                                    EmailInfo(taskList[i], "错误原因：发送请求响应错误，异常：" + ex.Message);
                                }
                                continue;
                            }

                            string sendResult = ser.ReadToEnd();
                            //string sendResult = "{\"status\":true,\"errNo\":101,\"Desc\":\"执行失败，请与管理员联系exec [p_SIWeb_OpenTask] @tNumber='20150821101401245',@tSupply=N'MVSHD',@tAgent=N'admin',@userCode=N'L0098677',@tCaller=N'孙先生',@tTime1='2015-08-21 10:12:00',@tIssue=N'中毒',@tStatus=N'新建',@tPriority='P2',@tPrimary=N'外设支付设备',@tSecondary=N'门店网络问题',@tThird=N'中毒',@iAction=N'新建',@iSolution=N'开案',@iAttachment=N''\"}";
                            Logger.GetLogger(this.GetType()).Info("Windows定时调用接口任务,任务ID：" + id + "，接口返回结果：" + sendResult + "\n\n", null);

                            JObject obj = null;
                            try
                            {
                                obj = JObject.Parse(sendResult);
                            }
                            catch (Exception ex)
                            {
                                Logger.GetLogger(this.GetType()).Info("Windows定时调用接口任务执行失败，任务ID=" + id + "，错误原因：JSON数据解析失败，异常：" + ex.Message + "\n\n", null);
                                if (WebServiceTaskBLL.Delete(id))
                                {
                                    Logger.GetLogger(this.GetType()).Info("Windows定时调用接口任务返回结果错误，任务ID=" + id + ",错误原因：JSON数据解析失败，异常：" + ex.Message + "删除数据库中的信息成功。\n\n", null);
                                }
                                else
                                {
                                    Logger.GetLogger(this.GetType()).Info("Windows定时调用接口任务返回结果错误，任务ID=" + id + ",错误原因：JSON数据解析失败，异常：" + ex.Message + "删除数据库中的信息失败。\n\n", null);
                                }
                                EmailInfo(taskList[i], "返回结果" + sendResult + ",错误原因：JSON数据解析失败，异常：" + ex.Message);
                                continue;
                            }
                            string errNo = string.Empty;
                            int errNoToInt = 0;
                            try
                            {
                                errNo = obj["errNo"].ToString();

                            }
                            catch (Exception ex)
                            {
                                Logger.GetLogger(this.GetType()).Info("Windows定时调用接口任务执行失败，任务ID=" + id + "，错误原因：从JSON数据中获取errNo值失败，异常：" + ex.Message + "\r\n", null);
                                if (WebServiceTaskBLL.Delete(id))
                                {
                                    Logger.GetLogger(this.GetType()).Info("Windows定时调用接口任务返回结果错误，任务ID=" + id + ",错误原因：从JSON数据中获取errNo值失败，异常：" + ex.Message + "删除数据库中的信息成功。\r\n", null);
                                }
                                else
                                {
                                    Logger.GetLogger(this.GetType()).Info("Windows定时调用接口任务返回结果错误，任务ID=" + id + ",错误原因：从JSON数据中获取errNo值失败，异常：" + ex.Message + "删除数据库中的信息失败。\r\n", null);
                                }
                                EmailInfo(taskList[i], "返回结果" + sendResult + ",错误原因：从JSON数据中获取errNo值失败，异常：" + ex.Message);
                                continue;
                            }
                            try
                            {
                                errNoToInt = Convert.ToInt32(errNo);
                            }
                            catch (Exception ex)
                            {
                                Logger.GetLogger(this.GetType()).Info("Windows定时调用接口任务执行失败，任务ID=" + id + "，错误原因：errNo转换为Int类型失败，异常" + ex.Message + "\r\n", null);
                                if (WebServiceTaskBLL.Delete(id))
                                {
                                    Logger.GetLogger(this.GetType()).Info("Windows定时调用接口任务返回结果错误，任务ID=" + id + ",错误原因：errNo转换为Int类型失败，异常" + ex.Message + "删除数据库中的信息成功。\r\n", null);
                                }
                                else
                                {
                                    Logger.GetLogger(this.GetType()).Info("Windows定时调用接口任务返回结果错误，任务ID=" + id + ",错误原因：errNo转换为Int类型失败，异常" + ex.Message + "删除数据库中的信息失败。\r\n", null);
                                }
                                EmailInfo(taskList[i], "返回结果" + sendResult + ",错误原因：errNo转换为Int类型失败，异常" + ex.Message);
                                continue;
                            }

                            if (errNoToInt == 0)
                            {
                                if (WebServiceTaskBLL.Delete(id))
                                {
                                    Logger.GetLogger(this.GetType()).Info("Windows定时调用接口任务执行成功，任务ID=" + id + "，删除数据库记录成功\r\n", null);
                                }
                                else
                                {
                                    Logger.GetLogger(this.GetType()).Info("Windows定时调用接口任务执行成功，任务ID=" + id + "，删除数据库记录失败\r\n", null);
                                }
                            }
                            else if (errNoToInt > 0)
                            {
                                Logger.GetLogger(this.GetType()).Info("Windows定时调用接口任务返回结果错误，任务ID=" + id + ",错误原因：errNo大于0。\r\n", null);
                                if (WebServiceTaskBLL.Delete(id))
                                {
                                    Logger.GetLogger(this.GetType()).Info("Windows定时调用接口任务返回结果错误，任务ID=" + id + ",错误原因：errNo大于0，删除数据库信息成功。\r\n", null);
                                }
                                else
                                {
                                    Logger.GetLogger(this.GetType()).Info("Windows定时调用接口任务返回结果错误，任务ID=" + id + ",错误原因：errNo大于0,删除数据库中的信息失败。\r\n", null);
                                }
                                EmailInfo(taskList[i], "返回结果" + sendResult + ",错误原因：errNo大于0");
                            }
                            else if (errNoToInt < 0)
                            {
                                Logger.GetLogger(this.GetType()).Info("Windows定时调用接口任务返回结果错误，任务ID=" + id + ",错误原因：errNo小于0。\r\n", null);
                                if (WebServiceTaskBLL.Delete(id))
                                {
                                    Logger.GetLogger(this.GetType()).Info("Windows定时调用接口任务返回结果错误，任务ID=" + id + ",错误原因：errNo小于0，删除数据库信息成功。\r\n", null);
                                }
                                else
                                {
                                    Logger.GetLogger(this.GetType()).Info("Windows定时调用接口任务返回结果错误，任务ID=" + id + ",错误原因：errNo小于0,删除数据库中的信息失败。\r\n", null);
                                }
                                EmailInfo(taskList[i], "返回结果" + sendResult + ",错误原因：errNo小于0");

                            }
                        }
                        //string paramStr = "\"Action\":\"新建\",\"cNumber\":\"20150210325995\",\"Supplier\":\"MVS\",\"Agent\":\"Simon\",\"stCode\":\"16594\",\"stMgr\":\"王先生\",\"Time1\":\"2015-7-1 15:23:33\",\"Issue\":\"无法打开电脑\",\"Priority\":\"P2\",\"Category1\":\"经理室硬件\",\"Category2\":\"PC机\",\"Category3\":\"显示屏故障\",\"Solution\":\"\",\"Attachment\":\"\"";
                    }

                }
                finally {
                    Logger.GetLogger(this.GetType()).Info("Windows定时调用接口任务执行完成，休眠" + taskTime + "分钟。\r\n", null);
                    Thread.Sleep(taskTimems);                    
                }
            }
            
        }
    }
}
