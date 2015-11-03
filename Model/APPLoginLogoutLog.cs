using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSMP.Model
{
    public class APPLoginLogoutLog
    {
        /// <summary>
        ///
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        ///用户ID
        /// </summary>
        public string OpenID { get; set; }

        /// <summary>
        ///签到时间
        /// </summary>
        public DateTime? LoginTime { get; set; }
        /// <summary>
        ///签到地点
        /// </summary>
        public string  LoginLocation { get; set; }
        /// <summary>
        ///签退时间
        /// </summary>
        public DateTime? LogoutTime { get; set; }
        /// <summary>
        ///签退地点
        /// </summary>
        public string LogoutLocation { get; set; }
        /// <summary>
        /// 状态1为签到，0为签退
        /// 与微信的这个状态是反的
        /// </summary>
        public int Status { get; set; }

    }
}
