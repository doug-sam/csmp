using System;

namespace CSMP.Model
{
    [SerializableAttribute]
    public class Class1Info
    {
        public Class1Info() { ID = 0; Name = string.Empty;IsClosed = false; CustomerID = 0; }

        /// <summary>
        ///
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        ///类名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///
        /// </summary>
        public bool IsClosed { get; set; }

        /// <summary>
        ///所属客户ID
        /// </summary>
        public int CustomerID { get; set; }


    }
}
