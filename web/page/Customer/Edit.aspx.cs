using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSMP.BLL;
using CSMP.Model;
using Tool;

public partial class page_Customer_Edit : _BaseData_CustomerBrand
{
    private static readonly ListItem DdlItemDefault = new ListItem("请选择", "0");

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ddlProvince.DataSource = ProvincesBLL.GetList();
            ddlProvince.DataBind();

            CustomersInfo info = GetInfo();
            if (null == info)
            {
                ddlProvince.Items.Insert(0, DdlItemDefault);
            }
            else
            {
                LabAction.Text = "编辑";

                CityInfo cinfo = CityBLL.Get(info.CityID);
                if (cinfo == null)
                {
                    return;
                }
                TxbName.Text = info.Name;
                TxbContact.Text = info.Contact;
                TxbPhone.Text = info.Phone;
                TxbEmail.Text = info.Email;
                ddlProvince.SelectedValue = cinfo.ProvinceID.ToString();
                ddlProvince_SelectedIndexChanged(sender, e);
                ddlCity.SelectedValue = info.CityID.ToString();
                RblIsClose.SelectedIndex = info.IsClosed ? 1 : 0;
                HlInitClass.Visible = true;
                HlInitClass.NavigateUrl = "/page/system/InitClass.aspx?CustomerID=" + info.ID;
            }
        }
    }

    private CustomersInfo GetInfo()
    {
        CustomersInfo info;
        if (ViewState["info"] != null)
        {
            info = (CustomersInfo)ViewState["info"];
        }
        int ID = Function.GetRequestInt("ID");
        if (ID > 0)
        {
            info = CustomersBLL.Get(ID);
            if (null != info)
            {
                ViewState["info"] = info;
            }
            return info;
        }
        return null;
    }

    protected void ddlProvince_SelectedIndexChanged(object sender, EventArgs e)
    {
        int ID = Function.ConverToInt(ddlProvince.SelectedValue);
        if (ID > 0)
        {
            ddlCity.DataSource = CityBLL.GetList(ID);
            ddlCity.DataBind();
            ddlCity.Items.Insert(0, DdlItemDefault);
        }
    }
    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        TxbName.Text = TxbName.Text.Trim();
        CustomersInfo info = GetInfo();
        if (null == info)
        {
            info = new CustomersInfo();
            if (CustomersBLL.NameExit(TxbName.Text.Trim()))
            {
                Function.AlertMsg("该名称已存在"); return;
            }
        }

        info.CityID = Function.ConverToInt(ddlCity.SelectedValue);
        info.Contact = TxbContact.Text.Trim();
        info.Email = TxbEmail.Text.Trim();
        info.IsClosed = RblIsClose.SelectedValue == "0";
        info.Name = TxbName.Text.Trim();
        info.Phone = TxbPhone.Text.Trim();
        if (info.CityID <= 0)
        {
            Function.AlertMsg("请选择城市"); return;
        }
        if (info.Contact.Length > 50)
        {
            Function.AlertMsg("联系人不能长于50字"); return;
        }
        if (info.Phone.Length > 50)
        {
            Function.AlertMsg("电话不能长于50字"); return;
        }
        if (info.Email.Length > 50)
        {
            Function.AlertMsg("邮箱不能长于50字"); return;
        }
        if (info.Name.Length > 50)
        {
            Function.AlertMsg("客户名不能长于50字"); return;
        }
        if (GetInfo() == null)
        {
            if (CustomersBLL.Add(info) > 0)
            {
                Function.AlertRefresh("添加成功","main");
            }
            else
            {
                Function.AlertMsg("添加失败");
            }
        }
        else
        {
            if (CustomersBLL.Edit(info))
            {
                Function.AlertRefresh("修改成功", "main");
            }
            else
            {
                Function.AlertMsg("修改失败");
            }
        }

    }
}
