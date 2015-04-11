using System;

namespace CSMP.Model
{
    [SerializableAttribute]
    public class SlaModeDetailInfo
    {
		/// <summary>
///
/// </summary>
public int ID { get; set; }

/// <summary>
///
/// </summary>
public int SlaModeID { get; set; }

/// <summary>
///
/// </summary>
public string DayOfWeek { get; set; }

/// <summary>
///
/// </summary>
public DateTime TimerStart { get; set; }

/// <summary>
///
/// </summary>
public DateTime TimeEnd { get; set; }


    }
}
