using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSMP.BLL;
using CSMP.Model;
using Tool;

public partial class page_CustomerRequest_Add : _CustomerRequest_Add
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DdlProvince.DataSource = ProvincesBLL.GetList();
            DdlProvince.DataBind();
            DdlProvince.Items.Insert(0, new ListItem("请选择", "0"));
        }
    }



    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        StoreInfo info = null;
        if (DdlStore.SelectedValue != "0" && !string.IsNullOrEmpty(DdlStore.SelectedValue))
        {
            info = StoresBLL.Get(Function.ConverToInt(DdlStore.SelectedValue));
        }
        if (null == info)
        {
            info = StoresBLL.GetByStoreNo(TxtStoreNo.Text.Trim());
        }
        if (null == info)
        {
            Function.AlertRefresh("没有找到店铺信息");
            return;
        }
        CustomerRequestInfo crinfo = new CustomerRequestInfo();
        crinfo.AddDate = DateTime.Now;
        crinfo.CallID = 0;
        crinfo.Details = TxbDetail.Text;
        crinfo.Enable = true;
        crinfo.ErrorReportDate = crinfo.AddDate;
        crinfo.ErrorReportUserID = 0;
        crinfo.ErrorReportUserName = (TxbErrorReportUser.Text == "当前登录用户") ? CurrentUserName : TxbErrorReportUser.Text;
        crinfo.BrandID = info.BrandID;
        crinfo.StoreID = info.ID;
        crinfo.StoreName = info.Name;
        crinfo.StoreNo = info.No;
        crinfo.UserID = CurrentUser.ID;
        crinfo.UserName = CurrentUser.Name;

        if (string.IsNullOrEmpty(crinfo.Details))
        {
            Function.AlertMsg("请填写点报修内容");
            return;
        }
        if (string.IsNullOrEmpty(crinfo.ErrorReportUserName) || crinfo.ErrorReportUserName.Length > 40)
        {
            Function.AlertMsg("请填写报修人，并且报修人不要超过40个字");
            return;
        }

        int Result = CustomerRequestBLL.Add(crinfo);
        if (Result > 0)
        {
            Function.AlertRefresh("添加成功", "main");
            return;
        }
        else
        {
            Function.AlertMsg("添加失败，请重试");
            return;
        }


    }

    protected void BtnStore_Click(object sender, EventArgs e)
    {
        StoreInfo info = StoresBLL.GetByStoreNo(TxtStoreNo.Text.Trim());
        if (null == info)
        {
            Function.AlertRefresh("没有找到店铺信息");
            return;
        }
        LabStoreMsg.Text = string.Format("店铺名：{0}；店铺编号：{1}；", info.Name, info.No);
        TxbTel.Text = info.Tel;
    }
    protected void DdlProvince_SelectedIndexChanged(object sender, EventArgs e)
    {
        int ProvinceID = Function.ConverToInt(DdlProvince.SelectedValue);
        if (ProvinceID > 0)
        {
            DdlCity.DataSource = CityBLL.GetList(ProvinceID);
            DdlCity.DataBind();
            DdlCity.Items.Insert(0, new ListItem("请选择", "0"));
        }
        else
        {
            DdlCity.DataSource = new List<string>();
            DdlCity.DataBind();
        }
        DdlStore.DataSource =new List<string>();
        DdlStore.DataBind();
    }
    protected void DdlCity_SelectedIndexChanged(object sender, EventArgs e)
    {
        int CityID = Function.ConverToInt(DdlCity.SelectedValue);
        if (CityID <= 0)
        {
            
            DdlStore.DataSource = new List<string>();
            DdlStore.DataBind();
        }
        List<StoreInfo> list = StoresBLL.GetListByCityWorkGroup(CityID, CurrentUser.WorkGroupID);
        if (null == list || list.Count == 0)
        {
            DdlStore.DataSource = new List<string>();
            DdlStore.DataBind();
            DdlStore.Items.Insert(0, new ListItem("该城市下没有店铺", "0"));
        }
        else
        {
            DdlStore.DataSource = list;
            DdlStore.DataBind();
        }
    }
}
