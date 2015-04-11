using System;
using System.Collections.Generic;

namespace CSMP.Model
{
    [SerializableAttribute]
    public class PunchInViewInfo
    {
        public DateTime DateAdd { get; set; }
        public List<PunchInItemInfo> ItemInfo { get; set; }
    }
    [SerializableAttribute]
    public class PunchInItemInfo
    {
        public string Detail { get; set; }
        public int IsStartWork { get; set; }
        public int ID { get; set; }
        public string PositionAddress { get; set; }
        public string Memo { get; set; }
    }


    [SerializableAttribute]
    public class PunchInInfo
    {
        /// <summary>
        ///
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        ///填写人id
        /// </summary>
        public int AddByUserID { get; set; }

        /// <summary>
        ///填写人名
        /// </summary>
        public string AddByUserName { get; set; }

        /// <summary>
        ///登记人id
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        ///登记人名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        ///所在工作组
        /// </summary>
        public int GroupID { get; set; }

        /// <summary>
        ///系统记录日期
        /// </summary>
        public DateTime DateAdd { get; set; }

        /// <summary>
        ///用户声称日期
        /// </summary>
        public DateTime DateRegister { get; set; }

        /// <summary>
        ///HR表决日期，默认为AddDate
        /// </summary>
        public DateTime DateRegisterAbs { get; set; }

        /// <summary>
        ///使用设备
        /// </summary>
        public string Device { get; set; }

        /// <summary>
        ///登记人说明
        /// </summary>
        public string Memo { get; set; }

        /// <summary>
        ///是否为上班打卡。1为上班打卡，2为下班打卡，3为其它
        /// </summary>
        public int IsStartWork { get; set; }

        /// <summary>
        ///登记位置地址
        /// </summary>
        public string PositionAddress { get; set; }

        /// <summary>
        ///登记位置经度
        /// </summary>
        public decimal PositionLog { get; set; }

        /// <summary>
        ///登记地址纬度
        /// </summary>
        public decimal PositionLat { get; set; }

        /// <summary>
        ///使用设备详细信息
        /// </summary>
        public string DeviceDetail { get; set; }


    }
}
