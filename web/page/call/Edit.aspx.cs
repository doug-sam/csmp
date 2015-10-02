using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSMP.BLL;
using CSMP.Model;
using Tool;

public partial class page_call_Edit : _Call_Edit
{
    public CallInfo info;
    protected void Page_Load(object sender, EventArgs e)
    {
        GetInfo();
        if (!IsPostBack)
        {
            if (!CallBLL.EnableEdit(info,CurrentUser))
            {
                GroupBLL.NoPowerRedirect();
                return;
            }
            DdlCategory.DataSource = CallCategoryBLL.GetListEnable();
            DdlCategory.DataBind();


            ddlClass1.DataSource = Class1BLL.GetList(info.CustomerID);
            ddlClass1.DataBind();

            ddlClass1.SelectedValue = info.Class1.ToString();
            ddlClass1_SelectedIndexChanged(sender, e);
            ddlClass2.SelectedValue = info.Class2.ToString();
            ddlClass2_SelectedIndexChanged(sender, e);
            ddlClass3.SelectedValue = info.Class3.ToString();
            ddlClass3_SelectedIndexChanged(sender, e);
            TxtDetails.Text = info.Details;

            StoreInfo sinfo = StoresBLL.Get(info.StoreID);
            if (null == sinfo)
            {
                Response.End();
            }
            LtlStoreName.Text = sinfo.Name;
            LtlTel.Text = sinfo.Tel;
            LtlAddress.Text = sinfo.Address;
            LtlStoreType.Text = sinfo.StoreType;
            TxbSLA.Text = info.SLA.ToString();
            TxbSlaExt.Text = info.SLA2;

            TxbCallNo3.Text = info.CallNo3;
            CbIsSameCall.Checked = info.IsSameCall;

            DdlCategory.SelectedValue = info.Category.ToString();
        }
    }

    private void GetInfo()
    {
        int ID = Function.GetRequestInt("ID");
        if (ID <= 0)
        {
            Response.End();
        }
        info = CallBLL.Get(ID);
        if (null == info)
        {
            Response.End();
        }
    }

    protected void ddlClass1_SelectedIndexChanged(object sender, EventArgs e)
    {
        int ID = Function.ConverToInt(ddlClass1.SelectedValue);
        if (ID > 0)
        {

            ddlClass2.DataSource = Class2BLL.GetList(ID);
            ddlClass2.DataBind();
            ddlClass2.Items.Insert(0, new ListItem("请选择", "0"));
            ddlClass2.Enabled = true;
        }
        else
        {
            ddlClass2.DataSource = null;
            ddlClass2.DataBind();
        }
    }

    protected void ddlClass2_SelectedIndexChanged(object sender, EventArgs e)
    {
        int ID = Function.ConverToInt(ddlClass2.SelectedValue);
        if (ID > 0)
        {
            ddlClass3.DataSource = Class3BLL.GetList(ID);
            ddlClass3.DataBind();
            ddlClass3.Items.Insert(0, new ListItem("请选择", "0"));
            ddlClass3.Enabled = true;
        }
        else
        {
            ddlClass3.DataSource = null;
            ddlClass3.DataBind();
        }
    }

    protected void ddlClass3_SelectedIndexChanged(object sender, EventArgs e)
    {
        int ID = Function.ConverToInt(ddlClass3.SelectedValue);
        if (ID > 0)
        {
            Class3Info info = Class3BLL.Get(ID);
            if (null != info)
            {
                PrioritiesInfo pinfo = PrioritiesBLL.Get(info.PriorityID);
                if (pinfo != null)
                {
                    LtlPriority.Text = pinfo.Name;
                }
            }
        }
    }
    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        GetInfo();
        Class1Info c1 = Class1BLL.Get(Function.ConverToInt(ddlClass1.SelectedValue));
        Class2Info c2 = Class2BLL.Get(Function.ConverToInt(ddlClass2.SelectedValue));
        Class3Info c3 = Class3BLL.Get(Function.ConverToInt(ddlClass3.SelectedValue));

        if (null == c1)
        {
            Function.AlertBack("请选择大类故障");
        }
        if (c1.CustomerID != info.CustomerID)
        {
            Function.AlertBack("不要作弊！");
        }
        if (c2 == null || c2.Class1ID != c1.ID)
        {
            Function.AlertBack("请选择中类！");
        }
        if (c3 == null || c3.Class2ID != c2.ID)
        {
            Function.AlertBack("请选择小类！");
        }
        if (TxbCallNo3.Text.Trim().Length > 50)
        {
            Function.AlertBack("外部单号过长");
        }
        if (Function.ConverToInt(TxbSLA.Text.Trim())<0)
        {
            Function.AlertBack("sla需要正整数");
        }
        if (TxbSlaExt.Text.Trim().Length>50)
        {
            Function.AlertBack("扩展SLA过长");
        }

        info.Class1 = c1.ID;
        info.Class2 = c2.ID;
        info.Class3 = c3.ID;
        info.ClassName1 = c1.Name;
        info.ClassName2 = c2.Name;
        info.ClassName3 = c3.Name;
        info.PriorityID = c3.PriorityID;
        info.PriorityName = LtlPriority.Text;
        info.SLA = Function.ConverToInt(TxbSLA.Text.Trim(),0);
        info.Details = TxtDetails.Text.Trim();
        info.CallNo3 = TxbCallNo3.Text.Trim();
        info.SLA2 = TxbSlaExt.Text.Trim();
        info.IsSameCall = CbIsSameCall.Checked;
        info.Category = Function.ConverToInt(DdlCategory.SelectedValue);
        if (CallBLL.EditWithLog(info,CurrentUserName+"修改了Call"))
        {
            //Function.AlertRedirect("修改成功", "list.aspx?state=" + info.StateMain, "main");
            Function.AlertRefresh("修改成功","main");
        }
        else
        {
            Function.AlertBack("修改失败");
        }
    }
}
