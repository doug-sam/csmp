using System;

namespace CSMP.Model
{
    [SerializableAttribute]
    public class ManufacturerInfo
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
public string PassWord { get; set; }

/// <summary>
///
/// </summary>
public string Tel { get; set; }

/// <summary>
///
/// </summary>
public int ProvinceID { get; set; }

/// <summary>
///
/// </summary>
public int CityID { get; set; }

/// <summary>
///
/// </summary>
public int WorkGroupID { get; set; }

/// <summary>
///
/// </summary>
public string Type { get; set; }

/// <summary>
///
/// </summary>
public bool Enable { get; set; }


    }
}
