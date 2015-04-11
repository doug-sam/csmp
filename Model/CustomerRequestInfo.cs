using System;

namespace CSMP.Model
{
    [SerializableAttribute]
    public class CustomerRequestInfo
    {
		/// <summary>
///
/// </summary>
public int ID { get; set; }

/// <summary>
///添加记录人员
/// </summary>
public int UserID { get; set; }

/// <summary>
///添加记录人员名
/// </summary>
public string UserName { get; set; }

/// <summary>
///报修id。如果有转成报修，则提醒，否则为0
/// </summary>
public int CallID { get; set; }

/// <summary>
///记录添加时间
/// </summary>
public DateTime AddDate { get; set; }

/// <summary>
///报修时间
/// </summary>
public DateTime ErrorReportDate { get; set; }

/// <summary>
///
/// </summary>
public int BrandID { get; set; }

/// <summary>
///报修店铺
/// </summary>
public int StoreID { get; set; }

/// <summary>
///店铺编号
/// </summary>
public string StoreNo { get; set; }

/// <summary>
///店铺名
/// </summary>
public string StoreName { get; set; }

/// <summary>
///报修人id
/// </summary>
public int ErrorReportUserID { get; set; }

/// <summary>
///报修人名
/// </summary>
public string ErrorReportUserName { get; set; }

/// <summary>
///报修详细
/// </summary>
public string Details { get; set; }

/// <summary>
///是否可用
/// </summary>
public bool Enable { get; set; }


    }
}
