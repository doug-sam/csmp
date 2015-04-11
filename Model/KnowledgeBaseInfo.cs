using System;

namespace CSMP.Model
{
    [SerializableAttribute]
    public class KnowledgeBaseInfo
    {
        /// <summary>
        ///
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string AddByUserName { get; set; }

        /// <summary>
        ///
        /// </summary>
        public bool Enable { get; set; }

        /// <summary>
        ///
        /// </summary>
        public DateTime AddDate { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int ViewCount { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int GoodCount { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string Labs { get; set; }


    }
}
