using System;

namespace CSMP.Model
{
    [SerializableAttribute]
    public class WorkGroupEmailInfo
    {
        /// <summary>
        ///
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        ///邮箱地址
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        ///显示名
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        ///所属工作组
        /// </summary>
        public int GroupID { get; set; }
        /// <summary>
        ///收件人组
        /// </summary>
        public int EmailGroupID { get; set; }

        /// <summary>
        ///所属品牌
        /// </summary>
        public int BrandID { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int OrderNo { get; set; }

        /// <summary>
        ///
        /// </summary>
        public bool Enable { get; set; }


    }
}
