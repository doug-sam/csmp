using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


using CSMP.Model;
using CSMP.BLL;
using Tool;

public partial class page_KnowledgeBase_list : _KnowledgeBase_Library
{
    

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DdlCustomer.DataSource = CustomersBLL.GetList();
            DdlCustomer.DataBind();
            DdlCustomer.Items.Insert(0, new ListItem("不限", "0"));

            Sch();

            LtlAdd.Visible = GroupBLL.PowerCheck((int)PowerInfo.P6_KnowledgeBase.知识库添加);
            LtlDelete.Visible = GroupBLL.PowerCheck((int)PowerInfo.P6_KnowledgeBase.知识库编辑删除);
            List<string> HideHead = new List<string>();
            if (!LtlDelete.Visible)
            {
                HideHead.Add("选择");
                HideHead.Add("详细资料");
            }
            GridViewHide(HideHead);
        }
    }

    private void GridViewHide(List<string> header)
    {
        foreach (DataControlField column in GridView1.Columns)
        {
            if (header.Contains(column.HeaderText.Trim()))
            { column.Visible = false; }
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
                DdlCustomer.SelectedValue = cinfo.ID.ToString();
                DdlCustomer_SelectedIndexChanged(null, null);
                BrandInfo binfo = BrandBLL.Get(Function.GetRequestInt("BrandID"));
                if (null != binfo)
                {
                    strWhere += " and  id in(select f_KnowledgeID from sys_KnowkedgeBaseBrand where f_BrandID=" + binfo.ID + ")";
                    url += "&BrandID=" + binfo.ID;
                    url += "&CustomerID="+binfo.CustomerID;
                    DdlBrand.SelectedValue = binfo.ID.ToString();
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
       Control1.Visible= AttachmentBLL.GetList(ID, AttachmentInfo.EUserFor.KnowledgeBase).Count > 0;

    }

    protected void BtnDelete_Click(object sender, EventArgs e)
    {
        string delList = Function.GetRequestSrtring("ckDel");
        if (string.IsNullOrEmpty(delList))
        {
            Function.AlertBack("没有选中数据");
            return;
        }
        int Flag = 0;
        foreach (string item in delList.Split(','))
        {
            int ID = Function.ConverToInt(item);
            if (ID > 0)
            {
                if (KnowledgeBaseBLL.Delete(ID))
                {
                    Flag++;
                }
            }
        }
        Function.AlertRefresh(Flag + "条数据删除成功，\n如果数据未能删除，是由于有店铺属于该城市，\n请先删除在该城市下的店铺");
    }


    protected void BtnSch_Click(object sender, EventArgs e)
    {
        string Url = "list.aspx";
        Url += "?CustomerID="+DdlCustomer.SelectedValue;
        Url += "&BrandID=" + DdlBrand.SelectedValue;
        Url += "&wd=" + TxbLabs.Text.Trim();
        Response.Redirect(Url);
    }

    protected void DdlCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        int ID = Function.ConverToInt(DdlCustomer.SelectedValue);
        if (ID > 0)
        {
            DdlBrand.DataSource = BrandBLL.GetList(ID);
        }
        else
        {
            DdlBrand.DataSource = null;
        }
        DdlBrand.DataBind();
        DdlBrand.Items.Insert(0, new ListItem("不限", "0"));
    }



}
