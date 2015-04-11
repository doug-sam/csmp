using System;

namespace CSMP.Model
{
    [SerializableAttribute]
    public class LogInfo
    {
        /// <summary>
        ///
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        ///
        /// </summary>
        public DateTime AddDate { get; set; }

        /// <summary>
        ///
        /// </summary>
        public DateTime ErrorDate { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int Serious { get; set; }

        /// <summary>
        ///
        /// </summary>
        public bool SendEmail { get; set; }

        public LogInfo()
        {
            ID = 0;
            Category = string.Empty;
            UserName = string.Empty;
            Content = string.Empty;
            AddDate = DateTime.Now;
            ErrorDate = DateTime.Now;
            Serious = 0;
            SendEmail = false;
        }
    }
}
