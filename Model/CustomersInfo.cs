using System;

namespace CSMP.Model
{
    [SerializableAttribute]
    public class CustomersInfo
    {
        /// <summary>
        ///
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        ///客户名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///联系人
        /// </summary>
        public string Contact { get; set; }

        /// <summary>
        ///联系人手机
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        ///联系人邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        ///所在城市的id
        /// </summary>
        public int CityID { get; set; }

        /// <summary>
        ///
        /// </summary>
        public bool IsClosed { get; set; }


    }
}
