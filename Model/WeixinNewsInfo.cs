using System;

namespace CSMP.Model
{
    [SerializableAttribute]
    public class WeixinNewsInfo
    {
		/// <summary>
///
/// </summary>
public int ID { get; set; }

/// <summary>
///
/// </summary>
public int MsgType { get; set; }

/// <summary>
///
/// </summary>
public string Pic { get; set; }

/// <summary>
///
/// </summary>
public string Title { get; set; }

/// <summary>
///
/// </summary>
public string Content { get; set; }

/// <summary>
///
/// </summary>
public string Description { get; set; }

/// <summary>
///
/// </summary>
public string LinkURL { get; set; }

/// <summary>
///
/// </summary>
public string KeyWord { get; set; }

/// <summary>
///
/// </summary>
public bool IsConst { get; set; }

/// <summary>
///
/// </summary>
public int OrderID { get; set; }


    }
}
