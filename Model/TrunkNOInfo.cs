using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSMP.Model
{
    [SerializableAttribute]
    public class TrunkNOInfo
    {
        /// <summary>
        ///主键ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        ///物理号码
        /// </summary>
        public string PhysicalNo { get; set; }

        /// <summary>
        ///对外显示号码
        /// </summary>
        public string VirtualNo { get; set; }

        /// <summary>
        ///描述
        /// </summary>
        public string Description { get; set; }
    }
}
