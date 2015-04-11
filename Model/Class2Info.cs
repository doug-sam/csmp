using System;

namespace CSMP.Model
{
    [SerializableAttribute]
    public class Class2Info
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
///大类id
/// </summary>
public int Class1ID { get; set; }

/// <summary>
///大类名
/// </summary>
public string Class1Name { get; set; }

/// <summary>
///
/// </summary>
public bool IsClosed { get; set; }


    }
}
