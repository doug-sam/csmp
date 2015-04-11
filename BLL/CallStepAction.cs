using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSMP.Model;
using Tool;

namespace CSMP.BLL
{
    /// <summary>
    /// 处理步骤中会出现额外的操作
    /// </summary>
    public static class CallStepActionBLL
    {
        public static class ActionInfo
        {
            private static int YUMID;
            private static List<int> ZaraID;

            public static bool IsYUM(CallInfo info)
            {
                if (YUMID <= 0)
                {
                    UpdateCustomerID();
                }
                if (null == info)
                {
                    return false;
                }
                return info.CustomerID == YUMID;
            }

            public static bool IsZara(CallInfo info)
            {
                if (ZaraID == null || ZaraID.Count == 0)
                {
                    UpdateCustomerID();
                }
                if (null == info)
                {
                    return false;
                }
                return ZaraID.Contains(info.BrandID);
            }

            public static void UpdateCustomerID()
            {
                YUMID = Function.ConverToInt(ProfileBLL.GetValue(ProfileInfo.YUM_API.YUM客户ID, true), 0);
                ZaraID = new List<int>();
                foreach (string item in ProfileBLL.GetValue(ProfileInfo.API_ZARA.ZARA客户ID, true).Split('|'))
                {
                    ZaraID.Add(Function.ConverToInt(item, 0));

                }
            }
        }



        /// <summary>
        /// 新建报修时
        /// </summary>
        /// <param name="cinfo"></param>
        /// <returns></returns>
        public static string StepNewCall(CallInfo cinfo)
        {

            if (ActionInfo.IsYUM(cinfo) && ActionYUM.Enable())
            {
                return ActionYUM.API_Step1(cinfo);
            }
            if (ActionInfo.IsZara(cinfo) && ActionZara.Enable(cinfo))
            {
                return ActionZara.API_Step1(cinfo);
            }
            return null;
        }

        /// <summary>
        /// 预约安排工程师上门时
        /// </summary>
        /// <param name="cinfo"></param>
        /// <param name="csReadyDropIn"></param>
        /// <returns></returns>
        public static string StepReadyDropIn(CallInfo cinfo, CallStepInfo csReadyDropIn)
        {
            if (ActionInfo.IsYUM(cinfo) && ActionYUM.Enable())
            {
                return ActionYUM.API_Step2(cinfo);
            }
            if (ActionInfo.IsZara(cinfo) && ActionZara.Enable(cinfo))
            {
                return ActionZara.API_Step2(cinfo, csReadyDropIn);
            }
            return null;
        }

        /// <summary>
        /// 工程师离场时
        /// </summary>
        /// <param name="cinfo"></param>
        /// <param name="csLeaveInfo"></param>
        /// <returns></returns>
        public static string StepDropInLeave(CallInfo cinfo, CallStepInfo csLeaveInfo)
        {
            if (ActionInfo.IsYUM(cinfo) && ActionYUM.Enable())
            {
                return ActionYUM.API_Step3(cinfo);
            }
            return null;
        }

        /// <summary>
        /// 二线工程师对离场跟进确认
        /// </summary>
        /// <param name="cinfo"></param>
        /// <param name="csLeaveInfo"></param>
        /// <returns></returns>
        public static string StepDropInSlove(CallInfo cinfo, CallStepInfo csLeaveInfo)
        {
            if (ActionInfo.IsYUM(cinfo) && ActionYUM.Enable())
            {
                
                return ActionYUM.API_StepDropInSlove(cinfo);
            }
            return null;
        }


        /// <summary>
        /// call解决时
        /// </summary>
        /// <param name="cinfo"></param>
        /// <param name="csinfo">解决的那条记录</param>
        /// <returns></returns>
        public static string StepSloved(CallInfo cinfo, CallStepInfo csinfo)
        {
            if (ActionInfo.IsZara(cinfo) && ActionZara.Enable(cinfo))
            {
                return ActionZara.API_Step3(cinfo, csinfo);
            }
            return null;
        }

