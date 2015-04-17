using System;
using System.Collections.Generic;

namespace CSMP.Model
{
    [SerializableAttribute]
    public class UserInfo
    {
        /// <summary>
        ///
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        ///用户名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///登录名
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        ///密码
        /// </summary>
        public string PassWord { get; set; }

        /// <summary>
        ///邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string Tel { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        ///
        /// </summary>
        public bool Sex { get; set; }

        /// <summary>
        ///所在城市
        /// </summary>
        public int CityID { get; set; }

        /// <summary>
        ///
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        ///上次登录id
        /// </summary>
        public DateTime LastDate { get; set; }

        /// <summary>
        ///
        /// </summary>
        public bool Enable { get; set; }

        /// <summary>
        ///所在工作组
        /// </summary>
        public int WorkGroupID { get; set; }

        /// <summary>
        ///所在权限组
        /// </summary>
        public int PowerGroupID { get; set; }
        /// <summary>
        /// 在系统中的角色
        /// </summary>
        public List<string> Rule { get; set; }

        /// <summary>
        /// 是否作为邮件候选收件人
        /// </summary>
        public bool EmailContact { get; set; }
        /// <summary>
        /// 是否作为默认的收件人
        /// </summary>
        public bool EmailTo { get; set; }
        /// <summary>
        /// 是否作为邮件默认抄送人
        /// </summary>
        public bool EmailCC { get; set; }

    }
}
