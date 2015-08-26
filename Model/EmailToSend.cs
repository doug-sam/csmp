using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSMP.Model
{
    public class EmailToSend
    {
        /// <summary>
        ///主键ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 单号No
        /// </summary>
        public string CallNo { get; set; }
        /// <summary>
        /// 邮件主题
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// 收件人地址，多个以;隔开
        /// </summary>
        public string MailAddress { get; set; }
        /// <summary>
        /// 邮件抄送人地址，多个以;隔开
        /// </summary>
        public string CC { get; set; }
        /// <summary>
        /// 邮件答复地址
        /// </summary>
        public string ReplayTo { get; set; }
        /// <summary>
        /// 附件信息，暂时不使用
        /// </summary>
        public string Attachment { get; set; }
        /// <summary>
        /// 发件人地址
        /// </summary>
        public string FromEmailAddress { get; set; }
        /// <summary>
        /// 发件人显示名称
        /// </summary>
        public string FromEmailDisplayName { get; set; }
        /// <summary>
        /// 发件 主机地址
        /// </summary>
        public string FromEmailHost { get; set; }
        /// <summary>
        /// 发件 邮箱密码
        /// </summary>
        public string FromEmailPwd { get; set; }
        /// <summary>
        /// 发件 使用端口号
        /// </summary>
        public string FromPort { get; set; }

        /// <summary>
        /// 邮件类容
        /// </summary>
        public string Body { get; set; }
        /// <summary>
        /// 客户ID
        /// </summary>
        public int CustomerID { get; set; }
        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// 品牌ID
        /// </summary>
        public int BrandID { get; set; }
        /// <summary>
        /// 品牌名称
        /// </summary>
        public string BrandName { get; set; }

    }
}
