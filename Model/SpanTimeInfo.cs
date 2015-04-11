using System;

namespace CSMP.Model
{
    [SerializableAttribute]
    public class SpanTimeInfo
    {
		/// <summary>
///
/// </summary>
public int ID { get; set; }

/// <summary>
///报修ID
/// </summary>
public int CallID { get; set; }

/// <summary>
///单号
/// </summary>
public string CallNo { get; set; }

/// <summary>
///添加日期
/// </summary>
public DateTime AddDate { get; set; }

/// <summary>
///恢复操作人ID
/// </summary>
public int UserIDStart { get; set; }

/// <summary>
///恢复操作人名
/// </summary>
public string UserNameStart { get; set; }

/// <summary>
///暂时起始点
/// </summary>
public DateTime DateBegin { get; set; }

/// <summary>
///暂停结束点
/// </summary>
public DateTime DateEnd { get; set; }

/// <summary>
///暂停几小时
/// </summary>
public decimal Hours { get; set; }

/// <summary>
///由什么恢复执行
/// </summary>
public int StartupBy { get; set; }

/// <summary>
///暂停原因
/// </summary>
public string Reason { get; set; }

/// <summary>
///备注
/// </summary>
public string Memo { get; set; }

/// <summary>
///实际暂停几分钟
/// </summary>
public int TotalMinutes { get; set; }

/// <summary>
///使其停止人ID
/// </summary>
public int UserIDStop { get; set; }

/// <summary>
///使其停止人名
/// </summary>
public string UserNameStop { get; set; }


    }
}
