using System;

namespace CSMP.Model
{
    [SerializableAttribute]
    public class OperationRecInfo
    {
		/// <summary>
///
/// </summary>
public int ID { get; set; }

/// <summary>
///操作单号
/// </summary>
public int CallID { get; set; }

/// <summary>
///操作人id
/// </summary>
public int UserID { get; set; }

/// <summary>
///操作人名
/// </summary>
public string UserName { get; set; }

/// <summary>
///日志类型
/// </summary>
public string LogType { get; set; }

/// <summary>
///概要（备用）
/// </summary>
public string Memo { get; set; }

/// <summary>
///备用字段
/// </summary>
public int FlagID { get; set; }

/// <summary>
///详细信息
/// </summary>
public string Details { get; set; }

/// <summary>
///操作时间
/// </summary>
public DateTime AddDate { get; set; }


    }
}
