using System;

namespace CSMP.Model
{
    [SerializableAttribute]
    public class EmailGroupInfo
    {
		/// <summary>
///
/// </summary>
public int ID { get; set; }

/// <summary>
///邮件组名
/// </summary>
public string Name { get; set; }

/// <summary>
///所属工作组ID
/// </summary>
public int WorkGroupID { get; set; }

/// <summary>
///添加日期
/// </summary>
public DateTime AddDate { get; set; }

/// <summary>
///是否可用
/// </summary>
public bool Enable { get; set; }


    }
}
