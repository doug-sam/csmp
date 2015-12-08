using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSMP.Model
{
    /// <summary>
    /// 跑马灯消息报表统计，2015.11.26ZQL加，现在只作为APP使用
    /// </summary>
    [SerializableAttribute]
    public class MarqueeMessageReport
    {
        /// <summary>
        ///主键ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// call的no
        /// </summary>
        public string No { get; set; }
        /// <summary>
        /// 二线负责人名称
        /// </summary>
        public int MaintainUserID { get; set; }
        /// <summary>
        /// 二线负责人姓名
        /// </summary>
        public string MaintainUserName { get; set; }
        /// <summary>
        ///现场工程师负责人ID
        /// </summary>
        public int MajorUserID { get; set; }
        /// <summary>
        /// 现场工程师负责人姓名
        /// </summary>
        public string MajorUserName { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime ActionTime { get; set; }
        /// <summary>
        /// 跑马灯消息类容
        /// </summary>
        public string Content { get; set; }
    }
}
