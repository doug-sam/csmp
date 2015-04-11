using System;
using System.Collections.Generic;

namespace CSMP.Model
{
    [SerializableAttribute]
    public class CommentInfo
    {
        public enum ScoreType { Total = 1, Score2, Score3 }

        public enum MachineType { web=1,android=2,ios=3}
        /// <summary>
        ///
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int DropInUserID { get; set; }

        /// <summary>
        ///二线人员id
        /// </summary>
        public int SupportUserID { get; set; }

        /// <summary>
        ///对call的哪次上门的评论
        /// </summary>
        public int CallStepID { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int CallID { get; set; }

        /// <summary>
        ///是否为现场工程师发起的评论
        /// </summary>
        public bool IsDropInUserDoIt { get; set; }

        /// <summary>
        ///使用电脑还是android还是什么设备
        /// </summary>
        public string ByMachine { get; set; }

        /// <summary>
        ///添加时间
        /// </summary>
        public DateTime AddDate { get; set; }

        /// <summary>
        ///评分值
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        ///评论详细
        /// </summary>
        public string Details { get; set; }

        public int WorkGroupID { get; set; }

        /// <summary>
        ///评分值
        /// </summary>
        public int Score2 { get; set; }

        /// <summary>
        ///评分值
        /// </summary>
        public int Score3 { get; set; }

    }
}
