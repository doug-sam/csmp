using System;

namespace CSMP.Model
{
    [SerializableAttribute]
    public class loginlogInfo
    {
		/// <summary>
///
/// </summary>
public int ID { get; set; }

/// <summary>
///
/// </summary>
public string openid { get; set; }

/// <summary>
///
/// </summary>
public DateTime logintime { get; set; }

/// <summary>
///
/// </summary>
public string loginlocation { get; set; }

/// <summary>
///
/// </summary>
public DateTime logouttime { get; set; }

/// <summary>
///
/// </summary>
public string logoutlocation { get; set; }

/// <summary>
///
/// </summary>
public int status { get; set; }


    }
}
