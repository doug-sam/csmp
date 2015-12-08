using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using CSMP.BLL;
using CSMP.Model;
using Tool;
using System.Web.UI.HtmlControls;

/// <summary>
///的摘要说明
/// </summary>
public class BasePage : System.Web.UI.Page
{
    /// <summary>
    /// 当前登录员工
    /// </summary>
    public UserInfo CurrentUser;
    /// <summary>
    /// 当前登录员工ID
    /// </summary>
    public int CurrentUserID;

    /// <summary>
    /// 当前登录员工登录名
    /// </summary>
    public string CurrentUserName;
    public bool IsAdmin;
    //public SysEnum.Rule CurrentRule;
    public void GoLogin()
    {
        string js = "<script>top.location.href='/login.aspx';</script>";
        Response.Write(js);
        Response.End();
    }

    protected override void OnPreInit(EventArgs e)
    {
        base.OnPreInit(e);
        CurrentUser = UserBLL.GetCurrent();
        if (null == CurrentUser)
        {
            GoLogin();
        }
        CurrentUserID = UserBLL.GetCurrentEmployeeID();
        CurrentUserName = UserBLL.GetCurrentEmployeeName();
        IsAdmin = CurrentUser.Code == DicInfo.Admin;
        //GroupInfo ginfo = GroupBLL.Get(CurrentUser.WorkGroupID);
        //if (null==ginfo)
        //{
        //    GoLogin(); return;
        //}
        if (string.IsNullOrEmpty(CurrentUserName))
        {
            GoLogin(); return;
        }
    }


}
/*-------------------------------------------------------------------------------------------------------------------*/

public class _Call : BasePage
{

    protected override void OnPreInit(EventArgs e)
    {
        base.OnPreInit(e);
        GroupBLL.EnterCheck((int)PowerInfo.PMain.报修管理);
    }

}



public class _Call_Add : _Call
{
    protected void Page_PreRender(object sender, EventArgs e)
    {

        if (!GroupBLL.PowerCheck((int)PowerInfo.P1_Call.新建报修))
        {
            Response.Redirect("/page/store/list.aspx?tel="+Function.GetRequestSrtring("callNumber").Trim('0'));
        }
    }
}

public class _Call_list : _Call
{
    protected void Page_PreRender(object sender, EventArgs e)
    {
        GroupBLL.EnterCheck((int)PowerInfo.PMain.报修管理);
    }
}

public class _Call_Edit : _Call
{

    protected void Page_PreRender(object sender, EventArgs e)
    {
        GroupBLL.EnterCheck((int)PowerInfo.P1_Call.编辑记录);
    }
}

public class _Call_Sch : _Call
{
    protected void Page_PreRender(object sender, EventArgs e)
    {
        GroupBLL.EnterCheck((int)PowerInfo.PMain.报修管理);
    }
}

public class _Call_Sln1 : _Call
{
    /// <summary>
    /// 用户是否有权限处理该报修
    /// </summary>
    /// <param name="CallID"></param>
    public void NotMyCallCheck(int CallID)
    {//这里的if判断顺序很重要的，不要上下动
        CallInfo info = CallBLL.Get(CallID);
        if (IsAdmin)
        {
            return;
        }
        if (info.StateMain == (int)SysEnum.CallStateMain.已完成)
        {
            GroupBLL.EnterCheck(false); return;
        }
        if (null == info)
        {
            GroupBLL.EnterCheck(false); return;
        }



        if (!WorkGroupBrandBLL.HasRelaction(CurrentUser.WorkGroupID, info.BrandID) && !CallBLL.IsCrossWorkGroup(info,CurrentUser))
        {
            GroupBLL.EnterCheck(false); return;
        }
        if (info.MaintainUserID == CurrentUserID || info.CreatorID == CurrentUserID || info.AssignUserID == CurrentUserID)
        {
            return;
        }
        if (CurrentUser.Rule.Contains(SysEnum.Rule.一线.ToString()))
        {
            return;
        }
        if (!GroupBLL.PowerCheck((int)PowerInfo.P1_Call.处理组内所有报修))
        {
            GroupBLL.EnterCheck(false);
        }
        GroupBLL.EnterCheck(true);
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        base.OnPreInit(e);
        GroupBLL.EnterCheck((int)PowerInfo.PMain.报修管理);
    }
}


