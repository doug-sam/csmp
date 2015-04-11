using System;

namespace CSMP.Model
{
    [SerializableAttribute]
    public class CallStepInfo
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
        public DateTime AddDate { get; set; }

        /// <summary>
        ///
        /// </summary>
        public DateTime DateBegin { get; set; }

        /// <summary>
        ///
        /// </summary>
        public DateTime DateEnd { get; set; }

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
        public string StepName { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int StepIndex { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int SolutionID { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string SolutionName { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string Details { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int MajorUserID { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string MajorUserName { get; set; }

        public bool IsSolved { get; set; }

        public int StepType { get; set; }
    }
}
