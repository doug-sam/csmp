using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSMP.BLL;
using CSMP.Model;
using Tool;

public partial class page_Class1_Edit : _BaseData_Class
{
    private static readonly ListItem DdlItemDefault = new ListItem("请选择", "0");

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DdlCustomer.DataSource = CustomersBLL.GetList();
            DdlCustomer.DataBind();


            Class1Info info = GetInfo();
            if (null == info)
            {
                DdlCustomer.Items.Insert(0, DdlItemDefault);
            }
            else
            {
                if (IsProject(info.Name)) return;

                LabAction.Text = "编辑";
                CustomersInfo cinfo = CustomersBLL.Get(info.CustomerID);
                if (cinfo == null)
                {
                    return;
                }
                TxbName.Text = info.Name;
                DdlCustomer.SelectedValue = cinfo.ID.ToString();
                RblIsClose.SelectedIndex = info.IsClosed ? 1 : 0;
            }
        }
    }

    private Class1Info GetInfo()
    {
        Class1Info info;
        if (ViewState["info"] != null)
        {
            info = (Class1Info)ViewState["info"];
        }
        int ID = Function.GetRequestInt("ID");
        if (ID > 0)
        {
            info = Class1BLL.Get(ID);
            if (null != info)
            {
                ViewState["info"] = info;
            }
            return info;
        }
        return null;
    }


    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        Class1Info info = GetInfo();
        if (null == info)
        {
            info = new Class1Info();
        }
        CustomersInfo cinfo = CustomersBLL.Get(Function.ConverToInt(DdlCustomer.SelectedValue));
        if (null == cinfo)
        {
            Function.AlertMsg("请选择所属品牌"); return;
        }
        Class1Info ExistInfo=Class1BLL.Get(TxbName.Text.Trim(), cinfo.ID);
        if (ExistInfo != null && ExistInfo.ID != info.ID)
        {
            Function.AlertMsg("该名称已存在"); return;
        }

        info.Name = TxbName.Text.Trim();
        info.CustomerID = cinfo.ID;
        info.IsClosed = RblIsClose.SelectedValue == "0";
        if (info.Name.Length > 50)
        {
            Function.AlertMsg("名称过长"); return;
        }
        if (IsProject(TxbName.Text.Trim()))
        {
            return;
        }

        if (GetInfo() == null)
        {
            if (Class1BLL.Add(info) > 0)
            {
                Function.AlertRefresh("添加成功", "main");
            }
            else
            {
                Function.AlertMsg("添加失败"); return;
            }
        }
        else
        {
            if (Class1BLL.Edit(info))
            {
                Function.AlertRefresh("修改成功", "main");
            }
            else
            {
                Function.AlertMsg("修改失败"); return;
            }
        }

    }
}
