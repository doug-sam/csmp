using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSMP.Model
{
    /// <summary>
    /// 跑马灯消息，2015.11.18ZQL加，现在只作为APP有动作时在页面顶部显示给二线看
    /// </summary>
    [SerializableAttribute]
    public class MarqueeMessage
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
        /// 跑马灯消息类容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 是否已读
        /// </summary>
        public bool IsRead { get; set; }
    }
}