public class _Call_Step : _Call
{
    /// <summary>
    /// 用户是否有权限处理该报修
    /// </summary>
    /// <param name="CallID"></param>
    public void NotMyCallCheck(int CallID)
    {//这里的if判断顺序很重要的，不要上下动
        CallInfo info = CallBLL.Get(CallID);
        if (IsAdmin)
        {
            return;
        }
        if (info.StateMain == (int)SysEnum.CallStateMain.已关闭)
        {
            GroupBLL.EnterCheck(false); return;
        }
        if (null == info)
        {
            GroupBLL.EnterCheck(false); return;
        }
        if (!WorkGroupBrandBLL.HasRelaction(CurrentUser.WorkGroupID, info.BrandID) && !CallBLL.IsCrossWorkGroup(info, CurrentUser))
        {
            GroupBLL.EnterCheck(false); return;
        }
        if (info.MaintainUserID == CurrentUserID || info.CreatorID == CurrentUserID || info.AssignUserID == CurrentUserID)
        {
            return;
        }
        if (!GroupBLL.PowerCheck((int)PowerInfo.P1_Call.处理组内所有报修))
        {
            GroupBLL.EnterCheck(false);
        }



    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        base.OnPreInit(e);
        GroupBLL.EnterCheck((int)PowerInfo.P1_Call.跟进处理);
    }
}
public class _Call_Trace : _Call
{
    protected void Page_PreRender(object sender, EventArgs e)
    {
        base.OnPreInit(e);
        GroupBLL.EnterCheck((int)PowerInfo.P1_Call.店铺催促);
    }
}
//public class _Call_RockBack : _Call
//{
//    /// <summary>
//    /// 用户是否有权限处理该报修
//    /// </summary>
//    /// <param name="CallID"></param>
//    public void NotMyCallCheck(CallsInfo info)
//    {//这里的if判断顺序很重要的，不要上下动
//        if (IsAdmin)
//        {
//            return;
//        }
//        if (null == info)
//        {
//            GroupBLL.EnterCheck(false); return;
//        }
//        if (!WorkGroupBrandBLL.HasRelaction(CurrentUser.WorkGroupID, info.BrandID))
//        {
//            GroupBLL.EnterCheck(false); return;
//        }
//        if (info.MaintainUserID == CurrentUserID || info.CreatorID == CurrentUserID)
//        {
//            return;
//        }
//        if (!GroupBLL.PowerCheck((int)PowerInfo.P1_Call.处理组内所有报修))
//        {
//            GroupBLL.EnterCheck(false);
//        }
//    }

