using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSMP.BLL;
using CSMP.Model;
using Tool;

public partial class page_Class2_Edit : _BaseData_Class
{
    // private static readonly ListItem DdlItemDefault = new ListItem("请选择", "0");

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DdlCustomer.DataSource = CustomersBLL.GetList();
            DdlCustomer.DataBind();

            Class2Info info = GetInfo();
            DdlClass1.DataSource = Class1BLL.GetList();
            DdlClass1.DataBind();
            if (null == info)
            {
                DdlCustomer.Items.Insert(0, new ListItem("请选择", "0"));
            }
            else
            {
                if (IsProject(info.Name)) return;
                Class1Info c1info = Class1BLL.Get(info.Class1ID);
                if (null == c1info)
                {
                    Function.AlertBack("数据有误"); return;
                }
                

                LabAction.Text = "编辑";
                TxbName.Text = info.Name;
                DdlCustomer.SelectedValue = c1info.CustomerID.ToString();
                DdlCustomer_SelectedIndexChanged(sender, e);
                DdlClass1.SelectedValue = info.Class1ID.ToString();

                RblIsClose.SelectedIndex = info.IsClosed ? 1 : 0;
            }
        }
    }

    private Class2Info GetInfo()
    {
        Class2Info info;
        if (ViewState["info"] != null)
        {
            info = (Class2Info)ViewState["info"];
        }
        int ID = Function.GetRequestInt("ID");
        if (ID > 0)
        {
            info = Class2BLL.Get(ID);
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
        Class2Info info = GetInfo();
        if (null == info)
        {
            info = new Class2Info();
        }
        Class1Info cinfo = Class1BLL.Get(Function.ConverToInt(DdlClass1.SelectedValue));
        if (null == cinfo)
        {
            Function.AlertMsg("请选择所属大类"); return;
        }

        Class2Info ExistInfo = Class2BLL.Get(TxbName.Text.Trim(), cinfo.ID);
        if (ExistInfo != null && ExistInfo.ID != info.ID)
        {
            Function.AlertMsg("该名称已存在"); return;
        }
        if (IsProject(TxbName.Text.Trim()))
        {
            return;
        }


        info.Name = TxbName.Text.Trim();
        info.Class1ID = cinfo.ID;
        info.Class1Name = cinfo.Name;
        info.IsClosed = RblIsClose.SelectedValue == "0";
        if (info.Name.Length > 50)
        {
            Function.AlertMsg("名称过长"); return;
        }


        if (GetInfo() == null)
        {
            if (Class2BLL.Add(info) > 0)
            {
                Function.AlertRefresh("添加成功", "main");
            }
            else
            {
                Function.AlertMsg("添加失败");
            }
        }
        else
        {
            if (Class2BLL.Edit(info))
            {
                Function.AlertRefresh("修改成功", "main");
            }
            else
            {
                Function.AlertMsg("修改失败");
            }
        }

    }
    protected void DdlCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        int ID=Function.ConverToInt(DdlCustomer.SelectedValue,0);
        if (ID<=0)
        {
            DdlClass1.DataSource = null;//TODO::把它改正确过来
        }
        else
        {
            DdlClass1.DataSource = Class1BLL.GetList(ID);
        }
        DdlClass1.DataBind();
    }
}
