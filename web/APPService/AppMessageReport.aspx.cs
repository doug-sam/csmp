using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using CSMP.BLL;
using CSMP.Model;
using Tool;
using System.Text;
using System.Data;


//public partial class APPService_AppMessageReport : System.Web.UI.Page
public partial class APPService_AppMessageReport : _Report_APPReport
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //this.DdMajorUser.DataSource = UserBLL.GetList(0, SysEnum.Rule.现场工程师.ToString().Trim());
            //this.DdMajorUser.DataBind();
            //DdMajorUser.Items.Insert(0, new ListItem("请选择", "0"));

            DdlProvince.DataSource = ProvincesBLL.GetList();
            DdlProvince.DataBind();
            DdlProvince.Items.Insert(0, new ListItem("请选择", "0"));
            //DataTable tblDatas = new DataTable();

            //tblDatas.Columns.Add("MajorUserName", Type.GetType("System.String"));
            //tblDatas.Columns.Add("APPOnsiteCount", Type.GetType("System.String"));
            //tblDatas.Columns.Add("APPLeaveCount", Type.GetType("System.String"));
            //tblDatas.Columns.Add("APPAllCount", Type.GetType("System.String"));
            //tblDatas.Rows.Add(new object[]{"李意辛","2","2","4"});
            //tblDatas.Rows.Add(new object[] { "林华夏", "3", "3", "6" });
            //GridView1.DataSource = tblDatas;
            //GridView1.DataBind();

        }
    }

    protected void BtnSearch_Click(object sender, EventArgs e)
    {
        AspNetPager1.CurrentPageIndex = 1;
        DataBlind();

    }
    protected void AspNetPager1_PageChanged(object sender, EventArgs e)
    {
        //调用绑定分页和GridView
        DataBlind();
    }


    protected void DataBlind()
    {
        DateTime DateBegin = Function.ConverToDateTime(TxtDateBegin.Text.Trim());
        DateTime DateEnd = Function.ConverToDateTime(TxbDateEnd.Text.Trim());
        int provinceID =Function.ConverToInt(DdlProvince.SelectedValue, 0);
        int workgroupID=Function.ConverToInt(DdlWorkGroup.SelectedValue, 0);
        int majorUserID = Function.ConverToInt(DdMajorUser.SelectedValue, 0);
        string majorUserName = this.txtMajorUserName.Text.Trim(); 
        if (DateBegin == Function.ErrorDate)
        {
            Function.AlertMsg("开始日期有误"); return;
        }
        if (DateEnd == Function.ErrorDate)
        {
            Function.AlertMsg("结束日期有误"); return;
        }
        if (DateBegin > DateEnd)
        {
            Function.AlertMsg("开始日期不能大于结束日期"); return;
        }
        //string sqlStr = string.Empty;
        //sqlStr += "select f_majorusername as MajorUserName ";
        //sqlStr += ",SUM(CASE WHEN f_Content='到场签到' THEN 1 Else 0 END) AS APPOnsiteCount ";
        //sqlStr += ",SUM(CASE WHEN f_Content='离场' THEN 1 Else 0 END) AS APPLeaveCount";
        //sqlStr += ",COUNT(*) AS APPAllCount ";
        //sqlStr += "from sys_MarqueeMessageReport ";
        //sqlStr += "WHERE f_ActionTime >'"+DateBegin.ToString("yyyy-MM-dd ")+"00:00:00'";
        //sqlStr += "AND f_ActionTime >'" + DateEnd.ToString("yyyy-MM-dd ") + "00:00:00'";
        //if (majorUserID > 0)
        //{
        //    sqlStr += "AND f_MaintainUserID= "+majorUserID;
        //}
        //sqlStr += "GROUP BY f_majorusername ";
        string DateBeginStr = DateBegin.ToString("yyyy-MM-dd ") + "00:00:00";
        string DateEndStr = DateEnd.ToString("yyyy-MM-dd ") + "23:59:59";
        string MajorUserIDStr = string.Empty;
        if (majorUserID > 0)
        {
            MajorUserIDStr = majorUserID.ToString().Trim();
        }
        DataTable dt = MarqueeMessageReportBLL.GetDailyReportBySP(DateBeginStr, DateEndStr, majorUserName, provinceID, workgroupID, majorUserID);
        //初始化分页数据源实例
        PagedDataSource pds = new PagedDataSource();
        //设置总行数
        AspNetPager1.RecordCount = dt.Rows.Count;

        //设置分页的数据源
        pds.DataSource = dt.DefaultView;
        //设置当前页
        pds.CurrentPageIndex = AspNetPager1.CurrentPageIndex - 1;
        //设置每页显示页数
        pds.PageSize = AspNetPager1.PageSize;
        //启用分页
        pds.AllowPaging = true;
        //设置GridView的数据源为分页数据源
        GridView1.DataSource = pds;
        //绑定GridView
        GridView1.DataBind();
    }


    protected void DdlProvince_SelectedIndexChanged(object sender, EventArgs e)
    {
        int ID = Function.ConverToInt(DdlProvince.SelectedValue);
        if (ID > 0)
        {
            DdlWorkGroup.DataSource = WorkGroupBLL.GetList(ID);
            DdlWorkGroup.DataBind();
            DdlWorkGroup.Items.Insert(0, new ListItem("请选择", "0"));
            DdlWorkGroup.Visible = true;
        }
    }
    protected void DdlWorkGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        int ID = Function.ConverToInt(DdlWorkGroup.SelectedValue);
        if (ID > 0)
        {

            List<UserInfo> list = UserBLL.GetList(ID, SysEnum.Rule.现场工程师.ToString());
            foreach (UserInfo item in list)
            {
                item.Name = Hz2Py.JoinFirstPy(item.Name);
            }
            DdMajorUser.DataSource = list;
            DdMajorUser.DataBind();
            DdMajorUser.Items.Insert(0, new ListItem("请选择", "0"));
        }
    }
}