        public static class ActionYUM
        {
            /// <summary>
            /// 系统设置中是否启用
            /// </summary>
            /// <returns></returns>
            public static bool Enable()
            {
                string Val = ProfileBLL.GetValue(ProfileInfo.YUM_API.总开关, true);
                return Val.ToLower() == "true";
            }


            public static string API_Step1(CallInfo info)
            {
                EmailInfo einfo = GetEmailInfo();

                einfo.Subject = string.Format("##{0}## {1}", info.No, info.Details);

                einfo.Body = Tool.DocHelper.Read("/page/System/API_YUM/Email1.txt", true);// ProfileBLL.GetValue(ProfileInfo.YUM_API.邮件模板一, true);
                einfo.Body = einfo.Body.Replace("(((((报修时间)))))", info.ErrorDate.ToString("yyyy-MM-dd HH:mm:ss"));
                einfo.Body = einfo.Body.Replace("(((((店铺名)))))", StoresBLL.GetName(info.StoreID));
                einfo.Body = einfo.Body.Replace("(((((店铺号)))))", info.StoreName);
                einfo.Body = einfo.Body.Replace("(((((来源单号)))))", info.ReportSourceNo);
                einfo.Body = einfo.Body.Replace("(((((大类故障)))))", info.ClassName1);
                einfo.Body = einfo.Body.Replace("(((((中类故障)))))", info.ClassName2);
                einfo.Body = einfo.Body.Replace("(((((小类故障)))))", info.ClassName3);
                einfo.Body = einfo.Body.Replace("(((((SLA)))))", info.SLA.ToString());
                einfo.Body = einfo.Body.Replace("(((((扩展SLA)))))", info.SLA2);
                einfo.Body = einfo.Body.Replace("(((((故障描述)))))", info.Details);
                return EmailBLL.Email_Send(einfo);
            }