//    protected void Page_PreRender(object sender, EventArgs e)
//    {
//        base.OnPreInit(e);
//        GroupBLL.EnterCheck((int)PowerInfo.P1_Call.操作回滚);
//    }
//}
public class _Call_Close : _Call
{
    /// <summary>
    /// 用户是否有权限处理该报修
    /// </summary>
    /// <param name="CallID"></param>
    public void NotMyCallCheck(int CallID)
    {//这里的if判断顺序很重要的，不要上下动
        CallInfo info = CallBLL.Get(CallID);
        if (IsAdmin)
        {
            return;
        }
        if (info.StateMain != (int)SysEnum.CallStateMain.已完成)
        {
            GroupBLL.EnterCheck(false); return;
        }
        if (null == info)
        {
            GroupBLL.EnterCheck(false); return;
        }
        if (!WorkGroupBrandBLL.HasRelaction(CurrentUser.WorkGroupID, info.BrandID))
        {
            GroupBLL.EnterCheck(false); return;
        }
        if (info.MaintainUserID == CurrentUserID || info.CreatorID == CurrentUserID)
        {
            return;
        }
        if (GroupBLL.PowerCheck((int)PowerInfo.P1_Call.处理组内所有报修))
        {
            return;
        }
        GroupBLL.EnterCheck(false);
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        base.OnPreInit(e);
        GroupBLL.EnterCheck((int)PowerInfo.P1_Call.关闭报修);
    }
}
public class _Call_Assign : _Call
{
    /// <summary>
    /// 用户是否有权限处理该报修
    /// </summary>
    /// <param name="CallID"></param>
    public void NotMyCallCheck(int CallID)
    {//这里的if判断顺序很重要的，不要上下动
        CallInfo info = CallBLL.Get(CallID);
        if (IsAdmin)
        {
            return;
        }
        if (null == info)
        {
            GroupBLL.EnterCheck(false); return;
        }



        if (!WorkGroupBrandBLL.HasRelaction(CurrentUser.WorkGroupID, info.BrandID) && !CallBLL.IsCrossWorkGroup(info, CurrentUser))
        {
            GroupBLL.EnterCheck(false); return;
        }
        if (info.MaintainUserID == CurrentUserID || info.CreatorID == CurrentUserID||info.AssignUserID==CurrentUserID)
        {
            return;
        }

    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        GroupBLL.EnterCheck((int)PowerInfo.P1_Call.转派);
    }
}
public class _Call_Feedback : _Call
{
    public void NotMyCallCheck(int CallID)
    {//这里的if判断顺序很重要的，不要上下动
        CallInfo info = CallBLL.Get(CallID);
        if (IsAdmin)
        {
            return;
        }
        if (info.StateMain != (int)SysEnum.CallStateMain.已完成)
        {
            GroupBLL.EnterCheck(false); return;
        }
        if (null == info)
        {
            GroupBLL.EnterCheck(false); return;
        }
        if (!WorkGroupBrandBLL.HasRelaction(CurrentUser.WorkGroupID, info.BrandID))
        {
            GroupBLL.EnterCheck(false); return;
        }
        if (info.MaintainUserID == CurrentUserID || info.CreatorID == CurrentUserID)
        {
            return;
        }
        if (GroupBLL.PowerCheck((int)PowerInfo.P1_Call.处理组内所有报修))
        {
            return;
        }
        GroupBLL.EnterCheck(false);
    }


    protected void Page_PreRender(object sender, EventArgs e)
    {
        GroupBLL.EnterCheck((int)PowerInfo.P1_Call.回访);
    }
}
public class _Call_AddMany : _Call
{
    protected void Page_PreRender(object sender, EventArgs e)
    {
        GroupBLL.EnterCheck((int)PowerInfo.P1_Call.批量报修);
    }
}
public class _CallStep_Edit : _Call
{
    public bool CallStepEnableEdit(CallInfo info)
    {
        if (null == info)
        {
            return false;
        }
        if (IsAdmin)
        {
            return true;
        }
        //if (info.StateMain == (int)SysEnum.CallStateMain.已关闭)
        //{
        //    return false;
        //}
        if (!GroupBLL.PowerCheck((int)PowerInfo.P1_Call.处理记录编辑))
        {
            return false;
        }
        if (!WorkGroupBrandBLL.HasRelaction(CurrentUser.WorkGroupID, info.BrandID))
        {
            return false;
        }
        if (info.MaintainUserID == CurrentUserID || info.CreatorID == CurrentUserID)
        {
            return true;
        }
        if (!GroupBLL.PowerCheck((int)PowerInfo.P1_Call.处理组内所有报修))
        {
            return false;
        }
        return true;
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        base.OnPreInit(e);
        GroupBLL.EnterCheck((int)PowerInfo.P1_Call.处理记录编辑);

    }

    /// <summary>
    /// 当前用户能正编辑这个报修
    /// </summary>
    /// <param name="cinfo"></param>
    /// <returns></returns>
    private bool EnableEdit(CallInfo cinfo)
    {
        if (cinfo.StateMain==(int)SysEnum.CallStateMain.已关闭)
        {
            return false;
        }
        if (IsAdmin)
        {
            return true;
        }
        if (!WorkGroupBrandBLL.HasRelaction(CurrentUser.WorkGroupID,cinfo.BrandID))
        {
            return false;
        }
        return true;
    }
}
public class _Call_StepReplacement : _Call
{
    protected void Page_PreRender(object sender, EventArgs e)
    {
        GroupBLL.EnterCheck((int)PowerInfo.P1_Call.备件记录);
    }
    public void NotMyCallCheck(CallInfo info)
    {

        if (IsAdmin)
        {
            return;
        }
        if (null == info)
        {
            GroupBLL.EnterCheck(false); return;
        }



        if (!WorkGroupBrandBLL.HasRelaction(CurrentUser.WorkGroupID, info.BrandID) && !CallBLL.IsCrossWorkGroup(info, CurrentUser))
        {
            GroupBLL.EnterCheck(false); return;
        }
        if (info.MaintainUserID == CurrentUserID || info.CreatorID == CurrentUserID || info.AssignUserID == CurrentUserID)
        {
            return;
        }

    }

}


