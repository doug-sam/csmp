using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSMP.Model
{
    public class LeftMenuData
    {
        /// <summary>
        ///等待安排上门
        /// </summary>
        public int ToBeOnSite { get; set; }

        /// <summary>
        ///未处理
        /// </summary>
        public int ToBeDisposed { get; set; }

        /// <summary>
        ///处理中
        /// </summary>
        public int Disposing { get; set; }

        /// <summary>
        ///已完成
        /// </summary>
        public int Complete { get; set; }
        /// <summary>
        ///已关闭
        /// </summary>
        public int Closed { get; set; }
        /// <summary>
        ///店铺催促
        /// </summary>
        public int StoreUrgency { get; set; }

        /// <summary>
        ///店铺请求
        /// </summary>
        public int StoreRequest { get; set; }
        /// <summary>
        ///用户ID
        /// </summary>
        public int UserID { get; set; }
        /// <summary>
        ///用户所属组ID
        /// </summary>
        public int WorkGroupID { get; set; }

        /// <summary>
        ///是否有权限
        /// </summary>
        public bool HaveGroupPower { get; set; }
    }
}
