using System;

namespace CSMP.Model
{
    [SerializableAttribute]
    public class DropInMemoInfo
    {
        /// <summary>
        ///
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int StepID { get; set; }

        /// <summary>
        ///
        /// </summary>
        public DateTime AddDate { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string Details { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int OrderNumber { get; set; }

        /// <summary>
        ///
        /// </summary>
        public bool Enable { get; set; }

        /// <summary>
        ///事件发生时间
        /// </summary>
        public DateTime MemoDate { get; set; }

        public string TypeName { get; set; }

    }
}
