using System;

namespace CSMP.Model
{
    [SerializableAttribute]
    public class SlaWarnRecordInfo
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
public int WarnTime { get; set; }

/// <summary>
///
/// </summary>
public string Detail { get; set; }


    }
}