/*-------------------------------------------------------------------------------------------------------------------*/
/*-------------------------------------------------------------------------------------------------------------------*/
public class _Sys_Log : BasePage
{
    protected void Page_PreRender(object sender, EventArgs e)
    {
        GroupBLL.EnterCheck((int)PowerInfo.P2_Manager.日志管理);
    }
}
public class _Sys_Inport : BasePage
{
    protected void Page_PreRender(object sender, EventArgs e)
    {
        GroupBLL.EnterCheck((int)PowerInfo.P2_Manager.数据导入);
    }
}
public class _Sys_ServerMsg : BasePage
{
    protected void Page_PreRender(object sender, EventArgs e)
    {
        GroupBLL.EnterCheck((int)PowerInfo.P2_Manager.系统信息);
    }
}
public class _Sys_Profile : BasePage
{
    protected void Page_PreRender(object sender, EventArgs e)
    {
        GroupBLL.EnterCheck((int)PowerInfo.P2_Manager.系统设置);
    }
}
public class _Sys_Attachment_Upload : BasePage
{
    protected void Page_PreRender(object sender, EventArgs e)
    {
        GroupBLL.EnterCheck((int)PowerInfo.P2_Manager.文件上传);
    }
}
public class _Sys_Attachment : BasePage
{
    protected void Page_PreRender(object sender, EventArgs e)
    {
        GroupBLL.EnterCheck((int)PowerInfo.P2_Manager.文件查看);
    }
}

/*-------------------------------------------------------------------------------------------------------------------*/
/*-------------------------------------------------------------------------------------------------------------------*/
public class _BaseData_CustomerBrand : BasePage
{
    protected void Page_PreRender(object sender, EventArgs e)
    {
        GroupBLL.EnterCheck((int)PowerInfo.P3_BaseData.客户品牌管理);
    }
}
public class _BaseData_Store : BasePage
{
    protected void Page_PreRender(object sender, EventArgs e)
    {
        bool HavePower = GroupBLL.PowerCheck((int)PowerInfo.P3_BaseData.店铺查看) || GroupBLL.PowerCheck((int)PowerInfo.P3_BaseData.店铺管理);
        GroupBLL.EnterCheck(HavePower);
    }
}
public class _BaseData_CallCategory : BasePage
{
    protected void Page_PreRender(object sender, EventArgs e)
    {
        bool HavePower = GroupBLL.PowerCheck((int)PowerInfo.P3_BaseData.服务类型管理) || GroupBLL.PowerCheck((int)PowerInfo.P3_BaseData.服务类型管理);
        GroupBLL.EnterCheck(HavePower);
    }
}
public class _BaseData_Store_Edit : BasePage
{
    protected void Page_PreRender(object sender, EventArgs e)
    {
        GroupBLL.EnterCheck((int)PowerInfo.P3_BaseData.店铺管理);
    }
}
public class _BaseData_Class : BasePage
{
    /// <summary>
    /// 故障类名为“项目”的则停止页面输出
    /// </summary>
    /// <param name="Name"></param>
    /// <returns></returns>
    protected bool IsProject(string Name)
    {
        if (Name=="项目")
        {
            Response.Write("不能编辑该项");
            Response.End();
            return true;
        }
        return false;
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        GroupBLL.EnterCheck((int)PowerInfo.P3_BaseData.故障类管理);
    }
}
public class _BaseData_ProvinceCity : BasePage
{
    protected void Page_PreRender(object sender, EventArgs e)
    {
        GroupBLL.EnterCheck((int)PowerInfo.P3_BaseData.地区管理);
    }
}
/// <summary>
/// 中继号码管理ZQL 2015 6 1
/// </summary>
public class _BaseData_TrunkNO : BasePage
{
    protected void Page_PreRender(object sender, EventArgs e)
    {
        GroupBLL.EnterCheck((int)PowerInfo.P3_BaseData.中继号码管理);
    }
}

