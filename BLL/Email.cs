using System;
using System.Collections.Generic;
using CSMP.DAL;
using CSMP.Model;
using System.Text;
using System.Net.Mail;
using System.Net;

namespace CSMP.BLL
{
    public static class EmailBLL
    {
        private static Encoding Encod = Encoding.GetEncoding(936);
        public static string Email_Send(EmailInfo einfo)
        {
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
                return "there is no Mail address";
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

                LogInfo info = new LogInfo();
                info.AddDate = DateTime.Now;
                info.Category = Enum.GetName(typeof(SysEnum.LogType), SysEnum.LogType.邮件发送记录);
                info.Content = "发给给：";
                foreach (MailAddress item in mm.To)
                {
                    info.Content += item.DisplayName + "(" + item.Address + ")+<br/>";
                }
                info.Content += "标题:" + mm.Subject;
                info.Content += "内容:" + mm.Body;
                info.ErrorDate = Tool.Function.ErrorDate;
                info.SendEmail = false;
                info.UserName = DicInfo.Admin;
                info.Serious = 1;
                LogBLL.Add(info);
                return string.Empty;
            }
            catch (Exception ex)
            {
                LogInfo info = new LogInfo();
                info.AddDate = DateTime.Now;
                info.Category = Enum.GetName(typeof(SysEnum.LogType), SysEnum.LogType.YUM邮件接口发送失败);
                info.Content = "邮件发送失败，具体信息：" + ex.Message + mm.Subject;
                info.ErrorDate = Tool.Function.ErrorDate;
                info.SendEmail = true;
                info.UserName = DicInfo.Admin;
                info.Serious = 1;
                LogBLL.Add(info);

                return "System Error" + ex.Message.ToString();
            }

        }


    }
}