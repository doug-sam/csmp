using System;

namespace CSMP.Model
{
    [SerializableAttribute]
    public class FeedbackChooseInfo
    {
		/// <summary>
///
/// </summary>
public int ID { get; set; }

/// <summary>
///选择项名称
/// </summary>
public string Name { get; set; }

/// <summary>
///所属问题
/// </summary>
public int QuestionID { get; set; }

/// <summary>
///添加日期
/// </summary>
public DateTime AddDate { get; set; }

/// <summary>
///是否默认选中
/// </summary>
public bool IsDefault { get; set; }

/// <summary>
///排序号，越大越靠前
/// </summary>
public int OrderNumber { get; set; }

/// <summary>
///
/// </summary>
public bool Enable { get; set; }


    }
}