public class _BaseData_Priorities : BasePage
{
    protected void Page_PreRender(object sender, EventArgs e)
    {
        GroupBLL.EnterCheck((int)PowerInfo.P3_BaseData.优先级管理);
    }
}
public class _BaseData_ThirdParty : BasePage
{
    protected void Page_PreRender(object sender, EventArgs e)
    {
        GroupBLL.EnterCheck((int)PowerInfo.P3_BaseData.第三方管理);
    }
}
public class _BaseData_WorkGroupEmail : BasePage
{
    protected void Page_PreRender(object sender, EventArgs e)
    {
        GroupBLL.EnterCheck((int)PowerInfo.P3_BaseData.常用收件人);
    }
}
public class _BaseData_Jobcode : BasePage
{
    protected void Page_PreRender(object sender, EventArgs e)
    {
        GroupBLL.EnterCheck((int)PowerInfo.P3_BaseData.JobCode管理);
    }
}
public class _BaseData_SLAModel : BasePage
{
    protected void Page_PreRender(object sender, EventArgs e)
    {
        GroupBLL.EnterCheck((int)PowerInfo.P3_BaseData.工作周期管理);
    }
}
/*-------------------------------------------------------------------------------------------------------------------*/
public class _User_User : BasePage
{
    protected void Page_PreRender(object sender, EventArgs e)
    {
        bool HavePower = GroupBLL.PowerCheck((int)PowerInfo.P4_User.用户查看) || GroupBLL.PowerCheck((int)PowerInfo.P4_User.用户管理);
        GroupBLL.EnterCheck(HavePower);
    }
}
public class _User_User_Edit : BasePage
{
    protected void Page_PreRender(object sender, EventArgs e)
    {
        GroupBLL.EnterCheck((int)PowerInfo.P4_User.用户管理);
    }
}
public class _User_WorkGroup : BasePage
{
    protected void Page_PreRender(object sender, EventArgs e)
    {
        GroupBLL.EnterCheck((int)PowerInfo.P4_User.工作组管理);
    }
}
public class _User_PowerGroup : BasePage
{
    protected void Page_PreRender(object sender, EventArgs e)
    {
        GroupBLL.EnterCheck((int)PowerInfo.P4_User.权限组管理);
    }
}
public class _User_WorkWorkGroupBrand : BasePage
{
    protected void Page_PreRender(object sender, EventArgs e)
    {
        GroupBLL.EnterCheck((int)PowerInfo.P4_User.组_品牌关系管理);
    }
}
/*-------------------------------------------------------------------------------------------------------------------*/
public class _Report_Report : BasePage
{
    protected void Page_PreRender(object sender, EventArgs e)
    {
        GroupBLL.EnterCheck((int)PowerInfo.PMain.报表);
    }
}
public class _Report_ReportA : _Report_Report
{
    protected void Page_PreRender(object sender, EventArgs e)
    {
        GroupBLL.EnterCheck((int)PowerInfo.P5_Report.城市报修统计);
    }
}
public class _Report_ReportB : _Report_Report
{
    protected void Page_PreRender(object sender, EventArgs e)
    {
        GroupBLL.EnterCheck((int)PowerInfo.P5_Report.城市大类故障);
    }
}
public class _Report_ReportC : _Report_Report
{
    protected void Page_PreRender(object sender, EventArgs e)
    {
        GroupBLL.EnterCheck((int)PowerInfo.P5_Report.时段故障分析);
    }
}
public class _Report_ReportD : _Report_Report
{
    protected void Page_PreRender(object sender, EventArgs e)
    {
        GroupBLL.EnterCheck((int)PowerInfo.P5_Report.pending分析);
    }
}
public class _Report_ReportE : _Report_Report
{
    protected void Page_PreRender(object sender, EventArgs e)
    {
        GroupBLL.EnterCheck((int)PowerInfo.P5_Report.品牌pending分析);
    }
}
public class _Report_ReportF : _Report_Report
{
    protected void Page_PreRender(object sender, EventArgs e)
    {
        GroupBLL.EnterCheck((int)PowerInfo.P5_Report.明细数据);
    }
}
public class _Report_ReportG : _Report_Report
{
    protected void Page_PreRender(object sender, EventArgs e)
    {
        GroupBLL.EnterCheck((int)PowerInfo.P5_Report.品牌_大类故障占比);
    }
}
public class _Report_ReportH : _Report_Report
{
    protected void Page_PreRender(object sender, EventArgs e)
    {
        GroupBLL.EnterCheck((int)PowerInfo.P5_Report.品牌_服务方式占比);
    }
}
public class _Report_ReportI : _Report_Report
{
    protected void Page_PreRender(object sender, EventArgs e)
    {
        GroupBLL.EnterCheck((int)PowerInfo.P5_Report.品牌大中类故障);
    }
}
public class _Report_ReportJ : _Report_Report
{
    protected void Page_PreRender(object sender, EventArgs e)
    {
        GroupBLL.EnterCheck((int)PowerInfo.P5_Report.报修总量走势);
    }
}
public class _Report_ReportK : _Report_Report
{
    protected void Page_PreRender(object sender, EventArgs e)
    {
        GroupBLL.EnterCheck((int)PowerInfo.P5_Report.门店的TOP10);
    }
}
public class _Report_ReportL : _Report_Report
{
    protected void Page_PreRender(object sender, EventArgs e)
    {
        GroupBLL.EnterCheck((int)PowerInfo.P5_Report.二线考核);
    }
}
public class _Report_ReportM : _Report_Report
{
    protected void Page_PreRender(object sender, EventArgs e)
    {
        GroupBLL.EnterCheck((int)PowerInfo.P5_Report.一线考核);
    }
}
/// <summary>
/// 客户报表验证 ZQL2.26
/// </summary>
public class _Report_ReportCDBrand : _Report_Report
{
    protected void Page_PreRender(object sender, EventArgs e)
    {

        GroupBLL.EnterCheck((int)PowerInfo.P5_Report.客户_日报修统计品牌);
    }
}
/// <summary>
/// 客户报表验证 ZQL2.26
/// </summary>
public class _Report_ReportCDClass : _Report_Report
{
    protected void Page_PreRender(object sender, EventArgs e)
    {

        GroupBLL.EnterCheck((int)PowerInfo.P5_Report.客户_日报修统计故障);
    }
}
/// <summary>
/// 客户报表验证 ZQL2.26
/// </summary>
public class _Report_ReportCMBrand : _Report_Report
{
    protected void Page_PreRender(object sender, EventArgs e)
    {

        GroupBLL.EnterCheck((int)PowerInfo.P5_Report.客户_月报修统计品牌);
    }
}
/// <summary>
/// 客户报表验证 ZQL2.26
/// </summary>
public class _Report_ReportCMClass : _Report_Report
{
    protected void Page_PreRender(object sender, EventArgs e)
    {

        GroupBLL.EnterCheck((int)PowerInfo.P5_Report.客户_月报修统计故障);
    }
}
/// <summary>
/// 客户报表验证 ZQL2.26
/// </summary>
public class _Report_ReportCDTDBrand : _Report_Report
{
    protected void Page_PreRender(object sender, EventArgs e)
    {

        GroupBLL.EnterCheck((int)PowerInfo.P5_Report.客户_日期段统计品牌);
    }
}
/// <summary>
/// 客户报表验证 ZQL2.26
/// </summary>
public class _Report_ReportCDTDClass : _Report_Report
{
    protected void Page_PreRender(object sender, EventArgs e)
    {

        GroupBLL.EnterCheck((int)PowerInfo.P5_Report.客户_日期段统计故障);
    }
}

