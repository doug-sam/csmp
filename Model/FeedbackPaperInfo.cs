using System;

namespace CSMP.Model
{
    [SerializableAttribute]
    public class FeedbackPaperInfo
    {
		/// <summary>
///
/// </summary>
public int ID { get; set; }

/// <summary>
///
/// </summary>
public string Name { get; set; }

/// <summary>
///
/// </summary>
public int OrderNumber { get; set; }

/// <summary>
///
/// </summary>
public bool Enable { get; set; }

/// <summary>
///
/// </summary>
public string Memo { get; set; }


    }
}
