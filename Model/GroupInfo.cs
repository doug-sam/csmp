using System;
using System.Collections.Generic;

namespace CSMP.Model
{
    [SerializableAttribute]
    public class GroupInfo
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
        public int LeaderID { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string LeaderName { get; set; }

        /// <summary>
        ///
        /// </summary>
        public bool Enable { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string PowerList { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int CityID { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string CityName { get; set; }

        /// <summary>
        /// 在系统中的角色
        /// </summary>
        public List<string> Rule { get; set; }
        /// <summary>
        /// 指定状态下所需显示字段
        /// </summary>
        public string ItemList { get; set; }

        /// <summary>
        /// 查询页中所需显示字段
        /// </summary>
        public string ItemList2 { get; set; }



    }
}
