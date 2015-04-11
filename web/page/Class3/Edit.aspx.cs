using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSMP.BLL;
using CSMP.Model;
using Tool;

public partial class page_Class3_Edit : _BaseData_Class
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DdlCustomer.DataSource = CustomersBLL.GetList();
            DdlCustomer.DataBind();

            DdlPriority.DataSource=PrioritiesBLL.GetList();
            DdlPriority.DataBind();
            
            Class3Info info = GetInfo();
            if (null == info)
            {
                DdlCustomer.Items.Insert(0, new ListItem("请选择", "0"));
            }
            else
            {

                if (IsProject(info.Name)) return;
                Class2Info c2 = Class2BLL.Get(info.Class2ID);
                if (c2 == null) return;
                Class1Info c1 = Class1BLL.Get(c2.Class1ID);
                if (c1 == null) return;

                LabAction.Text = "编辑";
                TxbName.Text = info.Name;

                DdlCustomer.SelectedValue = c1.CustomerID.ToString();
                DdlCustomer_SelectedIndexChanged(sender, e);
                DdlClass1.SelectedValue = c1.ID.ToString();
                DdlClass1_SelectedIndexChanged(sender, e);
                DdlClass2.SelectedValue = c2.ID.ToString();
                DdlPriority.SelectedValue = info.PriorityID.ToString();
                TxbSla.Text = info.SLA.ToString();
                RblIsClose.SelectedIndex = info.IsClosed ? 1 : 0;
            }
        }
    }

    private Class3Info GetInfo()
    {
        Class3Info info;
        if (ViewState["info"] != null)
        {
            info = (Class3Info)ViewState["info"];
        }
        int ID = Function.GetRequestInt("ID");
        if (ID > 0)
        {
            info = Class3BLL.Get(ID);
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
        Class3Info info = GetInfo();
        if (null == info)
        {
            info = new Class3Info();
        }
        Class2Info c2 = Class2BLL.Get(Function.ConverToInt(DdlClass2.SelectedValue));
        if (null == c2)
        {
            Function.AlertMsg("请选择所属中类"); return;
        }
        Class3Info ExistInfo = Class3BLL.Get(TxbName.Text.Trim(), c2.ID);
        if (ExistInfo != null && ExistInfo.ID != info.ID)
        {
            Function.AlertMsg("该名称已存在"); return;
        }

        PrioritiesInfo pinfo = PrioritiesBLL.Get(Function.ConverToInt(DdlPriority.SelectedValue));
        if (null==pinfo)
        {
            Function.AlertMsg("请选择优先级"); return;
        }
        if (Function.ConverToInt(TxbSla.Text.Trim())<=0||TxbSla.Text.Trim().Length>6)
        {
            Function.AlertMsg("sla应为正整数"); return;
        }
        info.Name = TxbName.Text.Trim();
        info.Class2ID = c2.ID;
        info.Class2Name = c2.Name;
        info.PriorityID = pinfo.ID;
        info.PriorityName = pinfo.Name;
        info.SLA = Function.ConverToInt(TxbSla.Text.Trim());
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
            if (Class3BLL.Add(info) > 0)
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
            if (Class3BLL.Edit(info))
            {
                Function.AlertRefresh("修改成功","main");
            }
            else
            {
                Function.AlertMsg("修改失败");
            }
        }

    }
    protected void DdlClass1_SelectedIndexChanged(object sender, EventArgs e)
    {
        int ID = Function.ConverToInt(DdlClass1.SelectedValue);
        if (ID>0)
        {
            DdlClass2.DataSource = Class2BLL.GetList(ID);
            DdlClass2.DataBind();
            DdlClass2.Items.Insert(0, new ListItem("0", "请选择"));
        }
    }

    protected void DdlCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        int ID = Function.ConverToInt(DdlCustomer.SelectedValue, 0);
        if (ID <= 0)
        {
            DdlClass1.DataSource = null;//TODO::把它改正确过来
        }
        else
        {
            DdlClass1.DataSource = Class1BLL.GetList(ID);
        }
        DdlClass1.DataBind();
        DdlClass1.Items.Insert(0, new ListItem("0", "请选择"));
    }

}