/// <summary>
/// APP使用情况报表验证 ZQL2015.11.26
/// </summary>
public class _Report_APPReport: _Report_Report
{
    protected void Page_PreRender(object sender, EventArgs e)
    {

        GroupBLL.EnterCheck((int)PowerInfo.P5_Report.APP使用情况报表);
    }
}

public class _Report_ReportPMDClass : _Report_Report
{
    protected void Page_PreRender(object sender, EventArgs e)
    {

        GroupBLL.EnterCheck((int)PowerInfo.P5_Report.PM_日报修统计故障);
    }
}
public class _Report_ReportPMDBrand : _Report_Report
{
    protected void Page_PreRender(object sender, EventArgs e)
    {

        GroupBLL.EnterCheck((int)PowerInfo.P5_Report.PM_日报修统计品牌);
    }
}
public class _Report_ReportPMMClass : _Report_Report
{
    protected void Page_PreRender(object sender, EventArgs e)
    {

        GroupBLL.EnterCheck((int)PowerInfo.P5_Report.PM_月报修统计故障);
    }
}
public class _Report_ReportPMMBrand : _Report_Report
{
    protected void Page_PreRender(object sender, EventArgs e)
    {

        GroupBLL.EnterCheck((int)PowerInfo.P5_Report.PM_月报修统计品牌);
    }
}
public class _Report_ReportPMDTDBrand : _Report_Report
{
    protected void Page_PreRender(object sender, EventArgs e)
    {

        GroupBLL.EnterCheck((int)PowerInfo.P5_Report.PM_日期段统计品牌);
    }
}
public class _Report_ReportPMDTDClass : _Report_Report
{
    protected void Page_PreRender(object sender, EventArgs e)
    {

        GroupBLL.EnterCheck((int)PowerInfo.P5_Report.PM_日期段统计故障);
    }
}