            private static EmailInfo GetEmailInfo()
            {
                EmailInfo einfo = new EmailInfo();
                einfo.FromPort = Tool.Function.ConverToInt(ProfileBLL.GetValue(ProfileInfo.YUM_API.发件邮箱端口));
                einfo.FromEmailAddress = ProfileBLL.GetValue(ProfileInfo.YUM_API.发件邮箱地址);
                einfo.FromEmailPwd = ProfileBLL.GetValue(ProfileInfo.YUM_API.发件邮箱密码);
                einfo.FromEmailHost = ProfileBLL.GetValue(ProfileInfo.YUM_API.发件邮件主机);
                einfo.FromEmailDisplayName = ProfileBLL.GetValue(ProfileInfo.YUM_API.发件人显示名);


                einfo.Attachment = null;
                einfo.CC = null;
                einfo.MailAddress = new List<System.Net.Mail.MailAddress>();
                List<string> SendTo = ProfileBLL.GetValue(ProfileInfo.YUM_API.对方邮箱地址, true).Split(';').ToList();
                foreach (string item in SendTo)
                {
                    if (string.IsNullOrEmpty(item.Trim()))
                    {
                        continue;
                    }
                    einfo.MailAddress.Add(new System.Net.Mail.MailAddress(item.Trim()));
                }
                return einfo;
            }
            public static string API_Step2(CallInfo info)
            {
                if (string.IsNullOrEmpty(info.CallNo3))
                {
                    return "该报修未记录YUM单号";
                }

                EmailInfo einfo = GetEmailInfo();


                einfo.Subject = string.Format("[Ticket#{0}]##{1}## {2}", info.CallNo3, info.No, info.Details);

                List<CallStepInfo> steplist = CallStepBLL.GetList(info.ID);
                if (null == steplist || steplist.Count == 0)
                {
                    return "报修数据不全，没有处理记录";
                }

                CallStepInfo ReadyDropIn = null;//备件情况
                CallStepInfo ReadyDropIn1 = null;//上门安排
                foreach (CallStepInfo item in steplist)
                {
                    if (item.StepType == (int)SysEnum.StepType.上门安排)
                    {
                        ReadyDropIn1 = item;
                    }
                    if (item.StepType == (int)SysEnum.StepType.上门准备)
                    {
                        ReadyDropIn = item;
                    }

                }
                if (null == ReadyDropIn1 || ReadyDropIn1.ID <= 0)
                {
                    return "没有上门记录";
                }


                einfo.Body = Tool.DocHelper.Read("/page/System/API_YUM/Email2.txt", true);// ProfileBLL.GetValue(ProfileInfo.YUM_API.邮件模板二, true);
                einfo.Body = einfo.Body.Replace("(((((响应时间)))))", steplist[0].AddDate.ToString("yyyy-MM-dd HH:mm:ss"));
                einfo.Body = einfo.Body.Replace("(((((上门工程师)))))", ReadyDropIn1.MajorUserName);
                einfo.Body = einfo.Body.Replace("(((((描述)))))", (ReadyDropIn == null ? "" : ReadyDropIn.Details));

                return EmailBLL.Email_Send(einfo);
            }
            public static string API_Step3(CallInfo info)
            {
                if (string.IsNullOrEmpty(info.CallNo3))
                {
                    return "该报修未记录YUM单号";
                }

                EmailInfo einfo = GetEmailInfo();


                einfo.Subject = string.Format("[Ticket#{0}]##{1}## {2}", info.CallNo3, info.No, info.Details);

                List<CallStepInfo> steplist = CallStepBLL.GetList(info.ID);
                if (null == steplist || steplist.Count == 0)
                {
                    return "报修数据不全，没有处理记录";
                }
                CallStepInfo DropInDone = null;
                CallStepInfo DropInIng = null;
                foreach (CallStepInfo item in steplist)
                {
                    if (item.StepType == (int)SysEnum.StepType.上门详细)
                    {
                        DropInDone = item;
                    }
                    if (item.StepType == (int)SysEnum.StepType.到达门店处理)
                    {
                        DropInIng = item;
                    }
                }
                if (null == DropInDone || DropInDone.ID <= 0 || null == DropInIng || DropInIng.ID <= 0)
                {
                    return "没有上门详细记录";
                }


                einfo.Body = Tool.DocHelper.Read("/page/System/API_YUM/Email3.txt", true);//  ProfileBLL.GetValue(ProfileInfo.YUM_API.邮件模板三, true);
                einfo.Body = einfo.Body.Replace("(((((到达现场时间)))))", DropInIng.DateBegin.ToString("yyyy-MM-dd HH:mm:ss"));
                einfo.Body = einfo.Body.Replace("(((((完成时间)))))", DropInDone.DateEnd.ToString("yyyy-MM-dd HH:mm:ss"));
                einfo.Body = einfo.Body.Replace("(((((上门工程师)))))", DropInDone.MajorUserName);
                einfo.Body = einfo.Body.Replace("(((((描述)))))", DropInDone.Details);
                return EmailBLL.Email_Send(einfo);

            }

            public static string API_StepDropInSlove(CallInfo info)
            {
                if (string.IsNullOrEmpty(info.CallNo3))
                {
                    return "该报修未记录YUM单号";
                }

                EmailInfo einfo = GetEmailInfo();


                einfo.Subject = string.Format("[Ticket#{0}]##{1}## {2}", info.CallNo3, info.No, info.Details);

                List<CallStepInfo> steplist = CallStepBLL.GetList(info.ID);
                if (null == steplist || steplist.Count == 0)
                {
                    return "报修数据不全，没有处理记录";
                }
                CallStepInfo DropInDone = null;
                CallStepInfo DropInIng = null;
                foreach (CallStepInfo item in steplist)
                {
                    if (item.StepType == (int)SysEnum.StepType.上门详细)
                    {
                        DropInDone = item;
                    }
                    if (item.StepType == (int)SysEnum.StepType.到达门店处理)
                    {
                        DropInIng = item;
                    }
                }
                if (null == DropInDone || DropInDone.ID <= 0 || null == DropInIng || DropInIng.ID <= 0)
                {
                    return "没有上门详细记录";
                }


                einfo.Body = Tool.DocHelper.Read("/page/System/API_YUM/EmailSlove.txt", true);//  ProfileBLL.GetValue(ProfileInfo.YUM_API.邮件模板三, true);
                einfo.Body = einfo.Body.Replace("(((((到达现场时间)))))", DropInIng.DateBegin.ToString("yyyy-MM-dd HH:mm:ss"));
                einfo.Body = einfo.Body.Replace("(((((完成时间)))))", DropInDone.DateEnd.ToString("yyyy-MM-dd HH:mm:ss"));
                einfo.Body = einfo.Body.Replace("(((((上门工程师)))))", DropInDone.MajorUserName);
                einfo.Body = einfo.Body.Replace("(((((描述)))))", DropInDone.Details);
                return EmailBLL.Email_Send(einfo);

            }

        }

