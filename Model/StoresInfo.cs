using System;

namespace CSMP.Model
{
    [SerializableAttribute]
    public class StoreInfo
    {
        public StoreInfo()
        {
            this.CustomerID = 0;
            this.CustomerName = string.Empty;
        }

        /// <summary>
        ///
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string No { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int BrandID { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int ProvinceID { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int CityID { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string Tel { get; set; }

        /// <summary>
        ///
        /// </summary>
        public bool IsClosed { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string BrandName { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string ProvinceName { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string CityName { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int CustomerID { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string CustomerName { get; set; }


        public string Email { get; set; }
        /// <summary>
        /// 店铺类型
        /// </summary>
        public string StoreType { get; set; }
        
        /// <summary>
        /// 上次编辑时间
        /// </summary>
        public DateTime AddDate { get; set; }
    }
}