public class _Report_PunchIn : BasePage
{
    protected void Page_PreRender(object sender, EventArgs e)
    {
        GroupBLL.EnterCheck((int)PowerInfo.P5_Report.考勤管理);
    }
}
public class _Report_EmailRecord : BasePage
{
    protected void Page_PreRender(object sender, EventArgs e)
    {
        GroupBLL.EnterCheck((int)PowerInfo.P5_Report.派工邮件统计);
    }
}

/*-------------------------------------------------------------------------------------------------------------------*/
public class _Feedback : BasePage
{
    protected void Page_PreRender(object sender, EventArgs e)
    {
        GroupBLL.EnterCheck((int)PowerInfo.P6_KnowledgeBase.回访管理);
    }
}
public class _BaseData_Solution : BasePage
{
    protected void Page_PreRender(object sender, EventArgs e)
    {
        GroupBLL.EnterCheck((int)PowerInfo.P6_KnowledgeBase.解决方案管理);
    }
}
public class _KnowledgeBase_Comment : BasePage
{
    protected void Page_PreRender(object sender, EventArgs e)
    {
        GroupBLL.EnterCheck((int)PowerInfo.P6_KnowledgeBase.评价管理);
    }
}
public class _KnowledgeBase_Library : BasePage
{
    protected void Page_PreRender(object sender, EventArgs e)
    {
        GroupBLL.EnterCheck((int)PowerInfo.P6_KnowledgeBase.知识库查看);
    }
}
  
/*-------------------------------------------------------------------------------------------------------------------*/
public class _CustomerRequest : BasePage
{
    protected void Page_PreRender(object sender, EventArgs e)
    {
        GroupBLL.EnterCheck((int)PowerInfo.PMain.客户请求管理);
    }
}
public class _CustomerRequest_Add : BasePage
{
    protected void Page_PreRender(object sender, EventArgs e)
    {
        GroupBLL.EnterCheck((int)PowerInfo.P7_CustomerRequest.添加请求);
    }
}

public class _CustomerRequest_Edit : BasePage
{
    protected void Page_PreRender(object sender, EventArgs e)
    {
        GroupBLL.EnterCheck((int)PowerInfo.P7_CustomerRequest.编辑请求);
    }
}

/*-------------------------------------------------------------------------------------------------------------------*/
public class AdminPage : BasePage
{
    protected void Page_PreRender(object sender, EventArgs e)
    {

        GroupBLL.EnterCheck(IsAdmin);
    }
}


