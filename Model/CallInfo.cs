using System;
using System.Text;

namespace CSMP.Model
{
    [SerializableAttribute]
    public class CallInfo
    {
        public CallInfo()
        {
            ReplacementStatus = (int)SysEnum.ReplacementStatus.没有备件跟进;
            SLADateEnd = DicInfo.DateZone;
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
        public int CreatorID { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string CreatorName { get; set; }

        /// <summary>
        ///
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int CustomerID { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string ErrorReportUser { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int BrandID { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string BrandName { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int ProvinceID { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string ProvinceName { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int CityID { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string CityName { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int StoreID { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string StoreName { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string ReporterName { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int ReportSourceID { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string ReportSourceName { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string ReportSourceNo { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int Class1 { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int Class2 { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int Class3 { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string ClassName1 { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string ClassName2 { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string ClassName3 { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int PriorityID { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string PriorityName { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string Details { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int MaintainUserID { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string MaintaimUserName { get; set; }

        /// <summary>
        ///
        /// </summary>
        public DateTime ErrorDate { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int StateMain { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int StateDetail { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int SuggestSlnID { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string SuggestSlnName { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int SlnID { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string SlnName { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string SloveBy { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int SLA { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string StoreNo { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int SLAMinute { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int WorkGroupID { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int AssignUserID { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int AssignID { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string AssignUserName { get; set; }

        /// <summary>
        ///
        /// </summary>
        public bool IsClosed { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string CallNo2 { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string CallNo3 { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string VideoSrc { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string VideoID { get; set; }

        /// <summary>
        ///
        /// </summary>
        public DateTime FinishDate { get; set; }

        /// <summary>
        ///
        /// </summary>
        public bool IsSameCall { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string SLA2 { get; set; }

        public int ReplacementStatus { get; set; }

        /// <summary>
        /// sla结束日期。insert 时赋默认错误时间。后边有触发器计算这个值
        /// </summary>
        public DateTime SLADateEnd { get; set; }

        public int Category { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("\n单号:").Append(No);
            sb.Append("\nID:").Append(ID);
            sb.Append("\n创建人ID:").Append(CreatorID);
            sb.Append("\n创建人:").Append(CreatorName);            
            sb.Append("\n创建日期:").Append(CreateDate);
            sb.Append("\n客户ID:").Append(CustomerID);
            sb.Append("\n客户名:").Append(CustomerName);
            sb.Append("\n报修人:").Append(ErrorReportUser);
            sb.Append("\n品牌ID:").Append(BrandID);
            sb.Append("\n品牌名:").Append(BrandName);
            sb.Append("\n省份:").Append(ProvinceName);
            sb.Append("\n城市:").Append(CityName);
            sb.Append("\n店铺ID:").Append(StoreID);
            sb.Append("\n店铺名:").Append(StoreName);
            sb.Append("\n店铺编号:").Append(StoreNo);
            sb.Append("\n报修人名:").Append(ReporterName);
            sb.Append("\n报修源:").Append(ReportSourceNo);
            sb.Append("\n大类故障ID:").Append(Class1);
            sb.Append("\n大类故障名:").Append(ClassName1);
            sb.Append("\n中类故障ID:").Append(Class2);
            sb.Append("\n中类故障名:").Append(ClassName2);
            sb.Append("\n小类故障ID:").Append(Class3);
            sb.Append("\n小类故障名:").Append(ClassName3);
            sb.Append("\n优先级:").Append(PriorityName);
            sb.Append("\n报修详细:").Append(Details);
            sb.Append("\n二线负责人ID:").Append(MaintainUserID);
            sb.Append("\n二线负责人名:").Append(MaintaimUserName);
            sb.Append("\n报修日期:").Append(ErrorDate);
            sb.Append("\n当前状态:").Append(StateMain);
            sb.Append("\n状态详细:").Append(StateDetail);
            sb.Append("\n解决方案ID:").Append(SlnID);
            sb.Append("\n解决方案名:").Append(SlnName);
            sb.Append("\n解决方法:").Append(SloveBy);
            sb.Append("\nSLA:").Append(SLA);
            sb.Append("\n所在工作组ID:").Append(WorkGroupID);
            sb.Append("\n异地转派人ID:").Append(AssignUserID);
            sb.Append("\n异地转派人名:").Append(AssignUserName);
            sb.Append("\nCallNo2").Append(CallNo2);
            sb.Append("\nCallNo3:").Append(CallNo3);
            sb.Append("\n录音地址:").Append(VideoSrc);
            sb.Append("\n录音iD:").Append(VideoID);
            sb.Append("\n完成日期:").Append(FinishDate);
            sb.Append("\n是否为重复报修:").Append(IsSameCall);
            return sb.ToString();
        }
    }
}