        public static class ActionZara
        {
            /// <summary>
            /// 系统设置中是否启用
            /// </summary>
            /// <returns></returns>
            public static bool Enable(CallInfo info)
            {
                string Val = ProfileBLL.GetValue(ProfileInfo.API_ZARA.总开关, true);
                return Val.ToLower() == "true" && !string.IsNullOrEmpty(info.ReportSourceNo.Trim());;
            }

            private static EmailInfo GetEmailInfo()
            {
                EmailInfo einfo = new EmailInfo();
                einfo.FromPort = Tool.Function.ConverToInt(ProfileBLL.GetValue(ProfileInfo.API_ZARA.发件邮箱端口));
                einfo.FromEmailAddress = ProfileBLL.GetValue(ProfileInfo.API_ZARA.发件邮箱地址);
                einfo.FromEmailPwd = ProfileBLL.GetValue(ProfileInfo.API_ZARA.发件邮箱密码);
                einfo.FromEmailHost = ProfileBLL.GetValue(ProfileInfo.API_ZARA.发件邮件主机);
                einfo.FromEmailDisplayName = ProfileBLL.GetValue(ProfileInfo.API_ZARA.发件人显示名);

                einfo.ReplayTo = new System.Net.Mail.MailAddress(ProfileBLL.GetValue(ProfileInfo.API_ZARA.回复地址, true), ProfileBLL.GetValue(ProfileInfo.API_ZARA.回复时显示名, true));

                einfo.Attachment = null;
                einfo.CC = null;
                einfo.MailAddress = new List<System.Net.Mail.MailAddress>();
                return einfo;
            }


