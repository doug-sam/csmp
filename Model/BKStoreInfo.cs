using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSMP.Model
{
    /// <summary>
    /// 汉堡王转呈系统店铺信息数据同步专用店铺类
    /// </summary>
    public class BKStoreInfo
    {
        /// <summary>
        ///主键ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 全球编号
        /// </summary>
        public string GlobalCode { get; set; }
        /// <summary>
        /// 本地编号
        /// </summary>
        public string LocalCode { get; set; }
        /// <summary>
        /// 地区
        /// </summary>
        public string Region { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string StoreType { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 开业时间
        /// </summary>
        public DateTime OpenDate { get; set; }
        /// <summary>
        /// 关业时间
        /// </summary>
        public DateTime CloseDate { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string Tel { get; set; }
        /// <summary>
        /// 传真
        /// </summary>
        public string FAX { get; set; }
        /// <summary>
        /// 电子邮箱地址
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// OM
        /// </summary>
        public string OM { get; set; }
        /// <summary>
        /// OC
        /// </summary>
        public string OC { get; set; }
        /// <summary>
        /// 经理房IP
        /// </summary>
        public string ServerIP { get; set; }
        /// <summary>
        /// POS IP段
        /// </summary>
        public string LANGateway { get; set; }
        /// <summary>
        /// TSI
        /// </summary>
        public string TSI { get; set; }
        /// <summary>
        /// 宽带用户名
        /// </summary>
        public string BroadbandUsername { get; set; }
        /// <summary>
        /// 宽带密码
        /// </summary>
        public string BroadbandPWD { get; set; }
        /// <summary>
        /// Teamview账户
        /// </summary>
        public string Teamviewer { get; set; }
        /// <summary>
        /// 菜单面包
        /// </summary>
        public string MenuBread { get; set; }
        /// <summary>
        /// 价格T次
        /// </summary>
        public string PriceTier { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public string Longitude { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public string Latitude { get; set; }
    }
}
