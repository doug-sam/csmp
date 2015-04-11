using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSMP.BLL;
using CSMP.Model;
using Tool;
using System.Threading;

public partial class page_Call_AddMany : _Call_AddMany
{
    private Random rand = new Random();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DdlCustomer.DataSource = CustomersBLL.GetList(CurrentUser);
            DdlCustomer.DataBind();
            DdlCustomer.Items.Insert(0, new ListItem("不限", "0"));
            DdlProvince.DataSource = ProvincesBLL.GetList();
            DdlProvince.DataBind();
            DdlProvince.Items.Insert(0, new ListItem("不限", "0"));

            DdlCategory.DataSource = CallCategoryBLL.GetListEnable();
            DdlCategory.DataBind();

            int BrandID = Function.GetRequestInt("BrandID");
            if (BrandID > 0)
            {
                BrandInfo binfo = BrandBLL.Get(BrandID);
                if (binfo != null)
                {
                    Sch(sender, e);
                    ddlL2Id.DataSource = UserBLL.GetList(CurrentUser.WorkGroupID, SysEnum.Rule.二线.ToString());
                    ddlL2Id.DataBind();
                    ddlL2Id.Items.Insert(0, new ListItem("请选择", "0"));

                    ddlReportSource.DataSource = ReportSourceBLL.GetList();
                    ddlReportSource.DataBind();
                    TxtErrorDate.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                    ddlClass1.DataSource = Class1BLL.GetList(binfo.CustomerID);
                    ddlClass1.DataBind();
                    ddlClass1.Items.Insert(0, new ListItem("请选择", "0"));
                    BtnSubmit.Visible = true;
                }
            }
        }
    }

    private void Sch(object sender, EventArgs e)
    {
        int PageSize = 9999999;  //每页记录数
        int PageIndex = 1;  //当前页码
        int page = 10;      //分页显示数
        int count = 0;     //记录总数
        string url = "";
        string strWhere = " 1=1 and f_IsClosed=0 ";
        SchWd(ref url, ref strWhere);

        SchCustomerBrand(sender, e, ref url, ref strWhere);

        SchProvinceCity(sender, e, ref url, ref strWhere);

        strWhere += " order by ID desc ";
        GridView1.DataSource = StoresBLL.GetList(PageSize, PageIndex, strWhere, out count);
        GridView1.DataBind();
        LtlAllCount.Text = count.ToString();
    }

    private void SchProvinceCity(object sender, EventArgs e, ref string url, ref string strWhere)
    {
        int ProvinceID = Function.GetRequestInt("ProvinceID");
        if (ProvinceID > 0)
        {
            DdlProvince.SelectedValue = ProvinceID.ToString();
            DdlProvince_SelectedIndexChanged(sender, e);
            url += "&ProvinceID=" + ProvinceID;
            strWhere += string.Format(" and f_ProvinceID={0} ", ProvinceID);
            int CityID = Function.GetRequestInt("CityID");
            if (CityID > 0)
            {
                DdlCity.SelectedValue = CityID.ToString();
                strWhere += string.Format(" and f_CityID={0} ", CityID);
                url += "&CityID=" + CityID;
            }
        }
    }

    private void SchCustomerBrand(object sender, EventArgs e, ref string url, ref string strWhere)
    {
        int CustomerID = Function.GetRequestInt("CustomerID");
        if (CustomerID > 0)
        {
            DdlCustomer.SelectedValue = CustomerID.ToString();
            DdlCustomer_SelectedIndexChanged(sender, e);
            url += "&CustomerID=" + CustomerID;
            strWhere += string.Format(" and f_CustomerID={0} ", CustomerID);

            int BrandID = Function.GetRequestInt("BrandID");
            if (BrandID > 0)
            {
                DdlBrand.SelectedValue = BrandID.ToString();
                strWhere += string.Format(" and f_BrandID={0} ", BrandID);
                url += "&BrandID=" + BrandID;
            }
        }
    }

    private void SchWd(ref string url, ref string strWhere)
    {
        string wd = Function.GetRequestSrtring("wd");
        if (!string.IsNullOrEmpty(wd))
        {
            strWhere += " and (";
            strWhere += " f_Name like '%" + Function.ClearText(wd) + "%' ";
            strWhere += " or f_No like '%" + Function.ClearText(wd) + "%' ";
            strWhere += " ) ";
            url += "&wd=" + wd;
            TxbWd.Text = wd;
        }
        string Tel = Function.GetRequestSrtring("Tel");
        if (!string.IsNullOrEmpty(Tel))
        {
            strWhere += " and f_Tel like '%" + Function.ClearText(Tel) + "%' ";
            url += "&Tel=" + Tel;
            TxbTel.Text = Tel;
        }
    }



    protected void BtnSch_Click(object sender, EventArgs e)
    {
        if (Function.ConverToInt(DdlBrand.SelectedValue) <= 0)
        {
            Function.AlertBack("请选择品牌，必需选品牌作为批量报修的筛选条件");
            return;
        }

        string Url = "addmany.aspx";
        Url += "?wd=" + TxbWd.Text.Trim();
        Url += "&Tel=" + TxbTel.Text.Trim();
        Url += "&ProvinceID=" + DdlProvince.SelectedValue;
        Url += "&CityID=" + DdlCity.SelectedValue;
        Url += "&CustomerID=" + DdlCustomer.SelectedValue;
        Url += "&BrandID=" + DdlBrand.SelectedValue;
        Response.Redirect(Url);
    }
    protected void DdlProvince_SelectedIndexChanged(object sender, EventArgs e)
    {
        int ID = Convert.ToInt16(DdlProvince.SelectedValue);
        DdlCity.Enabled = ID > 0;
        if (ID <= 0)
        {
            DdlCity.DataSource = null;
            DdlCity.DataBind();
        }
        else
        {
            DdlCity.DataSource = CityBLL.GetList(ID);
            DdlCity.DataBind();
            DdlCity.Items.Insert(0, new ListItem("不限", "0"));
        }
    }


    protected void DdlCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        int ID = Function.ConverToInt(DdlCustomer.SelectedValue);
        if (ID <= 0)
        {
            DdlBrand.DataSource = null;
            DdlBrand.DataBind();
        }
        else
        {
            DdlBrand.DataSource = BrandBLL.GetList(ID);
            DdlBrand.DataBind();
            DdlBrand.Items.Insert(0, new ListItem("不限", "0"));
        }
    }
    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        #region 数据验证

        Class3Info c3info = Class3BLL.Get(Function.ConverToInt(ddlClass3.SelectedValue));
        if (c3info == null)
        {
            Function.AlertMsg("请认真选择报修的大、中、小类"); return;
        }
        Class2Info c2info = Class2BLL.Get(c3info.Class2ID);
        if (c2info == null)
        {
            Function.AlertMsg("请认真选择报修的大、中、小类"); return;
        }
        Class1Info c1info = Class1BLL.Get(c2info.Class1ID);
        if (c1info == null)
        {
            Function.AlertMsg("请认真选择报修的大、中、小类"); return;
        }
        UserInfo uinfo = UserBLL.Get(Function.ConverToInt(ddlL2Id.SelectedValue));
        if (uinfo == null)
        {
            Function.AlertMsg("请选择技术工程师"); return;
        }

        if (ddlReportSource.SelectedValue == "0")
        {
            Function.AlertMsg("请选择报修源"); return;
        }
        if (txtSourceNo.Text.Length > 100)
        {
            Function.AlertMsg("源单号不能多于100个字"); return;
        }
        if (Function.ConverToDateTime(TxtErrorDate.Text) == Tool.Function.ErrorDate)
        {
            Function.AlertMsg("报修日期有误"); return;
        }
        if (TxbSLA2.Text.Trim().Length > 50)
        {
            Function.AlertMsg("扩展SLA不能超50字"); return;
        }
        if (Function.ConverToInt(TxbSLA.Text.Trim()) < 0)
        {
            Function.AlertMsg("SLA需要是一个正整数"); return;
        }

        #endregion


        string delList = Function.GetRequestSrtring("ckDel");
        if (string.IsNullOrEmpty(delList))
        {
            Function.AlertBack("没有选中数据");
            return;
        }
        StoreInfo sinfo = null;
        int FlagSuccess = 0;
        int FlagFailed = 0;
        foreach (string item in delList.Split(','))
        {
            if (item.Length > 0)
            {
                sinfo = StoresBLL.Get(Function.ConverToInt(item));
                if (sinfo == null)
                {
                    continue;
                }
                CallInfo info = new CallInfo();
                info.CreatorID = CurrentUserID;
                info.CreatorName = CurrentUserName;
                info.CreateDate = DateTime.Now;
                info.No = CallBLL.GetCallNoNew(); ;
                info.ErrorReportUser = "系统批量报修";
                info.CustomerID = sinfo.CustomerID;
                info.CustomerName = sinfo.CustomerName;
                info.BrandID = sinfo.BrandID;
                info.BrandName = sinfo.BrandName;
                info.ProvinceID = sinfo.ProvinceID;
                info.ProvinceName = sinfo.ProvinceName;
                info.CityID = sinfo.CityID;
                info.CityName = sinfo.CityName;
                info.StoreID = sinfo.ID;
                info.StoreName = sinfo.No;
                info.StoreNo = sinfo.Name;
                info.ReportSourceID = Function.ConverToInt(ddlReportSource.SelectedValue);
                info.ReportSourceName = ddlReportSource.SelectedItem.Text;
                info.ReportSourceNo = txtSourceNo.Text.Trim();
                info.ReporterName = "系统批量报修";
                info.ErrorDate = Function.ConverToDateTime(TxtErrorDate.Text);
                info.Class1 = c1info.ID;
                info.Class2 = c2info.ID;
                info.Class3 = c3info.ID;
                info.ClassName1 = c1info.Name;
                info.ClassName2 = c2info.Name;
                info.ClassName3 = c3info.Name;
                info.PriorityID = c3info.PriorityID;
                info.PriorityName = c3info.PriorityName;
                info.Details = TxtDetails.Text.Trim();
                info.MaintainUserID = uinfo.ID;
                info.MaintaimUserName = uinfo.Name;
                info.StateMain = (int)SysEnum.CallStateMain.未处理;
                info.StateDetail = (int)SysEnum.CallStateDetails.系统接单_未处理;
                info.SuggestSlnID = 0;
                info.SuggestSlnName = "";
                info.SlnID = 0;
                info.SlnName = "";
                info.SLA = Function.ConverToInt(TxbSLA.Text.Trim(), 0);
                info.SloveBy = "";
                info.IsClosed = false;
                info.AssignUserID = 0;
                info.AssignUserName = string.Empty;
                info.Category = Function.ConverToInt(DdlCategory.SelectedValue, 0);

                #region 扩展信息
                string VideoID = "";//这是一个录音id
                info.VideoID = VideoID;
                info.CallNo2 = "";
                info.CallNo3 = "";
                info.VideoSrc = "";
                info.FinishDate = Tool.Function.ErrorDate;
                info.SLA2 = TxbSLA2.Text.Trim();

                #endregion

                if (CallBLL.Add(info) > 0)
                {
                    FlagSuccess++;
                }
                else
                {
                    FlagFailed++;
                }

            }
        }
        Function.AlertRefresh(string.Format("{0}条数据添加成功，{1}条数据添加失败", FlagSuccess, FlagFailed));
    }

    protected void ddlClass1_SelectedIndexChanged(object sender, EventArgs e)
    {
        int ID = Convert.ToInt16(ddlClass1.SelectedValue);
        if (ID > 0)
        {

            ddlClass2.DataSource = Class2BLL.GetList(ID);
            ddlClass2.DataBind();
            ddlClass2.Items.Insert(0, new ListItem("请选择", "0"));
            ddlClass2.Enabled = true;
        }
        else
        {
            ddlClass2.DataSource = null;
            ddlClass2.DataBind();
        }
    }

    protected void ddlClass2_SelectedIndexChanged(object sender, EventArgs e)
    {
        int ID = Convert.ToInt16(ddlClass2.SelectedValue);
        if (ID > 0)
        {
            ddlClass3.DataSource = Class3BLL.GetList(ID);
            ddlClass3.DataBind();
            ddlClass3.Items.Insert(0, new ListItem("请选择", "0"));
            ddlClass3.Enabled = true;
        }
        else
        {
            ddlClass3.DataSource = null;
            ddlClass3.DataBind();
        }
    }

    protected void ddlClass3_SelectedIndexChanged(object sender, EventArgs e)
    {
        int ID = Convert.ToInt16(ddlClass3.SelectedValue);
        if (ID > 0)
        {
            Class3Info info = Class3BLL.Get(ID);
            if (null != info)
            {
                PrioritiesInfo pinfo = PrioritiesBLL.Get(info.PriorityID);
                if (pinfo != null)
                {
                    LtlPriority.Text = pinfo.Name;
                    TxbSLA.Text = info.SLA.ToString();
                    TxtDetails.Text = info.Name;
                }
            }
        }
    }


}
