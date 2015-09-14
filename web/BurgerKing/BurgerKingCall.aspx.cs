using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSMP.BLL;
using CSMP.Model;
using Tool;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Net.Mail;
using System.Net;
using System.Configuration;

public partial class BurgerKing_BurgerKingCall : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string result = Request["param"];
        result = HttpUtility.UrlDecode(result);
        if (string.IsNullOrEmpty(result))
        {
            result = "{\"status\":true,\"errNo\":108,\"Desc\":\"URL中不存在参数param\"}";
            Logger.GetLogger(this.GetType()).Info("BK调用汉堡王接口，错误原因为" + result, null);
            EmailInfo("null", result);
        }
        else {
            Logger.GetLogger(this.GetType()).Info("BK调用汉堡王接口，传入参数为" + result, null);
            result = DoComplete(result);
        }
        Context.Response.ContentType = "application/json";
        Context.Response.Write(result);
    }

    protected string DoComplete(string JSONInfo) 
    {
        string result = string.Empty;
        //if (result.Contains("cNumber") && result.Contains("solutionDetails") && result.Contains("TSI"))
        if (JSONInfo.Contains("cNumber") && JSONInfo.Contains("solutionDetails"))
        {
            JObject obj = null;
            try
            {
                obj = JObject.Parse(JSONInfo);
            }
            catch (Exception ex)
            {
                result = "{\"status\":true,\"errNo\":101,\"Desc\":\"JSON格式不正确\"}";
                Logger.GetLogger(this.GetType()).Info("BK调用汉堡王接口，第一次解析JSON数据错误，原因为" + result+"异常："+ex.Message, null);
                if (JSONInfo.Contains("\\"))
                {
                    JSONInfo = JSONInfo.Replace("\\","\\\\");
                    Logger.GetLogger(this.GetType()).Info("BK调用汉堡王接口，替换JSON数据中单斜杠为双斜杠后的结果为：" + JSONInfo, null);
                    try
                    {
                        obj = JObject.Parse(JSONInfo);
                    }
                    catch (Exception exTow)
                    {
                        result = "{\"status\":true,\"errNo\":101,\"Desc\":\"JSON格式不正确\"}";
                        Logger.GetLogger(this.GetType()).Info("BK调用汉堡王接口，将其中的单斜杠替换为双斜杠后第二次解析JSON数据错误，原因为" + result + "异常：" + exTow.Message, null);
                        //发送错误邮件
                        EmailInfo(JSONInfo+"备注：CSMP已将其中的单斜杠替换为双斜杠。", result);
                        return result;
                    }
                }
                else {
                    //发送错误邮件
                    EmailInfo(JSONInfo, result);
                    return result;
                }
                
            }
            string cNumber = string.Empty;
            try 
            {
                cNumber = obj["cNumber"].ToString();
            }
            catch (Exception ex) 
            {
                result = "{\"status\":true,\"errNo\":107,\"Desc\":\"JSON属性cNumber错误\"}";
                Logger.GetLogger(this.GetType()).Info("BK调用汉堡王接口，错误原因为" + result + "异常：" + ex.Message, null);
                //发送错误邮件
                EmailInfo(JSONInfo, result);
                return result;
            }
            cNumber = cNumber.Trim('"');
            string solutionDetails = string.Empty;
            try
            {
                solutionDetails = obj["solutionDetails"].ToString();
            }
            catch (Exception ex)
            {
                result = "{\"status\":true,\"errNo\":107,\"Desc\":\"JSON属性solutionDetails错误\"}";
                Logger.GetLogger(this.GetType()).Info("BK调用汉堡王接口，错误原因为" + result + "异常：" + ex.Message, null);
                //发送错误邮件
                EmailInfo(JSONInfo, result);
                return result;
            }
            

            string TSI = string.Empty;
            if (JSONInfo.Contains("TSI"))
            { 
                TSI = obj["TSI"].ToString();
                TSI = TSI.Trim('"');
                if (!string.IsNullOrEmpty(TSI))
                {
                    string tpWhere = "f_Name like '%&$&" + TSI.Trim() + "%'";
                    List<ThirdPartyInfo> tpList = ThirdPartyBLL.GetList(tpWhere);
                    if (tpList.Count > 0)
                    {
                        TSI = tpList[0].Name;
                    }
                }

            }

            if (string.IsNullOrEmpty(cNumber))
            {
                result = "{\"status\":true,\"errNo\":103,\"Desc\":\"执行失败，cNumber值不能为空\"}";
                Logger.GetLogger(this.GetType()).Info("BK调用汉堡王接口，错误原因为" + result, null);
                //发送错误邮件
                EmailInfo(JSONInfo, result);
                return result;
            }

            if (solutionDetails.Substring(0, 1) == "\"")
            {
                solutionDetails = solutionDetails.Remove(0, 1);
            }
            if (solutionDetails.Substring(solutionDetails.Length - 1, 1) == "\"")
            {
                solutionDetails = solutionDetails.Remove(solutionDetails.Length - 1, 1);
            }

            if (solutionDetails.Length > 500)
            {
                result = "{\"status\":true,\"errNo\":104,\"Desc\":\"执行失败，solutionDetails处理过程备注不能超过500字\"}";
                Logger.GetLogger(this.GetType()).Info("BK调用汉堡王接口，错误原因为" + result, null);
                //发送错误邮件
                EmailInfo(JSONInfo, result);
                return result;
            }
            

            CallInfo cinfo = CallBLL.Get(cNumber);
            if (cinfo == null)
            {
                result = "{\"status\":true,\"errNo\":105,\"Desc\":\"执行失败，单号：" + cNumber + "不存在\"}";
                Logger.GetLogger(this.GetType()).Info("BK调用汉堡王接口，错误原因为" + result, null);
                //发送错误邮件
                EmailInfo(JSONInfo, result);
                return result;
            }
            else
            {
                CallStepInfo sinfo = new CallStepInfo();

                //如果能获取到第三方预约的TSI信息，则在操作备注中显示该预约信息，如果没有再考虑在操作备注中显示BK传来的TSI信息
                List<CallStepInfo> sinfoList = new List<CallStepInfo>();
                sinfoList = CallStepBLL.GetList(cinfo.ID, SysEnum.StepType.第三方预约上门);
                if (sinfoList.Count > 0)
                {
                    string stepDetails = sinfoList[0].Details;
                    if (stepDetails.Contains("。第三方信息："))
                    {
                        stepDetails = stepDetails.Substring(stepDetails.IndexOf("。第三方信息："));
                        TSI = stepDetails;
                    }
                }
                sinfo.StepType = (int)SysEnum.StepType.第三方上门备注;
                sinfo.MajorUserID = 0;
                sinfo.MajorUserName = "汉堡王";
                sinfo.AddDate = DateTime.Now;
                sinfo.CallID = cinfo.ID;
                sinfo.DateBegin = DateTime.Now;
                sinfo.DateEnd = sinfo.DateBegin;
                sinfo.Details = solutionDetails+TSI;
                sinfo.StepIndex = CallStepBLL.GetMaxStepIndex(cinfo.ID) + 1;
                sinfo.IsSolved = false;
                sinfo.SolutionID = 0;
                sinfo.SolutionName = "";

                sinfo.StepName = SysEnum.StepType.第三方上门备注.ToString();
                sinfo.UserName = "汉堡王";
           
                //sinfo.StepType = (int)SysEnum.StepType.升级到客户;
                ////sinfo.MajorUserID = CurrentUserID;
                //sinfo.MajorUserName = "汉堡王推送";
                //sinfo.AddDate = DateTime.Now;
                //sinfo.CallID = cinfo.ID;
                //sinfo.DateBegin = DateTime.Now;
                //sinfo.DateEnd = sinfo.DateBegin;
                //sinfo.Details = solutionDetails;
                //sinfo.StepIndex = CallStepBLL.GetMaxStepIndex(cinfo.ID) + 1;
                //sinfo.IsSolved = true;
                //sinfo.SolutionID = -1;
                //sinfo.SolutionName = "客户自行解决";
                //cinfo.StateMain = (int)SysEnum.CallStateMain.已完成;
                //cinfo.StateDetail = (int)SysEnum.CallStateDetails.处理完成;
                //cinfo.SloveBy = SysEnum.SolvedBy.客户解决.ToString();
                //cinfo.FinishDate = sinfo.DateBegin;
                //sinfo.StepName = SysEnum.CallStateDetails.升级到客户.ToString();
                ////sinfo.UserID = CurrentUser.ID;
                //sinfo.UserName = "汉堡王推送";

                if (CallStepBLL.Add(sinfo)>0)
                {
                    result = "{\"status\":true,\"errNo\":0,\"Desc\":\"执行成功\"}";
                    Logger.GetLogger(this.GetType()).Info("BK调用汉堡王接口，执行成功", null);
                }
                else
                {
                    result = "{\"status\":true,\"errNo\":106,\"Desc\":\"执行失败,提交失败。请联系管理员\"}";
                    Logger.GetLogger(this.GetType()).Info("BK调用汉堡王接口，错误原因为" + result, null);
                    //发送错误邮件
                    EmailInfo(JSONInfo, result);
                    
                }
                return result;
            }
        }
        else
        {
            result = "{\"status\":true,\"errNo\":102,\"Desc\":\"执行失败，无效的url参数\"}";
            Logger.GetLogger(this.GetType()).Info("BK调用汉堡王接口，错误原因为" + result, null);
            //发送错误邮件
            EmailInfo(JSONInfo, result);
            return result;
        }
    
    }


    /// <summary>
    /// 拼装邮件信息
    /// </summary>
    /// <param name="item"></param>
    /// <param name="infoBrand"></param>
    private static void EmailInfo(string JSONInfo, string errorMessage)
    {
        EmailInfo einfo = new EmailInfo();
        einfo.Attachment = new List<System.Net.Mail.Attachment>();
        einfo.Body = string.Format("BK调用汉堡王接口错误，调用时间{0}<br/>参数内容:{1}<br/>错误信息:{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), JSONInfo, errorMessage);
        einfo.CC = new List<System.Net.Mail.MailAddress>();
        einfo.FromEmailAddress = ConfigurationManager.AppSettings["FromEmailAddress"].ToString();
        
        einfo.FromEmailDisplayName = ConfigurationManager.AppSettings["FromEmailDisplayName"].ToString();
        einfo.FromEmailHost =ConfigurationManager.AppSettings["FromEmailHost"].ToString() ;
        einfo.FromEmailPwd =ConfigurationManager.AppSettings["FromEmailPwd"].ToString();
        einfo.FromPort = Function.ConverToInt(ConfigurationManager.AppSettings["FromPort"].ToString(), 25);
        einfo.MailAddress = new List<System.Net.Mail.MailAddress>();
        string mailTo = ConfigurationManager.AppSettings["BKErrorEmailTo"].ToString();
        string mailCCto = ConfigurationManager.AppSettings["BKErrorEmailCCTo"].ToString();
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
        einfo.ReplayTo = new System.Net.Mail.MailAddress(ConfigurationManager.AppSettings["ReplayTo"].ToString());
        einfo.Subject = "BK调用汉堡王接口错误告警邮件";
        //由于是第一次发送，第三个参数emailToSendID=0
        SendEmail(einfo,JSONInfo,errorMessage);

    }
    /// <summary>
    /// 发送邮件，发送失败的将插入到数据库
    /// </summary>
    /// <param name="einfo"></param>
    /// <param name="taskInfo"></param>
    /// <param name="emailToSendID"></param>
    public static void SendEmail(EmailInfo einfo,string JSONInfo, string errorMessage)
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
            Logger.GetLogger("BurgerKing_BurgerKingCall.SendEmail").Info("BK调用汉堡王接口错误告警邮件发送失败，邮件发送错误原因：收件地址为空。接口收到的参数内容：" + JSONInfo+"，接口调用错误原因："+errorMessage, null);
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
            Logger.GetLogger("BurgerKing_BurgerKingCall.SendEmail").Info("BK调用汉堡王接口错误告警邮件发送成功。接口收到的参数内容：" + JSONInfo + "，接口调用错误原因：" + errorMessage, null);

        }
        catch (Exception ex)
        {

            Logger.GetLogger("BurgerKing_BurgerKingCall.SendEmail").Info("BK调用汉堡王接口错误告警邮件发送失败,邮件发送失败原因：" + ex.Message + "接口收到的参数内容：" + JSONInfo + "，接口调用错误原因：" + errorMessage, null);
            EmailToSend emailToSend = new EmailToSend();

            emailToSend.CustomerID = 0;
            emailToSend.CustomerName = "BK调用汉堡王接口错误";
            emailToSend.BrandID = 0;
            emailToSend.BrandName = "BK调用汉堡王接口错误";
            emailToSend.CallNo = "";
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
                Logger.GetLogger("BurgerKing_BurgerKingCall.SendEmail").Info("BK调用汉堡王接口错误告警邮件发送失败，邮件信息记录到数据库成功。" + "接口收到的参数内容：" + JSONInfo + "，接口调用错误原因：" + errorMessage, null);
            }
            else
            {
                Logger.GetLogger("BurgerKing_BurgerKingCall.SendEmail").Info("BK调用汉堡王接口错误告警邮件发送失败，邮件信息记录到数据库失败。" + "接口收到的参数内容：" + JSONInfo + "，接口调用错误原因：" + errorMessage, null);
            }
        }
    }
}
