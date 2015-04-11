using System;

namespace CSMP.Model
{
    [SerializableAttribute]
    public class WorkGroupInfo
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
        public string ProvinceName { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int CityID { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string CityName { get; set; }

        /// <summary>
        ///
        /// </summary>
        public bool Enable { get; set; }

        /// <summary>
        /// 工作组类型
        /// </summary>
        public int Type { get; set; }
    }
}
