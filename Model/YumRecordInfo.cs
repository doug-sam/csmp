using System;

namespace CSMP.Model
{
    [SerializableAttribute]
    public class YumRecordInfo
    {
		/// <summary>
///
/// </summary>
public int ID { get; set; }

/// <summary>
///报修记录表的自增ID
/// </summary>
public int CallID { get; set; }

/// <summary>
///报修表的报修单号
/// </summary>
public int CallNo { get; set; }

/// <summary>
///发送日期
/// </summary>
public DateTime SendDate { get; set; }

/// <summary>
///是否发送成功
/// </summary>
public bool IsSuccess { get; set; }

/// <summary>
///
/// </summary>
public string Step { get; set; }

/// <summary>
///
/// </summary>
public string Flag { get; set; }


    }
}
