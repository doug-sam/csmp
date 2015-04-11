using System;

namespace CSMP.Model
{
    [SerializableAttribute]
    public class RecordInfo
    {
        /// <summary>
        ///
        /// </summary>
        public int dbid { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string recordname { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string filepath { get; set; }

        /// <summary>
        ///
        /// </summary>
        public DateTime stardate { get; set; }

        /// <summary>
        ///
        /// </summary>
        public DateTime enddate { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string unickid { get; set; }



    }
}
