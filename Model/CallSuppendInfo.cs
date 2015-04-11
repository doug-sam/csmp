using System;

namespace CSMP.Model
{
    [SerializableAttribute]
    public class CallSuppendInfo
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
///
/// </summary>
public DateTime DateStart { get; set; }

/// <summary>
///
/// </summary>
public DateTime DateEnd { get; set; }

/// <summary>
///
/// </summary>
public string Reason { get; set; }

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
public DateTime AddDate { get; set; }


    }
}
