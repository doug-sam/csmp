using System;

namespace CSMP.Model
{
    [SerializableAttribute]
    public class BillRecInfo
    {
		/// <summary>
///
/// </summary>
public int ID { get; set; }

/// <summary>
///店铺号
/// </summary>
public int StoreID { get; set; }

/// <summary>
///报修NO
/// </summary>
public int CallID { get; set; }

/// <summary>
///哪次上门步骤的服务单。因为可能一个call有多次上门
/// </summary>
public int CallStepID { get; set; }

/// <summary>
///服务单号
/// </summary>
public string CallNo { get; set; }

/// <summary>
///服务单地址
/// </summary>
public string Url { get; set; }

/// <summary>
///添加进系统日期
/// </summary>
public DateTime AddDate { get; set; }

/// <summary>
///验证密码
/// </summary>
public string Pwd { get; set; }

/// <summary>
///是否已收到服务单
/// </summary>
public bool Confirm { get; set; }

/// <summary>
///创建人
/// </summary>
public string CreateBy { get; set; }

/// <summary>
///未使用的标记
/// </summary>
public int Flag { get; set; }


    }
}
