using System;

namespace CSMP.Model
{
    [SerializableAttribute]
    public class JobcodeInfo
    {
		/// <summary>
///
/// </summary>
public int ID { get; set; }

/// <summary>
///
/// </summary>
public int WorkGroupID { get; set; }

/// <summary>
///
/// </summary>
public string CodeNo { get; set; }

/// <summary>
///
/// </summary>
public decimal Money { get; set; }

/// <summary>
///
/// </summary>
public string TimeAction { get; set; }

/// <summary>
///
/// </summary>
public string TimeArrive { get; set; }


    }
}
