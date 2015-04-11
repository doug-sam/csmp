using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSMP.BLL;
using CSMP.Model;
using Tool;

public partial class page_WorkGroupBrand_Add2 : _User_WorkWorkGroupBrand
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            
            DdlCustomer.Items.Insert(0, new ListItem("请选择", "0"));
            DdlProvince.DataSource = ProvincesBLL.GetList();
            DdlProvince.DataBind();
            DdlProvince.Items.Insert(0, new ListItem("请选择", "0"));
        }
    }
    protected void BtnAdd_Click(object sender, EventArgs e)
    {
        for (int i = LbBrandLeft.Items.Count - 1; i >= 0; i--)
        {
            if (LbBrandLeft.Items[i].Selected)
            {
                LbBrandRight.Items.Insert(0, LbBrandLeft.Items[i]);
                LbBrandLeft.Items.Remove(LbBrandLeft.Items[i]);
            }
        }
    }
    protected void BtnDel_Click(object sender, EventArgs e)
    {
        for (int i = LbBrandRight.Items.Count - 1; i >= 0; i--)
        {
            if (LbBrandRight.Items[i].Selected)
            {
                LbBrandLeft.Items.Insert(0, LbBrandRight.Items[i]);
                LbBrandRight.Items.Remove(LbBrandRight.Items[i]);
            }
        }
    }
    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        int WorkGroupID = Function.ConverToInt(DdlWorkGroup.SelectedValue);
        if (WorkGroupID <= 0) return;
        int CustomerID=Function.ConverToInt(DdlCustomer.SelectedValue);
        if (CustomerID<=0)return;

        WorkGroupInfo winfo = WorkGroupBLL.Get(WorkGroupID);
        if (null == winfo) return;

        WorkGroupBrandInfo info = new WorkGroupBrandInfo();
        WorkGroupBrandBLL.Delete(WorkGroupID, CustomerID);
        foreach (ListItem item in LbBrandRight.Items)
        {
            info = new WorkGroupBrandInfo();
            info.MID = Function.ConverToInt(item.Value);
            info.MName = item.Text;
            info.WorkGroupID = winfo.ID;
            info.WorkGroupName = winfo.Name;
            WorkGroupBrandBLL.Add(info);
        }
        Function.AlertMsg("提交成功");
    }
    protected void DdlCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        LbBrandRight.DataSource = null;
        LbBrandRight.DataBind();


        int CustomerID = Function.ConverToInt(DdlCustomer.SelectedValue);
        LbBrandLeft.DataSource = null;//TODO::把它改正确过来

        List<BrandInfo> list = BrandBLL.GetList(CustomerID);
        LbBrandLeft.DataSource = list;
        LbBrandLeft.DataBind();
        InitLeftRight(list);
    }
    private void InitLeftRight(List<BrandInfo> list)
    {
        LbBrandRight.DataSource = null;
        LbBrandRight.DataBind();
        for (int i = LbBrandRight.Items.Count-1; i >=0; i--)
        {
            LbBrandRight.Items.RemoveAt(i);
        }

        int WorkGroupID = Function.ConverToInt(DdlWorkGroup.SelectedValue);
        if (WorkGroupID <= 0)
        {
            Function.AlertBack("请先选择工作组");
            return;
        }
        for (int i = list.Count; i > 0; i--)
        {
            if (WorkGroupBrandBLL.HasRelaction(WorkGroupID, list[i - 1].ID))
            {
                LbBrandRight.Items.Insert(0, new ListItem(LbBrandLeft.Items[i - 1].Text, LbBrandLeft.Items[i - 1].Value));
                LbBrandLeft.Items.RemoveAt(i - 1);
            }
        }
    }

    protected void DdlProvince_SelectedIndexChanged(object sender, EventArgs e)
    {
        int ID = Function.ConverToInt(DdlProvince.SelectedValue);
        if (ID <= 0)
        {
            DdlWorkGroup.DataSource = null;//TODO::把它改正确过来
        }
        else
        {
            DdlWorkGroup.DataSource = WorkGroupBLL.GetList(ID);
        }
        DdlWorkGroup.DataBind();
        DdlWorkGroup.Items.Insert(0, new ListItem("请选择", "0"));
    }
    protected void DdlWorkGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        DdlCustomer.DataSource = CustomersBLL.GetList();
        DdlCustomer.DataBind();
        DdlCustomer.Items.Insert(0, new ListItem("请选择", "0"));
        DdlCustomer_SelectedIndexChanged(sender, e);
    }
}
