using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSMP.BLL;
using CSMP.Model;
using Tool;

public partial class page_WorkGroupBrand_add : _User_WorkWorkGroupBrand
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DdlCustomer.DataSource = CustomersBLL.GetList();
            DdlCustomer.DataBind();
            DdlCustomer.Items.Insert(0, new ListItem("请选择", "0"));
            //ddlBrand.DataSource = BrandBLL.GetList();
            //ddlBrand.DataBind();
            //ddlBrand.Items.Insert(0, new ListItem("请选择", "0"));
        }
    }
    protected void BtnAdd_Click(object sender, EventArgs e)
    {
        for (int i = LbUserLeft.Items.Count - 1; i >= 0; i--)
        {
            if (LbUserLeft.Items[i].Selected)
            {
                LbUserRight.Items.Insert(0, LbUserLeft.Items[i]);
                LbUserLeft.Items.Remove(LbUserLeft.Items[i]);
            }
        }
    }
    protected void BtnDel_Click(object sender, EventArgs e)
    {
        for (int i = LbUserRight.Items.Count - 1; i >= 0; i--)
        {
            if (LbUserRight.Items[i].Selected)
            {
                LbUserLeft.Items.Insert(0, LbUserRight.Items[i]);
                LbUserRight.Items.Remove(LbUserRight.Items[i]);
            }
        }
    }
    protected void ddlBrand_SelectedIndexChanged(object sender, EventArgs e)
    {
        int ID = Function.ConverToInt(ddlBrand.SelectedValue);
        if (ID > 0)
        {
            LbUserLeft.DataSource = WorkGroupBLL.GetList(ID, false);
            LbUserRight.DataSource = WorkGroupBLL.GetList(ID, true);

        }
        else
        {
            LbUserLeft.DataSource = null;
            LbUserRight.DataSource = null;
        }
        LbUserLeft.DataBind();
        LbUserRight.DataBind();
    }
    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        int BrandID = Function.ConverToInt(ddlBrand.SelectedValue);
        if (BrandID<=0)return;
        BrandInfo cinfo = BrandBLL.Get(BrandID);
        if(null==cinfo)return;

        WorkGroupBrandInfo info = new WorkGroupBrandInfo();
        WorkGroupInfo winfo;
        WorkGroupBrandBLL.DeleteByMID(cinfo.ID);
        foreach (ListItem item in LbUserRight.Items)
        {
            winfo = WorkGroupBLL.Get(Function.ConverToInt(item.Value));
            if (null == winfo)
            {
                continue;
            }
            info = new WorkGroupBrandInfo();
            info.MID = cinfo.ID;
            info.MName = cinfo.Name;
            info.WorkGroupID = winfo.ID;
            info.WorkGroupName = winfo.Name;
            WorkGroupBrandBLL.Add(info);
        }
        Function.AlertRefresh("提交成功", "main");
    }
    protected void DdlCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        int CustomerID = Function.ConverToInt(DdlCustomer.SelectedValue);
        if (CustomerID <= 0)
        {
            ddlBrand.DataSource = null;
            ddlBrand.DataBind();
        }
        else
        {
            ddlBrand.DataSource = BrandBLL.GetList(CustomerID);
            ddlBrand.DataBind();
            ddlBrand.Items.Insert(0, new ListItem("请选择品牌","0"));
        }
    }
}
