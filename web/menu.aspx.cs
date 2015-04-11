using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSMP.BLL;
using CSMP.Model;
using Tool;

public partial class menu : BasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            if (GroupBLL.PowerCheck((int)PowerInfo.PMain.报修管理))
            {
                LCallSln1.Visible = GroupBLL.PowerCheck((int)PowerInfo.P1_Call.跟进处理)||GroupBLL.PowerCheck((int)PowerInfo.P1_Call.安排上门);
                LCallAdd.Visible = GroupBLL.PowerCheck((int)PowerInfo.P1_Call.新建报修);
                LCallSch.Visible= GroupBLL.PowerCheck((int)PowerInfo.PMain.报修管理);
                LCallList.Visible = GroupBLL.PowerCheck((int)PowerInfo.P1_Call.查询不同状态报修)&&!CurrentUser.Rule.Contains(SysEnum.Rule.客户.ToString());
                LAddMany.Visible = GroupBLL.PowerCheck((int)PowerInfo.P1_Call.批量报修);
                LCustomerRequest.Visible = GroupBLL.PowerCheck((int)PowerInfo.PMain.客户请求管理);
            }
            else PCall.Visible = false;
            if (GroupBLL.PowerCheck((int)PowerInfo.PMain.系统管理))
            {
                LLog.Visible = GroupBLL.PowerCheck((int)PowerInfo.P2_Manager.日志管理);
                LInport.Visible = GroupBLL.PowerCheck((int)PowerInfo.P2_Manager.数据导入);
                LProfile.Visible = GroupBLL.PowerCheck((int)PowerInfo.P2_Manager.系统设置);
                LServerMsg.Visible = GroupBLL.PowerCheck((int)PowerInfo.P2_Manager.系统信息);
                LAttachment.Visible = GroupBLL.PowerCheck((int)PowerInfo.P2_Manager.文件查看);
            }
            else PSystem.Visible = false;
            if (GroupBLL.PowerCheck((int)PowerInfo.PMain.基础数据管理))
            {
                LCustomerBrand.Visible = GroupBLL.PowerCheck((int)PowerInfo.P3_BaseData.客户品牌管理);
                LProvinceCity.Visible = GroupBLL.PowerCheck((int)PowerInfo.P3_BaseData.地区管理);
                LClass.Visible = GroupBLL.PowerCheck((int)PowerInfo.P3_BaseData.故障类管理);
                LStore.Visible = GroupBLL.PowerCheck((int)PowerInfo.P3_BaseData.店铺查看) || GroupBLL.PowerCheck((int)PowerInfo.P3_BaseData.店铺管理);
                LJobcode.Visible = GroupBLL.PowerCheck((int)PowerInfo.P3_BaseData.JobCode管理);
                LThirdParty.Visible = GroupBLL.PowerCheck((int)PowerInfo.P3_BaseData.第三方管理);
                LWorkGroupEmail.Visible = GroupBLL.PowerCheck((int)PowerInfo.P3_BaseData.常用收件人);
                LAttachment.Visible = GroupBLL.PowerCheck((int)PowerInfo.P3_BaseData.文件管理);
                LSLAModel.Visible = GroupBLL.PowerCheck((int)PowerInfo.P3_BaseData.工作周期管理);
                LCallCategory.Visible = GroupBLL.PowerCheck((int)PowerInfo.P3_BaseData.服务类型管理);
            }
            else PBasedata.Visible = false;
            if (GroupBLL.PowerCheck((int)PowerInfo.PMain.操作人员管理))
            {
                LUser.Visible = GroupBLL.PowerCheck((int)PowerInfo.P4_User.用户查看) || GroupBLL.PowerCheck((int)PowerInfo.P4_User.用户管理);
                LPower.Visible = GroupBLL.PowerCheck((int)PowerInfo.P4_User.权限组管理);
                LWorkGroup.Visible = GroupBLL.PowerCheck((int)PowerInfo.P4_User.工作组管理) ;
                LGroupCustomer.Visible = GroupBLL.PowerCheck((int)PowerInfo.P4_User.组_品牌关系管理);
            }
            else PUser.Visible = false;

            

            if (GroupBLL.PowerCheck((int)PowerInfo.PMain.报表))
            {
                LReportA.Visible = GroupBLL.PowerCheck((int)PowerInfo.P5_Report.城市报修统计);
                LReportB.Visible = GroupBLL.PowerCheck((int)PowerInfo.P5_Report.城市大类故障);
                LReportC.Visible = GroupBLL.PowerCheck((int)PowerInfo.P5_Report.时段故障分析);
                LReportF.Visible = GroupBLL.PowerCheck((int)PowerInfo.P5_Report.明细数据);
                LReportH.Visible = GroupBLL.PowerCheck((int)PowerInfo.P5_Report.品牌_服务方式占比);
                LReportJ.Visible = GroupBLL.PowerCheck((int)PowerInfo.P5_Report.报修总量走势);
                LReportK.Visible = GroupBLL.PowerCheck((int)PowerInfo.P5_Report.门店的TOP10);
                LReportN.Visible = GroupBLL.PowerCheck((int)PowerInfo.P5_Report.品牌pending分析);
                LPunchIn.Visible = GroupBLL.PowerCheck((int)PowerInfo.P5_Report.考勤管理);
                LEmailRecord.Visible = GroupBLL.PowerCheck((int)PowerInfo.P5_Report.派工邮件统计);

                //2.26 ZQL 判断是否查看客户报表
                LiteralL.Visible = GroupBLL.PowerCheck((int)PowerInfo.P5_Report.客户_日报修统计品牌);
                LiteralM.Visible = GroupBLL.PowerCheck((int)PowerInfo.P5_Report.客户_日报修统计故障);
                LiteralP.Visible = GroupBLL.PowerCheck((int)PowerInfo.P5_Report.客户_月报修统计品牌);
                LiteralQ.Visible = GroupBLL.PowerCheck((int)PowerInfo.P5_Report.客户_月报修统计故障);
                LiteralR.Visible = GroupBLL.PowerCheck((int)PowerInfo.P5_Report.客户_日期段统计品牌);
                LiteralS.Visible = GroupBLL.PowerCheck((int)PowerInfo.P5_Report.客户_日期段统计故障);
                //3.2 ZQL 判断是否查看项目经理视觉报表
                LiteralPMDB.Visible = GroupBLL.PowerCheck((int)PowerInfo.P5_Report.PM_日报修统计品牌);
                LiteralPMDC.Visible = GroupBLL.PowerCheck((int)PowerInfo.P5_Report.PM_日报修统计故障);
                LiteralPMMB.Visible = GroupBLL.PowerCheck((int)PowerInfo.P5_Report.PM_月报修统计品牌);
                LiteralPMMC.Visible = GroupBLL.PowerCheck((int)PowerInfo.P5_Report.PM_月报修统计故障);
                LiteralPMDTDB.Visible = GroupBLL.PowerCheck((int)PowerInfo.P5_Report.PM_日期段统计品牌);
                LiteralPMDTDC.Visible = GroupBLL.PowerCheck((int)PowerInfo.P5_Report.PM_日期段统计故障);
               

                //bool showCustomReport = false;
                //string username = CurrentUser.Name;
                //if (CurrentUser.Name == "LVMH" || CurrentUser.Name == "admin")
                //{
                //    showCustomReport = true;
                //}
               
                //LiteralL.Visible = showCustomReport;
                //LiteralM.Visible = showCustomReport;
                //LiteralP.Visible = showCustomReport;
                //LiteralQ.Visible = showCustomReport;
                //LiteralR.Visible = showCustomReport;
                //LiteralS.Visible = showCustomReport;

            }
            else PReport.Visible = false;
            if (GroupBLL.PowerCheck((int)PowerInfo.PMain.知识库))
            {
                LSolution.Visible = GroupBLL.PowerCheck((int)PowerInfo.P6_KnowledgeBase.解决方案管理);
                LFeedback.Visible = GroupBLL.PowerCheck((int)PowerInfo.P6_KnowledgeBase.回访管理);
                LComment.Visible = GroupBLL.PowerCheck((int)PowerInfo.P6_KnowledgeBase.评价管理);
                LKnowLedgeBaseLibrary.Visible = GroupBLL.PowerCheck((int)PowerInfo.P6_KnowledgeBase.知识库查看);
            }
            else PKnowledgeBase.Visible = false;
        }
    }


}
