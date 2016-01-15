using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CSMP.Model;
using CSMP.BLL;
using Tool;

public partial class page_KnowledgeBase_ListOnSolutionPage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            Sch();

        }
    }


    private void Sch()
    {
        int PageSize = 20;  //每页记录数
        int PageIndex = Function.GetRequestInt("page");  //当前页码
        int page = 10;      //分页显示数
        int count = 0;     //记录总数
        string url = "";
        string strWhere = " 1=1 ";

        int CustomerID = Function.GetRequestInt("CustomerID");
        if (CustomerID > 0)
        {
            CustomersInfo cinfo = CustomersBLL.Get(CustomerID);
            if (null != cinfo)
            {
                hidCustomerID.Value = cinfo.ID.ToString();
                labCustomer.Text = cinfo.Name.Trim();
                BrandInfo binfo = BrandBLL.Get(Function.GetRequestInt("BrandID"));
                if (null != binfo)
                {
                    strWhere += " and  id in(select f_KnowledgeID from sys_KnowkedgeBaseBrand where f_BrandID=" + binfo.ID + ")";
                    url += "&BrandID=" + binfo.ID;
                    url += "&CustomerID=" + binfo.CustomerID;
                    hidBrandID.Value = binfo.ID.ToString();
                    labBrand.Text = binfo.Name.Trim();
                }
            }

        }
        string wd = Function.ClearText(Function.GetRequestSrtring("wd"));
        if (!string.IsNullOrEmpty(wd))
        {
            strWhere += string.Format(" and ( f_Labs like '%{0}%' or  f_Title like  '%{0}%')", wd);
            url += "&wd=" + wd;
            TxbLabs.Text = wd;
        }

        strWhere += " order by id desc ";
        GridView1.DataSource = KnowledgeBaseBLL.GetList(PageSize, PageIndex, strWhere, out count);
        GridView1.DataBind();
        this.List_Page.Text = Function.Paging2(PageSize, count, page, PageIndex, url);

    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.DataItem == null) return;
        int ID = Function.ConverToInt(DataBinder.Eval(e.Row.DataItem, "ID").ToString());
        Image Control1 = (Image)e.Row.FindControl("ImgHaveAttachment");
        Control1.Visible = AttachmentBLL.GetList(ID, AttachmentInfo.EUserFor.KnowledgeBase).Count > 0;

    }

    

    protected void BtnSch_Click(object sender, EventArgs e)
    {
        string Url = "ListOnSolutionPage.aspx";
        Url += "?CustomerID=" + hidCustomerID.Value;
        Url += "&BrandID=" + hidBrandID.Value;
        Url += "&wd=" + TxbLabs.Text.Trim();
        Response.Redirect(Url);
    }

    
}
