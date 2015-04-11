using System;

namespace CSMP.Model
{
    [SerializableAttribute]
    public class CityInfo
    {
        /// <summary>
        ///
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int ProvinceID { get; set; }

        /// <summary>
        ///
        /// </summary>
        public bool IsClosed { get; set; }


    }
}
