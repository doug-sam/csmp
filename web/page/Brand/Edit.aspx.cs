using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSMP.BLL;
using CSMP.Model;
using Tool;

public partial class page_Brand_Edit : _BaseData_CustomerBrand
{
    private static readonly ListItem DdlItemDefault = new ListItem("请选择", "0");

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DdlCustomer.DataSource = CustomersBLL.GetList();
            DdlCustomer.DataBind();
            DdlUser.DataSource = UserBLL.GetList(" 1=1 ");
            DdlUser.DataBind();

            DdlSlaMode.DataSource = SlaModeBLL.GetList();
            DdlSlaMode.DataBind();
            DdlSlaMode.Items.Insert(0, new ListItem("请选择", "0"));

            BrandInfo info = GetInfo();
            if (null == info)
            {
                DdlCustomer.Items.Insert(0, DdlItemDefault);
                DdlUser.Items.Insert(0, DdlItemDefault);
                
            }
            else
            {
                LabAction.Text = "编辑";

                CustomersInfo cinfo = CustomersBLL.Get(info.CustomerID);
                if (cinfo == null)
                {
                    return;
                }
                TxbName.Text = info.Name;
                DdlCustomer.SelectedValue = cinfo.ID.ToString();
                DdlUser.SelectedValue = info.UserID.ToString();
                TxbContact.Text = info.Contact;
                TxbPhone.Text = info.Phone;
                TxbEmail.Text = info.Email;
                RblIsClose.SelectedIndex = info.IsClose ? 1 : 0;
                DdlSlaMode.SelectedValue = info.SlaModeID.ToString();
                TxbSlaTimer1.Text = info.SlaTimer1.ToString();
                TxbSlaTimer2.Text = info.SlaTimer2.ToString();
                TxbSlaTimerTo.Text = info.SlaTimerTo;
            }
        }
    }

    private BrandInfo GetInfo()
    {
        BrandInfo info;
        if (ViewState["info"] != null)
        {
            info = (BrandInfo)ViewState["info"];
        }
        int ID = Function.GetRequestInt("ID");
        if (ID > 0)
        {
            info = BrandBLL.Get(ID);
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
        CustomersInfo cinfo = CustomersBLL.Get(Function.ConverToInt(DdlCustomer.SelectedValue));
        if (null == cinfo)
        {
            Function.AlertMsg("请选择所属客户"); return;
        }
        BrandInfo info = GetInfo();
        if (null == info)
        {
            info = new BrandInfo();
            BrandInfo infotemp = BrandBLL.Get(TxbName.Text.Trim(), cinfo.ID);
            if (null!=infotemp)
            {
                Function.AlertMsg("该名称已存在"); return;
            }
           
        }
        //UserInfo uinfo = UserBLL.Get(Function.ConverToInt(DdlUser.SelectedValue));
        //if (null==uinfo)
        //{
        //    Function.AlertMsg("请选择品牌经理"); return;
        //}
        info.SlaModeID = Function.ConverToInt(DdlSlaMode.SelectedValue, 0);
        info.Name = TxbName.Text.Trim();
        info.CustomerID = cinfo.ID;
        info.CustomerName = cinfo.Name;
        info.UserID =CurrentUserID;
        info.Contact = TxbContact.Text.Trim();
        info.Phone = TxbPhone.Text.Trim();
        info.Email = TxbEmail.Text.Trim();
        info.IsClose = RblIsClose.SelectedValue == "0";
        info.SlaTimer1 = Function.ConverToInt(TxbSlaTimer1.Text, 0);
        info.SlaTimer2 = Function.ConverToInt(TxbSlaTimer2.Text, 0);
        info.SlaTimerTo = TxbSlaTimerTo.Text.Trim();
        if (info.Name.Length > 100)
        {
            Function.AlertMsg("名称过长"); return;
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
        if (info.SlaModeID<=0)
        {
            Function.AlertMsg("请选择sla模型"); return;
        }

        if (GetInfo() == null)
        {
            if (BrandBLL.Add(info) > 0)
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
            if (BrandBLL.Edit(info))
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
