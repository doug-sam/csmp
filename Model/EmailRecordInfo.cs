using System;

namespace CSMP.Model
{
    [SerializableAttribute]
    public class EmailRecordInfo
    {
		/// <summary>
///
/// </summary>
public int ID { get; set; }

/// <summary>
///
/// </summary>
public int UserID { get; set; }

/// <summary>
///
/// </summary>
public string UserName { get; set; }

/// <summary>
///
/// </summary>
public string CallNo { get; set; }

/// <summary>
///
/// </summary>
public int CallID { get; set; }

/// <summary>
///
/// </summary>
public DateTime DateAdd { get; set; }

/// <summary>
///
/// </summary>
public string ToUser { get; set; }


    }
}
