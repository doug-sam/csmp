using System;

namespace CSMP.Model
{
    [SerializableAttribute]
    public class ProfileInfo
    {
        /// <summary>
        ///
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string GroupID { get; set; }

        /// <summary>
        /// YUM客户的API
        /// </summary>
        public static class YUM_API
        {
            public static string 总开关 = "user_API_YUM_Enable";
            public static string YUM客户ID = "user_API_YUM_ID";
            public static string 对方邮箱地址 = "user_API_YUM_MailAddress";
            public static string 邮件模板一 = "user_API_YUM_Template1";
            public static string 邮件模板二 = "user_API_YUM_Template2";
            public static string 邮件模板三 = "user_API_YUM_Template3";

            /// <summary>
            /// 发件邮箱地址
            /// </summary>
            public static string 发件邮箱地址 = "user_API_Email_Address";
            /// <summary>
            /// 发件邮件主机如smtp.163.com
            /// </summary>
            public static string 发件邮件主机 = "user_API_Email_Host";
            /// <summary>
            /// 发件邮箱端口，默认25
            /// </summary>
            public static string 发件邮箱端口 = "user_API_Email_Port";
            /// <summary>
            /// 发件邮箱密码
            /// </summary>
            public static string 发件邮箱密码 = "user_API_Email_Pwd";
            /// <summary>
            /// 发件人显示名
            /// </summary>
            public static string 发件人显示名 = "user_API_Email_DisplayName";
            /// <summary>
            /// 回复时显示名
            /// </summary>
            public static string 回复时显示名 = "user_API_Email_ReplyName";
            /// <summary>
            /// 回复地址 
            /// </summary>
            public static string 回复地址 = "user_API_Email_ReplyAddress";


        }

        /// <summary>
        /// YUM客户的API
        /// </summary>
        public static class API_ZARA
        {
            public static string 总开关 = "user_API_ZARA_Enable";
            public static string ZARA客户ID = "user_API_ZARA_ID";
            public static string 对方邮箱地址1 = "user_API_ZARA_MailAddress1";
            public static string 对方邮箱地址2 = "user_API_ZARA_MailAddress2";
            public static string 对方邮箱地址3 = "user_API_ZARA_MailAddress3";
            public static string 邮件模板一 = "user_API_ZARA_Template1";
            public static string 邮件模板二 = "user_API_ZARA_Template2";
            public static string 邮件模板三 = "user_API_ZARA_Template3";

            /// <summary>
            /// 发件邮箱地址
            /// </summary>
            public static string 发件邮箱地址 = "user_API_Email_Address_ZARA";
            /// <summary>
            /// 发件邮件主机如smtp.163.com
            /// </summary>
            public static string 发件邮件主机 = "user_API_Email_Host_ZARA";
            /// <summary>
            /// 发件邮箱端口，默认25
            /// </summary>
            public static string 发件邮箱端口 = "user_API_Email_Port_ZARA";
            /// <summary>
            /// 发件邮箱密码
            /// </summary>
            public static string 发件邮箱密码 = "user_API_Email_Pwd_ZARA";
            /// <summary>
            /// 发件人显示名
            /// </summary>
            public static string 发件人显示名 = "user_API_Email_DisplayName_ZARA";
            /// <summary>
            /// 回复时显示名
            /// </summary>
            public static string 回复时显示名 = "user_API_Email_ReplyName_ZARA";
            /// <summary>
            /// 回复地址 
            /// </summary>
            public static string 回复地址 = "user_API_Email_ReplyAddress_ZARA";


        }

        public static class API_Message
        {
            /// <summary>
            /// 总开关
            /// </summary>
            public static string 总开关 = "user_API_Message_Enable";
            /// <summary>
            /// host
            /// </summary>
            public static string Host = "user_API_Message_host";
            /// <summary>
            /// Port
            /// </summary>
            public static string Port = "user_API_Message_port";
            /// <summary>
            /// accountId
            /// </summary>
            public static string accountId = "user_API_Message_accountId";
            /// <summary>
            /// password
            /// </summary>
            public static string password = "user_API_Message_password";
            /// <summary>
            /// serviceId
            /// </summary>
            public static string serviceId = "user_API_Message_serviceId";
            /// <summary>
            /// 1号短信模板
            /// </summary>
            public static string MsgTemplate1 = "user_API_Message_MsgTemplate1";
        }

        /// <summary>
        /// 暂时无用
        /// </summary>
        public static class UserKey
        {

            public static class MobileMsgKey {
                public static string Host = "user_MobileMsg_Host";
                public static string Port = "user_MobileMsg_Port";
                public static string AccountID = "user_MobileMsg_AccountID";
                public static string ServiceID = "user_MobileMsg_ServiceID";
                public static string PassWord = "user_MobileMsg_PassWord";

            }

            /// <summary>
            /// 发件邮箱地址
            /// </summary>
            public static string 发件邮箱地址 = "user_Email_Address";
            /// <summary>
            /// 发件邮件主机如smtp.163.com
            /// </summary>
            public static string 发件邮件主机 = "user_Email_Host";
            /// <summary>
            /// 发件邮箱端口，默认25
            /// </summary>
            public static string 发件邮箱端口 = "user_Email_Port";
            /// <summary>
            /// 发件邮箱密码
            /// </summary>
            public static string 发件邮箱密码 = "user_Email_Pwd";
            /// <summary>
            /// 发件人显示名
            /// </summary>
            public static string 发件人显示名 = "user_Email_DisplayName";
            /// <summary>
            /// 回复时显示名
            /// </summary>
            public static string 回复时显示名 = "user_Email_ReplyName";
            /// <summary>
            /// 回复地址 
            /// </summary>
            public static string 回复地址 = "user_Email_ReplyAddress";

            /// <summary>
            /// 邮件提醒时间间隔
            /// </summary>
            public static string 邮件提醒时间间隔 = "user_Email_AutoSentInterval";

            /// <summary>
            /// 邮件模板
            /// </summary>
            public static string 邮件模板 = "user_Email_TempLate";

            /// <summary>
            /// 系统地址
            /// </summary>
            public static string 系统地址 = "user_Profile_Url";
            /// <summary>
            /// 系统外网地址
            /// </summary>
            public static string 系统外网地址 = "user_Profile_Url_WWW";
            /// <summary>
            /// 电话服务器录音根地址
            /// </summary>
            public static string 电话服务器录音根地址 = "user_Profile_RecordUrl";
            /// <summary>
            /// APP版本
            /// </summary>
            public static string AppVersion = "user_Profile_AppVersion";
            /// <summary>
            /// 全国组ID
            /// </summary>
            public static string GrobalGroupID = "user_Profile_GrobalGroupID";

            
        }

    }
}
