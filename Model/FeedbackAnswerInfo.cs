using System;

namespace CSMP.Model
{
    [SerializableAttribute]
    public class FeedbackAnswerInfo
    {
        /// <summary>
        ///
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int PaperID { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int QuestionID { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string Answer { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int Answer2 { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int RecorderID { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string RecorderName { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string FeedbackUserName { get; set; }

        public int CallID { get; set; }

        public int CallStepID { get; set; }

        public DateTime AddDate { get; set; }
        public string QuestionName { get; set; }
    }
}
