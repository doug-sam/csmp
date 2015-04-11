using System;

namespace CSMP.Model
{
    [SerializableAttribute]
    public class Class3Info
    {
		/// <summary>
///
/// </summary>
public int ID { get; set; }

/// <summary>
///类名
/// </summary>
public string Name { get; set; }

/// <summary>
///二级类id
/// </summary>
public int Class2ID { get; set; }

/// <summary>
///二级类名
/// </summary>
public string Class2Name { get; set; }

/// <summary>
///优先级id
/// </summary>
public int PriorityID { get; set; }

/// <summary>
///优先级名
/// </summary>
public string PriorityName { get; set; }

/// <summary>
///
/// </summary>
public int SLA { get; set; }

/// <summary>
///
/// </summary>
public bool IsClosed { get; set; }


    }
}
