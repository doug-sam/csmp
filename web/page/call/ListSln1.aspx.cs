using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSMP.BLL;
using CSMP.Model;
using Tool;
using System.Text;


public partial class page_call_ListSln1 : _Call_list
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {


            DdlProvince.DataSource = ProvincesBLL.GetList();
            DdlProvince.DataBind();
            DdlProvince.Items.Insert(0, new ListItem("不限", "0"));
            DdlCity.Items.Insert(0, new ListItem("不限", "0"));
            DdlL2.DataSource = UserBLL.GetList(CurrentUser.WorkGroupID, SysEnum.Rule.二线.ToString());
            DdlL2.DataBind();
            DdlL2.Items.Insert(0, new ListItem("不限", "0"));
            DdlL1.DataSource = UserBLL.GetList(CurrentUser.WorkGroupID, SysEnum.Rule.一线.ToString());
            DdlL1.DataBind();
            DdlL1.Items.Insert(0, new ListItem("不限", "0"));

            DdlCustomer.DataSource = CustomersBLL.GetList(CurrentUser);
            DdlCustomer.DataBind();
            DdlCustomer.Items.Insert(0, new ListItem("不限", "0"));

            BindData();

        }
    }

    private void BindData()
    {

        int PageSize = 20;  //每页记录数
        int PageIndex = Function.GetRequestInt("page");  //当前页码
        int page = 10;      //分页显示数
        int count = 0;     //记录总数
        string url = "";
        StringBuilder strWhere = new StringBuilder();
        strWhere.Append(" 1=1  ");

        #region 搜索条件

        SchDate(ref url, ref strWhere);

        SchUser(ref url, ref strWhere);

        SchStoreBrand(ref url, ref strWhere);

        SchPrivinceCity(ref url, ref strWhere);
        SchNo(ref url, ref strWhere);

        SchMain(ref url, ref strWhere);

        #endregion



        strWhere.Append(" order by id desc ");
        List<CallInfo> list = CallBLL.GetList(PageSize, PageIndex, strWhere.ToString(), out count);
        GridView1.DataSource = list;
        GridView1.DataBind();
        this.List_Page.Text = Function.Paging3(PageSize, count, page, PageIndex, url);
    }


    #region 搜索条件
    private void SchNo(ref string url, ref StringBuilder strWhere)
    {
        string CallNo = Function.ClearText(Function.GetRequestSrtring("CallNo"));
        if (!string.IsNullOrEmpty(CallNo))
        {
            strWhere.Append(" AND F_No LIKE '%").Append(CallNo).Append("%' ");
            url += "&CallNo=" + CallNo;
            TxbCallNo.Text = CallNo;
        }
    }

    private void SchPrivinceCity(ref string url, ref StringBuilder strWhere)
    {
        int PrivinceID = Function.GetRequestInt("PrivinceID");
        int CityID = Function.GetRequestInt("CityID");
        if (PrivinceID > 0)
        {
            DdlProvince.SelectedValue = PrivinceID.ToString();
            strWhere.Append(" and f_ProvinceID = ").Append(PrivinceID);
            url += "&PrivinceID=" + PrivinceID;
            DdlCity.DataSource = CityBLL.GetList(PrivinceID);
            DdlCity.DataBind();
            DdlCity.Items.Insert(0, new ListItem("不限", "0"));

            if (CityID > 0)
            {
                DdlCity.SelectedValue = CityID.ToString();
                strWhere.Append(" and f_CityID =").Append(CityID);
                url += "&CityID=" + CityID;
            }
        }
    }
    private void SchStoreBrand(ref string url, ref StringBuilder strWhere)
    {
        string StoreNo = Function.ClearText(Function.GetRequestSrtring("StoreNo"));
        if (!string.IsNullOrEmpty(StoreNo))
        {
            TxbStoreNo.Text = StoreNo;
            strWhere.Append(string.Format(" and( f_StoreName  like '%{0}%' or f_StoreNo like '%{0}%' ) ", StoreNo));
            url += "&StoreNo=" + StoreNo;
        }
        int CustomerID = Function.GetRequestInt("CustomerID");
        if (CustomerID > 0)
        {
            DdlCustomer.SelectedValue = CustomerID.ToString();
            DdlCustomer_SelectedIndexChanged(null, null);
            strWhere.Append(" AND f_CustomerID=").Append(CustomerID);
            url += "&CustomerID=" + CustomerID;

            int BrandID = Function.GetRequestInt("BrandID");
            if (BrandID > 0)
            {
                DdlBrand.SelectedValue = BrandID.ToString();
                strWhere.Append(" and f_BrandID =").Append(BrandID);
                url += "&BrandID=" + BrandID;
            }
        }

    }


    private void SchUser(ref string url, ref StringBuilder strWhere)
    {
        int L1 = Function.GetRequestInt("L1");
        if (L1 > 0)
        {
            DdlL1.SelectedValue = L1.ToString();
            strWhere.Append(" and f_CreatorID=").Append(L1);
            url += "&L1=" + L1;
        }
        int L2 = Function.GetRequestInt("L2");
        if (L2 > 0)
        {
            DdlL2.SelectedValue = L2.ToString();
            strWhere.Append(" and f_MaintainUserID=").Append(L2);
            url += "&L2=" + L2;
        }
    }

    private void SchDate(ref string url, ref StringBuilder strWhere)
    {
        DateTime DtBegin = Function.GetRequestDateTime("DtBegin");
        DateTime DtEnd = Function.GetRequestDateTime("DtEnd");
        if (DtBegin > DtEnd)
        {
            Function.AlertBack("开始日期必需小于结束日期");
        }
        if (DtBegin != Tool.Function.ErrorDate)
        {
            TxtDateBegin.Text = DtBegin.ToString("yyyy-MM-dd");
            strWhere.Append(" and DATEDIFF(day,'").Append(DtBegin).Append("',f_ErrorDate)>=0 ");
            url += "&DtBegin=" + TxtDateBegin.Text;
        }
        if (DtEnd != Tool.Function.ErrorDate)
        {
            TxbDateEnd.Text = DtEnd.ToString("yyyy-MM-dd");
            strWhere.Append(" and DATEDIFF(day,'").Append(DtEnd).Append("',f_ErrorDate)<=0 ");
            url += "&DtEnd=" + TxbDateEnd.Text;
        }
    }

    private void SchMain(ref string url, ref StringBuilder strWhere)
    {

        strWhere.Append(" and f_StateMain=").Append((int)SysEnum.CallStateMain.处理中);
        strWhere.Append(" and (f_StateDetail in(").Append((int)SysEnum.CallStateDetails.等待安排上门);
        strWhere.Append(" , ").Append((int)SysEnum.CallStateDetails.等待备件);
        strWhere.Append(") ) ");

            strWhere.Append(" AND (");
                strWhere.Append(" ( f_BrandID in(SELECT f_MID FROM sys_WorkGroupBrand WHERE f_WorkGroupID=").Append(CurrentUser.WorkGroupID).Append(") ");
                strWhere.Append(" )");
               // strWhere.Append(" AND  f_ParentID=0)");
                strWhere.Append(" OR f_WorkGroupID=").Append(CurrentUser.WorkGroupID);
            strWhere.Append(" ) ");
    }
    #endregion


    protected void BtnSch_Click(object sender, EventArgs e)
    {
        string Url = "listSln1.aspx";
        Url += "?State=" + Function.GetRequestInt("State");
        Url += "&DtBegin=" + TxtDateBegin.Text.Trim();
        Url += "&DtEnd=" + TxbDateEnd.Text.Trim();
        Url += "&L2=" + DdlL2.SelectedValue;
        Url += "&L1=" + DdlL1.SelectedValue;
        Url += "&PrivinceID=" + DdlProvince.SelectedValue;
        Url += "&CityID=" + DdlCity.SelectedValue;
        Url += "&StoreNo=" + TxbStoreNo.Text.Trim();
        Url += "&BrandID=" + DdlBrand.SelectedValue;
        Url += "&CustomerID=" + DdlCustomer.SelectedValue;
        Url += "&CallNo=" + TxbCallNo.Text.Trim();


        Response.Redirect(Url);
    }

    #region 异步响应
    protected void DdlProvince_SelectedIndexChanged(object sender, EventArgs e)
    {
        int ID = Function.ConverToInt(DdlProvince.SelectedValue);
        DdlCity.DataSource = (ID > 0) ? CityBLL.GetList(ID) : null;
        DdlCity.DataBind();
        DdlCity.Items.Insert(0, new ListItem("不限", "0"));

    }

    protected void DdlCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        int ID = Function.ConverToInt(DdlCustomer.SelectedValue);
        if (ID > 0)
        {
            DdlBrand.DataSource = BrandBLL.GetList(ID);
            DdlBrand.DataBind();
        }
        else
        {
            DdlBrand.DataSource = null;//TODO::把它改正确过来
            DdlBrand.DataBind();
        }
        DdlBrand.Items.Insert(0, new ListItem("不限", "0"));
    }

    #endregion
}
