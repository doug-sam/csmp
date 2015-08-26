using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSMP.Model
{
    public class WebServiceTaskInfo
    {
        /// <summary>
        ///主键ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 单号ID
        /// </summary>
        public string CallNo { get; set; }
        /// <summary>
        ///要发送的参数
        /// </summary>
        public string TaskUrl { get; set; }
        /// <summary>
        /// 是否已执行完成
        /// </summary>
        public bool IsDone { get; set; }
        /// <summary>
        /// 客户ID
        /// </summary>
        public int CustomerID { get; set; }
        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// 品牌ID
        /// </summary>
        public int BrandID { get; set; }
        /// <summary>
        /// 品牌名称
        /// </summary>
        public string BrandName { get; set; }
        /// <summary>
        /// 备注 冗余字段
        /// </summary>
        public string Remark { get; set; }
    }
}
