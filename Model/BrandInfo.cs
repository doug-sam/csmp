using System;

namespace CSMP.Model
{
    [SerializableAttribute]
    public class BrandInfo
    {
        /// <summary>
        ///
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        ///品牌名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///客户id
        /// </summary>
        public int CustomerID { get; set; }

        /// <summary>
        ///客户名
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        ///用户id
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        ///联系人
        /// </summary>
        public string Contact { get; set; }

        /// <summary>
        ///联系人手机
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        ///联系人邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        ///
        /// </summary>
        public bool IsClose { get; set; }

        public int SlaModeID { get; set; }

        /// <summary>
        /// 超时第一次警报，无获取为0，以分钟分单位
        /// </summary>
        public int SlaTimer1 { get; set; }

        /// <summary>
        /// 超时第二次警报，无获取为0，以分钟分单位
        /// </summary>
        public int SlaTimer2 { get; set; }

        /// <summary>
        /// 超时警报收件人
        /// </summary>
        public string SlaTimerTo { get; set; }

    }
}