            public static string API_Step1(CallInfo info)
            {
                EmailInfo einfo = new EmailInfo();
                einfo.Attachment = null;
                einfo.CC = null;
                einfo.MailAddress = new List<System.Net.Mail.MailAddress>();
                List<string> SendTo = ProfileBLL.GetValue(ProfileInfo.API_ZARA.对方邮箱地址1, true).Split(';').ToList();
                foreach (string item in SendTo)
                {
                    if (string.IsNullOrEmpty(item.Trim()))
                    {
                        continue;
                    }
                    einfo.MailAddress.Add(new System.Net.Mail.MailAddress(item.Trim()));
                }


                einfo.Subject = Tool.DocHelper.Read("/page/System/API_Zara/EmailSubject1.txt", true);
                einfo.Subject = einfo.Subject.Replace("(((((来源单号)))))", info.ReportSourceNo);
                einfo.Subject = einfo.Subject.Replace("(((((系统单号)))))", info.No);

                einfo.Body = Tool.DocHelper.Read("/page/System/API_Zara/EmailBody1.txt", true);
                einfo.Body = einfo.Body.Replace("(((((系统单号)))))", info.No);
                einfo.Body = einfo.Body.Replace("(((((店铺号)))))", info.StoreName);
                einfo.Body = einfo.Body.Replace("(((((店铺名称)))))", info.StoreNo);
                einfo.Body = einfo.Body.Replace("(((((来源单号)))))", info.ReportSourceNo);
                einfo.Body = einfo.Body.Replace("(((((故障详细)))))", info.Details);
                einfo.Body = einfo.Body.Replace("(((((工程师)))))", info.MaintaimUserName);
                return EmailBLL.Email_Send(einfo);
            }
            public static string API_Step2(CallInfo info, CallStepInfo csReadyDropIn)
            {
                EmailInfo einfo = new EmailInfo();
                einfo.Attachment = null;
                einfo.CC = null;
                einfo.MailAddress = new List<System.Net.Mail.MailAddress>();
                List<string> SendTo = ProfileBLL.GetValue(ProfileInfo.API_ZARA.对方邮箱地址2, true).Split(';').ToList();
                foreach (string item in SendTo)
                {
                    if (string.IsNullOrEmpty(item.Trim()))
                    {
                        continue;
                    }
                    einfo.MailAddress.Add(new System.Net.Mail.MailAddress(item.Trim()));
                }


                einfo.Subject = Tool.DocHelper.Read("/page/System/API_Zara/EmailSubject2.txt", true);
                einfo.Subject = einfo.Subject.Replace("(((((来源单号)))))", info.ReportSourceNo);
                einfo.Subject = einfo.Subject.Replace("(((((系统单号)))))", info.No);

                einfo.Body = Tool.DocHelper.Read("/page/System/API_Zara/EmailBody2.txt", true);
                einfo.Body = einfo.Body.Replace("(((((系统单号)))))", info.No);
                einfo.Body = einfo.Body.Replace("(((((店铺号)))))", info.StoreName);
                einfo.Body = einfo.Body.Replace("(((((店铺名称)))))", info.StoreNo);
                einfo.Body = einfo.Body.Replace("(((((来源单号)))))", info.ReportSourceNo);
                einfo.Body = einfo.Body.Replace("(((((故障详细)))))", info.Details);
                einfo.Body = einfo.Body.Replace("(((((工程师)))))", info.MaintaimUserName);
                einfo.Body = einfo.Body.Replace("(((((上门工程师)))))", csReadyDropIn.MajorUserName);
                einfo.Body = einfo.Body.Replace("(((((预约上门时间)))))", csReadyDropIn.DateBegin.ToString("yyyy-MM-dd HH:mm"));
                return EmailBLL.Email_Send(einfo);
            }
            public static string API_Step3(CallInfo info, CallStepInfo csinfo)
            {
                EmailInfo einfo = new EmailInfo();
                einfo.Attachment = null;
                einfo.CC = null;
                einfo.MailAddress = new List<System.Net.Mail.MailAddress>();
                List<string> SendTo = ProfileBLL.GetValue(ProfileInfo.API_ZARA.对方邮箱地址3, true).Split(';').ToList();
                foreach (string item in SendTo)
                {
                    if (string.IsNullOrEmpty(item.Trim()))
                    {
                        continue;
                    }
                    einfo.MailAddress.Add(new System.Net.Mail.MailAddress(item.Trim()));
                }


                einfo.Subject = Tool.DocHelper.Read("/page/System/API_Zara/EmailSubject3.txt", true);
                einfo.Subject = einfo.Subject.Replace("(((((来源单号)))))", info.ReportSourceNo);
                einfo.Subject = einfo.Subject.Replace("(((((系统单号)))))", info.No);

                einfo.Body = Tool.DocHelper.Read("/page/System/API_Zara/EmailBody3.txt", true);
                einfo.Body = einfo.Body.Replace("(((((解决详细)))))", csinfo.Details);
                einfo.Body = einfo.Body.Replace("(((((系统单号)))))", info.No);
                einfo.Body = einfo.Body.Replace("(((((系统单号)))))", info.No);
                einfo.Body = einfo.Body.Replace("(((((店铺号)))))", info.StoreName);
                einfo.Body = einfo.Body.Replace("(((((店铺名称)))))", info.StoreNo);
                einfo.Body = einfo.Body.Replace("(((((来源单号)))))", info.ReportSourceNo);
                einfo.Body = einfo.Body.Replace("(((((故障详细)))))", info.Details);
                einfo.Body = einfo.Body.Replace("(((((工程师)))))", info.MaintaimUserName);

                return EmailBLL.Email_Send(einfo);

            }

        }

    }
}
