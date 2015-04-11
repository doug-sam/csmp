using System;

namespace CSMP.Model
{
    [SerializableAttribute]
    public class AssignInfo
    {
        /// <summary>
        ///
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int CallID { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int Step { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int UseID { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        ///
        /// </summary>
        public DateTime AddDate { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string OldName { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int OldID { get; set; }

        /// <summary>
        ///转到的工作组
        /// </summary>
        public int WorkGroupID { get; set; }

        /// <summary>
        ///添加人id，系统转的默认为0
        /// </summary>
        public int CreatorID { get; set; }

        /// <summary>
        ///添加人名
        /// </summary>
        public string CreatorName { get; set; }

        public bool CrossWorkGroup { get; set; }
    }
}
