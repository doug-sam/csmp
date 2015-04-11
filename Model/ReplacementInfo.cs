using System;

namespace CSMP.Model
{
    [SerializableAttribute]
    public class ReplacementInfo
    {
		/// <summary>
///
/// </summary>
public int ID { get; set; }

/// <summary>
///
/// </summary>
public int CallID { get; set; }

/// <summary>
///状态ID
/// </summary>
public int StateID { get; set; }

/// <summary>
///状态名
/// </summary>
public string StateName { get; set; }

/// <summary>
///负责备件人（具体拿备件的人）ID
/// </summary>
public int MaintainUserID { get; set; }

/// <summary>
///负责备件人（具体拿备件的人）名字
/// </summary>
public string MaintainUserName { get; set; }

/// <summary>
///记录人ID
/// </summary>
public int RecordUserID { get; set; }

/// <summary>
///记录人名字
/// </summary>
public string RecordUserName { get; set; }

/// <summary>
///事发时间
/// </summary>
public DateTime DateAction { get; set; }

/// <summary>
///在系统记录时间
/// </summary>
public DateTime DateAdd { get; set; }

/// <summary>
///详细内容
/// </summary>
public string Detail { get; set; }

/// <summary>
///备件什么牌子
/// </summary>
public string RpBrand { get; set; }

/// <summary>
///备件型号
/// </summary>
public string RpMode { get; set; }

/// <summary>
///备件序列号
/// </summary>
public string RpSerialNo { get; set; }


    }
}
