using System;

namespace CSMP.Model
{
    [SerializableAttribute]
    public class ThirdPartyInfo
    {
        /// <summary>
        ///
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        ///所属工作组
        /// </summary>
        public int WorkGroupID { get; set; }

        /// <summary>
        ///厂商名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string Contact { get; set; }

        /// <summary>
        ///排序号
        /// </summary>
        public int OrderNo { get; set; }

        /// <summary>
        ///
        /// </summary>
        public bool Enable { get; set; }


    }
}
